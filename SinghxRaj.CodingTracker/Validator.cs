namespace SinghxRaj.CodingTracker
{
    internal class Validator
    {
        internal static bool ValidateOption(int option)
        {
            return option >= 0 && option <= 2;
        }
    }
}