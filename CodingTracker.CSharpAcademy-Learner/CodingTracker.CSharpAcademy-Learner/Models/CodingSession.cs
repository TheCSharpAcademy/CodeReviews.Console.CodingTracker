using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.CSharpAcademy_Learner.Models
{
    internal class CodingSession
    {
        public int Id { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int Duration { get; set; } = 0;
    }
}
