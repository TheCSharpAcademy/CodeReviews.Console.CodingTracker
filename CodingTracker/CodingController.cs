using Spectre.Console;

namespace CodingTracker
{
    public class CodingController(Database db)
    {
        private readonly Database database = db;

        public void Create()
        {
            CodingSession obj = UserInput.PromptNewCodingSession();
            database.Save(obj);

            AnsiConsole.WriteLine("saved");
        }

        public void Read()
        {
            AnsiConsole.WriteLine("filter?");
            bool confirmation = UserInput.Confirm();
            Enums.SessionFilter? filter = null;
            if (confirmation)
            {
                filter = UserInput.SelectFilter();
            }

            AnsiConsole.WriteLine("order?");
            confirmation = UserInput.Confirm();
            Enums.SessionOrder? order = null;
            if (confirmation)
            {
                order = UserInput.SelectOrder();
            }

            List<FilteredCodingSession> filteredCodingSessions;
            switch (filter)
            {
                case Enums.SessionFilter.Day:
                    filteredCodingSessions = database.GetGroupByDay(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                case Enums.SessionFilter.Week:
                    filteredCodingSessions = database.GetGroupByWeek(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                case Enums.SessionFilter.Year:
                    filteredCodingSessions = database.GetGroupByYear(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                default:
                    List<CodingSession> codingSessions = database.Get(order);
                    ShowCodingSessionTable(codingSessions);
                    break;
            }
        }

        public void Update()
        {
            List<CodingSession> codingSessions = database.Get();
            if (codingSessions.Count == 0)
            {
                AnsiConsole.WriteLine("There is no record of coding session");
                return;
            }

            CodingSession selectedSession = UserInput.SelectCodingSession(codingSessions);

            CodingSession updatedSession = UserInput.PromptNewCodingSession(selectedSession);
            selectedSession.Start = updatedSession.Start;
            selectedSession.End = updatedSession.End;
            selectedSession.UpdateDuration();

            database.Update(selectedSession);
        }

        public void Delete()
        {
            List<CodingSession> updateData = database.Get();
            CodingSession deletedId = UserInput.SelectCodingSession(updateData);
            ShowCodingSessionTable([deletedId]);
            bool confirmation = UserInput.Confirm();
            database.Delete(deletedId);

            AnsiConsole.WriteLine(confirmation);
        }

        public void Start()
        {
            CodingSession? codingSession = null;
            DateTime start = DateTime.Now;

            bool confirmation = false;
            while (!confirmation)
            {
                AnsiConsole.WriteLine("Happy Coding");
                AnsiConsole.WriteLine("Press any key to stop");
                _ = Console.ReadKey();

                DateTime end = DateTime.Now;
                codingSession = new(start, end);
                AnsiConsole.WriteLine($"You have coded for {codingSession.DurationToString()}, Confirm to stop");
                confirmation = UserInput.Confirm();
            }

            if (codingSession != null)
            {
                database.Save(codingSession);
            }
        }

        private static void ShowCodingSessionTable(List<CodingSession> codingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("id")
            .AddColumn("start")
            .AddColumn("end")
            .AddColumn("duration");
            foreach (CodingSession cs in codingSessions)
            {
                // TODO: handle locale thing in one place
                cs.UpdateDuration();
                _ = readTable.AddRow(new Text(cs.Id.ToString()), new Text(cs.Start.ToString()), new Text(cs.End.ToString()), new Text(cs.DurationToStringComplete()));
            }

            AnsiConsole.Write(readTable);
        }

        private static void ShowFilteredCodingSessionTable(List<FilteredCodingSession> filteredCodingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("filter id")
                .AddColumn("total duration");
            foreach (FilteredCodingSession fcs in filteredCodingSessions)
            {
                // TODO: handle locale thing in one place
                _ = readTable.AddRow(new Text(fcs.FilterId), new Text(fcs.DurationToStringComplete()));
            }

            AnsiConsole.Write(readTable);
        }
    }
}
