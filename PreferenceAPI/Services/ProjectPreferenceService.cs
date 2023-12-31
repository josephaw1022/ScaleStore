using MongoDB.Bson;
using MongoDB.Driver;
using PreferenceDTO;
using System.Text.Json.Serialization;

namespace PreferenceAPI.Services;

public class ProjectPreferenceService
{
    private readonly IMongoCollection<ProjectPreferenceCollection> _projectPreferencesCollection;
    private readonly ILogger<ProjectPreferenceService> _logger;
    private readonly ServiceScalingHttpApiService _serviceScalingHttpClient;


    public ProjectPreferenceService(IMongoClient mongoClient, ILogger<ProjectPreferenceService> logger, ServiceScalingHttpApiService serviceScalingHttpClient)
    {
        _projectPreferencesCollection = mongoClient.GetDatabase("preference")
                                                   .GetCollection<ProjectPreferenceCollection>("projectpreferences");
        _logger = logger;
        _serviceScalingHttpClient = serviceScalingHttpClient;
    }

    public async Task<IProjectPreference> GetProjectPreferenceAsync(int userId)
    {
        var projectPreferences = await _projectPreferencesCollection
             .Find(p => p.UserId == userId)
             .FirstOrDefaultAsync();

        // If the project preference exists, return it
        if ((projectPreferences is not null) && (projectPreferences is not { ProjectId: 0, UserId: 0 }))
        {
            return projectPreferences;
        }


        // If the project preference does not exist, get the list of projects from the Projects API and choose the first one as the default (alphabetical order)
        var projectToInsert = new ProjectPreferenceCollection
        {
            ProjectId = 0,
            UserId = userId
        };

        var existingProjectsFromScalingApi = await _serviceScalingHttpClient.GetListOfProjects(userId);

        // If no items are returned from the scaling api, insert the default project preference
        if(existingProjectsFromScalingApi.Count == 0)
        {
            await _projectPreferencesCollection.InsertOneAsync(projectToInsert);
            return projectToInsert;
        }
        
        // If items are returned from the scaling api, insert the first item as the default project preference
        var firstProject = existingProjectsFromScalingApi.OrderBy(p => p.Name).First();

        projectToInsert.ProjectId = firstProject.Id;
        
        _projectPreferencesCollection.InsertOne(projectToInsert);
        
        var newProjectPreference = await _projectPreferencesCollection
         .Find(p => p.UserId == userId)
         .FirstOrDefaultAsync();

        return newProjectPreference;
    }

    public async Task<bool> UpdateProjectPreferenceAsync(int projectId, int userId)
    {
        try
        {
            var projectPreference = await _projectPreferencesCollection
                 .Find(p => p.UserId == userId)
                 .FirstOrDefaultAsync();

            if (projectPreference is not null)
            {

                projectPreference.ProjectId = projectId;

                await _projectPreferencesCollection.ReplaceOneAsync(p => p.UserId == userId, projectPreference);

                return true;
            }

            var newProjectPreference = new ProjectPreferenceCollection
            {
                ProjectId = projectId,
                UserId = userId
            };

            await _projectPreferencesCollection.InsertOneAsync(newProjectPreference);

            return true;
        }
        catch (Exception ex)
        {
            // TODO: Log the exception
            return false;
        }
    }
}


public class ProjectPreferenceCollection : IProjectPreference
{
    [JsonIgnore]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public int ProjectId { get; set; } = 0;
    public int UserId { get; set; } = 0;
}
