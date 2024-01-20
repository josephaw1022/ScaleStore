using MediatR;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;

public class CreateProjectResponse : ICreateProjectResponse
{
	public int Id { get; set; } = default!;
	public string Name { get; set; } = null!;
}



public class CreateProjectRequest : IRequest<CreateProjectResponse>, ICreateProjectRequest
{
	public string Name { get; set; } = null!;

	public CreateProjectRequest(string name)
	{
		Name = name;
	}


	public CreateProjectRequest()
	{
	}
}



public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
	private readonly IScalingDbContext dbContext;

	public CreateProjectRequestHandler(IScalingDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
	{
		var newProject = new Project
		{
			ProjectName = request.Name
		};

		dbContext.Projects.Add(newProject);
		await dbContext.SaveChangesAsync(cancellationToken);

		return new CreateProjectResponse
		{
			Id = newProject.ProjectID,
			Name = newProject.ProjectName
		};
	}
}