namespace CodingTracker;

class ReportController
{
    private Database database;
    private MainMenuController? mainMenuController;

    public ReportController(Database database)
    {
        this.database = database;
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void TotalAndAverage(ReportPeriod period)
    {
        var results = database.GetTotalAndAverage(period);
        var view = new ReportView(this, results);
        view.Show();
    }

    public void BackToMainMenu()
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu();
    }
}