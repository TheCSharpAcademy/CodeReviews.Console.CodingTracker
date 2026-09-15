# Coding Tracker
Simple coding tracker build with C#, SQLite, and Spectre.Console!

## Features
- SQLite database
- Nice-looking console interface with Spectre.Console
- Record coding sessions by custom time
- Record coding sessions live with a stopwatch

## Running the App

1. Clone the repo
2. Make sure they required libraries are downloaded
```
dotnet add package Dapper
dotnet add package Microsoft.Data.Sqlite
dotnet add package Microsoft.Extensions.Configuration 
dotnet add package Microsoft.Extensions.Configuration.Json
```
3. Run the app
```
dotnet run
```

## Architectural Choices

- Classes were organized in 3 directories:
  -   `Models`: For database entities
  -   `Data`: For data access
      - Tried to make 1 unified data access class with all the CRUD methods.
  -   `Helpers` For general helpers
 
## Experience

Definitely learned a lot about how to read the docs for an external library, and how it integrates with the base language in apps.

One of the challenges I faced was how to use Dapper with dates considering how SQLite does not have a DATETIME data type, which makes it always stored as TEXT.

I had to write an extension method of sorts to it, but i still need to revisit that.
