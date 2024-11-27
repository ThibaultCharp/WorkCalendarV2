using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;
using MySql.Data.MySqlClient;
using LogicLayer.IRepos;
using LogicLayer.Entities;

namespace DAL.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly dbConnection _dbConnection;

        public AuthRepo()
        {
            _dbConnection = new dbConnection();
        }

        public bool UserExists(string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
            try
            {
                if (_dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, _dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        int userCount = Convert.ToInt32(command.ExecuteScalar());
                        return userCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                _dbConnection.CloseConnection();
            }
            return false;
        }

        public void AddUser(User user)
        {
            string userQuery = @"
                INSERT INTO Users (name, email, password, role_Id)
                VALUES (@Name, @Email, @Password, 3)";
            string employeeQuery = @"
                INSERT INTO Employees (user_id)
                VALUES ((SELECT id FROM Users WHERE email = @Email))";

            try
            {
                if (_dbConnection.OpenConnection())
                {
                    using (var transaction = _dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            using (var userCommand = new MySqlCommand(userQuery, _dbConnection.connection, transaction))
                            {
                                userCommand.Parameters.AddWithValue("@Name", user.name);
                                userCommand.Parameters.AddWithValue("@Email", user.email);
                                userCommand.Parameters.AddWithValue("@Password", user.password);
                                userCommand.ExecuteNonQuery();
                            }

                            using (var employeeCommand = new MySqlCommand(employeeQuery, _dbConnection.connection, transaction))
                            {
                                employeeCommand.Parameters.AddWithValue("@Email", user.email);
                                employeeCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
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
                _dbConnection.CloseConnection();
            }
        }

        public User GetUserByEmail(string email)
        {
            string query = @"
        SELECT u.*, r.id AS RoleId, r.name AS RoleName 
        FROM users u
        JOIN roles r ON u.role_id = r.id  -- Correct column name for foreign key
        WHERE u.email = @Email";  // Note the case sensitivity for 'email'

            try
            {
                if (_dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, _dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Create and return the User object with Role
                                return new User
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    name = reader["name"].ToString(),
                                    email = reader["email"].ToString(),
                                    password = reader["password"].ToString(),
                                    role = new Role
                                    {
                                        id = Convert.ToInt32(reader["RoleId"]),
                                        name = reader["RoleName"].ToString()
                                    }
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (if necessary)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                _dbConnection.CloseConnection();
            }
            return null; // Return null if no user is found
        }
    }
}