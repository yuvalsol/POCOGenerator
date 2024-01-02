using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database index of a database view.</summary>
    public sealed class ViewIndex : Index
    {
        internal ViewIndex(POCOGenerator.DbObjects.IIndex index, View view)
            : base(index)
        {
            this.View = view;
        }

        /// <summary>Gets the view that this view index belongs to.</summary>
        /// <value>The view that this view index belongs to.</value>
        public View View { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, ViewIndexColumn> indexColumns;
        /// <summary>Gets the columns of the view index.</summary>
        /// <value>The columns of the view index.</value>
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
}
