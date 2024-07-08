using CodingTracker.Dates;
using CodingTracker.Database;
using CodingTracker.Input;
using CodingTracker.Model;
using Spectre.Console;

namespace CodingTracker.UserInterface
{
    public static class UIHelper
    {

        public static void InitMainMenu()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[turquoise2]Welcome to my coding tracker![/]");
            AnsiConsole.MarkupLine("Press [lime]C[/] to [lime]create a record[/], [lime]U[/] to [lime]update[/], [lime]D[/] to [lime]delete[/], [lime]V[/] to [lime]view all[/], [lime]R[/] to [lime]record[/], and [lime]0[/] to [lime]exit[/]");

            var input = AnsiConsole.Ask<string>("[silver]What do you want to do?[/]");
            input = input.Trim().ToLower();

            switch (input)
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "c":
                    InitCreateMenu();
                    break;
                case "d":
                    InitDeleteMenu();
                    break;
                case "u":
                    InitUpdateMenu();
                    break;
                case "v":
                    Console.Clear();
                    PrintAllEntries(false);
                    break;
                case "r":
                    InitRecordMenu();
                    break;
                default:
                    AnsiConsole.MarkupLine("[red3_1]Input not recognized. Please try again[/]");
                    break;
            }
        }

        public static List<CodingSession>? PrintAllEntries(bool returnList)
        {
            var results = DatabaseController.GetList();

            var table = new Table();
            table.AddColumns(["ID", "Total", "Start", "End"]);

            foreach (var result in results)
            {
                var start = DateTime.Parse(result.Start!);
                var end = DateTime.Parse(result.End!);
                var (hours, minutes) = DateHelper.GetTotalTime(start, end);

                table.AddRow([result.ID.ToString()!, $"{hours}H {minutes}M", start.ToString(), end.ToString()]);
            }

            AnsiConsole.Write(table);

            if (returnList && results.Any()) return results.ToList();
            else if (!results.Any())
            {
                AnsiConsole.MarkupLine("[red3_1]No entries found[/]");
            }

            AnsiConsole.MarkupLine("[lime]Press any key to continue...[/]");
            Console.ReadKey();

            return null;
        }

        public static void InitCreateMenu()
        {
            Console.Clear();

            var result = InputHelper.GetAllDateTimes();

            if (result == null) return;

            var (date1, date2) = result.Value;
            DatabaseController.Insert(date1.ToString(), date2.ToString());
            AnsiConsole.MarkupLine("[lime]Session inserted successfully. Press any key to continue...[/]");
            Console.ReadKey();
        }

        public static void InitDeleteMenu()
        {
            while (true)
            {
                Console.Clear();
                var list = PrintAllEntries(true);

                if (list == null) return;

                AnsiConsole.MarkupLine("Please [lime]input the number ID[/] of the entry you would like to [red3_1]delete[/], or type [lime]0[/] to [lime]quit[/]");
                var index = InputHelper.CheckIndex(list);

                if (index == null) return;
                else if (index == -1) continue;

                AnsiConsole.MarkupLine("[silver]Are you sure you want to delete this entry?[/]");

                if (!AnsiConsole.Confirm("[lime]Enter Y to confirm[/] or [red3_1]N to go back[/]")) continue;

                DatabaseController.Delete(index.Value);
                AnsiConsole.MarkupLine("[lime]Session deleted successfully. Press any key to continue...[/]");
                Console.ReadKey();
            }
        }

        public static void InitUpdateMenu()
        {
            while (true)
            {
                Console.Clear();
                var list = PrintAllEntries(true);

                if (list == null) return;

                AnsiConsole.MarkupLine("Please [lime]input the number ID[/] of the entry you would like to [turquoise2]update[/], or type [lime]0 to quit[/]");
                var index = InputHelper.CheckIndex(list);

                if (index == null) return;
                else if (index == -1) continue;

                var result = InputHelper.GetAllDateTimes();

                if (result == null) return;

                AnsiConsole.MarkupLine("[silver]Are you sure you want to update this entry?[/]");

                if (!AnsiConsole.Confirm("[lime]Enter Y to confirm[/] or [red3_1]N to go back[/]")) continue;

                var (date1, date2) = result.Value;
                DatabaseController.Update(index.Value, date1.ToString(), date2.ToString());
                AnsiConsole.MarkupLine("[lime]Session updated successfully. Press any key to continue...[/]");
                Console.ReadKey();
            }
        }

        public static void InitRecordMenu()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[turquoise2]You can record your coding time here.[/]");

            if (!AnsiConsole.Confirm("[lime]Select Y to start recording or N to return[/]")) return;

            var startDate = DateHelper.GetCurrentDate();
            var startTime = DateHelper.GetCurrentTime();

            if (!AnsiConsole.Confirm("[lime]Select Y to stop recording or N to return without saving[/]")) return;

            var endDate = DateHelper.GetCurrentDate();
            var endTime = DateHelper.GetCurrentTime();

            DatabaseController.Insert($"{startDate} {startTime}", $"{endDate} {endTime}");
            AnsiConsole.MarkupLine("[lime]Session recorded successfully. Press any key to continue...[/]");
            Console.ReadKey();
        }
    }
}