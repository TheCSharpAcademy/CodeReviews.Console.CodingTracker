namespace FancyMenu
{
    public class Menu
    {
        private static readonly string[] options = ["New Coding session", "All coding sessions", "About", "Exit"];
        private static string prefix = "*";

        private static readonly string WelcomeMessage = @"
 ██████╗ ██████╗ ██████╗ ██╗███╗   ██╗ ██████╗     ████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗ 
██╔════╝██╔═══██╗██╔══██╗██║████╗  ██║██╔════╝     ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗
██║     ██║   ██║██║  ██║██║██╔██╗ ██║██║  ███╗       ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝
██║     ██║   ██║██║  ██║██║██║╚██╗██║██║   ██║       ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗
╚██████╗╚██████╔╝██████╔╝██║██║ ╚████║╚██████╔╝       ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║
 ╚═════╝ ╚═════╝ ╚═════╝ ╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝                                                                                                
Welcome to the Coding Tracker
Use the Up and Down arrow key to cycle through options, press Enter to select";

        public static void RunMainMenu()
        {
            while (true)
            {
                int selectedIndex = Run(options);
                switch (selectedIndex)
                {
                    case 0:
                        Console.WriteLine("New Coding session()");
                        // Add new coding session
                        break;
                    case 1:
                        Console.WriteLine("All coding sessions()");
                        // Display all coding sessions
                        break;
                    case 2:
                        // Display about information
                        About();
                        break;
                    case 3:
                        // Exit
                        ExitProgram();
                        break;
                }
            }
        }



        private static void ExitProgram()
        {
            string[] exitOptions = ["Yes", "No"];
            int exitSelectedIndex = Run(exitOptions, "Are you sure you want to exit?");
            if (exitSelectedIndex == 0)
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        private static void Display(string[] options, int selectedIndex, string prompt = "")
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                Console.WriteLine(prompt);
            }
            else
            {
                string[] lines = WelcomeMessage.Split('\n');
                int maxLength = 0;
                foreach (string line in lines)
                {
                    if (line.Length > maxLength)
                    {
                        maxLength = line.Length;
                    }
                }

                int windowWidth = Console.WindowWidth;
                int padding = (windowWidth - maxLength) / 2;

                Console.Clear();
                foreach (string line in lines)
                {
                    if (padding > 0)
                    {
                        Console.WriteLine(line.PadLeft(line.Length + padding));
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                }
            }

            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];

                if (i == selectedIndex)
                {
                    prefix = "*";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix} << {currentOption} >>");
            }
            Console.ResetColor();
        }

        private static int Run(string[] options, string prompt = "")
        {
            int selectedIndex = 0;
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                Display(options, selectedIndex, prompt);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                // update selectedIndex based on key pressed
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= options.Length)
                    {
                        selectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                    {
                        selectedIndex = options.Length - 1;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);
            return selectedIndex;
        }

        private static void About()
        {
            Console.Clear();
            Console.WriteLine("About Coding Tracker");
            Console.WriteLine("====================");
            Console.WriteLine("Version: 1.0.0");
            Console.WriteLine("Author: batus");
            Console.WriteLine("Description: This application helps you track your coding sessions and manage your projects efficiently.");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
        }
    }
}
