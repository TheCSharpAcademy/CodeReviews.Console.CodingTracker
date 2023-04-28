namespace CodingTracker.barakisbrown;

using System.Configuration;

public class CodingController
{
    private readonly string ConnectionName = "myDB";
    private readonly string? DataSource;

    public CodingController()
    {
        ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[ConnectionName];
        if (settings != null)
            DataSource = settings.ConnectionString;
    }
}
