using System;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a column of a database view index.</summary>
    public sealed class ViewIndexColumn : IndexColumn
    {
        internal ViewIndexColumn(POCOGenerator.DbObjects.IIndexColumn indexColumn, ViewIndex index)
            : base(indexColumn)
        {
            this.Index = index;
        }

        /// <summary>Gets the view index that this view index column belongs to.</summary>
        /// <value>The view index that this view index column belongs to.</value>
        public ViewIndex Index { get; private set; }

        private ViewColumn viewColumn;
        /// <summary>Gets the view column associated with this view index column.</summary>
        /// <value>The view column associated with this view index column.</value>
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
