using Microsoft.Data.Sqlite;

using System.Configuration;

namespace CodingTracker.Wolffles;
	internal class Program
	{
		static void Main()
		{
			string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
			string tableName = "CodingHours";

			SQLiteIO sqliteDatabase = new SQLiteIO(connectionString, tableName);

			sqliteDatabase.Insert("2","10-01-2023", "10-02-2023", "200");
			sqliteDatabase.Read();
		}
	}
