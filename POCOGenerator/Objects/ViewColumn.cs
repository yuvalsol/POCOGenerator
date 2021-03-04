using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
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

        public View View { get; private set; }

        public IDbObject DbObject { get { return this.View; } }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn> indexColumns;
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

        public string ColumnName { get { return this.tableColumn.ColumnName; } }
        public int? ColumnOrdinal { get { return this.tableColumn.ColumnOrdinal; } }
        public string DataTypeName { get { return this.tableColumn.DataTypeName; } }
        public string DataTypeDisplay { get { return this.tableColumn.DataTypeDisplay; } }
        public string Precision { get { return this.tableColumn.Precision; } }
        public int? StringPrecision { get { return this.tableColumn.StringPrecision; } }
        public int? NumericPrecision { get { return this.tableColumn.NumericPrecision; } }
        public int? NumericScale { get { return this.tableColumn.NumericScale; } }
        public int? DateTimePrecision { get { return this.tableColumn.DateTimePrecision; } }
        public bool IsUnsigned { get { return this.tableColumn.IsUnsigned; } }
        public bool IsNullable { get { return this.tableColumn.IsNullable; } }
        public bool IsIdentity { get { return this.tableColumn.IsIdentity; } }
        public bool IsComputed { get { return this.tableColumn.IsComputed; } }

        public string ColumnDefault { get { return this.tableColumn.ColumnDefault; } }

        public string Description { get { return this.tableColumn.Description; } }

        public string ToFullString()
        {
            return this.tableColumn.ToFullString();
        }

        public override string ToString()
        {
            return this.tableColumn.ToString();
        }
    }
}
