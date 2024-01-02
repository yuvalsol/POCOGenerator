using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database unique key of a database table.</summary>
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

        /// <summary>Gets the table that this unique key belongs to.</summary>
        /// <value>The table that this unique key belongs to.</value>
        public Table Table { get; private set; }

        /// <summary>Gets the name of the unique key.</summary>
        /// <value>The name of the unique key.</value>
        public string Name { get { return this.uniqueKey.Name; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IUniqueKeyColumn, UniqueKeyColumn> uniqueKeyColumns;
        /// <summary>Gets the columns of the unique key.</summary>
        /// <value>The columns of the unique key.</value>
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

        /// <summary>Returns a string that represents this unique key.</summary>
        /// <returns>A string that represents this unique key.</returns>
        public override string ToString()
        {
            return this.uniqueKey.ToString();
        }
    }
}
