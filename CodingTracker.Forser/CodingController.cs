using Microsoft.Data.Sqlite;

internal class CodingController
{
    string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

    internal void Post(Coding coding)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText = $"INSERT INTO Coding (StartDate, EndDate, TotalDuration) VALUES ('{coding.StartDate}', '{coding.EndDate}', '{coding.TotalDuration}')";
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
                tableCmd.CommandText = "SELECT * FROM Coding";

                using (var reader = tableCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            tableData.Add(new Coding 
                            { 
                                    Id = reader.GetInt32(0), 
                                    StartDate = reader.GetDateTime(1),
                                    EndDate = reader.GetDateTime(2), 
                                    TotalDuration = reader.GetDouble(3)
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
        TableVisulation.ShowTable(tableData);
    }

    internal Coding GetById(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();

                tableCmd.CommandText = $"SELECT * FROM Coding WHERE Id = '{id}'";

                using (var reader = tableCmd.ExecuteReader())
                {
                    Coding coding = new Coding();
                    if(reader.HasRows)
                    {
                        reader.Read();
                        coding.Id = reader.GetInt32(0);
                        coding.StartDate = reader.GetDateTime(1);
                        coding.EndDate = reader.GetDateTime(2);
                        coding.TotalDuration = reader.GetDouble(3);
                    }

                    Console.WriteLine("\n\n");

                    return coding;
                }
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
                tableCmd.CommandText = $"DELETE from Coding WHERE Id = '{id}'";
                tableCmd.ExecuteNonQuery();

                Console.WriteLine($"\n\nRecord with Id {id} was deleted.\n\n");
            }
        }
    }

    internal void Update(Coding coding)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            using (var tableCmd = connection.CreateCommand())
            {
                connection.Open();
                tableCmd.CommandText =
                    $@"UPDATE Coding SET
                        StartDate = '{coding.StartDate}',
                        EndDate = '{coding.EndDate}',
                        TotalDuration = '{coding.TotalDuration}'
                    WHERE
                        Id = {coding.Id}";

                tableCmd.ExecuteNonQuery();
            }
        }

        Console.WriteLine($"\n\nRecord with Id {coding.Id} was updated. \n\n");
    }
}