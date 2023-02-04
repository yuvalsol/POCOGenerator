using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class ComplexTypeTable : IDbObject
    {
        private readonly POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable;

        internal ComplexTypeTable(POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable, Database database)
        {
            this.complexTypeTable = complexTypeTable;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IComplexTypeTable complexTypeTable)
        {
            return this.complexTypeTable == complexTypeTable;
        }

        internal string ClassName { get { return this.complexTypeTable.ClassName; } }
        public string Error { get { return (this.complexTypeTable.Error != null ? this.complexTypeTable.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ComplexTypeTableColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTableColumn, ComplexTypeTableColumn> complexTypeTableColumns;
        public IEnumerable<ComplexTypeTableColumn> ComplexTypeTableColumns
        {
            get
            {
                if (this.complexTypeTable.ComplexTypeTableColumns.IsNullOrEmpty())
                    yield break;

                if (this.complexTypeTableColumns == null)
                    this.complexTypeTableColumns = new CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTableColumn, ComplexTypeTableColumn>(this.complexTypeTable.ComplexTypeTableColumns, c => new ComplexTypeTableColumn(c, this));

                foreach (var complexTypeTableColumn in this.complexTypeTableColumns)
                {
                    yield return complexTypeTableColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITable, Table> tables;
        public IEnumerable<Table> Tables
        {
            get
            {
                if (this.complexTypeTable.Tables.IsNullOrEmpty())
                    yield break;

                if (this.tables == null)
                {
                    this.tables = new CachedEnumerable<POCOGenerator.DbObjects.ITable, Table>(
                        this.complexTypeTable.Tables,
                        t1 => this.Database.Tables.First(t2 => t2.InternalEquals(t1))
                    );
                }

                foreach (var table in this.tables)
                {
                    yield return table;
                }
            }
        }

        public string Name { get { return this.complexTypeTable.Name; } }

        public string Schema
        {
            get
            {
                if (this.complexTypeTable is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.complexTypeTable).Schema;
                return null;
            }
        }

        public string Description { get { return null; } }

        public override string ToString()
        {
            return this.complexTypeTable.ToString();
        }
    }
}
