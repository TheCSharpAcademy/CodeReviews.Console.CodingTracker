using System.Globalization;

namespace CodingTracker
{
    internal class Main
    {
        private SqliteOperations operations;
        public Main(SqliteOperations operations) {
            this.operations = operations;

        }

        public void Run()
        {
            bool exitTracker = false;
            Display.DisplayIntroMessage();
            do
            {
                Display.DisplaySeperator();
                Display.DisplayMenu();
                string input = Console.ReadLine();
                try
                {
                    int option = Int32.Parse(input);
                    switch (option)
                    {
                        case 1:
                            List<CoddingSession> habbits = operations.GetSessions();
                            Display.DisplayAllSessions(habbits);
                            break;
                        case 2:
                            HandleRecordInput();
                            break;
                        case 3:
                            HandleDelete();
                            break;
                        case 4:
                            HandleUpdate();
                            break;
                        case 0: exitTracker = true; break;
                        default: Display.DisplayIncorrectMenuOption(); break;
                    }

                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        Display.DisplayIncorrectNumber();
                    }
                }
            } while (!exitTracker);
        }

        void HandleRecordInput()
        {
            Display.DisplaySeperator();
            try
            {
                Display.DisplayEnterStartDate();
                DateTime dateStart = (DateTime)HandleDateInput();
                Display.DisplayEnterEndDate();
                DateTime dateEnd = (DateTime)HandleDateInput(true, dateStart);
                
                operations.InsertSession(dateStart, dateEnd);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException)
                {
                    return;
                }
                Display.SqliteException();
                return;
            }
        }

        DateTime? HandleDateInput(bool endDate = false, DateTime? startDate = null)
        {
            Display.DisplayDateInput();
            CultureInfo provider = CultureInfo.InvariantCulture;
            do
            {
                try
                {
                    string input = Console.ReadLine();
                    DateTime dateTime = DateTime.ParseExact(input, SqliteOperations.GetDateFormat(), provider);
                    if (endDate && startDate != null) {
                        Verifier.VerifyDate((DateTime)startDate, dateTime);
                    }
                    return dateTime;
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException)
                    {
                        Display.DisplayIncorrectFormat();
                    }else if(e is ArgumentException)
                    {
                        Console.WriteLine(e.Message);
                    }
                    else
                    {
                        Display.IOException();
                        return null;
                    }
                }

            } while (true);
        }

        void HandleDelete()
        {
            Display.DisplayDeleteLog();
            Display.DisplaySeperator();
            Display.DisplayAllSessions(operations.GetSessions());
            bool exitDelete = false;

            do
            {
                try
                {
                    string input = Console.ReadLine();
                    long id = Int64.Parse(input);
                    if (id <= 0)
                    {
                        throw new FormatException();
                    }
                    operations.DeleteSession(id);
                    exitDelete = true;
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        Display.DisplayIncorrectNumber();
                    }
                    else
                    {
                        Display.IncorrectId();
                    }
                }


            } while (!exitDelete);
        }

        void HandleUpdate()
        {
            Display.DisplaySeperator();
            Display.DisplayWhichRecordToUpdate();
            Display.DisplayAllSessions(operations.GetSessions());
            long id = -1;
            bool exitHabbitChoice = false;

            do
            {
                try
                {
                    string input = Console.ReadLine();
                    id = Int64.Parse(input);
                    if (id <= 0)
                    {
                        throw new FormatException();
                    }
                    operations.GetSessionWithId(id);
                    exitHabbitChoice = true;
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        Display.DisplayIncorrectNumber();
                    }
                    else
                    {
                        Display.IncorrectId();
                    }
                }


            } while (!exitHabbitChoice);

            if (id > 0)
            {
                Display.DisplaySeperator();
                Display.DisplayUpdateMenu();

                bool updateLoop = false;

                do
                {
                    try
                    {
                        string menuInput = Console.ReadLine();
                        int menuChoice = Int32.Parse(menuInput);
                        switch (menuChoice)
                        {
                            case 1:
                                var quantityInput = HandleDateInput();
                                if (quantityInput == null)
                                {
                                    return;
                                }
                                DateTime quantity = (DateTime)quantityInput;
                                operations.UpdateDate(id, quantity);
                                return;
                            case 2:
                                var dateInput = HandleDateInput();
                                if (dateInput == null)
                                {
                                    return;
                                }
                                DateTime dateOnly = (DateTime)dateInput;
                                operations.UpdateDate(id, dateOnly, false);
                                return;
                            case 0:
                                return;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                        {
                            Display.DisplayIncorrectNumber();
                        }else if(e is ArgumentException)
                        {
                            Console.WriteLine(e.Message);
                        }
                        else
                        {
                            Display.SqliteException();
                            return;
                        }
                    }
                } while (!updateLoop);
            }
        }

    }
}
