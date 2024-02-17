namespace CodingTracker.iGoodw1n;

public class CodingApp
{
    private InfoForOutput _info = new InfoForOutput(null, InfoType.Text);
    private readonly DataContext _dataContext;
    private readonly Action<InfoForOutput> _update;

    public CodingApp(DataContext dataContext, Action<InfoForOutput> update)
    {
        _dataContext = dataContext;
        _update = update;
    }

    public InfoForOutput Info
    {
        get => _info;
        set
        {
            _info = value;
            _update(_info);
        }
    }

    public void UpdateRecord(CodingSession record)
    {
        _dataContext.UpdateRecord(record);

        var updatedRecord = _dataContext.GetRecord(record.Id);

        Info = new InfoForOutput(updatedRecord, InfoType.OneSession);
    }

    public void DeleteRecord(int id)
    {
        _dataContext.DeleteRecord(id);
    }

    public void InsertSession(CodingSession record)
    {
        _dataContext.CreateRecord(record);
    }

    public void ShowRecord(int id)
    {
        var record = _dataContext.GetRecord(id);

        Info = new InfoForOutput(record, InfoType.OneSession);
    }

    public void ShowAllRecords()
    {
        var records = _dataContext.GetAllRecords();

        Info = new InfoForOutput(records, InfoType.AllSessions);
    }

    public void ShowReport(int year)
    {
        var recordsForOneYear = _dataContext.GetAllRecords()?.Where(r => r.Start.Year == year).ToList();

        Info = new InfoForOutput(recordsForOneYear, InfoType.AnnualReport);
    }

    public void ShowAllYears()
    {
        var records = _dataContext.GetAllRecords();

        var years = records?.GroupBy(r => r.Start.Year).Select(g => g.Key).ToList();

        Info = new InfoForOutput(years, InfoType.Years);
    }
}
