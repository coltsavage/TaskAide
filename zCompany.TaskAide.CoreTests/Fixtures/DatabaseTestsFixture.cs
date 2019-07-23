using System;
using System.Collections.Generic;

namespace zCompany.TaskAide.Tests
{
    public class DatabaseTestsFixture : IDisposable
    {
        // Constructors
        public DatabaseTestsFixture()
        {
            this.Db = new Database("Filename=peopleTestDb.db");

            this.TableName = "People";
            this.IdColumn = new Database.TableColumn("Id", Database.DataType.Int);
            this.NameColumn = new Database.TableColumn("Name", Database.DataType.String);

            this.PeopleData = new List<string[]>();
            this.PeopleData.Add(new[] { "1", "Alice" });
            this.PeopleData.Add(new[] { "2", "Bill" });
            this.PeopleData.Add(new[] { "3", "Charlie" });
            this.PeopleData.Add(new[] { "4", "Charlie" });
            this.PeopleData.Add(new[] { "5", "Dennie" });

            this.Populate();
        }

        // Destructors
        public void Dispose()
        {
            this.Db.DestroyTable(this.TableName);
        }

        // Properties
        internal Database Db { get; private set; }

        internal Database.TableColumn IdColumn { get; private set; }

        internal Database.TableColumn NameColumn { get; private set; }

        internal List<string[]> PeopleData { get; private set; }

        internal string TableName { get; private set; }

        // Methods
        internal void Reset()
        {
            this.Db.DestroyTable(this.TableName);
            this.Populate();
        }

        // Helpers
        private void Populate()
        {
            this.Db.CreateTableIfNotExists(this.TableName, new[] { this.IdColumn, this.NameColumn });

            foreach (var person in this.PeopleData)
            {
                var rowData = new[]
                {
                    new Database.TableField(this.IdColumn, person[0]),
                    new Database.TableField(this.NameColumn, person[1])
                };
                this.Db.Add(this.TableName, rowData);
            }
        }
    }
}
