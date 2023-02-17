using CodingTracker.kraven88.Models;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.kraven88.Data;

internal class SqliteDB
{
    private readonly string DBname = ConfigurationManager.AppSettings["DBname"]!;

    public SqliteDB()
    {
        if (File.Exists(DBname) == false)
            CreateDatabase(DBname);
    }

    private static string LoadConnectionString(string id = "Default")
    {
        return ConfigurationManager.ConnectionStrings[id].ConnectionString;
    }

    private void CreateDatabase(string DBname)
    {
        SQLiteConnection.CreateFile(DBname);
        var sql =
            $@"CREATE TABLE ""Sessions"" (
            ""Id""     INTEGER NOT NULL,
            ""StartDate"" TEXT NOT NULL,
            ""EndDate""   TEXT NOT NULL,
            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );";

        SaveData(sql);
    }

    internal void DeleteById(string sqlCommand, CodingSession session)
    {
        using (var connection = new SQLiteConnection(LoadConnectionString()))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = sqlCommand;
            sql.Parameters.AddWithValue("@id", session.Id);

            sql.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void SaveData(string sqlCommand)
    {
        using (var connection = new SQLiteConnection(LoadConnectionString()))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = sqlCommand;
            sql.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void SaveData(string sqlCommand, CodingSession session)
    {
        using (var connection = new SQLiteConnection(LoadConnectionString()))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = sqlCommand;
            sql.Parameters.AddWithValue("@start", session.Start);
            sql.Parameters.AddWithValue("@end", session.End);

            sql.ExecuteNonQuery();
            connection.Close();
        }
    }

    public List<CodingSession> LoadData(string sqlCommand)
    {
        var output = new List<CodingSession>();

        using (var connection = new SQLiteConnection(LoadConnectionString()))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = sqlCommand;
            var reader = sql.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var cs = new CodingSession();
                    cs.Id = reader.GetInt32(0);
                    cs.Start = DateTime.Parse(reader.GetString(1));
                    cs.End = DateTime.Parse(reader.GetString(2));
                    output.Add(cs);
                }
            }
            reader.Close();
            connection.Close();
        }

        return output;
    }

    public List<CodingSession> LoadData(string sqlCommand, CodingSession session)
    {
        var output = new List<CodingSession>();

        using (var connection = new SQLiteConnection(LoadConnectionString()))
        {
            connection.Open();
            var sql = connection.CreateCommand();
            sql.CommandText = sqlCommand;
            sql.Parameters.AddWithValue("@start", session.Start);
            sql.Parameters.AddWithValue("@end", session.End);
            var reader = sql.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var cs = new CodingSession();
                    cs.Id = reader.GetInt32(0);
                    cs.Start = DateTime.Parse(reader.GetString(1));
                    cs.End = DateTime.Parse(reader.GetString(2));
                    output.Add(cs);
                }
            }
            reader.Close();
            connection.Close();
        }

        return output;
    }
}
