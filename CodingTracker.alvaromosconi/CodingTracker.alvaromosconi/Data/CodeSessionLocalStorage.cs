using CodingTracker.alvaromosconi.Model;

namespace CodingTracker.alvaromosconi.Data;

internal class CodeSessionLocalStorage
{
    private SQLiteDB Database = new SQLiteDB();
    private HashSet<CodeSessionModel> codeSessions = new HashSet<CodeSessionModel>();
    public CodeSessionLocalStorage()
    {
        Database.OnCreate();
        LoadSessionsFromDatabase();
    }

    private void LoadSessionsFromDatabase()
    {
        var query = $@"SELECT * FROM {DBConstants.TABLE_NAME}";

        var sessionsFromDb = Database.GetData(query);
        codeSessions = sessionsFromDb;
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

    public HashSet<CodeSessionModel> GetAllSessionsBetween(DateTime start, DateTime end)
    {
        var sessionsInRange = codeSessions
                  .Where(session => session.StartDateTime >= start && session.EndDateTime <= end)
                  .OrderBy(session => session.EndDateTime)
                  .ToList();

        if (sessionsInRange.Count == 0)
        {
            var query = $@"
                        SELECT * FROM {DBConstants.TABLE_NAME}
                        WHERE {DBConstants.START_DATE}
                        BETWEEN {start} and {end}
                        ORDER BY {DBConstants.END_DATE}
                       ";

            sessionsInRange = Database.GetData(query);
            codeSessions.UnionWith(sessionsInRange);
        }

        return codeSessions;
    }
}
