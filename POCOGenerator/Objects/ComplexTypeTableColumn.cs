using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents entity framework complex type column.</summary>
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

        /// <summary>Gets the complex type that this complex type column belongs to.</summary>
        /// <value>The complex type that this complex type column belongs to.</value>
        public ComplexTypeTable ComplexTypeTable { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn> tableColumns;
        /// <summary>Gets the table columns associated with this complex type column.</summary>
        /// <value>The table columns associated with this complex type column.</value>
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

        /// <inheritdoc />
        public string ColumnName { get { return this.complexTypeTableColumn.ColumnName; } }
        /// <inheritdoc />
        public int? ColumnOrdinal { get { return this.complexTypeTableColumn.ColumnOrdinal; } }
        /// <inheritdoc />
        public string DataTypeName { get { return this.complexTypeTableColumn.DataTypeName; } }
        /// <inheritdoc />
        public string DataTypeDisplay { get { return this.complexTypeTableColumn.DataTypeDisplay; } }
        /// <inheritdoc />
        public string Precision { get { return this.complexTypeTableColumn.Precision; } }
        /// <inheritdoc />
        public int? StringPrecision { get { return this.complexTypeTableColumn.StringPrecision; } }
        /// <inheritdoc />
        public int? NumericPrecision { get { return this.complexTypeTableColumn.NumericPrecision; } }
        /// <inheritdoc />
        public int? NumericScale { get { return this.complexTypeTableColumn.NumericScale; } }
        /// <inheritdoc />
        public int? DateTimePrecision { get { return this.complexTypeTableColumn.DateTimePrecision; } }
        /// <inheritdoc />
        public bool IsUnsigned { get { return this.complexTypeTableColumn.IsUnsigned; } }
        /// <inheritdoc />
        public bool IsNullable { get { return this.complexTypeTableColumn.IsNullable; } }
        /// <inheritdoc />
        public bool IsIdentity { get { return this.complexTypeTableColumn.IsIdentity; } }
        /// <inheritdoc />
        public bool IsComputed { get { return this.complexTypeTableColumn.IsComputed; } }

        /// <summary>Gets the default value of the column.</summary>
        /// <value>The default value of the column.</value>
        public string ColumnDefault { get { return this.complexTypeTableColumn.ColumnDefault; } }

        /// <summary>Returns a robust string that represents this column.</summary>
        /// <returns>A robust string that represents this column.</returns>
        public string ToFullString()
        {
            return this.complexTypeTableColumn.ToFullString();
        }

        /// <inheritdoc cref="IDbColumn.ToString" />
        public override string ToString()
        {
            return this.complexTypeTableColumn.ToString();
        }
    }
}
