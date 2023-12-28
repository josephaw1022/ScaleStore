using ServiceScalingCore;

namespace ScaleStoreWebUI.Services;

public class ScaleStoreApiService(HttpClient httpClient)
{
    public async Task<List<ProjectTableRow>?> GetProjects()
    {
        return await httpClient.GetFromJsonAsync<List<ProjectTableRow>>("api/projects");
    }

    public async Task<List<EnvironmentTableRow>?> GetEnvironments(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<EnvironmentTableRow>>($"api/environment?projectId={projectId}");
    }

    public async Task<List<ApplicationTableRow>?> GetApplications(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<ApplicationTableRow>>($"api/application?projectId={projectId}");
    }


    public async Task<List<ScalingConfigurationTableRow>?> GetScalingConfigurations(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<ScalingConfigurationTableRow>>($"api/ScalingConfiguration?projectId={projectId}");
    }


}



public class ProjectTableRow : IProjectTableViewResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int NumberOfEnvironments { get; set; }
    public int NumberOfApplications { get; set; }
}


public class EnvironmentTableRow : IGetManyEnvironmentsResponse
{

    public int EnvironmentID { get; set; }

    public string EnvironmentName { get; set; } = null!;

    
    public string ProjectName { get; set; } = null!;

 }


public class ApplicationTableRow : IApplicationsGetManyResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectId { get; set; }
}


public class ScalingConfigurationTableRow : IGetScalingConfigurationResponse
{
    public int ScalingID { get; set; }

    public int ApplicationID { get; set; }

    public int EnvironmentID { get; set; }

    public int NumberOfInstances { get; set; }

    public string ApplicationName { get; set; } = null!;

    public string EnvironmentName { get; set; } = null!;

    public string ProjectName { get; set; } = null!;
}
