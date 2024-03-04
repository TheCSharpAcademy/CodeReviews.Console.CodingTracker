

namespace CodingTracker.Models
{
    internal class Habit
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }


        public Habit(int id, string name, string measurementUnit)
        {
            Id = id;
            Name = name;
            MeasurementUnit = measurementUnit;
        }
    }
}
