using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_tracker;

public class CodingSession
{
    public int Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Duration { get; set; }
    [NotMapped]
    public string WeekNumber { get; set; }
    [NotMapped]
    public string Date { get; set; }
}
