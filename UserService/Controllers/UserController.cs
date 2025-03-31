using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult UserRegistration()
        {
            return Ok(true);
        }
        [HttpPost]
        public IActionResult Login()
        {
            return Ok(true);
        }
    }
}
