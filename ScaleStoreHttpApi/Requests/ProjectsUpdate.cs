using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests
{

    public class UpdateProjectRequest : IRequest<UpdateProjectResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UpdateProjectRequest(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class UpdateProjectResponse
    {
        public int Id { get; set; } = default!;
        public string Name { get; set; } = null!;


        public bool IsSuccess { get; set; } = default!;
    }



    public class UpdateProjectRequestHandler : IRequestHandler<UpdateProjectRequest, UpdateProjectResponse>
    {
        private readonly ScalingDbContext dbContext;

        public UpdateProjectRequestHandler(ScalingDbContext dbContext)
        {
            this.dbContext = dbContext;
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
