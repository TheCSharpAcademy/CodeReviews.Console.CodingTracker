# SinghxRaj.CodingTracker

## Requirements
- [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "ConsoleTableExt" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

## Challenges
- [ ] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [ ] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [ ] Create reports where the users can see their total and average coding session per period.
- [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

## Features
- A sqlite database is created with the necessary table, if it does not exist. The database stores data about the coding sessions that were logged by the user.
- User are able to:
  - Log a new coding session. This session can be from a while back, your next session, or a session far in the future.
  - View all the coding sessions that they have logged.
  
 ## Application Implementation Details
 - The application performs CRUB operations on the database through SQL Queries. 
 - Validation is performed on user data before interacting with the database.
 
 ## Walkthrough
 A simple walkthrough of the application.
 
 It starts by displaying the introduction and menu of operations that the user can choose from:
 ![image](https://user-images.githubusercontent.com/69612398/213860283-f28527b8-e7b8-44e7-887f-c5f1eb499713.png)

 If you choose option 2, the application will display all coding session that have been logged.
 ![image](https://user-images.githubusercontent.com/69612398/213860295-bb36193a-99c1-43e3-a33d-efc12dd215cb.png)

 The application ends when 0 is chosen as the option.
 
 ![image](https://user-images.githubusercontent.com/69612398/213860308-ef96fe42-89c5-4ecf-8216-8ada4cb6c305.png)


 Challenges
 - One hurdle I faced was working the DateTime struct. At times, I struggled with working with it but through perseverance and time I was able to learn how to work with them in this project. I had previously used the DateTime struct but unlike in the past, this project gave me a chance to work more in depth with the DateTime struct and learn more about. For example, I learned about parsing and formatting strings to convert them into DateTime structs. Furthermore, I got a chance to work with DateOnly and TimeOnly structs as well which I had never worked with in the past.
 - Another hurdle I faced was with working with the raw SQL queries. While the queries were difficult, I had problems with minor syntax errors in the SQL queries which took time to figure out. I was able to work through this hurdle by debugging the code. Furthermore, this hurdle gave me a chance to build my attention to detail since I had to make sure a minor syntax error didn't exist (This is a bit difficult when there is no syntax highlight). Using a tool such as EF Core will probably be must in  best bet in the future which I have been learning and will work with in a future project.
 
 
 Things I learned:
 - As talked about earlier, I got a chance to work with parsing/formatting the DateTime, DateOnly, and TimeOnly structs.
 - Improved my separation of concerns skills by separting different parts into their own separate classes based on what I believed would work best.
 - Working with SQLite databases in C# and working with SQL queries.
 - Learning to work with a configuration file to store data that's separate from the application but still used by the application such as the connection string.
 - Working with third-party nuget packages such as ConsoleTableExt.
 
Resources:
- [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
- [Using a Configuration Manager](https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/store-custom-information-config-file)
- [Parsing/Formatting DateTime in C#](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-7.0)
- For information about the SQLiteConnection class - [Microsoft.Data.SQLite documentation](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- Various StackOverflow posts
