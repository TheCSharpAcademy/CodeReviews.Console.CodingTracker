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

        public static int? RequestCodingId()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("ID: ");
            Console.ForegroundColor = ConsoleColor.White;
            return ReadNumber();
        }

        public static CodingSession? RequestCodingDates(CodingSession codingSession)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(Time format MUST BE: dd/MM/yyyy hh:mm)");
            Console.Write("Introduce start coding time: ");
            Console.ForegroundColor = ConsoleColor.White;
            codingSession.Coding_session_start_date_time_nv = UserInput.ReadDateTimeString();

            if (string.IsNullOrEmpty(codingSession.Coding_session_start_date_time_nv)) return null;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Introduce end coding time: ");
            Console.ForegroundColor = ConsoleColor.White;
            codingSession.Coding_session_end_date_time_nv = UserInput.ReadDateTimeString();

            if (string.IsNullOrEmpty(codingSession.Coding_session_end_date_time_nv)) return null;

            codingSession.CalculateDuration();

            return codingSession;
        }
    }
}
