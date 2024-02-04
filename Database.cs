using System.Data.SQLite;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace CodingTracker
{
    public class Database
    {
        private string _connectionString;
        private string _fileName;
        public Database(string connectionString, string fileName)
        {
            _connectionString = connectionString;
            _fileName = fileName;
            InitializeDatabase();
        }
        private void InitializeDatabase()
        {
            if (!File.Exists(_fileName))
            {
                Console.WriteLine("Database file does not exist. A new database will be created.");
            }

            using (var connection = new SQLiteConnection(_connectionString))
            {

                string commandText = "CREATE TABLE IF NOT EXISTS coding_tracker(id INTEGER PRIMARY KEY AUTOINCREMENT, notes TEXT, date_start TEXT, date_end TEXT, duration TEXT)";
                SQLiteCommand command = new(commandText, connection);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private void ExecuteCommand(string commandText)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    SQLiteCommand command = new(commandText, connection);

                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
            catch (SQLiteException ex)
            {
                UserInput.DisplayMessage($"SQLite error: {ex.Message}");
            }
        }
        private List<CodingSession> ReadCommand(string commandText)
        {
            var codingSessionList = new List<CodingSession>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(commandText, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CodingSession codingSession = new()
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    StartTime = reader["date_start"] != DBNull.Value ? DateTime.Parse(reader["date_start"].ToString()) : DateTime.MinValue,
                                    EndTime = reader["date_end"] != DBNull.Value ? DateTime.Parse(reader["date_end"].ToString()) : DateTime.MinValue,
                                    Duration = reader["duration"] != DBNull.Value ? TimeSpan.Parse(reader["duration"].ToString()) : TimeSpan.MinValue,
                                    Note = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : ""
                                };
                                codingSessionList.Add(codingSession);
                            }

                            return codingSessionList;
                        }
                    }
                }
            }

            catch (SQLiteException ex)
            {
                UserInput.DisplayMessage($"SQLite error: {ex.Message}");
                return codingSessionList;
            }
            catch (ArgumentNullException nullEx)
            {
                UserInput.DisplayMessage($"Null exception: {nullEx.Message}");
                return codingSessionList;
            }
            catch (Exception e)
            {
                UserInput.DisplayMessage($"Exception: {e.Message}");
                return codingSessionList;
            }
        }

        public void Insert(string notes, string dateStart, string dateEnd, string duration)
        {
            string commandText = $"INSERT INTO coding_tracker (notes, date_start, date_end, duration) VALUES ('{notes}','{dateStart}','{dateEnd}','{duration}')";
            ExecuteCommand(commandText);
        }
        public void Update(int id, string notes, string dateStart, string dateEnd, string duration)
        {
            string commandText = $"UPDATE coding_tracker SET notes = '{notes}', date_start = '{dateStart}', date_end = '{dateEnd}',duration = '{duration}' WHERE Id = {id}";
            ExecuteCommand(commandText);
        }
        public void Delete(int id)
        {
            string commandText = $"DELETE FROM coding_tracker WHERE Id = {id}";
            ExecuteCommand(commandText);
        }

        public List<CodingSession> GetAll()
        {
            string commandText = "SELECT * FROM coding_tracker";
            return ReadCommand(commandText);
        }

        public List<CodingSession> GetByIndex(int index)
        {
            string commandText = $"SELECT * FROM coding_tracker WHERE Id = {index}";
            return ReadCommand(commandText);
        }
    }

}