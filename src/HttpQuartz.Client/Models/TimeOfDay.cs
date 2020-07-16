using System;

namespace HttpQuartz.Client.Models
{
    [Serializable]
    public class TimeOfDayInfo
    {
        /// <summary>
        /// Create a TimeOfDay instance for the given hour, minute and second.
        /// </summary>
        /// <param name="hour">The hour of day, between 0 and 23.</param>
        /// <param name="minute">The minute of the hour, between 0 and 59.</param>
        /// <param name="second">The second of the minute, between 0 and 59.</param>
        public TimeOfDayInfo(int hour, int minute, int second)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
            this.Validate();
        }

        private void Validate()
        {
            if (this.Hour < 0 || this.Hour > 23)
                throw new ArgumentException("Hour must be from 0 to 23");
            if (this.Minute < 0 || this.Minute > 59)
                throw new ArgumentException("Minute must be from 0 to 59");
            if (this.Second < 0 || this.Second > 59)
                throw new ArgumentException("Second must be from 0 to 59");
        }

        /// <summary>
        /// 每天的小时数( 0 到 23).
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 每小时的分钟数( 0 到 59)
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// 每分钟的秒数( 0 到 59).
        /// </summary>

        public int Second { get; set; }
    }
}