public static class Utilities
{
    public static DateTime GenerateRandomDate()
    {
        var year = Random.Shared.Next(DateTime.Now.Year, DateTime.Now.Year + 3);
        var month = Random.Shared.Next(1, 13);
        var day = Random.Shared.Next(1, DateTime.DaysInMonth(year, month) + 1); 
        var hour = Random.Shared.Next(0, 24); 
        var minute = Random.Shared.Next(0, 60); 
        var second = Random.Shared.Next(0, 60); 
        return new DateTime(year, month, day, hour, minute, second);
    }
}