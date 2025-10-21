using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;

namespace CodingTracker;

public static class CodingController
{
    static void Save(CodingSession session)
    {
        // connect
        var sAll = ConfigurationManager.AppSettings;
        var connectionString = sAll.Get("ConnectionString");
        using var db = new SqliteConnection(connectionString);
        db.Open();
        var query = "insert into coding_sessions (start_time, end_time, duration) values(@Start, @End, @Duration)";

        // insert
        db.Execute(query, session);
    }
    static List<CodingSession> Get()
    {
        var sAll = ConfigurationManager.AppSettings;
        var connectionString = sAll.Get("ConnectionString");
        using var db = new SqliteConnection(connectionString);
        db.Open();
        // var records = db.Query<CodingSession>("select id, start_time as start, end_time as end, duration from coding_sessions").ToList();
        var records = db.Query<CodingSession>("select id, start_time as start, end_time as end, duration from coding_sessions").ToList();
        return records;
    }
    static void Update(CodingSession session)
    {
        var sAll = ConfigurationManager.AppSettings;
        var connectionString = sAll.Get("ConnectionString");
        using var db = new SqliteConnection(connectionString);
        db.Open();
        var query = "update coding_sessions set start_time = @Start, end_time = @End, duration = @Duration where id = @Id";
        db.Execute(query, session);
    }
    static void Delete(CodingSession session)
    {
        var sAll = ConfigurationManager.AppSettings;
        var connectionString = sAll.Get("ConnectionString");
        using var db = new SqliteConnection(connectionString);
        db.Open();
        var query = "delete from coding_sessions where id = @Id";
        db.Execute(query, session);

    }
    public static void Create()
    {
        var start = AnsiConsole.Prompt(new TextPrompt<DateTime>("start date(dd/mm/yy hh:ii)"));
        AnsiConsole.WriteLine(start.ToString());
        var end = AnsiConsole.Prompt(new TextPrompt<DateTime>("start date(mm/dd/yy hh:ii)").Validate((n) =>
        {
            if (n <= start)
            {
                return ValidationResult.Error("must be after start time");
            }
            else
            {
                return ValidationResult.Success();
            }
        }));
        AnsiConsole.WriteLine(end.ToString());
        //
        // make object
        var obj = new CodingSession(start, end);
        // save
        Save(obj);
        AnsiConsole.WriteLine("saved");
    }
    public static void Read()
    {
        var testData = Get();
        var readTable = new Table();
        readTable.AddColumn("id");
        readTable.AddColumn("start");
        readTable.AddColumn("end");
        readTable.AddColumn("duration");
        foreach (var item in testData)
        {
            readTable.AddRow(new Text(item.Id.ToString()), new Text(item.Start.ToString()), new Text(item.End.ToString()), new Text(item.Duration.ToString()));
        }

        // read
        // write to table
        AnsiConsole.Write(readTable);

    }
    public static void Update()
    {
        var updateData = Get();
        var updateId = AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                .Title("what do?")
                .PageSize(10)
                .MoreChoicesText("move up or down to choose")
                .UseConverter(static a => $"{a.Id} {a.Start} {a.Duration}")
                .AddChoices(updateData)
                );
        // get
        // start with default chosen if empty
        // end, same thing
        // TODO: handle empty
        updateId.Start = AnsiConsole.Prompt(new TextPrompt<DateTime>("start date(dd/mm/yy hh:ii)").DefaultValue(updateId.Start));
        AnsiConsole.WriteLine(updateId.Start.ToString());
        updateId.End = AnsiConsole.Prompt(new TextPrompt<DateTime>("start date(mm/dd/yy hh:ii)")
                .DefaultValue(updateId.End)
                .Validate((n) =>
        {
            if (n <= updateId.Start)
            {
                return ValidationResult.Error("must be after start time");
            }
            else
            {
                return ValidationResult.Success();
            }
        }));
        AnsiConsole.WriteLine(updateId.End.ToString());
        // update
        AnsiConsole.WriteLine($"{updateId.Id} {updateId.Start}");
        updateId.UpdateDuration();
        Update(updateId);
    }
    public static void Delete()
    {
        var updateData = Get();
        var deletedId = AnsiConsole.Prompt(
                new SelectionPrompt<CodingSession>()
                .Title("what do?")
                .PageSize(10)
                .MoreChoicesText("move up or down to choose")
                .UseConverter(static a => $"{a.Id} {a.Start} {a.Duration}")
                .AddChoices(updateData)
                );
        // show what it is
        // TODO: show table
        // delete confirm
        var confirmation = AnsiConsole.Prompt(
                new TextPrompt<bool>("u sure?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n")
                );
        Delete(deletedId);
        AnsiConsole.WriteLine(confirmation);

    }
}
