using Microsoft.Data.Sqlite;

namespace CodingTracker.MartinL_no.DAL;

internal class CodingSessionRepository
{
    private readonly string ConnString;
    private readonly string DbName;

	internal CodingSessionRepository(string connString, string dbName)
	{
        ConnString = connString;
        DbName = dbName;
        CreateTable();
    }

    private void CreateTable()
    {
        using (var connection = new SqliteConnection($"{ConnString}{DbName}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = """
                CREATE TABLE IF NOT EXISTS CodingSession (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT NOT NULL
                );
                """;
            command.ExecuteNonQuery();
        }
    }
}