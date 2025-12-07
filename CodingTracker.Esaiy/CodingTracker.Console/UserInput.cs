using System.Globalization;
using Spectre.Console;

namespace CodingTracker
{
    public class UserInput
    {
        public static CodingSession PromptNewCodingSession(CodingSession? oldValue = null)
        {
            TextPrompt<string> startPrompt = new($"Enter Start Date Time ({DateFormats.DateDisplayFormat})");
            TextPrompt<string> endPrompt = new($"Enter End Date Time ({DateFormats.DateDisplayFormat})");

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            _ = startPrompt.DefaultValue(today.ToDateTime(TimeOnly.MinValue).ToString(DateFormats.DateStringFormat));
            _ = endPrompt.DefaultValue(today.ToDateTime(TimeOnly.MaxValue).ToString(DateFormats.DateStringFormat));

            if (oldValue != null)
            {
                _ = startPrompt.DefaultValue(oldValue.Start.ToString(DateFormats.DateStringFormat));
                _ = endPrompt.DefaultValue(oldValue.End.ToString(DateFormats.DateStringFormat));
            }

            string start = AnsiConsole.Prompt(startPrompt
                    .Validate(Validation.ValidateDateString));
            AnsiConsole.WriteLine(start.ToString());

            string end = AnsiConsole.Prompt(
                    endPrompt
                    .Validate(Validation.ValidateDateString)
                    .Validate((end) =>
                    {
                        return Validation.ValidateEndDate(end, start);
                    }));
            AnsiConsole.WriteLine(end.ToString());

            DateTime startDate = DateTime.ParseExact(start, DateFormats.DateStringFormat, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(end, DateFormats.DateStringFormat, CultureInfo.InvariantCulture);
            CodingSession obj = new(startDate, endDate);
            return obj;
        }

        public static CodingSession SelectCodingSession(List<CodingSession> selections)
        {
            CodingSession updateId = AnsiConsole.Prompt(
                    new SelectionPrompt<CodingSession>()
                    .Title("Select Coding Session")
                    .PageSize(10)
                    .MoreChoicesText("Move Up Or Down to Choose")
                    .UseConverter(static a => $"{a.Id} {a.Start.ToString(DateFormats.DateStringFormat)}\t| {DurationFormatter.DurationToHourString(a.Duration)}")
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
            TextPrompt<string> EndPrompt = new($"Enter End Date Time ({DateFormats.DateOnlyDisplayFormat})");

            string end = AnsiConsole.Prompt(EndPrompt
                    .Validate((end) =>
                    {
                        ValidationResult res = Validation.ValidateDateOnlyString(end);
                        if (!res.Successful)
                        {
                            return res;
                        }
                        DateOnly endDate = DateOnly.ParseExact(end, DateFormats.DateOnlyStringFormat, CultureInfo.InvariantCulture);
                        return Validation.ValidateEndDate(endDate.ToDateTime(TimeOnly.MaxValue), start.ToDateTime(TimeOnly.MinValue));
                    }));
            AnsiConsole.WriteLine(end.ToString());

            int duration = AnsiConsole.Prompt(new TextPrompt<int>("Enter Duration in Hour (Must Be a Round Number)").Validate(Validation.ValidatePositiveInteger));

            DateOnly endDate = DateOnly.ParseExact(end, DateFormats.DateOnlyStringFormat, CultureInfo.InvariantCulture);
            CodingGoal obj = new(start, endDate, duration);
            return obj;
        }
    }
}
