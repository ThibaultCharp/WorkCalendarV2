using Microsoft.AspNetCore.Mvc;
using LogicLayer.Classes;
using DAL.Repos;
using System.Diagnostics;
using WorkCalendarV2.Models;
using LogicLayer.IRepos;
using LogicLayer.Entitys;


namespace WorkCalendarV2.Controllers
{
    public class ActivityController : Controller
    {
        ActivityService activityService = new ActivityService(new ActivityRepo());
        
        public IActionResult GetAllActivitiesPerEmployee()
        {
            List<LogicLayer.Entitys.Activity> activities = activityService.GetAllActivitiesPerEmployee();
            return Json(activities);
        }
    }
}
