using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

	public class UpdateProjectRequest : IRequest<UpdateProjectResponse>, IUpdateProjectRequest
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public UpdateProjectRequest(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}

	public class UpdateProjectResponse : IUpdateProjectResponse
	{
		public int Id { get; set; } = default!;
		public string Name { get; set; } = null!;


		public bool IsSuccess { get; set; } = default!;
	}



	public class UpdateProjectRequestHandler : IRequestHandler<UpdateProjectRequest, UpdateProjectResponse>
	{
		private readonly IScalingDbContext dbContext;

		private readonly IDistributedCache cache;

		public UpdateProjectRequestHandler(IScalingDbContext dbContext, IDistributedCache cache)
		{
			this.dbContext = dbContext;
			this.cache = cache;
		}

		public async Task<UpdateProjectResponse> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
		{
			var id = request.Id;
			var name = request.Name;

			var project = await dbContext.Projects.SingleOrDefaultAsync(project => project.ProjectID == id, cancellationToken);

			if (project is null)
			{
				return new UpdateProjectResponse
				{
					Id = id,
					Name = name,
					IsSuccess = false
				};

				// Right here I want to clear all of the cache keys that include project in them



			}

			project.ProjectName = name;
			await dbContext.SaveChangesAsync(cancellationToken);

			return new UpdateProjectResponse
			{
				Id = id,
				Name = name,
				IsSuccess = true
			};
		}
	}




}