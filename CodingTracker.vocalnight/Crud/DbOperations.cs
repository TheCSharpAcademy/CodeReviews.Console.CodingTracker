using CodingTracker.Model;
using ConsoleTableExt;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Crud
{
    public class DbOperations {

        public DbOperations() {
            connection = DbManagement.GetConnection();
            CreateTable();
        }

        static SqliteConnection connection;


        private void CreateTable() {
            DbManagement.CreateTable();
        }

        public void AddEntry(CodingSession session) {
            using (connection) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $"INSERT INTO time_tracking (startTime, endTime, duration)" +
                    $"VALUES ('{session.StartTime}', '{session.EndTime}', '{session.Duration}')";

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void GetAllSessions() {

            var tableData = new List<CodingSession>();

            using (connection) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT id, startTime as start, endTime as end from time_tracking";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        
                        var id = reader.GetString(0);
                        var start = reader.GetString(1);
                        var end = reader.GetString(2);

                        CodingSession session = new CodingSession(id, start, end);
                        
                        tableData.Add(session);                
                    }
                    ConsoleTableBuilder.From(tableData).ExportAndWriteLine();
                }
                connection.Close();
            }
        }

        public void GetFilteredSessions(string startTime, string endTime) {

            var tableData = new List<CodingSession>();

            using (connection) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @$"SELECT id, startTime as start, endTime as end, duration from time_tracking 
                    WHERE startTime > '{startTime} 24:00'
                    AND endTime < '{endTime} 24:00'";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {

                        var id = reader.GetString(0);
                        var start = reader.GetString(1);
                        var end = reader.GetString(2);

                        CodingSession session = new CodingSession(id, start, end);

                        tableData.Add(session);
                    }
                    ConsoleTableBuilder.From(tableData).ExportAndWriteLine();
                }
                connection.Close();
            }
        }

        public void GetShortFormReport(string startTime, string endTime) {

            double total = 0;
            int sessionQuantity = 0;
            

            using (connection) {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @$"SELECT id, startTime as start, endTime as end, duration from time_tracking 
                    WHERE startTime > '{startTime} 00:00'
                    AND endTime < '{endTime} 24:00'";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {

                        var time = TimeSpan.Parse(reader.GetString(3));

                        total += time.TotalMinutes;
                        sessionQuantity++;
                    }

                    var totalTime = TimeSpan.FromMinutes(total);
                    var averageTime = TimeSpan.FromMinutes(total / sessionQuantity);

                    var tableData = new List<List<object>> 
                    {
                        new List<object> { "Total Time studied: ", $"{totalTime.Hours}:{totalTime.Minutes}" },
                        new List<object> { "Average Time studied: ", $"{averageTime.Hours}:{averageTime.Minutes}"}
                    };

                    ConsoleTableBuilder.From(tableData).ExportAndWriteLine();
                }
                connection.Close();
            }
        }
    }
}
