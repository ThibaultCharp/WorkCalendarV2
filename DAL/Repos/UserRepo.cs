using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.IRepos;
using MySql.Data.MySqlClient;
using LogicLayer.Entitys;

namespace DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        dbConnection dbConnection = new dbConnection();


        public bool UserExists(string userId)
        {
            string query = "SELECT COUNT(1) FROM users WHERE id = @userId";
            bool exists = false;

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
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

            return exists;
        }


        public void CreateUser(User user)
        {
            string userQuery = "INSERT INTO `users` (`id`, `name`, `email`) VALUES (@id, @name, @email);";
            string employeeQuery = "INSERT INTO `employees` (`user_id`, `employer_id`) VALUES (@user_id, @employer_id);";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    // Insert user
                    using (var userCommand = new MySqlCommand(userQuery, dbConnection.connection))
                    {
                        userCommand.Parameters.AddWithValue("@id", user.id);
                        userCommand.Parameters.AddWithValue("@name", user.name);
                        userCommand.Parameters.AddWithValue("@email", user.email);

                        userCommand.ExecuteNonQuery();
                    }

                    // Insert employee linked to the created user
                    using (var employeeCommand = new MySqlCommand(employeeQuery, dbConnection.connection))
                    {
                        employeeCommand.Parameters.AddWithValue("@user_id", user.id);
                        employeeCommand.Parameters.AddWithValue("@employer_id", null);
                        
                        employeeCommand.ExecuteNonQuery();
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


        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string query = "SELECT users.id FROM users";

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
                                User user = new User();
                                user.id = reader["id"].ToString();

                                users.Add(user);
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

            return users;
        }

    }
}
