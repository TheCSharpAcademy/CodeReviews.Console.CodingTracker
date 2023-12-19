using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Chad1082.Data
{
    static class Database
    {
        internal static readonly string connString = ConfigurationManager.AppSettings.Get("connString");
        internal static readonly string DBLocation = ConfigurationManager.AppSettings.Get("DBLocation");
        public static void SetupDB()
        {
            if (!DatabaseExists())
            {
                Console.WriteLine("Database does not exist, creating...");
                CreateDatabase();
            }
        }
        private static bool DatabaseExists()
        {
            bool doesExist = false;

            doesExist = File.Exists(DBLocation);
            return doesExist;
        }
        private static void CreateDatabase()
        {
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"CREATE TABLE ""CodingSessions"" (
	                                    ""ID""	INTEGER NOT NULL UNIQUE,
	                                    ""StartDateTime""	TEXT NOT NULL,
	                                    ""EndDateTime""	TEXT NOT NULL,
	                                    PRIMARY KEY(""ID"" AUTOINCREMENT)
                                    );";

                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
