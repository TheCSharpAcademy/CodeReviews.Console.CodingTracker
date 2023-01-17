using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    public static class SqliteTypeConverter
    {
        private static Dictionary<Type, SqliteType> typeMap;

        // Create and populate the dictionary in the static constructor
        static SqliteTypeConverter()
        {
            typeMap = new Dictionary<Type, SqliteType>();


            typeMap[typeof(string)] = SqliteType.Text;
            typeMap[typeof(char[])] = SqliteType.Text;
            typeMap[typeof(byte)] = SqliteType.Integer;
            typeMap[typeof(short)] = SqliteType.Integer;
            typeMap[typeof(int)] = SqliteType.Integer;
            typeMap[typeof(long)] = SqliteType.Integer;
            typeMap[typeof(bool)] = SqliteType.Integer;
            typeMap[typeof(DateTime)] = SqliteType.Text;
            typeMap[typeof(DateOnly)] = SqliteType.Text;
            typeMap[typeof(double)] = SqliteType.Integer;
            typeMap[typeof(TimeSpan)] = SqliteType.Text;
        }

        // Non-generic argument-based method
        public static SqliteType GetDbType(Type giveType)
        {
            // Allow nullable types to be handled
            giveType = Nullable.GetUnderlyingType(giveType) ?? giveType;

            if (typeMap.ContainsKey(giveType))
            {
                return typeMap[giveType];
            }

            throw new ArgumentException($"{giveType.FullName} is not a supported .NET class");
        }

        public static SqliteType GetDbType<T>()
        {
            return GetDbType(typeof(T));
        }
    }
}
