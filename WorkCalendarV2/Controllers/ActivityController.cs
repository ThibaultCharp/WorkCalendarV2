using Microsoft.AspNetCore.Mvc;
using LogicLayer.Classes;
using DAL.Repos;
using System.Diagnostics;
using WorkCalendarV2.Models;
using LogicLayer.IRepos;
using LogicLayer.Entitys;


namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ActivityController : Controller
    {
        ActivityService activityService = new ActivityService(new ActivityRepo());

        [HttpGet("GetAllActivitiesPerEmployee")]
        public IActionResult GetAllActivitiesPerEmployee()
        {
            List<LogicLayer.Entitys.Activity> activities = activityService.GetAllActivitiesPerEmployee();
            return Json(activities);
        }

        [HttpPost("CreateActivity")]
        public IActionResult CreateActivity([FromBody] LogicLayer.Entitys.Activity activity)
        {
            activityService.CreateActivity(activity);
            return Json("Activity created");
        }
    }
}