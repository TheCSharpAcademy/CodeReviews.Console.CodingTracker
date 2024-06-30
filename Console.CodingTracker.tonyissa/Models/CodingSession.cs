namespace CodingTracker.CodingSession
{
    public class CodingSession
    {
        public int ID;
        public DateTime Start;
        public DateTime End;

        public CodingSession(int id, DateTime start, DateTime end)
        {
            ID = id;
            Start = start;
            End = end;
        }
    }

}