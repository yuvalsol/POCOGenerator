using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.Db.DbObjects
{
    internal sealed class ComplexTypeTableColumn : IComplexTypeTableColumn
    {
        public IComplexTypeTable ComplexTypeTable { get; set; }
        public string ColumnDefault { get; internal set; }
        public List<ITableColumn> TableColumns { get; internal set; }

        internal string toFullString;
        public string ToFullString()
        {
            return toFullString;
        }

        #region IColumn

        public string ColumnName { get; internal set; }
        public int? ColumnOrdinal { get; internal set; }
        public string DataTypeName { get; internal set; }
        public string DataTypeDisplay { get; internal set; }
        public string Precision { get; internal set; }
        public int? StringPrecision { get; internal set; }
        public int? NumericPrecision { get; internal set; }
        public int? NumericScale { get; internal set; }
        public int? DateTimePrecision { get; internal set; }
        public bool IsUnsigned { get; internal set; }
        public bool IsNullable { get; internal set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }

        #endregion

        #region IDbObject

        internal string toString;
        public override string ToString()
        {
            return toString;
        }

        #endregion
    }
}
