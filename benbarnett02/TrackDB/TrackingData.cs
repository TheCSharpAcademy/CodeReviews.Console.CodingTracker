using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;
namespace TrackingProgram;

public class TrackingData
{
    public static string connectionString = ConfigurationManager.ConnectionStrings["TrackDB"].ConnectionString;
    public static void StartupDatabase()
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

    public static List<CodeEntry> GetAllCodeRecords()
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

    public static void InsertCodeEntry(CodeEntry codeEntry)
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

    public static void UpdateCodeEntry(CodeEntry codeEntry)
    {
        string format = "dd/MM/yyyy HH:mm tt";
        using (var myDatabase = new SqliteConnection(connectionString))
        {
            myDatabase.Open();
            var tableCmd = myDatabase.CreateCommand();
            tableCmd.CommandText =
                $"UPDATE coding_entries SET Label = '{codeEntry.Label}', StartDate = '{codeEntry.StartDate.ToString(format)}', EndDate = '{codeEntry.EndDate.ToString(format)}' WHERE Id = {codeEntry.Id.ToString()}";
            tableCmd.ExecuteNonQuery();
            myDatabase.Close();
        }
    }

    public static void DeleteCodeEntry(CodeEntry codeEntry)
    {
        DeleteCodeEntry(codeEntry.Id);
    }
    public static void DeleteCodeEntry(int id)
    {
        DeleteCodeEntry(id.ToString());
    }

    public static void DeleteCodeEntry(string id)
    {
        using (var myDatabase = new SqliteConnection(connectionString))
        {
            myDatabase.Open();
            var tableCmd = myDatabase.CreateCommand();
            tableCmd.CommandText =
            $"DELETE FROM coding_entries WHERE Id = '{id}'";
            tableCmd.ExecuteNonQuery();
            myDatabase.Close();
        }
    }

    public static bool TryGetCodeEntry(string id)
    {
        bool exists;
        using (var myDatabase = new SqliteConnection(connectionString))
        {
            myDatabase.Open();
            var checkCmd = myDatabase.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_entries WHERE Id = '{id}')";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                exists = false;
            }
            else if (checkQuery == 1)
            {
                exists = true;
            }
            else
            {
                throw new Exception($"More than one or less than zero records were found with record ID {id}. Review the database and check for duplicate entries.");
            }
            myDatabase.Close();
        }
        return exists;
    }

    public static bool TryGetCodeEntry(int id)
    {
        bool exists = TryGetCodeEntry(id.ToString());
        return exists;
    }

    public static CodeEntry GetCodeEntry(string id)
    {
        using (var myDatabase = new SqliteConnection(connectionString))
        {
            myDatabase.Open();
            var tableCmd = myDatabase.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM coding_entries WHERE Id = '{id}'";
            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                CodeEntry codeEntry = new CodeEntry
                {
                    Id = reader.GetInt32(0),
                    Label = reader.GetString(1),
                    StartDate = DateTime.ParseExact(reader.GetString(2), "dd/MM/yyyy HH:mm tt", new CultureInfo("en-US")),
                    EndDate = DateTime.ParseExact(reader.GetString(3), "dd/MM/yyyy HH:mm tt", new CultureInfo("en-US"))
                };
                myDatabase.Close();
                return codeEntry;
            }
            else
            {
                myDatabase.Close();
                return null;
            }
        }
    }

    public static TimeSpan getDurationOfCodeEntry(CodeEntry codeEntry)
    {
        TimeSpan duration = codeEntry.EndDate.Subtract(codeEntry.StartDate);
        return duration;
    }

    public class CodeEntry
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}