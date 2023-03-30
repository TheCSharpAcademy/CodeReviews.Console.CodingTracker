using static CodingTracker.DataAccess;
using static CodingTracker.Menu;

InitializeDatabase();

bool closeApp = false;

do
{
    DisplayMenu();
} while (closeApp == false);

