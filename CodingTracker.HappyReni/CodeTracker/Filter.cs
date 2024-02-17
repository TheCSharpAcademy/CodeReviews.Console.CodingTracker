using System.Globalization;

namespace CodeTracker
{
    internal class Filter
    {
        private List<CodingSession> SessionData { get; set; }
        private FILTER_SELECTOR Selector { get; set; }
        private int? Order {  get; set; }
        private int StartYear { get; set; }
        private int EndYear { get; set; }
        private string? StartWeek { get; set; }
        private string? EndWeek { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        public Filter(List<CodingSession> sessionData) 
        { 
            SessionData = sessionData;
        }
        public void SetParameters(List<object> param)
        {
            Order = (int?)param[3];
            if ((FILTER_SELECTOR)param[0] == FILTER_SELECTOR.YEAR)
            {
                StartYear = (int)param[1];
                EndYear = (int)param[2];
                FilterByYear();
            }
            else if ((FILTER_SELECTOR)param[0] == FILTER_SELECTOR.WEEK)
            {
                StartWeek = (string)param[1];   
                EndWeek = (string)param[2];
                FilterByWeek();
            }
            else if ((FILTER_SELECTOR)param[0] == FILTER_SELECTOR.DAY)
            {
                StartDate = DateTime.Parse((string)param[1]);
                EndDate = DateTime.Parse((string)param[2]);
                FilterByDays();
            }
            else
            {
                return;
            }
            
        }
        public List<List<object>> FilterByYear()
        {
            List<List<object>> sessionList = new();

            foreach(var session in SessionData)
            {
                sessionList.AddRange(DivideByYear(session));
            }

            IOrderedEnumerable<List<object>> sortedList;
            if (Order == 0)
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] ascending
                    select session;
            }
            else
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] descending
                    select session;
            }
            foreach (var session in sortedList)
            {
                sessionList.Add(session);
            }

            return sessionList;
        }
        private List<List<object>> DivideByYear(CodingSession session)
        {
            var StartTime = session.StartTime;
            var EndTime = session.EndTime;
            List<List<object>> sessionList = new();

            var current = 0;
            var end = 0;

            if (StartYear >= StartTime.Year)
            {
                current = StartYear;
            }
            else
            {
                current = StartTime.Year;
            }

            if (EndYear < EndTime.Year)
            {
                end = EndYear;
            }
            while (current < end)
            {
                var list = new List<object>() { current };

                if (current == StartTime.Year)
                {
                    var endDate = new DateTime(StartTime.Year, 12, 31, 0, 0, 0);
                    list.AddRange(new CodingSession(StartTime, endDate).GetField());

                    current = current + 1;
                }
                else if (current == EndTime.Year)
                {
                    var begin = new DateTime(current, 01, 01, 0, 0, 0);
                    list.AddRange(new CodingSession(begin, EndTime).GetField());
                    current = current + 1;
                }
                else
                {
                    var begin = new DateTime(current, 01, 01, 0, 0, 0);
                    list.AddRange(new CodingSession(begin, begin.AddYears(1)).GetField());
                    current = current + 1;
                }
                sessionList.Add(list);
            }
            return sessionList;
        }
        public List<List<object>> FilterByWeek()
        {
            List<List<object>> sessionList = new();

            foreach (var session in SessionData)
            {
                sessionList.AddRange(DivideByWeek(session));
            }

            sessionList = CheckFilterdWeek(sessionList);
            IOrderedEnumerable<List<object>> sortedList;

            if (Order == 0)
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] ascending
                    select session;
            }
            else
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] descending
                    select session;
            }
            var ret = new List<List<object>>();
            foreach (var session in sortedList)
            {
                ret.Add(session);
            }
            return ret;
        }
        private List<List<object>> DivideByWeek(CodingSession session)
        {
            var StartTime = session.StartTime;
            var EndTime = session.EndTime;
            List<List<object>> sessionList = new();
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Calendar calendar = cultureInfo.Calendar;
            CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            var current = StartTime;
            while (current < EndTime)
            {
                int year = calendar.GetYear(current);
                int week = calendar.GetWeekOfYear(current, weekRule, firstDayOfWeek) - 1;
                var day = calendar.GetDayOfWeek(current);
                string week_str = week < 10 ? $"0{week}" : week.ToString();
                var list = new List<object>() { $"{year}-{week_str}" };

                if (current == StartTime)
                {
                    int move = (int)(7 - day);
                    var endDate = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day + move, 0, 0, 0);
                    if (endDate > EndTime)
                    {
                        list.AddRange(new CodingSession(current, EndTime).GetField());
                    }
                    else
                    {
                        list.AddRange(new CodingSession(current, endDate).GetField());
                    }
                    current = endDate;
                }
                else if (DateTime.Compare(current.AddDays(7), EndTime) == 1)
                {
                    list.AddRange(new CodingSession(current, EndTime).GetField());
                    current = current.AddDays(7);
                }
                else
                {
                    list.AddRange(new CodingSession(current, current.AddDays(7)).GetField());
                    current = current.AddDays(7);
                }
                sessionList.Add(list);
            }
            return sessionList;
        }
        private List<List<object>> CheckFilterdWeek(List<List<object>> sessions)
        {
            List<List<object>> ret = new();
            for (int i = 0; i < sessions.Count; i++)
            {
                List<object>? session = sessions[i];
                var week = (string)session[0];

                if (IsWeekValid(StartWeek, EndWeek, week))
                {
                    ret.Add(sessions[i]);
                }
            }
            return ret;
        }
        private bool IsWeekValid(string week1, string week2, string week3)
        {
            string[] w1 = week1.Split('-');
            string[] w2 = week2.Split('-');
            string[] w3 = week3.Split('-');

            var w1_year = Int32.Parse(w1[0]);
            var w1_week = Int32.Parse(w1[1]);
            var w2_year = Int32.Parse(w2[0]);
            var w2_week = Int32.Parse(w2[1]);
            var w3_year = Int32.Parse(w3[0]);
            var w3_week = Int32.Parse(w3[1]);

            if (w1_year > w3_year) return false;
            else if (w3_year > w2_year) return false;
            else if (w1_year == w3_year && w1_week > w3_week) return false;
            else if (w2_year == w3_year && w2_week < w3_week) return false;
            else return true;
        }
        public List<List<object>> FilterByDays()
        {
            List<List<object>> sessionList = new();

            foreach (var session in SessionData)
            {
                sessionList.AddRange(DivideByDays(session));
            }

            IOrderedEnumerable<List<object>> sortedList;

            if (Order == 0)
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] ascending
                    select session;
            }
            else
            {
                sortedList =
                    from session in sessionList
                    orderby session[0] descending
                    select session;
            }
            var ret = new List<List<object>>();
            foreach (var session in sortedList)
            {
                ret.Add(session);
            }
            return ret;
        }
        private List<List<object>> DivideByDays(CodingSession session)
        {
            var StartTime = session.StartTime;
            var EndTime = session.EndTime;
            List<List<object>> sessionList = new();
            var current = new DateTime();

            if (StartDate >= StartTime)
            {
                current = StartDate;
            }
            else
            {
                current = StartTime;
            }

            if (EndDate < EndTime)
            {
                EndTime = EndDate;
            }

            while (current < EndTime)
            {
                var list = new List<object>() { $"{current.Date}" };

                if (current.Date == StartTime.Date)
                {
                    var endDate = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day + 1, 0, 0, 0);
                    list.AddRange(new CodingSession(current, endDate).GetField());

                    current = endDate;
                }
                else if (current.Date == EndTime.Date)
                {
                    list.AddRange(new CodingSession(current, EndTime).GetField());
                }
                else
                {
                    list.AddRange(new CodingSession(current, current.AddDays(1)).GetField());
                }
                current = current.AddDays(1);
                sessionList.Add(list);
            }
            return sessionList;
        }
    }
}
