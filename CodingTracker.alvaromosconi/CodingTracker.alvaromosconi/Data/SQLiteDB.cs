using Microsoft.Data.Sqlite;

namespace CodingTracker.alvaromosconi.Data;

internal class SQLiteDB
{
    public void OnCreate()
    {
        ExecuteQuery(DBConstants.CREATE_SESSIONS_TABLE);
    }
    
    public void ExecuteQuery(string query)
    {
        using (var connection = new SqliteConnection(DBConstants.CONNECTION_STRING))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = query;
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void Insert(string query)
    {
        ExecuteQuery(query);
    }
    public void Delete(string query)
    {
        ExecuteQuery(query);
    }
}
