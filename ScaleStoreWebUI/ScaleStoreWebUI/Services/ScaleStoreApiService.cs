using ServiceScalingDTO;

namespace ScaleStoreWebUI.Services
{
    public class ScaleStoreApiService(HttpClient httpClient)
    {

        public async Task<List<ProjectsListModel>> GetProjects()
        {
            return await httpClient.GetFromJsonAsync<List<ProjectsListModel>>("api/projects");
        }



        public async Task<List<EnvironmentListModel>> GetEnvironments(int projectId)
        {
            return await httpClient.GetFromJsonAsync<List<EnvironmentListModel>>($"api/environment?projectId={projectId}");
        }


        public async Task<List<ApplicationListModel>> GetApplications(int projectId)
        {
            return await httpClient.GetFromJsonAsync<List<ApplicationListModel>>($"api/application?projectId={projectId}");
        }
    }


    public class ApplicationListModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;


        public int ProjectId { get; set; }

    }




    public class ProjectsListModel
    {
        public string name { get; set; } = null!;

        public int numberOfApplications { get; set; }

        public int numberOfEnvironments { get; set; }

    }



    public class EnvironmentListModel
    {

        public int environmentID { get; set; }

        public string environmentName { get; set; } = null!;

        public string projectName { get; set; } = null!;
    }

}
