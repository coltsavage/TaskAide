using System;
using System.Collections.Generic;

namespace zCompany.TaskAide
{
    internal abstract class DatabaseTable<T>
    {
        // Fields
        private Database db;

        // Constructors
        private DatabaseTable() { }

        public DatabaseTable(string tableName, Database db)
        {
            this.TableName = tableName;
            this.db = db;

            this.db.CreateTableIfNotExists(this.TableName, this.Columns);
        }

        // Destructors
        ~DatabaseTable()
        {
            
        }

        // Properties
        public abstract Database.TableColumn[] Columns { get; }

        public string TableName { get; private set; }

        // Methods
        public bool Add(T instance)
        {            
            return this.db.Add(this.TableName, this.ConvertToRow(instance));
        }

        public T Get(string key)
        {
            var list = this.Get(Database.Comparison.EqualTo, this.CreatePrimaryKeyField(key), null);
            return (list.Count > 0) ? list[0] : default(T);
        }

        public bool Remove(string key)
        {
            return this.db.Remove(this.TableName, this.CreatePrimaryKeyField(key));
        }

        // Helpers
        protected abstract T ConvertFromRow(string[] fields);

        protected abstract Database.TableField[] ConvertToRow(T instance);

        protected abstract Database.TableField CreatePrimaryKeyField(string key);

        protected List<T> Get(Database.TableColumn[] columns)
        {
            return this.Get(null, null, columns);
        }

        protected List<T> Get(Database.Comparison op, Database.TableField field)
        {
            return this.Get(op, field, null);
        }

        protected List<T> Get(Database.Comparison op, Database.TableField? field, Database.TableColumn[] columns)
        {
            List<string[]> rowDataSet = this.db.Get(this.TableName, op, field, columns);
            return rowDataSet.ConvertAll(new Converter<string[], T>(this.ConvertFromRow));
        }

        protected bool Update(string key, Database.TableField field)
        {
            return this.db.UpdateField(this.TableName, this.CreatePrimaryKeyField(key), field);
        }
    }
}
