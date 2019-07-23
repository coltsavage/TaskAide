using System;
using zCompany.Utilities;

namespace zCompany.TaskAide
{
    internal class SessionTable : DatabaseTable<Session>
    {
        // Constructors
        public SessionTable(string tableName, Database db)
            : base(tableName, db)
        {

        }

        // Properties
        public override Database.TableColumn[] Columns
        {
            get => new[]
            {
                SessionTable.Column.SessionId,
                SessionTable.Column.Date,
                SessionTable.Column.TimeZone
            };
        }

        // Methods
        public Session FindRecent(IDateTimeZone dateTime)
        {
            Session session = null;
            var recent = new Database.TableField(SessionTable.Column.Date, dateTime.UtcTicks.ToString());
            var sessions = base.Get(Database.Comparison.GreaterThan, recent);
            if (sessions.Count > 0)
            {
                sessions.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
                session = sessions[sessions.Count - 1];
            }
            return session;
        }

        // Helpers
        protected override Session ConvertFromRow(string[] row)
        {
            Session instance = null;
            if (row != null)
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(row[2]);
                var dateTime = new DateTimeOffset(Convert.ToInt64(row[1]), TimeSpan.Zero);
                instance = new Session(Convert.ToInt32(row[0]), new DateTimeZone(dateTime, timeZone));
            }
            return instance;
        }

        protected override Database.TableField[] ConvertToRow(Session instance)
        {
            return new[]
            {
                new Database.TableField(SessionTable.Column.SessionId, instance.SID.ToString()),
                new Database.TableField(SessionTable.Column.Date, instance.Date.UtcTicks.ToString()),
                new Database.TableField(SessionTable.Column.TimeZone, instance.Date.InitialTimeZoneId),
            };
        }

        protected override Database.TableField CreatePrimaryKeyField(string key)
        {
            return new Database.TableField(SessionTable.Column.SessionId, key);
        }

        // Structs
        public struct Column
        {
            public static readonly Database.TableColumn Date =
                new Database.TableColumn("Date", Database.DataType.Int);

            public static readonly Database.TableColumn SessionId =
                new Database.TableColumn("Id", Database.DataType.Int);

            public static readonly Database.TableColumn TimeZone =
                new Database.TableColumn("TimeZone", Database.DataType.String);
        }
    }
}
