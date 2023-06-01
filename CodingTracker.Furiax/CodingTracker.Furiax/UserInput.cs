using CodingTracker.Furiax;
using System.Diagnostics;

namespace CodingTracker.Furiax
{
	public static class UserInput
	{
		public static void GetUserInput(string connectionString)
		{
			bool closeApp = false;
			while (!closeApp)
			{
				Menu();
				string menuChoice = Console.ReadLine();
				switch (menuChoice)
				{
					case "1":
						Crud.InsertRecord(connectionString); Console.ReadLine(); Console.Clear();
						break;
					case "2":
						Crud.ShowTable(connectionString); Console.ReadLine(); Console.Clear();
						break;
					case "3":
						Crud.UpdateRecord(connectionString); Console.ReadLine(); Console.Clear();
						break;
					case "4":
						Crud.DeleteRecord(connectionString); Console.ReadLine(); Console.Clear();
						break;
					case "5":
						UseStopwatch(connectionString); Console.ReadLine(); Console.Clear();
						break;
					//case "6":
					//	SetGoal(); Console.Clear();
					//	break;
					case "0":
						closeApp = true; Environment.Exit(0);
						break;
					default: Console.WriteLine("Inval input, make choice by entering the corresponding number");
							 menuChoice = Console.ReadLine();
						break;
				}
			}
		}

		internal static void UseStopwatch(string connectionString)
		{
			Console.WriteLine("Press any key to start the stopwatch.");
			Console.ReadKey();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			DateTime getStartTime = DateTime.Now;
			string startTime = getStartTime.ToString("dd/MM/yy HH:mm");
            Console.WriteLine("\nStopwatch has started, press any key to stop the time and add the timings to the database");
			Console.ReadKey();
            Console.WriteLine();
            stopwatch.Stop();
			DateTime getStopTime = DateTime.Now;
			string stopTime = getStopTime.ToString("dd/MM/yy HH:mm");
			Crud.InsertStopwatchRecord(connectionString, getStartTime, getStopTime, startTime, stopTime);
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
            Console.WriteLine("5. Stopwatch");
            //Console.WriteLine("6. Set goal");
            Console.WriteLine("0. Close the application");
            Console.WriteLine("---------------------------");
        }
		public static DateTime GetStartDate(string question)
		{
			DateTime output = DateTime.Now;		
			bool isValid = false;
			while (isValid == false)
			{
				Console.Write(question + " : ");
				string input = Console.ReadLine();
				isValid = Validation.ValidateDate(input);
				if (isValid)
				{ 
					output = DateTime.Parse(input);
                }
				else
				{ Console.WriteLine("Invalid date or date in future"); }
			}

			return output;
		}
		public static DateTime GetEndDate(string question, DateTime startTime)
		{
			DateTime output = DateTime.Now;
			bool isValid = false;
			while (isValid == false)
			{
				Console.Write(question + " : ");
				string input = Console.ReadLine();
				isValid = Validation.ValidateDate(input);
				if (isValid)
				{
					output = DateTime.Parse(input);
					if (output < startTime)
					{
						Console.WriteLine("End time can't be before start time");
						isValid = false;
					}
				}
				else
				{ Console.WriteLine("Invalid date or date in future"); }
			}

			return output;
		}
		public static int GetId(string question)
		{
			int output = 0;
			bool isValid = false;
			while (isValid == false)
			{
				Console.Write(question);
				string input = Console.ReadLine();
				isValid = Validation.ValidateId(input);
				if (isValid)
					output = Convert.ToInt32(input);
				else
                    Console.WriteLine("input is not a number, try again");

            }
			return output;
		}
	}
}
