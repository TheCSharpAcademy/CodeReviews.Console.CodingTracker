# CodingTracker

Console based CRUD application to keep track of coding hours. Developed using C# and SQLite.

## Given Requirements
- [x] Same requirements as [HabitTrackerApp](https://github.com/kimfom01/HabitTrackerApp), except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the ["ConsoleTableExt"](https://github.com/minhhungit/ConsoleTableExt) library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration.
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.
## Challenges (not completed yet)
- [ ] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years)
- [ ] Let the users sort their coding records in order ascending or descending
- [ ] Create reports where the users can see their total and average coding session per period.
- [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

## Features
* SQLite database connection
    - The program uses a SQLite db connection to store and read information.
    - If no database exists, or the correct table does not exist they will be created.
* CRUD DB functions
    - From the main menu users can Create, Read, Update or Delete entries.
    
## Things I learnt
* Parsing DateTime
* Seperation of concerns
* Using an external library
* Getting connection string from an external source (config file)

## Resources
* [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
* [Using a Configuration Manager](https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/store-custom-information-config-file)
* [Parsing DateTime in C#](https://stackoverflow.com/questions/371987/how-to-validate-a-datetime-in-c)
