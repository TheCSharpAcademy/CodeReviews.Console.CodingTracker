namespace CodingTracker;

internal class CodingTracker
{
    static void Main()
    {
        SessionStopWatch currentSession = new();
        CodingGoals codingGoal = new("00/00/00","00/00/00","0.00:00");

        string? userSelection;
        bool IsGoalSet = false;
        bool runCodingTracker = true;
        
        do{
            userSelection = UserDisplay.MainMenu(currentSession, IsGoalSet);
            switch(userSelection)
            {
                case("1"):
                    UserDisplay.InsertCodingSession();
                    break;

                case("2"):
                    UserDisplay.MofidyCodingSession();
                    break;

                case("3"):
                    UserDisplay.DeleteCodingSession();
                    break;

                case("4"):
                    UserDisplay.DisplayRecords();
                    break;                                                    

                case("5"):
                    if (!currentSession.IsRunning)
                        {
                            currentSession = UserDisplay.StartCodingSession(currentSession);
                        }
                    else
                        {
                            currentSession = UserDisplay.StopCodingSession(currentSession);
                        }
                    break;                    

                case("6"):
                    UserDisplay.DisplayCodingReport();
                    break;

                case("7"):
                    if(IsGoalSet)
                        {
                            UserDisplay.DisplayCodingGoal(codingGoal);
                        }
                        else
                        {
                            codingGoal = UserDisplay.SetCodingGoal();
                            IsGoalSet = true;
                        }
                    break;
                
                case("8"):
                    IsGoalSet = UserDisplay.DeleteCodingGoal();
                    break;

                case("0"):
                    runCodingTracker = false;
                    break;                 
                default:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.Clear();
                    break;                                           
            }
        }
        while(runCodingTracker);
    }
}
