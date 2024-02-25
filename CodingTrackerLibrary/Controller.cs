using Dapper;
using System.Data.SQLite;

namespace CodingTracker
{
    
    public class Controller
    {
        public static void CreateDatabase()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=database.db; Version = 3;"))
                {
                    conn.Open();
                    string createTableQuery = "CREATE TABLE IF NOT EXISTS records ( Id INTEGER PRIMARY KEY AUTOINCREMENT, DateStart TEXT, DateEnd TEXT, Duration TEXT )";
                    conn.Execute(createTableQuery);
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }

        public static void InsertData(Record CodingRecord)
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=database.db;"))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO records (DateStart, DateEnd, Duration) VALUES(@DateStart, @DateEnd, @Duration)";
                    conn.Query(insertQuery, CodingRecord);
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
        }
        public static IEnumerable<dynamic> ReadData()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=database.db;"))
                {
                    conn.Open();
                    string selectQuery = "SELECT * FROM records";
                    var records = conn.Query(selectQuery);
                    return records;
                }
            }
            catch (SQLiteException ex) { Console.WriteLine($"SQLiteException Error: {ex.Message}"); return null; }
            catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); return null; }
        }

    }

}
