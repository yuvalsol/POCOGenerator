using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.POCOIterators
{
    public sealed class ComplexTypeColumn : ITableColumn
    {
        public ComplexTypeColumn(string complexTypeName, string complexTypeColumnName, ITableColumn tableColumn)
        {
            this.ComplexTypeName = complexTypeName;
            this.ComplexTypeColumnName = complexTypeColumnName;
            this.ColumnName = complexTypeColumnName;

            this.toString = complexTypeName + tableColumn.ToString().Substring(tableColumn.ColumnName.Length);
            this.toStringFull = complexTypeName + tableColumn.ToFullString().Substring(tableColumn.ColumnName.Length);

            this.ColumnOrdinal = tableColumn.ColumnOrdinal;
            this.DataTypeName = tableColumn.DataTypeName;
            this.DataTypeDisplay = tableColumn.DataTypeDisplay;
            this.Precision = tableColumn.Precision;
            this.StringPrecision = tableColumn.StringPrecision;
            this.NumericPrecision = tableColumn.NumericPrecision;
            this.NumericScale = tableColumn.NumericScale;
            this.DateTimePrecision = tableColumn.DateTimePrecision;
            this.IsUnsigned = tableColumn.IsUnsigned;
            this.IsNullable = tableColumn.IsNullable;
            this.IsIdentity = tableColumn.IsIdentity;
            this.IsComputed = tableColumn.IsComputed;

            this.Table = tableColumn.Table;
            this.PrimaryKeyColumn = tableColumn.PrimaryKeyColumn;
            this.UniqueKeyColumns = tableColumn.UniqueKeyColumns;
            this.ForeignKeyColumns = tableColumn.ForeignKeyColumns;
            this.PrimaryForeignKeyColumns = tableColumn.PrimaryForeignKeyColumns;
            this.IndexColumns = tableColumn.IndexColumns;
            this.ColumnDefault = tableColumn.ColumnDefault;

            this.Description = tableColumn.Description;
        }

        #region IComplexTypeColumn

        public string ComplexTypeName { get; set; }
        public string ComplexTypeColumnName { get; set; }

        #endregion

        #region IDbObject

        private string toString;
        public override string ToString()
        {
            return toString;
        }

        #endregion

        #region IColumn

        public string ColumnName { get; set; }
        public int? ColumnOrdinal { get; set; }
        public string DataTypeName { get; set; }
        public string DataTypeDisplay { get; set; }
        public string Precision { get; set; }
        public int? StringPrecision { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public int? DateTimePrecision { get; set; }
        public bool IsUnsigned { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }

        #endregion

        #region ITableColumn

        public ITable Table { get; set; }
        public IPrimaryKeyColumn PrimaryKeyColumn { get; set; }
        public List<IUniqueKeyColumn> UniqueKeyColumns { get; set; }
        public List<IForeignKeyColumn> ForeignKeyColumns { get; set; }
        public List<IForeignKeyColumn> PrimaryForeignKeyColumns { get; set; }
        public List<IIndexColumn> IndexColumns { get; set; }
        public string ColumnDefault { get; set; }

        private string toStringFull;
        public string ToFullString()
        {
            return toStringFull;
        }

        #endregion

        #region IDescription

        public string Description { get; set; }

        #endregion
    }
}