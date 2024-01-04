using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database foreign key.</summary>
    public sealed class ForeignKeyColumn
    {
        private readonly POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn;

        internal ForeignKeyColumn(POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn, ForeignKey foreignKey)
        {
            this.foreignKeyColumn = foreignKeyColumn;
            this.ForeignKey = foreignKey;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn)
        {
            return this.foreignKeyColumn == foreignKeyColumn;
        }

        /// <summary>Gets the foreign key that this foreign key column belongs to.</summary>
        /// <value>The foreign key that this foreign key column belongs to.</value>
        public ForeignKey ForeignKey { get; private set; }

        private TableColumn foreignTableColumn;
        /// <summary>Gets the table column that this foreign key column is referencing from.</summary>
        /// <value>The table column that this foreign key column is referencing from.</value>
        public TableColumn ForeignTableColumn
        {
            get
            {
                if (this.foreignTableColumn == null)
                    this.foreignTableColumn = this.ForeignKey.ForeignTable.TableColumns.First(c => c.InternalEquals(this.foreignKeyColumn.ForeignTableColumn));

                return this.foreignTableColumn;
            }
        }

        private TableColumn primaryTableColumn;
        /// <summary>Gets the table column that this foreign key column is referencing to.</summary>
        /// <value>The table column that this foreign key column is referencing to.</value>
        public TableColumn PrimaryTableColumn
        {
            get
            {
                if (this.foreignKeyColumn.PrimaryTableColumn == null)
                    return null;

                if (this.primaryTableColumn == null)
                    this.primaryTableColumn = this.ForeignKey.PrimaryTable.TableColumns.First(c => c.InternalEquals(this.foreignKeyColumn.PrimaryTableColumn));

                return this.primaryTableColumn;
            }
        }

        /// <summary>Gets a value indicating whether <see cref="ForeignTableColumn" /> is part of the primary key of <see cref="ForeignKey.ForeignTable" />.</summary>
        /// <value>
        ///   <c>true</c> if the foreign table column is part of a primary key; otherwise, <c>false</c>.</value>
        public bool IsForeignColumnPK { get { return this.foreignKeyColumn.Is_Foreign_Column_PK; } }

        /// <summary>Gets a value indicating whether <see cref="PrimaryTableColumn" /> is part of the primary key of <see cref="ForeignKey.PrimaryTable" />.</summary>
        /// <value>
        ///   <c>true</c> if the primary table column is part of a primary key; otherwise, <c>false</c>.</value>
        public bool IsPrimaryColumnPK { get { return this.foreignKeyColumn.Is_Primary_Column_PK; } }

        /// <summary>Gets the ordinal position of the foreign key column.</summary>
        /// <value>The ordinal position of the foreign key column.</value>
        public int Ordinal { get { return this.foreignKeyColumn.Ordinal; } }

        /// <summary>Returns a string that represents this foreign key column.</summary>
        /// <returns>A string that represents this foreign key column.</returns>
        public override string ToString()
        {
            return this.foreignKeyColumn.ToString();
        }

        /// <summary>Returns a robust string that represents this foreign key column.</summary>
        /// <returns>A robust string that represents this foreign key column.</returns>
        public string ToFullString()
        {
            return this.foreignKeyColumn.ToFullString();
        }
    }
}
