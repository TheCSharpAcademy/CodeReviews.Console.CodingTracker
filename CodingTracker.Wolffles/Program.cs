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

			SQLiteIO sqliteDatabase	 = new SQLiteIO(connectionString, tableName);

			Menu menu = new Menu(sqliteDatabase);
			menu.MainMenu();
		}
	}
