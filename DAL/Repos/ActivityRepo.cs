using LogicLayer.Entitys;
using MySql.Data.MySqlClient;
using DAL.Repos;
using LogicLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mysqlx.Crud;

namespace DAL.Repos
{
    public class ActivityRepo : IActivityRepo
    {
        dbConnection dbConnection = new dbConnection();

        public void CreateActivity(int position_id, string begintime, string endtime, string date, int employee_id)
        {
            string query = "INSERT INTO `activities` (`begintime`, `endtime`, `date`, `employee_id`, `position_id`) " +
                "VALUES (@bigintime, @endtime, @date, @employee_id, @position_id);";
            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@position_id", position_id);
                        command.Parameters.AddWithValue("@bigintime", begintime);
                        command.Parameters.AddWithValue("@endtime", endtime);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@employee_id", employee_id);

                        command.ExecuteNonQuery();
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
        }

        public void UpdateActivity(int id, string position, string begintime, string endtime, string date, string employer_name, string employer_email)
        {
            string query =  "UPDATE `activities` SET `title` = @title, `begintime` = @begintime, `endtime` = @endtime, `date` = @date WHERE `id` = @id;";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        // Bind parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@title", position);
                        command.Parameters.AddWithValue("@begintime", begintime);
                        command.Parameters.AddWithValue("@endtime", endtime);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating the activity: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
        }

        public List<Activity> GetActivitiesByUserId(string userId)
        {
            List<Activity> activities = new List<Activity>();
            string query = @"
                    SELECT 
                        activities.*,
                        employees.id AS employee_id, 
                        users.id AS user_id, users.name AS user_name, users.email AS user_email, 
                        employers.id AS employer_id, employer_user.id AS employer_user_id, employer_user.name AS employer_user_name, employer_user.email AS employer_user_email,
                        positions.name AS position_name
                    FROM activities
                    JOIN employees ON activities.employee_id = employees.id
                    JOIN users ON employees.user_id = users.id
                    JOIN employers ON employees.employer_id = employers.id
                    JOIN users AS employer_user ON employers.user_id = employer_user.id
                    JOIN positions ON activities.position_id = positions.id
                    WHERE employees.user_id = @userId
                    ORDER BY activities.date ASC";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        // Replace the placeholder @userId with the actual user ID from the parameter
                        command.Parameters.AddWithValue("@userId", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Activity activity = new Activity();
                                activity.id = Convert.ToInt32(reader["id"]);
                                activity.date = reader.GetDateTime(reader.GetOrdinal("date"));
                                activity.begintime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("begintime")));
                                activity.endtime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("endtime")));

                                // Employee info
                                activity.employee = new Employee();
                                activity.employee.id = Convert.ToInt32(reader["employee_id"]);

                                // Position info
                                activity.position = new Position();
                                activity.position.name = reader["position_name"].ToString();
                                activity.position.id = Convert.ToInt32(reader["position_id"]);

                                // Employee's user info
                                activity.employee.user = new User();
                                activity.employee.user.id = reader["user_id"].ToString();
                                activity.employee.user.name = reader["user_name"].ToString();
                                activity.employee.user.email = reader["user_email"].ToString();

                                // Employer info
                                activity.employee.employer = new Employer();
                                activity.employee.employer.id = Convert.ToInt32(reader["employer_id"]);

                                // Employer's user info
                                activity.employee.employer.user = new User();
                                activity.employee.employer.user.id = reader["employer_user_id"].ToString();
                                activity.employee.employer.user.name = reader["employer_user_name"].ToString();
                                activity.employee.employer.user.email = reader["employer_user_email"].ToString();

                                activities.Add(activity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            return activities;
        }


    }
}
