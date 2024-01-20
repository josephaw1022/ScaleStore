namespace ScaleStoreHttpApi.Requests
{

	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using MediatR;

	using Microsoft.EntityFrameworkCore;

	using ServiceScalingCore;

	using ServiceScalingDb.ScalingDb;

	public class GetManyEnvironmentsRequest : IRequest<List<GetManyEnvironmentsResponse>>, IGetManyEnvironmentsRequest
	{
		public int ProjectID { get; set; } = default!;
	}

	public class GetManyEnvironmentsResponse : IGetManyEnvironmentsResponse
	{
		public int EnvironmentID { get; set; }
		public string EnvironmentName { get; set; } = null!;
		public string ProjectName { get; set; } = null!;
	}

	public class GetManyEnvironmentsRequestHandler : IRequestHandler<GetManyEnvironmentsRequest, List<GetManyEnvironmentsResponse>>
	{
		private readonly IScalingDbContext dbContext;

		public GetManyEnvironmentsRequestHandler(IScalingDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<List<GetManyEnvironmentsResponse>> Handle(GetManyEnvironmentsRequest request, CancellationToken cancellationToken)
		{
			var environments = await dbContext.Environments
			  .Include(e => e.Project)
			  .Where(e => e.ProjectID == request.ProjectID)
			  .Select(e => new GetManyEnvironmentsResponse
			  {
				  EnvironmentID = e.EnvironmentID,
				  EnvironmentName = e.EnvironmentName,
				  ProjectName = e.Project.ProjectName
			  }
			  )
			  .ToListAsync(cancellationToken);

			return environments;
		}
	}

}