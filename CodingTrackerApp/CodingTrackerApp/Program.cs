using DataAcess;
using Spectre.Console;
using System.Text;

namespace CodingTrackerApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        bool openApp = true;
        await Spin.Check(openApp);
    }
}