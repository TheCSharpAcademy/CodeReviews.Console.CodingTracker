using CodingTracker.kwm0304.Enums;
using CodingTracker.kwm0304.Models;

namespace CodingTracker.kwm0304.Views;

public class SelectionPrompt
{
  public static string genericPrompt = "What would you like to do?";
  public static string MainMenu()
  {
    var menuOptions = new List<string> { "Sessions", "Goals", "Reports", "Exit" };
    var menu = new Menu<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static string CodingSessionsMenu()
  {
    var menuOptions = new List<string> { "Create", "View", "Edit", "Delete", "Back" };
    var menu = new Menu<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static CodingSession ViewCodingSessionsMenu(List<CodingSession> sessions)
  {
    var menu = new Menu<CodingSession>("Select a coding session if you want to edit or delete", sessions);
    return menu.Show();
  }

  public static string ViewGoalsOptions()
  {
    var menuOptions = new List<string> { "View all goals", "View active goals", "Back" };
    var menu = new Menu<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static string ChangeEndTimeOptions()
  {
    var menuOptions = new List<string> { "Increase", "Decrease" };
    var menu = new Menu<string>("Dou you need to increase or decrease your previous ending time?", menuOptions);
    return menu.Show();
  }

  public static string TimeOptions()
  {
    var menuOptions = new List<string> { "Seconds", "Minutes", "Hours" };
    var menu = new Menu<string>("", menuOptions);
    return menu.Show();
  }

  public static string GoalsMenu()
  {
    var menuOptions = new List<string> { "Create new goal", "View goals", "Back" };
    var menu = new Menu<string>(genericPrompt, menuOptions);
    return menu.Show();
  }

  public static DateRange ReportsMenu()
  {
    var menuOptions = Global.RangeList;
    var menu = new Menu<DateRange>("What range should the report cover?", menuOptions);
    return menu.Show();
  }

  public static DateRange SelectGoalType()
  {
    var menuOptions = Global.RangeList;
    var menu = new Menu<DateRange>("What type of goal would you like to create?", menuOptions);
    return menu.Show();
  }
}