using Spectre.Console;

namespace CodingTracker
{
    public class UserInput
    {
        public static CodingSession PromptNewCodingSession(CodingSession? oldValue = null)
        {
            TextPrompt<DateTime> startPrompt = new("Enter Start Date Time (mm/dd/yy hh:ii)");
            TextPrompt<DateTime> endPrompt = new("Enter End Date Time (mm/dd/yy hh:ii)");

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            _ = startPrompt.DefaultValue(today.ToDateTime(TimeOnly.MinValue));
            _ = endPrompt.DefaultValue(today.ToDateTime(TimeOnly.MaxValue));

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
                    .Title("Select Coding Session")
                    .PageSize(10)
                    .MoreChoicesText("Move Up Or Down to Choose")
                    .UseConverter(static a => $"{a.Id} {a.Start} {a.Duration}")
                    .AddChoices(selections)
                    );
            return updateId;
        }

        public static bool Confirm()
        {
            bool confirmation = AnsiConsole.Prompt(
                    new TextPrompt<bool>("Confirm?")
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
                    .Title("Choose Filter")
                    .PageSize(10)
                    .MoreChoicesText("Move Up Or Down to Choose")
                    .AddChoices(Enum.GetValues<Enums.SessionFilter>())
                    );
            return filter;
        }

        public static Enums.SessionOrder SelectOrder()
        {
            Enums.SessionOrder order = AnsiConsole.Prompt(
                    new SelectionPrompt<Enums.SessionOrder>()
                    .Title("Choose Order")
                    .PageSize(10)
                    .MoreChoicesText("Move Up Or Down to Choose")
                    .AddChoices(Enum.GetValues<Enums.SessionOrder>())
                    );
            return order;
        }

        public static CodingGoal PromptNewCodingGoal()
        {
            DateOnly start = DateOnly.FromDateTime(DateTime.Now);
            TextPrompt<DateOnly> EndPrompt = new("Enter End Date Time (mm/dd/yy hh:ii)");

            DateOnly end = AnsiConsole.Prompt(EndPrompt.Validate((end) =>
            {
                return Validation.ValidateEndDate(end.ToDateTime(TimeOnly.MaxValue), start.ToDateTime(TimeOnly.MinValue));
            }));
            AnsiConsole.WriteLine(end.ToString());

            int duration = AnsiConsole.Prompt(new TextPrompt<int>("Enter Duration in Hour (Must Be a Round Number)").Validate(Validation.ValidatePositiveInteger));

            CodingGoal obj = new(start, end, duration);
            return obj;
        }
    }
}
