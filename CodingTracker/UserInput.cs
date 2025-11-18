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

        public static Enums.SessionFilter SelectFilter()
        {
            Enums.SessionFilter filter = AnsiConsole.Prompt(
                    new SelectionPrompt<Enums.SessionFilter>()
                    .Title("what filter")
                    .PageSize(10)
                    .MoreChoicesText("move up or down to choose")
                    .AddChoices(Enum.GetValues<Enums.SessionFilter>())
                    );
            return filter;
        }

        public static Enums.SessionOrder SelectOrder()
        {
            Enums.SessionOrder order = AnsiConsole.Prompt(
                    new SelectionPrompt<Enums.SessionOrder>()
                    .Title("what order")
                    .PageSize(10)
                    .MoreChoicesText("move up or down to choose")
                    .AddChoices(Enum.GetValues<Enums.SessionOrder>())
                    );
            return order;
        }

        public static CodingGoal PromptNewCodingGoal()
        {
            DateOnly start = DateOnly.FromDateTime(DateTime.Now);
            TextPrompt<DateOnly> EndPrompt = new("end date(mm/dd/yy)");

            DateOnly end = AnsiConsole.Prompt(EndPrompt.Validate((end) =>
            {
                return Validation.ValidateEndDate(end.ToDateTime(TimeOnly.MaxValue), start.ToDateTime(TimeOnly.MinValue));
            }));
            AnsiConsole.WriteLine(end.ToString());

            int duration = AnsiConsole.Prompt(new TextPrompt<int>("duration in hour? must be round number").Validate(Validation.ValidatePositiveInteger));

            CodingGoal obj = new(start, end, duration);
            return obj;
        }
    }
}
