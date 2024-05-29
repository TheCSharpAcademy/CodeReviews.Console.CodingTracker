# Coding Tracker

## Requirements
- [] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [] To show the data on the console, you should use the "Spectre.Console" library.
- [] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id StartTime, EndTime, Duration
- [] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [] The user should be able to input the start and end times manually.
- [] You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
- [] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions. 
