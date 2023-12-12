# Coding Tracker
This application allows its user to register their coding sessions to keep track of their coding habbits. The user needs to enter the starting and ending times of the coding session.

## Usage

When the application is open, the user is presented with the following options:
#### 1 Insert a new coding session

The user is able to log new sessions with the following parameters:

**Date**: The format used is yyyy/MM/dd. The program validates if the user entered a valid date.
**Time**: The format used is HH:mm. The program validates if the ending time is higher than the starting time.

#### 2 Modify a previous session

The user need to input the coding session ID to be modified. The program allows the user to filter the coding sessions by date.

#### 3 Delete a previous session

The user need to input the coding session ID to be deleted. The program allows the user to filter the coding sessions by date.

#### 4 Display records
#### 5 Start a new coding session

The user is able to start a current coding session. The program will automatically start a stopwatch to track the session lenght.

#### 6 Display coding session reports

The user is able to diplay the total lenght and average lenght of the session within the specified dates.

#### 7 Set a coding goal

The user is able to set a coding goal within the sepecified date. It's possible to check the average coding time of each day to achieve the goal.

#### 0 Exit the application


## To be done

1. Migrate session lenght format used in the database. Currently it uses d.HH:mm to store the session lenght. But it's not useful to do operations on the database.
2. Improve usage of the coding goals. Currently they are implemented as objects and they are deleted on closing the program.

