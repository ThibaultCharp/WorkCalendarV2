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

        public void UpdateActivity(int id, int position_id, string begintime, string endtime, string date)
        {
            repository.UpdateActivity(id, position_id, begintime, endtime, date);
        }
    }
}