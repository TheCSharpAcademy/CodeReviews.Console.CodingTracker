using System.Globalization;

namespace CodingTracker.Furiax
{
	internal class Validation
	{
		internal static bool ValidateDate(string? input)
		{
			if (string.IsNullOrWhiteSpace(input)) return false;
			if (!DateTime.TryParseExact(input, "dd/MM/yy HH:mm", new CultureInfo("nl-BE"), DateTimeStyles.None, out _))
				return false;
			if (DateTime.Parse(input) > DateTime.Now ) 
				return false;
			return true;
		}
	}
}
	