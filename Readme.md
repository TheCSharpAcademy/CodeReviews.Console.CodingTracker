# Console Coding Tracker
Console-based CRUD application to track time spent coding. Developed using C#, SQLite, Dapper, and Spectre.Console.

## Given Requirements
- Show the data in the console using the Spectre.Console library.
- Have separate classes in different files (i.e., UserInput.cs, Validation.cs, CodingController.cs).
- Tell the user the specific date and time format to use and not allow any other format.
- Create a configuration file called appsettings.json, which will contain the database path, connection strings, and any other configurations you might need.
- Create a CodingSession class in a separate file with the following properties: Id, StartTime, EndTime, and Duration.
- The user shouldn't input the duration of the session. It should be calculated based on the start and end times.
- The user should be able to input the start and end times manually.
- Use the Dapper ORM for data access instead of ADO.NET.

## Features
- Users can create a real-time coding session and track its duration.
- Users can add, edit, or remove a previous session.
- Users can view sessions using a basic search (time period of session activity).
- Users can autofill their activity using the latest entry in the database.
- Date and time inputs are checked to ensure they are in the correct and realistic format.

## Challenges
I struggled a lot with the Elapsed.Time and DateTime paradigms. As I wanted to let the user input dates in a user-friendly way, I had to do a lot more conversions than I expected.
As I developed the application, I encountered issues where the information I extracted was in a different format from the one previously built for conversion, and I ended up having a lot of helper methods for specific cases.
This made naming and code readability challenging.

I find it hard to remember the solutions I used in other projects. When facing the same issues again, I feel like I either reinvent the wheel a second time or check what I did before and get it done.
It's hard to find the balance between learning by heart and being pragmatic and efficient. I don't think it makes a lot of sense to learn every coding scenario or library.

## Lessons Learned
- I'm getting more at ease with how DateTime and Elapsed.Time work.
- I should indeed plan the project beforehand. I started with what was challenging and motivating me and ended up working in a less efficient way. Knowing which features I want and writing user stories will help me stay focused.
- I tend to add too many features or make design decisions on the go.
- Having a side project to test features quickly.
- Getting used to do micro-commit. Till here I only used Github to send my work once done. Now I am trying to get used a habbit of commiting new progress increment.

## Areas to improve
- I really need to learn how delegates and generics work. I always struggle to understand code that uses them, and they seem really practical.
- Error handling seems to be a vast topic that I will have to dig into.

## Resources Used
- LearnDapper.com
- My previous habit tracker project
- Spectre documentation
- Various Stack Overflow forum posts
Copilot to get some syntax quickly and explain how it works.
