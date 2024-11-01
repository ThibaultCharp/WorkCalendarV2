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
        List<Activity> GetAllActivitiesPerEmployee();
        void CreateActivity(string position, string begintime, string endtime, string date, int employee_id);
        void UpdateActivity(int id, string position, string begintime, string endtime, string date, string employer_name, string employer_email);
    }
}
