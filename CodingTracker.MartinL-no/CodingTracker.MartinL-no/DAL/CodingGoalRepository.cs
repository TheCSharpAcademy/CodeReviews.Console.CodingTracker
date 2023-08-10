using Microsoft.Data.Sqlite;
using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.DAL;

internal class CodingGoalRepository : ICodingGoalRepository
{
    private readonly string? _connString;
    private readonly string? _dbPath;

    public CodingGoalRepository()
    {
        _connString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnString");
        _dbPath = System.Configuration.ConfigurationManager.AppSettings.Get("DbPath");
        CreateTable();
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection($"{_connString}{_dbPath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS CodingGoal (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL,
                    Hours INT NOT NULL
                );
                """;
            command.ExecuteNonQuery();
        }
    }

    public List<CodingGoal> GetCodingGoals()
    {
        using (var connection = new SqliteConnection($"{_connString}{_dbPath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                SELECT Id, StartTime, EndTime, Hours
                FROM CodingGoal;
                """;

            using (var reader = command.ExecuteReader())
            {
                var codingGoals = new List<CodingGoal>();

                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var startTime = DateTime.Parse(reader.GetString(1));
                    var endTime = DateTime.Parse(reader.GetString(2));
                    var hours = reader.GetInt32(0);


                    codingGoals.Add(new CodingGoal(id, startTime, endTime, hours));
                }

                return codingGoals;
            }
        }
    }

    public CodingGoal GetCodingGoal(int id)
    {
        using (var connection = new SqliteConnection($"{_connString}{_dbPath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
            SELECT StartTime, EndTime, Hours
            FROM CodingGoal
            WHERE Id = $id;
            """;

            command.Parameters.AddWithValue("$id", id);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var startTime = DateTime.Parse(reader.GetString(0));
                    var endTime = DateTime.Parse(reader.GetString(1));
                    var hours = reader.GetInt32(2);
                    return new CodingGoal(id, startTime, endTime, hours);
                }
            }
            return null;
        }
    }

    public bool InsertCodingGoal(CodingGoal codingGoal)
    {
        using (var connection = new SqliteConnection($"{_connString}{_dbPath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                INSERT INTO CodingGoal (StartTime, EndTime, Hours)
                VALUES ($startTime, $endTime, $hours)
                """;

            command.Parameters.AddWithValue("$startTime", ToSqLiteDateFormat(codingGoal.StartTime));
            command.Parameters.AddWithValue("$endTime", ToSqLiteDateFormat(codingGoal.EndTime));
            command.Parameters.AddWithValue("$hours", codingGoal.Hours);


            return command.ExecuteNonQuery() != 0;
        }
    }

    private string ToSqLiteDateFormat(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
    }
}
