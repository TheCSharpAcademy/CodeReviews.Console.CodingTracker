using Spectre.Console;

public class GoalBarItem : IBarChartItem
{
    public string Label { get; set; }
    public double Value { get; set; }
    public Color? Color { get; set; }

    public GoalBarItem(string label, double value, Color? color = null)
    {
        Label = label;
        Value = value;
        Color = color;
    }
}
