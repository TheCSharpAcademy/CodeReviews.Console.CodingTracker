using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CodingTracker.Wolffles;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Wolffles;

internal class SQLiteIO
{
	public string connectionString { get; set; }
	public string tableName { get; set; }
	public ISession session { get; set; }

	public SQLiteIO( string inputConnectionString, string inputTableName)
	{
		connectionString = inputConnectionString;
		tableName = inputTableName; 
				
		ExecuteCommand(@$"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT,Start_Date TEXT,End_Date TEXT,Duration INTEGER)");
	}

	private bool ExecuteCommand(string commandString)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();

			SqliteCommand sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = commandString;
			bool result = sqlCommand.ExecuteReader().HasRows;

			connection.Close();
				
			return result;
		}
	}
	private bool CheckIfExists(string inputID)
	{
		bool commandCheck = ExecuteCommand(@$"SELECT * FROM {tableName} WHERE ID = '{inputID}' ");

		return commandCheck;
	}
	public bool Insert(string inputID, string inputStartDate, string inputEndDate, string inputDuration)
	{
		if(CheckIfExists(inputID))
		{
			return false;
		}
		ExecuteCommand(@$"INSERT INTO {tableName} (Id,Start_Date,EndDate,Duration) VALUES ('{inputID}','{inputStartDate}','{inputEndDate}','{inputDuration}')");

		return true;
	}
	public bool Delete(string inputID)
	{
		if (CheckIfExists(inputID))
		{
			return false;
		}
		ExecuteCommand(@$"DELETE FROM {tableName} WHERE Id = '{inputID}'");

		return true;
	}
	public bool Update(string inputID)
	{
		if (!CheckIfExists(inputID))
		{
			return false;
		}

		ExecuteCommand(@$"UPDATE {tableName} SET {inputStartDate} = '{value}' WHERE Date = '{time}'");
		return true;
	}


}
