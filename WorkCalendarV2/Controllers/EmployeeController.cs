using DAL.Repos;
using LogicLayer.Classes;
using LogicLayer.Entitys;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkCalendarV2.Models;

namespace WorkCalendarV2.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeService employeeService = new EmployeeService(new EmployeeRepo());

        public IActionResult GetAllEmployeesPerEmployer()
        {
            List<Employee> employees = employeeService.GetAllEmployeesPerEmployer();
            return Json(employees);
        }
    }
}
