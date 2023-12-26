using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScaleStoreHttpApi.Requests;

namespace ScaleStoreHttpApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ScalingConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScalingConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: scalingconfiguration/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await _mediator.Send(new GetScalingConfigurationRequest { ScalingID = id });
        if (response == null) return NotFound();
        return Ok(response);
    }

    // GET: scalingconfiguration
    [HttpGet]
    public async Task<IActionResult> GetAll(int projectId)
    {
        var response = await _mediator.Send(new GetManyScalingConfigurationsRequest(projectId));
        return Ok(response);
    }

    // POST: scalingconfiguration
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateScalingConfigurationRequest request)
    {
        var response = await _mediator.Send(request);
        return CreatedAtAction(nameof(Get), new { id = response.ScalingID }, response);
    }

    // PUT: scalingconfiguration/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateScalingConfigurationRequest request)
    {
        if (id != request.ScalingID) return BadRequest("ID mismatch");

        var response = await _mediator.Send(request);
        if (response == null) return NotFound();
        return Ok(response);
    }
}
