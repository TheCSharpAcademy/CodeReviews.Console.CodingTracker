global using System.Configuration;
using HabitTracker_obitom67;
using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;


namespace CodingTracker.obitom67
{
    internal static class DBHandling
    {
        

        public static void TableHandling()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            using (var connection = new SqliteConnection(connectionString))
            {
                string sql = @"CREATE TABLE IF NOT EXISTS CodingSessions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT
                        )";
                connection.Execute(sql);
                
            }
        }

        public static void Insert()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            CodingSession codingSession= new CodingSession();
            codingSession.StartTime = DateTime.ParseExact(UserInput.GetDateInput(),"dd-MM-yy H:mm", new CultureInfo("en-US"));
            codingSession.EndTime = DateTime.ParseExact(UserInput.GetDateInput(), "dd-MM-yy H:mm", new CultureInfo("en-US"));
            while (!codingSession.CheckDates(codingSession.StartTime, codingSession.EndTime))
            {
                AnsiConsole.WriteLine("End Time is before Start Time, please try again.");
                codingSession.EndTime = DateTime.ParseExact(UserInput.GetDateInput(), "dd-MM-yy H:mm", new CultureInfo("en-US"));
            }
            codingSession.Duration = codingSession.GetDuration(codingSession.StartTime,codingSession.EndTime);

            string sql = $"INSERT INTO CodingSessions(StartTime, EndTime, Duration) VALUES ('{codingSession.StartTime.ToString()}','{codingSession.EndTime.ToString()}','{codingSession.Duration.ToString()}')";
            using (var connection = new SqliteConnection(connectionString))
            {
                
                connection.Execute(sql);
                

            }
            
        }

        

        public static void Update()
        {
            string connectionString =ConfigurationManager.AppSettings.Get("key1");
            AnsiConsole.Clear();
            ViewRecords();

            var tableId = UserInput.GetNumberInput("\n\nPlease input the Id of the record that you would like to update.\n\n");
            using( var connection = new SqliteConnection(connectionString))
            {
                string sql = $"SELECT EXISTS(SELECT 1 FROM CodingSessions WHERE Id = {tableId})";
                int checkQuery = Convert.ToInt32(connection.ExecuteScalar(sql));

                if (checkQuery == 0)
                {
                    AnsiConsole.WriteLine("\n\nThere is no record with that Id");
                    Update();
                }

                CodingSession codingSession = new CodingSession();
                codingSession.StartTime = DateTime.ParseExact(UserInput.GetDateInput(), "dd-MM-yy H:mm", new CultureInfo("en-US"));
                codingSession.EndTime = DateTime.ParseExact(UserInput.GetDateInput(), "dd-MM-yy H:mm", new CultureInfo("en-US"));
                while (!codingSession.CheckDates(codingSession.StartTime,codingSession.EndTime))
                {
                    AnsiConsole.WriteLine("End Time is before Start Time, please try again.");
                    codingSession.EndTime = DateTime.ParseExact(UserInput.GetDateInput(), "dd-MM-yy H:mm", new CultureInfo("en-US"));
                }
                codingSession.Duration = codingSession.GetDuration(codingSession.StartTime, codingSession.EndTime);

                sql = $"UPDATE CodingSessions SET StartTime = '{codingSession.StartTime}', EndTime = '{codingSession.EndTime}', Duration = '{codingSession.Duration}' WHERE Id = {tableId}";
                connection.Execute(sql);
            }


            
        }

        public static void Delete()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            AnsiConsole.Clear();
            ViewRecords();

            var tableId = UserInput.GetNumberInput("\n\nPlease type the Id of the record you want to delete\n\n");
            using( var connection = new SqliteConnection(connectionString))
            {
                string sql = $"DELETE FROM CodingSessions WHERE Id = {Convert.ToInt32(tableId)}";
                connection.Execute(sql);
            }
            

        }
        public static void ViewRecords()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("key1");
            var sql = $"SELECT * FROM CodingSessions";
            List<CodingSession> sessionsList = new List<CodingSession>();
            using (var connection = new SqliteConnection(connectionString))
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    CodingSession session = new CodingSession();
                    session.Id = reader.GetInt32(0);
                    session.StartTime = DateTime.Parse(reader.GetString(1));
                    session.EndTime = DateTime.Parse(reader.GetString(2));
                    session.Duration = TimeSpan.Parse(reader.GetString(3));
                    sessionsList.Add(session);
                }
                reader.Close();
                
            }
            UserInput.ShowList(sessionsList);

            

        }
    }
}
