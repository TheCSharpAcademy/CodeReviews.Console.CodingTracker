namespace Ohshie.CodingTracker.Menus;

public abstract class MenuBase
{
    public void Initialize()
    {
        bool chosenExit = false;
        while (!chosenExit)
        {
            chosenExit = Menu();
        }
    }

    protected abstract bool Menu();

    protected abstract string MenuBuilder();
}