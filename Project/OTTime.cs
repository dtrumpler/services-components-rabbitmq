using System;
using System.Collections.Generic;

namespace OpenTable.Services.Components.Common
{
    public static class OTTime
    {
        // Complete list of OT time zones ids as stored in WebDb, 3/2014
        private static readonly Dictionary<int, string> TimeZoneIdToName = new Dictionary<int, string>()
            {
                {1, "Samoa Standard Time"},
                {2, "Hawaiian Standard Time"},
                {3, "Alaskan Standard Time"},
                {4, "Pacific Standard Time"},
                {5, "US Mountain Standard Time"},
                {6, "Mountain Standard Time"},
                {7, "Central Standard Time"},
                {8, "Eastern Standard Time"},
                {9, "US Eastern Standard Time"},
                {10, "Atlantic Standard Time"},
                {11, "Newfoundland Standard Time"},
                {12, "Argentina Standard Time"},
                {13, "SA Eastern Standard Time"},
                {14, "South Africa Standard Time"},
                {15, "GMT Standard Time"},
                {16, "Central Europe Standard Time"},
                {17, "E. Europe Standard Time"},
                {18, "Egypt Standard Time"},
                {19, "E. Africa Standard Time"},
                {20, "Iran Standard Time"},
                {21, "Arabian Standard Time"},
                {22, "West Asia Standard Time"},
                {23, "India Standard Time"},
                {24, "Central Asia Standard Time"},
                {25, "SE Asia Standard Time"},
                {26, "China Standard Time"},
                {27, "Tokyo Standard Time"},
                {28, "Cen. Australia Standard Time"},
                {29, "AUS Eastern Standard Time"},
                {30, "Central Pacific Standard Time"},
                {31, "New Zealand Standard Time"},
                {32, "Central Standard Time (Mexico)"},
                {33, "SA Western Standard Time"},
                {34, "Central America Standard Time"},
                {35, "Mountain Standard Time (Mexico)"},
                {36, "Pacific Standard Time (Mexico)"},
                {37, "W. Europe Standard Time"},
                // As of 4/14/2014, the following time zone is mislabeled in WebDB as "Western Caribbean Standard Time"
                {38, "SA Pacific Standard Time"}
            };

        private static string MapFrom(int tzid)
        {
            if (!TimeZoneIdToName.ContainsKey(tzid))
                throw new ArgumentOutOfRangeException(string.Format("Invalid OT TimeZone value {0}", tzid));

            return TimeZoneIdToName[tzid];
        }

        public static TimeZoneInfo GetTimeZoneInfo(int timeZoneId)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(MapFrom(timeZoneId));
        }

        // Convert local datetime corresponding to given tz id to a DateTimeOffset appropriate for the location
        public static DateTimeOffset ConvertToDateTimeOffset(DateTime dateTime, int timeZoneId)
        {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);

            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(MapFrom(timeZoneId));

            DateTime utc = TimeZoneInfo.ConvertTimeToUtc(dateTime, tzi);

            DateTimeOffset dtoUtc = new DateTimeOffset(utc);

            TimeSpan tsZone = tzi.GetUtcOffset(dtoUtc);

            return dtoUtc.ToOffset(tsZone);
        }

        // Convert local datetime corresponding to given tz id to a UTC datetime
        public static DateTime ConvertToUtc(DateTime dateTime, int timeZoneId)
        {
            DateTimeOffset dto = ConvertToDateTimeOffset(dateTime, timeZoneId);
            return dto.ToUniversalTime().DateTime;
        }

        // Convert UTC datetime to equivalent time in the time zone specified
        public static DateTime ConvertFromUtc(DateTime dateTimeUtc, int timeZoneId)
        {
            dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(MapFrom(timeZoneId));
            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, tzi);
        }

        // Return current time in the specified time zone
        public static DateTime GetLocalNow(int timeZoneId)
        {
            return ConvertFromUtc(SystemTime.UtcNow(), timeZoneId);
        }

        // Render local datetime corresponding to given tz id as an ISO 8601 string
        public static string ToIsoDateTimeString(DateTime dateTime, int timeZoneId, bool withSeconds = false)
        {
            return ConvertToDateTimeOffset(dateTime, timeZoneId).ToString(
                withSeconds ? "yyyy-MM-ddTHH:mm:sszzz" : "yyyy-MM-ddTHH:mmzzz");
        }
    }
}
