using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
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
        public string Error { get { return (this.tvp.Error != null ? this.tvp.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.TVPColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITVPColumn, TVPColumn> tvpColumns;
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

        public string Name { get { return this.tvp.Name; } }

        public string Schema
        {
            get
            {
                if (this.tvp is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.tvp).Schema;
                return null;
            }
        }

        public string Description { get { return this.tvp.Description; } }

        public override string ToString()
        {
            return this.tvp.ToString();
        }
    }
}
