using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.SamGannon
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

        internal void Post(Coding coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration VALUES (' {coding.Date}', '{coding.Duration }";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void Get()
        {
            List<Coding> tableData = new List<Coding>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                    new Coding
                                    {
                                        Id = reader.GetInt32(0),
                                        Date = reader.GetString(1),
                                        Duration = reader.GetString(2)
                                    });
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n\nNo rows found");
                        }

                        TableVisualisation.ShowTable(tableData);
                    }
                }
            }
        }
    }
}