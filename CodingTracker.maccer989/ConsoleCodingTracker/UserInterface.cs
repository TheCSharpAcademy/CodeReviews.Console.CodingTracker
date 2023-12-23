using CodingTrackerConsoleUI;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ConsoleCodingTracker
{

    class UserInterface
    {
        static SqliteCrud sql = new SqliteCrud(GetConnectionString());

        static void Main(string[] args)
        {
           GetUserInput();  
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (closeApp == false)
            {
                Console.WriteLine("\nMain Menu");
                Console.WriteLine("---------\n");
                Console.WriteLine("What would you like to do?\n");
                Console.WriteLine("Type 1 to View All Records");
                Console.WriteLine("Type 2 to Insert Record");
                Console.WriteLine("Type 3 to Delete record");
                Console.WriteLine("Type 4 to Update Record\n");
                Console.WriteLine("Type 0 to Close Application");
                Console.WriteLine("---------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        ReadAllSessions(sql);
                        break;
                    case "2":
                        CreateNewSession(sql);
                        break;
                    case "3":
                        DeleteSession(sql);
                        break;
                    case "4":
                        UpdateSession(sql);
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.");
                        break;
                }
            }
        }

        static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = "";
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            output = config.GetConnectionString(connectionStringName);
            return output;
        }

        static void CreateNewSession(SqliteCrud sql)
        {
            ReadAllSessions(sql);
            
            string startTime = GetTime("\nPlease insert the start time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
            string endTime = GetTime("\nPlease insert the end time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
            
            int duration = CalculateDuration(startTime, endTime);
            CodingSessionModel session = new CodingSessionModel
            {
                StartTime = startTime,
                EndTime = endTime,
                DurationInMinutes = duration
            };
            sql.CreateContact(session);          
        }

        private static int CalculateDuration(string startTime, string endTime)
        {
            int duration;
            int totalStartMinutes = 0;
            int totalEndMinutes= 0;
            TimeSpan durationStart;
            TimeSpan durationEnd;          

            if (TimeSpan.TryParseExact(startTime, @"hh\:mm", null, out durationStart))
            {
                totalStartMinutes = (durationStart.Hours * 60) + durationStart.Minutes;
            }
            else
            {
                startTime = GetTime("\nError calculating start time. Please re-insert the start time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
            }
            

            if (TimeSpan.TryParseExact(endTime, @"hh\:mm", null, out durationEnd))
            {
                totalEndMinutes = (durationEnd.Hours * 60) + durationEnd.Minutes;
            }
            else
            {
                endTime = GetTime("\nError calculating end time. Please re-insert the end time: (24 Hour Format:HH:mm). Type 0 to return to main menu");  
            }

            while (!((totalEndMinutes > totalStartMinutes) || (totalStartMinutes == totalEndMinutes)))
            {
                Console.WriteLine("\nInvalid duration, end time must be greater than and not equal to start time.");
                startTime = GetTime("\nPlease insert the start time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
                endTime = GetTime("\nPlease insert the end time: (24 Hour Format:HH:mm). Type 0 to return to main menu");

                if (TimeSpan.TryParseExact(startTime, @"hh\:mm", null, out durationStart))
                {
                    totalStartMinutes = (durationStart.Hours * 60) + durationStart.Minutes;
                }

                if (TimeSpan.TryParseExact(endTime, @"hh\:mm", null, out durationEnd))
                {
                    totalEndMinutes = (durationEnd.Hours * 60) + durationEnd.Minutes;
                }                

            }

            duration = totalEndMinutes - totalStartMinutes;        
            return duration;
        }

        static void ReadAllSessions(SqliteCrud sql)
        {
            Console.Clear();
            var rows = sql.GetAllSessions();
            TableVisualisation.ShowTable(rows);
        }

        static void UpdateSession(SqliteCrud sql)
        {
            ReadAllSessions(sql);
            var recordId = GetNumberInput("\nPlease type the ID of the record you want to update. Type 0 to return to Main Menu");
            string startTime = GetTime("\nPlease insert the start time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
            string endTime = GetTime("\nPlease insert the end time: (24 Hour Format:HH:mm). Type 0 to return to main menu");
            int duration = CalculateDuration(startTime, endTime);
            sql.UpdateCodingSession(recordId, startTime, endTime, duration);
        }

        static string GetTime(string message)
        {
            Console.WriteLine(message);
            string timeInput = Console.ReadLine();
            if (timeInput == "0") GetUserInput();
            timeInput = Validation.CheckValidTime(timeInput);
            return timeInput;
        }

        static void DeleteSession(SqliteCrud sql)
        {
            Console.Clear();
            ReadAllSessions(sql);
            var recordId = GetNumberInput("\n\nPlease type the ID of the record you want to delete. Type 0 to return to Main Menu\n\n");
            sql.RemoveSession(recordId);
        }

        static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0") GetUserInput();
            Validation.CheckValidNumber(numberInput);
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}