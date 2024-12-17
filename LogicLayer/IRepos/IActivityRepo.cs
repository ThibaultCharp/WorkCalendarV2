using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;

namespace LogicLayer.IRepos
{
    public interface IActivityRepo
    {
        List<Activity> GetActivitiesByUserEmail(string email);
        List<Activity> GetActivitiesByEmployerEmail(string email);
        void CreateActivity(int position_id, string begintime, string endtime, string date, int employee_id);
        void UpdateActivity(int id, int position_id, string begintime, string endtime, string date);
    }
}
