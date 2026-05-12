using CodingTracker.urasylmaz1.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.urasylmaz1.Helpers
{
    public class Validation
    {
        public static bool IsValidDateTime(string input)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out _); // TryParseExact returns true if parsing is successful, false otherwise
        }
        public static bool SessionExists(string connectionString, int id)
        {
            using var connection = new SqliteConnection(connectionString);

            string sql ="SELECT * FROM CodingSessions WHERE Id = @Id";

            var session = connection.QuerySingleOrDefault<CodingSession>(sql,new { Id = id } );

            return session != null;
        }
    }
}
