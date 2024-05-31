namespace Patryk_MM.Console.CodingTracker.Utilities {
    /// <summary>
    /// Provides extension methods for manipulating dates.
    /// </summary>
    public static class DateManipulation {
        /// <summary>
        /// Truncates the specified <see cref="DateTime"/> to the nearest interval defined by the given <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to be truncated.</param>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> interval to truncate to.</param>
        /// <returns>The truncated <see cref="DateTime"/>.</returns>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan) {
            if (timeSpan == TimeSpan.Zero)
                return dateTime;

            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
                return dateTime;

            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
