# CodingTracker
CRUD based console application allowing the user to track its session of code by date and session time.
Developed using C# .net and Sqlite.

# Features
## Sqlite database
- Sqlite db connection to store and read informations.
- Automaticaly create a database and a table at startup if they don't exist.

## Console UI
- Table UI make use of the [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt) library to get a clean table presentation.

## Insert
- Manually input the session beginning and end.
- Automatically calculates the session duration.
- Apply proper calculation for session over 2 days.

## Update/Delete
- Possibility to Update and Delete session from the database.

# Challenges
- Working with DateTime was a bit tricky some times.
- Implementing an external library was both a challenging and fun thing to do.

# Future
There are multiple functionnalities I would like to implement in this app, but since I am planning to recreate it as a desktop application using MAUI this will have to wait. 
Those functionnalities are:

- Add a built in clock so that the user can click to start a session and click when it finishes. And the session should be automatically inserted into the database with the date of the day.
- A report function, to be able to get the number of hours spent coding from particular time span.
- A way to display the session history by date and/or amount of time spent coding.
- A goal system, when the user can insert a number of hours for a choosen time span. And as the user inserts new session a goal bar would fill.

# Resources
- Microsoft documentation
- [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
- [The C# Academy](https://www.thecsharpacademy.com/) for general guidance
