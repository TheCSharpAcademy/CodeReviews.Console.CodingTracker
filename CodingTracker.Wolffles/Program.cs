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
			
			
			
		}
	}
