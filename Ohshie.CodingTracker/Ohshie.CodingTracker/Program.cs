using Ohshie.CodingTracker.Menus;

class Program
{ 
    public static void Main(string[] args)
    {
        DbOperations dbOperations = new();
        dbOperations.CreateDb();
        
        MainMenu menus = new();
        menus.Initialize();
    }
}
