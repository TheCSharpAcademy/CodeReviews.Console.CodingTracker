using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace CodingTracker
{
    
    public class Controller
    {
        public static void CreateDatabase()
        {
            using (var conn = new SQLiteConnection("Data Source=database.db;"))
            {
                conn.Open();
                string createTableQuery = "CREATE TABLE IF NOT EXISTS records ( Id INTEGER PRIMARY KEY AUTOINCREMENT, DateStart TEXT, DateEnd TEXT, Duration TEXT )";
                conn.Execute(createTableQuery);
            }
        }

        public static void InsertData(Record CodingRecord)
        {
            using (var conn = new SQLiteConnection("Data Source=database.db;"))
            {
                conn.Open();
                string insertQuery = "INSERT INTO records (DateStart, DateEnd, Duration) VALUES(@DateStart, @DateEnd, @Duration)";
                conn.Query(insertQuery, CodingRecord);
        
            }
        }
        public static IEnumerable<dynamic> ReadData()
        {
            using (var conn = new SQLiteConnection("Data Source=database.db;"))
            {
                conn.Open();
                string selectQuery = "SELECT * FROM records";
                var records = conn.Query(selectQuery);
                return records;

            }
        }

    }

}
