namespace CodingTracker;

public static class UserOption
{
    // Option for the main menu selection prompt
    public enum MainMenu
    {
        Insert,
        InsertLive,
        View,
        Quit
    }

    // Option for the view menu selection prompt
    public enum ViewMenu
    {
        Previous,
        Next,
        Quit
    }
    
    // Different prompt that can be shown
    public enum PromptMenus
    {
        Main,
        Add,
        AddLive,
        View
    }

    public static string MainMenuToString(MainMenu option)
    {
        return option switch
        {
            MainMenu.Insert => "Insert a new coding session",
            MainMenu.InsertLive => "Start a live recording session",
            MainMenu.View => "View other months in the calendar",
            MainMenu.Quit => "Quit the app",
            _ => option.ToString()
        };
    }

    public static string ViewMenuToString(ViewMenu option)
    {
        return option switch
        {
            ViewMenu.Previous => "Previous month",
            ViewMenu.Next => "Next month",
            ViewMenu.Quit => "Go back to the menu",
            _ => option.ToString()
        };
    }
}