using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database user-defined table type.</summary>
    /// <remarks>
    /// Table-valued parameters (TVPs) are declared by using user-defined table types and are scoped to stored procedures and functions. POCO Generator generates user-defined table types but uses the acronym TVP (table-valued parameter) interchangeably with user-defined table type although they are different things.
    /// </remarks>
    public sealed class TVP : IDbObject
    {
        private readonly POCOGenerator.DbObjects.ITVP tvp;

        internal TVP(POCOGenerator.DbObjects.ITVP tvp, Database database)
        {
            this.tvp = tvp;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.ITVP tvp)
        {
            return this.tvp == tvp;
        }

        internal string ClassName { get { return this.tvp.ClassName; } }

        /// <summary>Gets the error message that occurred during the generating process of this TVP.</summary>
        /// <value>The error message that occurred during the generating process of this TVP.</value>
        public string Error { get { return (this.tvp.Error != null ? this.tvp.Error.Message : null); } }

        /// <summary>Gets the database that this TVP belongs to.</summary>
        /// <value>The database that this TVP belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database columns that belong to this TVP.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.TVPColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITVPColumn, TVPColumn> tvpColumns;
        /// <summary>Gets the columns of the TVP.</summary>
        /// <value>The columns of the TVP.</value>
        public IEnumerable<TVPColumn> TVPColumns
        {
            get
            {
                if (this.tvp.TVPColumns.IsNullOrEmpty())
                    yield break;

                if (this.tvpColumns == null)
                    this.tvpColumns = new CachedEnumerable<POCOGenerator.DbObjects.ITVPColumn, TVPColumn>(this.tvp.TVPColumns, c => new TVPColumn(c, this));

                foreach (var tvpColumn in this.tvpColumns)
                {
                    yield return tvpColumn;
                }
            }
        }

        /// <summary>Gets the name of the TVP.</summary>
        /// <value>The name of the TVP.</value>
        public string Name { get { return this.tvp.Name; } }

        /// <summary>Gets the schema of the TVP.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the TVP.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.tvp is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.tvp).Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the TVP.</summary>
        /// <value>The description of the TVP.</value>
        public string Description { get { return this.tvp.Description; } }

        /// <summary>Returns a string that represents this TVP.</summary>
        /// <returns>A string that represents this TVP.</returns>
        public override string ToString()
        {
            return this.tvp.ToString();
        }
    }
}
