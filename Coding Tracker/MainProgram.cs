namespace CodingTracker;

public class Program
{
    public static void Main(string[] args)
    {
        Model.CreateDatabase();
        GoalsModel.CreateGoalsDatabase();
        Controller.Run();
    }
}