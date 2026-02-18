using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Dapper;
using CodingTracker.Models;

namespace CodingTracker.Data;

internal class Database
{
  private readonly string _connectionString;

  public Database()
  {
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    _connectionString = config.GetConnectionString("Default");
  }

  public SqliteConnection GetConnection()
  {
    return new SqliteConnection(_connectionString);
  }

  public void Initialize()
  {
    using var connection = GetConnection();

    string sql = @"
    CREATE TABLE IF NOT EXISTS CodingSessions (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        StartTime TEXT NOT NULL,
        EndTime TEXT NOT NULL,
        Description TEXT
    );";

    connection.Execute(sql);
  }


  public List<CodingSession> GetAll()
  {
    using var connection = GetConnection();

    string sql = "SELECT * FROM CodingSessions";

    return connection.Query<CodingSession>(sql).ToList();
  }

  public void Insert(CodingSession session)
  {
    using var connection = GetConnection();

    string sql = @"
INSERT INTO CodingSessions (StartTime, EndTime, Description)
VALUES (@StartTime, @EndTime, @Description)";

    connection.Execute(sql, session);
  }
  public void Delete(int id)
  {
    using var connection = GetConnection();
    string sql = "DELETE FROM CodingSessions WHERE Id = @Id";
    connection.Execute(sql, new { Id = id });
  }

}
