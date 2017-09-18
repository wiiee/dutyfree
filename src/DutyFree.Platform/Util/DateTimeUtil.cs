namespace DutyFree.Platform.Util
{
    using System;

    public static class DateTimeUtil
    {
        public static DateTime GetCurrentMonday()
        {
            var today = DateTime.Today;
            int interval = 1 - (int)today.DayOfWeek;
            interval = interval == 1 ? -6 : interval;
            return today.AddDays(interval);
        }

        public static DateTime GetCurrentWeekEndDay()
        {
            return DateTime.Today.AddDays(7 - (int)DateTime.Today.DayOfWeek);
        }

        public static DateTime GetCurrentMonthStartDay()
        {
            DateTime date = DateTime.Today;
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime GetCurrentMonthEndDay()
        {
            DateTime date = DateTime.Today;
            return new DateTime(date.Year, date.Month+1, 1).AddDays(-1);
        }

        public static int GetWorkingDays(DateTime startDate, DateTime endDate)
        {
            var firstDate = new DateTime(startDate.Ticks);
            var result = 0;

            while(firstDate <= endDate)
            {
                if (firstDate.DayOfWeek != DayOfWeek.Saturday && firstDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    result++;
                }

                firstDate = firstDate.AddDays(1);
            }

            return result;
        }

        public static DateTime GetNextMonthStartDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month + 1, 1);
        }

        public static DateTime GetNextMonthEndDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month + 2, 1).AddDays(-1);
        }
    }
}
