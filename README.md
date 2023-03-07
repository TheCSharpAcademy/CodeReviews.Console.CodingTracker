# Coding Tracker

A CRUD based console application that allows the user to track the time spent coding. Developed using C#/.NET and SQLite.

## Features

### SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.
  
### Console Based UI
- Most UI elements make use of the [ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt) to write the content in neat tables

### View
- View all logs by their table order
- View logs between user specified dates
- View logs in ascending/descending order of dates

### Insert
- Manual Insert
  - When inserting a new log, a start date and time and an end date and time is required
  - Checks made
  - Input is in the correct format
  - End date and time isn't earlier than the start date and time
  - There's no overlap with the current logs
  
- Stopwatch insert
  - Starts a timer, that can then be stopped to record a new session
  - Current time in the session is displayed in the console
 
### Update/Delete logs
- What the title implies
 
### Reports
<img src ="https://user-images.githubusercontent.com/64802476/223473097-af5cdbb8-d387-4686-80cd-5c4d5dfb5874.png" width=25%>

- Allows the user the view statistic about all logs or a date interval of his choice

### Goals
<img src="https://user-images.githubusercontent.com/64802476/223470652-3db8f1ce-160f-4a82-b00a-a0146a35af86.png" width=25%>

- The user can set a goal for his coding sessions
  - A goal has a start date, an end date and a target number of hours
  - The user can view the statistics of his goal, and be punished with existential-dread-causing messages when the goal can no longer be achieved
  


## Challenges
- Learning deal with the DateTime's immutable properties
- Working with ConsoleTableExt for the first time
- Working with appsettings
- Continuing to work with SQLite
- Planning ahead when using classes


## Lessons learned
- Became a bit more familiar with SQL(ite)
- Organized my code better, with more classes
- Learned to use appsettings, which will definitely use in future projects


## Areas to improve
- Keep learning more SQL
- Separate classes even further. Towards the end of the project I realized I should've had more classes, but at that point I was to close to finishing to go restructure
- Be more efficient, and organize what I am currently working on. When a new bug pops up, I try to immediately solve it and I end up losing track of what I was doing before, which ends up costing me a lot of unnecessary time.



## Resources
- The [C#Academy project](https://www.thecsharpacademy.com/project/13) was the project guide.
- I used a lot of code from my [previous project](https://github.com/ThePortugueseMan/CodeReviews.Console.HabitTracker).
- The [C#Academy discord community](https://discord.com/invite/JVnwYdM79C) that are always ready to help!
- The [C#Academy coding coventions](https://thecsharpacademy.com/article/58) to help clean up the code and stick to the coding conventions.
- This [tidy ReadMe](https://github.com/thags/ConsoleTimeLogger#readme) was an inspiration for my own
- Various resources from all over the web.
