namespace CodingTracker
{
    internal class CodingGoal
    {
        public static int LoadGoalValue()
        {
            string filePath = "storedValue.txt";

            if (File.Exists(filePath))
            {
                string valueString = File.ReadAllText(filePath);

                if (int.TryParse(valueString, out int storedValue))
                {
                    return storedValue;
                }
            }

            return 0;
        }

        public static void SaveGoalValue(int value)
        {
            string filePath = "storedValue.txt";
            File.WriteAllText(filePath, value.ToString());
        }
    }
}
