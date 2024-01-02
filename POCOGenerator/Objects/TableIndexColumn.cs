using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database table index.</summary>
    public sealed class TableIndexColumn : IndexColumn
    {
        internal TableIndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn, TableIndex index)
            : base(indexColumn)
        {
            this.Index = index;
        }

        /// <summary>Gets the table index that this table index column belongs to.</summary>
        /// <value>The table index that this table index column belongs to.</value>
        public TableIndex Index { get; private set; }

        private TableColumn tableColumn;
        /// <summary>Gets the table column associated with this table index column.</summary>
        /// <value>The table column associated with this table index column.</value>
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.Index.Table.TableColumns.First(c => c.InternalEquals(this.indexColumn.TableColumn));

                return this.tableColumn;
            }
        }
    }
}
