using Microsoft.Data.Sqlite;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace CodingTracker.Ramseis
{
    internal class FileIO
    {
        public static void WriteSetting(string key, string value)
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
        public static string ReadSetting(string key)
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
        public static void InitializeDatabase(string connectionString)
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
        public static void SqlWrite(string command, string connectionString)
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
                    Console.Clear();
                }
                connection.Close();
            }
        }
        public static List<List<object>> SqlRead(string command, string connectionString)
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
                        tableData.Add(new List<object> { session.ID, session.Start, session.End, session.Duration() });
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Write($"\nNo rows found. Verify database command.\n\nConnection: {connectionString}\nCommand: {command}");
                    Console.ReadKey();
                    Console.Clear();
                }
                connection.Close();
            }
            return tableData;
        }
    }
}
