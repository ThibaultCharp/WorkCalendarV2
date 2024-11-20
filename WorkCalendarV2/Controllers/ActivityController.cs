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
        public IActionResult GetActivitiesForCurrentUser([FromQuery] int? userId)
        {
            if (!userId.HasValue || userId <= 0)
            {
                return BadRequest("Valid User ID is required.");
            }

            // Call the service to retrieve activities for the given userId
            var activities = activityService.GetActivitiesByUserId(userId.Value);

            if (activities == null || !activities.Any())
            {
                return NotFound("No activities found for the given User ID.");
            }

            return Ok(activities);
        }

        [HttpPost("CreateActivity")]
        public IActionResult CreateActivity([FromBody] CreateActivityRequest request)
        {
            Console.WriteLine("CreateActivity hit");
            activityService.CreateActivity(request.position_id, request.begintime, request.endtime, request.date, request.employee_id);
            return new JsonResult("Activity created");
        }


        [HttpPut("UpdateActivity")]
        public ActionResult UpdateActivity(int id, [FromBody] UpdateActivityRequest request)
        {
            if (request == null)
            {
                return BadRequest("Activity data is null.");
            }

            try
            {
                activityService.UpdateActivity(
                    id, 
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