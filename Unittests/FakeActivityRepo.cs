using System;
using System.Collections.Generic;
using System.Linq;
using LogicLayer.Entitys;
using LogicLayer.IRepos;

namespace LogicLayer.Tests
{
    public class FakeActivityRepo : IActivityRepo
    {
        private readonly List<Activity> _activities = new List<Activity>();
        private int _nextId = 1;

        public FakeActivityRepo()
        {
            // Dummy data
            _activities.Add(new Activity
            {
                id = _nextId++,
                begintime = new TimeOnly(9, 0),
                endtime = new TimeOnly(17, 0),
                date = DateTime.Now.Date,
                employee = new Employee
                {
                    id = 1,
                    user = new User { id = "user1", name = "John Doe", email = "john.doe@example.com" },
                    employer = new Employer { id = 1, user = new User { id = "emp1", name = "Employer1", email = "emp1@example.com" } }
                },
                position = new Position { id = 1, name = "Developer" }
            });
        }

        public List<Activity> GetActivitiesByUserId(string userId)
        {
            return _activities.Where(a => a.employee.user.id == userId).ToList();
        }

        public void CreateActivity(int position_id, string begintime, string endtime, string date, int employee_id)
        {
            _activities.Add(new Activity
            {
                id = _nextId++,
                begintime = TimeOnly.Parse(begintime),
                endtime = TimeOnly.Parse(endtime),
                date = DateTime.Parse(date),
                employee = new Employee { id = employee_id }, // Simplified for demo
                position = new Position { id = position_id } // Simplified for demo
            });
        }

        public void UpdateActivity(int id, string position, string begintime, string endtime, string date, string employer_name, string employer_email)
        {
            var activity = _activities.FirstOrDefault(a => a.id == id);
            if (activity != null)
            {
                activity.position.name = position;
                activity.begintime = TimeOnly.Parse(begintime);
                activity.endtime = TimeOnly.Parse(endtime);
                activity.date = DateTime.Parse(date);
                activity.employee.employer.user.name = employer_name;
                activity.employee.employer.user.email = employer_email;
            }
        }
    }
}
