[
    "# Head",
    "## Item",
    "### Detail"
]

# ConsoleCodingTracker

## Introduction

Console based CRUD application to track time spent coding and setting up coding
 goals.
Developed using C# and SQLite.

## Given Requirements

* To show the data on the console, you should use the "Spectre.Console" library.
* You're required to have separate classes in different files (ex. UserInput.cs,
 Validation.cs, CodingController.cs)
* You should tell the user the specific format you want the date and time
to be logged and not allow any other format.
* You'll need to create a configuration file that you'll contain your database
path and connection strings.
* You'll need to create a "CodingSession" class in a separate file. It will
contain the properties of your coding session: Id, StartTime, EndTime, Duration
* The user shouldn't input the duration of the session. It should be calculated
based on the Start and End times, in a separate "CalculateDuration" method.
* The user should be able to input the start and end times manually.
* When reading from the database, you can't use an anonymous object,
you haveto read your table into a List of Coding Sessions.

## Given Challenges

* Add the possibility of tracking the coding time via a stopwatch,
so the user can track the session as it happens.
* Let the users filter their coding records per period (weeks, days, years)
and/or order ascending or descending.
* Create reports where the users can see their total and average coding
session per period.
* Create the ability to set coding goals and show how far the users are
from reaching their goal, along with how many hours a day they would
have to code to reach their goal. You can do it via SQL queries or with C#.

## Features

### SQLite database connection

* The program uses a SQLite db connection to store and read information.
* The program uses two tables, one for Coding sessions other one for Goals
* If no database exists, or the correct table does not exist
they will be created on program start.

### A console based UI where users can choose options by arrow keys

![image](https://github.com/czerviik/CodeReviews.Console.CodingTracker/assets/137193704/b7c90135-42b4-45ee-8f86-9dfd92646122>)

### Displaying tables using the Spectre library, scrolling tables functionality

![image](https://github.com/czerviik/CodeReviews.Console.CodingTracker/assets/137193704/1cf462a4-16a2-42f6-a811-c602da86fcbf>)
  
### Coding sessions functions

* Users can add a Coding session manually or by using a stopwatch
* When entering manual time and date, user can use a fastforward method
to automatically set the date as "today", or set the end date
of the session the same as the start date
* Manually entered times and dates are checked and ask for
correction if wrong
* When using stopwatch, user can pause it, if the break is longer than 10s,
it is not counted in the session's final duration
* User is asked for confirmation and a note (optional) after each session
* From the displayed table users can sort (asc, desc), update or delete entries
* User can filter coding sessions by years, months and weeks
* The Coding session table is scrollable when there are more than 8 entries

### Goals functions

* Users can set up multiple active goals
* When user adds, updates or deletes a coding session, the goal
(even when previously finished) updates it's progress accordingly
* Users can display goals in a scrollable table, which includes
a status bar of their progress in each goal

### Goals table

![image](https://github.com/czerviik/CodeReviews.Console.CodingTracker/assets/137193704/be58d5fc-e1a0-4973-8ebb-ed9ed00e00ff>)

## Challenges
* I tried to make a more solid menu handling than just number of loops
and checks so I implemented a MenuManager class that handles menu objects
in a stack. Calling that stack through the methods was found
an effective way of handling multiple menu screens.
As it was a new feature for me, it took some time to get used to it.
* One of the biggest challenge was my approach to make a "graphical" UI
inside a console app, I inplemented OptionsPicker class that handles
options with arrows, and other special keys. At first this approach
seemed to be solid but as the program grew, I have to redesign
the class multiple times so it works in all cases properly.
* Another challenge was to inplement a UserInput method that will take
key strokes and write them to the screen, but also take escape key
as a "back" function while typing.
* Another great challenge was to design a correctly working logic
that updates the goals progress and status when user changes or deletes
a past coding session. When I finally made it work it was a brutal spaghetti,
so I had to refactor several methods to actually make it understandable
* At first I struggled with implementing the correct table scrolling
behavior as the program was already in almost finalized state,
but it turned out to be working correctly at the end.
* The biggest issue was with resizing the console window. My UI is strongly
index based (depending on console size), so when the console window
is too small, or is resized, the UI starts to do weird things. I tried
to lock the window dimensions, or to check for dimensions change
and handle it, but I learned that this functionality isn't fully
working on UNIX based systems, so I left it as I am not aiming
for console applications in the future
* Here and there I learned some new syntax features and rules that
I previously didn't understand or use
* I also scratched a surface of multithreading, the Coding session clock
runs at separate thread so you can pause it, continue, etc.
* I again messed up with the repository, accidentally commited to "master"
instead of "main". At the project's finish it led to big confusion
and troubles. I had to delete folder, restructuralize, etc.
It was really bad.

## Lessons Learned

* It's a very good idea to do some basic architect work before start.
I made my in app.diagrams.net and it saved a lot of investigations
and aimless try-fail attempts.
* Next time I will not so try-hard with the UI, it's not that important
for me in console apps.
* The refactorization helps to read and understand the code even
for me as an author.
* Try-catch blocks very important as soon as possible.
* Next time I will try to use more external libraries than trying
to code most of the functions on my own.
* Should learn more about multithreading, it came out useful.
