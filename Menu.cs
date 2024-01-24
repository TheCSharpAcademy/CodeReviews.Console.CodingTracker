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
        UserInterface.MainMenu();
        switch(OptionsPicker.CurrentIndex)
        {
            case 0:
            CodingSessionMenu codingSessionMenu = new(MenuManager);
            MenuManager.NewMenu(codingSessionMenu);
            MenuManager.DisplayCurrentMenu();
            break;
        }
    }
}

public class CodingSessionMenu : Menu
{
    public CodingSessionMenu(MenuManager menuManager) : base(menuManager){}
    public override void Display()
    {
        UserInterface.CodingSessionMenu();
        
        switch(OptionsPicker.CurrentIndex)
        {
            case 2:
            MenuManager.GoBack();
            MenuManager.DisplayCurrentMenu();
            break;
        }
    }
}
