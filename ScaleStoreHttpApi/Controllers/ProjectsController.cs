using Microsoft.AspNetCore.Mvc;
using ServiceScalingDb.ScalingDb;
using ServiceScalingDTO;
using MediatR;
using ScaleStoreHttpApi.Requests;

namespace ScaleStoreHttpApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<List<ProjectsGetOneRequestResponse>>> GetProjects()
        {
            return Ok(await _mediator.Send(new ProjectsTableViewRequest()));
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectsGetOneRequestResponse>> GetProject(int id)
        {
            var project = await _mediator.Send(new ProjectsGetOneRequest(id));

            if (project is null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(ProjectCreateDTO updateProject)
        {

            var project = await _mediator.Send(new CreateProjectRequest(updateProject.Name));


            return CreatedAtAction("GetProject", new { id = project.Id }, project);

        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            throw new NotImplementedException();
        }

    }
}
