# Coding Tracker
<h4>Thought process: </h4>

1. I started by creating a class for the things i know i will need (CodingSession class, TimeController, UserInterface)
2. I havent used Spectre Console before and it seemed interesting so i just played around creating different stuff in order to get a feel for it, i make an interface that i was satisfied with and remembered that enums were mentioned several times in the OOP course, so i went ahead and created Enums.cs where all my menu options were. From what i understood they are stricter/safer versions of lists where i dont want it being modified or flexible, i want it to be just the way it is. 
3. Dapper, i didnt get what the whole point of this is at first, but once i started using it i realised how nice having it do the connection part for me is. I went ahead and coded the different methods i needed for my menu
4. The finish up touches where i tweaked some things, made a nicer output, made a table as an output, tried to make my code more readable
5. The Tips section mentioned user validation, but im not sure if im supposed to make it manually since i feel like SpectreConsole already does a good job there (you cant input non-existant stuff, so the only thing i could maybe make is to check if the user is inputting dates that are after DateTime.Now), since i dont have manual user validation i didnt feel like i needed unit tests
6. Not sure if i should create a seeding function just like i did in the previous project, as it wasnt mentioned anywhere in the project description

<h4>While creating this project i have learned: </h4>

1. Spectre Console 
    Spectre is my favorite thing ever, learning how to use it has been so rewarding, i went back to my previous project and the difference it makes is huge! Not only does it look much better and save time (by not having to hard-code everything) but its also amazing for user input verification
2. Dapper - really nice, makes the code feel smoother, where i no longer have to deal with all that connection stuff that looks ugly and takes space. Much easier to use SQL with it as well
3. Classes - In my previous project i dumped everything into Program.cs and going back to it i can definetly tell why its recommended to create a separate class when needed
4. Stopwatch - it was interesting learning about stopwatch, and learning about '\r' which allows the stopwatch to stay in the same spot instead of printing out new stuff
