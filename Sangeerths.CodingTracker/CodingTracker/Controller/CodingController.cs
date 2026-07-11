using CodingTracker.Service;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace CodingTracker.Controller
{
    public class CodingController
    {
        private readonly CodingTrackerService _service;
        public CodingController(CodingTrackerService service)
        {
            _service = service;
        }

       public void Start()
        {
            _service.CreateTables();
           
            bool exit = false;
            while (!exit)
            {
                _service.ShowHeader();
                string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                            "1. Insert Coding Session",
                            "2. View Coding Sessions",
                            "3. Update Coding Session",
                            "4. Delete Coding Session",
                            "5. Time Coding Session",
                            "6. Filter Coding Sessions",
                            "7. Exit"
                        }));
               char currentChoice = choice[0];
                switch (currentChoice)
                {
                    case '1':
                        _service.InsertCodingSession();
                        
                        break;
                    case '2':
                        _service.GetCodingSessions();
                        break;
                    case '3':
                        _service.UpdateCodingSession();
                        break;
                    case '4':
                        _service.DeleteCodingSession();
                        break;
                    case '5':
                        _service.TimeCodingSession();
                        break;
                    case '6':
                        _service.FilterCodingSessions();
                        break;
                    case '7':
                        exit = true;

                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
                Console.Clear();
               
            }
            AnsiConsole.MarkupLine("[bold yellow]Goodbye![/]");
        }



    }
}
