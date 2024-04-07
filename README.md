# Console Coding Tracker
A coding tracker console application is designed to help users track and manage their coding time through a text-based interface in the console window

Developed using C# and SQLite.

# Requirements
- [X] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "Spectre.Console" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.


# Challenges
- [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and average coding session per period.
- [x] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

## Features
* A Console based UI where the user can track their coding sessions

![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/0ddb6bf1-c0de-4470-a97c-36acd6a52c06)

* **SQLite Database**
     - Using SQLite to store and read information
     - If no database or table exists it will automatically create the following tables _Coding_Session, Coding_Goal & All_Coding_Goals_
 
* **CRUD Functions**
    - Directly from the Main Menu the user can perform all CRUD functions (Create, Read, Update and Delete)
    - View all available tables in the database
    - Date validation to ensure that dates are correctly formatted

* **Code Tracking with Stopwatch**
    - User can enable stopwatch tracking under enter new session menu 
    - After user starts the stopwatch it will begin tracking and user can only stop it with a key press (F) 
    
![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/7c5969b5-8742-4d17-ae98-e6e0c6a44b30)

---
![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/060d2263-127e-49a1-b9bd-3e8e690dd2d7)

* **Reporting**
    - **View Coding Goals:**
        - View all previous goals that have been recorded
          
      ![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/50210fb4-50c2-4195-8ef4-0082646ec78f)
    - **View By Date Range:**
        - Display all users coding sessions based on entered date range.
    - **View By Biweekly Range:**
        - Display all user coding sessions in the past two weeks.
    - **View by 12 Month Range:**
        - Display users coding session for the past 12 months.
          
          ![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/693ca9d8-ca5b-49d7-886e-cde2abad65c5)
    - **Report Sorting:**
        - User can sort data by duration of hours or by date
          
          ![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/441ab7d7-147e-4afa-9487-bc59581fd2bf)
    - **Total and Average Hours Spent**
        - Each report will display the current range total and average hours spent coding. 
 ![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/cf70479a-4092-4466-b5f1-674a854fb410)
* **Other Reporting**
    - **Current Goal:**
        - After setting a goal day the user will be shown their currnt goal on startup
        - days remaining and how many hours to achieve their goal
        - Hours already completed vs Hours remaining
        
![image](https://github.com/GetTeched/CodeReviews.Console.CodingTracker/assets/1556111/253063fc-cf10-4e7a-a279-9dd4f0bb5620)

## Installation
When launching your coding tracker please ensure to set the following in the Program.cs
Currently set to testingMode as true, this is just to test the coding trackers ability with multiple tables and data.

With testing enabled it will generate the following:
* 30 - 260 random coding session entries
* 30 coding goals in the _All_Coding_Goals_ table
* All entries are set to 2023 this is to not break testing adding current goal

```
using coding_tracker;
using System.Configuration;

bool testMode = true \\Set to false;

string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;
```

## Resources
- [Spectre Console documentation](https://spectreconsole.net/)
- [Using Configuration Manager](https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/store-custom-information-config-file)
- [Parsing DateTime in C#](https://stackoverflow.com/questions/3719/how-to-validate-a-datetime-in-c)
- [Dapper Tutorial](https://www.learndapper.com/)
- [MS Documentation Configuration File](https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/store-custom-information-config-file)
- StackOverflow Articles
