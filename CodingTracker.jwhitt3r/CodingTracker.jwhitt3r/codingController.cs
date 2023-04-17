using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.jwhitt3r
{
    /// <summary>
    /// The CodingController is the class responsible for interacting with the database
    /// </summary>
    internal class CodingController
    {
        /// <summary>
        /// Connection string that takes the parameter from the configuration file
        /// </summary>
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        /// <summary>
        /// Deletes a record based on the ID provided by the user
        /// </summary>
        /// <param name="id">The primary key used within the database</param>
        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand()) 
                {
                    connection.Open();

                    tableCmd.CommandText = $"DELETE from coding WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();

                    Console.WriteLine($"Record with the id of {id} has been deleted...");
                }
            }
        }

        /// <summary>
        /// Get retrieves all the records found within the database table 'coding'
        /// </summary>
        internal void Get()
        {
            List<CodingSession> tableData = new List<CodingSession>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                    new CodingSession
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = reader.GetString(1),
                                        Duration = reader.GetString(2),
                                        StartTime = reader.GetString(3),
                                        EndTime = reader.GetString(4),
                            });
                            }
                        } else
                        {
                            Console.WriteLine("\n\nNo rows founds.\n\n");
                        }
                    }

                }
                Console.WriteLine("\n\n");
            }
            TableVisualisation.ShowTable(tableData);
        }

        /// <summary>
        /// GetById finds the record from the Database by the primary key id
        /// </summary>
        /// <param name="id">An integer to represent the record in the database</param>
        /// <returns>Returns a CodingSession object</returns>
        internal CodingSession GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();

                    tableCmd.CommandText = $"SELECT * FROM coding Where Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        CodingSession coding = new();
                        if (reader.HasRows)
                        {
                            reader.Read();
                            coding.Id = reader.GetInt32(0);
                            coding.Date = reader.GetString(1);
                            coding.Duration = reader.GetString(2);
                            coding.StartTime = reader.GetString(3);
                            coding.EndTime = reader.GetString(4);
                        }

                        Console.WriteLine("\n\n");

                        return coding;
                    }
                }
            }
        }

        /// <summary>
        /// Post submits the data into the database, this holds the date and duration of the session
        /// </summary>
        /// <param name="coding">passes a coding object to be posted to the database</param>
        internal void Post(CodingSession coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration, starttime, endtime) VALUES ('{coding.Date}', '{coding.Duration}', '{coding.StartTime}', '{coding.EndTime}')";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update finds the the record with the ID and updates the chosen field based on the updated elements.
        /// This will submit a new object regardless of the field not being changed
        /// </summary>
        /// <param name="coding">passes a coding object to be posted to the database</param>
        internal void Update(CodingSession coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText =
                    $@"UPDATE coding SET 
                          Date = '{coding.Date}', 
                          Duration = '{coding.Duration}',
                          StartTime = '{coding.StartTime}' ,
                          EndTime = '{coding.EndTime}' 
                          WHERE 
                          Id = {coding.Id}
                    ";

                    tableCmd.ExecuteNonQuery();
                }
            }

            Console.WriteLine($"\n\nRecord with Id {coding.Id} was updated. \n\n");
        }
    }
}