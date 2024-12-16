using LogicLayer.Entitys;
using LogicLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Classes
{
    public class ActivityService
    {
        private readonly IActivityRepo repository;

        public ActivityService(IActivityRepo activityRepo)
        {
            repository = activityRepo;
        }

        public List<Activity> GetActivitiesByUserEmail(string email, string role)
        {
            if (role == "Employer")
            {
                return repository.GetActivitiesByEmployerEmail(email);
            }
            else
            {
                return repository.GetActivitiesByUserEmail(email);
            }
        }

        public void CreateActivity (int position_id, string begintime, string endtime, string date, int employee_id) 
        {
            repository.CreateActivity(position_id, begintime, endtime, date, employee_id);
        }

        public void UpdateActivity(int id, string position, string begintime, string endtime, string date, string employer_name, string employer_email)
        {
            repository.UpdateActivity(id, position, begintime, endtime, date, employer_name, employer_email);
        }
    }
}