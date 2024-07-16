using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;
internal class Output()
{
    public static void StartTimed()
    {
        Console.WriteLine("Starting new Timed Session");
    } // end of StartSession Method
    
    public static void EndTimed()
    {
        Console.WriteLine("Ending current Timed Session");
    } // end of EndSession Method
    
    public static void NewSession()
    {
        Console.WriteLine("Creating new Session");
    } // end of NewSession Method
    
    public static void ModifySession()
    {
        Console.WriteLine("Which Session would you like to modify?");
        string response = UserInput.CleanString(Console.ReadLine());
    } // end of ModifySession Method
    
    public static void RemoveSession()
    {
        Console.WriteLine("Which Session would you like to remove?");
        string response = UserInput.CleanString(Console.ReadLine());
    } // end of RemoveSession Method
    
    public static void ViewSessions()
    {
        Console.WriteLine(@"
    Coding Sessions:
    * 07-01-2024 | 2 hours
    * 07-02-2024 | 3 hours
    * 07-04-2024 | 1 hours
    * 07-06-2024 | 2 hours
    * 07-07-2024 | 4 hours
    --------------------------
        ");
    } // end of ViewSessions Method
} // end of UserInput Class
