﻿using System;
using System.ComponentModel;

namespace Humanizer
{
    [Localizable(true)]
    public static class DateHumanizeExtensions
    {
        // http://stackoverflow.com/questions/11/how-do-i-calculate-relative-time
        /// <summary>
        /// Turns the current or provided date into a human readable sentence
        /// </summary>
        /// <param name="input">The date to be humanized</param>
        /// <param name="utcDate">Boolean value indicating whether the date is in UTC or local</param>
        /// <param name="dateToHumanize">Date to humanize. The current date is humanized if this value is null</param>
        /// <returns></returns>
        public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToHumanize = null)
        {
            if (dateToHumanize == null)
                dateToHumanize = DateTime.UtcNow;
            
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            const int month = 30 * day;

            var comparisonBase = dateToHumanize.Value;
            if (!utcDate)
                comparisonBase = comparisonBase.ToLocalTime();

            if (input > comparisonBase)
                return Resources.DateExtensions_FutureDate_not_yet;

            var ts = new TimeSpan(comparisonBase.Ticks - input.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * minute)
                return ts.Seconds == 1 ? Resources.DateExtensions_OneSecondAgo_one_second_ago : string.Format(Resources.DateExtensions_SecondsAgo__seconds_ago, ts.Seconds);

            if (delta < 2 * minute)
                return Resources.DateExtensions_OneMinuteAgo_a_minute_ago;

            if (delta < 45 * minute)
                return string.Format(Resources.DateExtensions_MinutesAgo__minutes_ago, ts.Minutes);

            if (delta < 90 * minute)
                return Resources.DateExtensions_OneHourAgo_an_hour_ago;

            if (delta < 24 * hour)
                return string.Format(Resources.DateExtensions_HoursAgo__hours_ago, ts.Hours);

            if (delta < 48 * hour)
                return Resources.DateExtensions_Yesterday_yesterday;

            if (delta < 30 * day)
                return string.Format(Resources.DateExtensions_DaysAgo__days_ago, ts.Days);

            if (delta < 12 * month)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? Resources.DateExtensions_OneMonthAgo_one_month_ago : string.Format(Resources.DateExtensions_MonthsAgo__months_ago, months);
            }

            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? Resources.DateExtensions_OneYearAgo_one_year_ago : string.Format(Resources.DateExtensions_YearsAgo__years_ago, years);
        }
    }
}