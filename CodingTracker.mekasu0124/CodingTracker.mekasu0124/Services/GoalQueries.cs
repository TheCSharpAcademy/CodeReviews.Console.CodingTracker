using CodingTracker.Models;
using System.Data.SQLite;

namespace CodingTracker.Services;

public class GoalQueries
{
    private static readonly string? dbFile = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    public static void NewGoal(Goal? goal)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();

        cmd.CommandText = @"INSERT INTO goals(Name, DateStarted, DateEnded, DaysToGoal, 
                                    HoursPerDay, Achieved) VALUES ($Name, $StartDate,
                                    $EndDate, $DaysToGoal, $HoursPerDay, $Achieved)";
        cmd.Parameters.AddWithValue("$Name", goal.Name);
        cmd.Parameters.AddWithValue("$StartDate", goal.StartDate);
        cmd.Parameters.AddWithValue("$EndDate", goal.EndDate);
        cmd.Parameters.AddWithValue("$DaysToGoal", goal.DaysToGoal);
        cmd.Parameters.AddWithValue("$HoursPerDay", goal.HoursPerDay);
        cmd.Parameters.AddWithValue("$Achieved", goal.Achieved);

        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(null, goal, "Saved");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void UpdateGoal(Goal? goal)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();

        cmd.CommandText = @"UPDATE goals SET Name=$Name, DateStarted=$StartDate, DateEnded=$EndDate,
                                DaysToGoal=$DaysToGoal, HoursPerDay=$HoursPerDay, Achieved=$Achieved
                                WHERE Id=$SelectedId";
        cmd.Parameters.AddWithValue("$Name", goal.Name);
        cmd.Parameters.AddWithValue("$StartDate", goal.StartDate);
        cmd.Parameters.AddWithValue("$EndDate", goal.EndDate);
        cmd.Parameters.AddWithValue("$DaysToGoal", goal.DaysToGoal);
        cmd.Parameters.AddWithValue("$HoursPerDay", goal.HoursPerDay);
        cmd.Parameters.AddWithValue("$Achieved", goal.Achieved);
        cmd.Parameters.AddWithValue("$SelectedId", goal.Id);

        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(null, goal, "Edited");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static void DeleteGoal(Goal? goal)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        conn.Open();
        cmd.CommandText = "DELETE FROM goals WHERE Id=$selectedId";
        cmd.Parameters.AddWithValue("$selectedId", goal.Id);

        try
        {
            cmd.ExecuteNonQuery();
            Helpers.Finished(null, goal, "Deleted");
        }
        catch (SQLiteException ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static List<Goal?>? GetAllGoals()
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();
        SQLiteDataReader? reader;
        List<Goal?>? goals = new();

        conn.Open();

        cmd.CommandText = "SELECT * FROM goals";

        reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            int? id = int.Parse(reader["Id"].ToString());
            string? name = reader["Name"].ToString();
            string? startDate = reader["DateStarted"].ToString();
            string? endDate = reader["DateEnded"].ToString();
            double? daysToGoal = double.Parse(reader["DaysToGoal"].ToString());
            int? hoursPerDay = int.Parse(reader["HoursPerDay"].ToString());
            string? achieved = reader["Achieved"].ToString();

            Goal? goal = new()
            {
                Id = id,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                DaysToGoal = daysToGoal,
                HoursPerDay = hoursPerDay,
                Achieved = achieved
            };

            goals.Add(goal);
        }

        reader.Close();
        return goals;
    }

    public static Goal? GetSelectedGoal(int? selectedId)
    {
        using SQLiteConnection? conn = new SQLiteConnection(dbFile);
        using SQLiteCommand? cmd = conn.CreateCommand();

        SQLiteDataReader? reader;

        conn.Open();
        cmd.CommandText = "SELECT * FROM goals WHERE Id=$selectedId";
        cmd.Parameters.AddWithValue("$selectedId", selectedId);

        reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            int? id = int.Parse(reader["Id"].ToString());
            string? name = reader["Name"].ToString();
            string? startDate = reader["DateStarted"].ToString();
            string? endDate = reader["DateEnded"].ToString();
            int? daysToGoal = int.Parse(reader["DaysToGoal"].ToString());
            int? hoursPerDay = int.Parse(reader["HoursPerDay"].ToString());
            string? achieved = reader["Achieved"].ToString();

            Goal? goal = new()
            {
                Id = id,
                Name = name,
                StartDate = startDate,
                EndDate = endDate,
                DaysToGoal = daysToGoal,
                HoursPerDay = hoursPerDay,
                Achieved = achieved
            };

            reader.Close();
            return goal;
        }
        else
        {
            return new Goal();
        }
    }
}
