# Console CodingTracker
  
  Basic CRUD Application using C#, SQLite3, Dapper ORM, and Spectre.Console. Made using Visual Studio and VScode
  
# Given Requirments:
  - [x] This application has the same requirements as the HabitTracker project, except that now you'll be logging your daily coding time.
  - [ ] To show the data on the console, you should use the "Spectre.Console" library.
  - [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
  - [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
  - [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
  - [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
  - [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
  - [x] The user should be able to input the start and end times manually.
  - [x] You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
  - [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

### Challenge Requirments:
  - [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
  - [ ] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
  - [ ] Create reports where the users can see their total and average coding session per period.
  - [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#

# Features
  * Console UI that allows the user to interact with the program
  * DatabaseUtilities Namespace that handles all CRUD Operations quickly and efficiently by using Dapper ORM
  * Ouputs data to console in easy to read way using [Spectre.Console](#)

# Challenges


# Lessions Learned


# Areas To Improve
  * Not many areas that I feel need improvement with this one. Most of the issues with the HabitTracker have been fixed in this one

# Resources Used
  * Reused and optimized HabitTracker code
  * Dapper ORM Documentation
  * Multiple Stack Overflow articles for different issues I ran into with C#, Dapper, and Spectre.Console
  * SQLite and C# documentation
