# Coding Tracker

## Table of Contents
- [General Info](#general-info)
- [Technologies](#technologies)
- [Features](#features)
- [Examples](#examples)
- [Requirements](#requirements)
- [Challenges](#challenges)
- [Used Resources](#used-resources)

## General Info
Project made for @TheCSharpAcademy.  
It provides a command-line application for tracking your coding sessions, managing goals and generating reports.

## Technologies
- C#
- SQLite
- Dapper
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)

## Features
- SQLite database connection
	- Populate the database with initial sample data if it's empty
- User-friendly interface: Provides clear menus and prompts for interaction.
- Input validation: Ensure data entered by the user is valid.
- Set and Manage Goals:
	- Define personal coding goals.
	- Update or delete existing goals.
	- View a list of all goals.
	- Track progress towards achieving goals.
- Track Coding Sessions:
	- Start, stop, and record coding sessions with accurate duration measurement.
	- Delete specific sessions if needed.
	- Generate reports to analyze monthly and yearly coding activity.

## Examples
- Main Menu
- Yearly Report
- Coding Sessions
- Goals
- Goal Progress

## Requirements
- [x] To show the data on the console, you should use the "Spectre.Console" library.
- [x] You're required to have separate classes in different files (ex. UserInput.cs, Validation.cs, CodingController.cs)
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.
- [x] You'll need to create a configuration file that you'll contain your database path and connection strings.
- [x] You'll need to create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times, in a separate "CalculateDuration" method.
- [x] The user should be able to input the start and end times manually.
- [x] You need to use Dapper ORM for the data access instead of ADO.NET.
- [x] When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.

## Challenges
- [x] Add the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.
- [x] Create reports where the users can see their total and average coding session per period.
- [x] Create the ability to set coding goals and show how far the users are from reaching their goal.
- [ ] Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.

## Used Resources
- [Microsoft Docs for SQLite](https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli)
- Various StackOverflow articles
- [Spectre Console documentation](https://spectreconsole.net)
- [Learn Dapper](https://www.learndapper.com)