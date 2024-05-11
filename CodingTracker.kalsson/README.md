# Coding Session Tracker

## Overview

The **Coding Session Tracker** is a console application designed to help developers track their coding sessions. Users can add, update, view, and delete coding sessions, which are stored in a SQLite database.

## Features

- **Add Session**: Record a new coding session with start and end times.
- **View Sessions**: Display all recorded coding sessions in a table.
- **Update Session**: Modify existing coding sessions.
- **Delete Session**: Remove a coding session from the database.
- **User-Friendly**: Includes options to cancel operations and intuitive prompts.

## Requirements

- **.NET 8.0**: Ensure .NET 8.0 SDK is installed.
- **SQLite**: The application uses SQLite for data storage.
- **Dapper**: A lightweight ORM for data access.

## Usage

### Adding a Session
- Follow the prompts to enter the start and end times.
- Type 'cancel' to abort the operation.

### Viewing Sessions
- View all coding sessions in a table format.

### Updating a Session
- Select a session to update.
- Enter new start and end times.
- Type 'cancel' to abort the operation.

### Deleting a Session
- Enter the ID of the session to delete.
- Confirm the deletion.

## Contribution

Feel free to fork this project and submit pull requests. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
