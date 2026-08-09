using CyrillParfen.CodingTracker.Data;
using CyrillParfen.CodingTracker.UI;
using Microsoft.Extensions.Configuration;

namespace CyrillParfen.CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        string connectionString = GetConnectionString();
        var dataAccess = new DataAccess(connectionString);
        dataAccess.CreateTable();

        UserMenu userMenu = new(dataAccess);
        userMenu.RunUserMenu();
    }

    private static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        return config.GetConnectionString("DefaultConnection");
    }
}
