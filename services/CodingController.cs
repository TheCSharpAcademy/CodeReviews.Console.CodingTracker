﻿using CodingTracker.enums;
 using static CodingTracker.utils.Validation;
using static CodingTracker.utils.Utilities;

using CodingTracker.models;
using CodingTracker.utils;
using CodingTracker.views;
using Spectre.Console;

namespace CodingTracker.services;

/// <summary>
/// This class initializes the database and handles the user's input for CRUD operations. Methods are invoked with
/// reflection, based on the method name passed as custom attribute above corresponding enum entry.
/// </summary>
internal class CodingController(DatabaseService databaseService) 
{
    private readonly DatabaseService _databaseService = databaseService;

    internal void ViewRecords()
    {
        try
        {
            AnsiConsole.Write(PrepareRecords().summaryForRender);
        }
        catch (ArgumentNullException)
        {
            return;
        }
        
        ContinueMessage();
    }

    internal void AddRecord()
    {
        CodingSession session = new();
        UserInput userInput = new();
        DateTime[] dates;

        try
        {
            dates = userInput.GetDateInputs();
        }
        catch (ReturnBackException)
        {
            return;
        }
        
        session.StartTime = dates[0];
        session.EndTime = dates[1];
        
        _databaseService.UpdateData(DatabaseUpdateActions.Insert, session);
    }
    
    internal void DeleteRecord()
    {
        int id;
        var userInput = new UserInput();
        
        try
        {
            AnsiConsole.Write(PrepareRecords().summaryForRender);
            id = userInput.GetIdInput();
        }
        catch (ReturnBackException)
        {
            return;
        }
        catch (ArgumentNullException)
        {
            return;
        }
        
        if (!AnsiConsole.Confirm("Are you sure?"))
        {
            return;
        }

        var response = _databaseService.UpdateData(
            action:DatabaseUpdateActions.Delete, 
            recordId:id
            );
        
        var responseMessage = response < 1 ? "No record with that ID exists." : "Record deleted successfully.";
        
        AnsiConsole.WriteLine(responseMessage);
        
        ContinueMessage();
    }
    
    internal void UpdateRecord()
    {
        var userInput = new UserInput();
        DateTime[] dates;
        int id;
        CodingSession session;
        
        var sessions = _databaseService.RetrieveCodingSessions(null, null);
        
        if (sessions is null)
        {
            ContinueMessage();
            
            return;
        }
        
        try
        {
            PrepareRecords();
            
            id = userInput.GetIdInput();
            session = sessions.Single(x => x.Id == id);
            dates = userInput.GetDateInputs();
        }
        catch (ReturnBackException)
        {
            return;
        }
        catch (InvalidOperationException)
        {
            AnsiConsole.WriteLine("\nNo record with that ID exists.");
            ContinueMessage();

            return;
        }
        catch (ArgumentNullException)
        {
            return;
        }
        
        session.StartTime = dates[0];
        session.EndTime = dates[1];
        
        _databaseService.UpdateData(
            action:DatabaseUpdateActions.Update, 
            session:session
            );
        
        AnsiConsole.WriteLine("Record deleted successfully.");
        ContinueMessage();
    }
    
    internal (Table summaryForRender, Table summaryForSave, string formattedDuration) PrepareRecords()
    {
        var tableConstructor = new SummaryConstructor();
        var records = _databaseService.RetrieveCodingSessions(null, null);
        
        if (records is null)
        {
            AnsiConsole.WriteLine("No records found.");
            ContinueMessage();
            
            throw new ArgumentNullException();
        }

        var codingSessions = records.ToList();
        tableConstructor.PopulateWithRecords(codingSessions);

        return (
            tableConstructor.SummaryTable, 
            tableConstructor.SummaryTableForSaving, 
            tableConstructor.FormattedDuration
        );
    }
}