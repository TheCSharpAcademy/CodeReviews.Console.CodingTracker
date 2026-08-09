# Coding Tracker

## About app
Coding Tracker is a C# console application for tracking coding time. Users can create, view, update, and delete coding sessions. Each coding session includes a start time, end time, and a calculated duration. Data is stored in an SQLite database, with access implemented via Dapper. The UI is built using the the Spectre.Console library. The app supports both manual time entry and a built-in stopwatch, as well as filtering sessions by date range.

## Architecture
### UI (User Interface)
- `UI/UserMenu.cs` - The main menu and the flow for interacting with the user (viewing, adding, deleting, updating sessions, starting a stopwatch, filtering by dates)
- Responsible for navigation and input/output data into the console.
### Service layer
- `Services/CodingStopwatch.cs` -  the stopwatch logic: starting, stopping, calculating the session duration, and then outputting the results via Spectre.Console.
-  It works with the `CodingSession` model and doesn't know about the database.

### Work with the Data (Data Access)
- `Data/DataAccess.cs` - access to SQLite via Dapper:  tables creation, CRUD operations, retrieving filtered queries.
- Constains SQL query logic and all interaction with the database.
### Helper classes
- `Helpers/DateHelper.cs` - validation and data parsing, helper methods for checking correct date ranges.
- `Helpers/InputValidation.cs` - numeric input validation from the console, divided into clear parsing logic and a wrapper arount `Console.ReadLine`.
- `Helpers/UserPrompts.cs` - console prompts and table/message outputs via Spectre.Console.

### Exceptions
-  `Exceptions/ReturnToMainMenuException.cs` - the special exception for returning to the main menu from withing the logic, without hard dependancy on the UI.

### Entry Point
- `Program.cs` - the database connection configuration (via `appsettings.json`), initializing `DataAccess` and running `UserMenu`.

This separation keeps database access, console input/output, business-logic (validation, filtering, duration calculation) in distinct places, which made it possible to isolate and unit test the business-logic on its own.

## Thought process (design decisions)
### Applying DRY (Don't Repeat Yourself)
Early in the project I noticed that some pieces of logic were being repeated across multiple methods. For example, the logic for retrieving a single `CodingSession` by its Id was needed in both `DeleteCodingSession` and `UpdateCodingSession`. Instead of duplicating the query and mapping code in each method, I extracted this chunk of code into a dedicated `SelectById` method inside `DataAccess`. This allowed both delete and update operations to reuse the same implementation, reduced duplication.

The same DRY principle was applied to other helpers. Common console prompts and table rendering were centralized in `UserPrompts`, rather than being hand‑coded in each UI flow. This keeps the UI consistent and avoids repeating Spectre.Console setup code in multiple places. Likewise, the logic for validating numbers and dates was moved into `InputValidation` and `DateHelper` so that all input checks share the same rules.

### Separating concerns between layers
Another important decision was to separate responsibilities into layers:

- The `UserMenu` class focuses on navigation and user interaction (main menu, choices, prompts).
- The `DataAccess` class is responsible for talking to SQLite via Dapper (creating tables, inserting, updating, deleting, querying).
- Helper classes (`DateHelper`, `InputValidation`, `UserPrompts`) handle validation and common UI/console behavior.
- The `CodingStopwatch` service encapsulates stopwatch behavior and the logic for measuring a coding session without being tightly coupled to the database.

This separation makes the code easier to reason about: when something goes wrong with the database, the problem is likely in `DataAccess`; when input validation fails, the issue is probably in the helpers; and UI behavior lives in the menu/service layer. It also prepares the project for future changes, like replacing the console UI or switching to a different persistence mechanism.

### Refactoring for testability
While working on unit tests, I refactored methods that mixed business logic with console I/O. For example, `GetValidDate` originally both read from `Console.ReadLine` and parsed the date. To make the parsing logic testable, I extracted a pure method `IsValidDate` that only takes a string and a format and returns a boolean and a parsed `DateTime`. The remaining method now focuses on the user interaction loop (prompts, retry messages) and delegates the actual validation to the helper function.

A similar approach was used for numeric input: instead of testing `GetNumberInput` directly, a `TryParseValidNumber` function was introduced to encapsulate the rules for valid integer values. This keeps business rules in testable, side‑effect‑free helpers, while the console‑specific logic remains in the UI layer.

### Improving clarity of business logic
The `CodingSession` model exposes a computed `Duration` property (`EndTime - StartTime`). This decision helps keep duration logic in one place instead of recomputing it in multiple parts of the UI. Methods like `IsStartBeforeEnd` in `DateHelper` express important business rules explicitly (a session cannot end before it starts), which makes the code more self‑documenting and easier to test.

Together, these decisions (applying DRY, separating concerns into layers, and extracting pure functions for validation and calculation) were aimed at making the project easier to maintain, extend, and cover with unit tests.

### Unit Tests
### Tech Stack
- C# / .NET — application language and runtime.
- SQLite — local storage for coding sessions.
- Dapper — lightweight micro-ORM for database access.
- Spectre.Console — console UI (menus, tables, prompts, live updates).
- xUnit — unit testing framework.

### Getting Started
- Prerequisites
  - .NET SDK installed.
  - appsettings.json configured with a valid SQLite connection string under ConnectionStrings:DefaultConnection.
### Features
- Add, view, update, and delete coding sessions.
- Track a session in real time with a built-in stopwatch.
- Filter sessions by last 24 hours, last 7 days, last 30 days, last year, or a custom date range.
- Sort filtered results in ascending or descending order by start time.