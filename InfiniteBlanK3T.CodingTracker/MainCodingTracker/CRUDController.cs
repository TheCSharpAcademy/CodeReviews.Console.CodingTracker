using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Configuration;

namespace CodingTracker;

class CrudController
{        
    UserInput userInput = new();
	Validation val = new();
    readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;    

    public void GetAllRecords(string record)
	{
        if (CheckEmptyTable(record)) return;        

        using var conn = new SqliteConnection(connectionString);

		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"SELECT * FROM {record} ORDER BY Date DESC";	

		FromQueryToTable(tableCmd, record);
	}

	public void Insert(string record, string dateInsert, List<int> timeInsert)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();
		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"INSERT INTO {record} (Date, StartTime, EndTime, Duration) 
			VALUES ('{dateInsert}', '{timeInsert[0]}:{timeInsert[1].ToString("D2")}', 
			'{timeInsert[2].ToString("D2")}:{timeInsert[3].ToString("D2")}', {timeInsert[4]})";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }
    }

	public void Delete(string record)
	{
        if (CheckEmptyTable(record)) return;

        GetAllRecords(record);
        var recordId = val.GetNumber("Please type the Id of record you want to delete or Type 0 to back to the Menu: ");
		if (recordId == 0) return;        

        using var conn = new SqliteConnection(connectionString);
		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"DELETE FROM {record} WHERE Id = '{recordId}'";
		int rowCount = tableCmd.ExecuteNonQuery();

		if (rowCount == 0)
		{
			Console.WriteLine($"\n\nRecord with Id <<{recordId}>> does not exist. Press ENTER to try again.\n\n");
			Console.ReadLine();
			Console.Clear();
			Delete(record);
			return;
		}

		Thread.Sleep(1000);
        GetAllRecords(record);
        Console.WriteLine($"\n\nRecord with Id <<{recordId}>> was deleted.\n\n");
	}

	public void Update(string record)
	{
        if (CheckEmptyTable(record)) return;

		GetAllRecords(record);
        var recordId = val.GetNumber("Please type the Id of record you want to Update or Type 0 to back to the Menu: ");
        if (recordId == 0) return;

        using var conn = new SqliteConnection(connectionString);

        conn.Open();

        var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {record} WHERE Id = '{recordId}')";
        int rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (rowCount == 0)
        {
            Console.Write($"\n\nRecord with Id {recordId} does not exist. Press ENTER to try again.\n\n");
            Console.ReadLine();
            Console.Clear();
            Update(record);
            return;
        }

		string date = userInput.GetDate();
        List<int> timeInsert = userInput.GetUserTime();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"UPDATE {record} SET date = '{date}', 
			StartTime = '{timeInsert[0]}:{timeInsert[1]}',
			EndTime = '{timeInsert[2]}:{timeInsert[3]}',
			Duration = '{timeInsert[4]}'
            WHERE Id = '{recordId}'";
        try
        {
            tableCmd.ExecuteNonQuery();
            Thread.Sleep(1000);
            GetAllRecords(record);
            Console.WriteLine("\n\n Update Completed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }
    }

	internal bool CheckEmptyTable(string table)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();		
		var checkCmd = conn.CreateCommand();		
		checkCmd.CommandText = $"SELECT COUNT(*) FROM {table}";
		int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

		if (checkQuery == 0)
		{            
			Console.WriteLine("No record in this table.");
			return true;
		}

		return false;
	}

    public bool Report(string table)
    {
        Console.Clear();
		using var connection = new SqliteConnection(connectionString);
        
        connection.Open();
        if (CheckEmptyTable(table)) return false;

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT COUNT(*) FROM {table}";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery < 3)
        {
            Console.WriteLine($"\n\nInsert at least {3 - checkQuery} more entries for the report !\n\n");

            connection.Close();
			return false;
        }

		return true;        
    }

	public void ReportWithTimeFilter(string table)
	{        
        var filterQuery = userInput.GetReportFilter();
        using var conn = new SqliteConnection(connectionString);
		conn.Open();        
        var queryRecord = conn.CreateCommand();
		queryRecord.CommandText = $"SELECT * FROM {table} WHERE Date LIKE '{filterQuery[0]}-{filterQuery[1]}%' ORDER BY Date {filterQuery[2]}";

        Thread.Sleep(500);
		FromQueryToTable(queryRecord, table);      
        Console.WriteLine("-------------------------------");
        Console.WriteLine("\tREPORT SUMMARY");
        Console.WriteLine("-------------------------------");
        GetAverageTimeReport(table, filterQuery);
        GetMaxTimeReport(table, filterQuery);
        GetMinTimeReport(table, filterQuery);
    }

    public void GetAverageTimeReport(string table, List<string> list)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var queryAvgRecord = conn.CreateCommand();
        queryAvgRecord.CommandText = $"SELECT AVG(Duration) FROM {table} WHERE Date LIKE '{list[0]}-{list[1]}%'";
        try
        {
            var avgRecordInt = queryAvgRecord.ExecuteScalar();
            var avgRecord = $"Avearage time spent per day: {val.ConvertTime(avgRecordInt)}";
            Console.WriteLine(avgRecord);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }     
    }

    public void GetMaxTimeReport(string table, List<string> list) 
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var queryMaxRecord = conn.CreateCommand();
        var queryDay = conn.CreateCommand();
        queryMaxRecord.CommandText = $"SELECT MAX(DURATION) FROM {table} WHERE DATE LIKE '{list[0]}-{list[1]}%'";        
        try
        {
            var maxRecordInt = queryMaxRecord.ExecuteScalar();
            queryDay.CommandText = $"SELECT Date FROM {table} WHERE DATE LIKE '{list[0]}-{list[1]}%' AND DURATION = {Convert.ToInt16(maxRecordInt)}";
            var dayRecord = queryDay.ExecuteScalar();            
            var maxTimeReport = $"Day with highest time spent: {val.ConvertTime(maxRecordInt)} ({dayRecord})";
            Console.WriteLine(maxTimeReport);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void GetMinTimeReport(string table, List<string> list)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var queryMinRecord = conn.CreateCommand();
        var queryDay = conn.CreateCommand();
        queryMinRecord.CommandText = $"SELECT MIN(DURATION) FROM {table} WHERE DATE LIKE '{list[0]}-{list[1]}%'";        
        try
        {
            var minRecordInt = queryMinRecord.ExecuteScalar();
            queryDay.CommandText = $"SELECT Date FROM {table} WHERE DATE LIKE '{list[0]}-{list[1]}%' AND DURATION = {Convert.ToInt16(minRecordInt)}";
            var dayRecord = queryDay.ExecuteScalar();
            var maxTimeReport = $"Day with lowest time spent: {val.ConvertTime(minRecordInt)} ({dayRecord})";
            Console.WriteLine(maxTimeReport);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void FromQueryToTable(SqliteCommand query, string table)
    {
        DataVisualisation makingTable = new();
        List<CodingSession> tableData = new();
        try
        {
            SqliteDataReader reader = query.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", new CultureInfo("end-US")),
                            StartTime = reader.GetString(2),
                            EndTime = reader.GetString(3),
                            Duration = reader.GetInt32(4)
                        }
                        );
                }
            }
            else
            {
                Console.WriteLine("No rows found");
            }

            makingTable.ShowingTable(tableData, table);
        }
        catch (Exception ex)
        {
            Console.WriteLine (ex.Message );
        }
    }

    public bool GoalExists(string record)
    {
        using var conn = new SqliteConnection(connectionString);

        conn.Open();

        var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Goals WHERE Name = '{record}')";
        int rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (rowCount == 1) { return true; }
        else { return false; }
    }

    public void InsertGoal(string record, int time, int goal)
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var tableCmd = conn.CreateCommand();
        tableCmd.CommandText =
            $@"INSERT INTO Goals (Name, Date, TimePerDay, Goal) 
			VALUES ('{record}', '{today}', {time}, {goal})";
        try
        {
            tableCmd.ExecuteNonQuery();
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Goal set! Press ENTER to return.");
            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }        
    }

    public void UpdateGoal(string record, int time, int goal)
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var tableCmd = conn.CreateCommand();
        tableCmd.CommandText =
            $@"UPDATE Goals SET Date = '{today}', 
            TimePerDay = {time}, 
            Goal = {goal}  
            WHERE Name = '{record}'";
        try
        {
            tableCmd.ExecuteNonQuery();
            Thread.Sleep(500);
            Console.WriteLine("Update Completed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }
    }

    public void GetGoal(string table)
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var queryGoal = conn.CreateCommand();
        var queryTimePerDay = conn.CreateCommand();
        var queryDate = conn.CreateCommand();
        queryTimePerDay.CommandText = $"SELECT TimePerDay FROM Goals WHERE Name = '{table}'";
        queryGoal.CommandText = $"SELECT Goal FROM Goals WHERE Name = '{table}'";
        queryDate.CommandText = $"SELECT Date FROM Goals WHERE Name= '{table}'";
        Console.Clear();
        Console.WriteLine("-------------------------------");
        Console.WriteLine("\tYOUR GOAL SUMMARY");
        Console.WriteLine("-------------------------------");
        try
        {

            var userTimePerDay = queryTimePerDay.ExecuteScalar();
            var userGoal = queryGoal.ExecuteScalar();
            var getDate = queryDate.ExecuteScalar();
            Console.WriteLine($"Date Set: " + getDate);
            Console.WriteLine($"Time per day: {val.ConvertTime(userTimePerDay)}");
            Console.WriteLine($"Your Goal: {val.ConvertTime(userGoal)}");
            Console.WriteLine("-------------------------------");
            WhenGoalReach(table, userTimePerDay, userGoal);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void WhenGoalReach(string table, object userTimePerDay, object userGoal)
    {
        Console.WriteLine("-------------------------------");
        Console.WriteLine("\tYOUR PROGRESS");
        Console.WriteLine("-------------------------------");
        CompareTimeWithRecord(table, userTimePerDay);
        EstimateGoal(table, userGoal, userTimePerDay);
        Console.WriteLine("-------------------------------");
    }

    public void CompareTimeWithRecord(string table, object compare)
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        float userTimePerDay = Convert.ToInt32(compare);
        using var conn = new SqliteConnection (connectionString);
        conn.Open();
        var queryRecord = conn.CreateCommand();        
        queryRecord.CommandText = $"SELECT DURATION FROM {table} WHERE Date = '{today}'";
        try
        {
            float todaySpent = Convert.ToInt32(queryRecord.ExecuteScalar());
            if ((userTimePerDay - todaySpent) < 0)
            {
                Console.WriteLine("Congratulation! You have reached your Today Goal! ");
            }
            else
            {
                var timeNeed = val.ConvertTime(userTimePerDay - todaySpent);
                string percent = (todaySpent / userTimePerDay * 100).ToString("0.0");       
                Console.WriteLine($"Today time left: {timeNeed} ({percent}%)");
            }
        }
        catch(Exception ex) 
        { 
            Console.WriteLine(ex.Message); 
        }
    }

    public void EstimateGoal(string table, object compare, object timePerDay)
    {
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        float userGoal = Convert.ToInt32(compare);
        float time = Convert.ToInt32(timePerDay);

        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        var queryRecord = conn.CreateCommand();
        var getDateStarted = conn.CreateCommand();
        getDateStarted.CommandText = $"SELECT DATE FROM Goals WHERE Name = '{table}'";        
        try
        {
            var dateStarted = (getDateStarted.ExecuteScalar()).ToString();
            queryRecord.CommandText = $"SELECT SUM(DURATION) FROM {table} WHERE Date BETWEEN '{dateStarted}' AND '{today}'";
            float passRecord = Convert.ToInt32(queryRecord.ExecuteScalar());
            if ((userGoal - passRecord) < 0)
            {
                Console.WriteLine("Congratulation! You have reached your goal! ");
            }
            else
            {
                var timeNeed = val.ConvertTime(userGoal - passRecord);
                string percent = (passRecord / userGoal * 100).ToString("0.00");
                string dayleft = ((userGoal - passRecord)/time).ToString("0.00");                  
                Console.WriteLine($"Goal time left: {timeNeed} ({percent}%) - Date left: {dayleft} days");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
