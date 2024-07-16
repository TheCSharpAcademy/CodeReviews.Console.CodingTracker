namespace CodingTracker.Data.Extensions;

/// <summary>
/// DateTime class extension methods.
/// </summary>
public static class DateTimeExtensions
{
    #region Constants

    private static readonly string SqliteDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

    #endregion
    #region Methods

    public static string ToSqliteDateTimeString(this DateTime dateTime)
    {
        return dateTime.ToString(SqliteDateTimeFormat);
    }

    #endregion
}
