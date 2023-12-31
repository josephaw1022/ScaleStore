using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PreferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Project = "Test Project"
            });
        }




    }
}
