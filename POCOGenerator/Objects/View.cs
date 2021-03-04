using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    public sealed class View : IDbObject
    {
        private readonly POCOGenerator.DbObjects.IView view;

        internal View(POCOGenerator.DbObjects.IView view, Database database)
        {
            this.view = view;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IView view)
        {
            return this.view == view;
        }

        internal string ClassName { get { return this.view.ClassName; } }
        public string Error { get { return (this.view.Error != null ? this.view.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ViewColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, ViewColumn> viewColumns;
        public IEnumerable<ViewColumn> ViewColumns
        {
            get
            {
                if (this.view.TableColumns.IsNullOrEmpty())
                    yield break;

                if (this.viewColumns == null)
                    this.viewColumns = new CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, ViewColumn>(this.view.TableColumns, c => new ViewColumn(c, this));

                foreach (var viewColumn in this.viewColumns)
                {
                    yield return viewColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndex, ViewIndex> indexes;
        public IEnumerable<ViewIndex> Indexes
        {
            get
            {
                if (this.view.Indexes.IsNullOrEmpty())
                    yield break;

                if (this.indexes == null)
                    this.indexes = new CachedEnumerable<POCOGenerator.DbObjects.IIndex, ViewIndex>(this.view.Indexes, i => new ViewIndex(i, this));

                foreach (var index in this.indexes)
                {
                    yield return index;
                }
            }
        }

        public string Name { get { return this.view.Name; } }

        public string Schema
        {
            get
            {
                if (this.view is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.view).Schema;
                return null;
            }
        }

        public string Description { get { return this.view.Description; } }

        public override string ToString()
        {
            return this.view.ToString();
        }
    }
}
