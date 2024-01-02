using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database index.</summary>
    public abstract class Index
    {
        private protected readonly POCOGenerator.DbObjects.IIndex index;

        internal Index(POCOGenerator.DbObjects.IIndex index)
        {
            this.index = index;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IIndex index)
        {
            return this.index == index;
        }

        /// <summary>Gets the name of the index.</summary>
        /// <value>The name of the index.</value>
        public string Name { get { return this.index.Name; } }

        /// <summary>Gets a value indicating whether this index is a unique index.</summary>
        /// <value>
        ///   <c>true</c> if this index is a unique index; otherwise, <c>false</c>.</value>
        /// <remarks>A unique index guarantees that the index key contains no duplicate values and therefore every row is in some way unique.</remarks>
        public bool Is_Unique { get { return this.index.Is_Unique; } }

        /// <summary>Gets a value indicating whether this index is a clustered index.</summary>
        /// <value>
        ///   <c>true</c> if this index is a clustered index; otherwise, <c>false</c>.</value>
        /// <remarks>A clustered index sorts and stores the data rows based on their key values. These key values are the columns included in the index definition.</remarks>
        public bool Is_Clustered { get { return this.index.Is_Clustered; } }

        /// <summary>Gets the description of the index.</summary>
        /// <value>The description of the index.</value>
        public string Description { get { return this.index.Description; } }

        /// <summary>Returns a string that represents this index.</summary>
        /// <returns>A string that represents this index.</returns>
        public override string ToString()
        {
            return this.index.ToString();
        }

        /// <summary>Returns a robust string that represents this index.</summary>
        /// <returns>A robust string that represents this index.</returns>
        public string ToFullString()
        {
            return this.index.ToFullString();
        }
    }
}
