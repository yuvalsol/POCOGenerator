using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class UniqueKey
    {
        private readonly POCOGenerator.DbObjects.IUniqueKey uniqueKey;

        internal UniqueKey(POCOGenerator.DbObjects.IUniqueKey uniqueKey, Table table)
        {
            this.uniqueKey = uniqueKey;
            this.Table = table;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IUniqueKey uniqueKey)
        {
            return this.uniqueKey == uniqueKey;
        }

        public Table Table { get; private set; }

        public string Name { get { return this.uniqueKey.Name; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IUniqueKeyColumn, UniqueKeyColumn> uniqueKeyColumns;
        public IEnumerable<UniqueKeyColumn> UniqueKeyColumns
        {
            get
            {
                if (this.uniqueKey.UniqueKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.uniqueKeyColumns == null)
                    this.uniqueKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IUniqueKeyColumn, UniqueKeyColumn>(this.uniqueKey.UniqueKeyColumns, ukc => new UniqueKeyColumn(ukc, this));

                foreach (var uniqueKeyColumn in this.uniqueKeyColumns)
                {
                    yield return uniqueKeyColumn;
                }
            }
        }

        public override string ToString()
        {
            return this.uniqueKey.ToString();
        }
    }

    public sealed class UniqueKeyColumn
    {
        private readonly POCOGenerator.DbObjects.IUniqueKeyColumn uniqueKeyColumn;

        internal UniqueKeyColumn(POCOGenerator.DbObjects.IUniqueKeyColumn uniqueKeyColumn, UniqueKey uniqueKey)
        {
            this.uniqueKeyColumn = uniqueKeyColumn;
            this.UniqueKey = uniqueKey;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IUniqueKeyColumn uniqueKeyColumn)
        {
            return this.uniqueKeyColumn == uniqueKeyColumn;
        }

        public UniqueKey UniqueKey { get; private set; }

        private TableColumn tableColumn;
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.UniqueKey.Table.TableColumns.First(c => c.InternalEquals(this.uniqueKeyColumn.TableColumn));

                return this.tableColumn;
            }
        }

        public byte Ordinal { get { return this.uniqueKeyColumn.Ordinal; } }

        public override string ToString()
        {
            return this.uniqueKeyColumn.ToString();
        }

        public string ToFullString()
        {
            return this.uniqueKeyColumn.ToFullString();
        }
    }
}
