using System;

namespace POCOGenerator.Objects
{
    public interface IDbColumn
    {
        string ColumnName { get; }
        int? ColumnOrdinal { get; }
        string DataTypeName { get; }
        string DataTypeDisplay { get; }
        string Precision { get; }
        int? StringPrecision { get; }
        int? NumericPrecision { get; }
        int? NumericScale { get; }
        int? DateTimePrecision { get; }
        bool IsUnsigned { get; }
        bool IsNullable { get; }
        bool IsIdentity { get; }
        bool IsComputed { get; }

        IDbObject DbObject { get; }

        string ToString();
    }
}
