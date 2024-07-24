# Console CodingTracker
  
  Basic CRUD Application using C#, SQLite3, Dapper ORM, and Spectre.Console. Made using Visual Studio and VScode
  
# Given Requirments:
  - [x] This application has the same requirements as the HabitTracker project, except that now you'll be logging your daily coding time.
  - [x] To show the data on the console, you should use the "Spectre.Console" library.
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
  - [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
  - [x] Create reports where the users can see their total and average coding session per period.
  - [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#

# Features
  * Console UI that allows the user to interact with the program a displays SQLite tables
      * ![image](https://github.com/user-attachments/assets/5a996c5f-6cee-4c36-b7f7-9885b2184f32)
      * ![image](https://github.com/user-attachments/assets/8c9bcbde-d8ed-41bc-ba97-cae3339ada79)

  * DatabaseUtilities Namespace that handles all CRUD Operations using [Dapper ORM](https://www.learndapper.com/)
  * Ouputs data to console in a table using [Spectre.Console](https://spectreconsole.net/)

# Challenges
  * Had some issues with the SQL database and config file at first but was able to get everything figured out pretty quickly

# Lessions Learned
  * I did not realize how much I could do with Spectre.Console and if I had been when I started coding I would have been able to make a much nicer application without wasting so much time rewriting code
  * Use things like Dapper and Spectre.Console as they make things a lot easier

# Resources Used
  * Reused and optimized HabitTracker code
  * Dapper ORM documentation
  * Spectre.Console documentation
  * Multiple Stack Overflow articles for different issues I ran into with C#, Dapper, and Spectre.Console
  * SQLite and C# documentation
