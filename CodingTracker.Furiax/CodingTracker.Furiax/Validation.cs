using Microsoft.Data.Sqlite;
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
		internal static bool ValidateId(string? input) 
		{
			if (string.IsNullOrEmpty(input) || !Int32.TryParse(input, out _)) return false;
			if (Int32.Parse(input) < 0 ) return false;
			return true;
		}
		internal static bool CheckIfRecordExists(int recordId, string connectionString)
		{
			using (var connection = new SqliteConnection(connectionString))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = $"SELECT * FROM CodeTracker WHERE Id = '{recordId}'";
				SqliteDataReader reader = command.ExecuteReader();
				if (reader.HasRows)
					return true;
				else
					return false;
			}
		}

		internal static bool ValidInteger(string? input)
		{
			while (string.IsNullOrWhiteSpace(input) || !Int32.TryParse(input, out _) || Int32.Parse(input) <= 0)
			{
				return false;
            }
			return true;
		}
	}
}
	