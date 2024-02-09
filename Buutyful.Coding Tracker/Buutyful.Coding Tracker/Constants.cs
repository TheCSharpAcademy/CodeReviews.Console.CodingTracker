namespace Buutyful.Coding_Tracker;

public static class Constants
{
    public static string ConnectionString => System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    public static string DateFormat => "yyyy-MM-dd HH:mm:ss";
}
