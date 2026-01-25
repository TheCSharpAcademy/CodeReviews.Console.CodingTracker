# Coding Tracker Console App

A simple console-based coding session tracker built with **.NET**, **Dapper**, **Microsoft.Extensions.Configuration**, **Microsoft.Data.Sqlite**, and **Spectre.Console**. The app allows you to **view, add, edit, and delete** coding sessions in a lightweight SQLite database with a clean console interface.
I like this project because I feel like I had better grasp on OOP and patterns like DRY and KISS

---

## Features

- Add new coding sessions with start and end times  
- View all recorded sessions in a formatted console table  
- Edit existing sessions directly from the console  
- Delete sessions when needed  
- Uses **Spectre.Console** for a colorful and user-friendly terminal UI  
- Configuration handled via **Microsoft.Extensions.Configuration** for flexible database and app settings  

---

## Challenges

One thing that really frustrated me was how **Dapper maps values** from the database to C# objects.  

At first, I named my columns `start_time` and `end_time`. When I tried to render them, Dapper didn’t automatically convert them into `DateTime` even though the types were declared in `models/CodingSession.cs`. The issue was that Dapper maps columns to properties **by name**, and I was sticking to C#’s PascalCase naming convention instead of the database’s snake_case convention.  

The fix was simple: rename the columns to `StartTime` and `EndTime` to match the property names in my model. Once that was done, Dapper handled the mapping perfectly.  

---

## Lessons Learned

- **Dapper is incredibly convenient**. You don’t need to manually open or close the database connection, and retrieving rows into a list is simple using Dapper’s built-in methods  
- Keeping column names and model property names consistent is crucial for smooth mapping  
- **Spectre.Console** makes even simple console apps feel interactive and polished  

---

## Resources Used

- [Dapper Documentation](https://dapper-tutorial.net/)  
- [Microsoft.Data.Sqlite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/)  
- [Microsoft.Extensions.Configuration](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)  
- [Spectre.Console](https://spectreconsole.net/)  
