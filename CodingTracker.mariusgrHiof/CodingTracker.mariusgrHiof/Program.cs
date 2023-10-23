using CodingTracker.Controllers;
using CodingTracker.Data;
using CodingTracker.Models;
using CodingTracker.Utils;
using ConsoleTableExt;

DbManager dbManager = new DbManager();
CodingController codingController = new CodingController(dbManager);
UserInput userInput = new UserInput();
bool keepGoing = true;

while (keepGoing)
{
    Console.WriteLine(@"
    What would you like to do?
    Type 0 to Close Application.
    Type 1 to View All Records.
    Type 2 to Insert Record.
    Type 3 to Update Record.
    Type 4 to Delete Record");

    string? choice = userInput.GetMenuChoice();
    Console.Clear();
    switch (choice)
    {

        case "0":
            Console.WriteLine("Closing app");
            keepGoing = false;
            break;
        case "1":
            ProcessGetAllSessions();
            break;
        case "2":
            ProcessAdd();
            break;
        case "3":
            ProcessUpdate();
            break;
        case "4":
            ProcessDelete();
            break;
        default:
            Console.WriteLine("Invalid choice.Try again.");
            break;
    }
}

void ProcessDelete()
{
    bool isValidId = false;
    string inputId = string.Empty;
    int id = 0;
    CodingSession session = null;

    while (isValidId == false)
    {
        ProcessGetAllSessions();
        Console.Write("Enter an id: ");
        inputId = Console.ReadLine();
        if (inputId.Equals("0"))
        {
            return;
        }
        if (!Validation.ValidateId(inputId))
        {
            continue;
        }
        id = Convert.ToInt32(inputId);

        session = codingController.GetSessionById(id);

        if (session == null)
        {
            Console.WriteLine("Can't find session.");
            continue;
        }
        isValidId = true;
    }

    var deleteSession = codingController.DeleteSession(id);
    if (deleteSession == null)
    {
        Console.WriteLine("Fail to delete");
    }
    else
    {
        Console.WriteLine("Session deleted");
    }
}

void ProcessUpdate()
{
    bool isValidId = false;
    string inputId = string.Empty;
    int id = 0;
    CodingSession session = null;
    while (isValidId == false)
    {
        ProcessGetAllSessions();
        Console.Write("Enter an id: ");
        inputId = Console.ReadLine();
        if (inputId.Equals("0"))
        {
            return;
        }
        if (!Validation.ValidateId(inputId))
        {
            continue;
        }
        id = Convert.ToInt32(inputId);
        session = codingController.GetSessionById(id);
        if (session == null)
        {
            Console.WriteLine("Can't find session.");
            continue;
        }

        isValidId = true;

    }
    bool isValidStartDate = false;
    string? startDate = string.Empty;
    string? endDate = string.Empty;

    while (isValidStartDate == false)
    {
        startDate = userInput.GetDateInput();
        if (startDate.Equals("0"))
        {
            return;
        }
        if (Validation.ValidateDate(startDate) == true)
        {
            isValidStartDate = true;
        }
        else
        {
            Console.WriteLine("Invalid date.Try again");
        }
    }
    bool isValidEndDate = false;

    while (isValidEndDate == false)
    {
        endDate = userInput.GetDateInput();
        if (endDate.Equals("0"))
        {
            return;
        }
        if (Validation.ValidateDate(endDate) == true)
        {
            isValidEndDate = true;
        }
        else
        {
            Console.WriteLine("Invalid date.Try again");
        }
    }
    codingController.UpdateSession(id, new CodingSession { Id = id, StartTime = startDate, EndTime = endDate });
}

void ProcessGetAllSessions()
{
    var tableData = new List<List<object>>();
    var sessions = codingController.GetAllSessions();

    foreach (var session in sessions)
    {
        tableData.Add(new List<object> { session.Id, DateTime.Parse(session.StartTime), DateTime.Parse(session.EndTime), session.CalculateDuration() });
    }

    ConsoleTableBuilder
    .From(tableData)
    .WithColumn("Id", "Start Date", "End Date", "Duration")
    .ExportAndWriteLine();
}

void ProcessAdd()
{
    bool isValidStartDate = false;
    string? startDate = string.Empty;
    string? endDate = string.Empty;

    while (isValidStartDate == false)
    {
        startDate = userInput.GetDateInput();
        if (startDate.Equals("0"))
        {
            return;
        }
        if (Validation.ValidateDate(startDate) == true)
        {
            isValidStartDate = true;
        }
        else
        {
            Console.WriteLine("Invalid date.Try again");
        }
    }

    bool isValidEndDate = false;

    while (isValidEndDate == false)
    {
        endDate = userInput.GetDateInput();
        if (endDate.Equals("0"))
        {
            return;
        }
        if (Validation.ValidateDate(endDate) == true && Validation.ValidateDateRange(DateTime.Parse(startDate), DateTime.Parse(endDate)))
        {
            isValidEndDate = true;
        }
        else
        {
            Console.WriteLine("Invalid date.Try again");
        }
    }

    codingController.CreateSession(new CodingSession
    {
        StartTime = startDate,
        EndTime = endDate,
    });
}
