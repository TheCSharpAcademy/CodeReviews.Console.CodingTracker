using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.rthring
{
    public class UserHandler
    {
        DatabaseController Database;
        public UserHandler(DatabaseController Database)
        {
            this.Database = Database;
        }

        public bool GetUserInput()
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update record.");
            Console.WriteLine("------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    return true;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
            return false;
        }
        private void GetAllRecords()
        {
            Console.Clear();
            var result = Database.GetRecords();

            if (result.Count == 0)
            {
                Console.WriteLine("No rows found");
                return;
            }

            foreach (var session in result)
            {
                Console.WriteLine($"{session.Id} - {session.StartTime.ToString("yyyy/MM/dd HH:mm")} - {session.EndTime.ToString("yyyy/MM/dd HH:mm")} - Quantity: {session.Duration}");
            }
            return;
        }
        private void Insert()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        private void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
