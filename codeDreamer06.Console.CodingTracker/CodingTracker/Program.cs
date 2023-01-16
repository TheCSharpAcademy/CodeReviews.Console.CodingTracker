namespace CodingTracker;

class Program
{
    static void Main()
    {
        const string help = @"
# Welcome to Code Time!
  It's a simple code time manager to measure your progress!
* exit or 0: stop the program
* show: display logs
* add [hours, optional: minutes]: insert data into the database
* update [id] [hours]: change existing data
* remove [id]: delete a log
";
        SqlAccess.CreateTable();
        Console.WriteLine(help);

        while (true)
        {
            var rawCommand = Console.ReadLine()!;
            var command = rawCommand.ToLower().Trim();

            if (command is "exit" or "0") break;

            else if (command is "help") Console.WriteLine(help);

            else if (command.StartsWith("add"))
            {
                var duration = rawCommand.SplitTime("add");
                if (duration is null) continue;

                if (!duration.Value.IsDurationValid())
                    Console.WriteLine("You can't code all day! Didn't you spend some time logging this? ;)");

                else SqlAccess.AddLog(duration.Value);
            }

            else if (command.StartsWith("remove"))
            {
                if (rawCommand is "remove")
                {
                    SqlAccess.RemoveLastLog();
                    continue;
                }

                int id = rawCommand.GetNumber("remove");

                if (id == 0)
                {
                    Console.WriteLine("Please enter a valid number!");
                    continue;
                }

                if (!SqlAccess.LogExists(id))
                {
                    Console.WriteLine("The log you are looking for doesn't exist.");
                    continue;
                }

                SqlAccess.RemoveLog(id);
            }

            else if (command == "show") SqlAccess.ShowLogs();

            else if (command.StartsWith("update"))
            {
                if (command.IsInvalidForUpdate()) continue;

                var commandProperties = command.RemoveKeyword("update");
                if (commandProperties is null) continue;

                var id = commandProperties.Split()[0].GetNumber("");
                var duration = commandProperties.Split()[1].SplitTime();

                 if (duration is null) continue;

                if (!SqlAccess.LogExists(id))
                {
                    Console.WriteLine("The log you are looking for doesn't exist.");
                    continue;
                }

                SqlAccess.UpdateLog(id, duration.Value);
            }

            else if (string.IsNullOrWhiteSpace(command)) continue;

            else Console.WriteLine("Not a command. Use 'help' if required.");
        }
    }
}
