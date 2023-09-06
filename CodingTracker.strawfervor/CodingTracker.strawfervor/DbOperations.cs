using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    partial class Tracker
    {
        string connectionString = @"Data Source=Coding-tracker.db";
        public Tracker()
        {
            string create_table_command = @"CREATE TABLE IF NOT EXISTS coding_tracker (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                StartTime TEXT,
                EndTime TEXT
                )";
            CommandNonQuery(create_table_command);
        }

        public void CommandNonQuery(string command)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = command;

                int rowCount = tableCmd.ExecuteNonQuery();//this is executing command without "output"

                if (rowCount > 0)
                {
                    Console.Write($"Record updated!");
                }

                connection.Close();
            }
        }

        public void NewEntry()
        {
            Console.Clear();
            Console.WriteLine($"\nAdding new entry.\n");
            String date = InputDate();
            string[] times = InputCorrectStartAndEndTime();

            string query = $"INSERT INTO coding_tracker(Date, StartTime, EndTime) VALUES('{date}', '{times[0]}', '{times[1]}')";

            CommandNonQuery(query);
            Console.Write(" Press any key.");
            Console.ReadLine();
            Console.Clear();
        }

        public void DeleteEntry()
        {
            Console.Clear();
            Console.WriteLine($"\nDeleting entry.\n");
            ShowTable();
            Console.WriteLine("Please enter ID of entry to delete or any other number to exit: ");
            int userSelection = InputNumber();

            if (userSelection > 0)
            {
                string currentQuery = $"DELETE FROM coding_tracker WHERE Id = '{userSelection}'";
                CommandNonQuery(currentQuery);
                Console.Write("Press any key.");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public void UpdateEntry()
        {
            Console.Clear();
            Console.WriteLine($"\nUpdating entry.\n");
            ShowTable();
            Console.WriteLine("Please choose ID of entry to update or any other number to exit: ");
            int userSelection = InputNumber();

            if (userSelection > 0 && CheckIfExists(userSelection))
            {
                String date = InputDate();
                string[] times = InputCorrectStartAndEndTime();

                string query = $"UPDATE coding_tracker SET Date = '{date}', StartTime = '{times[0]}', EndTime = '{times[1]}' WHERE Id = '{userSelection}'";

                CommandNonQuery(query);
                Console.Write(" Press any key.");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                Console.Clear();
            }
        }

        public List<CodingSession> AllRecords()
        {
            List<CodingSession> tableData = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT * FROM coding_tracker";

                SqliteDataReader reader = tableCmd.ExecuteReader(); //we are using this reader instead of ExecuteNonQuery because we want to 'feed' reader with data from DB
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),//0 indicate number of collumn
                            Date = reader.GetString(1),
                            StartTime = reader.GetString(2),
                            EndTime = reader.GetString(3),
                            Duration = CodingSession.CalculateDuration(reader.GetString(2), reader.GetString(3))
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found!");
                }
                connection.Close();
            }

            return tableData;
        }

        public bool CheckIfExists(int number)
        {
            bool exists = false;

            string command = $"SELECT count(*) FROM coding_tracker WHERE Id = '{number}'";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = command;

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    exists = true;
                }

                connection.Close();
            }

            return exists;
        }
    }
}
