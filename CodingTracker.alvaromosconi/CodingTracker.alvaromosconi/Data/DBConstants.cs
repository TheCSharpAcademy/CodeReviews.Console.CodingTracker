using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi.Data;

internal static class DBConstants
{
    public static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
    public const string TABLE_NAME = "sessions";
    public const string ID_COLUMN = "id";
    public const string START_DATE = "start_date";
    public const string END_DATE = "end_date";

    public const string CREATE_SESSIONS_TABLE =
        $@"CREATE TABLE {TABLE_NAME} 
           ({ID_COLUMN} integer PRIMARY KEY AUTOINCREMENT,
            {START_DATE} TEXT,
            {END_DATE} TEXT)";
}
