using Buutyful.Coding_Tracker.Command;

namespace Buutyful.Coding_Tracker;

public static class Constants
{
    public static string ConnectionString => System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    public static string DateFormat => "yyyy-MM-dd HH:mm:ss";
    public static Dictionary<Commands, string> MapCommands => new()
        {
            {Commands.Info, "Gets you all the infos u need" },
            {Commands.Menu, "Return to the main menu"},
            {Commands.View, "Display database records"},
            {Commands.Create, "Create new entry" },
            {Commands.Update, "Update record" },
            {Commands.Delete, "Delete record" },
            {Commands.Back, "Goes back to past state" },
            {Commands.Forward, "Goes to forward state" },
            {Commands.Clear, "Clear console"},
            {Commands.Break, "Breaks out of input loop"},
            {Commands.Quit, "Quit the application" }
        };
}
