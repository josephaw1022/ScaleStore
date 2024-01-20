using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingDb.ScalingDb;
using ServiceScalingCore;

namespace ScaleStoreHttpApi.Requests;


public class UpdateApplicationRequest : IRequest<UpdateApplicationResponse> , IUpdateApplicationRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProjectId { get; set; }

    public UpdateApplicationRequest(int id, string name, int projectId)
    {
        Id = id;
        Name = name;
        ProjectId = projectId;
    }
}


public class UpdateApplicationResponse : IUpdateApplicationResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProjectId { get; set; }
    public bool Success { get; set; }
}

public class UpdateApplicationRequestHandler : IRequestHandler<UpdateApplicationRequest, UpdateApplicationResponse>
{
    private readonly IScalingDbContext _context;

    public UpdateApplicationRequestHandler(IScalingDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateApplicationResponse> Handle(UpdateApplicationRequest request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.ApplicationID == request.Id, cancellationToken);

        if (application is null)
        {
            return new UpdateApplicationResponse { Success = false };
        }

        application.ApplicationName = request.Name;
        application.ProjectID = request.ProjectId;
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateApplicationResponse
        {
            Id = application.ApplicationID,
            Name = application.ApplicationName,
            ProjectId = application.ProjectID,
            Success = true
        };
    }
}
