namespace CodingTracker;

class Program
{
  static void Main(string[] args)
  {
    Database.CreateDB();
    Displayer.PrintTitle();
    UserInterface.MainMenu();
  }
}
