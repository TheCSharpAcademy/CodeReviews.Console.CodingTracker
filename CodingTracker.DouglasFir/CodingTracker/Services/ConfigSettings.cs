using System.Configuration;

namespace CodingTracker.Services;

public class ConfigSettings
{
    public static string DbFilePath => ConfigurationManager.AppSettings["DatabaseFilePath"]!;
    public static string DbConnectionString => ConfigurationManager.AppSettings["DatabaseConnectionString"]!;
    public static string DateFormatShort => ConfigurationManager.AppSettings["DateFormatShort"]!;
    public static string DateFormatLong => ConfigurationManager.AppSettings["DateFormatLong"]!;
    public static string TimeFormatType => ConfigurationManager.AppSettings["TimeFormatType"]!;
    public static string TimeFormatString => ConfigurationManager.AppSettings["TimeFormatString"]!;
    public static int NumberOfCodingSessionsToSeed => Int32.Parse(ConfigurationManager.AppSettings["NumberOfCodingSessionsToSeed"]!);
    public static int NumberOfCodingGoalsToSeed => Int32.Parse(ConfigurationManager.AppSettings["NumberOfCodingGoalsToSeed"]!);

}
