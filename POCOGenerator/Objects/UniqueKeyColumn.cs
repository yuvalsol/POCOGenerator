using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database unique key.</summary>
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

        /// <summary>Gets the unique key that this unique key column belongs to.</summary>
        /// <value>The unique key that this unique key column belongs to.</value>
        public UniqueKey UniqueKey { get; private set; }

        private TableColumn tableColumn;
        /// <summary>Gets the table column associated with this unique key column.</summary>
        /// <value>The table column associated with this unique key column.</value>
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.UniqueKey.Table.TableColumns.First(c => c.InternalEquals(this.uniqueKeyColumn.TableColumn));

                return this.tableColumn;
            }
        }

        /// <summary>Gets the ordinal position of the unique key column.</summary>
        /// <value>The ordinal position of the unique key column.</value>
        public byte Ordinal { get { return this.uniqueKeyColumn.Ordinal; } }

        /// <summary>Returns a string that represents this unique key column.</summary>
        /// <returns>A string that represents this unique key column.</returns>
        public override string ToString()
        {
            return this.uniqueKeyColumn.ToString();
        }

        /// <summary>Returns a robust string that represents this unique key column.</summary>
        /// <returns>A robust string that represents this unique key column.</returns>
        public string ToFullString()
        {
            return this.uniqueKeyColumn.ToFullString();
        }
    }
}
