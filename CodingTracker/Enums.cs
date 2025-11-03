namespace CodingTracker
{
    public class Enums
    {
        public enum MenuOption
        {
            Create,
            Read,
            Update,
            Delete,
            Start,
            Report,
            SetGoal,
            ShowGoalStatus,
        };

        public enum SessionFilter
        {
            Week,
            Day,
            Year
        }

        public enum SessionOrder
        {
            Ascending,
            Descending
        }
    }
}
