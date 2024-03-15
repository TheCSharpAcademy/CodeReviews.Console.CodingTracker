using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CodingTracker.obitom67
{
    internal static class UserInput
    {
        public static bool ContinueRunning { get; set; }
        public static void AskUser()
        {
            string[] menuChoices = 
            {   "Close Application",
                "View All Records",
                "Insert Record",
                "Delete Record",
                "Update Record"
            };
            SelectionPrompt<string> menuPrompt = new SelectionPrompt<string>();
            menuPrompt.Title("Please select an option from the menu");
            menuPrompt.PageSize(5);
            menuPrompt.AddChoices(menuChoices);

            var menuSelection = AnsiConsole.Prompt(menuPrompt);
            switch (menuSelection)
            {
                case "Close Application":
                    AnsiConsole.WriteLine("Goodbye!");
                    UserInput.ContinueRunning = false;
                    break;
                case "View All Records":
                    DBHandling.ViewRecords();
                    break;
                case "Insert Record":
                    DBHandling.Insert();
                    AnsiConsole.Clear();
                    break;
                case "Delete Record":
                    DBHandling.Delete();
                    break;
                case "Update Record":
                    DBHandling.Update();
                    break;
                default:
                    AnsiConsole.Clear();
                    AnsiConsole.WriteLine("\n\nInvalid command please type one of the commands on the menu.");
                    AskUser();
                    break;

            }
        }
        
        
        public static string GetDateInput()
        {
            DateTime date = new DateTime();

            
            string inputValue = AnsiConsole.Ask<string>("\n\n Please insert the date and time: (Format: dd-MM-yy hh:mm).");

            
            while (!DateTime.TryParseExact(inputValue, "dd-MM-yy H:mm", new CultureInfo("en-US"), DateTimeStyles.None, out date))
            {
                
                inputValue = AnsiConsole.Ask<string>("\n\n Please insert the date and time: (Format: dd-MM-yy hh:mm).");
            }
            AnsiConsole.WriteLine($"You have entered {inputValue}");
            return inputValue;

        }

        public static int GetNumberInput(string message)
        {
            AnsiConsole.WriteLine(message);
            string inputString = AnsiConsole.Ask<string>("\n\nInsert a number\n\n");

            

            while (!Int32.TryParse(inputString, out int number) || Convert.ToInt32(inputString) < 0)
            {
            
                inputString = AnsiConsole.Ask<string>("\n\nInvalid number. Try again.\n\n");
            }

            int finalInput = Convert.ToInt32(inputString);

            return finalInput;
        }

        public static void ShowList(List<CodingSession> sessions)
        {
            
            List<Text> sessionTexts = new List<Text>();
            
            foreach (CodingSession session in sessions)
            {
                Text text = new Text($"{session.Id} | {session.StartTime} | {session.EndTime} | {session.Duration}");
                sessionTexts.Add(text);
            }
            sessionTexts.Add(new Text("\n"));
            AnsiConsole.Write( new Rows(sessionTexts)); 
        }
    }
}
