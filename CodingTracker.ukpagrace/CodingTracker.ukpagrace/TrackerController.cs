using Application.Entities;
using DatabaseLibrary;
using System.Globalization;


namespace CodingTracker.ukpagrace
{
    internal class TrackerController
    {
        Database database = new();
        void CreateTable()
        {
            database.Create();
        }


        void InsertTable()
        {
            DateTime startDate = GetStartDate();
            DateTime endDate = GetEndDate();

            while (startDate > endDate)
            {
                Console.WriteLine("start date cannot be greater than end date");
                endDate = GetEndDate();

            }
            TimeSpan duration = endDate - startDate;
            database.Insert(startDate, endDate, duration);
        }

        void ListTable()
        {
            database.List();
        }

        void UpdateRecord()
        {
            database.List();
            int id = GetNumberInput("Enter the Id your want to update");

            UserEntity userEntity = database.GetOne(id);
            Console.WriteLine($"{userEntity.Id}, {userEntity.StartDate}, {userEntity.EndDate}, {userEntity.Duration}");
            Console.WriteLine("select the column you want to update");
            Console.WriteLine("1 - update start Date");
            Console.WriteLine("2 - update end Date");
            var option = Console.ReadLine();


            if (option == "1")
            {
                DateTime startDate = GetStartDate();
                DateTime endDate;

                DateTime.TryParseExact(userEntity.EndDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);

                while (startDate > endDate)
                {
                    Console.WriteLine("Start Date cannot greater than Start Date");
                    startDate = GetStartDate();
                }

                TimeSpan duration = endDate - startDate;

                database.Update(id, startDate, endDate, duration);

            }
            else if (option == "2")
            {
                DateTime endDate = GetEndDate();
                DateTime startDate;

                DateTime.TryParseExact(userEntity.StartDate, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

                while (endDate < startDate)
                {
                    Console.WriteLine("End Date cannot be less than Start Date");
                    endDate = GetEndDate();
                }

                TimeSpan duration = endDate - startDate;

                database.Update(id, startDate, endDate, duration);
            }
            else
            {
                Console.WriteLine("Select an option from the menu");
            }
        }

        public void DeleteRecord()
        {
            database.List();
            int id = GetNumberInput("Enter the Id your want to delete");
            database.Delete(id);
        }


        public void FilterRecords()
        {
            Console.WriteLine("Select a filter option");
            Console.WriteLine("1 - year");
            Console.WriteLine("2 - month");
            Console.WriteLine("3 - days");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    FilterTable("%Y", "year", "yyyy eg 2024");
                    break;
                case "2":
                    FilterTable("%Y-%m", "months", "yyyy-mm eg 2024-06");
                    break;
                case "3":
                    FilterTable("%Y-%m-%d", "day", "yyyy-mm-dd eg 2024-06-24");
                    break;
                default:
                    Console.WriteLine("InvalidInput, Select an option from menu");
                    FilterRecords();
                    break;
            }
        }


        public void FilterTable(string format, string filter, string filterFormat)
        {
            Console.WriteLine($"Range or a specific {filter}");
            Console.WriteLine("1 - range");
            Console.WriteLine($"2 - specific {filter}");
            var option = Console.ReadLine();
            if (option == "1")
            {
                Console.WriteLine($"Enter the first range value in the format of {filterFormat}");
                var firstRange = Console.ReadLine();
                Console.WriteLine($"Enter the second range value {filterFormat}");
                var secondRange = Console.ReadLine();
                string order = GetOrderInput();
                database.Filter(format, firstRange, secondRange, order);
            }
            else
            {
                Console.WriteLine($"Enter the {filter} to filter in the format of {filterFormat}");
                var firstRange = Console.ReadLine();
                string order = GetOrderInput();
                database.Filter(format, firstRange, order);
            }
        }

        public void StartWatch()
        {
            DateTime startDate = DateTime.Now;
            DateTime interval;
            Console.WriteLine("To Stop watch enter 0");
            Thread inputThread = new Thread(new ThreadStart(StopWatch));
            inputThread.Start();

            while (!stop)
            {
                interval = DateTime.Now;
                Console.Clear();
                Console.WriteLine($"{interval.ToString("yyyy-MM-dd hh:mm:ss")}");
                Thread.Sleep(1000);
            }
            DateTime endDate = DateTime.Now;
            TimeSpan duration = GetDuration(startDate, endDate);

            database.Insert(startDate, endDate, duration);
        }

        public void StopWatch()
        {
            while (!stop)
            {
                var input = Console.ReadLine();

                if (input == "0")
                {
                    stop = true;
                }
            }
        }


        void GetReport()
        {

            DateTime dateTime = DateTime.Now;
            string month = $"{dateTime.Year}-{dateTime.Month:D2}";

            database.Analyze("%Y-%m", month);
        }
    }
}
