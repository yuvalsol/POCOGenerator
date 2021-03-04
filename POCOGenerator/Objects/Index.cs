using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public abstract class Index
    {
        protected POCOGenerator.DbObjects.IIndex index;

        protected Index(POCOGenerator.DbObjects.IIndex index)
        {
            this.index = index;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IIndex index)
        {
            return this.index == index;
        }

        public string Name { get { return this.index.Name; } }
        public bool Is_Unique { get { return this.index.Is_Unique; } }
        public bool Is_Clustered { get { return this.index.Is_Clustered; } }

        public string Description { get { return this.index.Description; } }

        public override string ToString()
        {
            return this.index.ToString();
        }

        public string ToFullString()
        {
            return this.index.ToFullString();
        }
    }

    public sealed class TableIndex : Index
    {
        internal TableIndex(POCOGenerator.DbObjects.IIndex index, Table table)
            : base(index)
        {
            this.Table = table;
        }

        public Table Table { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn> indexColumns;
        public IEnumerable<TableIndexColumn> IndexColumns
        {
            get
            {
                if (this.index.IndexColumns.IsNullOrEmpty())
                    yield break;

                if (this.indexColumns == null)
                    this.indexColumns = new CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn>(this.index.IndexColumns, ic => new TableIndexColumn(ic, this));

                foreach (var indexColumn in this.indexColumns)
                {
                    yield return indexColumn;
                }
            }
        }
    }

    public sealed class ViewIndex : Index
    {
        internal ViewIndex(POCOGenerator.DbObjects.IIndex index, View view)
            : base(index)
        {
            this.View = view;
        }

        public View View { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn> indexColumns;
        public IEnumerable<ViewIndexColumn> IndexColumns
        {
            get
            {
                if (this.index.IndexColumns.IsNullOrEmpty())
                    yield break;

                if (this.indexColumns == null)
                    this.indexColumns = new CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn>(this.index.IndexColumns, ic => new ViewIndexColumn(ic, this));

                foreach (var indexColumn in this.indexColumns)
                {
                    yield return indexColumn;
                }
            }
        }
    }

    public abstract class IndexColumn
    {
        protected POCOGenerator.DbObjects.IIndexColumn indexColumn;

        protected IndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn)
        {
            this.indexColumn = indexColumn;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IIndexColumn indexColumn)
        {
            return this.indexColumn == indexColumn;
        }

        public byte Ordinal { get { return this.indexColumn.Ordinal; } }
        public bool Is_Descending { get { return this.indexColumn.Is_Descending; } }

        public override string ToString()
        {
            return this.indexColumn.ToString();
        }

        public string ToFullString()
        {
            return this.indexColumn.ToFullString();
        }
    }

    public sealed class TableIndexColumn : IndexColumn
    {
        internal TableIndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn, TableIndex index)
            : base(indexColumn)
        {
            this.Index = index;
        }

        public TableIndex Index { get; private set; }

        private TableColumn tableColumn;
        public TableColumn TableColumn
        {
            get
            {
                if (this.tableColumn == null)
                    this.tableColumn = this.Index.Table.TableColumns.First(c => c.InternalEquals(this.indexColumn.TableColumn));

                return this.tableColumn;
            }
        }
    }

    public sealed class ViewIndexColumn : IndexColumn
    {
        internal ViewIndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn, ViewIndex index)
            : base(indexColumn)
        {
            this.Index = index;
        }

        public ViewIndex Index { get; private set; }

        private ViewColumn viewColumn;
        public ViewColumn ViewColumn
        {
            get
            {
                if (this.viewColumn == null)
                    this.viewColumn = this.Index.View.ViewColumns.First(c => c.InternalEquals(this.indexColumn.TableColumn));

                return this.viewColumn;
            }
        }
    }
}
