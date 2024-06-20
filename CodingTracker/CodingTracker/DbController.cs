using System.Data.SQLite;
using System.Configuration;
using Dapper;
using System;
using System.Data.SqlClient;
namespace CodingTracker
{
    public class DbController
    {
        private readonly string _connectionString;
        public SQLiteConnection SQLiteConnection { get; private set; }
        public DbController()
        {
            _connectionString = ConfigurationManager.AppSettings["connectionString"];
            SQLiteConnection = new SQLiteConnection("Data Source=CodingSessions.db;");
            if(!File.Exists("./CodingSessions.db"))
            {

            }
            
        }
    }
}
