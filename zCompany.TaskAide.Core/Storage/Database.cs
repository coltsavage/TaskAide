using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Text;

namespace zCompany.TaskAide
{
    public class Database
    {
        // Constructors
        private Database() { }

        public Database(string databaseFilename)
        {
            this.Db = new SqliteConnection(databaseFilename);
        }

        // Destructors
        ~Database()
        {
            this.Db?.Dispose();
        }

        // Properties
        private SqliteConnection Db { get; set; }

        // Methods
        public bool Add(string tableName, Database.TableField[] rowData)
        {
            StringBuilder command = new StringBuilder();
            command.Append($"INSERT INTO {tableName} VALUES ({rowData[0].FormattedValue()}");
            for (int i = 1; i < rowData.Length; i++)
            {
                command.Append($", {rowData[i].FormattedValue()}");
            }
            command.Append(")");

            int rowsImpacted = 0;
            try
            {
                this.Db.Open();
                rowsImpacted = new SqliteCommand(command.ToString(), this.Db).ExecuteNonQuery();
                this.Db.Close();
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 19)
                {
                    // constraint violation
                    // expected when adding a row that already exists
                    // (eg. fails primary key uniqueness contraint)
                }
                else { throw e; }
            }

            return rowsImpacted == 1;
        }

        public int CreateTableIfNotExists(string tableName, TableColumn[] columns)
        {
            StringBuilder command = new StringBuilder();
            command.Append($"CREATE TABLE IF NOT EXISTS {tableName} ({columns[0].Text} {columns[0].Type} PRIMARY KEY");
            for (int i = 1; i < columns.Length; i++)
            {
                command.Append($", {columns[i].Text} {columns[i].Type}");
            }
            command.Append(")");

            this.Db.Open();
            var result = new SqliteCommand(command.ToString(), this.Db).ExecuteNonQuery();
            this.Db.Close();

            return result;
        }

        public void DestroyAllTables()
        {
            string command = $"SELECT name FROM sqlite_master WHERE type='table'";

            this.Db.Open();
            SqliteDataReader results = new SqliteCommand(command, this.Db).ExecuteReader();
            List<string[]> tableNames = this.ConvertToListOfRowFieldStrings(results);
            this.Db.Close();

            foreach (string[] name in tableNames)
            {
                this.DestroyTable(name[0]);
            }
        }

        public bool DestroyTable(string tableName)
        {
            string command = $"DROP TABLE {tableName}";

            bool found = true;
            try
            {
                this.Db.Open();
                new SqliteCommand(command.ToString(), this.Db).ExecuteNonQuery();
                this.Db.Close();
            }
            catch (SqliteException e)
            {
                if (e.SqliteErrorCode == 1)
                {
                    // no such table
                    found = false;
                }
                else { throw e; }
            }
            return found;
        }

        public List<string[]> Get(string tableName)
        {
            return this.Get(tableName, null, null, null);
        }

        public List<string[]> Get(string tableName, Database.TableColumn[] columns)
        {
            return this.Get(tableName, null, null, columns);
        }

        public List<string[]> Get(string tableName, Database.Comparison op, Database.TableField field)
        {
            return this.Get(tableName, op, field, null);
        }

        public List<string[]> Get(string tableName, Database.Comparison op, Database.TableField? field, Database.TableColumn[] columns)
        {
            string command = this.BuildGetCommandString(tableName, op, field, columns);

            this.Db.Open();
            SqliteDataReader results = new SqliteCommand(command, this.Db).ExecuteReader();
            List<string[]> rowsData = this.ConvertToListOfRowFieldStrings(results);
            this.Db.Close();

            return rowsData;
        }

        public bool Remove(string tableName, Database.TableField pKey)
        {
            string command = $"DELETE FROM {tableName} WHERE {pKey.Column.Text} == {pKey.FormattedValue()}";

            this.Db.Open();
            int rowsImpacted = new SqliteCommand(command, this.Db).ExecuteNonQuery();            
            this.Db.Close();

            return rowsImpacted == 1;
        }

        public bool UpdateField(string tableName, Database.TableField pKey, Database.TableField update)
        {
            string command = $"UPDATE {tableName} SET {update.Column.Text} = {update.FormattedValue()} WHERE {pKey.Column.Text} == {pKey.FormattedValue()}";

            this.Db.Open();
            int rowsImpacted = new SqliteCommand(command, this.Db).ExecuteNonQuery();
            this.Db.Close();

            return rowsImpacted == 1;
        }

        // Helpers
        private string BuildGetCommandString(string tableName, Database.Comparison op, Database.TableField? field, Database.TableColumn[] columns)
        {
            StringBuilder command = new StringBuilder();
            if ((columns == null) || (columns.Length == 0))
            {
                command.Append($"SELECT *");
            }
            else
            {
                command.Append($"SELECT {columns[0].Text}");
                for (int i = 1; i < columns.Length; i++)
                {
                    command.Append($", {columns[i].Text}");
                }
            }
            command.Append($" FROM {tableName}");
            if (op != null)
            {
                command.Append($" WHERE {field?.Column.Text} {op} {field?.FormattedValue()}");
            }
            return command.ToString();
        }

        private List<string[]> ConvertToListOfRowFieldStrings(SqliteDataReader results)
        {
            List<string[]> rowsData = new List<string[]>();
            if (results.HasRows)
            {
                while (results.Read())
                {
                    var rowData = new string[results.FieldCount];
                    for (int i = 0; i < results.FieldCount; i++)
                    {
                        rowData[i] = results.GetString(i);
                    }
                    rowsData.Add(rowData);
                }
            }
            return rowsData;
        }

        private List<string[]> GetListOfTables(SqliteConnection db)
        {
            string command = $"SELECT name FROM sqlite_master WHERE type='table'";

            db.Open();
            SqliteDataReader results = new SqliteCommand(command, db).ExecuteReader();

            List<string[]> tableNames = this.ConvertToListOfRowFieldStrings(results);
            //var tableNames = new List<string>();
            //if (results.HasRows)
            //{
            //    while (results.Read())
            //    {
            //        tableNames.Add(results.GetString(0));
            //    }
            //}

            db.Close();

            return tableNames;
        }

        // Structs
        public struct TableColumn
        {
            public readonly string Text;
            public readonly Database.DataType Type;

            public TableColumn(string text, Database.DataType type)
            {
                this.Text = text;
                this.Type = type;
            }
        }

        public struct TableField
        {
            public readonly TableColumn Column;
            public readonly string Value;

            public TableField(TableColumn column, string value)
            {
                this.Column = column;
                this.Value = value;
            }

            internal string FormattedValue()
            {
                return this.Column.Type.PackageValueForDatabase(this.Value);
            }
        }

        // Classes
        public class DataType
        {
            // Class Fields
            private static readonly DataType integer = new DataType("INT");
            private static readonly DataType real = new DataType("REAL");
            private static readonly DataType text = new DataType("TEXT");

            // Class Properties
            public static DataType Double { get => DataType.real; }
            public static DataType Int { get => DataType.integer; }
            public static DataType String { get => DataType.text; }

            // Fields
            private readonly string dataType;

            // Constructors
            public DataType(string dataType)
            {
                this.dataType = dataType;
            }

            // Methods
            public string PackageValueForDatabase(string value)
            {
                return (this == DataType.String) ? $"'{value}'" : value;
            }

            public override string ToString()
            {
                return this.dataType;
            }
        }

        public class Comparison
        {
            // Class Fields
            private static readonly Comparison equal = new Comparison("==");
            private static readonly Comparison greater = new Comparison(">");
            private static readonly Comparison lesser = new Comparison("<");

            // Class Properties
            public static Comparison EqualTo { get => Comparison.equal; }
            public static Comparison GreaterThan { get => Comparison.greater; }
            public static Comparison LessThan { get => Comparison.lesser; }

            // Fields
            private readonly string op;

            // Constructors
            private Comparison(string op)
            {
                this.op = op;
            }

            // Methods
            public override string ToString()
            {
                return this.op;
            }
        }
    }
}
