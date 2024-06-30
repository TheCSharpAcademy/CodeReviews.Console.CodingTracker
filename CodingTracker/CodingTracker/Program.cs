namespace CodingTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();
            DbController dbController = new DbController(menu);
            menu.SetDbController(dbController);
            menu.DisplayMenu();     
        }
    }
}
