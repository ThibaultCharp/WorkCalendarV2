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

        public List<Employee> GetAllemployeesPerEmployer()
        {
            List<Employee> employees = new List<Employee>();

            string query = "SELECT employees.*, employers.user_id AS employer_user_id, users.name AS employee_name, users.email AS employee_email, emp_users.name AS employer_name, emp_users.email AS employer_email " +
                            "FROM employees " +
                            "JOIN employers ON employees.employer_id = employers.id " +
                            "JOIN users ON employees.user_id = users.id " +
                            "JOIN users emp_users ON employers.user_id = emp_users.id " +
                            "WHERE employers.user_id = 'auth0|66e98737bc35834952224650' " +
                            "ORDER BY users.name ASC;";


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
                                Employee employee = new Employee();
                                employee.id = Convert.ToInt32(reader["id"]);

                                // Map the employee's user
                                employee.user = new User();
                                employee.user.id = reader["user_id"].ToString();
                                employee.user.name = reader["employee_name"].ToString();
                                employee.user.email = reader["employee_email"].ToString();

                                // Map the employer's user
                                employee.employer = new Employer();
                                employee.employer.id = Convert.ToInt32(reader["employer_id"]);

                                employee.employer.user = new User();
                                employee.employer.user.id = reader["employer_user_id"].ToString();
                                employee.employer.user.name = reader["employer_name"].ToString();
                                employee.employer.user.email = reader["employer_email"].ToString();

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                dbConnection.CloseConnection();
            }

            return employees;
        }




    }
}
