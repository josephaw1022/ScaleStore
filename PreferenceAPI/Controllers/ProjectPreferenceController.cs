using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PreferenceAPI.Services;
using PreferenceDTO;

namespace PreferenceAPI.Controllers;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProjectPreferenceController(ProjectPreferenceService projectPreferenceService) : ControllerBase
{
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<IProjectPreference>> GetProjectPreference(int userId)
    {
        var projectPreference = await projectPreferenceService.GetProjectPreferenceAsync(userId);
        return Ok(projectPreference);
    }

    [HttpPost("{projectId:int}/{userId:int}")]
    public async Task<IResult> UpdateProjectPreference(int projectId, int userId)
    {
        var result = await projectPreferenceService.UpdateProjectPreferenceAsync(projectId, userId);

        if (result)
        {
            return Results.Ok("Project preference updated successfully.");
        }

        return Results.BadRequest("Unable to update project preference.");
    }
}
