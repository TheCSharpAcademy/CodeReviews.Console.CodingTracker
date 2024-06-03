# Coding Sessions Tracker
In this Project we created A Coding Tracker App to track ones all
coding sessions. We have the CRUD operations and Report section
to display different reports to user based on the start and end date.
[Dapper](https://www.learndapper.com/) is used as mini ORM.

## Requirements

- [x] This application has the same requirements as the previous
project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the
"Spectre.Console" library.
- [x] You're required to have separate classes in different files
(e.g. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the
date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain
your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file.
It will contain the properties of your coding session: Id StartTime,
EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should
be calculated based on the Start and End times, in a separate
"CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] You need to use Dapper ORM for the data access instead of ADO.NET.
(This requirement was included in Feb/2024)
- [x] When reading from the database, you can't use an anonymous object,
you have to read your table into a List of Coding Sessions.

## Challenges

- [x] Add the possibility of tracking the coding time via a stopwatch so
the user can track the session as it happens.
- [x] Let the users filter their coding records per period
(weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and
average coding session per period.
- [] Create the ability to set coding goals and show how far the
users are from reaching their goal, along with how many hours a day
they would have to code to reach their goal. You can do it via SQL
queries or with C#.

## Features

- [Spectre Console](https://spectreconsole.net/) library is used
to used to build a Menu to navigate the application and to
visualize the reports in tables.
- Users use the arrow and use Enter keys to to select an operation  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/d5cc3f66-9dc7-4734-9fa0-10669ab6a057)
- All Coding sessions  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/8d1de1ef-ca15-47de-9245-87fb5d270d66)
- Adding new Coding Session. User enter the Start and End
datetime in yyyy-MM-dd HH:mm:ss format. The duration of the
session is stored in seconds. All the inputs are validated
using the Validation Class. The entered data is also showed
to the user.
  - Add new Coding Session Menu  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/f1e1484f-0aec-4074-82fc-a7b71b9351cf)
  - User input validations:  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/fd01ab70-6edb-4876-8677-4b845ff136e0)
  - Duration in seconds is calculated when correct start and
  end datetime are entered. It is displayed above the Menu.  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/951d2c22-7b40-43ba-9887-8806cd57844d)
- Stopwatch feature:  
    User first need to enter 'S' to start the session and once
    enter second time 'S' to stop the Stopwatch. A session will
    be added using the stopwatch data. User can enter '0' to
    cancel the Stopwatch anytime.  
    ![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/0e14d7f2-b7e6-49f6-8204-39af70775471)
- Update/Delete are also similar Operations.

## Reports

- There are total 4 reports as shown below:  
![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/1bfe99d4-e788-4066-8690-bfe15d73d8a1)
- Example Daily Report:  
![image](https://github.com/javedkhan2k2/42Heilbronn/assets/48986371/5f267d88-d5cb-4947-b162-7d1e79d39d1d)

