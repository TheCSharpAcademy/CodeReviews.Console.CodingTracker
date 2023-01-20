namespace SinghxRaj.CodingTracker
{
    internal class Validator
    {
        internal static bool ValidateOption(int option)
        {
            return option >= 0 && option <= 2;
        }

        internal static bool ValidateCodingSession(DateTime start, DateTime end)
        {
            // TODO
            return false;
        }

        internal static bool ValidateStartDate(string? dateInput)
        {
            throw new NotImplementedException();
        }

        internal static bool ValidateEnd(DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}