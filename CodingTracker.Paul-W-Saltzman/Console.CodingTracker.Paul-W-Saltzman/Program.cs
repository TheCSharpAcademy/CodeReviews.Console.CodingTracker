
namespace CodingTracker.Paul_W_Saltzman;
internal class Program
{ 
    private static void Main(string[] args)
    {
        string connectionString = Data.ConnectionString();
        Console.WriteLine(connectionString);
        Data.Init();
        Data.LoadData();
        Menu.TopMenu();

    }
}