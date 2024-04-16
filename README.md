# CodingTracker Console Applicaton

## Features
* The app logs your daily coding time.
* To show the data on the console, the "Spectre.Console" library is used.
* Separation of concerns is observed.
* A configuration file that contains database path and connection strings is used.
* A "CodingSession" class is in a separate file. It contains the properties of your coding session: Id, StartTime, EndTime, Duration
* The user won't enter the duration of the session. It will be calculated based on the Start and End times.
* The user will be able to input the start and end times manually.
* Dapper ORM is used for data access instead of ADO.NET.
* When reading from the database table is read in a List of Coding Sessions.
