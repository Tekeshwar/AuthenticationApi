using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        public UserController() { }

        // getuser endpoint
        [HttpGet("getuser")]
        public IActionResult GetUser()
        {
            // For demonstration, returning a static user object
            var user = new
            {
                Id = 1,
                Username = "admin",
                Email = ""
            };
            return Ok(user);
        }
    }
}
