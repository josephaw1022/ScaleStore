using MediatR;

using Microsoft.EntityFrameworkCore;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

namespace ServiceScalingWebApi.Requests;

public class ProjectGetManyNamesRequest : IProjectGetManyNamesRequest, IRequest<List<ProjectGetManyNamesResponseItem>>
{
}


public class ProjectGetManyNamesResponseItem : IProjectGetManyNamesResponseItem
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
}



public class ProjectGetManyNamesHandler : IRequestHandler<ProjectGetManyNamesRequest, List<ProjectGetManyNamesResponseItem>>
{
	private readonly ILogger<ProjectGetManyNamesHandler> _logger;
	private readonly IScalingDbContext _dbContext;
	public ProjectGetManyNamesHandler(ILogger<ProjectGetManyNamesHandler> logger, IScalingDbContext scalingDbContext)
	{
		_logger = logger;
		_dbContext = scalingDbContext;
	}

	public async Task<List<ProjectGetManyNamesResponseItem>> Handle(ProjectGetManyNamesRequest request, CancellationToken cancellationToken)
	{
		var projectNames = _dbContext.Projects.Select(p => new ProjectGetManyNamesResponseItem
		{
			Id = p.ProjectID,
			Name = p.ProjectName ?? string.Empty
		}).ToList();


		return projectNames;
	}


}