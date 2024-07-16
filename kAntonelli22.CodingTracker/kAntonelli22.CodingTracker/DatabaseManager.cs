using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DatabaseManager
{
    internal class DatabaseManager
    {
        string connString;
        string dbPath;
        public DatabaseManager(string databasePath, string connectionString)
        {
            this.dbPath = databasePath;
            this.connString = connectionString;
        } // end of DatabaseManager Constructor

        void RunQuery(string query)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message} in RunQuery");
            }
        } // end of RunQuery method

        static Tuple<List<string>, List<string>> SelectData(string query)
        {
            List<string> results1 = new List<string>();
            List<string> results2 = new List<string>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results1.Add(reader.GetString(1));
                                results2.Add(reader.GetString(2));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured: {ex.Message} in SelectData");
            }
            return new Tuple<List<string>, List<string>>(results1, results2);
        } // end of SelectData method

    } // end of DatabaseManager Class
} // end of DatabaseManager Namespace
