using System.Data.SQLite;

namespace CodingTracker
{
    public class GoalsDatabase
    {
        protected string _connectionString;
        protected string _fileName;
        public GoalsDatabase(string connectionString, string fileName)
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
                CREATE TABLE IF NOT EXISTS coding_tracker_goals(
                    id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    goal_time TEXT,
                    current_time TEXT,
                    start_date TEXT, 
                    until_date TEXT, 
                    fulfill_date TEXT,
                    status TEXT)";

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
        private List<Goal> ReadRowsCommand(string commandText)
        {
            var goalsList = new List<Goal>();
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
                                int id = reader["id"] != DBNull.Value ?
                                                int.Parse(reader["id"].ToString()) : 0;

                                var goalTime = reader["goal_time"] != DBNull.Value ?
                                                TimeSpan.Parse(reader["goal_time"].ToString()) : TimeSpan.MinValue;

                                var currentTime = reader["current_time"] != DBNull.Value ?
                                                TimeSpan.Parse(reader["current_time"].ToString()) : TimeSpan.MinValue;

                                var startDate = reader["start_date"] != DBNull.Value ?
                                            DateTime.Parse(reader["start_date"].ToString()) : DateTime.MinValue;

                                DateTime fulfillDate;
                                if (reader["fulfill_date"] != DBNull.Value && reader["fulfill_date"].ToString() != "unfulfilled")
                                    fulfillDate = DateTime.Parse(reader["fulfill_date"].ToString());
                                else
                                    fulfillDate = DateTime.MinValue;

                                var untilDate = reader["until_date"] != DBNull.Value ?
                                            DateTime.Parse(reader["until_date"].ToString()) : DateTime.MinValue;

                                var status = reader["status"] != DBNull.Value ?
                                            reader["status"].ToString() : "";

                                Goal goal = new(id, goalTime, currentTime, untilDate, startDate, fulfillDate, status);
                                goalsList.Add(goal);
                            }

                            return goalsList;
                        }
                    }
                }
            }

            catch (SQLiteException ex)
            {
                UserInput.DisplayMessage($"SQLite error: {ex.Message}");
                return goalsList;
            }
            catch (ArgumentNullException nullEx)
            {
                UserInput.DisplayMessage($"Null exception: {nullEx.Message}");
                return goalsList;
            }
            catch (Exception e)
            {
                UserInput.DisplayMessage($"Exception: {e.Message}");
                return goalsList;
            }
        }
        public List<Goal> GetAll()
        {
            string commandText = $@"
            SELECT * 
            FROM coding_tracker_goals";
            return ReadRowsCommand(commandText);
        }
        public List<Goal> GetActiveGoals()
        {
            string commandText = $@"
            SELECT * 
            FROM coding_tracker_goals
            WHERE status = 'active'";
            return ReadRowsCommand(commandText);
        }
        public void UpdateCurrentTime(int id, string currentTime)
        {
            string commandText = $@"
            UPDATE coding_tracker_goals
            SET current_time = '{currentTime}'
            WHERE id = {id}";
            ExecuteCommand(commandText);
        }
        public void UpdateFulfilledDate(int id, string fulfilledDate)
        {
            string commandText = $@"
            UPDATE coding_tracker_goals
            SET fulfill_date = '{fulfilledDate}'
            WHERE id = {id}";
            ExecuteCommand(commandText);
        }

        public void UpdateStatus(int id, string setKeyword)
        {
            string commandText = $@"
            UPDATE coding_tracker_goals
            SET status = '{setKeyword}'
            WHERE id = {id}";
            ExecuteCommand(commandText);
        }

        public void Insert(string goalTime, string currentTime, string startDate, string untilDate, string fulfillDate)
        {
            string commandText = @$"
            INSERT INTO coding_tracker_goals (goal_time, current_time, start_date, until_date, fulfill_date, status) 
            VALUES ('{goalTime}','{currentTime}','{startDate}','{untilDate}','{fulfillDate}','active')";

            ExecuteCommand(commandText);
        }
    }
}
