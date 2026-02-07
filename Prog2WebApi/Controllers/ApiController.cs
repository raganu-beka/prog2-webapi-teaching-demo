using Microsoft.AspNetCore.Mvc;

namespace Prog2WebApi.Controllers
{
    public record NameRequest(string Name);
    
    [ApiController]
    [Route("api")]
    public class ApiController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult HelloApi()
        {
            return Ok("Hello from the API!");
        }

        [HttpPost("hello")]
        public IActionResult HelloPost(NameRequest name)
        {
            return Ok($"Hello from the POST endpoint, {name.Name}");
        }
    }
}
