<div align="center">

<img src="./_resources/coding-tracker-logo.png" alt="coding tracker logo" width="100px" />
<h1>Coding Tracker</h1>

</div>

Welcome to the Coding Tracker App!

This is a simple demo console application that allows a user to perform CRUD operations against a database via Dapper.

## Requirements

- [x] Logs daily coding time.
- [x] Uses the "Spectre.Console" library to show the data on the console.
- [x] Has classes in separate files.
- [x] Tells the user the required date and time input format and not allow any other format.
- [x] Has a configuration file that contains the database connection string.
- [x] Stores and retrieve data from a real database.
- [x] Creates a sqlite database, if one isn’t present, when the application starts.
- [x] Creates a table in the database, if one isn’t present, where Coding Sessions will be logged.
- [x] Shows the user a menu of options.
- [x] Allows users to insert, delete, update and view Coding Sessions.
- [x] Has a "CodingSession" class in a separate file, which contains the properties of a coding session: Id, StartTime, EndTime, Duration.
- [x] Allows the user to input the start and end times manually.
- [x] Does not allow the user to input the duration of the session. It must be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] Uses Dapper ORM for the data access instead of ADO.NET.
- [x] Does not use anonymous objects when reading from the database. It must read table data into a list of Coding Sessions.
- [x] Handles all possible errors so that the application never crashes.
- [x] Allows the user to insert 0 to terminate the application.
- [x] Contains a Read Me file which explains how the app works.

### Additional Requirements

- [x] Allows the user to track a live Coding Session time via a stopwatch.
- [x] Allows the user to filter a report of Coding Sessions by period (weeks, days, years) and/or order ascending or descending.
- [x] Allows the user to see their total and average coding session per period on Coding Session reports.
- [x] Allows the user to set coding goals and show how far the user is from reaching their goal, along with how many hours a day they would have to code to reach their goal.

## Features

- **Recording**
 
	Add a new manual coding session entry in the database. Or, use the live session feature to record as you code and track your time and progress.

- **Reporting**

	View all coding sessions entries from the database, and displays them and the total and average durations in the console. Or, filter a coding session report by date range, group by day, week, month, year, and order by ascending or descending.

- **Management**

	Update or delete coding session entries in the database.

- **Database Seeding**

	Set `SeedDatabase` to `true` in the appsettings.json file if you wish to generate mocked up seed data on initial database creation.

## Getting Started

### Prerequisites

- .NET 8 SDK installed on your system.
- A code editor like Visual Studio or Visual Studio Code

### Installation

1. Clone the repository:
	- `git clone https://github.com/cjc-sweatbox/coding-tracker.git`

2. Navigate to the project directory:
	- `cd src\coding-tracker\CodingTracker.ConsoleApp`

3. Run the application using the .NET CLI:
	- `dotnet run`

### Running the Application

1. Run the application using the .NET CLI in the project directory:
	- `dotnet run`

## Usage

**Main Menu**

When you start the application, you will be presented with the main menu:

![coding tracker main menu](./_resources/coding-tracker-main-menu.png)
Choose an option from the choices to perform.

**Live Coding Session**

Start live coding session will prompt the user to start and stop a live coding session. A timer will show and track progress:

![coding tracker live session](./_resources/coding-tracker-live-session.png)
Press any key to start, and stop the session. A message page will appear once the session has been added to the database.

**View Coding Session Report**

Returns all CodingSessions from the database in a raw view. Shows the total and average durations below the table:

![coding tracker coding session report top](./_resources/coding-tracker-coding-session-report-top.png)
![coding tracker coding session report bottom](./_resources/coding-tracker-coding-session-report-bottom.png)
Press any key to return to the main menu.

**Filter Coding Session Report**

Allows the user to filter a CodingSessions report. Also shows the total and average durations for the filtered report below the table:

![coding tracker filter coding session report type](./_resources/coding-tracker-filter-report-type.png)
![coding tracker filter coding session report range](./_resources/coding-tracker-filter-report-range.png)
![coding tracker filter coding session report](./_resources/coding-tracker-filter-report.png)
Press any key to return to the main menu.

**Create Coding Session**

Allows the user to add a manual CodingSession to the database. Inputs must be in the correct format and valid. End date must be after the start date:

![coding tracker create coding session](./_resources/coding-tracker-create-coding-session.png)
A message page will appear once the session has been added to the database.

**Update Coding Session**

Allows the user to choose a CodingSession to be updated in the database. Then prompts for the updated values. Again, inputs must be in the correct format and valid. End date must be after the start date:

![coding tracker update coding session](./_resources/coding-tracker-update-coding-session.png)
A message page will appear once the session has been updated in the database.

**Delete Coding Session**

Allows the user to choose a CodingSession to be deleted in the database:

![coding tracker delete coding session](./_resources/coding-tracker-delete-coding-session.png)
A message page will appear once the session has been deleted from the database.

**Set Coding Goal**

Allows the user to set a CodingGoal:

![coding tracker set coding goal](./_resources/coding-tracker-set-coding-goal.png)
A message page will appear once the goal has been set in the database.

## How It Works

- **Console Application**: Display is mostly through the [Spectre Console](https://spectreconsole.net/) library.
- **Menu Navigation**: Navigate the application through the Selection Prompts class provided by Spectre to perform actions.
- **Data Storage**: A new sqlite database is created and the required schema is set up at run-time, or an existing database is used if previously created.
- **Data Access**: [Dapper](https://github.com/DapperLib/Dapper) is used for the database access methods.
- **Data Seeding**: If the associated configuration setting is set and there are no CodingSessions in the database, a set of 100 mock objects will be added.
- **Report Display**: Uses the Tables class  provided by Spectre to display structured and formatted tables.

## Database

![coding tracker entity relationship diagram](./_resources/coding-tracker-entity-relationship-diagram.png)

## Contributing

Contributions are welcome! Please fork the repository and create a pull request with your changes. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Contact

For any questions or feedback, please open an issue.

---
***Happy Coding Tracking!***
