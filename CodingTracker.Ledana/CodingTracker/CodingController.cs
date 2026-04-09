using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.Ledana
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        internal void Post(Coding coding)
        {
            using SqlConnection connection = new(connectionString);

            var insertQuery = $"INSERT INTO coding ([Date], [Start], [End], [Duration]) Values (@Date, @Start, @End, @Duration)";

            var rowsAffected = connection.Execute(insertQuery, coding);
        }
        internal void Get()
        {
            using SqlConnection connection = new(connectionString);

            var getQuery = "SELECT * FROM coding";
            var codingSessions = connection.Query<Coding>(getQuery).ToList();

            TableVisualisation.ShowTable(codingSessions);
        }

        internal Coding GetById(int id)
        {
            using SqlConnection connection = new(connectionString);
            
            var getByIdQuery = $"SELECT * FROM coding WHERE [Id] = @Id";

            var coding = connection.QuerySingleOrDefault<Coding>(getByIdQuery, new { Id = id });
            return coding;
        }

        internal void Delete(int id)
        {
            using SqlConnection connection = new(connectionString);
            
            var deleteQuery =  $"DELETE FROM coding WHERE [Id] = @Id";
            var rowsAffected = connection.Execute(deleteQuery, new {Id = id});

            Console.WriteLine($"\n\nRecord with Id {id} was deleted. \n\n");
        }

        internal void Update(Coding coding)
        {
            using SqlConnection connection = new(connectionString);
            
            var updateQuery = $"UPDATE coding SET [Date] = @Date, [Start] = @Start, [End] = @End, [Duration] = @Duration WHERE [Id] = @Id";
            
            var rowsAffected = connection.Execute(updateQuery, coding);

            Console.WriteLine($"\n\nRecord with Id {coding.Id} was updated. \n\n");
        }
    }
}