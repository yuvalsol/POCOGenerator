using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database table column.</summary>
    public sealed class TableColumn : IDbColumn
    {
        private readonly POCOGenerator.DbObjects.ITableColumn tableColumn;

        internal TableColumn(POCOGenerator.DbObjects.ITableColumn tableColumn, Table table)
        {
            this.tableColumn = tableColumn;
            this.Table = table;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.ITableColumn tableColumn)
        {
            return this.tableColumn == tableColumn;
        }

        /// <summary>Gets the table that this table column belongs to.</summary>
        /// <value>The table that this table column belongs to.</value>
        public Table Table { get; private set; }

        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }

        private PrimaryKeyColumn primaryKeyColumn;
        /// <summary>Gets the primary key column associated with this table column.
        /// <para>Returns <see langword="null" /> if this table column is not a part of a primary key.</para></summary>
        /// <value>The primary key column associated with this table column.</value>
        public PrimaryKeyColumn PrimaryKeyColumn
        {
            get
            {
                if (this.tableColumn.PrimaryKeyColumn == null)
                    return null;

                if (this.primaryKeyColumn == null)
                    this.primaryKeyColumn = this.Table.PrimaryKey.PrimaryKeyColumns.First(pkc => pkc.InternalEquals(this.tableColumn.PrimaryKeyColumn));

                return this.primaryKeyColumn;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IUniqueKeyColumn, UniqueKeyColumn> uniqueKeyColumns;
        /// <summary>Gets the columns of unique keys associated with this table column.</summary>
        /// <value>The columns of unique keys associated with this table column.</value>
        public IEnumerable<UniqueKeyColumn> UniqueKeyColumns
        {
            get
            {
                if (this.tableColumn.UniqueKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.uniqueKeyColumns == null)
                {
                    this.uniqueKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IUniqueKeyColumn, UniqueKeyColumn>(
                        this.tableColumn.UniqueKeyColumns,
                        c => this.Table.UniqueKeys
                            .SelectMany(uk => uk.UniqueKeyColumns)
                            .First(ukc => ukc.InternalEquals(c))
                    );
                }

                foreach (var uniqueKeyColumn in this.uniqueKeyColumns)
                {
                    yield return uniqueKeyColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn> foreignKeyColumns;
        /// <summary>
        /// Gets the columns of foreign keys associated with this table column.
        /// <para>This table column is the foreign table column.
        /// This table column is referencing to another table column.</para></summary>
        /// <value>The columns of foreign keys associated with this table column.</value>
        public IEnumerable<ForeignKeyColumn> ForeignKeyColumns
        {
            get
            {
                if (this.tableColumn.ForeignKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.foreignKeyColumns == null)
                {
                    this.foreignKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn>(
                        this.tableColumn.ForeignKeyColumns,
                        c => this.Table.ForeignKeys
                            .SelectMany(fk => fk.ForeignKeyColumns)
                            .First(fkc => fkc.InternalEquals(c))
                    );
                }

                foreach (var foreignKeyColumn in this.foreignKeyColumns)
                {
                    yield return foreignKeyColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn> primaryForeignKeyColumns;
        /// <summary>
        /// Gets the columns of primary foreign keys associated with this table column.
        /// <para>Primary foreign key is a foreign key that is referencing from another table to the table of this column.</para>
        /// <para>This table column is the primary table column.
        /// This table column is referenced from another table column.</para></summary>
        /// <value>The columns of primary foreign keys associated with this table column.</value>
        public IEnumerable<ForeignKeyColumn> PrimaryForeignKeyColumns
        {
            get
            {
                if (this.tableColumn.PrimaryForeignKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.primaryForeignKeyColumns == null)
                {
                    this.primaryForeignKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn>(
                        this.tableColumn.PrimaryForeignKeyColumns,
                        c => this.Table.Database.Tables
                            .Union(this.Table.Database.AccessibleTables)
                            .Where(t => t != this.Table)
                            .SelectMany(t => t.ForeignKeys)
                            .SelectMany(fk => fk.ForeignKeyColumns)
                            .First(fkc => fkc.InternalEquals(c))
                    );
                }

                foreach (var primaryForeignKeyColumn in this.primaryForeignKeyColumns)
                {
                    yield return primaryForeignKeyColumn;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn> indexColumns;
        /// <summary>Gets the columns of table indexes associated with this table column.</summary>
        /// <value>The columns of table indexes associated with this table column.</value>
        public IEnumerable<TableIndexColumn> IndexColumns
        {
            get
            {
                if (this.tableColumn.IndexColumns.IsNullOrEmpty())
                    yield break;

                if (this.indexColumns == null)
                {
                    this.indexColumns = new CachedEnumerable<POCOGenerator.DbObjects.IIndexColumn, TableIndexColumn>(
                        this.tableColumn.IndexColumns,
                        c => this.Table.Indexes
                            .SelectMany(i => i.IndexColumns)
                            .First(ic => ic.InternalEquals(c))
                    );
                }

                foreach (var indexColumn in this.indexColumns)
                {
                    yield return indexColumn;
                }
            }
        }

        /// <summary>Gets the complex type column associated with this table column.
        /// <para>Returns <see langword="null" /> if this table column is not a part of a complex type.</para></summary>
        /// <value>The complex type column associated with this table column.</value>
        public ComplexTypeTableColumn ComplexTypeTableColumn
        {
            get
            {
                if (this.tableColumn.ComplexTypeTableColumn == null)
                    return null;

                return this.Table.ComplexTypeTables
                    .SelectMany(ctt => ctt.ComplexTypeTableColumns)
                    .First(cttc => cttc.InternalEquals(this.tableColumn.ComplexTypeTableColumn));
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

        /// <summary>Returns a robust string that represents this column.
        /// <para>Includes information whether the column is a primary key, has foreign keys and whether is a computed column.</para></summary>
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
