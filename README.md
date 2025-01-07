# General features:
- Spectre.Console based UI where user can navigate by key presses.
- The application stores and reads data from a real SQLite database.
- When the application starts it creates a SQLite database if one isn’t present.
- It also creates two tables in the database where the coding sessions will be logged and populates another table with default goal values.
- The user can insert, delete, update and view their logged coding sessions.
- The app interacts with the database using Dapper.
- Connection string stored in "appsettings.json" file.

# Additional features
- Users can track the coding time via a stopwatch.
- Users can create quick reports for week, month and year periods.
- Users can create reports with their total and average coding session duration per period.
- Users can set coding goals and see how far they are from reaching their goal.