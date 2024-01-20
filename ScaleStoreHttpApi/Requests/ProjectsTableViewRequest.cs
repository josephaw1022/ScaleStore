using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;



public class ProjectsTableViewRequest : IRequest<List<ProjectTableViewRequestResponse>>, IProjectsTableViewRequest { }

public class ProjectTableViewRequestResponse : IProjectTableViewResponse
{
	public int Id { get; set; } = default!;
	public string Name { get; set; } = null!;
	public int NumberOfEnvironments { get; set; } = default!;
	public int NumberOfApplications { get; set; } = default!;

}

public class ProjectsTableViewRequestHandler : IRequestHandler<ProjectsTableViewRequest, List<ProjectTableViewRequestResponse>>
{
	private readonly IScalingDbContext dbContext;

	public ProjectsTableViewRequestHandler(IScalingDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<List<ProjectTableViewRequestResponse>> Handle(ProjectsTableViewRequest request, CancellationToken cancellationToken)
	{
		var projects = await dbContext.Projects
			.Include(p => p.Environments)
			.Include(p => p.Applications)
			.ToListAsync(cancellationToken);

		var response = projects.Select(p => new ProjectTableViewRequestResponse
		{
			Id = p.ProjectID,
			Name = p.ProjectName ?? string.Empty,
			NumberOfEnvironments = p.Environments.Count,
			NumberOfApplications = p.Applications.Count
		}).ToList();

		return response;
	}
}