using Microsoft.Data.Sqlite;

namespace CodingTracker.Crud
{
    public static class DbManagement {


        private static string dbAdress = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public static void CreateTable()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =

                    @"CREATE TABLE IF NOT EXISTS time_tracking (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    startTime TEXT NOT NULL,
                    endTime TEXT NOT NULL,
                    duration TEXT NOT NULL)";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection(dbAdress);
        }
    }
}
