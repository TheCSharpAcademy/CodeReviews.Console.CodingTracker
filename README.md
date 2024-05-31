
# Coding Tracker

A console-based CRUD application to track coding sessions; start-time, end-time,
and calculates the duration based on the two, developed using C# and SQLite.

## Given Requirements

- When the application starts, it should create a SQLite database if one isn’t present.
- It should also create a table in the database where the
coding sessions will be logged.
- Users need to be able to insert, delete, update, and view logged coding sessions.
- All possible errors should be handled so that the application never crashes.
- The application should only be terminated when the user inserts 0.
- Interaction with the database should be done using raw SQL;
no mappers such as Entity Framework are allowed.
- Users should be able to log start and end times manually.
- The application should calculate the duration of a coding
session based on start and end times.
- The application should use the "Spectre.Console" library to display data.
- A configuration file should contain the database path and connection strings.
- The code should be organized with separation of concerns.

## Features

- **SQLite Database Connection**
  - The program uses a SQLite database connection to store and read information.
  - If no database or correct table exists, they will be created on program start.
  - Live coding session with stopwatch to calculate coding time.
  - Filter records in ascending order by period.
  
- **Console-based UI**
  - Users can navigate using key presses.
  
- **CRUD DB Functions**
  - Users can Create, Read, Update, or Delete entries for coding sessions.
  - Inputted start and end times are validated to ensure they
are in the correct and realistic format.
  
- **Stopwatch Functionality**
  - Users can track coding sessions in real-time using a stopwatch feature.

## Challenges

- **Learning Dapper, Spectre, and SQLite from Scratch**
  - Understanding the basics of Dapper and Spectre.
  - Setting up and managing a SQLite database.
  
- **Handling Date and Time**
  - Parsing and formatting dates and times.
  - Calculating durations based on start and end times.
  
- **Implementing Separation of Concerns**
  - Organizing code into separate classes and methods.
  - Ensuring each class and method has a single responsibility.
  
- **Error Handling**
  - Ensuring the application handles all possible errors gracefully to prevent crashes.

## Lessons Learned

- **Planning and Mapping**
  - Planning and mapping out objects and methods before coding to
avoid spaghetti code.
  
- **Code Organization**
  - Refactoring code into more organized classes and methods.
  - Separating user input handling from other methods for better code reusability.
  
- **Proper SQL Usage**
  - Writing proper SQL commands for CRUD operations and reports.
  
- **Dapper**
  - Using Dapper to navigate database.

## Areas to Improve

- **Code Snippets**
  - Utilizing more code snippets for common tasks.
  
- **Adhering to SOLID Principles**
  - Ensuring methods adhere to the single responsibility principle.
  - Exploring method overloading and out arguments for cleaner code.
  
- **Advanced Reporting**
  - Adding more advanced reporting features and user interfaces.
  
- **User Experience**
  - Enhancing the user experience with more intuitive navigation and feedback.
  
- **Dapper**
  - Learning more advance use of Dapper.
  
- **LINQ**
  - Learning how to use LINQ in-depth.

## Resources Used

- [Dapper](https://github.com/DapperLib/Dapper) for navigating Database.
- [Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-8.0)
for DateTime.
- [Spectre.Console Documentation](https://spectreconsole.net/) for better
console output.
