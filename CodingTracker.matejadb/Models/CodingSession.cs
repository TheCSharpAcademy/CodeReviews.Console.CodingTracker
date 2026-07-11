using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Models;

internal class CodingSession {
    private int Id { get; set; }
    private string StartTime { get; set; }
    private string EndTime { get; set; }
    private int Duration { get; set; }

    public CodingSession(int id, string startTime, string endTime, int duration) {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }
}
