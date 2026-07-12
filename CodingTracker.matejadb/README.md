# Coding Tracker Console Application

Simple CRUD console application that tracks coding sessions.
Developed in C#, database used is SQLite.

## About

This is a console application for logging and tracking coding sessions by start time, end time, and calculated duration. Each entry records when a coding session began and ended, with the duration automatically computed.
Data is persisted in a local SQLite database and accessed through ADO.NET (`Microsoft.Data.Sqlite`) with Dapper as a micro-ORM for simplified query execution. The UI is enhanced with **Spectre.Console** for rich, colorful terminal output.

## How To Run

- Visual Studio 2026 or later (or .NET 10 SDK)
- Download dependencies from NuGet Package Manager (Dapper, Microsoft.Data.Sqlite, Spectre.Console, Microsoft.Extensions.Configuration)
- Run the application (CTRL + F5)
- On first run, the app automatically creates the `coding_tracker.db` SQLite database file and the `coding_tracker` table if they don't already exist, no manual setup needed

## Features

### SQLite Database
- Program uses SQLite to store and read coding session information.
- If no database or table exists, they will be created when the program starts.
- Database access leverages ADO.NET (`Microsoft.Data.Sqlite`) with Dapper for efficient parameterized queries.
- All queries are parameterized to prevent SQL injection.

### Dapper Micro-ORM
- Simplifies mapping between database results and C# objects.
- Lightweight alternative to full-featured ORMs like Entity Framework.
- Makes query execution and parameter binding more concise and readable.

### Rich Console UI
- **Spectre.Console** provides beautiful, colorful table output for displaying sessions.
- Interactive menu system with selection prompts.
- Color-coded messages for user feedback (success messages in green, etc.).

## Project Structure

```
CodingTracker.matejadb.slnx
├── CodingTracker.matejadb/           # Main console application
│   ├── Program.cs                    # Entry point, initializes DB and UI
│   ├── Config/
│   │   └── AppSettings.cs            # Configuration management
│   ├── Controllers/
│   │   ├── CodingSessionController.cs # CRUD operations for sessions
│   │   ├── BaseController.cs          # Base class for controllers
│   │   └── IBaseController.cs         # Controller interface
│   ├── Database/
│   │   └── DatabaseManager.cs         # Database operations
│   ├── Models/
│   │   └── CodingSession.cs           # CodingSession data model
│   ├── UI/
│   │   ├── UserInterface.cs           # Main menu loop and navigation
│   │   └── Enums/
│   │       └── Menu.cs                # Menu action enum
│   ├── Utils/
│   │   ├── CalculateSessionDuration.cs # Session duration calculation
│   │   ├── UserInput.cs               # User input handling
│   │   └── Validation.cs              # Input validation logic
│   ├── appsettings.json               # Configuration file
│   └── coding_tracker.db               # SQLite database file (auto-created on first run)
└── CodingTracker.matejadb.UnitTests/   # NUnit test project
	└── (Unit tests)

```

## Database Schema

**Table: `coding_tracker`**

| Column    | Type    | Notes                                     |
|-----------|---------|-------------------------------------------|
| Id        | INTEGER | Primary key, autoincrement                |
| StartTime | TEXT    | Start time of the coding session  |
| EndTime   | TEXT    | End time of the coding session  |
| Duration  | TEXT    | Calculated duration of the session        |

Created on startup with:

```sql
CREATE TABLE IF NOT EXISTS coding_tracker (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	StartTime TEXT NOT NULL,
	EndTime TEXT NOT NULL,
	Duration TEXT NOT NULL
);
```

## Menu Options

1. **View Sessions** – displays all logged coding sessions in a formatted table (Id, Start Time, End Time, Duration).
2. **Add Session** – prompts for start time and end time, automatically calculates duration, then inserts a new row.
3. **Update Session** – shows all entries, asks for a session selection, then overwrites the start time, end time, and duration for that row.
4. **Delete Session** – shows all entries, asks for a session selection, then deletes that row.
5. **Exit** – closes the application.

## Input Validation

- **DateTime inputs** are validated with `DateTime.TryParseExact` a predetermined format before being accepted, via a reusable `GetDateTimeFromUser` method that re-prompts until valid input is provided.
- **Duration calculation** automatically computes the time span between start and end times, eliminating manual user entry for duration.
- Centralizing this logic into small reusable helper methods (rather than repeating parsing/validation inline) follows the DRY principle.

## Error Handling

- All SQL commands use parameterized queries (`@startTime`, `@endTime`, etc.) to avoid SQL injection and type-mismatch issues.
- User input is validated *before* it ever reaches a SQL command, which keeps the data-access code simpler since it can trust the values it receives.

## Key Libraries

- **Microsoft.Data.Sqlite** – SQLite database provider for .NET.
- **Dapper** – Lightweight micro-ORM for ADO.NET.
- **Spectre.Console** – Rich console library for beautiful terminal UI.
- **Microsoft.Extensions.Configuration** – Configuration management.

## Unit Tests

The `CodingTracker.matejadb.UnitTests` project uses NUnit to test core logic in isolation, ensuring reliability of validation and calculation functions.

## Challenges

- Learning to work with Dapper for the first time and understanding how it simplifies parameter binding compared to raw ADO.NET.
- Integrating Spectre.Console to create an attractive and user-friendly console interface.
- Ensuring DateTime parsing is strict and predictable across different system locales.

## Lessons Learned

- Got hands-on practice with Dapper as a lightweight ORM alternative that balances convenience and control.
- Reinforced the importance of parameterized queries for security and type safety in database operations.
- Learned how Spectre.Console can significantly enhance user experience in console applications with minimal effort.
- Discovered how separating concerns (UI, Controllers, Database, Utils) makes the codebase more maintainable and testable.
- Writing validation logic as standalone static methods made it trivial to unit test without needing database access or console I/O.

## Areas to Improve

- Add filtering or searching capabilities (e.g., view sessions by date range).
- Consider adding data export functionality (e.g., to CSV).

## Future Features

- Adding the possibility of tracking coding time via a stopwatch
- Letting the user filter their coding records
