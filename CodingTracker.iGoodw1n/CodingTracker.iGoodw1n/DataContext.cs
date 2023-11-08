using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker.iGoodw1n;

public class DataContext
{
    private readonly string _connectionString;
    public DataContext(string connectionString)
    {
        _connectionString = connectionString;
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"CREATE TABLE IF NOT EXISTS coding_tracker (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    language TEXT NOT NULL,
                    start TEXT NOT NULL,
                    end TEXT NOT NULL
            );";

        command.ExecuteNonQuery();
    }

    public List<CodingSession>? GetAllRecords()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT * FROM coding_tracker;";

        using var reader = command.ExecuteReader();

        if (!reader.HasRows) return null;

        var records = new List<CodingSession>();
        while (reader.Read())
        {
            records.Add(CreateNext(reader));
        }

        return records;
    }

    public void CreateRecord(CodingSession record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"INSERT INTO coding_tracker (language, start, end)
                  VALUES ('{record.Language}', '{record.Start:o}', '{record.End:o}');";

        command.ExecuteNonQuery();
    }

    public CodingSession? GetRecord(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            @$"SELECT * FROM coding_tracker WHERE id = {id};";

        using var reader = command.ExecuteReader();
        reader.Read();

        if (!reader.HasRows) return null;

        return CreateNext(reader);
    }

    public void DeleteRecord(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"DELETE FROM coding_tracker WHERE id = {id};";

        command.ExecuteNonQuery();
    }

    public void UpdateRecord(CodingSession record)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        using var command = connection.CreateCommand();

        command.CommandText =
            @$"
            UPDATE coding_tracker
            SET language = '{record.Language}',
                start = '{record.Start:o}',
                end = '{record.End:o}'
            WHERE id = {record.Id};";

        command.ExecuteNonQuery();
    }

    private CodingSession CreateNext(SqliteDataReader reader)
    {
        return new CodingSession
        {
            Id = reader.GetInt32(0),
            Language = reader.GetString(1),
            Start = DateTime.ParseExact(reader.GetString(2), "o", CultureInfo.InvariantCulture),
            End = DateTime.ParseExact(reader.GetString(3), "o", CultureInfo.InvariantCulture)
        };
    }
}
