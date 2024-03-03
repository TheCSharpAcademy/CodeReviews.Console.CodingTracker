# HabitTracker
Console based CRUD application to track water intake
Developed using C# and SQLite

# Given Requirements:
- [x] When application starts, it creates a SQLite database if one is not present
- [x] Creates a database where coding sessions are logged
- [x] Shows a menu of options
- [x] Allows user to create an Auto Session, Manual Session, Get all Session History, Get Session History with a filter
- [x] Handles all errors so application doesn't crash
- [x] Only exits when user selects Exit

# Features

* SQLite database connection
		
- The program uses a SQLite db conneciton to store and read information.
- If no database exists, or the correct table does not exist, they will be created when the program starts.

* A console based UI using Spectre Console where users can navigate with selecting with their keyboard


# DB functions

## Main Menu

- Users can Exit the application, create an Auto Session, Manual Session, Get all Session History, Get Session History with a filter.

	![image](https://github.com/Fennikko/Images/blob/main/Main%20Menu.png)

### Auto Session

- The start time is auto generated and ends when you select End Session.

	![image](https://github.com/Fennikko/Images/blob/main/Auto%20Session.png)

### Manual Session

- Users enter a start time and end time, you can also type 0 to return to the Main Menu.

	![image](https://github.com/Fennikko/Images/blob/main/Manual%20Session.png)

- Date and time are checked to make sure they are in the correct format, you can also type 0 to return to the Main Menu.

	![image](https://github.com/Fennikko/Images/blob/main/Manual%20Session%20Format.png)

### Session History

- This populates a table with all of your logged coding sessions, It also calculates your avarage session time.

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20No%20Filter.png)

### Session History By Filter

- This populates session history by days,weeks,months, and years. It will also calculate your average session time by this filter.

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter.png)

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter%202.png)

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter%203.png)