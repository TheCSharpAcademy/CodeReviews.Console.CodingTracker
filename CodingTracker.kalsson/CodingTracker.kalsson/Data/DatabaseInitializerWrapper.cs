using Microsoft.Extensions.Configuration;

namespace CodingTracker.kalsson.Data;

public class DatabaseInitializerWrapper
{
    private readonly IConfiguration _configuration;

    public DatabaseInitializerWrapper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Initialize()
    {
        var databaseInitializer = new DatabaseInitializer(_configuration.GetConnectionString("DefaultConnection"));
        databaseInitializer.InitializeDatabase();
    }
}