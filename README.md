# Code Tracking Console Application
## Overview
A console based application to track time spent coding. Created for the C# Academy project based learning

## Requirements
- Application where you can log coding sessions
- Tracked by time spent coding. Calculated based off user input
- Users are able to input date, start time, end time
- Application stores data from a database
- Creates a table in a database to store and retrieve data
- Console interface using Spectre.Console library
- OOP based project using multiple classes
- Uses Dapper ORM
- Follow Dry principle

### Technologies
- C#
- SQLite
- Dapper ORM
- Spectre.Console

## Features
Features user friendly interface allowing navigation through menu options.

Launching the application will display a simple menu with the follwoing options:

See Existing Records	->	Displays all records stored in local database
Create Coding Session	->	Allows creation of new records
Exit					->	Closes application

### Creating a new coding session
From the main menu users are able to choose "Create Coding Session", where they will be prompted to choose, the record name, date, start time, and end time.

Using this information, the application will calculate the duration based off of the start and end time then log it into the database.

### Edit/Removing existing coding sessions
From the main menu users are able to choose "See Existing Records", which will display all records in the database. From there users will be able to select a record of their choosing and edit or remove the record from the database

Choosing edit will allow the user to edit the following:
- Record Name
- Date
- Start Time
- End Time

Choosing to edit either start or end time will also update the record duration