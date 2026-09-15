using System.Data;
using Dapper;

public class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
{
    public override void SetValue(IDbDataParameter parameter, TimeSpan value)
    {
        parameter.Value = value.ToString(@"hh\:mm\:ss");
    }

    public override TimeSpan Parse(object value)
    {
        return TimeSpan.Parse(value.ToString()!);
    }
}