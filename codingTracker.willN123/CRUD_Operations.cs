using System.Configuration;
using System.Data.SQLite;
using Dapper;

namespace CodingTracker
{
    internal class CrudOperations
    {
        readonly static string connectionString = ConfigurationManager.ConnectionStrings["CodeTracker"].ConnectionString;
        readonly SQLiteConnection connection = new(connectionString);

        public void CreateTable()
        {
            var createCommand = 
                @"CREATE TABLE IF NOT EXISTS CodeTrackerDatabase (
                id INTEGER PRIMARY KEY AUTOINCREMENT, date TEXT, 
                startTime TEXT, endTime TEXT, totalTime TEXT)";

            connection.Execute(createCommand);
        }

        public void AddToTable(string date, string start, string end, string total)
        {
            var addCommand = 
                $"INSERT INTO CodeTrackerDatabase(date, startTime, endTime, totalTime) VALUES('{date}', '{start}', '{end}', '{total}')";

            connection.Execute(addCommand);
        }

        public void DeleteFromTable(string id)
        {
            var deleteCommand =
                $"DELETE from CodeTrackerDatabase WHERE id = {id}";

            connection.Execute(deleteCommand);
        }

        public void UpdateTable(string date, string start, string end, string total, string id)
        {
            var updateCommand =
                $"UPDATE CodeTrackerDatabase SET date = '{date}', startTime = '{start}', endTime = '{end}', totalTime = '{total}' WHERE id = {id}";

            connection.Execute(updateCommand);
        }

        public List<CodingSession> ReadTable() 
        {
            var readCommand = "SELECT * FROM CodeTrackerDatabase";
            var sessions = connection.Query<CodingSession>(readCommand).ToList();
            return sessions;
        }
    }
}
