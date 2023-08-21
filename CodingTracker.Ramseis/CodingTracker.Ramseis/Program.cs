using System.Globalization;
using System.Text.RegularExpressions;
using ConsoleTableExt;

namespace CodingTracker.Ramseis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // get config keys or set defaults if missing
            string databasePath = FileIO.ReadSetting("databasePath");
            if (databasePath == "")
            {
                FileIO.WriteSetting("databasePath", "CodingTracker.db");
                databasePath = "CodingTracker.db";
            }

            string connectionString = FileIO.ReadSetting("connectionString");
            if (connectionString == "")
            {
                FileIO.WriteSetting("connectionString", $"Data Source={databasePath}");
                connectionString = $"Data Source={databasePath}";
            }

            string backgroundColor = FileIO.ReadSetting("backgroundColor");
            if (backgroundColor == "")
            {
                FileIO.WriteSetting("backgroundColor", "White");
                backgroundColor = "White";
            }
            ConsoleColor consoleBackgroundColor = ConsoleColor.White;
            try
            {
                consoleBackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), backgroundColor, true);
                Console.BackgroundColor = consoleBackgroundColor;
            }
            catch (Exception)
            {
                Console.Write("Error loading background color. Invalid color.\n");
                Console.ReadKey();
            }

            string foregroundColor = FileIO.ReadSetting("foregroundColor");
            if (foregroundColor == "")
            {
                FileIO.WriteSetting("foregroundColor", "Black");
                foregroundColor = "Black";
            }
            ConsoleColor consoleForegroundColor = ConsoleColor.Black;
            try
            {
                consoleForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foregroundColor, true);
                Console.ForegroundColor = consoleForegroundColor;
            } catch (Exception)
            {
                Console.Write("Error loading foreground color. Invalid color.\n");
                Console.ReadKey();
            }

            // check for db or create db
            FileIO.InitializeDatabase(connectionString);

            // initialize menu objects
            Menu mainMenu = new Menu
            {
                Titles = new List<string> { "Welcome to the coding tracker database utility!", "Where WE track YOUR time!" },
                Options = new List<string> {
                    " 1. View Records",
                    " 2. Modify Records",
                    " 3. New Records",
                    " 4. Settings",
                    " 5. Exit" }
            };

            Menu viewMenu = new Menu
            {
                Titles = new List<string> { "View Records" },
                Options = new List<string> {
                    " 1. View ALL Records",
                    " 2. View records within date range",
                    " 3. View record statistics",
                    " 4. Return to Main Menu" }
            };

            Menu modifyMenu = new Menu
            {
                Titles = new List<string> { "Modify Records"},
                Options = new List<string>
                {
                    " 1. Edit record",
                    " 2. Delete record",
                    " 3. Return to Main Menu"
                }
            };

            Menu newMenu = new Menu
            {
                Titles = new List<string> { "New Record" },
                Options = new List<string> {
                    " 1. Manual input",
                    " 2. Stopwatch input",
                    " 3. Return to Main Menu"
                }
            };

            Menu settingMenu = new Menu
            {
                Titles = new List<string> { "Settings" },
                Options = new List<string>
                {
                    " 1. Database file path",
                    " 2. Buffer background color",
                    " 3. Buffer foreground color",
                    " 4. Return to Main Menu"
                }
            };

            Menu subMenu = new Menu();

            Console.Clear();
            while (true)
            {
                mainMenu.DrawMenu();
                int input = Misc.GetIntegerInput();
                Console.SetCursorPosition(2, mainMenu.InputRow + 2);

                // view records
                if (input == 1)
                {
                    Console.Clear();
                    bool menuFlag = true;
                    while (menuFlag)
                    {
                        viewMenu.DrawMenu();
                        input = Misc.GetIntegerInput();
                        Console.SetCursorPosition(2, viewMenu.InputRow + 2);

                        // view all data
                        if (input == 1)
                        {
                            Console.Clear();
                            List<List<object>> data = FileIO.SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                Misc.PrintTable(data);
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("\n\nNo data to display.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        // view within date range
                        else if (input == 2) 
                        {
                            Console.Clear();
                            CodingSession session = new();
                            bool subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                subMenu.Titles = new List<string> { viewMenu.Titles[0] + ": Date Range" };
                                subMenu.Options = new List<string> { "Enter start time: YYYY.MM.DD.HH.MM.SS" };
                                subMenu.DrawMenu();
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.Start = inputDateTime;
                                    subMenuFlag = false;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(2, subMenu.InputRow);
                                    Console.Write("                                                ");
                                }
                            }

                            subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                subMenu.Titles = new List<string> { viewMenu.Titles[0] + ": Date Range" };
                                subMenu.Options = new List<string> { "Start:", "Enter end time: YYYY.MM.DD.HH.MM.SS" };
                                subMenu.DrawMenu();

                                Console.SetCursorPosition(9, subMenu.InputRow - 3);
                                Console.Write(session.Start.ToString());
                                Console.SetCursorPosition(3, subMenu.InputRow);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.End = inputDateTime;
                                    if (session.Duration().TotalSeconds < 0)
                                    {
                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                        Console.Write("                                                    ");
                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                        Console.Write("End date must be after start date!");
                                        Console.SetCursorPosition(2, subMenu.InputRow);
                                        Console.Write("                                                ");
                                    }
                                    else
                                    {
                                        subMenuFlag = false;
                                        Console.Clear();
                                    }
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(3, subMenu.InputRow);
                                    Console.Write("                                                ");
                                }
                            }
                            List<List<object>> datas = FileIO.SqlRead("SELECT * from coding_tracker", connectionString);
                            List<List<object>> dataFiltered = new();
                            foreach (List<object> data in datas)
                            {
                                if (((session.Start - (DateTime)data[1]).TotalSeconds < 1) & ((session.End - (DateTime)data[2]).TotalSeconds > 1))
                                {
                                    dataFiltered.Add(data);
                                }
                            }
                            if (dataFiltered.Count > 0)
                            {
                                Misc.PrintTable(dataFiltered);
                            }
                            else
                            {
                                Console.WriteLine("\n\nNo data to display.");
                                Console.WriteLine("Press any key to continue...");
                            }
                            Console.ReadKey();
                            Console.Clear();
                        }
                        // view statistics
                        else if (input == 3) 
                        {
                            Console.Clear();
                            List<List<object>> data = FileIO.SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                TimeSpan totalDuration = new TimeSpan(0);
                                int qty = 0;
                                foreach (List<object> dat in data)
                                {
                                    totalDuration += (TimeSpan)dat[3];
                                    qty++;
                                }
                                subMenu.Titles = new List<string> { viewMenu.Titles[0] + ": Statistics" };
                                subMenu.Options = new List<string> { "Number of records:", "Total Duration   :", "Average duration :" };
                                subMenu.DrawMenu();
                                Console.SetCursorPosition(21, subMenu.InputRow - 4);
                                Console.Write(qty);
                                Console.SetCursorPosition(21, subMenu.InputRow - 3);
                                Console.Write(totalDuration);
                                Console.SetCursorPosition(21, subMenu.InputRow - 2);
                                Console.Write(totalDuration / qty);
                                Console.SetCursorPosition(2, subMenu.InputRow);
                                Console.Write("Press any key to continue... ");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("\n\nNo data to display.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        // Return to main menu
                        else if (input == 4)
                        {
                            menuFlag = false;
                            Console.Clear();
                        }
                        // invalid input
                        else
                        {
                            Console.SetCursorPosition(2, viewMenu.InputRow);
                            Console.Write("                                     ");
                            Console.SetCursorPosition(2, viewMenu.InputRow + 2);
                            Console.Write($"{input}: Invalid input, select from list.");
                        }
                    }
                }
                // modify records
                else if (input == 2)
                {
                    Console.Clear();
                    bool menuFlag = true;
                    while (menuFlag)
                    {
                        modifyMenu.DrawMenu();
                        input = Misc.GetIntegerInput();
                        Console.SetCursorPosition(2, modifyMenu.InputRow + 2);
                                                
                        if (input == 1) // Edit record
                        {
                            Console.Clear();
                            List<List<object>> data = FileIO.SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                Misc.PrintTable(data);
                                (int left, int top) = Console.GetCursorPosition();
                                bool subMenuFlag = true;
                                while (subMenuFlag)
                                {
                                    Console.Write("Select ID to delete: ");
                                    input = Misc.GetIntegerInput();
                                    bool existFlag = false;
                                    foreach (List<object> dat in data)
                                    {
                                        if (input == (int)dat[0])
                                        {
                                            existFlag = true;
                                            subMenuFlag = false;
                                            int exID = (int)dat[0];
                                            DateTime exStart = (DateTime)dat[1];
                                            DateTime exEnd = (DateTime)dat[2];
                                            Console.Clear();
                                            CodingSession session = new();
                                            bool subdubMenuFlag = true;
                                            while (subdubMenuFlag)
                                            {
                                                subMenu.Titles = new List<string> { modifyMenu.Titles[0] + ": Edit"};
                                                subMenu.Options = new List<string>
                                                    {
                                                    "Existing start time:",
                                                    "Enter a new start time: YYYY.MM.DD.HH.MM.SS"
                                                    };
                                                subMenu.DrawMenu();
                                                Console.SetCursorPosition(23, subMenu.InputRow - 3);
                                                Console.Write(exStart);
                                                Console.SetCursorPosition(3, subMenu.InputRow);
                                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                                {
                                                    session.Start = inputDateTime;
                                                    subdubMenuFlag = false;
                                                    Console.Clear();
                                                }
                                                else
                                                {
                                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                                    Console.Write("Error parsing input, please try again and follow required format!");
                                                    Console.SetCursorPosition(3, subMenu.InputRow);
                                                    Console.Write("                                                ");
                                                }
                                            }
                                            subdubMenuFlag = true;
                                            while (subdubMenuFlag)
                                            {
                                                subMenu.Options = new List<string>
                                                    {
                                                    "Start:",
                                                    "Existing end time:",
                                                    "Enter new date and time: YYYY.MM.DD.HH.MM.SS"
                                                    };
                                                subMenu.DrawMenu();
                                                Console.SetCursorPosition(21, subMenu.InputRow - 3);
                                                Console.Write(exEnd);
                                                Console.SetCursorPosition(9, subMenu.InputRow - 4);
                                                Console.Write(session.Start);
                                                Console.SetCursorPosition(3, subMenu.InputRow);
                                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                                {
                                                    session.End = inputDateTime;
                                                    if (session.Duration().TotalSeconds < 0)
                                                    {
                                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                                        Console.Write("                                                                    ");
                                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                                        Console.Write("End date must be after start date!");
                                                        Console.SetCursorPosition(3, subMenu.InputRow);
                                                        Console.Write("                                                ");
                                                    }
                                                    else
                                                    {
                                                        subdubMenuFlag = false;
                                                        Console.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                                    Console.Write("Error parsing input, please try again and follow required format!");
                                                    Console.SetCursorPosition(3, subMenu.InputRow);
                                                    Console.Write("                                                ");
                                                }
                                            }
                                            FileIO.SqlWrite(
                                                $"UPDATE coding_tracker " +
                                                $"SET Start = '{session.Start}', " +
                                                $"End = '{session.End}', " +
                                                $"Duration = '{session.Duration}' " +
                                                $"WHERE ID = '{exID}'"
                                                , connectionString);
                                            subMenu.Options = new List<string>
                                                {"Start   :",
                                                "End     :",
                                                "Duration:"};
                                            subMenu.DrawMenu();
                                            Console.SetCursorPosition(12, subMenu.InputRow - 4);
                                            Console.Write(session.Start);
                                            Console.SetCursorPosition(12, subMenu.InputRow - 3);
                                            Console.Write(session.End);
                                            Console.SetCursorPosition(12, subMenu.InputRow - 2);
                                            Console.Write(session.Duration());
                                            Console.SetCursorPosition(2, subMenu.InputRow);
                                            Console.Write("Press any key to continue... ");
                                            Console.ReadKey();
                                            Console.Clear();
                                        }
                                    }
                                    if (!existFlag)
                                    {
                                        Console.SetCursorPosition(left, top);
                                        Console.Write("ID not found, try again or enter -1 to return to previous menu.\n");
                                    }
                                    if (input == -1)
                                    {
                                        subMenuFlag = false;
                                    }
                                }
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("\n\nNo data to display.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        else if (input == 2) // Delete record
                        {
                            Console.Clear();
                            List<List<object>> data = FileIO.SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                Misc.PrintTable(data);
                                (int left, int top) = Console.GetCursorPosition();
                                bool subMenuFlag = true;
                                while (subMenuFlag)
                                {
                                    Console.Write("Select ID to delete: ");
                                    input = Misc.GetIntegerInput();
                                    bool existFlag = false;
                                    foreach (List<object> dat in data)
                                    {
                                        if (input == (int)dat[0])
                                        {
                                            existFlag = true;
                                        }
                                    }
                                    if (existFlag)
                                    {
                                        FileIO.SqlWrite($"DELETE from coding_tracker WHERE ID = '{input}'", connectionString);
                                        subMenuFlag = false;
                                    } else
                                    {
                                        Console.SetCursorPosition(left, top);
                                        Console.Write("ID not found, try again or enter -1 to return to previous menu.\n");
                                    }
                                    if (input == -1)
                                    {
                                        subMenuFlag = false;
                                    }
                                }
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("\n\nNo data to display.");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                            }
                        }
                        else if (input == 3) // Return to main menu
                        {
                            menuFlag = false;
                            Console.Clear();
                        }
                        else // invalid input
                        {
                            Console.SetCursorPosition(2, modifyMenu.InputRow + 2);
                            Console.Write($"{input}: Invalid input, select from list.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                    Console.Clear();
                }
                // new records
                else if (input == 3)
                {
                    Console.Clear();
                    bool menuFlag = true;
                    while (menuFlag)
                    {
                        newMenu.DrawMenu();
                        input = Misc.GetIntegerInput();
                        Console.SetCursorPosition(2, newMenu.InputRow + 2);

                        if (input == 1) // Manual input
                        {
                            Console.Clear();
                            CodingSession session = new();
                            bool subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                subMenu.Titles = new List<string> { newMenu.Titles[0] + ": Manual Input" };
                                subMenu.Options = new List<string> { "Enter start time: YYYY.MM.DD.HH.MM.SS"};
                                subMenu.DrawMenu();
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.Start = inputDateTime;
                                    subMenuFlag = false;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(2, subMenu.InputRow);
                                    Console.Write("                                                ");
                                }
                            }
                            subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                subMenu.Options = new List<string> { "Start:", "Enter end time: YYYY.MM.DD.HH.MM.SS"};
                                subMenu.DrawMenu();
                                Console.SetCursorPosition(9, subMenu.InputRow - 3);
                                Console.Write(session.Start.ToString());
                                Console.SetCursorPosition(2, subMenu.InputRow);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.End = inputDateTime;
                                    if (session.Duration().TotalSeconds < 0)
                                    {
                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                        Console.Write("                                                                    ");
                                        Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                        Console.Write("End date must be after start date!");
                                        Console.SetCursorPosition(2, subMenu.InputRow);
                                        Console.Write("                                                ");
                                    } else
                                    {
                                        subMenuFlag = false;
                                        Console.Clear();
                                    }
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(2, subMenu.InputRow);
                                    Console.Write("                                                ");
                                }
                            }
                            FileIO.SqlWrite(
                                $"INSERT INTO coding_tracker(Start, End, Duration) " +
                                $"VALUES('{session.Start.ToString()}', '{session.End.ToString()}', '{session.Duration().ToString()}')", connectionString);
                            subMenu.Options = new List<string> { 
                                "Start   :",
                                "End     :",
                                "Duration:"};
                            subMenu.DrawMenu();

                            Console.SetCursorPosition(12, subMenu.InputRow - 4);
                            Console.Write(session.Start);
                            Console.SetCursorPosition(12, subMenu.InputRow - 3);
                            Console.Write(session.End);
                            Console.SetCursorPosition(12, subMenu.InputRow - 2);
                            Console.Write(session.Duration());
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            Console.Write("Press any key to continue... ");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else if (input == 2) // Stopwatch input
                        {
                            Console.Clear();
                            CodingSession session = new();
                            subMenu.Titles = new List<string> { newMenu.Titles[0] + ": Stopwatch" };
                            subMenu.Options = new List<string> { };
                            subMenu.DrawMenu();
                            Console.Write("Press any key to start timing... ");
                            Console.ReadKey();
                            session.Start = DateTime.Now;
                            Console.Clear();
                            subMenu.Options = new List<string> { "Start:" };
                            subMenu.DrawMenu();
                            Console.SetCursorPosition(9, subMenu.InputRow - 2);
                            Console.Write(session.Start);
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            Console.Write("Press any key to stop timing... ");
                            Console.ReadKey();
                            session.End = DateTime.Now;
                            Console.Clear();
                            FileIO.SqlWrite(
                                $"INSERT INTO coding_tracker(Start, End, Duration) " +
                                $"VALUES('{session.Start.ToString()}', '{session.End.ToString()}', '{session.Duration().ToString()}')", connectionString);
                            subMenu.Options = new List<string> {
                                "Start   :",
                                "End     :",
                                "Duration:"};
                            subMenu.DrawMenu();

                            Console.SetCursorPosition(12, subMenu.InputRow - 4);
                            Console.Write(session.Start);
                            Console.SetCursorPosition(12, subMenu.InputRow - 3);
                            Console.Write(session.End);
                            Console.SetCursorPosition(12, subMenu.InputRow - 2);
                            Console.Write(session.Duration());
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            Console.Write("Press any key to continue... ");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else if (input == 3) // Return to main menu
                        {
                            menuFlag = false;
                            Console.Clear();
                        }
                        else // invalid input
                        {
                            Console.SetCursorPosition(2, newMenu.InputRow + 2);
                            Console.Write($"{input}: Invalid input, select from list.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                    Console.Clear();
                }
                // Settings menu
                else if (input == 4)
                {
                    Console.Clear();
                    bool menuFlag = true;
                    while (menuFlag)
                    {
                        settingMenu.DrawMenu();
                        input = Misc.GetIntegerInput();

                        if (input == 1) // Set database file path
                        {
                            Console.Clear();
                            subMenu.Titles = new List<string> { settingMenu.Titles[0] + ": Database file path" };
                            subMenu.Options = new List<string> { "Stored path:", ">> CAUTION, path must end with filename.db <<"};
                            subMenu.DrawMenu();
                            Console.SetCursorPosition(15, subMenu.InputRow - 3);
                            Console.Write(databasePath);
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            string databasePathInput = Console.ReadLine().Trim();
                            if (databasePathInput.Contains('*') |
                                databasePathInput.Contains('\"') |
                                databasePathInput.Contains('<') |
                                databasePathInput.Contains('>') |
                                databasePathInput.Contains(':') |
                                databasePathInput.Contains('|') |
                                databasePathInput.Contains('?') |
                                databasePathInput.Contains('/') |
                                databasePathInput.Contains('\\') |
                                !databasePathInput.EndsWith(".db"))
                            {
                                Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                Console.Write("Invalid path entered...");
                                Console.ReadKey();
                            } else
                            {
                                bool writeSuccessFlag = false;
                                try
                                {
                                    FileIO.WriteSetting("databasePath", databasePathInput);
                                    writeSuccessFlag = true;
                                }
                                catch (Exception)
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.Write("Error saving new path...");
                                    Console.ReadKey();
                                }
                                if (writeSuccessFlag)
                                {
                                    databasePath = databasePathInput;
                                    connectionString = $"Data Source={databasePath}";
                                    FileIO.InitializeDatabase(connectionString);
                                }
                            }
                            Console.Clear();
                        }
                        else if (input == 2) // Set background color
                        {
                            Console.Clear();
                            subMenu.Titles = new List<string> { settingMenu.Titles[0] + ": Background Color" };
                            subMenu.Options = new List<string> { "Current color:", "Options:", "", "", "", ""};
                            subMenu.DrawMenu();
                            Console.SetCursorPosition(2, subMenu.InputRow - 5);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("DarkBlue   ");
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write("DarkGreen  ");
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("DarkCyan    ");
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.Write("DarkRed    ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 4);
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write("DarkYellow ");
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("DarkMagenta");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write("Gray        ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("DarkGray   ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 3);
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("Blue       ");
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("Green      ");
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("Cyan        ");
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("Red        ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 2);
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("Magenta    ");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write("Yellow     ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("White       ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("Black      ");
                            Console.SetCursorPosition(17, subMenu.InputRow - 7);
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write(consoleBackgroundColor);
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            string backgroundInput = Console.ReadLine().Trim();
                            try
                            {
                                ConsoleColor inputConsoleBackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), backgroundInput, true);
                                if (inputConsoleBackgroundColor != consoleForegroundColor)
                                {
                                    consoleBackgroundColor = inputConsoleBackgroundColor;
                                    Console.BackgroundColor = consoleBackgroundColor;
                                    FileIO.WriteSetting("backgroundColor", consoleBackgroundColor.ToString());
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.WriteLine("Background color must be different than foreground...");
                                    Console.ReadKey();
                                }
                            } catch (Exception)
                            {
                                Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                Console.WriteLine("Error parsing color input...");
                                Console.ReadKey();
                            }
                            Console.Clear();
                        }
                        else if (input == 3) // Set foreground color
                        {
                            Console.Clear();
                            subMenu.Titles = new List<string> { settingMenu.Titles[0] + ": Foreground Color" };
                            subMenu.Options = new List<string> { "Current color:", "Options:", "", "", "", "" };
                            subMenu.DrawMenu();
                            Console.SetCursorPosition(2, subMenu.InputRow - 5);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("DarkBlue   ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("DarkGreen  ");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("DarkCyan    ");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("DarkRed    ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 4);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("DarkYellow ");
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("DarkMagenta");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Gray        ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("DarkGray   ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 3);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("Blue       ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Green      ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Cyan        ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Red        ");
                            Console.SetCursorPosition(2, subMenu.InputRow - 2);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("Magenta    ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Yellow     ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("White       ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("Black      ");
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.SetCursorPosition(17, subMenu.InputRow - 7);
                            Console.Write(consoleForegroundColor);
                            Console.SetCursorPosition(2, subMenu.InputRow);
                            string foregroundInput = Console.ReadLine().Trim();
                            try
                            {
                                ConsoleColor inputConsoleForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foregroundInput, true);
                                if (inputConsoleForegroundColor != consoleBackgroundColor)
                                {
                                    consoleForegroundColor = inputConsoleForegroundColor;
                                    Console.ForegroundColor = consoleForegroundColor;
                                    FileIO.WriteSetting("foregroundColor", consoleForegroundColor.ToString());
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                    Console.WriteLine("Foreground color must be different than background...");
                                    Console.ReadKey();
                                }
                            }
                            catch (Exception)
                            {
                                Console.SetCursorPosition(2, subMenu.InputRow + 2);
                                Console.WriteLine("Error parsing color input...");
                                Console.ReadKey();
                            }
                            Console.Clear();
                        }
                        else if (input == 4) // Return to main menu
                        {
                            menuFlag = false;
                            Console.Clear();
                        }
                        else // invalid input
                        {
                            Console.SetCursorPosition(2, settingMenu.InputRow);
                            Console.Write($"{input}: Invalid input, select from list.");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
                // Exit program
                else if (input == 5)
                {
                    Environment.Exit(0);
                }
                // invalid input
                else
                {
                    Console.Write($"{input}: Invalid input, select from list.");
                    Console.SetCursorPosition(0, 0);
                }
            }
        }
    }
}