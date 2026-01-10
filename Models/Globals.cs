using System.Globalization;

public static class Globals
{
    public static readonly string DATE_FORMAT = "dd/MM/yyyy";
    public static readonly string TIME_FORMAT = "HH:mm:ss";
    public static readonly IFormatProvider CULTURE_INFO = new CultureInfo("en-US");
}

