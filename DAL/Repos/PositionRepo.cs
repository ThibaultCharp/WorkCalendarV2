using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entities;
using LogicLayer.Entitys;
using LogicLayer.IRepos;
using MySql.Data.MySqlClient;

namespace DAL.Repos
{
    public class PositionRepo : IPositionRepo
    {
        dbConnection dbConnection = new dbConnection();


        public List<Position> GetAllPositions()
        {
            List<Position> posititions = new List<Position>();

            string query = "SELECT * FROM positions";

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
                                Position position = new Position();
                                position.id = Convert.ToInt32(reader["id"]);
                                position.name = reader["name"].ToString();

                                posititions.Add(position);
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

            return posititions;
        }

        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            string query = "SELECT * FROM roles";

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
                                Role role = new Role();
                                role.id = Convert.ToInt32(reader["id"]);
                                role.name = reader["name"].ToString();

                                roles.Add(role);
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

            return roles;
        }

    }
}