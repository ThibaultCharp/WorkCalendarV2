using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;

namespace DAL.Repos
{
    public class EmployerRepo
    {
        dbConnection connection = new dbConnection();

        public List<Employer> GetAllEmployers(string user_id)
        {
            var employers = new List<Employer>();
            string query = "SELECT * FROM `employers` WHERE `user_id` = @user_id";

            try
            {
                if (connection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, connection.connection))
                    {
                        command.Parameters.AddWithValue("@user_id", user_id); // Pass user_id to query

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Employer employer = new Employer();
                                employer.id = Convert.ToInt32(reader["id"]);
                                employer.user = new User();
                                employer.user.id = reader["user_id"].ToString();

                                employers.Add(employer);
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
                connection.CloseConnection();
            }

            return employers;
        }

        public void CreateNewEmployer(Employer employer)
        {
            string query = "INSERT INTO `employers` (id, `user_id`) VALUES (NULL, @User_Id);";
            try
            {
                if (connection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, connection.connection))
                    {
                        command.Parameters.AddWithValue("@User_Id", employer.id);
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
                connection.CloseConnection();
            }
        }
    }
}
