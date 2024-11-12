using DAL.Repos;
using LogicLayer.Classes;
using LogicLayer.Entitys;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkCalendarV2.Models;

namespace WorkCalendarV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EmployeeController : ControllerBase
    {
        EmployeeService employeeService = new EmployeeService(new EmployeeRepo());

        [HttpGet("GetAllEmployeesPerEmployer")]
        public IActionResult GetAllEmployeesPerEmployer()
        {
            List<Employee> employees = employeeService.GetAllEmployeesPerEmployer();
            return new JsonResult(employees);
        }
    }
}
