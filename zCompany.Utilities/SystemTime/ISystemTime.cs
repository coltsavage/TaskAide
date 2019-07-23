using System;

namespace zCompany.Utilities
{
    public interface ISystemTime
    {
        // Properties
        TimeZoneInfo LocalTimeZone { get; }

        DateTimeOffset UtcNow { get; }
    }
}
