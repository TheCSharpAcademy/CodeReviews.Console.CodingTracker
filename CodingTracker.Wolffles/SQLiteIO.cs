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

//Functions:
//ExecuteCommand - Tested
//CheckIfFound - Tested
//Insert - Tested
//Delete - Tested
//Update - Tested
//Read - Tested
internal class SQLiteIO
{
	public string connectionString { get; set; }
	public string tableName { get; set; }

	public SQLiteIO(string inputConnectionString, string inputTableName)
	{
		connectionString = inputConnectionString;
		tableName = inputTableName;
		ExecuteCommand(@$"CREATE TABLE IF NOT EXISTS {tableName} (Id INTEGER PRIMARY KEY AUTOINCREMENT,Start_Date TEXT,End_Date TEXT,Duration INTEGER)");
	}
	private void ExecuteCommand(string commandString)
	{
		using (var connection = new SqliteConnection(connectionString))
		{
			connection.Open();

			SqliteCommand sqlCommand = connection.CreateCommand();
			sqlCommand.CommandText = commandString;
			sqlCommand.ExecuteReader();

			connection.Close();
		}
	}
	public void Insert(ISession session)
	{
		if (!CheckIfFound(session.StartDate.ToString()))
		{
			ExecuteCommand(@$"INSERT INTO {tableName} (Start_Date,End_Date,Duration) VALUES ('{session.StartDate}','{session.EndDate}','{session.Duration}')");
			Console.WriteLine("Insert - Entry Succesfuly");

			return;
		}

		Console.WriteLine("Insert - Entry already exists.");
		return;
	}
	public void Delete(string inputDate)
	{
		if(CheckIfFound(inputDate))
		{
			Console.WriteLine("No entry with this value found.");
		}
		ExecuteCommand(@$"DELETE FROM {tableName} WHERE Start_Date = '{inputDate}'");
	}
	public void Update(string inputDate, string replacementDate)
	{
        if (CheckIfFound(inputDate))
        {
            Console.WriteLine("No entry with this value found.");
        }
        ExecuteCommand($@"UPDATE {tableName} SET Start_Date = '{replacementDate}' WHERE Start_Date = '{inputDate}'");
	}
	public List<ISession> Read()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @$"SELECT * FROM {tableName} ORDER BY Start_Date";
            SqliteDataReader dataReader = tableCommand.ExecuteReader();

			List<ISession> list = new List<ISession>();	
	
            while (dataReader.Read())
            {
				int id = Int32.Parse(dataReader.GetString(0));
				DateTime startDate = DateTime.Parse(dataReader.GetString(1));
				DateTime endDate = DateTime.Parse(dataReader.GetString(2));
				TimeSpan duration = TimeSpan.Parse(dataReader.GetString(3));

				ISession currentReadSession = new CodingSession(id, startDate, endDate, duration);

				list.Add(currentReadSession);
            }
            connection.Close();

            return list;
        }
    }
	private bool CheckIfFound(string checkDate)
	{
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand tableCommand = connection.CreateCommand();
            tableCommand.CommandText = @$"SELECT * FROM {tableName} WHERE Start_Date = '{checkDate}'";
            SqliteDataReader dataReader = tableCommand.ExecuteReader();

			if(dataReader.Read())
			{
				return true;
			}
            return false;
        }
    }
}
