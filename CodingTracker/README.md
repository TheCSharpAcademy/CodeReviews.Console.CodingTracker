# Coding Tracker

**C# Console Application | SQLite | Dapper | Spectre.Console**

A console-based CRUD application to track daily coding sessions, built as part of the C# Academy learning path. This project builds on the Habit Logger project, introducing date/time handling, configuration files, an ORM (Dapper), and console UI styling with Spectre.Console.

---

## Requirements

| Feature | Status |
|---|---|
| Log daily coding sessions | ✅ |
| Session tracked by Start/End time, with Duration calculated automatically | ✅ |
| Users can input the date of the session | ✅ |
| Store and retrieve data from a real SQLite database | ✅ |
| Create a SQLite database on startup if not present | ✅ |
| Create a table in the database where sessions are logged | ✅ |
| Insert, delete, update and view logged sessions | ✅ |
| Enforce a strict, user-communicated date/time input format | ✅ |
| Display data using the Spectre.Console library | ✅ |
| Separate classes in different files (Separation of Concerns) | ✅ |
| Configuration file (appsettings.json) for DB path/connection string | ✅ |
| Dedicated CodingSession model class, read into a typed List (no anonymous objects) | ✅ |
| Use Dapper ORM instead of ADO.NET | ✅ |
| Follow the DRY Principle and avoid code repetition | ✅ |
| Filter records by date/year/month | ✅ |
| Stopwatch-based live session tracking | ✅ |
| Handle all possible errors so the application never crashes | ✅ |
| Unit tests for validation/filtering methods | ❌ |

---

## How It Works

From the main menu the user can:

- View all entries
- Start a new session, tracked live via a stopwatch
- Add a session manually, entering the date, start time, and end time
- Delete a session
- Update a session
- Filter entries by date, year, or month

Duration is never entered directly by the user — it's always calculated from the Start and End times.

---

## Challenges

The Dapper commands were very similar to the raw SQL I'd already used with ADO.NET in the Habit Logger project, which made the data-access layer easy to adapt.

My biggest problem in this project was integrating the `appsettings.json` file — getting it to actually load at runtime, and passing its values down cleanly into the classes that needed them (connection string, date/time formats, table name), instead of leaving them hardcoded. That took the most trial and error to get right.

I also ran into a couple of subtle date/time bugs along the way — mapping a `TimeSpan` column from SQLite through Dapper initially crashed, since Dapper can't auto-convert a stored string into a `TimeSpan`. I also hit a `FormatException` when I forgot to escape colons in a custom `TimeSpan` format string, which taught me that `TimeSpan` and `DateTime` don't follow identical formatting rules.

Other pieces — filtering, using Spectre.Console for table output, and building the stopwatch-based session tracker — were more straightforward once the core CRUD and configuration setup were in place.

---

## Areas to Improve

- **DRY Principle** — There's still some repetition in the filtering logic (parsing dates from strings in multiple places) that could be consolidated further.
- **Unit tests** — Not implemented in this project. Something I want to add going forward, particularly for input validation and filtering methods.

---

## What I Learned

- Connecting to and interacting with SQLite using Dapper
- Working with the Spectre.Console library for console table output
- Creating a JSON configuration file and wiring it into multiple classes via `IConfiguration`
- The differences between `DateTime`, `TimeOnly`, and `TimeSpan`, and where each one fits
- Custom format strings for `TimeSpan`, and how they differ from `DateTime` formatting
- Filtering data using LINQ (`Where`, `OrderBy`) combined with `DateTime.ParseExact`
- Using primary constructors to reduce repeated parameter passing across methods
