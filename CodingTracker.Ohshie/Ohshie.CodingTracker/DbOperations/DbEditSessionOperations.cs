using Microsoft.Data.Sqlite;

namespace Ohshie.CodingTracker.DbOperations;

public class DbEditSessionOperations : DbOperations
{
    public void EditSessionName(string? newName, Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "UPDATE 'CodingSessions'" +
                                       $"SET description = '{newName}'" +
                                       $"WHERE Id = {session.Id}";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void EditSessionNote(string newNote, Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "UPDATE 'CodingSessions'" +
                                       $"SET note = '{newNote}'" +
                                       $"WHERE Id = {session.Id}";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    public void EditSessionDate(string newDate, Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "UPDATE 'CodingSessions'" +
                                       $"SET date = '{newDate}'" +
                                       $"WHERE Id = {session.Id}";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }
    
    public void EditSessionLength(string newLength, Session session)
    {
        using (var connection = new SqliteConnection(DbConnection))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "UPDATE 'CodingSessions'" +
                                       $"SET sessionLength = '{newLength}'" +
                                       $"WHERE Id = {session.Id}";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }
}