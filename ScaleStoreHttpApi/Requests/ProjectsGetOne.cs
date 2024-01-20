using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;


namespace ScaleStoreHttpApi.Requests
{
	public class ProjectsGetOneRequest : IRequest<ProjectsGetOneRequestResponse>, IProjectsGetOneRequest
	{
		public ProjectsGetOneRequest(int id)
		{
			Id = id;
		}

		public int Id { get; set; }
	}


	public class ProjectsGetOneRequestResponse : IProjectsGetOneResponse
	{
		public int Id { get; set; } = default!;
		public string Name { get; set; } = null!;
	}




	public class ProjectsGetOneRequestHandler : IRequestHandler<ProjectsGetOneRequest, ProjectsGetOneRequestResponse>
	{
		private readonly IScalingDbContext dbContext;
		public ProjectsGetOneRequestHandler(IScalingDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<ProjectsGetOneRequestResponse> Handle(ProjectsGetOneRequest request, CancellationToken cancellationToken)
		{
			var id = request.Id;

			var project = await dbContext.Projects.Select(project =>
			new ProjectsGetOneRequestResponse
			{
				Id = project.ProjectID,
				Name = project.ProjectName ?? string.Empty,
			}).SingleOrDefaultAsync(project => project.Id == id, cancellationToken);

			return project;
		}
	}
}