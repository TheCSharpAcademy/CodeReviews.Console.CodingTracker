using DataAcess;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTrackerApp
{
    public class Spin
    {
        public static async Task Dots(string msg)
        {
            Console.OutputEncoding = Encoding.UTF8;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots2)
                .SpinnerStyle(Style.Parse("red"))
                .StartAsync(msg, async (StatusContext x) => { await Task.Delay(1500); });
        }
        public static async Task Check(bool openApp)
        {
            using (MyDbContext db = new MyDbContext())
            {
                db.Database.EnsureCreated();
                await Dots("Checking database connection...");

                if (!db.Database.CanConnect()) AnsiConsole.Write(new Markup("\n[red]No Database Exist[/]\n"));
                else
                {
                    AnsiConsole.Write(new Markup("\n[red]Database Exist[/]\n"));
                    while (openApp)
                    {
                        View.DisplayMainMenu(out int userInput);
                        Logic.Do(userInput, out bool status);
                        openApp = status;
                    }
                }
            }
        }
    }
}
