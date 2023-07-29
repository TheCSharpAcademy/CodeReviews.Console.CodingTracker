namespace CodingTrackerJMS;

public class UI
{   
    UILogic uiLogic = new UILogic();
    Validation validation = new Validation();

    public void ShowMenu()
    {
        int timeToGoal = 1;
        string goalSelected = "GeneralCoding";
        bool appClosed = false;

        while(appClosed == false)
        {
            Console.Clear();
            Console.WriteLine(@$"
Current goal selected: {goalSelected}

------- Main Menu:
    s - Start a coding session
    a - Add new coding session record
    u - Update existing record
    d - Delete a record
    v - View coding sessions

-------- Goals:
    cg - Create a new goal
    sg - Set a goal as active
    gr - Get Report (Percentage of goal completed and hours needed to complete goal).

-------- Terminate:
     c - Close app");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "s":
                    uiLogic.OnGoingSession(goalSelected, timeToGoal);
                    break;
                case "a":
                    uiLogic.GetRecordInput(userInput, goalSelected, timeToGoal);
                    break;
                case "u":
                    List<int> iDs = uiLogic.ViewSessionRecords();

                    bool idNotPresent = true;

                    while (idNotPresent == true && iDs.Count > 0)
                    {
                        int id = validation.GetValidID();
                        if (!iDs.Contains(id))
                        {
                            Console.WriteLine($"ID:{id} does not exist.");
                            idNotPresent = true;
                        }
                        else
                        {
                            idNotPresent = false;
                            uiLogic.GetRecordInput(userInput, goalSelected, timeToGoal, id);
                            Console.WriteLine("Record updated!");
                        }
                        Console.ReadLine();
                    }

                    break;
                case "d":
                    iDs = uiLogic.ViewSessionRecords();

                    idNotPresent = true;

                    while (idNotPresent == true && iDs.Count > 0)
                    {
                        int id = validation.GetValidID();
                        if (!iDs.Contains(id))
                        {
                            Console.WriteLine($"ID:{id} does not exist.");
                            idNotPresent = true;
                        }
                        else
                        {
                            uiLogic.UIDeleteRecord(id);
                            idNotPresent = false;
                            //implement way to delete the record based on the ID
                            Console.WriteLine("Record deleted!");
                        }
                        Console.ReadLine();
                    }
                    break;
                case "v":
                    uiLogic.ViewSessionRecords();
                    break;
                case "cg":
                    uiLogic.GetGoalsList();
                    uiLogic.CreateGoal(out goalSelected, out timeToGoal);
                    break;

                case "sg":
                    uiLogic.GetGoalsList();
                    uiLogic.SetGoalActive(out goalSelected, out timeToGoal);
                    break;
                case "gr":
                    int totalGoalTime = uiLogic.GetTotalTimeOfGoal(goalSelected);
                    uiLogic.CalculatePercentageGoalCompleted(totalGoalTime, timeToGoal, goalSelected);
                    break;
                
                case "c":
                    appClosed = true;
                    Console.WriteLine("Closing App...");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid user input!");
                    Console.ReadLine();
                    break;
            }
        }
    }
}

