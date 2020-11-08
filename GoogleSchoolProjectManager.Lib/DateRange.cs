using System;

namespace GoogleSchoolProjectManager.Lib.GoogleAPI.Sheets
{
    static class DateRange
    {
        //private static string[] Months = {
        //                                 "January", "February", "March", "April",
        //                                 "May", "June", "July", "August",
        //                                 "September", "October", "November", "December"
        //                             };
        private static string[] Months = {
                                         "ledna", "února", "března", "dubna",
                                         "května", "června", "července", "srpna",
                                         "září", "října", "listopadu", "prosince"
                                     };

        public static string Generate(DateTime startDatetime, DateTime endDateTime) => Generate(
            startDatetime.Year, startDatetime.Month, startDatetime.Day,
            endDateTime.Year, endDateTime.Month, endDateTime.Day);

        public static string Generate(
            int startYear, int startMonth, int startDay,
            int endYear, int endMonth, int endDay)
        {
            bool yearsSame = startYear == endYear;
            bool monthsSame = startMonth == endMonth;
            bool wholeMonths = (startDay == 1 && IsLastDay(endDay, endMonth, endYear));

            if (monthsSame && yearsSame && startDay == endDay)
            {
                return string.Format("{0} {1}, {2}", startDay, Month(startMonth), startYear);
            }

            if (monthsSame)
            {
                if (yearsSame)
                {
                    return wholeMonths
                               ? string.Format("{0} {1}", Month(startMonth), endYear)
                               : string.Format("{1}. - {2}. {0} {3}", Month(endMonth), startDay, endDay, endYear);
                }
                return wholeMonths
                           ? string.Format("{0} {1} - {2} {3}",
                                           Month(startMonth), startYear,
                                           Month(endMonth), endYear)
                           : string.Format("{1}. {0} {2} - {4}. {3} {5}",
                                           Month(startMonth), startDay, startYear,
                                           Month(endMonth), endDay, endYear);
            }

            if (yearsSame)
            {
                return wholeMonths
                           ? string.Format("{0} - {1} {2}", Month(startMonth), Month(endMonth), endYear)
                           : string.Format("{1}. {0} - {3}. {2} {4}",
                                           Month(startMonth), startDay,
                                           Month(endMonth), endDay,
                                           endYear);
            }
            return wholeMonths
                       ? string.Format("{0} {1} - {2} {3}",
                                       Month(startMonth), startYear,
                                       Month(endMonth), endYear)
                       : string.Format("{1}. {0} {2} - {4}. {3} {5}",
                                       Month(startMonth), startDay, startYear,
                                       Month(endMonth), endDay, endYear);
        }

        private static string Month(int month)
        {
            return Months[month - 1];
        }

        public static bool IsLastDay(int day, int month, int year)
        {
            switch (month + 1)
            {
                case 2:
                    if (DateTime.IsLeapYear(year))
                        return (day == 29);
                    else
                        return (day == 28);
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return (day == 31);
                case 4:
                case 6:
                case 9:
                case 11:
                    return (day == 30);
                default:
                    return false;
            }
        }

        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
    }
}