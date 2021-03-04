using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.POCOIterators
{
    public sealed class ComplexType : IColumn
    {
        public string ComplexTypeName { get; set; }

        public ComplexType(string complexTypeName, int? columnOrdinal)
        {
            ComplexTypeName = complexTypeName;
            ColumnOrdinal = columnOrdinal;
        }

        public override string ToString()
        {
            return ComplexTypeName;
        }

        #region IColumn

        public string ColumnName { get { return ComplexTypeName + "_CT"; } }
        public int? ColumnOrdinal { get; set; }
        public string DataTypeName { get { return ComplexTypeName; } }
        public string DataTypeDisplay { get { return ComplexTypeName; } }
        public string Precision { get { return null; } }
        public int? StringPrecision { get { return null; } }
        public int? NumericPrecision { get { return null; } }
        public int? NumericScale { get { return null; } }
        public int? DateTimePrecision { get { return null; } }
        public bool IsUnsigned { get { return false; } }
        public bool IsNullable { get { return true; } }
        public bool IsIdentity { get { return false; } set { } }
        public bool IsComputed { get { return false; } set { } }

        #endregion
    }
}
