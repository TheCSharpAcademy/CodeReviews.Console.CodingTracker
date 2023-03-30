using static CodingTracker.DataAccess;
using static CodingTracker.Menu;

InitializeDatabase();

bool closeApp = false;

do
{
    displayMenu();
} while (closeApp == false);

