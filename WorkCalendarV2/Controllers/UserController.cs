using Microsoft.AspNetCore.Mvc;
using LogicLayer.Classes;
using DAL.Repos;
using System.Diagnostics;
using WorkCalendarV2.Models;
using LogicLayer.IRepos;
using LogicLayer.Entitys;
using WorkCalendarV2.Requests;


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
    }
}
