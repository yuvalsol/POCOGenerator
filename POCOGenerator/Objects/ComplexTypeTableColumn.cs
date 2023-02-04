using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class ComplexTypeTableColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.IComplexTypeTableColumn complexTypeTableColumn;

        internal ComplexTypeTableColumn(POCOGenerator.DbObjects.IComplexTypeTableColumn complexTypeTableColumn, ComplexTypeTable complexTypeTable)
        {
            this.complexTypeTableColumn = complexTypeTableColumn;
            this.ComplexTypeTable = complexTypeTable;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IComplexTypeTableColumn complexTypeTableColumn)
        {
            return this.complexTypeTableColumn == complexTypeTableColumn;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }

        public IDbObject DbObject { get { return this.ComplexTypeTable; } }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn> tableColumns;
        public IEnumerable<TableColumn> TableColumns
        {
            get
            {
                if (this.complexTypeTableColumn.TableColumns.IsNullOrEmpty())
                    yield break;

                if (this.tableColumns == null)
                {
                    this.tableColumns = new CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn>(
                        this.complexTypeTableColumn.TableColumns,
                        c1 => this.ComplexTypeTable.Tables.First(t => t.InternalEquals(c1.Table)).TableColumns.First(c2 => c2.InternalEquals(c1))
                    );
                }

                foreach (var tableColumn in this.tableColumns)
                {
                    yield return tableColumn;
                }
            }
        }

        public string ColumnName { get { return this.complexTypeTableColumn.ColumnName; } }
        public int? ColumnOrdinal { get { return this.complexTypeTableColumn.ColumnOrdinal; } }
        public string DataTypeName { get { return this.complexTypeTableColumn.DataTypeName; } }
        public string DataTypeDisplay { get { return this.complexTypeTableColumn.DataTypeDisplay; } }
        public string Precision { get { return this.complexTypeTableColumn.Precision; } }
        public int? StringPrecision { get { return this.complexTypeTableColumn.StringPrecision; } }
        public int? NumericPrecision { get { return this.complexTypeTableColumn.NumericPrecision; } }
        public int? NumericScale { get { return this.complexTypeTableColumn.NumericScale; } }
        public int? DateTimePrecision { get { return this.complexTypeTableColumn.DateTimePrecision; } }
        public bool IsUnsigned { get { return this.complexTypeTableColumn.IsUnsigned; } }
        public bool IsNullable { get { return this.complexTypeTableColumn.IsNullable; } }
        public bool IsIdentity { get { return this.complexTypeTableColumn.IsIdentity; } }
        public bool IsComputed { get { return this.complexTypeTableColumn.IsComputed; } }

        public string ColumnDefault { get { return this.complexTypeTableColumn.ColumnDefault; } }

        public string ToFullString()
        {
            return this.complexTypeTableColumn.ToFullString();
        }

        public override string ToString()
        {
            return this.complexTypeTableColumn.ToString();
        }
    }
}
