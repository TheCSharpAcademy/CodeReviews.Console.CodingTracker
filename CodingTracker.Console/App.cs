using DB;
using Models.Entities;

namespace CodingTracker;

public class App(CodingTimeDBContext dbContext)
{
    private readonly CodingTimeDBContext db = dbContext;

    public void Run()
    {
        db.SeedDatabase();
    }
}
