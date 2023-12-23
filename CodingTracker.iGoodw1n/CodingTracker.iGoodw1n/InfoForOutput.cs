namespace CodingTracker.iGoodw1n;

public class InfoForOutput
{
    public object? Information { get; }
    public InfoType Type { get; }

    public InfoForOutput(object? information, InfoType type)
    {
        Information = information;
        Type = type;
    }
}
