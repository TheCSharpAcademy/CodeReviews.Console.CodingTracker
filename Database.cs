using System.Data.SQLite;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using Spectre.Console;


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

                string commandText = @"
                CREATE TABLE IF NOT EXISTS coding_tracker(
                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    notes TEXT, 
                    date_start TEXT, 
                    date_end TEXT, 
                    duration TEXT, 
                    _year INTEGER, 
                    _month INTEGER, 
                    _week INTEGER)";

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
        private List<CodingSession> ReadRowsCommand(string commandText)
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
        private List<object> ReadColumnCommand(string commandText)
        {
            var columnValuesList = new List<object>();
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
                                columnValuesList.Add(reader[0]);
                            }

                            return columnValuesList;
                        }
                    }
                }
            }

            catch (SQLiteException ex)
            {
                UserInput.DisplayMessage($"SQLite error: {ex.Message}");
                return columnValuesList;
            }
            catch (ArgumentNullException nullEx)
            {
                UserInput.DisplayMessage($"Null exception: {nullEx.Message}");
                return columnValuesList;
            }
            catch (Exception e)
            {
                UserInput.DisplayMessage($"Exception: {e.Message}");
                return columnValuesList;
            }
        }

        public void Insert(string notes, string dateStart, string dateEnd, string duration, int year, int month, int week)
        {
            string commandText = @$"
            INSERT INTO coding_tracker (notes, date_start, date_end, duration, _year, _month, _week) 
            VALUES ('{notes}','{dateStart}','{dateEnd}','{duration}',{year},{month},{week})";

            ExecuteCommand(commandText);
        }
        public void Update(int id, string notes, string dateStart, string dateEnd, string duration, int year, int month, int week)
        {
            string commandText = @$"
            UPDATE coding_tracker 
            SET notes = '{notes}', date_start = '{dateStart}', date_end = '{dateEnd}', 
            duration = '{duration}', _year = {year}, _month = {month}, _week = {week} 
            WHERE Id = {id}";

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
            return ReadRowsCommand(commandText);
        }

        public List<CodingSession> GetByIndex(int index)
        {
            string commandText = $"SELECT * FROM coding_tracker WHERE Id = {index}";
            return ReadRowsCommand(commandText);
        }

        public string[] GetDistinctYears()
        {
            string commandText = $@"
            SELECT DISTINCT _year
            FROM coding_tracker";

            List<object> yearList = ReadColumnCommand(commandText);
            
            return LogicOperations.ToStringArray(yearList);

        }
        public string[] GetDistinctMonths(string year)
        {
            string commandText = $@"
            SELECT DISTINCT _month
            FROM coding_tracker
            WHERE _year = {year}";

            List<object> monthList = ReadColumnCommand(commandText);

            return LogicOperations.MonthsToStringArray(monthList);
        }
    }

}