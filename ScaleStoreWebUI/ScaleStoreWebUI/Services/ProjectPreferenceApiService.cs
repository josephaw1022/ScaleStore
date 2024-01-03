using PreferenceDTO;
using System.Text.Json;
namespace ScaleStoreWebUI.Services;

public class ProjectPreferenceApiService(HttpClient httpClient)
{
    public async Task<ProjectPreference> GetProjectPreference(int userId)
    {
        return await httpClient.GetFromJsonAsync<ProjectPreference>($"api/ProjectPreference/{userId}") ?? new();
    }


    public async Task<string> UpdateProjectPreference(int projectId, int userId)
    {
        var response = await httpClient.PostAsJsonAsync<UpdateProjectPreferenceRequest>($"api/ProjectPreference/{projectId}/{userId}", new UpdateProjectPreferenceRequest { });

        var responseContent = await response.Content.ReadAsStringAsync();

        return responseContent;
        
    }
}

public class UpdateProjectPreferenceRequest { }
