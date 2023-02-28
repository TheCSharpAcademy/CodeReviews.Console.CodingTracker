using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Text;
using ConsoleTableExt;

namespace ThePortugueseMan.CodingTracker;

public class DbCommands
{
    string? connectionString, mainTableName;
    AppSettings appSettings = new();
    public DbCommands() 
    {
        this.connectionString = appSettings.GetConnectionString();
        this.mainTableName = appSettings.GetMainTableName();
    }

    //if the main table doesn't exist, it's created
    public void Initialization()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {this.mainTableName}" +
                    "(Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "StartDate STRING, EndDate STRING, Diff STRING)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    public bool Insert(ref CodingSession sessionToInsert)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO {this.mainTableName}(StartDate, EndDate, Diff) " +
                $"VALUES ('{sessionToInsert.StartDateTime.ToString("dd-MM-yy_HH:mm")}'," +
                $"'{sessionToInsert.EndDateTime.ToString("dd-MM-yy_HH:mm")}'," +
                $"'{sessionToInsert.Duration.ToString(@"hh\:mm")}')";

            try 
            {
                tableCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                connection.Close();
                return false;
            }
            
        }
    }
    //checks if there is an entry at Id = index
    public bool CheckIfIndexExists(int index)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {this.mainTableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }
    public bool DeleteByIndex(int index, string? tableName)
    {
        string? subTableName = GetTableNameOrUnitsFromIndex(tableName, index, "TableName");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from {tableName} WHERE Id = '{index}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            connection.Close();
            if (rowCount == 0) return false;
            //Deleting a row from the main Table means deleting an habit, including the habit's table
            else if (tableName == this.mainTableName && !DeleteTable(subTableName)) return false;
            else return true;

        }
    }
    public bool DeleteTable(string? tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DROP TABLE {tableName}";

            if (tableCmd.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }
            else
            {
                connection.Close();
                return true;
            }
        }
    }
    //Updates entry on subTable by index - overload based on datatypes
    public bool Update(string tableName, int index, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE {tableName} SET date = '{date}', quantity = {quantity} WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                return true;
            }
        }
    }
    //Update entry on main table by index - overload based on datatypes
    public bool Update(string? mainTableName, int index, string? newTableName, string? newUnit)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {mainTableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                if (!ChangeSubTableName(
                        GetTableNameOrUnitsFromIndex(mainTableName, index, "TableName"),
                        newTableName))
                { return false; }

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE {mainTableName}" +
                    $" SET HabitTableName = '{newTableName}', HabitUnit = '{newUnit}'" +
                    $" WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                return true;
            }
        }
    }
    public void ViewAll()
    {
        ViewMainTable(this.mainTableName);

    }
    private void ViewMainTable(string mainTableName)
    {
        string? habitTableName_display = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {mainTableName}";

            var tableDisplayData = new List<List<string>>();
            var tableData = new List<CodingSession>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartDateTime = DateTime.ParseExact(reader.GetString(1),"dd-MM-yy_HH:mm", new CultureInfo("en-US")),
                        EndDateTime = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy_HH:mm", new CultureInfo("en-US")),
                        Duration = TimeSpan.ParseExact(reader.GetString(3), "h\\:mm", new CultureInfo("en-US"))
                    });

                    tableDisplayData.Add(
                        new List<string>
                        { reader.GetString(1), reader.GetString(2), reader.GetString(3) });
                }
            }
            else { Console.WriteLine("Empty"); }

            connection.Close();

            var displayTest= new List<List<object>>
            {
                new List<object>{ "Sakura Yamamoto", "Support Engineer", "London", 46},
                new List<object>{ "Serge Baldwin", "Data Coordinator", "San Francisco", 28, "something else" },
                new List<object>{ "Shad Decker", "Regional Director", "Edinburgh"},
            };

            ConsoleTableBuilder.From(tableDisplayData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Coding Tracker")
                .ExportAndWriteLine();
        }
    }
    private void ViewSubTable(string subTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {subTableName}";

            List<SubTable> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new SubTable
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    });
                }
            }
            else { Console.WriteLine("Empty"); }

            connection.Close();
            string? units = GetUnitFromTableName("HabitsTable", subTableName);

            Console.WriteLine("-----------------------------\n");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yyyy")} - Quantity: {dw.Quantity} " + units);
            }
            Console.WriteLine("\n-----------------------------");
        }
    }
    //returnType == "TableName" returns the name of the table at index of the mainTable
    //returnType == "HabitUnit" return the name of the habit's unit ^""
    public string? GetTableNameOrUnitsFromIndex(string? mainTableName, int index, string? returnType)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string? returnString = null;
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {mainTableName} WHERE Id = {index}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (!reader.HasRows)
            {
                connection.Close();
                return null;
            }
            else
            {
                reader.Read();
                if (returnType == "TableName") returnString = reader.GetString(1);
                else if (returnType == "HabitUnit") returnString = reader.GetString(2);

                reader.Close();
                return returnString;
            }
        }
    }
    public string? GetUnitFromTableName(string? mainTableName, string? subTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            string? returnString = null;
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT HabitUnit FROM {mainTableName} WHERE HabitTableName = '{subTableName}'";

            SqliteDataReader reader = tableCmd.ExecuteReader();


            if (!reader.HasRows)
            {
                connection.Close();
                return null;
            }
            else
            {
                reader.Read();
                returnString = reader.GetString(0);
                reader.Close();
                return returnString;
            }
        }
    }
    public bool ChangeSubTableName(string? currentTableName, string? newTableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            try
            {
                tableCmd.CommandText =
                $"ALTER TABLE {currentTableName} RENAME TO {newTableName}";

                tableCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch { return false; }
        }
    }
    public class SubTable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
    }
    //represents an entry in the main table - a habit
    //a habit is composed of it's tableName where entries are stores
    //and it's unit, the name of what is meant to be trackes
    public class Habit
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public string? Unit { get; set; }
    }
}
