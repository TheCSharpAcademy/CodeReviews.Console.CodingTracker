namespace CodingTracker.Furiax
{
	internal class Crud
	{
		internal static void DeleteRecord()
		{
			Console.Clear();
			Console.WriteLine("delete");
		}

		internal static void UpdateRecord()
		{
			Console.Clear();
			Console.WriteLine("update");
		}

		internal static void ShowTable()
		{
			Console.Clear();
			Console.WriteLine("view data");
		}

		internal static void InputDates()
		{
			Console.Clear();
			DateTime input = UserInput.AskDate("Please enter the start date in format dd/mm/yy hh:mm");
				
			//valid date test passed, output is wel met seconden (nakijken op gevolgen)
			//to do next: test with bad date
		}
	}
}
