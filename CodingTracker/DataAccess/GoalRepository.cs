using Dapper;
using Spectre.Console;

public class GoalRepository
{
    private DatabaseManager _databaseManager;

    public GoalRepository(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }

    public void AddGoal(int hours)
    {
        CodingGoals codingGoals = new CodingGoals
        {
            TotalHours = hours,
            StartDate = DateTime.Now,
            CompletionStatus = "Ongoing"
        };

        string query = "INSERT INTO goals (TotalHours, StartDate, CompletionStatus) VALUES (@TotalHours, @StartDate, @CompletionStatus)";
        using (var conn = _databaseManager.GetConnection())
        {
            conn.Execute(query, codingGoals);
        }
    }

    public List<CodingGoals> GetCodingGoals()
    {
        string query = "SELECT * FROM goals";
        using (var conn = _databaseManager.GetConnection())
        {
            return conn.Query<CodingGoals>(query).ToList();
        }
    }

    public void UpdateGoal(CodingGoals codingGoal)
    {
        string query = "UPDATE goals SET EndDate = @EndDate, CompletionStatus = @CompletionStatus WHERE Id = @Id";
        using (var conn = _databaseManager.GetConnection())
        {
            conn.Execute(query, codingGoal);
        }
    }

    public void RemoveGoal(CodingGoals codingGoal)
    {
        string query = "DELETE FROM goals WHERE Id = @Id";
        using (var conn = _databaseManager.GetConnection())
        {
            conn.Execute(query, codingGoal);
        }
    }

    public void InsertTestData(int number)
    {
        for (int i = 0; i < number; i++)
        {
            DateTime startDate;
            int totalHours;

            do
            {
                startDate = Utilities.GenerateRandomDate();
                totalHours = Utilities.RandomNumber(0, 50);
            } while (totalHours >= 5);

            CodingGoals codingGoals = new CodingGoals
            {
                TotalHours = totalHours,
                StartDate = startDate,
                CompletionStatus = "Ongoing"
            };

            string query = "INSERT INTO goals (TotalHours, StartDate, CompletionStatus) VALUES (@TotalHours, @StartDate, @CompletionStatus)";
            using (var conn = _databaseManager.GetConnection())
            {
                conn.Execute(query, codingGoals);
            }

            AnsiConsole.WriteLine($"{i + 1}. Added recored to database: {codingGoals.TotalHours} {codingGoals.StartDate} {codingGoals.CompletionStatus}");

        }
    }
}