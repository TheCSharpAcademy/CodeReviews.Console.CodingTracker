# CodingTracker

    The coding tracker challenge found on C# Academy (https://thecsharpacademy.com/project/13).
    Allows user to track coding sessions, storing the sessions in a database.

## Operation

### Main Menu

    Shows the main menu screen and allows you to choose options.
    Has options; View, Add, Update, Delete, Close.
    Navigate the menu using the arrow keys, press Enter to confirm selection.
    On any screen that takes user input, type 0 to return to menu.

### View

    Shows all stored sessions.
    Press Enter when ready to leave the view screen.

### Add

    Lets you add a new session to the database.
    1.  Input the date of the session you want to add.
	    Needs to be in format: dd-mm-yy. (ex. 01-02-21 for 1st Feb 2021)

    2.  Input the starting time in 24hour time with the format: hhmm. (ex. 1030 for ten thirty am)
	    Input ending time in the same way as starting time.
	    The maximum time a session can be is 23:59 (but you probably shouldn't code for that long anyway :) )
	    Can go over to the next day, but the date will be the date when you started the session.

    3.  You will then be shown the details of the session you want to add, and be prompted to keep or discard.
	    Press enter/Type y to confirm the details and add that session to the database.
	    Type n to discard the session and return to menu.

### Update

    Lets you update an existing sessions details.
    Shows all stored sessions.
    1.  Input the Id# of the session you want to update.

    2.  Follows the same steps for Adding a session.

### Delete

    Lets you delete an existing session from the database.
    1.  Input the Id# of the session you want to update.

    3.  You will then be be prompted to confirm.
		Press enter/Type y to confirm and delete that session from the database.
		Type n to discard the selection and return to menu.

### Thanks! -willN123