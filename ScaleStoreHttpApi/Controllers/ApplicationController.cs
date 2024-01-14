using Microsoft.AspNetCore.Mvc;
using MediatR;
using ScaleStoreHttpApi.Requests;
using ServiceScalingDTO;
using Microsoft.AspNetCore.OutputCaching;
using Asp.Versioning;


namespace ScaleStoreHttpApi.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[OutputCache(Duration = 15)]
public class ApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOne(int id)
    {
        
        var getOne = await _mediator.Send(new ApplicationGetOneRequest(id));

        if (getOne is null)
        {
            return NotFound();
        }

        return Ok(getOne);
    }

    [HttpGet]
    public async Task<IActionResult> GetApplicationsByProject([FromQuery] int projectId)
    {
        var getMany = await _mediator.Send(new ApplicationsGetManyRequest(projectId));
        return Ok(getMany);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateApplicationDTO application)
    {
        var create = await _mediator.Send(new CreateApplicationRequest(application.Name, application.ProjectId));
        return CreatedAtAction(nameof(GetOne), new { id = create.Id }, create);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateApplicationDTO application)
    {
        
        var update = await _mediator.Send(new UpdateApplicationRequest(id, application.Name, application.ProjectId));


        if (!update.Success)
        {
            return NotFound();
        }

        return Ok(update);

    }
}
