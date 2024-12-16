using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entities;
using LogicLayer.Entitys;
using LogicLayer.IRepos;

namespace TestUnit.FakeRepos
{
    public class FakeEmployeeRepo : IEmployeeRepo
    {
        private readonly List<Employee> _employees;

        public FakeEmployeeRepo()
        {
            _employees = new List<Employee>
            {
                new Employee
                {
                    id = 1,
                    user = new User
                    {
                        id = 1,
                        name = "John Doe",
                        email = "john.doe@example.com",
                        role = new Role { id = 1, name = "Employee" }
                    },
                    employer = new Employer
                    {
                        id = 1,
                        user = new User
                        {
                            id = 2,
                            name = "Employer1",
                            email = "employer1@example.com"
                        }
                    }
                },
                new Employee
                {
                    id = 2,
                    user = new User
                    {
                        id = 2,
                        name = "Jane Smith",
                        email = "jane.smith@example.com",
                        role = new Role { id = 1, name = "Employee" }
                    },
                    employer = new Employer
                    {
                        id = 1,
                        user = new User
                        {
                            id = 2,
                            name = "Employer1",
                            email = "employer1@example.com"
                        }
                    }
                },
                new Employee
                {
                    id = 3,
                    user = new User
                    {
                        id = 3,
                        name = "Mike Johnson",
                        email = "mike.johnson@example.com",
                        role = new Role { id = 1, name = "Employee" }
                    },
                    employer = new Employer
                    {
                        id = 3,
                        user = new User
                        {
                            id = 4,
                            name = "Employer2",
                            email = "employer2@example.com"
                        }
                    }
                }
            };
        }

        public List<Employee> GetAllemployeesPerEmployer(string email)
        {
            return _employees.Where(e => e.employer.user.email == email).ToList();
        }
    }
}
