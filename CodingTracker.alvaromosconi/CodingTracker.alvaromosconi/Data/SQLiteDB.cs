using CodingTracker.alvaromosconi.Model;
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

    public List<CodeSessionModel> GetData(string query)
    {
        var output = new List<CodeSessionModel>();

        using (var connection = new SqliteConnection(DBConstants.CONNECTION_STRING))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = query;
            var reader = sql.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    output.Add(
                      new CodeSessionModel
                      {
                          Id = reader.GetInt32(0),
                          StartDateTime = DateTime.Parse(reader.GetString(1)),
                          EndDateTime = DateTime.Parse(reader.GetString(2))
                      });
                }
            }
            reader.Close();
            connection.Close();
        }

        return output;
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
