
using System.Data;


namespace CodingTracker.Paul_W_Saltzman
{
    internal class CodingSession
    {
        internal int Id;
        internal DateTime StartTime;
        internal DateTime EndTime;
        internal TimeSpan TimeSpan;

        internal CodingSession()
        {

        }

        internal CodingSession(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        internal static CodingSession SessionTime(CodingSession session)
        {
            session.TimeSpan = session.EndTime - session.StartTime;
            return session;
        }

        internal static void TimedCodingSession()
        {
            CodingSession session = StopWatch.Timer();
            Data.AddSession(session);

            CheckGoals(session, 1);
        }

        internal static CodingSession GenerateRandomSession()
        {
            CodingSession session = new CodingSession();
            session.StartTime = Helpers.GenerateRandomDateTime();
            session.EndTime = Helpers.GenerateEndTime(session.StartTime);
            session = SessionTime(session);
            session = Data.AddSession(session);

            CheckGoals(session, 0);
            return session;

        }
        internal static DataTable BuildDataTableNoID(List<CodingSession> sessions)
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("StartTime", typeof(DateTime));
            dataTable.Columns.Add("EndTime", typeof(DateTime));
            dataTable.Columns.Add("TimeSpan", typeof(TimeSpan));

            foreach (CodingSession session in sessions)
            {
                dataTable.Rows.Add(session.StartTime, session.EndTime, session.TimeSpan);
            }

            return dataTable;
        }

        internal static DataTable BuildDataTableWithId(List<CodingSession> sessions)
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("StartTime", typeof(DateTime));
            dataTable.Columns.Add("EndTime", typeof(DateTime));
            dataTable.Columns.Add("TimeSpan", typeof(TimeSpan));

            foreach (CodingSession session in sessions)
            {
                dataTable.Rows.Add(session.Id, session.StartTime, session.EndTime, session.TimeSpan);
            }

            return dataTable;
        }

        internal static List<CodingSession> BubbleSortByStartTime(List<CodingSession> sessions)
        {
            int n = sessions.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (sessions[j].StartTime > sessions[j + 1].StartTime)
                    {
                        // Swap the sessions
                        CodingSession temp = sessions[j];
                        sessions[j] = sessions[j + 1];
                        sessions[j + 1] = temp;
                    }
                }
            }

            return sessions;
        }

        internal static List<CodingSession> ReverseBubbleSortByStartTime(List<CodingSession> sessions)
        {
            int n = sessions.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (sessions[j].StartTime < sessions[j + 1].StartTime)
                    {
                        // Swap the sessions
                        CodingSession temp = sessions[j];
                        sessions[j] = sessions[j + 1];
                        sessions[j + 1] = temp;
                    }
                }
            }

            return sessions;
        }

        internal static void Reporting(List<CodingSession> sessions)
        {
            TimeSpan totalTime = CodingSession.TotalList(sessions);
            int numberOfSession = sessions.Count;
            TimeSpan avgTime = totalTime / numberOfSession;
            DataTable report = new DataTable("Report");
            report.Columns.Add("Number of Sessions");
            report.Columns.Add("Total Time");
            report.Columns.Add("Avg Time");
            report.Rows.Add(numberOfSession, totalTime, avgTime);
            Helpers.ShowTable(report, "Reporting");
        }

        internal static TimeSpan TotalList(List<CodingSession> sessions)
        {
            TimeSpan totalTime = new TimeSpan();
            foreach (CodingSession session in sessions)
            {
                totalTime = totalTime = +session.TimeSpan;
            }
            return totalTime;
        }
        internal static List<CodingSession> LoadSessionsByWeek(int showYearWeek)
        {
            List<CodingSession> sessions = Data.LoadSessions();
            List<CodingSession> weekSessions = new List<CodingSession>();
            foreach (CodingSession session in sessions)
            {
                int sessionYearWeek = WeeklyTotals.YearWeekCreator(session.StartTime);
                if (showYearWeek == sessionYearWeek)
                {
                    weekSessions.Add(session);
                }
            }

            return weekSessions;
        }

        internal static List<CodingSession> LoadSessionsByDate(DateOnly showDate)
        {
            List<CodingSession> sessions = Data.LoadSessions();
            List<CodingSession> dateSessions = new List<CodingSession>();

            foreach (CodingSession session in sessions)
            {
                DateOnly date = DateOnly.FromDateTime(session.StartTime);
                if (showDate == date)
                {
                    dateSessions.Add(session);
                }
            }

            return dateSessions;
        }

        internal static bool DoesExist(int id)
        {
            bool exists = false;
            List<CodingSession> sessions = Data.LoadSessions();
            CodingSession session = sessions.FirstOrDefault(session => session.Id == id);

            if (session != null)
            {
                exists = true;
            }
            else
            { }
            return exists;
        }
        internal static CodingSession GetSession(int id)
        {

            List<CodingSession> sessions = Data.LoadSessions();
            CodingSession session = sessions.FirstOrDefault(session => session.Id == id);

            if (session != null)
            {

            }
            else
            { }
            return session;
        }
        internal static CodingSession UpdateAll(CodingSession session)
        {
            DateOnly date = Menu.GetDate("Date Entry");
            TimeOnly start = Menu.GetTime("Start Time");
            TimeOnly end = start;
            while (end <= start)
            {
                end = Menu.GetTime("End Time");

                if (end <= start)
                {
                    Console.WriteLine("Invalid time press ENTER to Continue");
                    Console.ReadLine();
                }
            }
            DateTime dateTimeStart = Helpers.DateTimeBuilder(date, start);
            DateTime dateTimeEnd = Helpers.DateTimeBuilder(date, end);
            session = new CodingSession(dateTimeStart, dateTimeEnd);
            session = CodingSession.SessionTime(session);
            Data.UpdateSession(session);

            CodingSession.CheckGoals(session,1);

            return session;
        }

        internal static CodingSession UpdateEndTime(CodingSession session, TimeOnly endTime)
        {
            DateOnly date = DateOnly.FromDateTime(session.StartTime);
            session.EndTime = Helpers.DateTimeBuilder(date, endTime);
            session = CodingSession.SessionTime(session);
            Data.UpdateSession(session);

            CheckGoals(session,1);

            return session;
        }
        internal static CodingSession UpdateStartTime(CodingSession session, TimeOnly startTime)
        {
            DateOnly date = DateOnly.FromDateTime(session.StartTime);
            session.StartTime = Helpers.DateTimeBuilder(date, startTime);
            session = CodingSession.SessionTime(session);
            Data.UpdateSession(session);

            CheckGoals(session,1);

            return session;
        }

        internal static CodingSession UpdateDate(CodingSession session, DateOnly date)
        {
            TimeOnly startTime = TimeOnly.FromDateTime(session.StartTime);
            TimeOnly endTime = TimeOnly.FromDateTime(session.EndTime);

            DateTime dateTimeStart = Helpers.DateTimeBuilder(date, startTime);
            DateTime dateTimeEnd = Helpers.DateTimeBuilder(date, endTime);

            session.StartTime = dateTimeStart;
            session.EndTime = dateTimeEnd;

            session = CodingSession.SessionTime(session);
            Data.UpdateSession(session);

            CheckGoals(session,1);

            return session;
        }

        internal static void CheckGoals(CodingSession session, int option)
        {
            DailyTotals daily = new DailyTotals();
            WeeklyTotals weekly = new WeeklyTotals();
            daily.Date = DateOnly.FromDateTime(session.StartTime);
            weekly.YearWeek = WeeklyTotals.YearWeekCreator(session.StartTime);
            daily = DailyTotals.CalculateDailyTotal(daily,option);

            if (option == 1)
            {
                if (!daily.TrophyPresented && daily.GoalMet)
                {
                    Helpers.DailyTrophy(daily);
                    daily.TrophyPresented = true;
                    Data.MarkPresented(daily);
                }
                else { }
            }


            weekly = WeeklyTotals.CalculateWeeklyTotal(weekly, option);

            if (option == 1)
            {
                if (!weekly.TrophyPresented && weekly.GoalMet)
                {
                    Helpers.WeeklyTrophy(weekly);
                    weekly.TrophyPresented = true;
                    Data.MarkPresented(weekly);
                }
                else { }
            }
            
        }
    }
}
