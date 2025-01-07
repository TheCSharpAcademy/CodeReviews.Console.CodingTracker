internal class ViewOrder
{
    internal static bool ChooseOrder()
    {
        while (true)
        {
            var action = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
                "Choose view order", [
                    "Ascending",
                    "Descending"]);

            if (action == DisplayInfoHelpers.Back)
            {
                Console.Clear();
                return false;
            }
            else if (action == "Ascending")
            {
                Console.Clear();
                if (!RecordRead.DisplayRecordsByOrder("ASC")) return true;
            }
            else if (action == "Descending")
            {
                Console.Clear();
                if (!RecordRead.DisplayRecordsByOrder("DESC")) return true;
            }
        }
    }
}
