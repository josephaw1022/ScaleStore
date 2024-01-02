using ScaleStoreWebUI.StateManagement;

namespace ScaleStoreWebUI.Services;

public class ProjectPreferenceStateService(StateManagementService stateManagementService, ProjectPreferenceApiService projectPreferenceApiService, ScaleStoreApiService scaleStoreApiService)
{

    public async Task<ProjectName> GetProjectPreference(int userId, string circuitId)
    {
        var projectPreference = await stateManagementService.Get<ProjectName>("projectPreference", circuitId);

        if (projectPreference is not null)
        {
            return projectPreference;
        }

        var apiProjectPreference = await projectPreferenceApiService.GetProjectPreference(userId);
        var listOfProjects = await scaleStoreApiService.GetListOfProjectNames(userId);

        ProjectName? project = null;

        foreach (var item in listOfProjects)
        {
            if (item.Id == apiProjectPreference.ProjectId)
            {
                project = item;
                await stateManagementService.Set("projectPreference", circuitId, project);
                break;
            }
        }


        if (project is not null && project is not { Id: 0 })
        {
            await SetProjectPreference(userId, project);
        }


        return project ?? new();
    }


    public async Task<bool> SetProjectPreference(int userId, ProjectName project)
    {
        return await projectPreferenceApiService.UpdateProjectPreference(project.Id, userId);
    }

    public async Task<List<ProjectName>> GetProjectNames(int userId)
    {
        var projectNames = await scaleStoreApiService.GetListOfProjectNames(userId) ?? new();
        return projectNames;
    }


}
