using ServiceScalingCore;
using ServiceScalingDTO;

namespace ScaleStoreWebUI.Services;

public class ScaleStoreApiService(HttpClient httpClient, ILogger<ScaleStoreApiService> logger)
{
    public async Task<List<ProjectTableRow>?> GetProjects()
    {
        return await httpClient.GetFromJsonAsync<List<ProjectTableRow>>("api/v1.0/projects");
    }

    public async Task<List<EnvironmentTableRow>?> GetEnvironments(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<EnvironmentTableRow>>($"api/v1.0/environment?projectId={projectId}");
    }

    public async Task<List<ApplicationTableRow>?> GetApplications(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<ApplicationTableRow>>($"api/v1.0/application?projectId={projectId}");
    }


    public async Task<List<ScalingConfigurationTableRow>?> GetScalingConfigurations(int projectId)
    {
        return await httpClient.GetFromJsonAsync<List<ScalingConfigurationTableRow>>($"api/v1.0/ScalingConfiguration?projectId={projectId}");
    }

    public async Task<List<ProjectName>> GetListOfProjectNames(int userId) =>
        await httpClient.GetFromJsonAsync<List<ProjectName>>($"api/v1.0/Projects?userId={userId}") ?? new List<ProjectName>();


    public async Task<UpdateScalingConfigurationResponse> UpdateScalingConfiguration(UpdateScalingConfigurationRequest scalingConfiguration)
    {
        await httpClient.PutAsJsonAsync($"api/v1.0/ScalingConfiguration/{scalingConfiguration.ScalingID}", scalingConfiguration);
        return new UpdateScalingConfigurationResponse
        {
            ScalingID = scalingConfiguration.ScalingID,
            ApplicationID = scalingConfiguration.ApplicationID,
            EnvironmentID = scalingConfiguration.EnvironmentID,
            NumberOfInstances = scalingConfiguration.NumberOfInstances
        };
    }

    public async Task<bool> CreateApplication(string name, int projectId)
    {
        try
        {

            var newApplication = new CreateApplicationDTO
            {
                Name = name,
                ProjectId = projectId
            };

            var message = await httpClient.PostAsJsonAsync($"api/v1.0/Application", newApplication);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating application");
            return false;
        }
    }



    public async Task<bool> CreateProject(string name)
    {
        try
        {

            var newProject = new ProjectCreateDTO
            {
                Name = name,
            };

            var message = await httpClient.PostAsJsonAsync($"api/v1.0/Projects", newProject);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating project");
            return false;
        }
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


public class ScalingConfigurationTableRow : IScalingConfigurationTableViewResponse
{
    public int Id { get; set; }
    public string EnvironmentName { get; set; } = null!;
    public string ApplicationName { get; set; } = null!;
    public int NumberOfInstances { get; set; }

}

public class ProjectName : IProjectGetManyNamesResponseItem
{
    public string Name { get; set; } = null!;

    public int Id { get; set; }
}




public class UpdateScalingConfigurationRequest : IUpdateScalingConfigurationRequest
{
    public int ScalingID { get; set; }
    public int ApplicationID { get; set; }
    public int EnvironmentID { get; set; }
    public int NumberOfInstances { get; set; }
}


public class UpdateScalingConfigurationResponse : IScalingConfigurationResponse
{
    public int ScalingID { get; set; }
    public int ApplicationID { get; set; }
    public int EnvironmentID { get; set; }
    public int NumberOfInstances { get; set; }
}