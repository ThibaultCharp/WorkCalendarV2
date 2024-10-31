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

        [HttpGet("GetAllActivitiesPerEmployee")]
        public IActionResult GetAllActivitiesPerEmployee()
        {
            List<LogicLayer.Entitys.Activity> activities = activityService.GetAllActivitiesPerEmployee();
            return new JsonResult(activities);
        }

        [HttpPost("CreateActivity")]
        public IActionResult CreateActivity([FromBody] CreateActivityRequest request)
        {
            Console.WriteLine("CreateActivity hit");
            activityService.CreateActivity(request.position, request.begintime, request.endtime, request.date, request.employee_id);
            return new JsonResult("Activity created");
        }


        [HttpPost("EditActivity")]
        public IActionResult EditActivity([FromBody] EditActivityRequest request)
        {
            activityService.EditActivity(request.position, request.begintime, request.endtime, request.date);
            return new JsonResult("Done");
        }

    }
}   