using System;
using System.Collections.Generic;

namespace zCompany.TaskAide
{
    internal class IntervalTable : DatabaseTable<Interval>
    {
        // Constructors
        public IntervalTable(string tableName, Database db)
            : base(tableName, db)
        {

        }

        // Properties
        public override Database.TableColumn[] Columns
        {
            get => new[]
            {
                IntervalTable.Column.IntervalId,
                IntervalTable.Column.Start,
                IntervalTable.Column.Span,
                IntervalTable.Column.TaskId,
                IntervalTable.Column.SessionId
            };
        }

        // Methods
        public List<Interval> GetSession(string sessionId)
        {
            var field = new Database.TableField(IntervalTable.Column.SessionId, sessionId);
            return base.Get(Database.Comparison.EqualTo, field);
        }

        public bool UpdateSpan(Interval interval)
        {
            var field = new Database.TableField(IntervalTable.Column.Span, interval.Span.TotalSeconds.ToString());
            return base.Update(interval.IID.ToString(), field);
        }

        // Helpers
        protected override Interval ConvertFromRow(string[] row)
        {
            Interval instance = null;
            if (row != null)
            {
                instance = new Interval(Convert.ToInt32(row[0]), Convert.ToInt32(row[3]), Convert.ToInt32(row[4]));
                instance.Start = new TimeSpan(0, 0, Convert.ToInt32(row[1]));
                instance.Span = new TimeSpan(0, 0, Convert.ToInt32(row[2]));
            }
            return instance;
        }

        protected override Database.TableField[] ConvertToRow(Interval instance)
        {
            return new[]
            {
                new Database.TableField(IntervalTable.Column.IntervalId, instance.IID.ToString()),
                new Database.TableField(IntervalTable.Column.Start, instance.Start.TotalSeconds.ToString()),
                new Database.TableField(IntervalTable.Column.Span, instance.Span.TotalSeconds.ToString()),
                new Database.TableField(IntervalTable.Column.TaskId, instance.TaskId.ToString()),
                new Database.TableField(IntervalTable.Column.SessionId, instance.SessionId.ToString())
            };
        }

        protected override Database.TableField CreatePrimaryKeyField(string key)
        {
            return new Database.TableField(IntervalTable.Column.IntervalId, key);
        }

        // Structs
        public struct Column
        {
            public static readonly Database.TableColumn IntervalId =
                new Database.TableColumn("Id", Database.DataType.Int);

            public static readonly Database.TableColumn SessionId =
                new Database.TableColumn("SessionId", Database.DataType.Int);

            public static readonly Database.TableColumn Span =
                new Database.TableColumn("Span", Database.DataType.Int);

            public static readonly Database.TableColumn Start =
                new Database.TableColumn("Start", Database.DataType.Int);

            public static readonly Database.TableColumn TaskId =
                new Database.TableColumn("TaskId", Database.DataType.Int);
        }
    }
}
