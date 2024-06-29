using Application.Entities;
using DatabaseLibrary;
using Spectre.Console;
using System.Globalization;


namespace CodingTracker.ukpagrace
{
    internal class TrackerController
    {
        Database database = new ();
        UserInput userInput = new ();
        Utility utility = new();
        Validation validate = new();
        bool stopSession;
        public void CreateTable()
        {
            database.Create();
        }


        public async void InsertRecord()
        {
            DateTime startDate = userInput.GetStartDate();
            DateTime endDate = userInput.GetEndDate();

            while (startDate > endDate)
            {
                AnsiConsole.MarkupLine("[bold yellow]start date cannot be greater than end date[/]");
                endDate = userInput.GetEndDate();

            }
            TimeSpan duration = utility.GetDuration(startDate, endDate);
            int affectedRows = await database.Insert(startDate, endDate, duration);
            AnsiConsole.MarkupLine($"[white]{affectedRows} [yellow]row(s)[/] inserted[/]");

        }

        public void ListRecords()
        {
            var records = database.List();

            var table = new Table();

            table.Title("[blue]Coding Tracker[/]").Centered();
            table.AddColumn(new TableColumn("[red]Id[/]").Centered());
            table.AddColumn(new TableColumn("[blue]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[blue]Duration[/]").Centered());

            foreach (UserEntity record in records)
            {
                TimeSpan duration = TimeSpan.Parse(record.Duration);
                table.AddRow(record.Id.ToString(), record.StartDate, record.EndDate, utility.FormatTimeSpan(duration));
            }

            AnsiConsole.Write(table);
        }

        public void UpdateRecord()
        {
            ListRecords();
            int id = userInput.GetNumberInput("Enter the Id your want to update");

            UserEntity record = database.GetOne(id);
            if(record == null)
            {
                AnsiConsole.MarkupLine($"[red]Record with {id} does not exist[/]");
            }
            else
            {
                var table = new Table();
                table.Title("[blue]Record[/]").Centered();

                table.AddColumn(new TableColumn("[red]Id[/]").Centered());
                table.AddColumn(new TableColumn("[red]StartDate[/]").Centered());
                table.AddColumn(new TableColumn("[red]EndDate[/]").Centered());
                table.AddColumn(new TableColumn("[red]Duration[/]").Centered());

                table.AddRow(record.Id.ToString(), record.StartDate, record.EndDate, record.Duration);
                AnsiConsole.Write(table);
                var prompt = new SelectionPrompt<string>()
                    .Title("What column do you want to update")
                    .AddChoices(new[]
                    {
                    "StartDate", "EndDate"
                    });
                var option = AnsiConsole.Prompt(prompt);

                if (option == "StartDate")
                {

                    UpdateStartDate(id, record);
                }
                else if (option == "EndDate")
                {
                    UpdateEndDate(id, record);
                }
            }

        }

        async void UpdateStartDate(int id, UserEntity record)
        {
            DateTime startDate = userInput.GetStartDate();
            DateTime endDate;

            DateTime.TryParseExact(record.EndDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

            while (startDate > endDate)
            {
                AnsiConsole.MarkupLine("[red]Start Date cannot greater than End Date[/]");
                startDate = userInput.GetStartDate();
            }

            TimeSpan duration = utility.GetDuration(startDate, endDate);

            int affectedRows = await database.Update(id, startDate, endDate, duration);
            AnsiConsole.MarkupLine($" [white]{affectedRows}[blue] row(s)[/] updated[/]");
        }

        async void UpdateEndDate(int id, UserEntity record)
        {
            DateTime endDate = userInput.GetEndDate();
            DateTime startDate;

            DateTime.TryParseExact(record.StartDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            while (endDate < startDate)
            {
                AnsiConsole.MarkupLine("[red]End Date cannot be less than Start Date[/]");
                endDate = userInput.GetEndDate();
            }

            TimeSpan duration = utility.GetDuration(startDate, endDate);
            int affectedRows = await database.Update(id, startDate, endDate, duration);
            AnsiConsole.MarkupLine($"[white]{affectedRows}[blue]row(s) updated");
        }
        public async void DeleteRecord()
        {
            ListRecords();
            int id = userInput.GetNumberInput("Enter the Id your want to delete");
            int affectedRows = await database.Delete(id);
            AnsiConsole.MarkupLine($" [white]{affectedRows}[/] [blue]row(s) deleted[/]");
        }

        public void FilterRecords()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a filter option")
                    .PageSize(3)
                    .AddChoices("Year", "Month", "Day")
            );

            switch (option.ToLower())
            {
                case "year":
                    FilterTable("%Y", "year", "yyyy");
                    break;
                case "month":
                    FilterTable("%Y-%m", "month", "yyyy-MM");
                    break;
                case "day":
                    FilterTable("%Y-%m-%d", "day", "yyyy-MM-dd");
                    break;
                default:
                    Console.WriteLine("InvalidInput, Select an option from menu");
                    FilterRecords();
                    break;
            }
        }


        public void FilterTable(string format, string filter, string filterFormat)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Range or specific {filter}")
                    .PageSize(3)
                    .AddChoices("Range", $"Specific {filter}")
            );
            if (option == "Range")
            {
                var rangeInput = userInput.GetRange(filterFormat);
                var firstRange = rangeInput.Item1;
                var secondRange = rangeInput.Item2;


                if(validate.ValidateRange(firstRange, secondRange, filterFormat))
                {
                    AnsiConsole.MarkupLine($"[red]start Date Cannot be greater than end Date[/]");
                    return;
                }
                string order = userInput.GetOrderInput();
                List<UserEntity> records = database.Filter(format, firstRange, secondRange, order);
                DisplayFilteredRecords( records );

            }
            else
            {
                var specificValue = userInput.DateInput(filterFormat);
                string order = userInput.GetOrderInput();
                List<UserEntity> records = database.Filter(format, specificValue, order);
                DisplayFilteredRecords( records );
            }
        }

        public void DisplayFilteredRecords(List<UserEntity> records)
        {
            var table = new Table();

            table.Title("[blue]Coding Tracker[/]").Centered();
            table.AddColumn(new TableColumn("[red]Id[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[blue]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Duration[/]").Centered());
            foreach (UserEntity record in records)
            {
                table.AddRow(record.Id.ToString(), record.StartDate, record.EndDate, record.Duration);
            }

            AnsiConsole.Write(table);
        }

        public async void LiveCodingSession()
        {
            DateTime startDate = DateTime.Now;
            DateTime interval;
            Thread inputThread = new Thread(StopWatch);
            inputThread.Start();

            while (!stopSession)
            {
                interval = DateTime.Now;
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;

                Console.SetCursorPosition(0, 0);
                AnsiConsole.MarkupLine($"[red]TIME[/] [grey50]{interval.ToString("yyyy-MM-dd hh:mm:ss")}[/]");
                
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Thread.Sleep(1000);
            }
            DateTime endDate = DateTime.Now;
            TimeSpan duration = utility.GetDuration(startDate, endDate);

            int affectedRows = await database.Insert(startDate, endDate, duration);
            AnsiConsole.Write($"[white]{affectedRows}[/][blue]row(s) inserted[/]");
        }

        public void StopWatch()
        {
            while (!stopSession)
            {
                var input = userInput.ConfirmAction("Stop Live session");

                if (input)
                {
                    stopSession = true;
                }
            }
        }

        public void GetReport()
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                string month = $"{dateTime.Year}-{dateTime.Month:D2}";

                var results = database.Analyze("%Y-%m", month);

                string totalString = utility.FormatTimeSpan(results.Item1);
                string averageString = utility.FormatTimeSpan(results.Item2);

                AnsiConsole.MarkupLine($"[blue]This month you spent a total of {totalString} coding and an average of {averageString}[/]");

                AnsiConsole.Write(
                new FigletText("Weldone")
                    .Centered()
                    .Color(Color.Chartreuse3));
            }
            catch (Exception e) {
                AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
            
            }

        }
    }
}
