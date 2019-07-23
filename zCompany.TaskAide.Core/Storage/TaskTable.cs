using System;
using System.Collections.Generic;

namespace zCompany.TaskAide
{
    internal class TaskTable : DatabaseTable<Task>
    {
        // Fields
        private bool gettingTaskList;

        // Constructors
        public TaskTable(string tableName, Database db)
            : base(tableName, db)
        {

        }

        // Properties
        public override Database.TableColumn[] Columns
        {
            get => new[]
            {
                TaskTable.Column.TaskId,
                TaskTable.Column.Name
            };
        }

        // Methods
        public List<Task> GetTaskList()
        {
            this.gettingTaskList = true;
            var list = base.Get(new[] { TaskTable.Column.TaskId, TaskTable.Column.Name });
            this.gettingTaskList = false;
            return list;
        }

        public bool UpdateName(Task task)
        {
            var nameField = new Database.TableField(TaskTable.Column.Name, task.Name);
            return base.Update(task.TID.ToString(), nameField);
        }

        // Helpers
        protected override Task ConvertFromRow(string[] row)
        {
            Task instance = null;
            if (row != null)
            {
                instance = new Task(Convert.ToInt32(row[0]), row[1]);
                if (!this.gettingTaskList)
                {
                    // add remaining fields
                }
            }
            return instance;
        }

        protected override Database.TableField[] ConvertToRow(Task instance)
        {
            return new[]
            {
                new Database.TableField(TaskTable.Column.TaskId, instance.TID.ToString()),
                new Database.TableField(TaskTable.Column.Name, instance.Name)
            };
        }

        protected override Database.TableField CreatePrimaryKeyField(string key)
        {
            return new Database.TableField(TaskTable.Column.TaskId, key);
        }

        // Structs
        public struct Column
        {
            public static readonly Database.TableColumn Actual =
                new Database.TableColumn("Actual", Database.DataType.Int);

            public static readonly Database.TableColumn Estimate =
                new Database.TableColumn("Estimate", Database.DataType.Int);

            public static readonly Database.TableColumn Name =
                new Database.TableColumn("Name", Database.DataType.String);

            public static readonly Database.TableColumn ProjectId =
                new Database.TableColumn("ProjectId", Database.DataType.Int);

            public static readonly Database.TableColumn Tags =
                new Database.TableColumn("Tags", Database.DataType.Int);

            public static readonly Database.TableColumn TaskId =
                new Database.TableColumn("Id", Database.DataType.Int);
        }
    }
}
