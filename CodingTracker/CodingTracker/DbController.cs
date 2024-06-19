using System.Data.SQLite;
using System.Configuration;
using Dapper;
using System;
namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;
        public DbController()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"];
        }
        public void ExecuteQuery(string query)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(query);
                connection.Close();
            }
        }
    }
}
