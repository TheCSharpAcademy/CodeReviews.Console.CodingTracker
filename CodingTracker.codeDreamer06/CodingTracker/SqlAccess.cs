using System.Data.SQLite;

namespace CodingTracker;

class SqlAccess
{
    private const string ConnectionString = "Data Source=./codeTime.db;Version=3;";

    internal class Log
    {
        public long Id { get; set; }
        public string? Duration { get; set; }
        public string? Date { get; set; }
    }

    protected static void Execute(string query)
    {
        try
        {
            using SQLiteConnection con = new(ConnectionString);
            con.Open();
            using var cmd = new SQLiteCommand(query, con);
            cmd.ExecuteNonQuery();
        }

        catch
        {
            Console.WriteLine("An unknown error occured.");
        }
    }

    public static int GetLogsCount()
    {
        using SQLiteConnection con = new(ConnectionString);
        con.Open();
        using var reader = new SQLiteCommand("SELECT COUNT(*) FROM logs", con).ExecuteReader();
        reader.Read();
        return reader.GetInt32(0);
    }

    public static void ShowLogs(string query = @"SELECT * FROM logs")
    {
        var logs = GetLogs(query);
        if (logs is null) return;

        logs.DisplayTable("No logs to display. Type 'help' to learn how to log your first workout!");
    }

    public static List<Log>? GetLogs(string query)
    {
        try
        {
            using SQLiteConnection con = new(ConnectionString);
            con.Open();
            using var cmd = new SQLiteCommand(query, con);
            var reader = cmd.ExecuteReader();
            var logs = new List<Log>();

            while (reader.Read())
            {
                var rawDuration = new DateTime(long.Parse(reader["hours"].ToString()!));
                string hoursSuffix = rawDuration.Hour == 1 ? " hour" : " hours";
                string minuteSuffix = rawDuration.Minute == 1 ? " minute" : " minutes";
                var time = Convert.ToDateTime(reader["created_at"]).ToString().Split()[0];
                var duration = rawDuration.Hour + hoursSuffix;

                if (rawDuration.Minute != 0) duration = duration + " and " + rawDuration.Minute + minuteSuffix;
                logs.Add(new Log { Id = Convert.ToInt64(reader["id"]), Duration = duration, Date = time });
            }

            return logs;
        }

        catch
        {
            Console.WriteLine("An unknown error occurred while trying to read your logs.");
            return null;
        }
    }

    internal static bool LogExists(int id)
    {
        using SQLiteConnection con = new(ConnectionString);
        con.Open();
        using var cmd = new SQLiteCommand($"SELECT COUNT(*) FROM logs WHERE id='{id}'", con);
        return (long)cmd.ExecuteScalar() == 1;
    }

    public static void CreateTable()
    {
        try
        {
            using SQLiteConnection con = new(ConnectionString);
            con.Open();

            var cmd = new SQLiteCommand(@"SELECT name FROM sqlite_master WHERE type='table' AND name='logs'", con);
            if (cmd.ExecuteScalar() == null)
                Execute(@"CREATE TABLE logs(id INTEGER PRIMARY KEY, hours BIGINT NOT NULL, created_at DATETIME DEFAULT CURRENT_DATE)");
        }
        catch
        {
            Console.WriteLine("Unable to check if table exists.");
        }
    }

    public static void AddLog(TimeSpan duration)
    {
        if (duration.Hours == 0) return; 
        Execute($"INSERT INTO logs(hours) VALUES({duration.Ticks});");
        Console.WriteLine("Your hours have been logged!");
        ShowLogs(@"select * from logs ORDER BY id DESC LIMIT 5");
    }

    public static void RemoveLog(int index)
    {
        Execute($"DELETE FROM logs WHERE id = {index};");
        Console.WriteLine($"The log at {index} has been removed.");
        ShowLogs(@"select * from logs ORDER BY id DESC LIMIT 5");
    }

    public static void RemoveLastLog()
    {
        Execute(@"DELETE FROM logs WHERE id = (SELECT MAX(id) FROM logs);");
        Console.WriteLine("The last log has been removed.");
        ShowLogs(@"select * from logs ORDER BY id DESC LIMIT 3");
    }

    public static void UpdateLog(int id, TimeSpan duration)
    {
        if (duration.Hours == 0 || duration.Hours > DateTime.Now.Hour) return;
        Execute($"UPDATE logs SET hours = {duration.Ticks}  WHERE id = {id}");
        Console.WriteLine("Updated successfully.");
        ShowLogs($"select * from logs WHERE id BETWEEN {id - 2} and {id + 2}");
    }
}
