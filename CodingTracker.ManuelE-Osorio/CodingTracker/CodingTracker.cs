using System;

namespace CodingTracker;

internal class CodingTracker
{
    static void Main(string[] args)
    {
        List<CodingSession> codingSessions = new();
        // CodingSession newCodingSession;
        // SessionStopWatch currentSession;
        string? userSelection;

        DBOperations.DeleteTable();//delete
        DBOperations.CreateTable();//delete
        InsertTrashData();

        bool runCodingTracker = true;

        do{
            userSelection = MainMenu();
            switch(userSelection)
            {
                case("1"):
                    InsertCodingSession();
                    break;
                case("2"):
                    MofidyCodingSession();
                    break;
                case("3"):
                    DeleteCodingSession();
                    break;
                case("4"):
                    DisplayRecords();
                    break;                                                    
                case("5"):
                    break;                    
                case("6"):
                    break;
                case("7"):
                    break;
                case("0"):
                    runCodingTracker = false;
                    break;                                                            
            }
        }
        while(runCodingTracker);


        // CodingSession obj1 = new("1","2023/11/22","01:20","2023/11/23","04:25");
        // Console.WriteLine(obj1.StartDateTime.TimeOfDay.ToString("hh\\:mm"));



        // InsertTrashData();
        
        // codingSessions = DBOperations.SelectValue();
        // codingSessions = DBOperations.SelectValue("2021/01/01", "2024/01/01",3);


        // SessionStopWatch obj3 = new();
        // obj3.EndSession();

        // CodingSession obj4 = new(1,obj3.StartDate,obj3.EndDate,obj3.SessionTimer);

        // codingSessions.Add(obj4);
        // DataVisualization.PrintTable(codingSessions);

        //Console.WriteLine(DataValidation.ValidateInteger("1",-3,0));
    }

    static string? MainMenu()
    {
        string? userSelection;

        Console.WriteLine("Please enter one of the following options:\n\n"+
            "1) Insert a new coding session record\n"+
            "2) Modify a coding session record\n"+
            "3) Delete a coding session record\n"+
            "4) Display records\n"+
            "5) Start a new coding session\n"+
            "6) Display a coding session report\n"+
            "7) Create a coding goal\n"+
            "0) Exit the application\n"
        );

        userSelection = Console.ReadLine();

        return userSelection;
    }

    static string?[] GetData()
    {
        string? [] data = new string[4];
        data[0] = UserInput.GetDate();
        data[1] = UserInput.GetTime();
        data[2] = UserInput.GetDate(data[0]);
        data[3] = UserInput.GetTime(data[0], data[1],data[2]);
        return data;
    }
    
    static void InsertCodingSession()
    {
        string?[] data = GetData();
        CodingSession newCodingSession = new("1", data[0], data[1], data[2], data[3]);   
        DBOperations.InsertValue(newCodingSession.GetString());   
    }
    
    static void InsertTrashData()
    {
        CodingSession trashobj = new CodingSession("1","2022/10/20","12:30","2022/10/20","14:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("2","2023/10/20","04:30","2023/10/20","07:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("3","2023/11/20","12:30","2023/11/20","14:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("2","2023/11/20","04:30","2023/11/20","07:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("3", "2023/11/21","12:30","2023/11/21","14:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("4", "2023/11/21","06:30","2023/11/21","07:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("5", "2023/11/22","12:30","2023/11/22","14:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("6","2023/11/22","07:30","2023/11/22","09:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("7", "2023/11/23","01:10","2023/11/23","04:25");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("8", "2023/11/23","06:32","2023/11/23","06:46");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("9", "2023/11/24","12:30","2023/11/24","14:30");

        DBOperations.InsertValue(trashobj.GetString());    

        trashobj = new CodingSession("10", "2023/11/24","12:30","2023/11/25","14:30");

        DBOperations.InsertValue(trashobj.GetString());    
    }

    static string? FilterByDate()
    {
        string? answer = UserInput.GetYesNoAnswer("filter by date");
        return answer;
    }

    static string? SortResults()
    {
        string? answer = UserInput.GetYesNoAnswer("sort the results");
        return answer;
    }
    
    static void MofidyCodingSession()
    {
        List<CodingSession> modifyCodingSession = DisplayRecords();
        string? modifyCodingSessionID = UserInput.GetID(modifyCodingSession);
        string?[] data = GetData();
        
        CodingSession codingSession = new(modifyCodingSessionID, data[0],
        data[1], data[2], data[3]);
        DBOperations.UpdateValue(codingSession.GetString());
    }

    static List<CodingSession> DisplayRecords()
    {
        List<CodingSession> codingSessions = new();
        string? startDate;
        string? endDate;
        string? sortOperation;        

        string? modifyByDate = FilterByDate();
        if(modifyByDate == "y")
        {
            startDate = UserInput.GetDate();
            endDate = UserInput.GetDate(startDate);
        }   
        else
        {
            startDate = null;
            endDate = null;
        }

        string? sort = SortResults();
        if (sort == "y")
        {
            sortOperation = UserInput.GetSortOperation();
        }
        else
        {
            sortOperation = null;
        }

        codingSessions = DBOperations.SelectValue(startDate, endDate, sortOperation);
        DataVisualization.PrintTable(codingSessions);
        return codingSessions;
    }

    static void DeleteCodingSession()
    {
        List<CodingSession> deleteCodingSession = DisplayRecords();
        string? deleteCodingSessionID = UserInput.GetID(deleteCodingSession);
        
        DBOperations.DeleteValue(deleteCodingSessionID);

    }
}
