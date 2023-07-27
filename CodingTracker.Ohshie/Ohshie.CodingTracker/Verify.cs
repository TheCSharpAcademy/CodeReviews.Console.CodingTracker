using System.Globalization;

public static class Verify
{
    public static bool GoBack(string? userInput)
    {
        if (string.IsNullOrEmpty(userInput) || userInput == "no")
        {
            return true;
        }

        return false;
    }
    
    public static bool DateFormat(string? userInput)
    {
        string format = "dd.MM.yyyy HH:mm";
        if (DateTime.TryParseExact(userInput, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var correctDate)) 
            if (correctDate <= DateTime.Now) return true;
        
        return false;
    }

    public static bool LengthFormat(string? userInput)
    {
        string format = @"hh\:mm\:ss";
        if (TimeSpan.TryParseExact(userInput, format, CultureInfo.InvariantCulture, TimeSpanStyles.None,
                out var correctLength)) return true;
        
        return false;
    }
}