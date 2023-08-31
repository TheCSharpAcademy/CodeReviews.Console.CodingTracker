namespace CodingTracker.w0lvesvvv
{
    public static class UserInput
    {
        public static int? ReadNumber()
        {
            string inputNumber = Console.ReadLine() ?? string.Empty;
            if (!Validation.ValidateNumber(inputNumber, out int parsedNumber)) return null;

            return parsedNumber;
        }

        public static string ReadDateTimeString() { 
            string inputDateTime = Console.ReadLine() ?? string.Empty;

            if (!Validation.ValidateDateTimeString(inputDateTime)) return string.Empty;

            return inputDateTime;
        }
    }
}
