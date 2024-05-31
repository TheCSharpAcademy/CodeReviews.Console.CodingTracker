# TODO List

- [x] Create a configuration file to store the database path and connection strings.
- [x] Create a `CodingSession` class in a separate file with properties: Id, StartTime, EndTime, Duration.
- [x] Create a SQLite database if not present at application start.
- [x] Create a table in the database for coding session logging.
- [ ] Implement data access using Dapper ORM.
- [x] Create a `UserInput` class to handle user input.
- [ ] Create a `Validation` class to validate user input.
- [ ] Ensure end date/time is not before the start date/time in `Validation`.
- [x] Use the "Spectre.Console" library to display data on the console.
- [x] Implement the main menu with options
- [ ] Implement a stopwatch feature to track coding time live.
- [ ] Allow users to filter coding records by period (weeks, days, years) and order them (ascending/descending).
- [ ] Create reports for total and average coding sessions per period.
- [ ] Add a feature to set coding goals and track progress.
- [ ] Include a ReadMe file explaining the app's functionality and how to use it.