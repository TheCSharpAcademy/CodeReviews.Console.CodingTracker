using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace CondingTracker.Wolffles;

internal class SQLiteIO
{
	public string connectionString { get; set; }
	public string tableName { get; set; }

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

}
