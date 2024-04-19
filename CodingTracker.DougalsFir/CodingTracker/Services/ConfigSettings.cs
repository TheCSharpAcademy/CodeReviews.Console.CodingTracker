namespace CodingTracker.Services;

public class ConfigSettings
{
    public static string DbFilePath => System.Configuration.ConfigurationManager.AppSettings["DatabaseFilePath"]!;
    public static string DbConnectionString => System.Configuration.ConfigurationManager.AppSettings["DatabaseConnectionString"]!;
    public static string DateFormatShort => System.Configuration.ConfigurationManager.AppSettings["DateFormatShort"]!;
    public static string DateFormatLong => System.Configuration.ConfigurationManager.AppSettings["DateFormatLong"]!;
    public static string TimeFormatType => System.Configuration.ConfigurationManager.AppSettings["TimeFormatType"]!;
    public static string TimeFormatString => System.Configuration.ConfigurationManager.AppSettings["TimeFormatString"]!;
    public static int NumberOfCodingSessionsToSeed => Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["NumberOfCodingSessionsToSeed"]!);
    public static int NumberOfCodingGoalsToSeed => Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["NumberOfCodingGoalsToSeed"]!);

}
