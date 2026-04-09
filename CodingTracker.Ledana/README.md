# Coding Tracker

A console application built in C# to log coding sessions.  
Users can enter start and end times, and the app calculates the duration automatically.  
Data is stored in **SQL Server** using **Dapper** for lightweight data access.


---

## Features
- Add, update, delete, and view coding sessions
- Input start and end times → app calculates duration
- Data persistence with SQL Server via Dapper
- Console-based interface

---

## Projects in this Solution
- **CodingTracker.Ledana** → main application
- **CodingTrackerTests** → unit test project

---

## Unit Testing Challenge
I’ve started adding unit tests with **NUnit**.  
Currently, some methods are still **loosely coupled** and do too much work, which makes them harder to test.  
Improving separation of concerns and refactoring for testability is part of my ongoing learning process.  

1. Clone the repository:
   ```bash
   git clone https://github.com/Ledana/CodingTracker.git
