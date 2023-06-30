
CodingTracker

This Coding Tracker app is a small console application I made in my journey to learn C#.
This excercise is part of www.thecsharpacademy.com where you can learn how to code in C# and .NET step by step.

The goal of the app is to let the user give a starttime and a endtime. The app then calculates the time between the two timings and stores all the data into a sqlite database.

We had to fullfill certain requirements while making this app:

- To display the data we had to use the ConsoleTableExt NuGen Package.
- Use a configuration file to connect to the database.
- Use a Model CodingSession to store the data in a certain way.
- Use diffrent pages for diffrent actions (UserInput, Crud commands, Validation).
- Store the data in a sqlite database.
- Start and enddate should be inputted by the user.
- The duration between the start and endtime could not be entered manualy but has to been calculated by the app.
- Implement the proper validation so there would be no bad data.
  
Challenges:
- Work with NuGen packages trough the ConsoleTableExt.
- Use the basic CRUD commands in a sqlite database.
- Learn to work with DateTime and TimeSpan values.
- Use App.config file to connect to the database.

Extra's:
- Implemented a stopwatch function that create start and endtime in realtime.
- Created a report function that shows the total and average time.
- Allow the user to set a weekly goal and show progress towards it.

My findings:

This excercise builds further on the previous one, the habittracker, but add some new things like putting the connnection to the database in an App.config file. 
It was interesting to learn how to work with a thirdparty NuGenPackage, we had to find out how it worked by reading the documentation provided on the GitHubpage.
Working with DateTime and TimeSpan values was also very interesting and took some research. The difficult part was in the fact that a Sqlite db doesn't have a DateTime field so the DateTime values needed to be converted to a text format.
