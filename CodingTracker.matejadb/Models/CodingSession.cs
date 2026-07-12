using Spectre.Console;

namespace CodingTracker.matejadb.Models;

internal class CodingSession {
    internal int Id { get; set; }
    internal string StartTime { get; set; }
    internal string EndTime { get; set; }
    internal string Duration { get; set; }

    internal CodingSession(int id, string startTime, string endTime, string duration) {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }

    internal CodingSession() { }
}
