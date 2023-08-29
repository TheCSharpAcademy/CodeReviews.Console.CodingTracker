using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.w0lvesvvv
{
    public class CodingSession
    {
        #region DATABASE PROPERTIES
        public int coding_session_id_i { get; set; }
        public string coding_session_start_date_time_nv { get; set; } = string.Empty;
        public string coding_session_end_date_time_nv { get; set; } = string.Empty;
        private int coding_session_duration_i { get; set; }
        #endregion

        #region METHODS
        public int getDuration () { return this.coding_session_duration_i; }
        public void calculateDuration()
        {
            DateTime codingStartTime = DateTime.ParseExact(coding_session_start_date_time_nv, Validation.dateTimeFormat, CultureInfo.InvariantCulture);
            DateTime codingEndTime = DateTime.ParseExact(coding_session_end_date_time_nv, Validation.dateTimeFormat, CultureInfo.InvariantCulture);
            TimeSpan codingDuration = codingEndTime - codingStartTime;

            this.coding_session_duration_i = (int)codingDuration.TotalMinutes;
        }
        #endregion
    }
}
