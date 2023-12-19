using CodingTracker.alvaromosconi.Model;

namespace CodingTracker.alvaromosconi.Data;

internal class CodeSessionLocalStorage
{
    private SQLiteDB Database = new SQLiteDB();
    private List<CodeSessionModel> codeSessions = new List<CodeSessionModel>();
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

    public void DeleteSession(int id)
    {
        var query = $@"
                        DELETE FROM {DBConstants.TABLE_NAME} 
                        WHERE ({DBConstants.ID_COLUMN} = {id})
                    ";

        Database.ExecuteQuery(query);
        codeSessions.RemoveAll(session => session.Id == id);
    }

    public List<CodeSessionModel> GetAllSesions()
    {
        return codeSessions;
    }

    public List<CodeSessionModel> GetAllSessionsBetween(DateTime start, DateTime end)
    {
        var sessionsInRange = 
            codeSessions
                  .Where(session => session.StartDateTime >= start && session.EndDateTime <= end)
                  .OrderBy(session => session.EndDateTime)
                  .ToList();

        if (sessionsInRange.Count == 0)
        {
            string query = $@"
                                SELECT * FROM {DBConstants.TABLE_NAME}
                                WHERE {DBConstants.START_DATE} >= '{start}' AND 
                                      {DBConstants.END_DATE} <= '{end}'
                                ORDER BY {DBConstants.END_DATE}
                            ";

            sessionsInRange = Database.GetData(query);
        }

        return sessionsInRange;
    }

    public bool GetSessionWithId(int id)
    {
        CodeSessionModel session = codeSessions.Find(x => x.Id == id);
        bool exists = false;

        if (session == null)
        {
            string query = $@"
                            SELECT * {DBConstants.TABLE_NAME} 
                            WHERE ({DBConstants.ID_COLUMN} = {id})
                           ";

            List<CodeSessionModel> sessionsFromDb = Database.GetData(query);
            exists = sessionsFromDb.Count > 0;
            
            if (exists)
                codeSessions.Union(sessionsFromDb);

        }

        return exists;
    }
}
