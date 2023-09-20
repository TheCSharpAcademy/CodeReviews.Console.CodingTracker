using System.Diagnostics;
using CodingTracker.Mo3ses.Controller;
using CodingTracker.Mo3ses.Models;
using ConsoleTableExt;

namespace CodingTracker.Mo3ses.UserMenu
{
    public class UserInput
    {
        private readonly CodingSessionController _sessionController;
        public UserInput(CodingSessionController sessionController)
        {
            _sessionController = sessionController;
        }
        public void Execute(){
            while(true){
                string answer = "";
                StartMenu();
                Console.Write("Choose one option: ");
                answer = Console.ReadLine();

                switch (answer)
                {
                    case "1":
                        AutoTrack();
                        break;
                    case "2":
                        ManualTrack();
                        break;
                    case "3":
                        UpdateSession();
                        break;
                    case "4":
                        DeleteSession();
                        break;
                    case "5":
                        ReportMenu();
                        break;
                    case "6":
                        GetSessionsDay();
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                    System.Console.WriteLine("This Option dont exist");
                    break;
                }
            }
        }

        public void StartMenu(){
            Console.Clear();
              var menuData = new List<List<object>>
            {
                new List<object> {"1 - Auto Track Coding Session"},
                new List<object>{ "2 - Manual Track Coding Session"},
                new List<object>{ "3 - Update Coding Session"},
                new List<object>{ "4 - Delete Coding Session"},
                new List<object>{ "5 - Coding Session Report"},
                new List<object>{ "0 - Exit"},
            };

            ConsoleTableBuilder.From(menuData)
               .WithFormat(ConsoleTableBuilderFormat.MarkDown)
               .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 0, TextAligntment.Left },
                    { 1, TextAligntment.Left },
                    { 3, TextAligntment.Left },
                    { 100, TextAligntment.Left }
               })
               .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
               })
               .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
               .WithTitle("CODING TRACKER MENU", ConsoleColor.Green, ConsoleColor.DarkGray, TextAligntment.Left)
               .WithFormatter(1, (text) =>
               {
                   return text.ToString().ToUpper().Replace(" ", "-") + " «";
               })
               .ExportAndWriteLine(TableAligntment.Left);
        }
        public void ManualTrack(){
            while (true)
            {
                Console.Clear();
                System.Console.WriteLine("---------- MANUAL TRACKING ----------");
                System.Console.WriteLine("Write the day and time it started(ex: 16-08-2023 08:41)");
                string startTime = Console.ReadLine();
                System.Console.WriteLine("Write the day and time it Ended(ex: 16-08-2023 09:41)");
                string endTime = Console.ReadLine();

                if(!Validation.CheckIsValidDates(startTime, endTime)){
                    System.Console.WriteLine("Invalid Dates, try again");
                    continue;
                }else{
                    _sessionController.Create(startTime, endTime);
                    System.Console.WriteLine("Coding session Added");
                    break;
                }


            }
        }
        public void UpdateSession(){
            while (true)
            {
                Console.Clear();
                AllSessions();
                System.Console.Write("Select One Id Session: ");
                string input = Console.ReadLine();

                if(Validation.CheckInt(input, out int id) ){
                    System.Console.WriteLine("Write the updated day and time it started(ex: 16-08-2023 08:41)");
                    string startTime = Console.ReadLine();
                    System.Console.WriteLine("Write the updated day and time it ended(ex: 16-08-2023 10:41)");
                    string endTime = Console.ReadLine();
                    
                    if(Validation.CheckIsValidDates(startTime, endTime)){
                        _sessionController.Update(id, startTime, endTime);
                        System.Console.WriteLine("Session Updated");
                        break;
                        
                    }else{
                        System.Console.WriteLine("Invalid Dates, try again");
                        continue;
                    }

                }else {
                    System.Console.WriteLine("Wrong Id try again");
                    break;
                    }
            }   
        }
        public void DeleteSession(){
             Console.Clear();
                AllSessions();
                System.Console.Write("Select One Id Session: ");
                string input = Console.ReadLine();

                if(Validation.CheckInt(input, out int id) ){
                    _sessionController.Delete(id);
                    System.Console.WriteLine("Session deleted!");
                }else System.Console.WriteLine("Wrong Id try again");
        }
        public void AutoTrack(){
            Stopwatch stopWatch = new Stopwatch();
            CodingSession codingSession = new();
            System.Console.WriteLine("Press Enter to Start the timer.");
            stopWatch.Start();
            codingSession.StartTime = DateTime.Now;
            do{
                        Console.Clear();
                        Console.WriteLine("Press Q to exit");
                        System.Console.WriteLine();
            }while(Console.ReadKey().Key != ConsoleKey.Q);
            stopWatch.Stop();
            codingSession.EndTime = DateTime.Now;
            TimeSpan ts = stopWatch.Elapsed;
            codingSession.Duration = ts.ToString();
            _sessionController.Create(codingSession);
        }
        public void ReportMenu(){
            AllSessions();
            System.Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
        public void GetSessionsDay(){
            System.Console.WriteLine("Do you want to see the sessions from how many days ago?");
            int input = Int32.Parse(Console.ReadLine());
            _sessionController.GetSessionsDay(input);
            System.Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
        public void GetSessionsMonth(){
            System.Console.WriteLine("Do you want to see the sessions from how many months ago?");
            int input = Int32.Parse(Console.ReadLine());
            _sessionController.GetSessionsMonth(input);
            System.Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
        public void GetSessionsYear(){
            System.Console.WriteLine("Do you want to see the sessions from how many years ago?");
            int input = Int32.Parse(Console.ReadLine());
            _sessionController.GetSessionsYear(input);
            System.Console.WriteLine("Press Any Key...");
            Console.ReadKey();
        }
        public void AllSessions(){
            _sessionController.GetAll();
        }
    }
}
