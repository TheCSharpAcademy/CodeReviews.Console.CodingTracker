using CodingTracker.kmakai.Models;
using ConsoleTableExt;

namespace CodingTracker.kmakai.Controllers
{
    public class ViewController
    {
        public void ViewSessions(List<CodeSession> codeSessions)
        {
            Console.Clear();
            ConsoleTableBuilder.From(codeSessions).ExportAndWriteLine();
        }

        public void ViewSession(CodeSession codeSession)
        {
            Console.Clear();
            ConsoleTableBuilder.From(new List<CodeSession> { codeSession }).ExportAndWriteLine();
        }

        public void MainMenu()
        {
            Console.WriteLine("Welcome to Coding Tracker!");
            Console.WriteLine("1. View sessions");
            Console.WriteLine("2. Create session");
            Console.WriteLine("3. Update session");
            Console.WriteLine("4. Delete session");
            Console.WriteLine("0. Exit");
        }
    }
}
