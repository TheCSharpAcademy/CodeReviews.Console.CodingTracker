class Program
{ 
    public static void Main(string[] args)
    {
        DbOperations dbOperations = new();
        dbOperations.CreateDb();
        
        Menus menus = new();
        menus.Main();
    }
}
