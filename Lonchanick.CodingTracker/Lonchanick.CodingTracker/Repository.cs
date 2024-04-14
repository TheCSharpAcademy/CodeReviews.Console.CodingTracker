using Microsoft.Data.SqlClient;
using Dapper;

namespace Lonchanick.CodingTracker;

internal class Repository
{
    static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial"
    + " Catalog=CSharpAcademy;Integrated Security=True;Connect"
    + " Timeout=30;Encrypt=False;Trust Server Certificate=False;Application"
    + " Intent=ReadWrite;Multi Subnet Failover=False";

    internal static List<CodingSession>? Select()
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "select * from CodingSession";

            var result = conn.Query<CodingSession>(query).ToList();
            if (result is not null)
                return result;
            else 
                return null;
        }

    }

    internal static bool Insert(CodingSession codSess)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO CodingSession" +
                " (DateTimeSessionInit, DateTimeSessionEnd, Duration)" +
                " VALUES(@V1,@V2,@V3)";
            
            var parameters = new
            {
                V1 = codSess.DateTimeSessionInit,
                V2 = codSess.DateTimeSessionEnd,
                V3 = codSess.Duration
            };

            int rowsAffected = conn.Execute(query, parameters);

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }
    }

    internal static bool Update(CodingSession codSess)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "UPDATE CodingSession" +
                " SET DateTimeSessionInit = @V1," +
                " DateTimeSessionEnd = @V2," +
                " Duration = @V3" +
                " WHERE Id = @Id";

            var parameters = new
            {
                V1 = codSess.DateTimeSessionInit,
                V2 = codSess.DateTimeSessionEnd,
                V3 = codSess.Duration,
                Id = codSess.Id
            };

            int rowsAffected = conn.Execute(query, parameters);

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }
    }

    internal static bool Delete(int Id)
    {
        using (var conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM CodingSession" +
                " WHERE Id = @Id";

            var parameters = new
            {
                Id = Id
            };

            int rowsAffected = conn.Execute(query, parameters);

            if (rowsAffected > 0)
                return true;
            else
                return false;
        }
    }
}
