using ServiceScalingCore;

namespace PreferenceAPI.Services;

public class ServiceScalingHttpApiService(HttpClient _httpClient)
{

    public async Task<List<ProjectName>> GetListOfProjects(int userId) => 
        await _httpClient.GetFromJsonAsync<List<ProjectName>>($"api/v1.0/Projects?userId={userId}") ?? new List<ProjectName>();
    
}

public class ProjectName : IProjectGetManyNamesResponseItem
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }
}
