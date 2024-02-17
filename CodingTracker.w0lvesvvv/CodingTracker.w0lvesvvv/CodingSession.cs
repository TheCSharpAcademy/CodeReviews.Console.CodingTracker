using System.Globalization;

namespace CodingTracker.w0lvesvvv
{
    public class CodingSession
    {
        #region DATABASE PROPERTIES
        public int Coding_session_id_i { get; set; }
        public string Coding_session_start_date_time_nv { get; set; } = string.Empty;
        public string Coding_session_end_date_time_nv { get; set; } = string.Empty;
        private int Coding_session_duration_i { get; set; }
        #endregion

        #region METHODS
        public int GetDuration () { return this.Coding_session_duration_i; }
        public void CalculateDuration()
        {
            DateTime codingStartTime = DateTime.ParseExact(Coding_session_start_date_time_nv, Validation.DateTimeFormat, CultureInfo.InvariantCulture);
            DateTime codingEndTime = DateTime.ParseExact(Coding_session_end_date_time_nv, Validation.DateTimeFormat, CultureInfo.InvariantCulture);
            TimeSpan codingDuration = codingEndTime - codingStartTime;

            this.Coding_session_duration_i = (int)codingDuration.TotalMinutes;
        }
        #endregion
    }
}
