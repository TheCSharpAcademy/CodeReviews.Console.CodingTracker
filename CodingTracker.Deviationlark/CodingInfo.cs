namespace CodingTracker
{
    internal class CodingSession
    {
        internal int Id { get; set; }
        internal string? Date { get; set; }
        internal string? Duration { get; set; }
    }

    internal class Filter
    {
        internal int Id { get; set; }
        internal int Date { get; set; }
        internal string? Duration { get; set; }
    }

    internal class Goals
    {
        internal int Id { get; set; }
        internal string? Hours { get; set; }

        internal string? Date { get; set; }
        internal string? RemainingDays { get; set; }
        internal string? RemainingHours { get; set; }
        internal string? HoursPerDay { get; set; }
    }
}