namespace ScaleStoreHttpApi.Requests
{

    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using ServiceScalingDb.ScalingDb;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetManyEnvironmentsRequest : IRequest<List<GetManyEnvironmentsResponse>>
    {
        public int ProjectID { get; set; } = default!;
    }

    public class GetManyEnvironmentsResponse
    {
        public int EnvironmentID { get; set; }
        public string EnvironmentName { get; set; } = null!;
        public int ProjectID { get; set; }
    }

    public class GetManyEnvironmentsRequestHandler : IRequestHandler<GetManyEnvironmentsRequest, List<GetManyEnvironmentsResponse>>
    {
        private readonly ScalingDbContext dbContext;

        public GetManyEnvironmentsRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetManyEnvironmentsResponse>> Handle(GetManyEnvironmentsRequest request, CancellationToken cancellationToken)
        {
            var environments = await dbContext.Environments.
                Where(e => e.ProjectID == request.ProjectID).
                ToListAsync(cancellationToken);

            return environments.Select(e => new GetManyEnvironmentsResponse
            {
                EnvironmentID = e.EnvironmentID,
                EnvironmentName = e.EnvironmentName,
                ProjectID = e.ProjectID
            }).ToList();
        }
    }

}
