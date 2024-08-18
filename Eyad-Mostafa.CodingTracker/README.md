# Coding Tracker

A simple console application to track your coding habits, built with C# and SQLite.

## Table of Contents

- Introduction
- Features
- Requirements
- Installation
- Usage
- Contributing

## Introduction

The Coding Tracker is a console application designed to help developers keep track of their coding habits. It allows users to log their daily coding activities and view their progress over time.

## Features

- Log daily coding activities
- Update existing sessions
- Delete sessions
- View all records
- Simple and intuitive console interface

## Requirements

To run this application, you need the following:

- **.NET SDK**: Version 6.0 or higher
- **SQLite**: Ensure SQLite is installed and accessible
- **Operating System**: Windows 10 or higher, macOS, or Linux

## Installation

To get started with the Coding Tracker, follow these steps:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/yourusername/coding-tracker.git
   ```
2. **Navigate to the project directory:**
   ```bash
   cd coding-tracker
   ```
3. **Build the project:**
   ```bash
   dotnet build
   ```
4. **Run the application:**
   ```bash
   dotnet run
   ```

## Usage

Once the application is running, you can use the following options from the main menu:

- **0 To Close Application:** Exit the application.
- **1 To Add Session:** Log a new coding session.
- **2 To Update Session:** Update an existing coding session.
- **3 To Delete Session:** Delete a coding session.
- **4 To View All Records:** View all logged coding sessions.

### Viewing All Records

When you choose to view all records, the application will display a table with the following columns:

- **ID:** The unique identifier for the session.
- **Date:** The date of the coding session.
- **Start Time:** The start time of the session.
- **End Time:** The end time of the session.
- **Duration:** The duration of the session.
- **Description:** A brief description of the session.

## Contributing

Contributions are welcome! If you have any ideas or improvements, feel free to open an issue or submit a pull request.
