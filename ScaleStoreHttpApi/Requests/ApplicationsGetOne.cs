using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceScalingCore;
using ServiceScalingDb.ScalingDb;

namespace ScaleStoreHttpApi.Requests;


public class ApplicationGetOneRequest : IRequest<ApplicationGetOneRequestResponse>, IApplicationGetOneRequest
{
    public ApplicationGetOneRequest(int id)
    {
        Id = id;
    }

    public int Id { get; }
}   

public class ApplicationGetOneRequestResponse : IApplicationGetOneResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProjectId { get; set; }
}


public class ApplicationGetOneRequestHandler : IRequestHandler<ApplicationGetOneRequest, ApplicationGetOneRequestResponse>
{
    private readonly ScalingDbContext _context;

    public ApplicationGetOneRequestHandler(ScalingDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationGetOneRequestResponse> Handle(ApplicationGetOneRequest request, CancellationToken cancellationToken)
    {
        var application = await _context.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.ApplicationID == request.Id, cancellationToken);

        if (application is null)
        {
            return null!;
        }

        return new ApplicationGetOneRequestResponse
        {
            Id = application.ApplicationID,
            Name = application.ApplicationName,
            ProjectId = application.ProjectID
        };
    }
}


