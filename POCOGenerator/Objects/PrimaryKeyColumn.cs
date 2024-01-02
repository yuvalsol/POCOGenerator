using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database primary key.</summary>
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

        /// <summary>Gets the primary key that this primary key column belongs to.</summary>
        /// <value>The primary key that this primary key column belongs to.</value>
        public PrimaryKey PrimaryKey { get; private set; }

        private TableColumn tableColumn;
        /// <summary>Gets the table column associated with this primary key column.</summary>
        /// <value>The table column associated with this primary key column.</value>
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.PrimaryKey.Table.TableColumns.First(c => c.InternalEquals(this.primaryKeyColumn.TableColumn));

                return this.tableColumn;
            }
        }

        /// <summary>Gets the ordinal position of the primary key column.</summary>
        /// <value>The ordinal position of the primary key column.</value>
        public byte Ordinal { get { return this.primaryKeyColumn.Ordinal; } }

        /// <summary>Returns a string that represents this primary key column.</summary>
        /// <returns>A string that represents this primary key column.</returns>
        public override string ToString()
        {
            return this.primaryKeyColumn.ToString();
        }

        /// <summary>Returns a robust string that represents this primary key column.</summary>
        /// <returns>A robust string that represents this primary key column.</returns>
        public string ToFullString()
        {
            return this.primaryKeyColumn.ToFullString();
        }
    }
}
