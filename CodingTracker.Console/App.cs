using DB;

namespace CodingTracker.Console;

public class App(CodingTimeDBContext dbContext)
{
    private readonly CodingTimeDBContext db = dbContext;

    public void Run()
    {
        db.SeedDatabase();
    }
}
