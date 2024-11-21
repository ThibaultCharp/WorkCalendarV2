using LogicLayer.Entitys;
using LogicLayer.IRepos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class EmployeeRepo : IEmployeeRepo
    {
        dbConnection dbConnection = new dbConnection();

        public List<Employee> GetAllemployeesPerEmployer(string email)
        {
            List<Employee> employees = new List<Employee>();

            string query = @"SELECT employees.*, employers.user_id AS employer_user_id, users.name AS employee_name, users.email AS employee_email, emp_users.name AS employer_name, emp_users.email AS employer_email 
        FROM employees 
        JOIN employers ON employees.employer_id = employers.id 
        JOIN users ON employees.user_id = users.id 
        JOIN users AS emp_users ON employers.user_id = emp_users.id 
        WHERE emp_users.email = @Email 
        ORDER BY users.name ASC;";

            try
            {
                if (dbConnection.OpenConnection())
                {
                    using (var command = new MySqlCommand(query, dbConnection.connection))
                    {
                        // Bind the email parameter
                        command.Parameters.AddWithValue("@Email", email);

                        // Log the query and parameter
                        Debug.WriteLine("Repo: Executing query: " + query);
                        Debug.WriteLine("Repo: With parameter: " + email);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Debug.WriteLine("Repo: Reading employee record");

                                Employee employee = new Employee
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    user = new User
                                    {
                                        id = Convert.ToInt32(reader["user_id"]),
                                        name = reader["employee_name"].ToString(),
                                        email = reader["employee_email"].ToString()
                                    },
                                    employer = new Employer
                                    {
                                        id = Convert.ToInt32(reader["employer_id"]),
                                        user = new User
                                        {
                                            id = Convert.ToInt32(reader["employer_user_id"]),
                                            name = reader["employer_name"].ToString(),
                                            email = reader["employer_email"].ToString()
                                        }
                                    }
                                };

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Repo: Error: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
                Console.WriteLine("Repo: Closed database connection");
            }

            return employees;
        }






    }
}
