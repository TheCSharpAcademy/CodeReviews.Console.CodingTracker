using Spectre.Console;
using CodingTracker;
using System.Diagnostics;
using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConConfig
{
    class Program
    {
        private static bool openApp = true;
        static void Main(string[] args)
        {
            DataBase.Connect();
            WelcomeMessage();

            do
            {
                char option = GetOption();
                switch (option)
                {
                    case '1':
                        WelcomeMessage();
                        OpenShowWindow();
                        break;
                    case '2':
                        WelcomeMessage();
                        OpenInsertWindow();
                        break;
                    case '3':
                        WelcomeMessage();
                        OpenUpdateWindow();
                        break;
                    case '4':
                        WelcomeMessage();
                        OpenDeleteWindow();
                        break;
                    case '5':
                        WelcomeMessage();
                        OpenStopWatchWindow();
                        break;
                    case '0':
                        openApp = false;
                        Console.WriteLine("Exit Program");
                        break;
                    default:
                        Console.WriteLine("please Enter valid input");
                        break;
                }
            } while (openApp);

        }
        private static void WelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine($@"       Welcome To Coding Tracker
+++++++++++++++++++++++++++++++++++++++++");
        }
        static char GetOption()
        {
            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("What would you like to do?")
                            .AddChoices(new[]
                            {
                                "1 - Show all records",
                                "2 - Insert new record",
                                "3 - Update record",
                                "4 - Delete record",
                                "5 - Stop Watch",
                                "0 - exit"
                            }));
            return option[0];
        }
        private static void OpenShowWindow()
        {
            CodingSessionController controller = new CodingSessionController();
            var records = controller.GetAllRecords();
            var table = new Table();
           
            table.AddColumn(new TableColumn("ID").Centered());
            table.AddColumn(new TableColumn("Date").Centered());
            table.AddColumn(new TableColumn("Start At").Centered());
            table.AddColumn(new TableColumn("End At").Centered());
            table.AddColumn(new TableColumn("Duration\n(hh:mm:ss)").Centered());
           
            foreach (var record in records)
            {
                table.AddRow($"{record.Id}", record.Date.ToString("MM/dd/yyyy"), record.StartAt.ToString("HH:mm"), record.EndAt.ToString("HH:mm"), $"{record.Duration}");
            }
            AnsiConsole.Write(table);
        }
        private static void OpenInsertWindow()
        {
            string date = Validations.GetValidatedDate("Please Enter Date in Excat date format (MM/dd/yyyy) : ");
            string startAt = Validations.GetValidatedTime("Please Enter Time in Excat time format (HH:mm) : ");
            string endAt = Validations.GetValidatedTime("Please Enter Time in Excat time format (HH:mm) : ");
            
            var controller = new CodingSessionController();
            controller.Insert(date , startAt , endAt);

            // display success message
            Console.WriteLine();
            AnsiConsole.MarkupLine("[green]Record has been inserted successfully[/]");
            Console.WriteLine();
        }
        private static void OpenUpdateWindow()
        {
            OpenShowWindow();
            int id = Validations.GetValidatedInteger("Enter record Id you want to update");

            if(!DataBase.IsExist(id))
            {
                AnsiConsole.MarkupLine($"[red]Couldn't find record with id {id} , Please valid ID[/]");
                OpenUpdateWindow();
            }
            Console.WriteLine("-------------------------------------------------\n");
            string date = Validations.GetValidatedDate("Please Enter Date in Excat date format (MM/dd/yyyy) : ");
            string startAt = Validations.GetValidatedTime("Please Enter Time in Excat time format (HH:mm) : ");
            string endAt = Validations.GetValidatedTime("Please Enter Time in Excat time format (HH:mm) : ");

            var controller = new CodingSessionController();

            controller.Update(id , date, startAt, endAt);

            // display success message
            Console.WriteLine();
            AnsiConsole.MarkupLine("[green]Record has been Updated successfully[/]");
            Console.WriteLine();
        }
        private static void OpenDeleteWindow()
        {
            OpenShowWindow();
            int id = Validations.GetValidatedInteger("Enter record Id you want to Delete");

            if (!DataBase.IsExist(id))
            {
                AnsiConsole.MarkupLine($"[red]Couldn't find record with id {id} , Please valid ID[/]");
                OpenDeleteWindow();
            }

            var controller = new CodingSessionController();
            controller.Delete(id);

            // display success message
            Console.WriteLine();
            AnsiConsole.MarkupLine("[green]Record has been Deleted successfully[/]");
            Console.WriteLine();
        }
        private static void OpenStopWatchWindow()
        {
            
            Stopwatch stopWatch = new Stopwatch();
            DateTime startAt = DateTime.Now;
            stopWatch.Start();
            Console.WriteLine($"starts : {startAt}");
            Console.WriteLine("counting...");
            Console.Write("Press any key to stop : ");
            Console.ReadKey();
            stopWatch.Stop();
            DateTime endAt = DateTime.Now;

            var controller = new CodingSessionController();
            controller.Insert(DateTime.Now.Date.ToString("MM/dd/yyyy") , startAt.ToString("HH:mm"), endAt.ToString("HH:mm"));

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = System.String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("press any key to continue ");
            Console.ReadKey();
        }
    }
}