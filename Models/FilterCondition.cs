public class FilterCondition
{
    public string ComparedTo { get; set; }
    public string Relational {  get; set; }
    public string Type { get; set; }

    public FilterCondition(ConditionType type, RelationalCondition relational, string compare)
    {
        this.Type = ConvertCondtionType(type);
        this.Relational = ConvertRelational(relational);
        this.ComparedTo = compare;
    }
    private string ConvertCondtionType(ConditionType type)
    {
        switch (type)
        {
            case ConditionType.Date:
                return "date";
            case ConditionType.StartTime:
                return "start";
            case ConditionType.EndTime:
                return "end";
            case ConditionType.Duration:
                return "duration";
            default:
                return "start";
        }
    }

    private string ConvertRelational(RelationalCondition relational)
    {
        switch (relational)
        {
            case RelationalCondition.GreaterThanOrEqual:
                return ">=";
            case RelationalCondition.LessThanOrEqual:
                return "<=";
            case RelationalCondition.Exactly:
                return "=";
            default:
                return "=";
        }
    }

    public enum ConditionType
    {
        Date,
        StartTime,
        EndTime,
        Duration,
        None
    }

    public enum RelationalCondition
    {
        GreaterThanOrEqual,
        LessThanOrEqual,
        Exactly,
        None
    }
}

