using LogicLayer.Entitys;
using MySql.Data.MySqlClient;
using DAL.Repos;
using LogicLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class ActivityRepo : IActivityRepo
    {
        dbConnection dbConnection = new dbConnection();
        public List<Activity> GetAllActivitiesPerEmployee()
        {
            List<Activity> activities = new List<Activity>();
            string query = "SELECT * FROM activities WHERE employee_id = 1";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Activity activity = new Activity();
                                activity.id = Convert.ToInt32(reader["id"]);
                                activity.title = reader["title"].ToString();
                                activity.date = reader.GetDateTime(reader.GetOrdinal("date"));
                                activity.begintime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("begintime")));
                                activity.endtime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("endtime")));
                                activity.employee = new Employee();
                                activity.employee.id = Convert.ToInt32(reader["employee_id"]);
                                activities.Add(activity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex);
            }

            finally
            {
                dbConnection.CloseConnection();
            }
            return activities;
        }
    }
}
