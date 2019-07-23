using System;

namespace zCompany.Utilities
{
    public interface IDateTimeZone
    {
        // Properties
        TimeSpan LocalTime { get; }

        string InitialTimeZoneId { get; }

        long UtcTicks { get; }

        // Methods
        IDateTimeZone Add(TimeSpan time);

        string LocalTimeString(string format);
    }
}
