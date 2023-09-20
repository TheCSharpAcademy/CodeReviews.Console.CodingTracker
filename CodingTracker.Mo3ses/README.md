# The C# Academy - CodingTracker 

## Introduction
This app should be very similar to the Habit Tracker you’ve previously completed. It will serve the purpose of reinforcing what you’ve learned with a bit of repetition and building on that knowledge with slightly more challenging requirements.

This time you’ll have to deal with the complexity of handling Dates and Times, which is a real challenge in any application. You’ll also be using your first external library. Often times in professional environments programmers don’t reinvent the wheel and save time by using public solutions provided by other coders. That’s the beauty of the internet. You have access to an amazing coding community!

In the first app we also didn’t have requirements for coding organization. This time you’ll have to use separation of concerns, one of the most important principles in modern programming. This is where you’ll start applying concepts of Object Oriented Programming. You’ll also need to use a “Model” or “Entity”, to to represent the data you are dealing with. In this case, your coding sessions. So let’s get started!

## Requirements
 -  [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.
 -  [x] To show the data on the console, you should use the "ConsoleTableExt" library.
 -  [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
 -  [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
 -  [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
 -  [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
 -  [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
 -  [x] The user should be able to input the start and end times manually.
 -  [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.
 
 
 ## Challenges
 -  [ ] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
 -  [ ] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
 -  [ ] Create reports where the users can see their total and average coding session per period.
 -  [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.