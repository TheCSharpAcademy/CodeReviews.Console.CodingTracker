using Microsoft.Data.Sqlite;
using ConsoleTableExt;

using System.Configuration;
using System.ComponentModel.Design;

namespace CodingTracker.Wolffles;
	internal class Program
	{
		static void Main()
		{
			string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
			string tableName = "CodingHours";

			SQLiteIO sqliteDatabase = new SQLiteIO(connectionString, tableName);

			//DateTime startTime = DateTime.Now;
			//Console.WriteLine("Wait for time to pass.");
			//Console.ReadKey();
			
			//DateTime endTime = DateTime.Now;

			//ISession currentSession = new CodingSession(0, startTime, endTime, duration);

			//sqliteDatabase.Insert(currentSession);

			Menu menu = new Menu(sqliteDatabase);
			menu.MainMenu();
		}
	}
