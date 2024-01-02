using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database view column.</summary>
    public sealed class ViewColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.ITableColumn tableColumn;

        internal ViewColumn(POCOGenerator.DbObjects.ITableColumn tableColumn, View view)
        {
            this.tableColumn = tableColumn;
            this.View = view;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.ITableColumn tableColumn)
        {
            return this.tableColumn == tableColumn;
        }

        /// <summary>Gets the view that this view column belongs to.</summary>
        /// <value>The view that this view column belongs to.</value>
        public View View { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn> indexColumns;
        /// <summary>Gets the columns of view indexes associated with this view column.</summary>
        /// <value>The columns of view indexes associated with this view column.</value>
        public IEnumerable<ViewIndexColumn> IndexColumns
        {
            get
            {
                if (this.tableColumn.IndexColumns.IsNullOrEmpty())
                    yield break;

                if (this.indexColumns == null)
                {
                    this.indexColumns = new CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn>(
                        this.tableColumn.IndexColumns,
                        c => this.View.Indexes.SelectMany(i => i.IndexColumns).First(ic => ic.InternalEquals(c))
                    );
                }

                foreach (var indexColumn in this.indexColumns)
                {
                    yield return indexColumn;
                }
            }
        }

        /// <inheritdoc />
        public string ColumnName { get { return this.tableColumn.ColumnName; } }
        /// <inheritdoc />
        public int? ColumnOrdinal { get { return this.tableColumn.ColumnOrdinal; } }
        /// <inheritdoc />
        public string DataTypeName { get { return this.tableColumn.DataTypeName; } }
        /// <inheritdoc />
        public string DataTypeDisplay { get { return this.tableColumn.DataTypeDisplay; } }
        /// <inheritdoc />
        public string Precision { get { return this.tableColumn.Precision; } }
        /// <inheritdoc />
        public int? StringPrecision { get { return this.tableColumn.StringPrecision; } }
        /// <inheritdoc />
        public int? NumericPrecision { get { return this.tableColumn.NumericPrecision; } }
        /// <inheritdoc />
        public int? NumericScale { get { return this.tableColumn.NumericScale; } }
        /// <inheritdoc />
        public int? DateTimePrecision { get { return this.tableColumn.DateTimePrecision; } }
        /// <inheritdoc />
        public bool IsUnsigned { get { return this.tableColumn.IsUnsigned; } }
        /// <inheritdoc />
        public bool IsNullable { get { return this.tableColumn.IsNullable; } }
        /// <inheritdoc />
        public bool IsIdentity { get { return this.tableColumn.IsIdentity; } }
        /// <inheritdoc />
        public bool IsComputed { get { return this.tableColumn.IsComputed; } }

        /// <summary>Gets the default value of the column.</summary>
        /// <value>The default value of the column.</value>
        public string ColumnDefault { get { return this.tableColumn.ColumnDefault; } }

        /// <summary>Gets the description of the column.</summary>
        /// <value>The description of the column.</value>
        public string Description { get { return this.tableColumn.Description; } }

        /// <summary>Returns a robust string that represents this column.</summary>
        /// <returns>A robust string that represents this column.</returns>
        public string ToFullString()
        {
            return this.tableColumn.ToFullString();
        }

        /// <inheritdoc cref="IDbColumn.ToString" />
        public override string ToString()
        {
            return this.tableColumn.ToString();
        }
    }
}
