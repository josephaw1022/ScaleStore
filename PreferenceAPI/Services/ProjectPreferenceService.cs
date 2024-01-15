using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using PreferenceDb.Preference;

namespace PreferenceAPI.Services;

public class ProjectPreferenceService
{
    private readonly PreferenceDbContext _dbContext;
    private readonly ILogger<ProjectPreferenceService> _logger;
    private readonly ServiceScalingHttpApiService _serviceScalingHttpClient;
    private readonly IDistributedCache _distributedCache;

    public ProjectPreferenceService(PreferenceDbContext dbContext, ILogger<ProjectPreferenceService> logger, ServiceScalingHttpApiService serviceScalingHttpClient, IDistributedCache cache)
    {
        _dbContext = dbContext;
        _logger = logger;
        _serviceScalingHttpClient = serviceScalingHttpClient;
        _distributedCache = cache;
    }

    public async Task<ProjectPreference> GetProjectPreferenceAsync(int userId)
    {
        // Try to find an existing project preference for the given user ID
        var projectPreference = await _dbContext.ProjectPreferences
                                                .FirstOrDefaultAsync(p => p.UserId == userId);

        // If the project preference exists and is valid, return it
        if (projectPreference is not null && projectPreference.ProjectId != 0)
        {
            return projectPreference;
        }

        // If the project preference does not exist, get the list of projects from the Projects API
        var existingProjectsFromScalingApi = await _serviceScalingHttpClient.GetListOfProjects(userId);

        // Choose the first project as the default (alphabetical order)
        var projectToInsert = new ProjectPreference
        {
            UserId = userId
        };

        if (existingProjectsFromScalingApi.Any())
        {
            var firstProject = existingProjectsFromScalingApi.OrderBy(p => p.Name).First();
            projectToInsert.ProjectId = firstProject.Id;
        }
        else
        {
            // Set default ProjectId if no projects are returned from the API
            projectToInsert.ProjectId = 0; // Or any default value
        }

        // Insert the default project preference into the database
        _dbContext.ProjectPreferences.Add(projectToInsert);
        await _dbContext.SaveChangesAsync();

        return projectToInsert;
    }
    public async Task<bool> UpdateProjectPreferenceAsync(int projectId, int userId)
    {
        try
        {
            // Find an existing project preference for the given user ID
            var projectPreference = await _dbContext.ProjectPreferences
                                                    .FirstOrDefaultAsync(p => p.UserId == userId);

            // If an existing preference is found, update it
            if (projectPreference != null)
            {
                projectPreference.ProjectId = projectId;
            }
            else
            {
                // If no preference is found, create a new one
                projectPreference = new ProjectPreference
                {
                    ProjectId = projectId,
                    UserId = userId
                };
                _dbContext.ProjectPreferences.Add(projectPreference);
            }

            // Save the changes to the database
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // Log the exception (replace with your logging mechanism)
            Console.WriteLine(ex.Message); // Example logging
            return false;
        }
    }


}
