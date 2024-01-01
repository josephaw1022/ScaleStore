using PreferenceDTO;

namespace ScaleStoreWebUI.Services;

public class ProjectPreferenceApiService(HttpClient httpclient)
{
    public async Task<ProjectPreference> GetProjectPreference(int userId)
    {
        return await httpclient.GetFromJsonAsync<ProjectPreference>($"api/ProjectPreference/{userId}") ?? new();
    }
}


