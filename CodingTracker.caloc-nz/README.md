# CodingTracker

## Project Requirements

### Habit Logger Requirements (From previous project)

- [X] The application should store and retrieve data from a real database
- [X] When the application starts, it should create a sqlite database, if one
  isn't present.
- [X] It should also create a table in the database, where the habit will be  
  logged.
- [X] The app should show the user a menu of options.
- [X] The users should be able to insert, delete, update and view their logged  
  habit.
- [X] You should handle all possible errors so that the application never  
  crashes.
- [X] The application should only be terminated when the user inserts 0.
- [X] You can only interact with the database using raw SQL. You can't use  
  mappers such as Entity Framework.
- [X] Your project needs to contain a Read Me file where you'll explain how your
  app works. Here's a nice example:

### Coding Tracker Requirements

- [X] This application has the same requirements as the previous project,  
  except that now you'll be logging your daily coding time.
- [X] To show the data on the console, you should use the "Spectre.Console"  
  library.
- [X] You're required to have separate classes in different files (ex.  
  UserInput.cs, Validation.cs, CodingController.cs)
- [X] You should tell the user the specific format you want the date and time  
  to be logged and not allow any other format.
- [X] You'll need to create a configuration file that you'll contain your  
  database path and connection strings.
- [X] You'll need to create a "CodingSession" class in a separate file. It  
  will contain the properties of your coding session: Id, StartTime, EndTime,  
  Duration
- [X] The user shouldn't input the duration of the session. It should be  
  calculated based on the Start and End times, in a separate  
  "CalculateDuration" method.
- [X] The user should be able to input the start and end times manually.
- [X] You need to use Dapper ORM for the data access instead of ADO.NET.  
(This requirement was included in Feb/2024)
- [X] When reading from the database, you can't use an anonymous object, you  
  have to read your table into a List of Coding Sessions.

## Features

- Insert new coding records with date and duration.
- Delete existing coding records by their ID.
- Update existing coding records' date or duration.
- View all coding records.
- Generate reports for coding sessions within specified date ranges.
- Timer functionality to track coding sessions in real-time.

## Project Overview

I took a bit of a long winded way to write this project. I followed the  
[CodingTracker App, C# Beginner Project. CRUD, ADO.NET, Sqlite,  
VSCode.](https://youtu.be/tvrfIMiG3-s?si=hNIiJe8F2qEh3dqQ), which got me to a  
working app, but did not include Spectre.Console and Dapper.  

The tutorial helped me understand the basics and the structure of the code  
for the CodingTracker app. Once I had a running app I researched and changed  
the code to support Dapper.  

After the Dapper changes, I changed the code to support Spectre.Console.

I have been through a few iterations of the code to optimise the code and  
make it more readable with a combination of the Visual Studio 2022 messages  
and Codacy identify and resolve issues.  I didn't just make changes to the  
code without understanding exactly what the recommended changes mean or do,  
and where I still didn't understand the changes I left unchanged.

The project is in a working state and I believe meets all the project  
requirements specified.

I quite enjoyed working on this project, as a loft of this was not handed to  
me on a silver plate. I know the code is not perfect and a lot of refactoring  
can be done to optimise it even more. As go through my learning in other  
projects I will come back to this project and apply my learnings, see what  
works and what doesn't.

## Resources

- [Coding Tracker App, C# Beginner Project. CRUD, ADO.NET, Sqlite, VSCode.](https://youtu.be/tvrfIMiG3-s?si=hNIiJe8F2qEh3dqQ)
- [C# Dapper Introduction](https://www.youtube.com/watch?v=ntjjdxcYs-c)
- [Introduction Spectre.Console](https://youtu.be/rXJ2p2Am_0I?si=CHVqfUCMFYFLNlGa)
- [Create Better Looking Console Applications With Spectre.Console](https://code-maze.com/csharp-create-better-looking-console-applications-with-spectre-console/)