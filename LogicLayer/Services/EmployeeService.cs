using LogicLayer.Entitys;
using LogicLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Classes
{
    public class EmployeeService
    {
        private readonly IEmployeeRepo repository;

        public EmployeeService(IEmployeeRepo employeeRepo)
        {
            repository = employeeRepo;
        }

        public List<Employee> GetAllEmployeesPerEmployer(string email)
        {
            Console.WriteLine("Service: Fetching employees for email: " + email);
            return repository.GetAllemployeesPerEmployer(email);
        }
    }
}
