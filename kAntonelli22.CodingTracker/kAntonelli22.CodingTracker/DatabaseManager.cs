using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace CodingTracker.DatabaseManager
{
    internal class dbManager
    {
        string connString;
        string dbPath;
        public dbManager(string databasePath, string connectionString)
        {
            this.dbPath = databasePath;
            this.connString = connectionString;
        } // end of DatabaseManager Constructor

        public void RunQuery(string query)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
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

        public Tuple<List<string>, List<string>> SelectData(string query)
        {
            List<string> results1 = new List<string>();
            List<string> results2 = new List<string>();
            try
            {
                using (var connection = new SQLiteConnection(connString))
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

    } // end of dbManager Class
} // end of DatabaseManager Namespace
