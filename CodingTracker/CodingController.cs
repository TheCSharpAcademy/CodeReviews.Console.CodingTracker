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
            List<CodingSession> codingSessions = database.Get();
            ShowCodingSessionTable(codingSessions);
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
                _ = readTable.AddRow(new Text(cs.Id.ToString()), new Text(cs.Start.ToString()), new Text(cs.End.ToString()), new Text(cs.Duration.ToString()));
            }

            AnsiConsole.Write(readTable);
        }
    }
}
