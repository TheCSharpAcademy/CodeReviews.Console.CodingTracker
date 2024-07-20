
using CodingTracker.kwm0304.Views;

namespace CodingTracker.kwm0304;

/* 
REQUIREMENTS
- Log daily coding time
- Use spectre console
- Have multiple classes
- Have config file for db path and connection string
- Create a coding session class in separate file, has: Id, startTime, EndTime, Duration
- User shouldn't input the duration of the session. Should be calculated off the start and end times in a separate calculateduration method
- User should be able to input start and end times manually
- Use dapper for data access
- Don't use anonymous db when reading from db
- Add the possibility of tracking the coding time via a stopwatch so user can track session as it happens
- Let the user filter their coding records per period(day, week, year) + ascending/descending
- create reports where users can see their average coding session per period
- have the ability to set coding goals and show how far the users are from reaching their goal,
  along with how many hours a day they would have to code to reach their goal

*/
class Program
{
  static void Main(string[] args)
  {
    
    while (true)
    {
      SessionLoop loop = new();
      loop.OnStart();
    }
  }
}