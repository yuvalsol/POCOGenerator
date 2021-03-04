using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class PrimaryKey
    {
        private readonly POCOGenerator.DbObjects.IPrimaryKey primaryKey;

        internal PrimaryKey(POCOGenerator.DbObjects.IPrimaryKey primaryKey, Table table)
        {
            this.primaryKey = primaryKey;
            this.Table = table;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IPrimaryKey primaryKey)
        {
            return this.primaryKey == primaryKey;
        }

        public Table Table { get; private set; }

        public string Name { get { return this.primaryKey.Name; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IPrimaryKeyColumn, PrimaryKeyColumn> primaryKeyColumns;
        public IEnumerable<PrimaryKeyColumn> PrimaryKeyColumns
        {
            get
            {
                if (this.primaryKey.PrimaryKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.primaryKeyColumns == null)
                    this.primaryKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IPrimaryKeyColumn, PrimaryKeyColumn>(this.primaryKey.PrimaryKeyColumns, pkc => new PrimaryKeyColumn(pkc, this));

                foreach (var primaryKeyColumn in this.primaryKeyColumns)
                {
                    yield return primaryKeyColumn;
                }
            }
        }

        public override string ToString()
        {
            return this.primaryKey.ToString();
        }
    }

    public sealed class PrimaryKeyColumn
    {
        private readonly POCOGenerator.DbObjects.IPrimaryKeyColumn primaryKeyColumn;

        internal PrimaryKeyColumn(POCOGenerator.DbObjects.IPrimaryKeyColumn primaryKeyColumn, PrimaryKey primaryKey)
        {
            this.primaryKeyColumn = primaryKeyColumn;
            this.PrimaryKey = primaryKey;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IPrimaryKeyColumn primaryKeyColumn)
        {
            return this.primaryKeyColumn == primaryKeyColumn;
        }

        public PrimaryKey PrimaryKey { get; private set; }

        private TableColumn tableColumn;
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.PrimaryKey.Table.TableColumns.First(c => c.InternalEquals(this.primaryKeyColumn.TableColumn));

                return this.tableColumn;
            }
        }

        public byte Ordinal { get { return this.primaryKeyColumn.Ordinal; } }

        public override string ToString()
        {
            return this.primaryKeyColumn.ToString();
        }

        public string ToFullString()
        {
            return this.primaryKeyColumn.ToFullString();
        }
    }
}
