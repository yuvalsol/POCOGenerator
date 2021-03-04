using System;

namespace POCOGenerator.DbObjects
{
    public interface IColumn : IDbObject
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
        bool IsIdentity { get; set; }
        bool IsComputed { get; set; }
    }
}
