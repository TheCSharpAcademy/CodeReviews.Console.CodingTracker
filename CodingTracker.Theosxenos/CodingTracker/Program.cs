try
{
    var db = new Database();
    db.Initialize();

    var controller = new MainController();
    controller.ShowMainMenu();
}
catch (Exception e)
{
    new BaseView().ShowError($"Unknown error occured: {e.Message}");
}