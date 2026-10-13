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
As I developed the application, I had to do a lot of helpers conversion methods. Way more that I expected. Sometimes I had the hour without the date, sometime the hour and the date etc. It felt complicated. This made naming and code readability challenging.
I made early the poor decision of letting the Database class be responsible for adding data in the correct format (Database.ConvertAndAdd()). I'm not redoing that again in the future. 
Otherwise I found it hard to remember some really specific solution I used in previous projects. When facing the same need again, I tend to directly reread what I did. 
It's hard to find the balance between learning by heart and being pragmatic and efficient. I don't think it makes a lot of sense to learn every coding scenario or library.

## Lessons Learned
- I think I have a good basic understanding of DateTime and Elapsed.Time now.
- I should plan the project beforehand. I started with what was challenging (the stopwatch) and ended up working in a chaotic way. Knowing which features I want and writing user stories will help me stay focused.
- Having a side project to test features quickly would be helpful.
- Getting used to do micro-commit. Till here I only used Github to send my work once done. Now I am trying to get used a habbit of commiting new progress increment.

## Areas to improve
- I really need to learn how delegates, generics and lambda work. I always struggle to understand code that uses them, and they seem really practical.
- the process of making an appsettings.json is still a bit mysterious to me
- Error handling is a vast topic that I will have to dig into.

## Resources Used
- LearnDapper.com
- My previous habit tracker project
- Spectre documentation
- Various Stack Overflow and Medium posts
- Copilot to get some syntax quickly and explain how it works, as well as for review when I know I'm going the wrong way
