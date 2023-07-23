# Console Habit Tracker

A console-based CRUD application to track coding hours.
Developed using C# and SQLite.

# Given Requirements:
- [x] Upon application start, it should create a SQLite database if one doesn't already exist.
- [x] The application should create a table in the database to store coding sessions.
- [x] Users should be able to insert, delete, update, and view their coding sessions.
- [x] All possible errors should be handled to prevent application crashes.
- [x] The application should only terminate when the user enters 0.
- [x] Raw SQL should be used for interacting with the database;

# Features

* SQLite database connection

	- The program establishes a connection to a SQLite database for storing and retrieving information.
	- If no database exists or the required table is missing, they will be created at program startup.

* Console-based UI with navigation using key presses
 
 	 ![image](https://github.com/alvaromosconi/CodeReviews.Console.HabitTracker/assets/77434507/c0dbeda7-30bf-42cd-a86f-f04beef9f6e3)

* CRUD database functions

	- Users can create, read, update, and delete habit entries from the main menu. Dates must be entered in the format dd-mm-yy.
	- Input validation ensures that time and dates are in the correct and realistic format.

* Basic reports of records grouped by habit name

	 ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/467f9020-900d-4c89-b580-49e3d9a8ad7b)

* New session

   ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/7d06e0f1-3e63-451d-b2dd-1ecd6202a828)

* View all past sessions
  
   ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/3165ccda-1421-4ddb-8224-8c2f3814f144)

* View sessions in a certain range

   ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/bdbeb0f3-b70b-4adc-81c9-023952e0f3f9)

   ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/0bdf8a72-4ead-4747-9533-26b1ae8c4802)

* Delete a session by id

   ![image](https://github.com/alvaromosconi/CodeReviews.Console.CodingTracker/assets/77434507/b9f78c82-695e-4044-a2fa-96e3c5059ce1)

  
# Challenges
	
- Handling users input and manage program flow effectively.
- Working with DateTime type and a external library.
- Separation of concerns

# Resources Used
- The CSharpAcademy guide.
- Various StackOverflow articles for C# syntax and resolving SQLite doubts.
- Microsoft Documentation
