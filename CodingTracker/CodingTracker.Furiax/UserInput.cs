using CodingTracker.Furiax;

namespace CodingTracker.Furiax
{
	public static class UserInput
	{
		public static void GetUserInput()
		{
			bool closeApp = false;
			while (!closeApp)
			{
				Menu();
				string menuChoice = Console.ReadLine();
				switch (menuChoice)
				{
					case "1":
						Crud.InputDates(); Console.ReadLine(); Console.Clear();
						break;
					case "2":
						Crud.ShowTable(); Console.ReadLine(); Console.Clear();
						break;
					case "3":
						Crud.UpdateRecord(); Console.ReadLine(); Console.Clear();
						break;
					case "4":
						Crud.DeleteRecord(); Console.ReadLine(); Console.Clear();
						break;
					/*case "5":
						ShowStopwatch(); Console.Clear();
						break;
					case "6":
						SetGoal(); Console.Clear();
						break;*/
					case "0":
						closeApp = true; Environment.Exit(0);
						break;
					default: Console.WriteLine("Inval input, make choice by entering the corresponding number");
							 menuChoice = Console.ReadLine();
						break;
				}
			}
		}

		public static void Menu()
		{
            Console.WriteLine("CODE TRACKER");
            Console.WriteLine("------------");
			Console.WriteLine("What do you want to do: ");
			Console.WriteLine("1. Enter the start and end time");
            Console.WriteLine("2. Overview coding time");
            Console.WriteLine("3. Change times");
			Console.WriteLine("4. Delete times");
            //Console.WriteLine("5. Stopwatch");
            //Console.WriteLine("6. Set goal");
            Console.WriteLine("0. Close the application");
            Console.WriteLine("---------------------------");
        }

		public static DateTime AskDate(string question)
		{
			DateTime output = DateTime.Now;
			bool isValid = false;
			while (isValid == false)
			{
				Console.WriteLine(question);
				string input = Console.ReadLine();
				isValid = Validation.ValidateDate(input);
				if (isValid)
				{ output = DateTime.Parse(input);
                    Console.WriteLine("Valid date");
                }
			}
			return output;
		}
	}
}
