using System;
using zCompany.Utilities;

namespace zCompany.TaskAide.Tests
{
    public class EmptyDatabaseFixture : IDisposable
    {
        // Constructors
        public EmptyDatabaseFixture()
        {
            this.Db = new Database("Filename=testFullDb.db");
            
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            var dateTimeLocal = new DateTimeOffset(2019, 7, 13, 8, 22, 35, timeZone.BaseUtcOffset);

            this.SystemTime = new SystemTimeDev(dateTimeLocal.ToUniversalTime(), timeZone);
            this.Timer = new TimerDev(this.SystemTime);
        }

        // Destructors
        public void Dispose()
        {
            this.Db.DestroyAllTables();
        }

        // Properties
        internal Database Db { get; private set; }

        internal SystemTimeDev SystemTime { get; private set; }

        internal TimerDev Timer { get; private set; }
    }
}
