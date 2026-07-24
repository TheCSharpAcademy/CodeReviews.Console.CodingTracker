# Coding Tracker

A console-based CRUD application to track coding sessions, written in C# and powered by SQLite.

## How It Works

The application connects to a local SQLite database using Dapper. Upon the very first execution, it automatically creates the coding_sessions table.
The user can then add, view, update and delete records via the console.

## Challenges Completed

1. **Stopwatch**: Added the functionality of tracking the coding time via a stopwatch so the user can track the session as it happens.

### What Was Easy?

A lot of the concepts were carried on from the Habit Tracker, although they had to be separated instead of it all being in one big Program.cs file.

### What Was Hard

This was the first project without following a video tutorial, so I had find things out by searching online and reading through documentation. There were also quite a few new concepts.

### What I Learned

I got a better grasp of separation of concerns and how code in different files can interact with each other.

### Extra Notes

I probably got a bit acrried away with some things. For example, I preferred to use Enums for the menu options instead of hardcoded strings, this introduced a new issue of having the menu options look good, as Enums can't have spaces. I managed to find a way to use the Display Attribute instead. Also, I tried to make the application what I think is a bit more user friendly by giving the user the option to Escape to the main menu after selecting a menu option, in case they selected the wrong option.
