using MongoDB.Bson;
using MongoDB.Driver;
using PreferenceDTO;
using System.Text.Json.Serialization;

namespace PreferenceAPI.Services;

public class ProjectPreferenceService
{
    private readonly IMongoCollection<ProjectPreferenceCollection> _projectPreferencesCollection;

    public ProjectPreferenceService(IMongoClient mongoClient)
    {
        _projectPreferencesCollection = mongoClient.GetDatabase("preference")
                                                   .GetCollection<ProjectPreferenceCollection>("projectpreferences");
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


        // If the project preference does not exist, create it
        await _projectPreferencesCollection.InsertOneAsync(new ProjectPreferenceCollection
        {
            ProjectId = 0,
            UserId = userId
        });

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
