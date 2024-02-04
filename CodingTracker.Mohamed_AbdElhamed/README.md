

# Coding Tracker - Console App

## Overview

Coding Tracker is C# Console App  that helps developers to track their coding activities. It provides a simple and user-friendly interface.
Developers can insert their activites manually or just start stopwatch while they coding and App can record their time automatically.

## install
You can clone this repo easly using `git clone` command and run it via dotnet cli or vs.

# Documentation
### When user start program the main menu will apear and then he will have 6 options user can choose one them by it's number :
#### + 1 - Show all records
This will retrieve all records stored in database ordered by it's Ids

#### + 2 - Insert new record
Insert new record by adding the date and start time of coding and end time

#### + 3 - Update record
when choose number '3' first it will retrieve all records to choose which record user want to update.
Whem user enter id first program check if the record exists then ask user to enter the information of code session(date , startAt , endAt)

#### + 4 - Delete record
when choose number '4' first it will retrieve all records to choose which record user want to delete.

#### + 5 - Stop Watch
when user enter 5 the stopwatch will start counting.
The counting will be prcesses until user press any key then it end and store this record in database

#### + 0 - exit
Exit from application


