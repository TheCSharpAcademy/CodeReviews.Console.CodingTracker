using CodingTracker.w0lvesvvv;

DataBaseManager.createDatabase();

CodingController controller = new();


do
{
    controller.displayMenu();
} while (true);