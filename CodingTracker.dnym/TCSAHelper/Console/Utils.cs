namespace TCSAHelper.Console;
internal static class Utils
{
    internal static void ClearRestOfLine(char c = ' ')
    {
        var currentLine = System.Console.CursorTop;
        var currentColumn = System.Console.CursorLeft;
        System.Console.Write(new string(c, System.Console.WindowWidth - currentColumn));
        System.Console.SetCursorPosition(currentColumn, currentLine);
    }
}
