using PreferenceDTO;

namespace ScaleStoreWebUI.Services;

public class ProjectPreferenceApiService(HttpClient httpclient)
{
    public async Task<ProjectPreference> GetProjectPreference(int userId)
    {
        return await httpclient.GetFromJsonAsync<ProjectPreference>($"api/ProjectPreference/{userId}") ?? new();
    }

    public async Task<bool> UpdateProjectPreference(int projectId, int userId)
    {
        var response = await httpclient.PutAsync($"api/ProjectPreference/{projectId}/{userId}", null!);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}


