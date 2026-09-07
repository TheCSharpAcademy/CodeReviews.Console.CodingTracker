**Project requirements:**



* Logging occurrence of a coding session 
* Users need to be bale to input the start date of the coding session
* Users need to be bale to input the end date of the coding session
* Duration of the sessions should be calculated from these values
* App should use a real database
* Users should be able to insert, delete, update and view their coding sessions
* All input errors should be handled
* Only Dapper should be used



**How the App works?**



When a user starts the app, he sees a menu with options:



\- View all Sessions

\- Add a Session

\- Update a Session

\- Delete a Session



&#x20;

Choosing "View all Sessions" will show him a table with all the sessions that are recorded in the database.



Choosing "Add a Session" will open another screen where he will be able to enter a start time in dd-mm-yy hh:mm format.

After entering it he will be able to enter a end time in dd-mm-yy hh:mm format for the session.



Choosing "Update a Session" will bring up a new screen where a user will be able to update an existing record.

You must use Id of the record that exists in the database.

After choosing an Id user will be able to enter new start time and time for the session.



Choosing "Delete a Session" bring up a new screen where a use will be able to delete the sessions that are already recorded.

You must use Id of the record that exists in the database.





**Thoughts on the project:**



This project made me use appsettings.json where I put the connection string to the Sqlite database. Dapper is easier to use than ADO.NET. For user interface I used spectre console, which has quite a few neat features. I really like the table feature. Also it's easier to get a list of options and a correct answer.

Hardest part was figuring out how to convert between a model and a database because Sqlite doesn't have a date data type. I was using strings in the model because I couldn't get it to work with DateTime type. But then I was wondering how to calculate the duration of the session. 

