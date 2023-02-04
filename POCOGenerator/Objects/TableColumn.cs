using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
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

        public Table Table { get; private set; }

        public IDbObject DbObject { get { return this.Table; } }

        private PrimaryKeyColumn primaryKeyColumn;
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
