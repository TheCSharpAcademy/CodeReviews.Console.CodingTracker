using Microsoft.Data.Sqlite;

namespace CodeTracker.csm_stough
{
    public class GoalDao
    {
        private static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

        public static void Init()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_goals (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Start TEXT,
                        End TEXT,
                        Target TEXT,
                        Current TEXT
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static CodingGoal InsertGoal(DateTime start, DateTime end, TimeSpan goal)
        {
            CodingGoal codingGoal = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                $@"INSERT INTO coding_goals(Start, End, Target, Current) VALUES('{start.ToString("yyyy-MM-dd hh:mm:ss")}', '{end.ToString("yyyy-MM-dd hh:mm:ss")}', '{goal}', '{TimeSpan.Zero}'); SELECT last_insert_rowid();";
                int id = Convert.ToInt32(command.ExecuteScalar());
                codingGoal = new CodingGoal(id, start, end, TimeSpan.Zero, goal);
                connection.Close();
            }

            return codingGoal;
        }

        public static List<CodingGoal> GetAllGoals(int limit = int.MaxValue, int offset = 0, string where = "", bool ascending = true)
        {
            List<CodingGoal> goals = new List<CodingGoal>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = $"SELECT * FROM coding_goals ";
                if (!string.IsNullOrEmpty(where))
                {
                    command.CommandText += "WHERE " + where + " ";
                }
                command.CommandText += "ORDER BY Start ";
                command.CommandText += ascending ? "ASC " : "DESC ";
                command.CommandText += $"LIMIT {limit} OFFSET {offset} ";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        goals.Add(new CodingGoal(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), reader.GetTimeSpan(4), reader.GetTimeSpan(3)));
                    }
                }

                connection.Close();
            }

            return goals;
        }

        public static void UpdateGoal(CodingGoal goal)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"UPDATE coding_goals SET Current='{goal.CurrentHours}' WHERE Id={goal.Id}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
