using System;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database user-defined table type column.</summary>
    public sealed class TVPColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.ITVPColumn tvpColumn;

        internal TVPColumn(POCOGenerator.DbObjects.ITVPColumn tvpColumn, TVP tvp)
        {
            this.tvpColumn = tvpColumn;
            this.TVP = tvp;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.ITVPColumn tvpColumn)
        {
            return this.tvpColumn == tvpColumn;
        }

        /// <summary>Gets the TVP that this TVP column belongs to.</summary>
        /// <value>The TVP that this TVP column belongs to.</value>
        public TVP TVP { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }

        /// <inheritdoc />
        public string ColumnName { get { return this.tvpColumn.ColumnName; } }
        /// <inheritdoc />
        public int? ColumnOrdinal { get { return this.tvpColumn.ColumnOrdinal; } }
        /// <inheritdoc />
        public string DataTypeName { get { return this.tvpColumn.DataTypeName; } }
        /// <inheritdoc />
        public string DataTypeDisplay { get { return this.tvpColumn.DataTypeDisplay; } }
        /// <inheritdoc />
        public string Precision { get { return this.tvpColumn.Precision; } }
        /// <inheritdoc />
        public int? StringPrecision { get { return this.tvpColumn.StringPrecision; } }
        /// <inheritdoc />
        public int? NumericPrecision { get { return this.tvpColumn.NumericPrecision; } }
        /// <inheritdoc />
        public int? NumericScale { get { return this.tvpColumn.NumericScale; } }
        /// <inheritdoc />
        public int? DateTimePrecision { get { return this.tvpColumn.DateTimePrecision; } }
        /// <inheritdoc />
        public bool IsUnsigned { get { return this.tvpColumn.IsUnsigned; } }
        /// <inheritdoc />
        public bool IsNullable { get { return this.tvpColumn.IsNullable; } }
        /// <inheritdoc />
        public bool IsIdentity { get { return this.tvpColumn.IsIdentity; } }
        /// <inheritdoc />
        public bool IsComputed { get { return this.tvpColumn.IsComputed; } }

        /// <summary>Gets the description of the column.</summary>
        /// <value>The description of the column.</value>
        public string Description { get { return this.tvpColumn.Description; } }

        /// <inheritdoc cref="IDbColumn.ToString" />
        public override string ToString()
        {
            return this.tvpColumn.ToString();
        }
    }
}
