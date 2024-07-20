using CodingTracker.kwm0304.Enums;

namespace CodingTracker.kwm0304;

public static class Global
{
    public static List<DateRange> RangeList =
    [
      DateRange.Week,
      DateRange.Month,
      DateRange.Year
    ];

    public static readonly string header = @"
   ______          ___                ______                __            
  / ____/___  ____/ (_)___  ____ _   /_  __/________ ______/ /_____  _____
 / /   / __ \/ __  / / __ \/ __ `/    / / / ___/ __ `/ ___/ //_/ _ \/ ___/
/ /___/ /_/ / /_/ / / / / / /_/ /    / / / /  / /_/ / /__/ ,< /  __/ /    
\____/\____/\__,_/_/_/ /_/\__, /    /_/ /_/   \__,_/\___/_/|_|\___/_/     
                         /____/                                           
";
}