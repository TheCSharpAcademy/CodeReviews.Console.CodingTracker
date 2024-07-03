using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker;

internal class DataAccess
{
	IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

	private string ConnectionString;
	public DataAccess()
	{
		ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"]!;
	}

	internal void CreateDatabase()
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string createTableQuery = @"
			CREATE TABLE IF NOT EXISTS records (
				Id INTEGER PRIMARY KEY AUTOINCREMENT,
				DateStart TEXT NOT NULL,
				DateEnd TEXT NOT NULL
			)";

			connection.Execute(createTableQuery);
		}
	}

	internal void InsertRecord(CodingRecord record)
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string insertQuery = @"
			INSERT INTO records (DateStart, DateEnd)
			VALUES (@DateStart, @DateEnd)";

			connection.Execute(insertQuery, new { record.DateStart, record.DateEnd });
		}
	}


}
