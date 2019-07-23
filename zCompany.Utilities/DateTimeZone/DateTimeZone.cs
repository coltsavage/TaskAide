using System;

namespace zCompany.Utilities
{
    public class DateTimeZone : IDateTimeZone
    {
        // Fields
        private DateTimeOffset dateTimeUtc;
        private TimeZoneInfo localTimeZoneWhenCreated;

        // Constructors
        public DateTimeZone(DateTimeOffset dateTimeUtc, TimeZoneInfo localTimeZone)
        {
            this.dateTimeUtc = dateTimeUtc;
            this.localTimeZoneWhenCreated = localTimeZone;
        }

        // Properties
        public TimeSpan LocalTime
        {
            get => dateTimeUtc.ToLocalTime().TimeOfDay;
        }

        public string InitialTimeZoneId
        {
            get => this.localTimeZoneWhenCreated.Id;
        }

        public long UtcTicks
        {
            get => this.dateTimeUtc.UtcTicks;
        }

        // Methods
        public IDateTimeZone Add(TimeSpan timeSpan)
        {
            return new DateTimeZone(this.dateTimeUtc.Add(timeSpan), localTimeZoneWhenCreated);
        }

        public string LocalTimeString(string format)
        {
            return this.dateTimeUtc.ToLocalTime().ToString(format);
        }
    }
}
