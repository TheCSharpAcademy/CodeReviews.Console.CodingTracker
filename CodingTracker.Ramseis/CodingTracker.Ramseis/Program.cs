using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Data.Sqlite;
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
            string databasePath = ReadSetting("databasePath");
            if (databasePath == "")
            {
                WriteSetting("databasePath", "CodingTracker.db");
                databasePath = "CodingTracker.db";
            }

            string connectionString = ReadSetting("connectionString");
            if (connectionString == "")
            {
                WriteSetting("connectionString", $"Data Source={databasePath}");
                connectionString = $"Data Source={databasePath}";
            }

            string backgroundColor = ReadSetting("backgroundColor");
            if (backgroundColor == "")
            {
                WriteSetting("backgroundColor", "White");
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

            string foregroundColor = ReadSetting("foregroundColor");
            if (foregroundColor == "")
            {
                WriteSetting("foregroundColor", "Black");
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
            InitializeDatabase(connectionString);

            Console.Clear();
            while (true)
            {
                Console.Write(
                    "\n╔════════════════════════════════════════════════════╛\n" +
                    "║ Welcome to the coding tracker database utility!   ┌┐\n" +
                    "║ Where WE track YOUR time!                         ││\n" +
                    "╟───────────────────────────────────────────────────┤│\n" +
                    "║ 1. View records                                   ││\n" +
                    "║ 2. Modify records                                 ││\n" +
                    "║ 3. New records                                    ││\n" +
                    "║ 4. Settings                                       ││\n" +
                    "║ 5. Exit                                           ││\n" +
                    "╟───────────────────────────────────────────────────┤│\n" +
                    "║ Selection option:                                 └┘\n" +
                    "╚════════════════════════════════════════════════════╕"
                    );
                Console.SetCursorPosition(20, 11);
                
                int input = GetIntegerInput();
                Console.SetCursorPosition(2, 13);

                // view records
                if (input == 1)
                {
                    Console.Clear();
                    bool menuFlag = true;
                    while (menuFlag)
                    {
                        Console.Write(
                            "\n╔════════════════════════════════════════════════════╛\n" +
                            "║ View Records                                      ┌┐\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ 1. View ALL records                               ││\n" +
                            "║ 2. View records within date range                 ││\n" +
                            "║ 3. View record statistics                         ││\n" +
                            "║ 4. Return to main menu                            ││\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ Selection option:                                 └┘\n" +
                            "╚════════════════════════════════════════════════════╕"
                            );
                        Console.SetCursorPosition(20, 9);

                        input = GetIntegerInput();
                        Console.SetCursorPosition(2, 11);

                        // view all data
                        if (input == 1)
                        {
                            Console.Clear();
                            List<List<object>> data = SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                PrintTable(data);
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
                                Console.Write(
                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                    "║ View Records: Date Range                          ┌┐\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ Enter start time: YYYY.MM.DD.HH.MM.SS             ││\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ :                                                 └┘\n" +
                                    "╚════════════════════════════════════════════════════╕"
                                );
                                Console.SetCursorPosition(3, 6);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.Start = inputDateTime;
                                    subMenuFlag = false;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 8);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(3, 6);
                                    Console.Write("                                                ");
                                    Console.SetCursorPosition(0, 0);
                                }
                            }
                            subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                Console.Write(
                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                    "║ View Records: Date Range                          ┌┐\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ Start:                                            ││\n" +
                                    "║ Enter end time: YYYY.MM.DD.HH.MM.SS               ││\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ :                                                 └┘\n" +
                                    "╚════════════════════════════════════════════════════╕"
                                );
                                Console.SetCursorPosition(9, 4);
                                Console.Write(session.Start.ToString());
                                Console.SetCursorPosition(3, 7);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.End = inputDateTime;
                                    if (session.Duration().TotalSeconds < 0)
                                    {
                                        Console.SetCursorPosition(2, 9);
                                        Console.Write("                                                                    ");
                                        Console.SetCursorPosition(2, 9);
                                        Console.Write("End date must be after start date!");
                                        Console.SetCursorPosition(3, 7);
                                        Console.Write("                                                ");
                                        Console.SetCursorPosition(0, 0);
                                    }
                                    else
                                    {
                                        subMenuFlag = false;
                                        Console.Clear();
                                    }
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 9);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(3, 7);
                                    Console.Write("                                                ");
                                    Console.SetCursorPosition(0, 0);
                                }
                            }
                            List<List<object>> datas = SqlRead("SELECT * from coding_tracker", connectionString);
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
                                PrintTable(dataFiltered);
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
                            List<List<object>> data = SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                TimeSpan totalDuration = new TimeSpan(0);
                                int qty = 0;
                                foreach (List<object> dat in data)
                                {
                                    totalDuration += (TimeSpan)dat[3];
                                    qty++;
                                }
                                Console.Write(
                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                    "║ View Records: Statistics                          ┌┐\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ Number of records:                                ││\n" +
                                    "║ Total duration   :                                ││\n" +
                                    "║ Average duration :                                ││\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ :                                                 └┘\n" +
                                    "╚════════════════════════════════════════════════════╕"
                                );
                                Console.SetCursorPosition(21, 4);
                                Console.Write(qty);
                                Console.SetCursorPosition(21, 5);
                                Console.Write(totalDuration);
                                Console.SetCursorPosition(21, 6);
                                Console.Write(totalDuration / qty);
                                Console.SetCursorPosition(4, 8);
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
                        // Return to main menu
                        else if (input == 4)
                        {
                            menuFlag = false;
                            Console.Clear();
                        }
                        // invalid input
                        else
                        {
                            Console.SetCursorPosition(2, 10);
                            Console.Write($"{input}: Invalid input, select from list.");
                            Console.ReadKey();
                            Console.Clear();
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
                        Console.Write(
                            "\n╔════════════════════════════════════════════════════╛\n" +
                            "║ Modify Records                                    ┌┐\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ 1. Edit existing record                           ││\n" +
                            "║ 2. Delete record                                  ││\n" +
                            "║ 3. Return to main menu                            ││\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ Selection option:                                 └┘\n" +
                            "╚════════════════════════════════════════════════════╕"
                        );
                        Console.SetCursorPosition(20, 8);

                        input = GetIntegerInput();
                        Console.SetCursorPosition(2, 10);
                                                
                        if (input == 1) // Edit record
                        {
                            Console.Clear();
                            List<List<object>> data = SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                PrintTable(data);
                                (int left, int top) = Console.GetCursorPosition();
                                bool subMenuFlag = true;
                                while (subMenuFlag)
                                {
                                    Console.Write("Select ID to delete: ");
                                    input = GetIntegerInput();
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
                                                Console.Write(
                                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                                    "║ Edit Record:                                     ┌┐\n" +
                                                    "╟───────────────────────────────────────────────────┤│\n" +
                                                    "║ Existing start time:                              ││\n" +
                                                    "║ Enter new start time: YYYY.MM.DD.HH.MM.SS         ││\n" +
                                                    "╟───────────────────────────────────────────────────┤│\n" +
                                                    "║ :                                                 └┘\n" +
                                                    "╚════════════════════════════════════════════════════╕"
                                                );
                                                Console.SetCursorPosition(23, 4);
                                                Console.Write(exStart);
                                                Console.SetCursorPosition(15, 2);
                                                Console.Write(exID);
                                                Console.SetCursorPosition(3, 7);
                                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                                {
                                                    session.Start = inputDateTime;
                                                    subdubMenuFlag = false;
                                                    Console.Clear();
                                                }
                                                else
                                                {
                                                    Console.SetCursorPosition(2, 9);
                                                    Console.Write("Error parsing input, please try again and follow required format!");
                                                    Console.SetCursorPosition(3, 7);
                                                    Console.Write("                                                ");
                                                    Console.SetCursorPosition(0, 0);
                                                }
                                            }
                                            subdubMenuFlag = true;
                                            while (subdubMenuFlag)
                                            {
                                                Console.Write(
                                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                                    "║ Edit Record:                                      ┌┐\n" +
                                                    "╟───────────────────────────────────────────────────┤│\n" +
                                                    "║ Start:                                            ││\n" +
                                                    "║ Existing end time:                                ││\n" +
                                                    "║ Enter new end time: YYYY.MM.DD.HH.MM.SS           ││\n" +
                                                    "╟───────────────────────────────────────────────────┤│\n" +
                                                    "║ :                                                 └┘\n" +
                                                    "╚════════════════════════════════════════════════════╕"
                                                );
                                                Console.SetCursorPosition(15, 2);
                                                Console.Write(exID);
                                                Console.SetCursorPosition(21, 5);
                                                Console.Write(exEnd);
                                                Console.SetCursorPosition(9, 4);
                                                Console.Write(session.Start.ToString());
                                                Console.SetCursorPosition(3, 8);
                                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                                {
                                                    session.End = inputDateTime;
                                                    if (session.Duration().TotalSeconds < 0)
                                                    {
                                                        Console.SetCursorPosition(2, 10);
                                                        Console.Write("                                                                    ");
                                                        Console.SetCursorPosition(2, 10);
                                                        Console.Write("End date must be after start date!");
                                                        Console.SetCursorPosition(3, 8);
                                                        Console.Write("                                                ");
                                                        Console.SetCursorPosition(0, 0);
                                                    }
                                                    else
                                                    {
                                                        subdubMenuFlag = false;
                                                        Console.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    Console.SetCursorPosition(2, 10);
                                                    Console.Write("Error parsing input, please try again and follow required format!");
                                                    Console.SetCursorPosition(3, 8);
                                                    Console.Write("                                                ");
                                                    Console.SetCursorPosition(0, 0);
                                                }
                                            }
                                            SqlWrite(
                                                $"UPDATE coding_tracker " +
                                                $"SET Start = '{session.Start}', " +
                                                $"End = '{session.End}', " +
                                                $"Duration = '{session.Duration}' " +
                                                $"WHERE ID = '{exID}'"
                                                , connectionString);
                                            Console.Write(
                                                "\n╔════════════════════════════════════════════════════╛\n" +
                                                "║ New Records: Manual Input                         ┌┐\n" +
                                                "╟───────────────────────────────────────────────────┤│\n" +
                                                "║ Start   :                                         ││\n" +
                                                "║ End     :                                         ││\n" +
                                                "║ Duration:                                         ││\n" +
                                                "╟───────────────────────────────────────────────────┤│\n" +
                                                "║ Press any key to return...                        └┘\n" +
                                                "╚════════════════════════════════════════════════════╕"
                                            );
                                            Console.SetCursorPosition(12, 4);
                                            Console.Write(session.Start);
                                            Console.SetCursorPosition(12, 5);
                                            Console.Write(session.End);
                                            Console.SetCursorPosition(12, 6);
                                            Console.Write(session.Duration());
                                            Console.SetCursorPosition(29, 8);
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
                            List<List<object>> data = SqlRead("SELECT * from coding_tracker", connectionString);
                            if (data.Count > 0)
                            {
                                PrintTable(data);
                                (int left, int top) = Console.GetCursorPosition();
                                bool subMenuFlag = true;
                                while (subMenuFlag)
                                {
                                    Console.Write("Select ID to delete: ");
                                    input = GetIntegerInput();
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
                                        SqlWrite($"DELETE from coding_tracker WHERE ID = '{input}'", connectionString);
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
                            Console.SetCursorPosition(2, 10);
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
                        Console.Write(
                            "\n╔════════════════════════════════════════════════════╛\n" +
                            "║ New Records                                       ┌┐\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ 1. Manual input                                   ││\n" +
                            "║ 2. Stopwatch input                                ││\n" +
                            "║ 3. Return to main menu                            ││\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ Selection option:                                 └┘\n" +
                            "╚════════════════════════════════════════════════════╕"
                        );
                        Console.SetCursorPosition(20, 8);

                        input = GetIntegerInput();
                        Console.SetCursorPosition(2, 10);

                        if (input == 1) // Manual input
                        {
                            Console.Clear();
                            CodingSession session = new();
                            bool subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                Console.Write(
                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                    "║ New Records: Manual Input                         ┌┐\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ Enter start time: YYYY.MM.DD.HH.MM.SS             ││\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ :                                                 └┘\n" +
                                    "╚════════════════════════════════════════════════════╕"
                                );
                                Console.SetCursorPosition(3, 6);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.Start = inputDateTime;
                                    subMenuFlag = false;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 8);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(3, 6);
                                    Console.Write("                                                ");
                                    Console.SetCursorPosition(0, 0);
                                }
                            }
                            subMenuFlag = true;
                            while (subMenuFlag)
                            {
                                Console.Write(
                                    "\n╔════════════════════════════════════════════════════╛\n" +
                                    "║ New Records: Manual Input                         ┌┐\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ Start:                                            ││\n" +
                                    "║ Enter end time: YYYY.MM.DD.HH.MM.SS               ││\n" +
                                    "╟───────────────────────────────────────────────────┤│\n" +
                                    "║ :                                                 └┘\n" +
                                    "╚════════════════════════════════════════════════════╕"
                                );
                                Console.SetCursorPosition(9, 4);
                                Console.Write(session.Start.ToString());
                                Console.SetCursorPosition(3, 7);
                                string inputDate = Regex.Replace(Console.ReadLine().Trim().ToLower(), @"[^0-9]", "");

                                if (DateTime.TryParseExact(inputDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, 0, out DateTime inputDateTime))
                                {
                                    session.End = inputDateTime;
                                    if (session.Duration().TotalSeconds < 0)
                                    {
                                        Console.SetCursorPosition(2, 9);
                                        Console.Write("                                                                    ");
                                        Console.SetCursorPosition(2, 9);
                                        Console.Write("End date must be after start date!");
                                        Console.SetCursorPosition(3, 7);
                                        Console.Write("                                                ");
                                        Console.SetCursorPosition(0, 0);
                                    } else
                                    {
                                        subMenuFlag = false;
                                        Console.Clear();
                                    }
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 9);
                                    Console.Write("Error parsing input, please try again and follow required format!");
                                    Console.SetCursorPosition(3, 7);
                                    Console.Write("                                                ");
                                    Console.SetCursorPosition(0, 0);
                                }
                            }
                            SqlWrite(
                                $"INSERT INTO coding_tracker(Start, End, Duration) " +
                                $"VALUES('{session.Start.ToString()}', '{session.End.ToString()}', '{session.Duration().ToString()}')", connectionString);
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ New Records: Manual Input                         ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Start   :                                         ││\n" +
                                "║ End     :                                         ││\n" +
                                "║ Duration:                                         ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Press any key to return...                        └┘\n" +
                                "╚════════════════════════════════════════════════════╕"
                            );
                            Console.SetCursorPosition(12, 4);
                            Console.Write(session.Start.ToString());
                            Console.SetCursorPosition(12, 5);
                            Console.Write(session.End.ToString());
                            Console.SetCursorPosition(12, 6);
                            Console.Write(session.Duration().ToString());
                            Console.SetCursorPosition(29, 8);
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else if (input == 2) // Stopwatch input
                        {
                            Console.Clear();
                            CodingSession session = new();
                            bool subMenuFlag = true;
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ New Records: Stopwatch                            ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Press any key to start timing...                  ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ :                                                 └┘\n" +
                                "╚════════════════════════════════════════════════════╕"
                            );
                            Console.SetCursorPosition(3, 6);
                            Console.ReadKey();
                            session.Start = DateTime.Now;
                            Console.Clear();
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ New Records: Stopwatch                            ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Start:                                            ││\n" +
                                "║ Press any key to stop timing...                   ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ :                                                 └┘\n" +
                                "╚════════════════════════════════════════════════════╕"
                            );
                            Console.SetCursorPosition(9, 4);
                            Console.Write(session.Start.ToString());
                            Console.SetCursorPosition(3, 7);
                            Console.ReadKey();
                            session.End = DateTime.Now;
                            Console.Clear();
                            SqlWrite(
                                $"INSERT INTO coding_tracker(Start, End, Duration) " +
                                $"VALUES('{session.Start.ToString()}', '{session.End.ToString()}', '{session.Duration().ToString()}')", connectionString);
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ New Records: Manual Input                         ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Start   :                                         ││\n" +
                                "║ End     :                                         ││\n" +
                                "║ Duration:                                         ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Press any key to return...                        └┘\n" +
                                "╚════════════════════════════════════════════════════╕"
                            );
                            Console.SetCursorPosition(12, 4);
                            Console.Write(session.Start.ToString());
                            Console.SetCursorPosition(12, 5);
                            Console.Write(session.End.ToString());
                            Console.SetCursorPosition(12, 6);
                            Console.Write(session.Duration().ToString());
                            Console.SetCursorPosition(29, 8);
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
                            Console.SetCursorPosition(2, 10);
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
                        Console.Write(
                        "\n╔════════════════════════════════════════════════════╛\n" +
                        "║ Settings                                          ┌┐\n" +
                        "╟───────────────────────────────────────────────────┤│\n" +
                        "║ 1. Database file path                             ││\n" +
                        "║ 2. Buffer background color                        ││\n" +
                        "║ 3. Buffer foreground color                        ││\n" +
                        "║ 4. Return to main menu                            ││\n" +
                        "╟───────────────────────────────────────────────────┤│\n" +
                        "║ Selection option:                                 └┘\n" +
                        "╚════════════════════════════════════════════════════╕"
                        );
                        Console.SetCursorPosition(20, 9);
                        input = GetIntegerInput();

                        if (input == 1) // Set database file path
                        {
                            Console.Clear();
                            Console.Write(
                            "\n╔════════════════════════════════════════════════════╛\n" +
                            "║ Settings: Database file path                      ┌┐\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ Stored path:                                      ││\n" +
                            "╟───────────────────────────────────────────────────┤│\n" +
                            "║ New path:                                         ││\n" +
                            "║ >> CAUTION, path must end with filename.db <<     └┘\n" +
                            "╚════════════════════════════════════════════════════╕"
                            );
                            Console.SetCursorPosition(15, 4);
                            Console.Write(databasePath);
                            Console.SetCursorPosition(12, 6);
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
                                Console.SetCursorPosition(2, 9);
                                Console.Write("Invalid path entered...");
                                Console.ReadKey();
                            } else
                            {
                                bool writeSuccessFlag = false;
                                try
                                {
                                    WriteSetting("databasePath", databasePathInput);
                                    writeSuccessFlag = true;
                                }
                                catch (Exception)
                                {
                                    Console.SetCursorPosition(2, 9);
                                    Console.Write("Error saving new path...");
                                    Console.ReadKey();
                                }
                                if (writeSuccessFlag)
                                {
                                    databasePath = databasePathInput;
                                    connectionString = $"Data Source={databasePath}";
                                    InitializeDatabase(connectionString);
                                }
                            }
                            Console.Clear();
                        }
                        else if (input == 2) // Set background color
                        {
                            Console.Clear();
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ Settings: Background Color                        ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Current color:                                    ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ New color:                                        ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Options:                                          ││\n" +
                                "║ "
                                );
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.Write("DarkBlue    ");
                            Console.BackgroundColor = ConsoleColor.DarkGreen;
                            Console.Write("DarkGreen   ");
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.Write("DarkCyan     ");
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.Write("DarkRed      ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.DarkYellow;
                            Console.Write("DarkYellow  ");
                            Console.BackgroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("DarkMagenta ");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.Write("Gray         ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.Write("DarkGray     ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write("Blue        ");
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write("Green       ");
                            Console.BackgroundColor = ConsoleColor.Cyan;
                            Console.Write("Cyan         ");
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write("Red          ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            Console.Write("Magenta     ");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.Write("Yellow      ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("White        ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Write("Black        ");
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.Write("└┘\n");
                            Console.Write("╚════════════════════════════════════════════════════╕");
                            Console.SetCursorPosition(17, 4);
                            Console.Write(consoleBackgroundColor);
                            Console.SetCursorPosition(13, 6);
                            string backgroundInput = Console.ReadLine().Trim();
                            try
                            {
                                ConsoleColor inputConsoleBackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), backgroundInput, true);
                                if (inputConsoleBackgroundColor != consoleForegroundColor)
                                {
                                    consoleBackgroundColor = inputConsoleBackgroundColor;
                                    Console.BackgroundColor = consoleBackgroundColor;
                                    WriteSetting("backgroundColor", consoleBackgroundColor.ToString());
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 14);
                                    Console.WriteLine("Background color must be different than foreground...");
                                    Console.ReadKey();
                                }
                            } catch (Exception)
                            {
                                Console.SetCursorPosition(2, 14);
                                Console.WriteLine("Error parsing color input...");
                                Console.ReadKey();
                            }
                            Console.Clear();
                        }
                        else if (input == 3) // Set foreground color
                        {
                            Console.Clear();
                            Console.Write(
                                "\n╔════════════════════════════════════════════════════╛\n" +
                                "║ Settings: Foreground Color                        ┌┐\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Current color:                                    ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ New color:                                        ││\n" +
                                "╟───────────────────────────────────────────────────┤│\n" +
                                "║ Options:                                          ││\n" +
                                "║ "
                                );
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("DarkBlue    ");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("DarkGreen   ");
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("DarkCyan     ");
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("DarkRed      ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("DarkYellow  ");
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("DarkMagenta ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("Gray         ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("DarkGray     ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("Blue        ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Green       ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Cyan         ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Red          ");
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.Write("││\n║ ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("Magenta     ");
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("Yellow      ");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("White        ");
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write("Black        ");
                            Console.ForegroundColor = consoleForegroundColor;
                            Console.BackgroundColor = consoleBackgroundColor;
                            Console.Write("└┘\n");
                            Console.Write("╚════════════════════════════════════════════════════╕");
                            Console.SetCursorPosition(17, 4);
                            Console.Write(consoleForegroundColor);
                            Console.SetCursorPosition(13, 6);
                            string foregroundInput = Console.ReadLine().Trim();
                            try
                            {
                                ConsoleColor inputConsoleForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), foregroundInput, true);
                                if (inputConsoleForegroundColor != consoleBackgroundColor)
                                {
                                    consoleForegroundColor = inputConsoleForegroundColor;
                                    Console.ForegroundColor = consoleForegroundColor;
                                    WriteSetting("foregroundColor", consoleForegroundColor.ToString());
                                }
                                else
                                {
                                    Console.SetCursorPosition(2, 14);
                                    Console.WriteLine("Foreground color must be different than background...");
                                    Console.ReadKey();
                                }
                            }
                            catch (Exception)
                            {
                                Console.SetCursorPosition(2, 14);
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
                            Console.SetCursorPosition(2, 11);
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
        static int GetIntegerInput()
        {
            int.TryParse(Console.ReadLine().Trim(), out int input);
            return input;
        }
        static void WriteSetting(string key, string value)
        {
            try
            {
                Configuration configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing settings.");
            }
        }
        static string ReadSetting(string key)
        {
            try
            {
                NameValueCollection settings = ConfigurationManager.AppSettings;
                return settings[key] ?? "";
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine($"Error reading setting {key}.");
            }
            return "";
        }
        static void InitializeDatabase(string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_tracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Start TEXT,
                    End TEXT,
                    Duration TEXT
                    )";
                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        static void SqlWrite(string command, string connectionString)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand tableCmd = connection.CreateCommand();
                tableCmd.CommandText = command;
                int rowCount = tableCmd.ExecuteNonQuery();
                if (rowCount == 0)
                {
                    Console.Clear();
                    Console.Write($"\nNo records changed. Verify database command.\n\nConnection: {connectionString}\nCommand: {command}");
                    Console.ReadKey();
                }
                connection.Close();
            }
        }
        static List<List<object>> SqlRead(string command, string connectionString)
        {
            List<List<object>> tableData = new();
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = command;
                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CodingSession session = new CodingSession
                        {
                            ID = reader.GetInt32(0),
                            Start = DateTime.Parse(reader.GetString(1), CultureInfo.InvariantCulture, 0),
                            End = DateTime.Parse(reader.GetString(2), CultureInfo.InvariantCulture, 0),
                        };
                        tableData.Add(new List<object> {session.ID, session.Start, session.End, session.Duration()});
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Write($"\nNo rows found. Verify database command.\n\nConnection: {connectionString}\nCommand: {command}");
                    Console.ReadKey();
                }
                connection.Close();
            }
            return tableData;
        }
        static void PrintTable(List<List<object>> data)
        {
            ConsoleTableBuilder
                .From(data)
                .WithTitle("Tracked Coding Sessions")
                .WithColumn("ID", "Start", "End", "Duration")
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                    {0, TextAligntment.Center },
                    {1, TextAligntment.Center },
                    {2, TextAligntment.Center },
                    {3, TextAligntment.Right }
                })
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '═' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╤' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, ' ' },
                    })
                .ExportAndWriteLine();
        }
    }
}