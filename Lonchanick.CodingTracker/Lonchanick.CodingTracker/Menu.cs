namespace Lonchanick.CodingTracker;

internal class Menu
{
    public static void Run()
    {
        string aux;
        int op;

        do
        {
            Console.Clear();
            Console.WriteLine("\tCoding Tracker");
            Console.WriteLine($"1) {Options.CreateNewRecord}");
            Console.WriteLine($"2) {Options.ReadAllRecords}");
            Console.WriteLine($"3) {Options.UpdateRecord}");
            Console.WriteLine($"4) {Options.DeleteRecord}");
            Console.Write($"0) {Options.Exit}\t");

            aux = Console.ReadLine();

            if (!int.TryParse(aux, out op))
                op = -1;

            switch (op)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("\t\tCreateNewRecord");
                    Controller.CreateNewRecord();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("\tReadAllRecords");
                    Controller.ReadAllRecords();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("\tUpdateRecord");
                    Controller.UpdateRecord();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("\tDeleteRecord");
                    Controller.DeleteRecord();
                    break;

                case 0:
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    Console.ReadLine();
                    break;
            }


        } while (true);


        int GetValidInteger(string param)
        {
            string aux = "";
            int result;

            while (!int.TryParse(aux, out result))
            {
                Console.Write($"Type {param}: ");
                aux = Console.ReadLine();
            }

            return result;
        }
    }

}


enum Options
{
    CreateNewRecord,
    ReadAllRecords,
    UpdateRecord,
    DeleteRecord,
    Exit
}