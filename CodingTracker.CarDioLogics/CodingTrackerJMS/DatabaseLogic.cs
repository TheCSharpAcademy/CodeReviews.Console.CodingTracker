using CodingTrackerJMS.Model;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTrackerJMS;

internal class DatabaseLogic
{
    Validation validation = new Validation();

    private readonly string connectionString = ConfigurationManager.AppSettings.Get("connectionString");
    
    public string TableName { get; set; }

    public DatabaseLogic()
    {
        TableName = "CodingTracker";
        CreateTable();
    }
   
    public void CreateTable()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {TableName}
                                    (Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    StartingDate TEXT,
                                    EndingDate TEXT,
                                    TotalSessionTime INTEGER,
                                    Goals TEXT,
                                    TimeToCompleteGoal INTEGER)";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void InsertRecord(string startDate, string endDate, int totalTime, string goal  = "GeneralCoding", int timeToGoal = 0)
    {
        using(SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"INSERT INTO {TableName} (StartingDate, EndingDate, TotalSessionTime, Goals, TimeToCompleteGoal) VALUES (@startDate, @endDate, @totalTime, @goal, @timeToGoal);";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);
            command.Parameters.AddWithValue("@totalTime", totalTime);
            command.Parameters.AddWithValue("@goal", goal);
            command.Parameters.AddWithValue("@timeToGoal", timeToGoal);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void UpdateRecord(int id, string startDate, string endDate, int totalTime, string goal = "GeneralCoding", int timeToGoal = 0) 
    {
        using(SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open ();
            string query = $@"UPDATE {TableName}
                            SET StartingDate = @startDate, EndingDate = @endDate, TotalSessionTime = @totalTime, Goals = @goal, TimeToCompleteGoal = @timeToGoal
                            Where id = @id;";
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@endDate", endDate);
            command.Parameters.AddWithValue("@totalTime", totalTime);
            command.Parameters.AddWithValue("@goal", goal);
            command.Parameters.AddWithValue("@timeToGoal", timeToGoal);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void DeleteRecord(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"DELETE FROM {TableName} WHERE id = @id";
            var command = connection.CreateCommand();  
            command.CommandText = query;
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<CodingSession> GetSessionRecords(ref string order) //need to fix this issue with the list
    {
        List<CodingSession> session = new List<CodingSession> ();

        using(SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT * FROM {TableName} ORDER BY TotalSessionTime {order}";
            var command = connection.CreateCommand();
            command.CommandText = query;
            Console.WriteLine($"Table selected: {TableName}");
            using( var reader = command.ExecuteReader() ) 
            {
                while( reader.Read() ) 
                {
                    int id = reader.GetInt32(0);
                    string startDate = reader.GetString(1);
                    string endDate = reader.GetString(2);
                    int totalTime = reader.GetInt32(3);
                    string goal = reader.GetString(4);
                    int timeToGoal = reader.GetInt32(5);

                    DateTime startDateT;
                    validation.GetValidDate(startDate,false, out startDateT);

                    DateTime endDateT;
                    validation.GetValidDate(endDate, false, out endDateT);

                    session.Add(new CodingSession(id, startDateT, endDateT, totalTime, goal, timeToGoal));
                }
            }
        }
        return session;
    }

    public List<Goals> GetGoalsRecords() 
    {
        List<Goals> goals = new List<Goals>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT DISTINCT Goals, TimeToCompleteGoal FROM {TableName}";
            var command = connection.CreateCommand();
            command.CommandText = query;
            Console.WriteLine($"Table selected: {TableName}");
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string goal = reader.GetString(0);
                    int timeToGoal = reader.GetInt32(1);
                    goals.Add(new Goals(goal, timeToGoal));
                }
            }
        }
        return goals;
    }
}
