using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.Models
{
    public class CodingSessionModel
    {
        public string Id { get; set; }
        public DateTime startTime {  get; set; }
        public DateTime endTime { get; set; }
        public float duration { get; set; }

    }
}
