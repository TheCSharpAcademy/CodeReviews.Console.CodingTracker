

namespace Services
{
    public static class DisplayInfo
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine(@"
 ██████╗ ██████╗ ██████╗ ██╗███╗   ██╗ ██████╗     ████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗ 
██╔════╝██╔═══██╗██╔══██╗██║████╗  ██║██╔════╝     ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗
██║     ██║   ██║██║  ██║██║██╔██╗ ██║██║  ███╗       ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝
██║     ██║   ██║██║  ██║██║██║╚██╗██║██║   ██║       ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗
╚██████╗╚██████╔╝██████╔╝██║██║ ╚████║╚██████╔╝       ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║
 ╚═════╝ ╚═════╝ ╚═════╝ ╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝                                                                                                
Welcome to the Coding Tracker
Use the Up and Down arrow key to cycle through options, press Enter to select
");
        }

        public static void About()
        {
            Console.WriteLine("====================");
            Console.WriteLine("About Coding Tracker");
            Console.WriteLine("====================");
            Console.WriteLine("Version: 1.0.0");
            Console.WriteLine("Author: batus");
            Console.WriteLine("Description: This application helps you track your coding sessions and manage your projects efficiently.");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
