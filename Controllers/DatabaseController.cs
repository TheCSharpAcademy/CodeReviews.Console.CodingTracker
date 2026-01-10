using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Data;
using System.Text;

internal class DatabaseController
{
    private static string _tableName = "Sessions";
    private static readonly IConfiguration _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
    private static string connectionString = _configuration.GetConnectionString("Default");

    public static void StartConnection()
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"CREATE TABLE IF NOT EXISTS {_tableName}(
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Name TEXT,
                                    Date TEXT,
                                    Start TEXT,
                                    End TEXT,
                                    Duration TEXT
                                    );";

                tableCmd.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                AnsiConsole.Write("An error occured! Could not create table.");
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Return"));
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static void AddRecord(CodingSession record)
    {
        using(var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                var tableCmd = $@"INSERT INTO {_tableName} (name, date, start, end, duration)
                             VALUES (@name, @date, @start, @end, @duration)";
                connection.Execute(tableCmd, new { name = record.RecordName, date = record.Date, start = record.StartTime, end = record.EndTime, duration = record.Duration });
            }
            catch (SqliteException)
            {
                AnsiConsole.Write("An error occured! Could not add record.");
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Return"));
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public static List<string> GetRecords(List<FilterCondition> filters)
    {
        var readerCmd = $"SELECT * FROM {_tableName} ";
        var filterCommands = new List<string>();
        var table = new DataTable();

        foreach (FilterCondition filter in filters)
        {
            filterCommands.Add($" {filter.Type} {filter.Relational} \"{filter.ComparedTo}\" ");
        }

        if (filters.Count > 0)
        {
            readerCmd += $"WHERE " + string.Join("AND", filterCommands) + ";";
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                var reader = connection.ExecuteReader(readerCmd);

                table.Load(reader);
            }
            catch (SqliteException)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write("An error occured! Could not retrieve records.");
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Return"));
            }
            finally
            {
                connection.Close();
            }
        }

        var allRows = new List<string>();

        foreach (DataRow rows in table.Rows)
        {
            StringBuilder output = new StringBuilder();

            foreach (DataColumn column in table.Columns)
            {
                output.AppendFormat("{0} ", rows[column]);
            }
            allRows.Add(output.ToString());
        }

        return allRows;
    }

    public static void RemoveRecord(CodingSession record)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();

                var removeCmd = @$"DELETE FROM {_tableName} WHERE
                          id = @id AND
                          name = @name AND
                          date = @date AND
                          start = @start AND
                          end = @end AND
                          duration = @duration;";

                connection.Execute(removeCmd, new { id = record.Id,
                                                    name = record.RecordName,
                                                    date = record.Date,
                                                    start = record.StartTime,
                                                    end = record.EndTime,
                                                    duration = record.Duration});

            }
            catch (SqliteException ex)
            {
                AnsiConsole.Clear();
                AnsiConsole.WriteLine("Something went wrong! Could not remove record! " + ex);
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Return"));
            }
            finally
            {
                connection.Close();
            }

        }
    }

    public static void EditRecord(CodingSession record, string valueToChange, string newValue)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();

                var editCmd = string.Empty;
                
                if(valueToChange == "start" || valueToChange == "end")
                {
                    editCmd = @$"Update {_tableName} SET {valueToChange} = @value, duration = @duration
                          WHERE id = @id AND
                          name = @name AND
                          date = @date AND
                          start = @start AND
                          end = @end";
                }
                else
                {
                    editCmd = @$"Update {_tableName} SET {valueToChange} = @value
                          WHERE id = @id AND
                          name = @name AND
                          date = @date AND
                          start = @start AND
                          end = @end AND
                          duration = @duration;";
                }

                connection.Execute(editCmd, new
                {
                    value = newValue,
                    id = record.Id,
                    name = record.RecordName,
                    date = record.Date,
                    start = record.StartTime,
                    end = record.EndTime,
                    duration = record.Duration
                });

            }
            catch (SqliteException ex)
            {
                AnsiConsole.Clear();
                AnsiConsole.WriteLine("Something went wrong! Could not edit record! " + ex);
                AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices("Return"));
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
