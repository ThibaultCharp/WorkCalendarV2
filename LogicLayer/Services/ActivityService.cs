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

        public List<Activity> GetAllActivitiesPerEmployee() 
        {
            return repository.GetAllActivitiesPerEmployee();
        }

        public void CreateActivity (string position, string begintime, string endtime, string date, int employee_id) 
        {
            repository.CreateActivity(position, begintime, endtime, date, employee_id);
        }

        public void EditActivity (string position, string begintime, string endtime, string date)
        {

        }

    }
}
