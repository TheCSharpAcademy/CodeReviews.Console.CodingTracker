namespace CodingTracker.K_MYR.Models
{
    internal class CodingGoal
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime Deadline { get; set; }

        public TimeSpan Goal { get; set; }

        public TimeSpan ElapsedTime { get; set; }

        public double ElapsedPercentage
        {
            get
            {   double percentage = ElapsedTime / Goal;

                if (percentage < 1)
                    return percentage;
                else
                    return 1;
            }
        }

        public TimeSpan AverageHoursToGoal
        {
            get
            {
                double days = (Deadline - DateTime.Now).TotalDays;

                if (days > 0)
                {
                    if (Goal > ElapsedTime)
                        return (Goal - ElapsedTime) / days;
                    else
                        return new TimeSpan(0, 0, 0);
                }                   
                else 
                    return new TimeSpan(0,0,0);                
            }
        }
    }
}
