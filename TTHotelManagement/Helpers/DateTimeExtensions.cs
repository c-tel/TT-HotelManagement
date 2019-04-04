using System;

namespace TTHotel.API.Helpers
{
    public static class DateTimeExtensions
    {
        public static string ToPostgresDateFormat(this DateTime dateTime)
        {
            return $"'{dateTime.ToString("yyyy-MM-dd")}'";
        }
        public static string ToPostgresTimestampFormat(this DateTime dateTime)
        {
            return $"'{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}'";
        }
    }
}
