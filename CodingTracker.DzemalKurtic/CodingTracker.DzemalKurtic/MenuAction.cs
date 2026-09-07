using System.ComponentModel;

namespace CodingTracker.DzemalKurtic;

internal enum MenuAction
{
    [Description("View Session")]
    ViewSessions,
    AddSession,
    UpdateSession,
    DeleteSession
}
