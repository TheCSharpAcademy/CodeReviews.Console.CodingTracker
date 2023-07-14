using Microsoft.Data.Sqlite;
using ConsoleTableExt;

using System.Configuration;

namespace CodingTracker.Wolffles;
	internal class Program
	{
		static void Main()
		{
			string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
			string tableName = "CodingHours";

			SQLiteIO sqliteDatabase = new SQLiteIO(connectionString, tableName);

			DateTime startTime = DateTime.Now;
			Console.WriteLine("Wait for time to pass.");
			Console.ReadKey();
			
			DateTime endTime = DateTime.Now;
		    TimeSpan duration = endTime - startTime;

			ISession currentSession = new CodingSession(0, startTime, endTime, duration);

			sqliteDatabase.Insert(currentSession);

			List<ISession> list = sqliteDatabase.Read();
			
			List<List<object>> sessionList = new List<List<object>>();
			
			//Need to convert ISession to a string List to be processed by ConsoleTableExt
			foreach (ISession session in list) 
			{
				sessionList.Add(new List<object> {session.Id, session.StartDate.ToString(),session.EndDate.ToString(),session.Duration.ToString() });
			}
			//Formatting for ConsoleTableExt
			ConsoleTableBuilder.From(sessionList)
			.WithTitle("Coding Sessions", ConsoleColor.Yellow)
			.WithColumn("Id","Start Date" , "End Date" , "Duration")
			.WithTextAlignment(new Dictionary<int, TextAligntment>
			{
                {0, TextAligntment.Center },
				{1, TextAligntment.Center },
				{2, TextAligntment.Center },
                {3, TextAligntment.Center }
            })
            .WithMinLength(new Dictionary<int, int> {
				{ 1, 25 },
				{ 2, 25 },
				{ 3, 25 }
			})
            .ExportAndWriteLine();

			



		}
	}
