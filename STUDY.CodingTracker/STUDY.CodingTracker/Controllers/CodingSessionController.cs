using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using STUDY.CodingTracker.Helper;
using STUDY.CodingTracker.Models;
using System.Globalization;

namespace STUDY.CodingTracker.Controllers;

internal class CodingSessionController
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public CodingSessionController(IConfiguration config)
    {
        _config = config;
        var section = _config.GetSection("DataBaseSettings");
        _connectionString = section["connectionString"] + section["databasePath"];

        CreateTable();
    }

    public List<CodingSessionModel> GetCodingSessions(FilterChoice filterChoice, DateTime periodDate, OrderChoice orderChoice)
    {
        List<CodingSessionModel> codingSessions = new();
        List<CodingSessionModel> filteredCodingSessions = new List<CodingSessionModel>();

        string order = orderChoice == OrderChoice.Descending ? "ORDER BY STARTTIME DESC" : orderChoice == OrderChoice.Ascending ? "ORDER BY STARTTIME ASC" : "";

        string command = $@"
            SELECT STARTTIME start,
            ENDTIME end, 
            ID newId, 
            DURATION newDuration FROM codingSessions
            {order}
        ";

        try
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            codingSessions = connection.Query<CodingSessionModel>(command).AsList();
        }
        catch (SqliteException e)
        {
            DBErrorMessage("getting the saved coding sessions", e.Message);
        }

        filteredCodingSessions = FilterCodingSession(filterChoice, periodDate, codingSessions);

        return filteredCodingSessions;
    }

    private static List<CodingSessionModel> FilterCodingSession(FilterChoice filterChoice, DateTime periodDate, List<CodingSessionModel> codingSessions)
    {
        switch (filterChoice)
        {
            case FilterChoice.Week:
                return codingSessions.FindAll(c => ISOWeek.GetWeekOfYear(c.startTime) == ISOWeek.GetWeekOfYear(periodDate) || ISOWeek.GetWeekOfYear(c.endTime) == ISOWeek.GetWeekOfYear(periodDate));
            case FilterChoice.Day:
                return codingSessions.FindAll(c => c.startTime.Date == periodDate || c.endTime.Date == periodDate);
            case FilterChoice.Year:
                return codingSessions.FindAll(c => c.startTime.Year == periodDate.Year || c.endTime.Year == periodDate.Year);
            default:
                return codingSessions;
        }
    }

    public bool AddCodingSession(CodingSessionModel codingSession)
    {
        bool success = false;

        try
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            string sql = "INSERT INTO codingSessions (STARTTIME, ENDTIME, DURATION) VALUES (@StartTime, @EndTime, @Duration)";
            var parameters = new { @StartTime = codingSession.startTime, @EndTime = codingSession.endTime, @Duration = codingSession.duration };
            connection.Execute(sql, parameters);

            success = true;
        }
        catch (SqliteException e)
        {
            DBErrorMessage("adding the coding session", e.Message);
        }

        return success;
    }

    public int DeleteCodingSession(int idToDelete)
    {
        int numberOfRowsDeleted = 0;

        try
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            numberOfRowsDeleted = connection.Execute("DELETE FROM codingSessions WHERE ID = @IdToDelete", new { @IdToDelete = idToDelete });
        }
        catch (SqliteException e)
        {
            DBErrorMessage("deleting the coding session", e.Message);
        }

        return numberOfRowsDeleted;
    }

    public int UpdateCodingSession(int idToUpdate, CodingSessionModel updatedCodingSession)
    {
        int numberOfRowsUpdated = 0;

        try
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            string sql = "UPDATE codingSessions SET STARTTIME = @StartTime, ENDTIME = @EndTime, DURATION = @Duration WHERE ID = @IdToUpdate";
            var parameters = new { @StartTime = updatedCodingSession.startTime, @EndTime = updatedCodingSession.endTime, @Duration = updatedCodingSession.duration, @IdToUpdate = idToUpdate };

            numberOfRowsUpdated = connection.Execute(sql, parameters);
        }
        catch (SqliteException e)
        {
            DBErrorMessage("updating the coding session", e.Message);
        }

        return numberOfRowsUpdated;
    }

    private void CreateTable()
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);

            connection.Open();

            connection.Execute(
                @"CREATE TABLE IF NOT EXISTS codingSessions(
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                STARTTIME TEXT,
                ENDTIME TEXT,
                DURATION TEXT
            )"
            );
        }
        catch (SqliteException e)
        {
            DBErrorMessage("creating the DB Table", e.Message);
        }
    }

    private void DBErrorMessage(string action, string errorMessage)
    {
        Console.WriteLine($"An error occured while {action}. Error: {errorMessage}");
    }
}
