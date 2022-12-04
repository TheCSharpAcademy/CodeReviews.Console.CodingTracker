using Microsoft.Data.Sqlite;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackingProgram
{
    public class TrackDB
    {
        public static void StartupDatabase(string connectionString)
        {
            using (var trackingDatabase = new SqliteConnection(connectionString))
            {
                trackingDatabase.Open();
                var tableCmd = trackingDatabase.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_entries (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Label TEXT,
StartDate TEXT,
EndDate TEXT
)";

                tableCmd.ExecuteNonQuery();
                trackingDatabase.Close();

            }
        }

        public static List<CodeEntry> GetAllCodeRecords(string connectionString)
        {
            using (var myDatabase = new SqliteConnection(connectionString))
            {
                myDatabase.Open();
                var tableCmd = myDatabase.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM coding_entries";

                List<CodeEntry> tableData = new();
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CodeEntry
                            {
                                Id = reader.GetInt32(0),
                                Label = reader.GetString(1),
                                StartDate = DateTime.ParseExact(reader.GetString(2), "dd/MM/yyyy HH:mm tt", new CultureInfo("en-US")),
                                EndDate = DateTime.ParseExact(reader.GetString(3), "dd/MM/yyyy HH:mm tt", new CultureInfo("en-US")),
                            }
                            );
                    }
                    myDatabase.Close();
                    return tableData;
                }
                else
                {
                    myDatabase.Close();
                    return null;
                }
            }
        }

        public static void InsertCodeEntry(CodeEntry codeEntry,string connectionString)
        {
            string format = "dd/MM/yyyy HH:mm tt";
            using (var myDatabase = new SqliteConnection(connectionString))
            {
                myDatabase.Open();
                var tableCmd = myDatabase.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO coding_entries(Label, StartDate, EndDate) VALUES('{codeEntry.Label}', '{codeEntry.StartDate.ToString(format)}','{codeEntry.EndDate.ToString(format)}')";
                tableCmd.ExecuteNonQuery();
                myDatabase.Close();
            }
        }

        public class CodeEntry
        {
            public int Id { get; set; }
            public string Label { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}