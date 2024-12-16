using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.IRepos;
using MySql.Data.MySqlClient;
using LogicLayer.Entitys;
using System.Collections;

namespace DAL.Repos
{
    public class UserRepo : IUserRepo
    {
        dbConnection dbConnection = new dbConnection();


        public void LinkUser(string loggedInUserEmail, string targetUserEmail)
        {
            string query = @"
                UPDATE employees
                SET employer_id = (
                    SELECT e.id
                    FROM employers e
                    INNER JOIN users u ON e.user_id = u.id
                    WHERE u.email = @EmployerEmail
                )
                WHERE user_id = (
                    SELECT id
                    FROM users
                    WHERE email = @EmployeeEmail
                );

                UPDATE users
                SET role_id = 3
                WHERE email = @EmployeeEmail
                AND role_id = 4;
            ";
                

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@EmployerEmail", loggedInUserEmail);
                        command.Parameters.AddWithValue("@EmployeeEmail", targetUserEmail);

                        command.ExecuteNonQuery();
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
        }


        public void ChangeUserRole(string email, int roleId, bool makeEmployer)
        {
            string query = "UPDATE users SET role_id = @RoleId WHERE email = @Email";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@RoleId", roleId);

                        command.ExecuteNonQuery();
                    }

                    if (makeEmployer)
                    {
                        string employerQuery = "INSERT INTO employers (user_id) SELECT id FROM users WHERE email = @Email";

                        using (var employerCommand = new MySqlCommand(employerQuery, dbConnection.connection))
                        {
                            employerCommand.Parameters.AddWithValue("@Email", email);

                            employerCommand.ExecuteNonQuery();
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
        }


        public bool UserExists(int userId)
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
                    using (var userCommand = new MySqlCommand(userQuery, dbConnection.connection))
                    {
                        userCommand.Parameters.AddWithValue("@id", user.id);
                        userCommand.Parameters.AddWithValue("@name", user.name);
                        userCommand.Parameters.AddWithValue("@email", user.email);

                        userCommand.ExecuteNonQuery();
                    }

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


        public List<User> GetAllEmployeesWithoutEmployer(string Input)
        {
            List<User> users = new List<User>();
            string query = @"
                SELECT users.id, users.name, users.email
                FROM users
                JOIN employees ON users.id = employees.user_id
                WHERE employees.employer_id IS NULL
                  AND (users.name LIKE @Input OR users.email LIKE @Input);
            ";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@Input", $"%{Input}%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    name = reader["name"].ToString(),
                                    email = reader["email"].ToString()
                                };

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

        public List<User> GetAllUsersWithCorrespondingRoles(string Input)
        {
            List<User> users = new List<User>();
            string query = @"
        SELECT 
            u.id AS UserId,
            u.name AS UserName,
            u.email AS UserEmail,      
            r.id AS RoleId,
            r.name AS RoleName
        FROM 
            users u
        LEFT JOIN 
            roles r
        ON 
            u.role_id = r.id
        WHERE 
            u.name LIKE @Input OR r.name LIKE @Input OR u.email LIKE @Input;";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        command.Parameters.AddWithValue("@Input", $"%{Input}%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    id = Convert.ToInt32(reader["UserId"]),
                                    name = reader["UserName"].ToString(),
                                    email = reader["UserEmail"].ToString(),
                                    role = new LogicLayer.Entities.Role
                                    {
                                        id = Convert.ToInt32(reader["RoleId"]),
                                        name = reader["RoleName"].ToString()
                                    }
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching users with roles.", ex);
            }
            finally
            {
                dbConnection.CloseConnection();
            }
            return users;
        }
    }
}