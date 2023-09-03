using CodingTracker.w0lvesvvv;

DataBaseManager.CreateDatabase();

CodingController controller = new();


do
{
    controller.DisplayMenu();
} while (true);