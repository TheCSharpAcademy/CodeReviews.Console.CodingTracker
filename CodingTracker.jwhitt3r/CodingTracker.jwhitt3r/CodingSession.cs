namespace CodingTracker.jwhitt3r
{
    /// <summary>
    /// The Coding class presents the structure of data that is represented in the database
    /// </summary>
    internal class CodingSession
    {
        /// <summary>
        /// Id is the primary key used within the database
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Date is the formatted date field used within the database
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Startt ime of the coding session
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// End time of the coding session
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// Duration states how long the coding session spanned
        /// </summary>
        public string Duration { get; set; }
    }
}