using System;
using System.Collections.Generic;
using Xunit;

namespace zCompany.TaskAide.Tests
{
    public class DatabaseTests : IClassFixture<DatabaseTestsFixture>
    {
        // Fields
        private Database db;
        private DatabaseTestsFixture dbMeta;
        private Database.TableColumn idColumn;
        private Database.TableColumn nameColumn;
        private List<string[]> peopleData;
        private string tableName;

        // Constructors
        public DatabaseTests(DatabaseTestsFixture fixture)
        {
            this.db = fixture.Db;
            this.dbMeta = fixture;
            this.idColumn = fixture.IdColumn;
            this.nameColumn = fixture.NameColumn;
            this.peopleData = fixture.PeopleData;
            this.tableName = fixture.TableName;
        }

        // Destructors
        ~DatabaseTests()
        {

        }

        // Tests
        [Fact]
        public void BAT()
        {
            // Configure
            var person = new[] { "99", "Willow" };

            // Setup
            var personFields = this.CreatePersonFields(person);

            // Test: Add
            bool success = this.db.Add(this.tableName, personFields);
            Assert.True(success);

            // Test: Get
            var results = this.db.Get(this.tableName, Database.Comparison.EqualTo, personFields[0]);
            Assert.Single(results);
            Assert.Equal(person[0], results[0][0]);
            Assert.Equal(person[1], results[0][1]);

            // Test: Remove
            success = this.db.Remove(this.tableName, personFields[0]);
            Assert.True(success);

            // Confirm (removal)
            results = this.db.Get(this.tableName, Database.Comparison.EqualTo, personFields[0]);
            Assert.Empty(results);
        }

        [Fact]
        public void Add_EntryExists()
        {
            // Configure
            int index = 1;

            // Setup
            var person = this.peopleData[index];
            var personFields = this.CreatePersonFields(person);

            // Test
            bool success = this.db.Add(this.tableName, personFields);
            Assert.False(success);
        }

        [Fact]
        public void CreateDestroyTable()
        {
            // Configure
            string tableName = "garbageTable";
            var columns = new[] { this.idColumn, this.nameColumn };

            // Setup

            // Test
            var found = this.db.DestroyTable(tableName);
            Assert.False(found);

            this.db.CreateTableIfNotExists(tableName, columns);

            found = this.db.DestroyTable(tableName);
            Assert.True(found);

            found = this.db.DestroyTable(tableName);
            Assert.False(found);
        }

        [Fact]
        public void DestroyAllTables()
        {
            // Configure
            var tableNames = new[] { "garbageTable1", "garbageTable2", "garbageTable3" };
            var columns = new[] { this.idColumn, this.nameColumn };

            // Setup
            foreach (string name in tableNames)
            {
                this.db.CreateTableIfNotExists(name, columns);
            }

            // Test
            this.db.DestroyAllTables();

            // Confirm
            var found = this.db.DestroyTable(this.tableName);
            Assert.False(found);

            foreach (string name in tableNames)
            {
                found = this.db.DestroyTable(name);
                Assert.False(found);
            }

            // Restore
            this.dbMeta.Reset();
        }

        [Fact]
        public void Get_All()
        {
            // Configure

            // Setup

            // Test
            var results = this.db.Get(this.tableName);
            Assert.Equal(this.peopleData.Count, results.Count);

            // Confirm
            foreach (var row in results)
            {
                var foundPerson = this.peopleData.Find((p) => p[0] == row[0]);
                Assert.NotNull(foundPerson);
                Assert.Equal(foundPerson[1], row[1]);
            }
        }

        [Fact]
        public void Get_ColumnSubset()
        {
            // Configure
            var columns = new[] { this.idColumn };

            // Setup

            // Test
            var results = this.db.Get(this.tableName, columns);
            Assert.Equal(this.peopleData.Count, results.Count);

            // Confirm
            foreach (var row in results)
            {
                Assert.Single(row);
                var foundPerson = this.peopleData.Find((p) => p[0] == row[0]);
                Assert.NotNull(foundPerson);
            }
        }

        [Fact]
        public void Get_EqualToMultiple()
        {
            // Configure
            int index = 2;
            var op = Database.Comparison.EqualTo;
            int expectedResultsRows = 2;

            // Setup
            var person = this.peopleData[index];
            var personFields = this.CreatePersonFields(person);

            // Test
            var results = this.db.Get(this.tableName, op, personFields[1]);
            Assert.Equal(expectedResultsRows, results.Count);

            // Confirm
            foreach (var row in results)
            {
                var foundPerson = this.peopleData.Find((p) => p[0] == row[0]);
                Assert.NotNull(foundPerson);
                Assert.Equal(foundPerson[1], row[1]);
            }
        }

        [Fact]
        public void Get_GreaterThanMultiple()
        {
            // Configure
            int index = 1;
            var op = Database.Comparison.GreaterThan;
            int expectedResultsRows = this.peopleData.Count - index - 1;

            // Setup
            var person = this.peopleData[index];
            var personFields = this.CreatePersonFields(person);

            // Test
            var results = this.db.Get(this.tableName, op, personFields[1]);
            Assert.Equal(expectedResultsRows, results.Count);

            // Confirm
            foreach (var row in results)
            {
                var foundPerson = this.peopleData.Find((p) => p[0] == row[0]);
                Assert.NotNull(foundPerson);
                Assert.Equal(foundPerson[1], row[1]);
            }
        }

        [Fact]
        public void Get_LessThanMultiple()
        {
            // Configure
            int index = 2;
            var op = Database.Comparison.LessThan;
            int expectedResultsRows = 2;

            // Setup
            var person = this.peopleData[index];
            var personFields = this.CreatePersonFields(person);

            // Test
            var results = this.db.Get(this.tableName, op, personFields[1]);
            Assert.Equal(expectedResultsRows, results.Count);

            // Confirm
            foreach (var row in results)
            {
                var foundPerson = this.peopleData.Find((p) => p[0] == row[0]);
                Assert.NotNull(foundPerson);
                Assert.Equal(foundPerson[1], row[1]);
            }
        }

        [Fact]
        public void Get_NoResult()
        {
            // Configure
            var person = new[] { "100", "Zack" };
            var op = Database.Comparison.EqualTo;

            // Setup
            var personFields = this.CreatePersonFields(person);

            // Test
            var results = this.db.Get(this.tableName, op, personFields[1]);
            Assert.Empty(results);
        }

        [Fact]
        public void Remove_NotFound()
        {
            // Configure
            var person = new[] { "100", "Zack" };

            // Setup
            var personFields = this.CreatePersonFields(person);

            // Test
            var success = this.db.Remove(this.tableName, personFields[0]);
            Assert.False(success);
        }

        [Fact]
        public void UpdateField()
        {
            // Configure
            int index = 1;
            string newName = "newName";

            // Setup
            var person = this.peopleData[index];
            var personFields = this.CreatePersonFields(person);
            var newNameField = new Database.TableField(this.nameColumn, newName);

            // Test
            bool success = this.db.UpdateField(this.tableName, personFields[0], newNameField);
            Assert.True(success);

            // Confirm
            var results = this.db.Get(this.tableName, Database.Comparison.EqualTo, personFields[0]);
            Assert.Single(results);
            Assert.Equal(person[0], results[0][0]);
            Assert.Equal(newName, results[0][1]);

            // Restore
            success = this.db.UpdateField(this.tableName, personFields[0], personFields[1]);
            Assert.True(success);
        }

        [Fact]
        public void UpdateField_NotFound()
        {
            // Configure
            var person = new[] { "100", "Zack" };
            string newName = "newName";

            // Setup
            var personFields = this.CreatePersonFields(person);
            var newNameField = new Database.TableField(this.nameColumn, newName);

            // Test
            bool success = this.db.UpdateField(this.tableName, personFields[0], newNameField);
            Assert.False(success);
        }

        // Helpers
        private Database.TableField[] CreatePersonFields(string[] person)
        {
            return new[]
            {
                new Database.TableField(this.idColumn, person[0]),
                new Database.TableField(this.nameColumn, person[1])
            };
        }
    }
}
