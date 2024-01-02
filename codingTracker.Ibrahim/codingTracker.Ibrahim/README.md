
# Coding Tracker App

## Introduction
The Coding Tracker App is a C# application designed to log and track coding sessions, emphasizing the handling of dates and times and the integration of external libraries. This project extends the concepts learned in the Habit Tracker project, focusing on Object-Oriented Programming and the separation of concerns, making it an advanced tool for monitoring coding activities.

## Features
- **Log Coding Sessions:** Users can log their coding sessions with start and end times.
- **Automatic Duration Calculation:** The app calculates the duration of each session based on the start and end times.
- **Data Storage and Management:** Uses SQLite for storing and managing session data.
- **Data Visualization:** Coding sessions are displayed in a structured table format.
- **Input Validation:** Robust validation is implemented for user inputs, date, and time entries to ensure data integrity.
- **Stopwatch Functionality:** Features a stopwatch to track coding sessions in real-time.
- **CRUD Operations:** Supports creating, reading, updating, and deleting coding sessions.

## Technologies Used
- **C#**
- **SQLite**
- **ConsoleTableExt**

## Application Components

## Validation in the Application
- **Date and Time Validation:** Ensures that the date and time inputs for coding sessions are in the correct format and logical (e.g., start time is before end time).
- **User Input Validation:** Checks for valid user inputs throughout the application, preventing erroneous data entries and ensuring smooth operation.
- **Session Existence Check:** Verifies the existence of a session before performing update or delete operations.

## User Interaction
- Users can interact with the application through a console interface, where they can add, update, delete, and view coding sessions. A stopwatch feature is also available for real-time session tracking.

