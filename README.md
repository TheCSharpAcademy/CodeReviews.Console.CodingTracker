# .NET SQLite Coding Tracker Console App

[This repository](https://github.com/frockett/CodeReviews.Console.CodingTracker/tree/main/frockett.CodingTracker) contains my version of [this](https://thecsharpacademy.com/project/13) project from the C# Academy website. It uses the ADO.NET framework to perform simple CRUD operations on an SQLite database that tracks programming sessions.

## Features
- Insert, update, and delete records based on date/time or record ID.
- Track work sessions with a stopwatch and automatically log session.
- Generate in-app reports to display all sessions, totals/averages per month, all sessions from one month, and more.
- Seed random data within a realistic range for testing.


## Technologies/Packages Used
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Microsoft.Data.SQLite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/)
	- Communication with SQLite database
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)
	- User input, table generation, and QOL features


## Operation
- Use the arrow and enter keys to navigate menus and submenus
- Follow on-screen prompts for entering dates and times
- Enter the start and end times of your session, the app will calculate duration, add it to the database, and present to you information about total hours spent in a month, average time per session, and average time per day upon report generation. 


## Reflection
- The architecture of this project is much better than my [previous one](https://github.com/frockett/CodeReviews.Console.HabitTracker)
- Planning and making a diagram of the program before even touching the IDE resulted in more deliberate and effective work.
- Some methods in the SQLiteDbMethods class, particularly regarding generating custom reports, are unfocused and could use substantial improvements. There's too much nesting. If I revisit this project, I should refactor some methods in that class, maybe into multiple smaller methods.
