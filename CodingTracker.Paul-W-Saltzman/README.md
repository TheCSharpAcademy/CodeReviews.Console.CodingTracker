# Console.CodingTracker.Paul-W-Saltzman
- [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "ConsoleTableExt" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.
 

# Additional Challanges
- [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and average coding session per period.
- [x] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

# Features
* SQLite database connection
- The program uses a SQLite db connection to store and read information. 
- If no database exists, or the correct table does not exist they will be created on program start.
  
* A console app where the user can navigate by key presses.

* CRUD DB Functions
 - All inputs are checked and sanitized to make sure data stays formatted
 - All non-queary calls to the database are tried and any errors are caught and displayed to the user.

* Test Mode
  - Test mode allows a product tester the ability to load random data for testing.

* Log a Session
  - Manually Log a session.

* Active Coding Session.
- A stopwatch times your coding session
  
* View Coding Sessions.
- View all session.
- View by day.
- View by Week.
- Sort ascending, decsending, or so sort (database)
- Run reports, total sessions, total time, avg time.
- Modify Sessions
- Delete Session

* View Weekly Totals and see if the user met their goal.
  
* View Daily Totals and see if the user met their daily goal.

* Modify goals
  
* Settings
 - toggle test mode

* Shooting Star
  - As the user meets goals they are presented with a shooting star for their hard work.

# Challanges
- I had to use async for the first time to make the Stopwatch work the way I wanted it to.
- The weekly totals took some thought and googling. I finally used the year week combination.
- This is the fist time I've used a bubble sort.  I had an opportunity to use one so I did.
- I recently switched to Windows 11 and found that Console.Clear() did not work the way it did in Windows 10.  After some searching I found an escape key that worked.
- I really find Ascii art fun so I was excited to get a chance to use some.
- This was my first time using Data Tables so that took a bit of time to figure out.

  
# Resources 

https://stackoverflow.com/questions/371987/how-to-validate-a-datetime-in-c
https://github.com/minhhungit/ConsoleTableExt
https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file
https://learn.microsoft.com/en-us/dotnet/standard/datetime/how-to-use-dateonly-timeonly
https://www.codemaggot.com/get-week-number-of-the-year-csharp/
https://stackoverflow.com/questions/11154673/get-the-correct-week-number-of-a-given-date
