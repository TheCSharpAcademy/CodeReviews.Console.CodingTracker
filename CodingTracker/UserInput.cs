using Spectre.Console;

namespace CodingTracker
{
    public class UserInput
    {
        public static CodingSession PromptNewCodingSession(CodingSession? oldValue = null)
        {
            TextPrompt<DateTime> startPrompt = new("start date(dd/mm/yy hh:ii)");
            TextPrompt<DateTime> endPrompt = new("start date(mm/dd/yy hh:ii)");

            if (oldValue != null)
            {
                _ = startPrompt.DefaultValue(oldValue.Start);
                _ = endPrompt.DefaultValue(oldValue.End);
            }

            DateTime start = AnsiConsole.Prompt(startPrompt);
            AnsiConsole.WriteLine(start.ToString());

            DateTime end = AnsiConsole.Prompt(endPrompt.Validate((end) =>
            {
                return Validation.ValidateEndDate(end, start);
            }));
            AnsiConsole.WriteLine(end.ToString());

            CodingSession obj = new(start, end);
            return obj;
        }

        public static CodingSession SelectCodingSession(List<CodingSession> selections)
        {
            CodingSession updateId = AnsiConsole.Prompt(
                    new SelectionPrompt<CodingSession>()
                    .Title("what do?")
                    .PageSize(10)
                    .MoreChoicesText("move up or down to choose")
                    .UseConverter(static a => $"{a.Id} {a.Start} {a.Duration}")
                    .AddChoices(selections)
                    );
            return updateId;
        }

        public static bool Confirm()
        {
            bool confirmation = AnsiConsole.Prompt(
                    new TextPrompt<bool>("u sure?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .DefaultValue(true)
                    // TODO: why static good?
                    .WithConverter(static choice => choice ? "y" : "n")
                    );
            return confirmation;
        }
    }
}
