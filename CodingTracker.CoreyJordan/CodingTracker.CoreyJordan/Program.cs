using CodingTrackerLibrary;

try
{
    CrudController.InitDatabase();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}