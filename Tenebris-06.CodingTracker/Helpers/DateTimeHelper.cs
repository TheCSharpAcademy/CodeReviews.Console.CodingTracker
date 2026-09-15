using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Spectre.Console;

public static class DateTimeHelper{
    public static bool TryGetDateTime(string input, out DateTime result)
{
    if (string.IsNullOrWhiteSpace(input))
    {
        result = DateTime.Now;
        return true;
    }

    return DateTime.TryParseExact(
        input,
        "yyyy-MM-dd HH:mm:ss",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out result
    );
}

}