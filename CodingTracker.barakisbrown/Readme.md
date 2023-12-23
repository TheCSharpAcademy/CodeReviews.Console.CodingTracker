# Console Coding Tracker  
This is the Console Coding Tracker app, created for [The C# Academy](https://www.thecsharpacademy.com/#), based on the requirements listed in the project.
The purpose of this app, is to manage complexity of implementing and handling Date and Time. Also, the app is using external library to handle data display.

## Requirements  
- [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "ConsoleTableExt" library.
- [x] You 're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You 'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You 'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

## CHALLENGE: 
- [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and average coding session per period.

## OVERVIEW
- This app which is an specialized version of Habit tracker where we tracked one habit which is coding session.  All the data is stored into an sqlite db.
- The app if it can not find the DB, it will create itself with table needed for the application to work.
- Like Havit tracker, this one has your basic crud commands (Create/Read/Update/Delete).
- I use [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt) for all tables including the one needed to ask the user to verify in update/delete

## PERSONAL CHALLENGES
- When using raw sql and using DateTime can be a serious challenge in learning and patience.
- Thankfully C# 8.0+ has added DateOnly and TimeOnly Constructs which make it a lot easier.

## Resources used

- Pablo and the entire [C# Academy](https://www.thecsharpacademy.com/#) community
- [MS Learn](https://learn.microsoft.com/en-us/) and the ADO.NET documentation
- [DB Browser for SQLite](https://sqlitebrowser.org/)
- [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt) documentation.
