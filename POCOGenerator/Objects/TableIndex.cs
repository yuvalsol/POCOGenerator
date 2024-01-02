using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database index of a database table.</summary>
    public sealed class TableIndex : Index
    {
        internal TableIndex(POCOGenerator.DbObjects.IIndex index, Table table)
            : base(index)
        {
            this.Table = table;
        }

        /// <summary>Gets the table that this table index belongs to.</summary>
        /// <value>The table that this table index belongs to.</value>
        public Table Table { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn> indexColumns;
        /// <summary>Gets the columns of the table index.</summary>
        /// <value>The columns of the table index.</value>
        public IEnumerable<TableIndexColumn> IndexColumns
        {
            get
            {
                if (this.index.IndexColumns.IsNullOrEmpty())
                    yield break;

                if (this.indexColumns == null)
                    this.indexColumns = new CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn>(this.index.IndexColumns, ic => new TableIndexColumn(ic, this));

                foreach (var indexColumn in this.indexColumns)
                {
                    yield return indexColumn;
                }
            }
        }
    }
}
