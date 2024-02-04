using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace CodingTracker;

public class CodingSessionController
{
    private string connectionString;
    private SqliteConnection connection;
    private SqliteCommand tableCmd;

    public CodingSessionController()
    {
        this.connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        this.connection = new SqliteConnection(this.connectionString);
        this.tableCmd = this.connection.CreateCommand();
    }

    public List<CodingSession> GetAllRecords()
    {
        this.connection.Open();
        List<CodingSession> tableData = new();

        try
        {
            this.tableCmd.CommandText = $"SELECT * FROM coding_session ";
            SqliteDataReader reader = this.tableCmd.ExecuteReader();

            while (reader.Read())
            {
                tableData.Add( new CodingSession(
                     reader.GetInt32(0),
                     DateTime.ParseExact(reader.GetString(1), "MM/dd/yyyy", new CultureInfo("en-US")),
                     DateTime.ParseExact(reader.GetString(2), "HH:mm", new CultureInfo("en-US")),
                     DateTime.ParseExact(reader.GetString(3), "HH:mm", new CultureInfo("en-US"))
                ) );
            }
            reader.Close();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        this.connection.Close();
        return tableData;
    }

    public void Insert(string date ,string startAt , string endAt)
    {
        this.connection.Open();
        try
        {
            this.tableCmd.CommandText = $@"INSERT INTO coding_session(Date,StartAt,EndAt) VALUES('{date}','{startAt}','{endAt}')";
            this.tableCmd.ExecuteNonQuery();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        this.connection.Close();
    }

    public void Update(int id , string date, string startAt, string endAt)
    {

        connection.Open();

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_session WHERE Id = {id})";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery == 0)
        {
            Console.WriteLine($"\n\nRecord with Id {id} doesn't exist.\n\n");
            connection.Close();
            return;
        }

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = $"UPDATE coding_session SET date = '{date}', StartAt = '{startAt}' , EndAt = '{endAt}' WHERE Id = '{id}'";
        tableCmd.ExecuteNonQuery();

        connection.Close();
    }

    public void Delete(int id)
    {
        try
        {
            this.connection.Open();
            this.tableCmd.CommandText = $@"DELETE FROM coding_session WHERE Id={id}";
            this.tableCmd.ExecuteNonQuery();
            this.connection.Close();
        }catch(Exception ex)
        {
            Console.WriteLine($"Err : {ex.Message}");
        }
    }

    public void Seed()
    {
        this.connection.Open();
        this.tableCmd.CommandText = $@"INSERT INTO coding_session(Date,StartAt,EndAt) VALUES
                        ('01/01/2024' , '06:00' , '13:00'),
                        ('01/02/2024' , '08:00' , '16:00'),
                        ('01/03/2024' , '10:00' , '20:00'),
                        ('01/04/2024' , '05:00' , '16:00'),
                        ('01/05/2024' , '06:00' , '16:00'),
                        ('01/06/2024' , '06:00' , '22:00')";

        this.tableCmd.ExecuteNonQuery();
        this.connection.Close();
    }
}
