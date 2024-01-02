# Coding Tracker for The C# Academy

For building this C# application I switched to JetBrains Rider as my preferred IDE instead of Visual Studio. 
The application uses the Microsoft.data.sqlite library to store data permanently in an sqlite database along side the application.
The application is intended to track the coding sessions, by date, start time and stop time.

## Requirements

The application had to meet the following requirements from The C# Academy:

- [x] This application has the same requirements as the previous project [Habit Tracker](https://github.com/stevietv/CodeReviews.Console.HabitTracker/tree/master/HabitTracker.StevieTV), except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "ConsoleTableExt" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

# Features

- SQLite database connection
    - Creates an sqlite database file and table to store data in, if it doesn't exist
- Configuration File
  - the connection string is stored in a app.config file - an XML file storing fixed configuration data
- Console Base UI
    - number entry based options (1 thru 4) for different actions
    - exit program by entering 0
- Table formatted output
  - The data is presented in a formatted table in the console using ConsoleTableExt package
- DB Functions
    - Ability to add, read, update and delete records through console entry
    - Dates entered are checked before being stored
    - Numbers are integer checked before being stored
    - Times are validated to be valid times
    - End time is validated to be later that start time
    - Attempts to update or delete non-existing entries are rejected

# Resources Used

- [The C# Acadamy Project](https://thecsharpacademy.com/project/13)
- [Youtube tutorial](https://www.youtube.com/watch?v=tvrfIMiG3-s)
- [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)