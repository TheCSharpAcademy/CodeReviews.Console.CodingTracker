using System.Data;
using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Spectre.Console;
using CodingTracker.obitom67;



namespace HabitTracker_obitom67
{
    class Program
    {
        static void Main()
        {
            UserInput.ContinueRunning = true;
            DBHandling.TableHandling();
            while (UserInput.ContinueRunning)
            {
                UserInput.AskUser();
                
            }
            
            
        }   
    }
}
