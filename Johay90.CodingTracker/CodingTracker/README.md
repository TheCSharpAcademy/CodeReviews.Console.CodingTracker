# Coding Tracker

This project is a console-based application designed to track coding sessions and set coding goals. It uses `Spectre.Console` for creating a rich, interactive user interface, and `Dapper` for database operations. The application allows users to start new coding sessions, add sessions manually, view previous sessions, manage coding goals, and insert test data.

## Features

- **New Coding Session**: Start a live tracking session to monitor your coding time.
- **Add Manual Session**: Manually add coding sessions by specifying start and end times.
- **View Sessions**: View and filter previous coding sessions.
- **Manage Goals**: Add, view, and delete coding goals.
- **Insert Test Data**: Insert random test data into the database for testing purposes.

## Project Structure

- **App.cs**: Main entry point of the application. Manages the flow of the program.
- **DatabaseManager.cs**: Handles database initialization and connection.
- **CodingRepository.cs**: Manages CRUD operations for coding sessions.
- **GoalRepository.cs**: Manages CRUD operations for coding goals.
- **UserInput.cs**: Handles user interactions and input validations.
- **LiveTracker.cs**: Tracks live coding sessions using a stopwatch.
- **Utilities.cs**: Provides utility methods for generating random data.
- **TimeSpanHandler.cs**: Custom handler for mapping `TimeSpan` to and from the database.

## Usage

### Main Menu Options

1. **Start a new session**: Begins a live coding session.
2. **Add a session manually**: Adds a session by specifying start and end times.
3. **View previous sessions**: Displays a list of previous coding sessions with filtering options.
4. **Add, update or view goals**: Manages coding goals.
5. **Insert Test Data**: Inserts random test data into the database.
6. **Exit the application**: Closes the application.

### New Session Options

- **Start**: Starts the stopwatch for the live session.
- **Reset**: Resets the stopwatch.
- **Get Updated Time**: Updates the elapsed time of the current session.
- **Save & Exit to main menu**: Saves the session and returns to the main menu.

### Goal Menu Options

- **Add a new goal**: Adds a new coding goal by specifying the number of hours.
- **View Current Goals**: Displays a list of current goals.
- **Remove a goal**: Deletes an existing goal.
- **Insert test goals**: Inserts random test goals into the database.