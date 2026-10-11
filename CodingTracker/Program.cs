namespace CodingTracker;

class Program
{
  static void Main(string[] args)
  {
    Repository.CreateDB();
    Displayer.PrintTitle();
    UserInterface.MainMenu();
  }
}
