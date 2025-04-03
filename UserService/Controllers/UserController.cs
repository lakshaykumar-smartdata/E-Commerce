using Microsoft.AspNetCore.Mvc;
using UserService.Dto;
using UserService.Services; // For accessing UserService
using System.Threading.Tasks;
using UserService.Models;
using UserService.Enums;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Inject IUserService into the controller
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // User Registration Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            // Create User model from DTO
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ContactNumber = request.ContactNumber,
                UserRole = request.UserRole,
                PasswordHash = request.Password
            };

            // Register the user and hash the password
            var registrationResult = await _userService.UserRegistration(user);
            if (registrationResult == "User registered successfully.")
                return Ok(new { success = true, message = registrationResult });

            return BadRequest(new { success = false, message = registrationResult });
        }

        // User Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            // Perform login with email and password
            var token = await _userService.UserLogin(request.Email, request.Password);

            if (token == "Invalid email or password.")
                return Unauthorized(new { success = false, message = token });

            // Return JWT token
            return Ok(new { success = true, message = "User logged in successfully.", token = token });
        }
    }
}
