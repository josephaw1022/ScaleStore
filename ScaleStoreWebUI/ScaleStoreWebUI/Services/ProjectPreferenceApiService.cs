using PreferenceDTO;
namespace ScaleStoreWebUI.Services;

public class ProjectPreferenceApiService(HttpClient httpClient)
{
	public async Task<ProjectPreference> GetProjectPreference(int userId)
	{
		return await httpClient.GetFromJsonAsync<ProjectPreference>($"api/v1.0/ProjectPreference/{userId}") ?? new();
	}

	public async Task<string> UpdateProjectPreference(int projectId, int userId)
	{
		var response = await httpClient.PostAsJsonAsync<UpdateProjectPreferenceRequest>($"api/v1.0/ProjectPreference/{projectId}/{userId}", new UpdateProjectPreferenceRequest { });

		var responseContent = await response.Content.ReadAsStringAsync();

		return responseContent;

	}
}

public class UpdateProjectPreferenceRequest { }