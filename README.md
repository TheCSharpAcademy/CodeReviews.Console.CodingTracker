# CodingTracker Console Application

## Features
* The app logs your daily coding time. A session of only 9 hours at maximum is allowed.
* To display the data on the console, the "Spectre.Console" library is used.
* Separation of concerns is observed.
* A configuration file containing database path and connection strings is used.
* A "CodingSession" class is in a separate file. It contains the properties of your coding session: Id, StartTime, EndTime, and Duration.
* The user won't enter the duration of the session. It will be calculated based on the Start and End times.
* The user will be able to input the start and end times manually.
* Dapper ORM is used for data access instead of ADO.NET.
* When reading from the database, the table is read into a List of Coding Sessions.
* The app shows the user a list of options. All possible errors are handled so that the application never crashes.
  
  ![Screenshot](/images/BasicCrashHandling.JPG)
* The user can view all previous records, which are sorted based on the Date and startTime, EndTime.
  
  ![Screenshot](/images/ViewRecords.JPG)
* The user can insert new records, which are validated to not clash with previous records, if any.
  
  ![Screenshot](/images/InsertRecord.JPG)
* The user can delete previous records based on SessionId's.
  
  ![Screenshot](/images/DeleteRecord1.JPG)

  ![Screenshot](/images/DeleteRecord2.JPG)
  
* The user can update records based on the dates, which are validated to not clash with any previous records.
  
  ![Screenshot](/images/UpdateRecord1.JPG)

  ![Screenshot](/images/UpdateRecord2.JPG)
  
* There is a functionality to start live coding sessions. The app records time for the same successfully and even handles corner cases like date change between the session to enter 2 separate records in the database with different dates. If the session limit of 9 hours is exceeded, the session closes automatically and the record is saved.
  
  ![Screenshot](/images/LiveSession.JPG)
