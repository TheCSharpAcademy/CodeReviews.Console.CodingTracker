using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker.StevieTV;

internal class CodingController
{
    private string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void Get()
    {
        List<CodingSession> tableData = new List<CodingSession>();

        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = "SELECT * FROM coding_sessions";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                Date = reader.GetString(1),
                                Duration = reader.GetString(2)
                            });
                        }
                    }
                    else Console.Write("\nNo Entries Found\n");
                }
            }

            Console.WriteLine("\n\n");
        }

        TableVisualisation.ShowTable(tableData);
    }

    internal void Post(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"INSERT INTO coding_sessions (Date, Duration) VALUES ('{codingSession.Date}', '{codingSession.Duration}')";
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
                tableCmd.CommandText = $"DELETE FROM coding_sessions WHERE Id = '{id}'";
                tableCmd.ExecuteNonQuery();
                
            }
        }
        Console.WriteLine($"Record with ID {id} was deleted");
    }
    
    public void Update(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"UPDATE coding_sessions SET Date = '{codingSession.Date}', Duration = '{codingSession.Duration}' WHERE Id = '{codingSession.Id}'";
                tableCmd.ExecuteNonQuery();
                
            }
        }    
        Console.WriteLine($"Record with ID {codingSession.Id} was updated");
    }

    internal CodingSession GetById(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"SELECT * FROM coding_sessions WHERE Id = '{id}'";

                using (var reader = tableCmd.ExecuteReader())
                {
                    CodingSession codingSession = new();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        codingSession.Id = reader.GetInt32(0);
                        codingSession.Date = reader.GetString(1);
                        codingSession.Duration = reader.GetString(2);
                    }

                    return codingSession;
                }
            }
        }
        
    }
}