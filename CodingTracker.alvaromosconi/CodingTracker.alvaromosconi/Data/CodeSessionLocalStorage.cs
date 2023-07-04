using CodingTracker.alvaromosconi.Model;

namespace CodingTracker.alvaromosconi.Data;

internal class CodeSessionLocalStorage
{
    private SQLiteDB Database = new SQLiteDB();
    private HashSet<CodeSessionModel> codeSessions = new HashSet<CodeSessionModel>();
    public CodeSessionLocalStorage()
    {
        Database.OnCreate();
    }

    public void SaveSession(CodeSessionModel model)
    {
        var query = $@"
                        INSERT INTO {DBConstants.TABLE_NAME} ({DBConstants.START_DATE}, {DBConstants.END_DATE})
                        VALUES ('{model.StartDateTime}', '{model.EndDateTime}')
                    ";

        Database.ExecuteQuery(query);
        codeSessions.Add(model);
    }

    public void DeleteSession(CodeSessionModel model)
    {
        var query = $@"
                        DELETE FROM {DBConstants.TABLE_NAME} 
                        WHERE ({DBConstants.ID_COLUMN} = {model.Id})
                    ";

        Database.ExecuteQuery(query);
        codeSessions.Remove(model);
    }

    public HashSet<CodeSessionModel> GetAllSesions()
    {
        return codeSessions;
    }

    public List<CodeSessionModel> GetAllSessionsBetween(DateTime start, DateTime end)
    {
        var query = $@"
                        SELECT * FROM {DBConstants.TABLE_NAME}
                        WHERE {DBConstants.START_DATE}
                        BETWEEN {start} and {end}
                        ORDER BY {DBConstants.END_DATE}
                       ";

        return Database.GetData(query);
    }
}
