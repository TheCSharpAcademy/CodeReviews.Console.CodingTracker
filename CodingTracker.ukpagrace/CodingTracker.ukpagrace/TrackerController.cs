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
        bool stopSession = false;
        void CreateTable()
        {
            database.Create();
        }


        public async void InsertTable()
        {
            DateTime startDate = userInput.GetStartDate();
            DateTime endDate = userInput.GetEndDate();

            while (startDate > endDate)
            {
                Console.WriteLine("start date cannot be greater than end date");
                endDate = userInput.GetEndDate();

            }
            TimeSpan duration = endDate - startDate;
            int affectedRows = await database.Insert(startDate, endDate, duration);
            AnsiConsole.WriteLine($"[white]{affectedRows} [yellow]row(s) inserted");

        }

        void ListRecords()
        {
            var records = database.List();

            var table = new Table();

            table.Title("[blue]Coding Tracker").Centered();
            table.AddColumn(new TableColumn("[red]Id[/]").Centered());
            table.AddColumn(new TableColumn("[red]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]Duration[/]").Centered());
            foreach (UserEntity record in records)
            {
                table.AddRow($"{record.Id}, {record.StartDate}, {record.EndDate}, {record.Duration}");
            }

            AnsiConsole.Write(table);
        }

        async void UpdateRecord()
        {
            ListRecords();
            int id = userInput.GetNumberInput("Enter the Id your want to update");

            UserEntity record = database.GetOne(id);
            var table = new Table();
            table.Title("[blue]Record").Centered();

            table.AddColumn(new TableColumn("[red]Id[/]").Centered());
            table.AddColumn(new TableColumn("[red]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]Duration[/]").Centered());

            table.AddRow($"{record.Id}, {record.StartDate}, {record.EndDate}, {record.Duration}");
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
                DateTime startDate = userInput.GetStartDate();
                DateTime endDate;

                DateTime.TryParseExact(record.EndDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

                while (startDate > endDate)
                {
                    AnsiConsole.Write("[red]Start Date cannot greater than Start Date[/]");
                    startDate = userInput.GetStartDate();
                }

                TimeSpan duration = utility.GetDuration(startDate, endDate);

                int affectedRows = await database.Update(id, startDate, endDate, duration);
                AnsiConsole.Write($"[white]{affectedRows}[blue]row(s) inserted");

            }
            else if (option == "EndDate")
            {
                DateTime endDate = userInput.GetEndDate();
                DateTime startDate;

                DateTime.TryParseExact(record.StartDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

                while (endDate < startDate)
                {
                    AnsiConsole.Write("[red]End Date cannot be less than Start Date[/]");
                    endDate = userInput.GetEndDate();
                }

                TimeSpan duration = endDate - startDate;
                int affectedRows = await database.Update(id, startDate, endDate, duration);
                AnsiConsole.Write($"[white]{affectedRows}[blue]row(s) inserted");
            }
        }

        public async void DeleteRecord()
        {
            ListRecords();
            int id = userInput.GetNumberInput("Enter the Id your want to delete");
            int affectedRows = await database.Delete(id);
            AnsiConsole.Write($"[white]{affectedRows}[blue]row(s) inserted");
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
                    FilterTable("%Y", "year", "yyyy eg 2024");
                    break;
                case "month":
                    FilterTable("%Y-%m", "months", "yyyy-mm eg 2024-06");
                    break;
                case "day":
                    FilterTable("%Y-%m-%d", "day", "yyyy-mm-dd eg 2024-06-24");
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
                    .PageSize(2)
                    .AddChoices("Range", $"Specific {filter}")
            );
            if (option == "1")
            {
                var firstRange = AnsiConsole.Ask<string>($"Enter the first range value in the format of [yellow]{filterFormat}[/]");
                var secondRange = AnsiConsole.Ask<string>($"Enter the second range value in the format of [yellow]{filterFormat}[/]");
                string order = userInput.GetOrderInput();
                List<UserEntity> records = database.Filter(format, firstRange, secondRange, order);
                DisplayFilteredRecords( records );

            }
            else
            {
                var specificValue = AnsiConsole.Ask<string>($"Enter the {filter} in the format of [yellow]{filterFormat}[/]");
                string order = userInput.GetOrderInput();
                List<UserEntity> records = database.Filter(format, specificValue, order);
                DisplayFilteredRecords( records );
            }
        }

        public void DisplayFilteredRecords(List<UserEntity> records)
        {
            var table = new Table();

            table.Title("[blue]Coding Tracker").Centered();
            table.AddColumn(new TableColumn("[red]Id[/]").Centered());
            table.AddColumn(new TableColumn("[red]StartDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]EndDate[/]").Centered());
            table.AddColumn(new TableColumn("[red]Duration[/]").Centered());
            foreach (UserEntity record in records)
            {
                table.AddRow($"{record.Id}, {record.StartDate}, {record.EndDate}, {record.Duration}");
            }

            AnsiConsole.Write(table);
        }

        public async void StartWatch()
        {
            DateTime startDate = DateTime.Now;
            DateTime interval;
            Console.WriteLine("To Stop watch enter 0");
            Thread inputThread = new Thread(StopWatch);
            inputThread.Start();

            while (!stopSession)
            {
                interval = DateTime.Now;
                Console.Clear();
                Console.WriteLine($"{interval.ToString("yyyy-MM-dd hh:mm:ss")}");
                Thread.Sleep(1000);
            }
            DateTime endDate = DateTime.Now;
            TimeSpan duration = utility.GetDuration(startDate, endDate);

            int affectedRows = await database.Insert(startDate, endDate, duration);
            AnsiConsole.Write($"[white]{affectedRows}[blue]row(s) inserted");
        }

        public void StopWatch()
        {
            while (!stopSession)
            {
                var input = Console.ReadLine();

                if (input == "0")
                {
                    stopSession = true;
                }
            }
        }


        void GetReport()
        {
            DateTime dateTime = DateTime.Now;
            string month = $"{dateTime.Year}-{dateTime.Month:D2}";

            database.Analyze("%Y-%m", month);
        }
    }
}
