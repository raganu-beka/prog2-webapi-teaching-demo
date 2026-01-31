using Microsoft.AspNetCore.Mvc;

namespace Prog2WebApi.Controllers
{
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
        public IActionResult HelloPost(Name name)
        {
            return Ok($"Hello from the POST endpoint, {name}");
        }
    }

    public record Name(string name);
}
