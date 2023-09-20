using CodingTracker.Mo3ses.Controller;
using CodingTracker.Mo3ses.Data;
using CodingTracker.Mo3ses.Interface;
using CodingTracker.Mo3ses.Models;
using CodingTracker.Mo3ses.UserMenu;

internal class Program
{
    private static void Main(string[] args)
    {
        DbConnect dbConnect = new();
        CodingSessionController sessionController = new CodingSessionController(dbConnect);
        UserInput user = new UserInput(sessionController);

        user.Execute();

    }
}