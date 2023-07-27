# Coding Tracker

CRUD, SQL, XML files, Separation of Concerns as well as other OOP principles.

## Description

- Create a database to store my coding hours(default/ user can create a new record if they want to).
- If you want to check out my code feel free to!
- The database is stored in [Your project folder]\db\coding-tracker.db
- Unit Tests included to check the calculations.
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
- [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [x] Create reports where the users can see their total and average coding session per period.
- [x] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.

## Features
- This project nearly has the same functionality as my previous [HabitLogger](https://github.com/InfiniteBlanK3T/STUDY.Personal.HabitLogger)
- There are some new functionalities that will have (*) after it - Challenges.
- ![image](https://user-images.githubusercontent.com/94949422/216315572-2300f60f-4deb-4466-98fa-f579963e8b31.png)
- Outstanding features compared to the previous project would be Report Option:
![image](https://user-images.githubusercontent.com/94949422/216317270-1eaabb50-999d-4d43-bc56-2d91a2079dba.png)
- Option (1) would let the table sort itself out according to filter inputs.
![image](https://user-images.githubusercontent.com/94949422/216317571-252662e8-1420-4ff1-b039-cd01e02d4f78.png)
-Then there will be a report accordingly based on that result with filters
![image](https://user-images.githubusercontent.com/94949422/216317803-763a3966-aaf8-4734-b25e-6c323e940404.png)
- Option (2) Report option will let users set their own goal according to their preferences and calculation based on their sets of goals.
![image](https://user-images.githubusercontent.com/94949422/216318314-388dd78d-632a-40c4-b81e-f64424dfced8.png)



## Resources

- [The C# Academy](https://thecsharpacademy.com/)
- [Microsoft C# Documents](https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/?view=vs-2022)
- [freeCodeCamp || how to write README guide](https://www.freecodecamp.org/news/how-to-write-a-good-readme-file/)

## License

[MIT](https://choosealicense.com/licenses/mit/)
