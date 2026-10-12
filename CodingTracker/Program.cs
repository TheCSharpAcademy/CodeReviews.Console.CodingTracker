using Spectre.Console;

namespace CodingTracker;

// finish auto autoFill
// add feedback from previous project
// write readme

class Program
{
  static void Main(string[] args)
  {
    Repository.CreateDB();
    Displayer.PrintTitle();
    UserInterface.MainMenu();
  }
}
