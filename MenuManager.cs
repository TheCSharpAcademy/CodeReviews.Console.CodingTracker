using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTracker;

public class MenuManager
{
    private readonly Stack<Menu> menuStack = new Stack<Menu>();
    
    public MenuManager()
    {
        menuStack.Push(new MainMenu(this));
    }

    public void DisplayCurrentMenu()
    {
        if (menuStack.Count > 0)
        {
            Menu currentMenu = menuStack.Peek();
            currentMenu.Display();
        }
    }

    public void NewMenu(Menu menu)
    {
        menuStack.Push(menu);
    }

    public void GoBack()
    {
        if (menuStack.Count > 1)
            menuStack.Pop();
    }

    public void ReturnToMainMenu()
    {
        while (menuStack.Count > 1)
            menuStack.Pop();
    }
}
