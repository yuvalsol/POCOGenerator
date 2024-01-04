using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database index.</summary>
    public abstract class IndexColumn
    {
        private protected readonly POCOGenerator.DbObjects.IIndexColumn indexColumn;

        internal IndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn)
        {
            this.indexColumn = indexColumn;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IIndexColumn indexColumn)
        {
            return this.indexColumn == indexColumn;
        }

        /// <summary>Gets the ordinal position of the index column.</summary>
        /// <value>The ordinal position of the index column.</value>
        public byte Ordinal { get { return this.indexColumn.Ordinal; } }

        /// <summary>Gets a value indicating whether this index column is a descending index column.</summary>
        /// <value>
        ///   <c>true</c> if this index column is a descending index column; otherwise, <c>false</c>.</value>
        public bool IsDescending { get { return this.indexColumn.Is_Descending; } }

        /// <summary>Returns a string that represents this index column.</summary>
        /// <returns>A string that represents this index column.</returns>
        public override string ToString()
        {
            return this.indexColumn.ToString();
        }

        /// <summary>Returns a robust string that represents this index column.</summary>
        /// <returns>A robust string that represents this index column.</returns>
        public string ToFullString()
        {
            return this.indexColumn.ToFullString();
        }
    }
}
