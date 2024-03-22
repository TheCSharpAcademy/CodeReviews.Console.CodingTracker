# Coding Tracker Console App

This is a one of the C# Academy projects. This console app is for tracking daily
coding time. The application allows users to enter their start and end times of
the coding session and application will calculate length of that session. Users
can perform various operations on the logged data. Below are the requirements, 
features, user manual, areas for improvement, and additional challenges of 
the application.

## Requirements

- [x] Same basic requirements as the Habit Tracker (C# Academy project)
- [x] The "Spectre.Console" library must be used to display data on the console
- [x] Separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
      must be implemented
- [x] Users should enter date and time in a specific format; no other formats are allowed
- [x] Include a configuration file containing the database path and connection strings
- [x] Implement a "CodingSession" class in a separate file, which contains the properties of a
      coding session: Id, StartTime, EndTime, Duration.
- [x] The application should calculate the duration of a coding session based on the Start and End times
      using a separate "CalculateDuration" method; users should not enter it manually.
- [x] User should be able to manually input the start and end times
- [x] Use Dapper ORM for the data access instead of ADO.NET
- [x] When reading from the database, retrieve data into a List of Coding Sessions rather
      than using anonymous objects

## Features and validations

- **SQLite database connection**: The application connects to a SQLite database
  to store and read data.
  - App.config file contains the configuration for that.
- **Spectre.Console -library utilization**: Used for menu selections, displaying record data to users, and enhancing console presentation with colors.
- **New Database Creation**: If the database or table doesn't exist, the application
  creates them when it starts.
- **User input checks**:
  - Date and time must be entered in format YYYY-MM-DD HH:mm.
  - Duplicate start times cannot be entered unless updating an existing record.
- **ID check**: Verify if the entered ID exists in the database before
  performing operations. 
  
## User Manual

### Menu Navigation

- Users navigate the Main Menu by selecting options using arrow keys and confirming their
  selection by pressing Enter 
  ![CodeMenu](https://github.com/HopelessCoding/learning/assets/161690352/08399f8e-ef48-4fa1-9f12-8a3ea03d509a)

### Menu Options

- **A - Add New Record**: Allows the user to add a new record.
  - User should enter a valid start time or leave it empty to use current date and time
  - If start time already exists program writes a message and returns to main menu
  - User should enter a valid end time (can already exist)
- **V - View All Records**: List all record from the database  
  ![CodeView](https://github.com/HopelessCoding/learning/assets/161690352/5b025667-ea95-4054-92c7-2ce2df3dc6a4)
- **U - Update Record**: Updates an existing record by entering its ID
  - User should enter a valid start time or leave it empty to use current date and time
  - If start time already exists program writes a message and returns to main menu
  - User should enter a valid end time (can already exist)
- **R - Show Reports**: Opens reports menu
- **D - Delete Record**: Deletes a record by entering its ID
- **0 - Close Application**: Terminates the application

### Reports Menu Options
- Users navigate the Reports Menu by selecting options using arrow keys and confirming their
  selection by pressing Enter  
![ReportsCodeMenu](https://github.com/HopelessCoding/learning/assets/161690352/4a2cf916-1b25-411c-a603-c2a1be010834)

- **X - Report for specific time period**: Shows records and calculates the total and average
  coding time for the specified time period entered by the user  
  ![ReportsCodeView](https://github.com/HopelessCoding/learning/assets/161690352/2c310f1a-f630-4dcf-9c07-05ee2531dc13)
- **0 - Exit to Main Menu**: Returns back to main menu

## Areas for Improvement and Lessons Learned

- **Enhanced Code Quality**: This code is a nice upgrade from the Habit Logger project,
  various areas of improvement identified in the previous project were now done better.
- **Code Structure**: The code has a mostly nice well-organized structure, and
  I'm happy at the results this time (There is probably still room for improvements).
- **Return to Main Menu**: It would be nice to add clear funtionality which
  would return user back to main menu when they have already selected a command.
  - Now the user is returned back to the main menu by pressing Enter or entering
    a faulty value
- **Spectre.Console -library**: This documentation is not very good in all areas and learning to
  use its full potential requires time and testing.
  - Mostly makes just things to look nice so not focusing to it too much now
  - May further explore and utilize this feature (maybe in future C# Academy projects)
 - **SQL**: It offers very wide range of funtionalities and possibilites. Learning to use those efficiently will
   require time.

## Additional challenges for future

- [x] Funtionality to generate reports where the users can see their total and average coding session per specified period.
- [ ] Possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [ ] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.
- [ ] Create the ability to set coding goals and show how far the users are from reaching their goal, along with how
      many hours a day they would have to code to reach their goal. You can do it via SQL queries or with C#.
