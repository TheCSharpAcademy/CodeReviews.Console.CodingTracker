using ConsoleTableExt;

using System.Data;


namespace CodingTracker.Paul_W_Saltzman
{
    internal class Helpers
    {
        internal static void ShowTable(DataTable dataTable, String title)
        {
            ConsoleTableBuilder
            .From(dataTable)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .WithTitle(title, ConsoleColor.Red, ConsoleColor.Black, TextAligntment.Center)
                .ExportAndWriteLine(TableAligntment.Center);
        }
        internal static void CenterText(string centered)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (centered.Length / 2)) + "}", centered));
        }

        internal static DateTime DateTimeBuilder(DateOnly dateOnly, TimeOnly timeOnly)
        {
            DateTime dateTime = dateOnly.ToDateTime(timeOnly);
            return dateTime;
        }

        internal static void DailyTrophy(DailyTotals dailyTotal)
        {
            string asciiArt = @"
         |                              ____.......__
         |\\      .'           _.--""''``             ``''--._
         | \\   .'/      ..--'`                             .-'`
  .._    |  \\.' /  ..-''                                .-'
   '.``'"":  '  .-'`                                  .-'``
     '.             __...----""""""""""--..           \\
     /         ..-''                       ``""-._     \\
    /  _.       \\                                --`\   \\
  _.- '` |  /-.  \\                                   `-. \\
        | //   `. \\                                      `.\\    
        |//      `-\\
        | ";

            // Print ASCII art to console
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(asciiArt);
                Thread.Sleep(500);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(asciiArt);
                Thread.Sleep(500);
            }


            Console.ResetColor();
            Helpers.CenterText($@"Congratulations You Have met your Daily Goal for {dailyTotal.Date}");
            Helpers.CenterText("Press ENTER to Continue");
            Helpers.CenterCursor();
            Console.ReadLine();

        }

        internal static void WeeklyTrophy(WeeklyTotals weeklyTotal)
        {
            string asciiArt = @"
         |                              ____.......__
         |\\      .'           _.--""''``             ``''--._
         | \\   .'/      ..--'`                             .-'`
  .._    |  \\.' /  ..-''                                .-'
   '.``'"":  '  .-'`                                  .-'``
     '.             __...----""""""""--..__         \\
     /         ..-''                       ``""-._     \\
    /  _.       \\                                --`\  \\
  _.- '` |  /-.  \\                                   `-. \\
        | //   `. \\                                      `.\\    
        |//      `-\\
        | ";

            // Print ASCII art to console
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(asciiArt);
                Thread.Sleep(500);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(asciiArt);
                Thread.Sleep(500);
            }


            Console.ResetColor();
            Helpers.CenterText($@"Congratulations You Have met your Weekly Goal for {weeklyTotal.YearWeek}");
            Helpers.CenterText("Press ENTER to Continue");
            Helpers.CenterCursor();
            Console.ReadLine();

        }

        internal static int LastWeek(int showYearWeek)
        {
            showYearWeek --;
            int lastTwoDigits = showYearWeek % 100;

            if (lastTwoDigits == 0)//no week 0
            {
                showYearWeek -= 48;
            }
            return showYearWeek;
        }
        internal static int NextWeek(int showYearWeek)
        {
            showYearWeek ++;
            int lastTwoDigits = showYearWeek % 100;

            if (lastTwoDigits == 53)//no week 53
            {
                showYearWeek += 48;
            }
            return showYearWeek;
        }
        internal static void ClearLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        internal static void CenterCursor()
        {
            int screenWidth = Console.WindowWidth;
            int cursorLine = Console.CursorTop;

            int centerRow = screenWidth / 2;
            Console.SetCursorPosition(centerRow, cursorLine);
        }

        internal static void Output(string userMessage)
         {
            Console.SetCursorPosition(0, Console.CursorTop - 3);
            ClearLine();
            CenterText(userMessage);
            Console.WriteLine();
            ClearLine();
            CenterCursor();
        }

        internal static DateTime GenerateRandomDateTime()
        {
            Random gen = new Random(); 
            
            DateTime start = new DateTime(2015, 1, 1);
            int rangeDays = (DateTime.Today - start).Days;
            DateTime randomDateTime = start.AddDays(gen.Next(rangeDays));
            randomDateTime = randomDateTime.AddHours(gen.Next(24));
            randomDateTime = randomDateTime.AddMinutes(gen.Next(60));
            return randomDateTime;

        }

        internal static DateTime GenerateEndTime(DateTime start)
        {
            Random gen = new Random();
            DateTime end = start;
            end = end.AddHours(gen.Next(1,5));
            int minAdd = gen.Next(1,5);
            switch (minAdd)
            {
                case 1:
                    end = end.AddMinutes(15);
                    break;
                case 2:
                    end = end.AddMinutes(30);
                    break;
                case 3:
                    end = end.AddMinutes(45);
                    break;
                case 4:
                    //add 0 minutes
                    break;
            }
            return end;
        }  
        
        
    }
}
