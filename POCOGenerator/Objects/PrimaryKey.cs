using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database primary key of a database table.</summary>
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

        /// <summary>Gets the table that this primary key belongs to.</summary>
        /// <value>The table that this primary key belongs to.</value>
        public Table Table { get; private set; }

        /// <summary>Gets the name of the primary key.</summary>
        /// <value>The name of the primary key.</value>
        public string Name { get { return this.primaryKey.Name; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IPrimaryKeyColumn, PrimaryKeyColumn> primaryKeyColumns;
        /// <summary>Gets the columns of the primary key.</summary>
        /// <value>The columns of the primary key.</value>
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

        /// <summary>Returns a string that represents this primary key.</summary>
        /// <returns>A string that represents this primary key.</returns>
        public override string ToString()
        {
            return this.primaryKey.ToString();
        }
    }
}
