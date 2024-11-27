using DAL.Repos;
using LogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using WorkCalendarV2.Requests;

namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _authService = new AuthService(new AuthRepo(), configuration);  // Pass configuration to the service
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                _authService.RegisterUser(request.name, request.email, request.password);
                return new JsonResult("User registered successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var token = _authService.LoginUser(request.email, request.password);

            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return new JsonResult(new { token });
        }
    }
}
