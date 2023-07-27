using Ohshie.CodingTracker.DbOperations;

DbOperations dbOperations = new();
dbOperations.CreateDb();
        
MainMenu menus = new();
menus.Initialize();