# Coding Tracker

My first C# console application using Object Oriented Programming, Dapper and Spectre.Console.

Console based CRUD application to track occurrences of different coding sessions. Developed using C#, SQLite and NUnit.

## Given Requirements
  - This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
  - To show the data on the console, you should use the Spectre.Console library.
  - You're required to have separate classes in different files (i.e. UserInput.cs, Validation.cs, CodingController.cs)
  - You should tell the user the specific format you want the date and time to be logged and not allow any other format.
  - You'll need to create a configuration file called appsettings.json, which will contain your database path and connection strings (and any other configs you might need).
  - You'll need to create a CodingSession class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration. When reading from the database, you can't use an anonymous object, you have to read your table into a List of CodingSession.
  - The user shouldn't input the duration of the session. It should be calculated based on the Start and End times
  - The user should be able to input the start and end times manually.
  - You need to use Dapper ORM for the data access instead of ADO.NET.
  - Follow the DRY Principle, and avoid code repetition.
  - Your project needs to contain a ReadMe file where you'll explain how your app works and tell a little about your thought progress.

## Optional Challenging Requirements
  - Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
  - Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
  - If you already have a bit of experience with programming, we highly recommend you get into the habit of writing unit tests for a few methods in your project. Any method that outputs data and doesn't talk to a database can be unit tested.

## Features
  - SQLite database connection
    -- The program uses a SQLite DB connection to store and read information.
    -- If no database exists, or the correct table does not exist, it will be created on program start.
  - A console based UI where users can navigate by entering options.
  ![Main menu](./ReadMeAssets/MainMenu.png)
  - CRUD DB functions
    -- From the main menu users can Create, Read, Update or Delete entries for whichever coding session they want. They need to enter the starting date and hour and the ending date and hour (format : dd-MMM-yyyy hh:MM).
    -- User input are automatically checked to make sure they are in the correct and realistic format.
  - Registered Habit output.  
  ![Coding Sessions Display](./ReadMeAssets/CodingSessionsDisplay.png)

## How to run it

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) 10.0 or later

### Steps

```bash
git clone https://github.com/Sephydev/STUDY.CodingTracker.git
cd HabbitLogger/STUDY.CodingTracker
dotnet run
```

## Challenges
  - It was my first time using Dapper. I had to search for a good documentation to see the differences with ADO.NET.
  - Managing `DateTime` and `TimeSpan` type of data was also challenging because of the differences with typical type like `int` or `string`. I mainly used Microsoft Learn documentation to solve the different problems I encountered.

## Lesson Learned
  - Coming from a JS background, I now see the differences with OOP programming. I can now see the benefits of OOP, notably how well the code is now organized and easy to read.
  - I understand now the benefits of using Dapper. It allow me to "skip" some code that I used using ADO.NET, making my code more readable and making my work done quicker.
  - Spectre.Console is a really good console package, making the UI more intuitive and beautiful, while being less tedious than using Console methods. It also give me basic validation.

## Areas to improve
  - I need to get better with git commit. I have a tendency of forgetting to commit when something is done. In this case, I commit when two or three things is done. It is not optimal.
  - I have the feeling that my code could be more organized too. I will train that by practicing on new projects.

## Resources Used
  - C# Academy for the specs and related articles : https://www.thecsharpacademy.com/project/13/coding-tracker
  - SQLitetutorial to learn the basic SQL command : https://www.sqlitetutorial.net/
  - Microsoft Learn on DateTime Struct official documentation to learn basic usage of DateTime: https://learn.microsoft.com/fr-fr/dotnet/api/system.datetime?view=net-8.0
  - Microsoft Learn on TimeSpan Struct official documentation to learn basic usage of TimeSpan: https://learn.microsoft.com/fr-fr/dotnet/api/system.timespan?view=net-8.0
  - This article helped me a lot seeing the differences between Dapper and ADO.NET, and to understand basic use of Dapper : https://medium.com/@pavanpitthdiya/the-ultimate-guide-to-dapper-in-net-everything-you-need-to-know-2025-edition-295ab8a4ced8
  - Spectre.Console documentation : https://spectreconsole.net/console