using CodingTracker.kmakai.Data;
using CodingTracker.kmakai.Models;
using ConsoleTableExt;

namespace CodingTracker.kmakai.Controllers;

public class CodeSessionController
{
    private readonly DbContext DbContext;
    private readonly ViewController ViewController = new();
    public List<CodeSession> CodeSessions = new();

    public CodeSessionController(DbContext dbContext)
    {
        DbContext = dbContext;
        CodeSessions = DbContext.GetCodeSessions();
    }

    public void CreateSession()
    {
        string? Date = InputController.GetDateInput();
        string StartTime = InputController.GetStartTimeInput();
        string EndTime = InputController.GetEndTimeInput(StartTime);

        CodeSession codeSession = new()
        {
            Date = Date,
            StartTime = StartTime,
            EndTime = EndTime
        };

        DbContext.Add(codeSession);
        codeSession.Id = DbContext.GetLastId();
        CodeSessions.Add(codeSession);
    }
    public void UpdateSession()
    {
        Console.Clear();
        ViewController.ViewSessions(CodeSessions);
        Console.WriteLine("Enter the id of the session you want to update: ");
        var id = InputController.GetIdInput();
        var codeSession = CodeSessions.FirstOrDefault(x => x.Id == id);

        if (codeSession is null)
        {
            Console.WriteLine("Session not found.");
            return;
        }

        Console.WriteLine("Session to be edited");
        ViewController.ViewSession(codeSession);

        string? Date = InputController.GetDateInput();
        string StartTime = InputController.GetStartTimeInput();
        string EndTime = InputController.GetEndTimeInput(StartTime);

        codeSession.Date = Date;
        codeSession.StartTime = StartTime;
        codeSession.EndTime = EndTime;

        DbContext.Update(codeSession);
    }

    public void DeleteSession()
    {
        ViewController.ViewSessions(CodeSessions);
        Console.WriteLine("Enter the id of the session you want to delete: ");
        var id = InputController.GetIdInput();
        DbContext.Delete(id);
        CodeSessions.RemoveAll(x => x.Id == id);
    }
}
