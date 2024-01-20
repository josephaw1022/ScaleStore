using MediatR;

using ServiceScalingCore;

using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{
	public class CreateScalingConfigurationRequest : IRequest<ScalingConfigurationResponse>, ICreateScalingConfigurationRequest
	{
		public int ApplicationID { get; set; }
		public int EnvironmentID { get; set; }
		public int NumberOfInstances { get; set; }
	}

	public class CreateScalingConfigurationRequestHandler : IRequestHandler<CreateScalingConfigurationRequest, ScalingConfigurationResponse>
	{
		private readonly IScalingDbContext dbContext;

		public CreateScalingConfigurationRequestHandler(IScalingDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<ScalingConfigurationResponse> Handle(CreateScalingConfigurationRequest request, CancellationToken cancellationToken)
		{
			var newConfig = new ScalingConfiguration
			{
				ApplicationID = request.ApplicationID,
				EnvironmentID = request.EnvironmentID,
				NumberOfInstances = request.NumberOfInstances
			};

			dbContext.ScalingConfigurations.Add(newConfig);
			await dbContext.SaveChangesAsync(cancellationToken);

			return new ScalingConfigurationResponse
			{
				ScalingID = newConfig.ScalingID,
				ApplicationID = newConfig.ApplicationID,
				EnvironmentID = newConfig.EnvironmentID,
				NumberOfInstances = newConfig.NumberOfInstances
			};
		}
	}

}