# CodingTracker

A console-based application for tracking and managing coding sessions with persistent storage capabilities.

## Overview

CodingTracker is a .NET 10.0 console application designed to help developers monitor their coding activity by recording session start times, end times, and calculating session durations. The application provides a user-friendly menu interface for managing coding session data.

## Design Thought Process

### 1. **Architecture Pattern: Layered Architecture**

The project follows a clean, layered architecture approach with separation of concerns:

```
Program.cs (Entry Point)
    ↓
CodingController (Presentation Layer)
    ↓
Database (Data Access Layer)
    ↓
Models (Domain Layer)
```

**Rationale:**
- **Separation of Concerns**: Each layer has a single responsibility
- **Maintainability**: Changes in one layer don't cascade through the entire application
- **Testability**: Each component can be tested independently
- **Scalability**: New features can be added without restructuring existing code

### 2. **Database Strategy: SQLite**

The application uses SQLite combined with Dapper for data persistence.

**Why SQLite?**
- Lightweight and file-based—no external database server needed
- Perfect for console applications and small-to-medium projects
- Easy deployment and distribution
- Excellent for development and prototyping

### 3. **Configuration Management**

The application uses `appsettings.json` with Microsoft.Extensions.Configuration.

**Benefits:**
- Externalized configuration separate from code
- Environment-specific settings can be easily swapped
- Connection strings are not hardcoded
- Follows .NET best practices and conventions

### 4. **User Interface: Spectre.Console**

Rich console formatting is implemented using Spectre.Console.

**Advantages:**
- Enhanced user experience with colors, prompts, and structured menus
- Professional-looking console output
- Cross-platform compatibility
- Improved readability and user engagement

### 5. **Data Model: CodingSession**

The `CodingSession` class represents the core domain entity.

**Properties:**
- `Id`: Unique identifier for each session
- `StartTime`: When the coding session began
- `EndTime`: When the coding session ended
- `Duration`: Calculated session length (formatted string)

**Design Decision:**
- Simple, focused model with only essential properties
- Duration stored as string for display purposes
- Follows POCO (Plain Old CLR Object) pattern

### 6. **Helper Utilities**

Separate utility classes handle cross-cutting concerns:

- **Validation.cs**: Input validation and business logic constraints
- **UserInput.cs**: User input collection and parsing

**Rationale:**
- Keeps controller logic clean and focused
- Reusable validation logic across the application
- Single responsibility principle

### 7. **Initialization Strategy**

Database initialization happens during startup (`db.Initialize()`).

**Purpose:**
- Creates tables if they don't exist
- Ensures data consistency
- Establishes the database schema

## Project Structure

```
CodingTracker.urasylmaz1/
├── Program.cs                 # Application entry point
├── appsettings.json          # Configuration file
├── CodingTracker.urasylmaz1.csproj  # Project file
├── Models/
│   └── CodingSession.cs      # Domain entity
├── Controllers/
│   └── CodingController.cs   # Application logic & UI
├── Data/
│   └── Database.cs           # Data access layer
└── Helpers/
    ├── Validation.cs         # Input validation
    └── UserInput.cs          # User input handling
```

## Technology Stack

| Technology | Purpose | Version |
|-----------|---------|---------|
| .NET | Runtime Framework | 10.0 |
| Dapper | ORM & Data Access | 2.1.72 |
| SQLite | Database | Latest |
| Spectre.Console | Rich Console UI | 0.55.2 |
| Microsoft.Extensions.Configuration | Configuration Management | 10.0.7 |

## Key Design Principles Applied

### 1. **SOLID Principles**

- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Dependency Inversion**: Depends on abstractions where possible

### 2. **DRY (Don't Repeat Yourself)**

- Common functionality extracted into helper classes
- Validation logic centralized
- Connection string management unified

### 3. **KISS (Keep It Simple, Stupid)**

- Straightforward class structures without unnecessary complexity
- Clear naming conventions
- Minimal external dependencies

### 4. **Maintainability**

- Clear separation between layers
- Easy to locate and modify functionality
- Consistent coding standards

## Features

- **Add Session**: Record a new coding session with start and end times
- **View Sessions**: Display all recorded coding sessions
- **Delete Session**: Remove a specific coding session
- **Persistent Storage**: All data is saved to SQLite database
- **Duration Calculation**: Automatic calculation of session duration

## Getting Started

### Prerequisites

- .NET 10.0 SDK or later
- Windows, Linux, or macOS

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running the Application

```bash
dotnet run
```

## Future Enhancement Considerations

- Add filtering and sorting capabilities
- Generate statistics and reports on coding habits
- Implement unit tests for validation logic
- Add persistent user sessions
- Create a web API wrapper for the console application
- Implement data export functionality (CSV, JSON)

## Conclusion

CodingTracker demonstrates clean architecture principles combined with practical .NET development patterns with the guidance of The C# Academy. The layered structure, clear separation of concerns, and use of proven libraries create a maintainable, scalable foundation for a session tracking application.
