# Coding Tracker

## Introduction

This project is similar to the Habit Tracker that I previously completed.
The main differences in this project are the use of dates and time to calculate time spent coding.
Further to this I need to start applying Object Oriented Programming Concepts to my code.

## Requirements

- [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
- [x] To show the data on the console, you should use the "Spectre.Console" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

## Tips

- It's up to you the order in which you'll build, but we recommend you do it in this order: configuration file, model, database/table creation, CRUD controller (where the operations will happen), TableVisualisationEngine (where the consoleTableExt code will be run) and finally: validation of data.
- Sqlite doesn't support dates. We recommend you store the datetime as a string in the database and then parse it using C#. You'll need to parse it to calculate the duration of your sessions.
- Don't forget to push your changes to github every time you stop working.
- Don't forget the user input's validation: Check for incorrect dates. What happens if a menu option is chosen that's not available? What happens if the users input a string instead of a number? Remember that the end date can't be before the start date.

## Project Challenges

- [ ] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and average coding session per period.
- [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

## Features of the program

# Visual tables

- By leveraging Spectre.Console I was able to make some smarter looking tables than the usual console application.

# CRUD functionality

- The program allows a user to Create, Read, Update & Delete data within the SQLite DB

# Selection Menu

- Reading the Spectre.Console documentation I stumbled across the ability to create interactive menus in the console which have been used throughout.

## Lessons Learned

Throughout the development of this project there were many new tools that I needed to learn to effectively complete the requirements. Below is a list of the lessons I learned:

- DateTime is a absolute nightmare to work with. I tried adding in days to the tracker and immediately regretted it as all calculations broke. Lots to learn here.
- You can spend a lot of time on the frontend of an app.
- I really enjoyed playing with Spectre and using it to assist validation on inputs and to enhance the user experience.
- Configuration files are an interesting way to store file paths and other information outside of the project.
- Propagated exceptions are a thing. Spent hours wondering why I hadn't caught the InputZero exceptions only to find out I had... in one area, needed to catch the exception in related methods too.
- Took a big step forward with OOP. the MVC pattern started to make it all click.

## Areas to Improve

- Whilst I began using OOP well to begin with I suspect there is quite a lot of refactoring still to do but I got a bit burnt out and wanted to end the project.
- DateTime. Does anyone even truly understand it?
- Focus on one area at a time. I probably lost time by cleaning up areas as I went and losing track of what I was originally setting out to do.

## Resources

- [Separation of concerns](https://en.wikipedia.org/wiki/Separation_of_concerns)
- [Spectre Console](https://spectreconsole.net)
- [MSFT Configuration Manager](https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/store-custom-information-config-file)
- [Rider Build configurations](https://www.jetbrains.com/help/rider/Build_Configurations.html)
- [Dapper Tutorial](https://www.jetbrains.com/help/rider/Build_Configurations.html)
- [SQL Injection](https://www.w3schools.com/sql/sql_injection.asp)
