namespace CodingTracker;

public static class OptionsPicker
{
    public static int MenuIndex { get; private set; }
    public static int ScrollingIndex { get; private set; }
    private static int currentIndex;
    private static int lastIndex;
    private static int lastMenuIndex;
    private static int longestStringLength;
    private static int higlightBoxWidth;
    private static readonly int higlightBoxMargins = 5;

    public static void Navigate(string[] menuOptions, bool escapeOption, bool scrollingEnabled = false)
    {
        bool enterPressed = false;
        bool onlyOneOption = menuOptions.Length == 0;
        int headerHeight = Console.GetCursorPosition().Top;
        lastMenuIndex = 0;
        MenuIndex = 0;
        lastIndex = headerHeight; //important when only single-option menu
        currentIndex = MenuIndex + headerHeight; //to count with each menu's header
        longestStringLength = menuOptions.OrderByDescending(s => s.Length).First().Length;
        higlightBoxWidth = longestStringLength + higlightBoxMargins;

        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (MenuIndex == i)
                HighlightOption();
            else
                Console.ResetColor();

            Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[i]));
            Console.ResetColor();
        }

        do
        {
            while (!Console.KeyAvailable) { };

            var keypress = Console.ReadKey(false).Key;

            if (keypress == ConsoleKey.DownArrow && MenuIndex < menuOptions.Length - 1)
            {
                lastIndex = currentIndex;
                lastMenuIndex = MenuIndex;
                MenuIndex++;
                currentIndex++;
            }
            else if (keypress == ConsoleKey.UpArrow && MenuIndex > 0)
            {
                lastIndex = currentIndex;
                lastMenuIndex = MenuIndex;
                MenuIndex--;
                currentIndex--;
            }
            else if (keypress == ConsoleKey.Enter)
            {
                EnterPressed(menuOptions);
                enterPressed = true;
            }
            else if (keypress == ConsoleKey.Escape && escapeOption)
            {
                EscapePressed(menuOptions, headerHeight);
                MenuIndex = menuOptions.Length - 1;
                break;
            }
            if (!onlyOneOption && !enterPressed)
            {
                SwitchHighlight(menuOptions);
            }
        } while (!enterPressed);
    }
    
    public static void NavigateWithScrolling(string[] menuOptions, bool escapeOption)
    {
        bool enterPressed = false;
        bool onlyOneOption = menuOptions.Length == 0;
        int headerHeight = Console.GetCursorPosition().Top;
        ScrollingIndex = 0;
        lastMenuIndex = 0;
        MenuIndex = 0;
        lastIndex = headerHeight; //important when only single-option menu
        currentIndex = MenuIndex + headerHeight; //to count with each menu's header
        longestStringLength = menuOptions.OrderByDescending(s => s.Length).First().Length; //to set the width of higlighted box in a menu
        higlightBoxWidth = longestStringLength + 5;

        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (MenuIndex == i)
            {
                HighlightOption();
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[i]));
            Console.ResetColor();
        }

        do
        {
            while (!Console.KeyAvailable) { };

            var keypress = Console.ReadKey(false).Key;

            if (keypress == ConsoleKey.DownArrow && MenuIndex < menuOptions.Length - 1)
            {
                lastIndex = currentIndex;
                lastMenuIndex = MenuIndex;
                MenuIndex++;
                currentIndex++;
            }
            else if (keypress == ConsoleKey.UpArrow && MenuIndex > 0)
            {
                lastIndex = currentIndex;
                lastMenuIndex = MenuIndex;
                MenuIndex--;
                currentIndex--;
            }
            else if (keypress == ConsoleKey.Enter)
            {
                EnterPressed(menuOptions);
                enterPressed = true;
            }
            else if (keypress == ConsoleKey.Escape && escapeOption)
            {
                EscapePressed(menuOptions, headerHeight);
                MenuIndex = menuOptions.Length - 1;
                break;
            }
            else if (keypress == ConsoleKey.A)
            {
                ScrollingIndex++;
                break;
            }

            else if (keypress == ConsoleKey.Q)
            {
                ScrollingIndex--;
                break;
            }
            if (!onlyOneOption && !enterPressed)
            {
                SwitchHighlight(menuOptions);
            }
        } while (!enterPressed);
    }

    private static void EnterPressed(string[] menuOptions)
    {
        Console.SetCursorPosition(0, currentIndex);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[MenuIndex]));
        Console.ResetColor();
        Thread.Sleep(100);
    }

    private static void HighlightOption()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }

    private static void SwitchHighlight(string[] menuOptions)
    {
        Console.SetCursorPosition(0, lastIndex);
        Console.ResetColor();
        Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[lastMenuIndex]));

        Console.SetCursorPosition(0, currentIndex);
        HighlightOption();
        Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[MenuIndex]));
    }

    private static void EscapePressed(string[] menuOptions, int headerHeight)
    {
        Console.SetCursorPosition(0, currentIndex);
        Console.ResetColor();
        Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[MenuIndex]));

        Console.SetCursorPosition(0, headerHeight + menuOptions.Length - 1);
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine(string.Format("  {0,-" + higlightBoxWidth + "}", menuOptions[menuOptions.Length - 1]));
        
        Console.ResetColor();
        Thread.Sleep(100);
    }
}