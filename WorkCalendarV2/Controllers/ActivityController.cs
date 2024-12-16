using Microsoft.AspNetCore.Mvc;
using LogicLayer.Classes;
using DAL.Repos;
using System.Diagnostics;
using WorkCalendarV2.Models;
using LogicLayer.IRepos;
using LogicLayer.Entitys;
using WorkCalendarV2.Requests;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LogicLayer.Entities;
using System.Xml.Linq;


namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ActivityController : ControllerBase
    {
        private readonly ActivityService activityService;

        private readonly ILogger<ActivityController> _logger;

        public ActivityController(ILogger<ActivityController> logger)
        {
            _logger = logger;
            activityService = new ActivityService(new ActivityRepo());
        }

        [HttpGet("GetActivitiesForCurrentUser")]
        [Authorize]
        public IActionResult GetActivitiesForCurrentUser()
         {
            var email = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value;
            var name = User.Claims.FirstOrDefault(b => b.Type == ClaimTypes.Name)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email not found in token.");
            }
            List<LogicLayer.Entitys.Activity> activities = activityService.GetActivitiesByUserEmail(email, role);
            return new JsonResult(new { activities, name, role });
        }


        [HttpPost("CreateActivity")]
        public IActionResult CreateActivity([FromBody] CreateActivityRequest request)
        {
            Console.WriteLine("CreateActivity hit");
            activityService.CreateActivity(request.position_id, request.begintime, request.endtime, request.date, request.employee_id);
            return new JsonResult("Activity created");
        }


        [HttpPut("UpdateActivity")]
        public ActionResult UpdateActivity([FromBody] UpdateActivityRequest request)
        {
            if (request == null)
            {
                return BadRequest("Activity data is null.");
            }

            try
            {
                activityService.UpdateActivity(
                    request.ActivityId,
                    request.position, 
                    request.begintime, 
                    request.endtime, 
                    request.date,
                    request.employer_name,
                    request.employer_email
                );
                
                return Ok("Activity updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating activity: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}   