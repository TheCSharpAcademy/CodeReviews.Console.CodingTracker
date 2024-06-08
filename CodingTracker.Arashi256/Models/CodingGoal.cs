
namespace CodingTracker.Arashi256.Models
{
    internal class CodingGoal
    {
        public int? Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public int Hours { get; set; }
        public DateTime DeadlineDateTime { get; set; }
    }
}
