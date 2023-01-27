using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.MySQL.DbObjects
{
    internal class Database : IDatabase
    {
        public string database_name { get; set; }
        public string CATALOG_NAME { get; set; }
        public string DEFAULT_CHARACTER_SET_NAME { get; set; }
        public string DEFAULT_COLLATION_NAME { get; set; }
        public string SQL_PATH { get; set; }

        public IServer Server { get; set; }
        public List<ITable> Tables { get; set; }
        public List<IView> Views { get; set; }
        public List<IProcedure> Procedures { get; set; }
        public List<IFunction> Functions { get; set; }
        public List<ITVP> TVPs { get; set; }
        public List<IComplexType> ComplexTypes { get; set; }
        public List<Exception> Errors { get; set; }

        public string Name { get { return database_name; } }

        public string Description { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public string GetDatabaseConnectionString(string connectionString)
        {
            return connectionString + (connectionString.EndsWith(";") ? string.Empty : ";") + string.Format("Database={0};", Name);
        }
    }
}
