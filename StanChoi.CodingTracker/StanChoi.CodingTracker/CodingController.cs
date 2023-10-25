using Microsoft.Data.Sqlite;

namespace StanChoi.CodingTracker
{
    internal class CodingController
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

        internal void Get()
        {
            List<CodingSession> tableData = new List<CodingSession>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding_session";


                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                new CodingSession
                                {
                                    Id = reader.GetInt32(0),
                                    StartTime = reader.GetString(1),
                                    EndTime = reader.GetString(2),
                                    Duration = reader.GetString(3)
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found.\n\n");
                        }
                    }
                }
                Console.WriteLine("\n\n");
            }

            TableVisualization.ShowTable(tableData);
        }

        internal CodingSession GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText = $"SELECT * FROM coding_session WHERE Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        CodingSession codingSession = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            codingSession.Id = reader.GetInt32(0);
                            codingSession.StartTime = reader.GetString(1);
                            codingSession.EndTime = reader.GetString(2);
                            codingSession.Duration = reader.GetString(3);
                        }

                        Console.WriteLine("\n\n");

                        return codingSession;
                    }
                }
            }
        }

        internal void Post(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding_session (starttime, endtime, duration) VALUES ('{codingSession.StartTime}', '{codingSession.EndTime}', '{codingSession.Duration}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE FROM coding_session WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"\n\nRecord with Id {id} was deleted. \n\n");
                }
            }
        }

        internal void Update(CodingSession codingSession)
        { 
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText =
                        $@"UPDATE coding_session SET
                            StartTime = '{codingSession.StartTime}',
                            EndTime = '{codingSession.EndTime}',
                            Duration = '{codingSession.Duration}'
                          WHERE
                            Id = {codingSession.Id}
                          ";

                    tableCmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"\n\nRecord with Id {codingSession.Id} was updated. \n\n");
        }
    }
}
