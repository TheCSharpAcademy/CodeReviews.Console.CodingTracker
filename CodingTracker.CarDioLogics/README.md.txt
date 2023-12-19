# Coding Tracker

This is a C# application that was a made to pratice coding in C# and SQLite.
The app allows the user to register, update and delete coding sessions on a database. It also allows to start an on going session that when terminated registers the dates and duration of session automatically. The user can define goals and the total ammount of time (in minutes) needed to complete them and to get a report on the state of those goals that print the ammount the user has completed (in %) and how much time its needed to complete the goal.

#####Difficulties and Lessons taken

-I had some trouble setting up the config which seemed to not be working with a xml file. I had to choose a specific xml file format from the options available to get it to work (config file).

-Installing the wrong nugget packages. I had some trouble setting up the SQLite connection because of it by installing the "Microsoft.Data.SQLite.Core" package which apparently does not have the native SQL library. So I had to install the correct package (Microsoft.Data.SQLite).
The takeaway is to beware of the nugget packages that are needed and of the differences between them, especially when names are similar.

-Spent alot of time thinking about how to separate everything and trying to come up with a perfect solution or best solution possible. When probably should not do that, better to not take too long making decisions of which direction to go in and to come up with a solution that works and that contains clean and readable code.

-Still... the code is not very "clean" and organized.

-Spent some time reviewing concepts of OOP and other programing concepts (separation of concerns, architecture or basically anything that had to do with how to better organize or structure the code)

-Spent some time reviewing some aspects of C# syntax and got the hang of few new tricks (like the out keyword in methods.. among other things).

-Just spending time playing around with the code (although time consuming) has helped me learn new things and consolidate others. Which is good in this learning phase.


##### Areas to improve:

-Become EVEN more familiar with using SQL, SQLite and C#.
-Become better at debugging (update method bug where the record didn't update at all). And also testing the code.
-Become better at organizing the code. Still. More emphasis now!

##### Resourses used:

-Csharpacademy github and website
-Youtube videos
-W3Schools SQL course
-Google
-chatGPT
-books