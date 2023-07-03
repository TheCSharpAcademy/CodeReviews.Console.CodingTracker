using CodingTracker.alvaromosconi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.Data;

internal class CodeSessionLocalStorage 
{
    private SQLiteDB Database = new SQLiteDB();
    private List<CodeSessionModel> codeSessions = new List<CodeSessionModel>();
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
    }

    public void DeleteSession(CodeSessionModel model)
    {
        var query = $@"
                        DELETE FROM {DBConstants.TABLE_NAME} 
                        WHERE ({DBConstants.ID_COLUMN} = {model.Id})
                    ";

        Database.ExecuteQuery(query);
    }

}
