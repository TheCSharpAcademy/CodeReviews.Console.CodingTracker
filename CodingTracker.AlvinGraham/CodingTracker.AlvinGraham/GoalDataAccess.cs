using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace CodingTracker;

internal class GoalDataAccess
{
	IConfiguration configuration = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

	private string ConnectionString;
	public GoalDataAccess()
	{
		ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"]!;
	}

	internal List<GoalRecord> GetGoalList()
	{
		List<GoalRecord> records = new List<GoalRecord>();

		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string selectQuery = "SELECT * FROM goals";

			var results = connection.Query<GoalRecord>(selectQuery);

			foreach (var result in results)
			{
				records.Add(new GoalRecord
				{
					Id = result.Id,
					TotalMinutes = result.TotalMinutes,
					DateEnd = result.DateEnd,
				});
			}
		}
		return records;
	}

	internal void InsertGoal(GoalRecord record)
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string insertQuery = @"
			INSERT INTO goals (TotalMinutes, DateEnd)
			VALUES (@TotalMinutes, @DateEnd)";

			connection.Execute(insertQuery, new { record.TotalMinutes, record.DateEnd });
		}
	}

	internal void UpdateGoal(GoalRecord updatedGoal)
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string updateQuery = @"
			UPDATE goals
			SET TotalMinutes = @TotalMinutes, DateEnd = @DateEnd
			WHERE Id = @Id";

			var queryParams = new { updatedGoal.TotalMinutes, updatedGoal.DateEnd, updatedGoal.Id };

			connection.Execute(updateQuery, queryParams);
		}
	}

	internal int DeleteGoal(int recordId)
	{
		using (var connection = new SqliteConnection(ConnectionString))
		{
			connection.Open();

			string deleteQuery = "DELETE FROM goals WHERE Id = @Id";
			var queryParams = new { Id = recordId };

			int rowsAffected = connection.Execute(deleteQuery, queryParams);

			return rowsAffected;
		}
	}
}
