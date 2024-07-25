# Console Coding Tracker

A C# application used to track coding time. Console based and uses a SQLite database to store coding sessions.

# Requirements

- Keep track of coding sessions with a start date, end date, and name of task worked on.
- Store and retrieve data from a real database.
- On startup, the application should create a database and table if not already created.
- Display a list of options for the user to select from.
- Users can insert, delete, update and view coding session data.
- Application should terminate if user enters '0'.
- User needs to follow specific formats for dates and times.
- Utilize [Spectre.Console Library](https://spectreconsole.net/) for the user interface.
- Utilize Dapper ORM for interacting with the database.
- Session duration should not input by the user but be calculated in a separate method.

# Features

- SQLite database connection

  The program uses a SQLite database to store coding session data. Upon launch, the program checks if a database has been created and creates one if not.

- A console based UI

  The user interface is developed using the [Spectre.Console](https://spectreconsole.net/) library. The UI displays options for creating, updating, reading, and deleting session data. Validation is also performed on all inputs.

- Basic reports of coding session data

  Displays a table containing coding session data.

- Quickstart mode

  Users can quickly start a coding session by using the quick start option in the main menu. After users are finished coding they can stop the session with another option. Users can also view how long a current session has been active.
