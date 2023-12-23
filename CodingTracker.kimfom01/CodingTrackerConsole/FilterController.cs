namespace CodingTrackerConsole;

public class FilterController
{
    private UserInput _input = new();
    private void ShowFilterMenu()
    {
        Console.WriteLine("d to filter by days");
        Console.WriteLine("m to filter by months");
        Console.WriteLine("y to filter by years");
    }

    public List<CodingTrackerModel> Filter(List<CodingTrackerModel> dataList)
    {
        var output = new List<CodingTrackerModel>();

        ShowFilterMenu();
        var choice = _input.GetChoice();
        while (choice != "b")
        {
            switch (choice)
            {
                case "d":
                    return FilterByDays(dataList, choice);
                case "m":
                    return FilterByMonths(dataList, choice);
                case "y":
                    return FilterByYears(dataList, choice);
                default:
                    Console.WriteLine("Wrong input!");
                    break;
            }

            ShowFilterMenu();
            choice = _input.GetChoice();
        }

        return output;
    }

    private string GetFilter(string choice)
    {
        switch (choice)
        {
            case "d":
                return _input.GetDay();
            case "m":
                return _input.GetMonth();
            case "y":
                return _input.GetYear();
            default:
                return "00";
        }
    }

    private List<CodingTrackerModel> FilterByDays(List<CodingTrackerModel> dataList, string choice)
    {
        var filter = GetFilter(choice);
        var output = dataList.Where(x => x.Date!.Substring(3, 2) == filter).ToList();

        return output;
    }

    private List<CodingTrackerModel> FilterByMonths(List<CodingTrackerModel> dataList, string choice)
    {
        var filter = GetFilter(choice);
        var output = dataList.Where(x => x.Date!.Substring(0, 2) == filter).ToList();

        return output;
    }

    private List<CodingTrackerModel> FilterByYears(List<CodingTrackerModel> dataList, string choice)
    {
        var filter = GetFilter(choice);
        var output = dataList.Where(x => x.Date!.Substring(6, 4) == filter).ToList();

        return output;
    }
}