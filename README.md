# Code Tracker
### Author: Corey Jordan - May 14, 2023
Code Tracker is a simple console application that allows you to track the amount of time you spend coding. It records the start and end times of your coding sessions, and calculates the total time coded.

## Requirements

- Must use ConsoleTableExt Library
- Must use seperate classes for seperation of concerns
- Must specify date format to user and validate
- Must create configuration file to connect to database
- Must conatin CodingSession class
- User cannot input session duration. Must be calculated
- User should be able to input start and end times
- Cannot use anonymous objects, must use a list of CodingSession

## Challenges
- Add the ability to track a live session.
- Let users filters their records
- Create a report
- Let the user set goals

## Thoughts

1. I sometimes wonder if I've over done the modularization. Other times I wonder if I could have created a few more classes. Finding the right balance is challenging.
2. One thing sorely missing from my app is escapes at several points. Once the user commits to certain choices, they have to follow through. 
3. Another possible improvement would be to utilize multiple goals by adding specificity to coding sessions, allowing different sessions to count against specific goals.
4. I don't like that my app displays the DateTime.MinValue to represent a Null db entry in an open session. Given more time, I might explore ways to format to hide/reveal data in a way that makes more sense.
