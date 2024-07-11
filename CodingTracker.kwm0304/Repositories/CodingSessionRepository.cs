using CodingTracker.kwm0304.Data;

namespace CodingTracker.kwm0304.Repositories;

public class CodingSessionRepository
{
    private readonly DbAction _dbActions;
    public CodingSessionRepository()
    {
      _dbActions = new DbAction();
      _dbActions.CreateDatabaseIfNotExists();
      _dbActions.CreateTableIfNotExists();
    }
}
