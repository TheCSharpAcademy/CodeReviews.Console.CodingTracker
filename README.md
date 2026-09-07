# Coding Tracker

A console-based productivity application to log and manage coding sessions, developed with C#, SQLite, Dapper, and Spectre.Console.

## Features

- **Live Session Timer**: Start an interactive coding session with a live-updating Spectre console display showing elapsed time in real-time.
- **Manual Logging**: Add past coding sessions manually with strict input parsing.
- **Session History & Management**: View recorded sessions formatted in clean CLI tables.
- **Full CRUD Operations**: Create, Read, Update, and Delete sessions with immediate ID validation.

## Architecture & Code Structure

The project strictly follows the Single Responsibility Principle and separation of concerns:

- `CodingSession`: Data model representing a coding session, storing timestamps and calculating durations.
- `CodingController`: Handles database persistence and executes parameterized SQL queries using **Dapper**.
- `UserInterface`: Manages menu flows, screen outputs, and table renderings via **Spectre.Console**.
- `UserInput`: Centralizes console input collection, menu prompts, and buffer management.
- `Validation`: Enforces constraints, positive integer parsing, and date-time validation (`dd-MM-yyyy HH:mm:ss`).
- `TimeCalculator`: Helper logic for calculating differences between session timestamps.

## Technologies Used

- **C# / .NET**
- **SQLite** (`Microsoft.Data.Sqlite`)
- **Dapper** (Micro-ORM for parameterized queries)
- **Spectre.Console** (CLI rendering and interactive prompts)
- **Microsoft.Extensions.Configuration** (Database connection string via `appsettings.json`)

## Thought Process
- **Architecture First**: Before writing business logic, I mapped out dedicated responsibilities (`CodingController` for persistence, `UserInput` for prompts, `Validation` for parsing) to ensure maintainability and clean data flow.
- **User Experience**: Console apps often suffer from clunky navigation. By leveraging `Spectre.Console`, I focused on clear table layouts, real-time visual feedback for active sessions, and defensive input parsing to prevent runtime crashes.
- **Data Integrity**: Enforcing strict timestamp formats and parameterized queries via Dapper was prioritized early to avoid malformed session records in SQLite.

## Configuration

Ensure `appsettings.json` contains a valid SQLite connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=codingTracker.db"
  }
}
