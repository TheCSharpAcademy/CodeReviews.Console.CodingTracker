
using System.Data;

namespace CodingTracker.Paul_W_Saltzman
{
        internal class Settings
        {
            public int ID { get; set; }
            public int Version { get; set; }
            public bool TestMode { get; set; }
            public int Theme { get; set; }

            internal static Settings ToggleTestMode(Settings settings)
            {
                
                settings = Data.GetSettings();
                Data.ToggleTest(settings);
                settings = Data.GetSettings();
                return settings;
            }
            internal static Settings ProgramSettings(Settings settings)
            {
                bool inMenu = true;
                while (inMenu)
                {
                    Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                    Console.Clear();
                    List<string> settingsMenu = new List<string>();
                    settingsMenu.Add($@"Version: {settings.Version}");
                    settingsMenu.Add($@"Test Mode: {settings.TestMode}");
                    DataTable dateMenuTable = Menu.MenuTable(settingsMenu);
                    Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                    Console.Clear();
                    Helpers.ShowTable(dateMenuTable, "Settings");
                    Helpers.CenterText("Options: X to Exit : T to toggle test mode :");
                    Helpers.CenterCursor();
                    string userInput = Console.ReadLine();
                    userInput = userInput.Trim().ToLower();

                    switch (userInput)
                    {
                        case "x":
                            inMenu = false;
                            break;
                        case "t":
                            settings = Settings.ToggleTestMode(settings);
                            break;
                        default: break;
                    }

                }
                return settings;

            }

        }
}

