using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Entities;
using LogicLayer.Entitys;
using LogicLayer.IRepos;

namespace TestUnit.FakeRepos
{
    public class FakeUserRepo : IUserRepo
    {
        private readonly List<User> _users = new List<User>();

        public FakeUserRepo()
        {
            // Seed some users for testing
            var roleAdmin = new Role { id = 1, name = "Admin" };
            var roleEmployee = new Role { id = 2, name = "Employee" };
            var roleEmployer = new Role { id = 3, name = "Employer" };

            _users.Add(new User { id = 1, name = "Alice", email = "alice@example.com", role = roleAdmin });
            _users.Add(new User { id = 2, name = "Bob", email = "bob@example.com", role = roleEmployee });
            _users.Add(new User { id = 3, name = "Charlie", email = "charlie@example.com", role = roleEmployer });
        }

        public void LinkUser(string loggedInUserEmail, string targetUserEmail)
        {
            var employer = _users.FirstOrDefault(u => u.email == loggedInUserEmail);
            var employee = _users.FirstOrDefault(u => u.email == targetUserEmail);

            if (employer != null && employee != null && employer.role.name == "Employer")
            {
                employee.role = new Role { id = 3, name = "Employee under Employer" };
            }
        }

        public void ChangeUserRole(string email, int roleId, bool makeEmployer)
        {
            var user = _users.FirstOrDefault(u => u.email == email);
            if (user != null)
            {
                user.role = new Role { id = roleId, name = makeEmployer ? "Employer" : "Other" };
            }
        }

        public bool UserExists(int userId)
        {
            return _users.Any(u => u.id == userId);
        }

        public void CreateUser(User user)
        {
            if (!_users.Any(u => u.id == user.id))
            {
                _users.Add(user);
            }
        }

        public List<User> GetAllEmployeesWithoutEmployer(string input)
        {
            return _users
                .Where(u => u.role.name == "Employee" &&
                            (u.name.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                             u.email.Contains(input, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public List<User> GetAllUsersWithCorrespondingRoles(string input)
        {
            return _users
                .Where(u => u.name.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                            u.email.Contains(input, StringComparison.OrdinalIgnoreCase) ||
                            u.role.name.Contains(input, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
