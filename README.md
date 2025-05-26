# 🖥️ Coding Tracker

Coding Tracker is a .NET 9 console application designed to help users log, analyze, and manage their coding sessions. It provides a clean interface to track when you start and end coding, calculates session durations, and offers rich analysis features to understand your habits.

Built using guidance from The C# Academy project task: https://thecsharpacademy.com/project/13/coding-tracker

---

## ✨ Key Features

- **Session Logging:** Start, stop, and record coding sessions with precise start and end times.

- **Session Management:** View, edit, and delete past coding records.

- **Data Analysis:** 
  - Filter sessions by custom date ranges  
  - View total and average coding time per session and per day  
  - Check if your daily coding goals are met for a selected period

- **User-Friendly Console UI:** Uses **Spectre.Console** for rich, interactive prompts and stylish output.

- **Validation:** Ensures all user input (dates, times, numbers) is correctly formatted and within valid ranges.

---

## ✅ Requirements & Challenges

- [x] Use Spectre.Console to print tables and collect input  
- [x] Separate logic into multiple classes and files (e.g., UserInput.cs, Validation.cs, CodingController.cs)  
- [x] Strict input formatting: only accept specified date/time formats  
- [x] Use a configuration file to store the DB path and connection strings  
- [x] Create a CodingSession class containing:
  - Id  
  - StartTime  
  - EndTime  
  - Duration  
- [x] Duration is calculated from Start and End time (not manually input)  
- [x] Manual time entry supported (Start/End)  
- [x] Use Dapper ORM instead of ADO.NET for data access (as of Feb 2024)  
- [x] Read full records from DB into a List<CodingSession> (not anonymous types)  
- [x] Follow the DRY Principle – avoid code repetition  
- [x] Support stopwatch-based live session tracking  
- [x] Let users filter records by week, day, month, or year with optional sort direction  
- [x] Create reports showing total and average coding time  
- [x] Allow setting coding goals and show progress toward achieving them (via SQL or C#)

---

## 📁 Project Structure

- Program.cs  
- Models/  
&nbsp;&nbsp;&nbsp;&nbsp;└── CodingRecord.cs  
-  Data/  
&nbsp;&nbsp;&nbsp;&nbsp;└── DataAccess.cs   
- Helpers/  
&nbsp;&nbsp;&nbsp;&nbsp;├── Extras.cs  
&nbsp;&nbsp;&nbsp;&nbsp;├── Sorting.cs  
&nbsp;&nbsp;&nbsp;&nbsp;└── Validation.cs  
-  Services/  
&nbsp;&nbsp;&nbsp;&nbsp;└── StopwatchService.cs  
-  UI/  
&nbsp;&nbsp;&nbsp;&nbsp;├── AnalyseOperations.cs  
&nbsp;&nbsp;&nbsp;&nbsp;├── Enums.cs   
&nbsp;&nbsp;&nbsp;&nbsp;├── RecordOperations.cs  
&nbsp;&nbsp;&nbsp;&nbsp;└── UserInterface.cs  

### Explanation:  
- **Program.cs:** Entry point of the application.
- **Models/:** Contains data models (e.g., CodingRecord).
- **Data/:** Handles data access and storage logic using Dapper.
- **Helpers/:** Utility classes for validation, sorting, and extra functions.
- **Services/:** Application services (e.g., stopwatch logic).
- **UI/:** User interface logic, enums, and operations.

---

## ⚙️ How It Works

1. **Main Menu Navigation**  
   When you launch the application, you are presented with a menu to start a new session, view or manage records, analyze your coding habits, or exit. All navigation is handled via keyboard input in the console.

2. **Starting a Coding Session**  
   - Choose to start a new session from the menu.
   - You can either use the built-in stopwatch to track your session in real time, or manually enter start and end times.
   - Input is validated for correct date and time formats.

3. **Ending a Session**  
   - If using the stopwatch, stop it when you finish coding; the app records the end time and calculates the duration automatically.
   - For manual entry, provide both start and end times. The app ensures the end time is after the start time and computes the session duration.

4. **Session Management**  
   - View all your coding records in a formatted table.
   - Edit or delete any session by selecting it from the list.
   - All changes are saved to the database using Dapper ORM.

5. **Data Analysis**  
   - Filter sessions by custom date ranges, week, month, or year.
   - Choose sort direction (ascending/descending) for your results.
   - View total and average coding time per session and per day.
   - Generate reports to see your coding activity over time.

6. **Goal Tracking**  
   - Set daily coding goals (in hours and minutes).
   - Analyze your progress: the app calculates your average coding time per day in a selected period and shows if you met or exceeded your goal.

7. **Validation and Error Handling**  
   - All user input is validated for format and logical correctness (e.g., dates, times, numbers).
   - The app provides clear error messages and prompts for re-entry if invalid data is detected.

8. **Rich Console UI**  
   - Uses Spectre.Console to display tables, colored text, and interactive prompts for a modern console experience.


