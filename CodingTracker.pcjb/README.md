# Coding Tracker
A very simple Coding Tracking console app. Exercise by [The C# Academy](https://www.thecsharpacademy.com)

## Usage

### Configuration
The configuration file 'App.config' enables you to set the following:

* DatabaseFilename
  * Path and name of the SQLite database file
  * Default value: CodingTracker.db
* DateTimeFormat
  * Format used to enter the coding session start- and end-time
  * Default value: yyyy-MM-dd HH:mm
  * see [Table of format specifiers](https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#table-of-format-specifiers)

### Execute the App
```
CodingTracker.pcjb$ dotnet run
```
If the app executes for the first time, it creates an SQLite database file at the location  configured in 'App.config' (see above).
After the app has started, you will see the main menu:

### Main Menu
```
============ Coding Tracker ============
1 - New Coding Session
2 - List Coding Sessions
3 - Stopwatch
4 - Report 'Totals and Average'
0 - Exit
Enter one of the numbers above to select a menu option.
```
### Add a New Coding Session
Press (1) on the main menu screen and enter start- and end-time to add a new coding session:
```
============ Coding Tracker ============
New Coding Session
Date & Time Format: yyyy-MM-dd HH:mm
Start: 2023-09-12 07:30
End  : 2023-09-12 08:45
```
After pressing enter, you will see the main menu screen again with a success message:
```
============ Coding Tracker ============
OK - Session successfully saved.
----------------------------------------
1 - New Coding Session
2 - List Coding Sessions
3 - Stopwatch
4 - Report 'Totals and Average'
0 - Exit
Enter one of the numbers above to select a menu option.
```

### List Coding Sessions
Press (2) to view a list of all coding sessions stored in the database:
```
============ Coding Tracker ============
All Coding Sessions
-----------------------------------------------------------------
| Id | StartTime             | EndTime               | Duration |
-----------------------------------------------------------------
| 4  | 8/15/2022 10:30:00 PM | 8/16/2022 12:15:00 AM | 01:45:00 |
-----------------------------------------------------------------
| 3  | 7/30/2023 4:00:00 PM  | 7/30/2023 10:00:00 PM | 06:00:00 |
-----------------------------------------------------------------
| 2  | 9/4/2023 7:00:00 AM   | 9/4/2023 9:00:00 AM   | 02:00:00 |
-----------------------------------------------------------------
| 1  | 9/12/2023 7:30:00 AM  | 9/12/2023 8:45:00 AM  | 01:15:00 |
-----------------------------------------------------------------
Order: '+' ascending, '-' decending
Filter: 'x' last 7 days, 'y' last 4 weeks, 'z' last 12 months, 'n' no filter
Enter ID and press enter to edit/delete a session.
Press enter alone to return to main menu.
```

You can sort and filter the list with the commands described below the list. The screen sorts and filters based on the StartTime.

### Edit/Delete a Coding Session
Enter the ID of the coding session you would like to edit/delete on the coding session list screen and press enter. Afterwards you will see the details of the selected coding session:

```
============ Coding Tracker ============
Edit/Delete Coding Session
----------------------------------------------------------------
| Id | StartTime            | EndTime               | Duration |
----------------------------------------------------------------
| 3  | 7/30/2023 4:00:00 PM | 7/30/2023 10:00:00 PM | 06:00:00 |
----------------------------------------------------------------
Enter 'e' to edit or 'd' to delete this session and press enter.
Press enter alone to return to list.
```

##### Edit a Coding Session
You selected coding session #3 from the list and want to edit it.
Enter 'e' and press enter. You will see the screen headline change to 'Edit Coding Session' and a prompt asking for the new start and end times:
```
============ Coding Tracker ============
Edit Coding Session
----------------------------------------------------------------
| Id | StartTime            | EndTime               | Duration |
----------------------------------------------------------------
| 3  | 7/30/2023 4:00:00 PM | 7/30/2023 10:00:00 PM | 06:00:00 |
----------------------------------------------------------------
Date & Time Format: yyyy-MM-dd HH:mm
New Start: 2023-07-30 05:30
New End  : 2023-07-30 09:45
```
After entering the new start/end-time and pressing enter, you will see the coding session detail screen again with a success-message and the modified coding session details:
```
============ Coding Tracker ============
OK - Session successfully saved.
----------------------------------------
Edit/Delete Coding Session
---------------------------------------------------------------
| Id | StartTime            | EndTime              | Duration |
---------------------------------------------------------------
| 3  | 7/30/2023 5:30:00 AM | 7/30/2023 9:45:00 AM | 04:15:00 |
---------------------------------------------------------------
Enter 'e' to edit or 'd' to delete this session and press enter.
Press enter alone to return to list.
```

#### Delete a Coding Session
You selected coding session #4 from the list and want to delete it:
```
============ Coding Tracker ============
Edit/Delete Coding Session
-----------------------------------------------------------------
| Id | StartTime             | EndTime               | Duration |
-----------------------------------------------------------------
| 4  | 8/15/2022 10:30:00 PM | 8/16/2022 12:15:00 AM | 01:45:00 |
-----------------------------------------------------------------
Enter 'e' to edit or 'd' to delete this session and press enter.
Press enter alone to return to list.
```
Enter 'd' and press enter. You will come back to the confirmation message list screen and see a confirmation-message. The deleted coding session is no longer in the list:
```
============ Coding Tracker ============
OK - Session #4 deleted.
----------------------------------------
All Coding Sessions
---------------------------------------------------------------
| Id | StartTime            | EndTime              | Duration |
---------------------------------------------------------------
| 3  | 7/30/2023 5:30:00 AM | 7/30/2023 9:45:00 AM | 04:15:00 |
---------------------------------------------------------------
| 2  | 9/4/2023 7:00:00 AM  | 9/4/2023 9:00:00 AM  | 02:00:00 |
---------------------------------------------------------------
| 1  | 9/12/2023 7:30:00 AM | 9/12/2023 8:45:00 AM | 01:15:00 |
---------------------------------------------------------------
Order: '+' ascending, '-' decending
Filter: 'x' last 7 days, 'y' last 4 weeks, 'z' last 12 months, 'n' no filter
Enter ID and press enter to edit/delete a session.
Press enter alone to return to main menu.
```

### Stopwatch
With the stopwatch you can directly track your time while coding without manually entering  start and end-times.

Press (3) on the main menu screen to start/view/stop the stopwatch:
```
============ Coding Tracker ============
Stopwatch not started yet.
Enter 's' and press enter to start the stopwatch.
Press enter alone to return to main menu.
```
Enter 's' and press enter to start the stopwatch:
```
============ Coding Tracker ============
Stopwatch is running.
Started at: 9/12/2023 9:15:00 PM.
Enter 's' and press enter to stop the stopwatch and save the session.
Press enter alone to return to main menu.
```
Press enter to go back to the main menu if you want. You can use all other parts of the app while the stopwatch is running and come back to the stopwatch screen later again.

Enter 's' and press enter to stop the running stopwatch:
```
============ Coding Tracker ============
OK - Session successfully saved.
----------------------------------------
Stopwatch not started yet.
Enter 's' and press enter to start the stopwatch.
Press enter alone to return to main menu.
```

Go back to the main menu and press (2) to see the new coding session created by the stopwatch in the list (#4 in this example screen):
```
============ Coding Tracker ============
All Coding Sessions
---------------------------------------------------------------
| Id | StartTime            | EndTime              | Duration |
---------------------------------------------------------------
| 3  | 7/30/2023 5:30:00 AM | 7/30/2023 9:45:00 AM | 04:15:00 |
---------------------------------------------------------------
| 2  | 9/4/2023 7:00:00 AM  | 9/4/2023 9:00:00 AM  | 02:00:00 |
---------------------------------------------------------------
| 1  | 9/12/2023 7:30:00 AM | 9/12/2023 8:45:00 AM | 01:15:00 |
---------------------------------------------------------------
| 4  | 9/12/2023 9:15:00 PM | 9/12/2023 9:18:00 PM | 00:03:00 |
---------------------------------------------------------------
Order: '+' ascending, '-' decending
Filter: 'x' last 7 days, 'y' last 4 weeks, 'z' last 12 months, 'n' no filter
Enter ID and press enter to edit/delete a session.
Press enter alone to return to main menu.
```

### Report 'Totals and Average'
The report initially shows yearly statistics:
```
============ Coding Tracker ============
Report 'Total and Average'
--------------------------------
| Period | Total    | Average  |
--------------------------------
| 2022   | 09:45:00 | 04:52:30 |
--------------------------------
| 2023   | 21:09:00 | 01:37:37 |
--------------------------------
Period: 'w' week, 'm' month, 'y' year
Press enter alone to return to main menu.
```
Enter 'm' and press enter to switch to monthly statistics:
```
============ Coding Tracker ============
Report 'Total and Average'
---------------------------------
| Period  | Total    | Average  |
---------------------------------
| 2022-12 | 09:45:00 | 04:52:30 |
---------------------------------
| 2023-07 | 11:21:00 | 01:53:30 |
---------------------------------
| 2023-09 | 09:48:00 | 01:24:00 |
---------------------------------
Period: 'w' week, 'm' month, 'y' year
Press enter alone to return to main menu.
```
Enter 'w' and press enter to switch to weekly statistics:
```
============ Coding Tracker ============
Report 'Total and Average'
---------------------------------
| Period  | Total    | Average  |
---------------------------------
| 2022-50 | 09:45:00 | 04:52:30 |
---------------------------------
| 2023-28 | 03:50:00 | 01:55:00 |
---------------------------------
| 2023-29 | 02:10:00 | 01:05:00 |
---------------------------------
| 2023-30 | 05:21:00 | 02:40:30 |
---------------------------------
| 2023-35 | 03:20:00 | 01:40:00 |
---------------------------------
| 2023-36 | 04:25:00 | 02:12:30 |
---------------------------------
| 2023-37 | 02:03:00 | 00:41:00 |
---------------------------------
Period: 'w' week, 'm' month, 'y' year
Press enter alone to return to main menu.
```

Note: The example reports above were generated from this coding session list:
```
All Coding Sessions
------------------------------------------------------------------
| Id | StartTime              | EndTime               | Duration |
------------------------------------------------------------------
| 6  | 12/15/2022 8:00:00 AM  | 12/15/2022 2:45:00 PM | 06:45:00 |
------------------------------------------------------------------
| 5  | 12/15/2022 10:00:00 AM | 12/15/2022 1:00:00 PM | 03:00:00 |
------------------------------------------------------------------
| 7  | 7/10/2023 7:00:00 PM   | 7/10/2023 9:15:00 PM  | 02:15:00 |
------------------------------------------------------------------
| 12 | 7/11/2023 5:45:00 PM   | 7/11/2023 7:20:00 PM  | 01:35:00 |
------------------------------------------------------------------
| 14 | 7/19/2023 7:15:00 AM   | 7/19/2023 8:25:00 AM  | 01:10:00 |
------------------------------------------------------------------
| 13 | 7/19/2023 10:00:00 PM  | 7/19/2023 11:00:00 PM | 01:00:00 |
------------------------------------------------------------------
| 3  | 7/30/2023 5:30:00 AM   | 7/30/2023 9:45:00 AM  | 04:15:00 |
------------------------------------------------------------------
| 15 | 7/30/2023 5:45:00 AM   | 7/30/2023 6:51:00 AM  | 01:06:00 |
------------------------------------------------------------------
| 8  | 9/1/2023 8:30:00 AM    | 9/1/2023 9:45:00 AM   | 01:15:00 |
------------------------------------------------------------------
| 11 | 9/2/2023 7:50:00 PM    | 9/2/2023 9:55:00 PM   | 02:05:00 |
------------------------------------------------------------------
| 2  | 9/4/2023 7:00:00 AM    | 9/4/2023 9:00:00 AM   | 02:00:00 |
------------------------------------------------------------------
| 10 | 9/4/2023 8:45:00 PM    | 9/4/2023 11:10:00 PM  | 02:25:00 |
------------------------------------------------------------------
| 9  | 9/11/2023 1:00:00 PM   | 9/11/2023 1:45:00 PM  | 00:45:00 |
------------------------------------------------------------------
| 1  | 9/12/2023 7:30:00 AM   | 9/12/2023 8:45:00 AM  | 01:15:00 |
------------------------------------------------------------------
| 4  | 9/12/2023 9:15:00 PM   | 9/12/2023 9:18:00 PM  | 00:03:00 |
------------------------------------------------------------------
```