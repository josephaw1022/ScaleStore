using Microsoft.AspNetCore.Mvc;
using ServiceScalingDb.ScalingDb;
using ServiceScalingDTO;
using MediatR;
using ScaleStoreHttpApi.Requests;
using ServiceScalingWebApi.Requests;
using Microsoft.AspNetCore.OutputCaching;
using Asp.Versioning;

namespace ScaleStoreHttpApi.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [OutputCache(Duration = 5)]
    public class ProjectsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectsGetOneRequestResponse>>> GetProjects()
        {
            return Ok(await _mediator.Send(new ProjectsTableViewRequest()));
        }


        [HttpGet]
        [Route("names")]
        public async Task <ActionResult<List<ProjectGetManyNamesResponseItem>>> GetProjectNames()
        {
            var result = await _mediator.Send(new ProjectGetManyNamesRequest());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProjectsGetOneRequestResponse>> GetProject(int id)
        {
            var project = await _mediator.Send(new ProjectsGetOneRequest(id));

            if (project is null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutProject(int id, ProjectUpdateDTO project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }


            var updateProject = await _mediator.Send(new UpdateProjectRequest(id, project.Name));


            if (!updateProject.IsSuccess)
            {
                return NotFound();
            }


            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectCreateDTO updateProject)
        {

            var project = await _mediator.Send(new CreateProjectRequest(updateProject.Name));


            return CreatedAtAction("GetProject", new { id = project.Id }, project);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

    }
}
