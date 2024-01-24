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
        switch (OptionsPicker.MenuIndex)
        {
            case 0:
                MenuManager.NewMenu(new CodingSessionMenu(MenuManager));
                break;
            case 1:
                MenuManager.NewMenu(new ShowRecordsMenu(MenuManager));
                break;
            default:
                Environment.Exit(0);
                break;
        }
        MenuManager.DisplayCurrentMenu();

    }
}

public class CodingSessionMenu : Menu
{
    public CodingSessionMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        UserInterface.CodingSessionMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 2:
                MenuManager.GoBack();
                MenuManager.DisplayCurrentMenu();
                break;
        }
    }
}

public class ShowRecordsMenu : Menu
{
    public ShowRecordsMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        UserInterface.ShowRecordsMenu();

        switch (OptionsPicker.MenuIndex)
        {
            case 4:
                MenuManager.GoBack();
                MenuManager.DisplayCurrentMenu();
                break;
        }
    }
}
