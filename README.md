# Coding Tracker

A CRUD based console application that allows the user to track the time spent coding. Developed using C#/.NET and SQLite.

## Features

### SQLite database connection

  - The program uses a SQLite db connection to store and read information.
  - If no database exists, or the correct table does not exist they will be created on program start.
  
### Console Based UI

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
  


