using System;
using System.Collections.Generic;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database view.</summary>
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

        /// <summary>Gets the error message that occurred during the generating process of this view.</summary>
        /// <value>The error message that occurred during the generating process of this view.</value>
        public string Error { get { return (this.view.Error != null ? this.view.Error.Message : null); } }

        /// <summary>Gets the database that this view belongs to.</summary>
        /// <value>The database that this view belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database columns that belong to this view.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.ViewColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, ViewColumn> viewColumns;
        /// <summary>Gets the columns of the view.</summary>
        /// <value>The columns of the view.</value>
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
        /// <summary>Gets the indexes of the view.</summary>
        /// <value>The indexes of the view.</value>
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

        /// <summary>Gets the name of the view.</summary>
        /// <value>The name of the view.</value>
        public string Name { get { return this.view.Name; } }

        /// <summary>Gets the schema of the view.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the view.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.view is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.view).Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the view.</summary>
        /// <value>The description of the view.</value>
        public string Description { get { return this.view.Description; } }

        /// <summary>Returns a string that represents this view.</summary>
        /// <returns>A string that represents this view.</returns>
        public override string ToString()
        {
            return this.view.ToString();
        }
    }
}
