using System;

namespace POCOGenerator.Objects
{
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

        public TVP TVP { get; private set; }

        public IDbObject DbObject { get { return this.TVP; } }

        public string ColumnName { get { return this.tvpColumn.ColumnName; } }
        public int? ColumnOrdinal { get { return this.tvpColumn.ColumnOrdinal; } }
        public string DataTypeName { get { return this.tvpColumn.DataTypeName; } }
        public string DataTypeDisplay { get { return this.tvpColumn.DataTypeDisplay; } }
        public string Precision { get { return this.tvpColumn.Precision; } }
        public int? StringPrecision { get { return this.tvpColumn.StringPrecision; } }
        public int? NumericPrecision { get { return this.tvpColumn.NumericPrecision; } }
        public int? NumericScale { get { return this.tvpColumn.NumericScale; } }
        public int? DateTimePrecision { get { return this.tvpColumn.DateTimePrecision; } }
        public bool IsUnsigned { get { return this.tvpColumn.IsUnsigned; } }
        public bool IsNullable { get { return this.tvpColumn.IsNullable; } }
        public bool IsIdentity { get { return this.tvpColumn.IsIdentity; } }
        public bool IsComputed { get { return this.tvpColumn.IsComputed; } }

        public string Description { get { return this.tvpColumn.Description; } }

        public override string ToString()
        {
            return this.tvpColumn.ToString();
        }
    }
}
