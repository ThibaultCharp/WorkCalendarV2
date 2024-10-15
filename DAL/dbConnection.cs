using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class dbConnection
    {
        public MySqlConnection connection;



        public dbConnection()
        {
            Initialize();
        }


        private void Initialize()
        {

            string connectionString;
            connectionString = "Server = 127.0.0.1; Uid = root; Database = db_workcalendarV2; Password = root;";

            connection = new MySqlConnection(connectionString);
        }


        // Open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


        // Close connection to database
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }
    }
}
