using Dapper;
using System.Data.SQLite;

namespace CodingTracker
{

    public class Controller
    {
        public static void CreateDatabase(string connectionString)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string createTableQuery = "CREATE TABLE IF NOT EXISTS records ( Id INTEGER PRIMARY KEY AUTOINCREMENT, DateStart TEXT, DateEnd TEXT, Duration TEXT )";
                    conn.Execute(createTableQuery);
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public static void InsertData(Record CodingRecord, string connectionString)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO records (DateStart, DateEnd, Duration) VALUES(@DateStart, @DateEnd, @Duration)";
                    conn.Query(insertQuery, CodingRecord);
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public static List<Record> ReadData(string connectionString)
        {
            try
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM records";
                    var records = conn.Query<Record>(selectQuery).ToList();
                    return records;
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); return null; }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); return null; }
        }

    }

}
