using CodingTracker.DataAccess;

namespace CodingTracker;

internal static class Program
{
    private const string _defaultConnectionString = "Data Source=CodingTracker.db";
    public const string mainDateFormat = "yyyy-MM-dd";
    public const string mainTimeFormat = "HH:mm:ss";
    public const string mainFullFormat = mainDateFormat + " " + mainTimeFormat;
    public static string[] dateFormats = { mainDateFormat };
    public static string[] timeFormats = { mainTimeFormat, "HH:mm" };

    static void Main()
    {
        string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? _defaultConnectionString;
        IDataAccess dataAccess = new SqliteStorage(connectionString);
        UI.MainMenu.Get(dataAccess).Show();
        Console.Clear();
    }
}