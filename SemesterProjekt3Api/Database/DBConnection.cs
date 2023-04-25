﻿using System.Data.Common;
using System.Data.SqlClient;

namespace SemesterProjekt3Api.Database
{
    public class DBConnection
    {
        SqlConnection con;
        static DBConnection? dbConnection;
        private string? connectionString;


        private DBConnection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            connectionString = configuration.GetConnectionString("VestbjergBio");
            con = new SqlConnection(connectionString);
        }


        public static DBConnection GetInstance()
        {
            if (dbConnection == null)
            {
                dbConnection = new DBConnection();
            }

            return dbConnection;
        }

        //Opens the sql connection an returns it
        public SqlConnection GetConnection()
        {
            OpenConnection();
            return con;
        }

        public void OpenConnection()
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        //Skal måske ikke være med?
        /**
         * public void ExecuteQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
         */





    }
}
