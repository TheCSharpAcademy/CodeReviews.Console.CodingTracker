using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
			sqlCommand.ExecuteReader();

			connection.Close();

			return true;
		}
	}
	private bool CheckIfExists(string inputID)
	{
		bool commandCheck = ExecuteCommand(@$"SELECT * FROM {tableName} WHERE ID = '{inputID}' ");

		return commandCheck;
	}
	public bool Insert(string inputID, string inputStartDate, string inputEndDate, string inputDuration)
	{

		ExecuteCommand(@$"INSERT INTO {tableName} (Start_Date,End_Date,Duration) VALUES ('{inputStartDate}','{inputEndDate}','{inputDuration}')");

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
	
	public bool Update(string inputID, string inputDate)
	{
		if(!CheckIfExists(inputID))
		{
			return false;
		}
		ExecuteCommand($@"UPDATE {tableName} SET Start_Date = {inputDate} WHERE Id = {inputID}");

		return true;
	}
    public void Read()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @$"SELECT * FROM {tableName} ORDER BY Start_Date";
            SqliteDataReader dataReader = tableCommand.ExecuteReader();

            while (dataReader.Read())
            {
                string date = dataReader.GetString(1);
                string tracked_value = dataReader.GetString(2);
                Console.WriteLine($"{date,0} | {tracked_value,0}");
            }
            connection.Close();
        }
    }
}
