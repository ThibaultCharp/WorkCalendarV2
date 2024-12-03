using Microsoft.AspNetCore.Mvc;
using LogicLayer.Classes;
using DAL.Repos;
using System.Diagnostics;
using WorkCalendarV2.Models;
using LogicLayer.IRepos;
using LogicLayer.Entitys;
using WorkCalendarV2.Requests;
using LogicLayer.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Org.BouncyCastle.Asn1.Ocsp;


namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        private readonly ILogger<UserController> _logger;


        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            userService = new UserService(new UserRepo());
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            userService.CreateUserIfNotExisting(user);
            return new JsonResult("User creation attempt complete");
        }

        [HttpGet("GetAllAvailableUsers")]
        public IActionResult GetAllAvailableUsers()
        {
            List<User> users = userService.GetAllUsersWithoutEmployer();
            return new JsonResult(users);
        }



        [HttpGet("GetCurrentUser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            // Extract claims from the token
            var email = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value;
            var name = User.Claims.FirstOrDefault(b => b.Type == ClaimTypes.Name)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Check if email is present (basic validation)
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email not found in token.");
            }

            // Return the extracted information
            return Ok(new { name, email, role });
        }


        [HttpPut("LinkUser")]
        [Authorize]
        public IActionResult LinkUserToEmployer(LinkUserRequest request)
        {
            userService.LinkUser(request.LoggedInUserEmail, request.TargetUserEmail);
            return new JsonResult("linked!!");
        }
    }
}