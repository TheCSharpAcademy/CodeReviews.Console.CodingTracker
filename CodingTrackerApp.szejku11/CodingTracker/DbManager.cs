using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;

namespace CodingTracker
{
    internal static class DbManager
    {
        private static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        private static SqliteConnection connection = new SqliteConnection(connectionString);

        internal static void InitializeConnection()
        {
            using (connection)
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_tracker (
	                  id	            INTEGER NOT NULL UNIQUE,
	                  start_time	    TEXT NOT NULL,
	                  end_time	        TEXT NOT NULL,
                      duration          TEXT NOT NULL,
	                  PRIMARY   KEY(id AUTOINCREMENT)
                      );";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static void InsertRow(bool stopwatch)
        {
            string? startTime = "";
            string? endTime = "";

            if (stopwatch)
            {
                (startTime, endTime) = UserInterface.GetInputStopwatch();
            }
            else
            {
                (startTime, endTime) = UserInterface.GetInput();
            }

            if (!string.IsNullOrEmpty(startTime))
            {
                string duration = Validation.CodingSessionDuration(startTime, endTime);

                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                        @$"INSERT INTO [coding_tracker]
                       ([start_time],[end_time],[duration])   
                       VALUES
                       ('{startTime}','{endTime}','{duration}');";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        internal static List<List<object>> AllDataToDisplay()
        {
            List<List<object>> data = new();

            using (connection)
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"SELECT * FROM coding_tracker";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new List<object>
                            {
                                reader["id"].ToString(),
                                reader["start_time"].ToString(),
                                reader["end_time"].ToString(),
                                reader["duration"].ToString()
                            });
                    }
                }
                connection.Close();
            }

            return data;

        }
        internal static List<CodingModel> AllDataToList()
        {
            List<CodingModel> data = new();

            using (connection)
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"SELECT * FROM coding_tracker";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        data.Add(
                            new CodingModel
                            {
                                Id = (int)(long)reader["id"],
                                StartTime = DateTime.ParseExact(reader["start_time"].ToString(), "dd-MM-yyyy HH:mm:ss",
                                            CultureInfo.InvariantCulture, DateTimeStyles.None),
                                EndTime = DateTime.ParseExact(reader["end_time"].ToString(), "dd-MM-yyyy HH:mm:ss",
                                          CultureInfo.InvariantCulture, DateTimeStyles.None),
                                Duration = TimeSpan.Parse(reader["duration"].ToString())
                            });
                    }
                }

                connection.Close();

                return data;
            }
        }
        internal static void UpdateRow()
        {
            (int id, string startTime, string endTime) = UserInterface.GetUpdateInfo(false);

            if (id > 0 && !string.IsNullOrEmpty(startTime))
            {
                string duration = Validation.CodingSessionDuration(startTime, endTime);

                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        @$"UPDATE[coding_tracker]
                       SET[start_time] = '{startTime}'
                        ,[end_time] = '{endTime}' 
                        ,[duration] = '{duration}'
                         WHERE [id] = {id};";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        internal static void DeleteRow()
        {
            (int id, string _, string _) = UserInterface.GetUpdateInfo(true);

            if (id > 0)
            {
                using (connection)
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        @$"DELETE FROM [coding_tracker]
                       WHERE [Id] = {id};";

                    tableCmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
