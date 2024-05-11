using CodingTracker.kalsson.Data;
using CodingTracker.kalsson.UI;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.kalsson.Startup;

public class ApplicationStarter
{
    private readonly IConfiguration _configuration;

    public ApplicationStarter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Start()
    {
        var consoleMenu = new ConsoleMenu(new CodingSessionRepository(_configuration.GetConnectionString("DefaultConnection")));
        consoleMenu.ShowMenu();
    }
}