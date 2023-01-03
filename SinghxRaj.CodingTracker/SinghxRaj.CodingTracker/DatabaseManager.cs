using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinghxRaj.CodingTracker
{
    internal class DatabaseManager
    {
        public static void CreateTable(string connectionString)
        {
            using(var conn = new SqliteConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = 
                                @"CREATE TABLE IF NOT EXISTS CODING_SESSION (
                                 Id INTEGER PRIMARY KEY AUTOINCREMENT
                                 Date DATETIME
                                 Duration INTEGER
                                )";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
