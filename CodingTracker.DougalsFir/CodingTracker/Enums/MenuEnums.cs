public enum MainMenuOption
{
    StartNewSession,
    LogManualSession,
    ViewAndEditPreviousSessions,
    ViewAndEditGoals,
    ViewReports,
    SeedDatabase,
    Exit
}

public enum StartSessionMenuOptions
{
    StartSession,
    RefreshElapsedTime,
    EndCurrentSession,
    ReturnToMainMenu
}

public enum LogManualSessionMenuOptions
{
    LogManualExternalSession,
    ReturnToMainMenu
}

public enum ManageSessionsMenuOptions
{
    ViewAllSessions,
    DeleteSessionRecord,
    DeleteAllSessions,
    ReturnToMainMenu
}

public enum ManageGoalsMenuOptions
{
    ShowProgress,
    SetNewGoal,
    DeleteGoal,
    DeleteAllGoals,
    ReturnToMainMenu
}
