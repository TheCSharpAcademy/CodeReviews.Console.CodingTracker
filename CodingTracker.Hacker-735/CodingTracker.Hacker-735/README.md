# Coding Tracker
Console application for logging daily coding sessions, built in C# with Spectre.Console for visuals, Dapper for interfacing with SQLite, and SQLite for a database.

## Overview
This app lets you log your coding sessions by inputting a start and end time and then automatically calculating the length from the two inputted times. You can view, update, and delete the sessions using the main menu.

## Features
* Add a coding session (each session includes date, start time, end time, duration is calculated based of of these)
* View all logged sessions in a table
* Update existing session from table
* Delete session from table
* It automatically creates the SQL table
* Date and time input validations
* A nice looking spectre interface
* you can type "td" to automatically input the date (As this is very annoying to do manually)

## Thought Process
Now i will preface this with the fact that i had at some point lost most access to my computer for about a month or two so somethings are not fresh in my memory. (I think in future i will make the readme along side with the project to avoid this issue in general, it will also allow me to think things through better)

#### Getting the Times
I first began with making the Time input system, just so i could think things through since it would be (in my opinion) the most important part of the program since it will be processing the data going on the table. At first it was very simple using two massive while loops with TryParseExact to just manually get the start and end time.

Now 
* GetTimeInput() calls a helper AskForTime() two times one for start and one for end
* User is asked Enter start time (HH:mm, 24-hour format):
* It reads the string
* we use Validation.IsValidTime()  to see if it is a valid format and it will loop until it is
* Then we get two values parsedStartTime and parsedEndTime

* Then we get totalMinutes from the start and end time which we have turned into pure minutes for calculation
* I also added handling for overnight sessions "if (totalMinutes < 0) totalMinutes += 24 * 60;"
* This assumes the session crossed midnight rather than being an error — so `23:00 → 01:00` is treated as a 2-hour session.
* We then formate the result as "string finalTime = $"{totalMinutes / 60:D2}:{totalMinutes % 60:D2}";"
* GetTimeInput() returns a tuple of: (FinalTime, StartTime, EndTime)


#### Error Handling

All error handling is within the Validation class

IsValidTime was just a method to see if it parsed as a time and if so it would be output

IsValidDate was mostly the same.

I wanted to store most info as strings as it made it more simple to produce and format (In my opinion)

Safe Execute is the main thing here though as i designed it to work with most things in the program. whatever code block gets ran within action will try and if it goes wrong either catch (SqliteException ex) (Incase SQLite messes up) or catch (Exception ex) (incase anything else messes up) will catch it. Each method has it's own errorContext
## A few things i struggled on
Mostly dapper and SQL in general since i didn't really know how to use it until now basically (I think i understand it alot better now though)

access levels really had me confused on some parts

Finding the best way to display information in a effective manner (Need some advice on this if possible, but i will look into it anyway)

Deciding which aspect was important to  handle first (Separation of Concerns)