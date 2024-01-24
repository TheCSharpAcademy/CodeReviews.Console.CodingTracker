using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker;

public abstract class Menu
{
    
    protected MenuManager MenuManager { get; }

    protected Menu(MenuManager menuManager)
    {
        MenuManager = menuManager;
    }
    public abstract void Display();
}

    public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {

    }
}
