using System;

namespace CodingTracker;

internal class CodingTracker
{
    static void Main(string[] args)
    {
        List<CodingSession> codingSessions = new();
        SessionStopWatch currentSession;
        string startDate;
        string startTime;
        string endDate;
        string endTime;
        string? userSelection;

        DBOperations.DeleteTable();//delete
        DBOperations.CreateTable();//delete


        bool runCodingTracker = true;

        do{
            userSelection = MainMenu();
            switch(userSelection)
            {
                case("1"):
                    break;
                case("2"):
                    break;
                case("3"):
                    break;
                case("4"):
                    break;                                                    
                case("5"):
                    break;                    
                case("6"):
                    break;
                case("7"):
                    break;
                case("0"):
                    break;                                                            
            }
        }
        while(runCodingTracker);


        CodingSession obj1 = new("1","2023/11/22","01:20","2023/11/23","04:25");
        Console.WriteLine(obj1.StartDateTime.TimeOfDay.ToString("hh\\:mm"));



        InsertTrashData();
        
        DBOperations.SelectValue(codingSessions, "2022/10/20", "2023/11/25",2);

        //startDate = UserInput.GetDate();
        //startTime = UserInput.GetTime();
        //endDate = UserInput.GetDate(startDate);
        //endTime = UserInput.GetTime(startDate, startTime,endDate);


        SessionStopWatch obj3 = new();
        Thread.Sleep(10000);
        obj3.EndSession();

        CodingSession obj4 = new(1,obj3.StartDate,obj3.EndDate,obj3.SessionTimer);

        codingSessions.Add(obj4);
        DataVisualization.PrintTable(codingSessions);
    }

    static string? MainMenu()
    {
        string? userSelection;

        Console.Clear();
        Console.WriteLine("Please enter one of the following options:\n"+
            "1) Insert a new coding session record\n"+
            "2) Modify a coding session record\n"+
            "3) Delete a coding session record\n"+
            "4) Display a coding session record\n"+
            "5) Start a new coding session\n"+
            "6) Display a coding session report\n"+
            "7) Create a coding goal\n"+
            "0) Exit the application"
        );

        userSelection = Console.ReadLine();

        return userSelection;
    }

    static void InsertTrashData()
    {
        CodingSession trashobj = new CodingSession("1","2022/10/20","12:30","2022/10/20","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("2","2023/10/20","04:30","2023/10/20","07:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("3","2023/11/20","12:30","2023/11/20","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("2","2023/11/20","04:30","2023/11/20","07:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("3", "2023/11/21","12:30","2023/11/21","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("4", "2023/11/21","06:30","2023/11/21","07:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("5", "2023/11/22","12:30","2023/11/22","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("6","2023/11/22","07:30","2023/11/22","09:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("7", "2023/11/23","01:10","2023/11/23","04:25");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("8", "2023/11/23","06:32","2023/11/23","06:46");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("9", "2023/11/24","12:30","2023/11/24","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    

        trashobj = new CodingSession("10", "2023/11/24","12:30","2023/11/25","14:30");

        DBOperations.InsertValue(trashobj.StartDateTime.Date.ToString("yyyy/MM/dd"), trashobj.StartDateTime.TimeOfDay.ToString("hh\\:mm"),
        trashobj.EndDateTime.Date.ToString("yyyy/MM/dd"), trashobj.EndDateTime.TimeOfDay.ToString("hh\\:mm"),trashobj.ElapsedTime.ToString());    
    }
}
