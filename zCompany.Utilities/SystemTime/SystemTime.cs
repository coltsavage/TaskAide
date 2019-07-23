using System;

namespace zCompany.Utilities
{
    public class SystemTime : ISystemTime
    {
        // Properties
        public TimeZoneInfo LocalTimeZone
        {
            get => TimeZoneInfo.Local;
        }

        public DateTimeOffset UtcNow
        {
            get => DateTimeOffset.UtcNow;
        }
    }
}
