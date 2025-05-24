using System;

namespace TyrDK
{
    public static class DateTimeExtension
    {
        public static string GetFormattedTimeSpan(this TimeSpan dateTime)
        {
            return dateTime.Days > 0
                ? $"{dateTime.Days}d {dateTime.Hours}h {dateTime.Minutes}m"
                : dateTime.Hours > 0
                    ? $"{dateTime.Hours}h {dateTime.Minutes}m"
                    : dateTime.Minutes > 0
                        ? $"{dateTime.Minutes}m"
                        : "";
        }
        
        public static string GetMinutes(this TimeSpan dateTime)
        {
            return dateTime.Days > 0
                ? $"{dateTime.Days}d {dateTime.Hours}h {dateTime.Minutes}m"
                : dateTime.Hours > 0
                    ? $"{dateTime.Hours}h {dateTime.Minutes}m"
                    : dateTime.Minutes > 0
                        ? $"{dateTime.Minutes}m"
                        : "";
        }
    }
}