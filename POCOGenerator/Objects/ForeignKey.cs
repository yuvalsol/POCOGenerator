using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class ForeignKey
    {
        private readonly POCOGenerator.DbObjects.IForeignKey foreignKey;

        internal ForeignKey(POCOGenerator.DbObjects.IForeignKey foreignKey, Table foreignTable)
        {
            this.foreignKey = foreignKey;
            this.ForeignTable = foreignTable;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IForeignKey foreignKey)
        {
            return this.foreignKey == foreignKey;
        }

        public Table ForeignTable { get; private set; }

        public string Name { get { return this.foreignKey.Name; } }
        public bool Is_One_To_One { get { return this.foreignKey.Is_One_To_One; } }
        public bool Is_One_To_Many { get { return this.foreignKey.Is_One_To_Many; } }
        public bool Is_Many_To_Many { get { return this.foreignKey.Is_Many_To_Many; } }
        public bool Is_Many_To_Many_Complete { get { return this.foreignKey.Is_Many_To_Many_Complete; } }
        public bool Is_Cascade_Delete { get { return this.foreignKey.Is_Cascade_Delete; } }
        public bool Is_Cascade_Update { get { return this.foreignKey.Is_Cascade_Update; } }

        private Table primaryTable;
        public Table PrimaryTable
        {
            get
            {
                if (this.foreignKey.PrimaryTable == null)
                    return null;

                if (this.primaryTable == null)
                {
                    this.primaryTable =
                        this.ForeignTable.Database.Tables
                        .Union(this.ForeignTable.Database.AccessibleTables)
                        .First(t => t.InternalEquals(this.foreignKey.PrimaryTable));
                }

                return this.primaryTable;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn> foreignKeyColumns;
        public IEnumerable<ForeignKeyColumn> ForeignKeyColumns
        {
            get
            {
                if (this.foreignKey.ForeignKeyColumns.IsNullOrEmpty())
                    yield break;

                if (this.foreignKeyColumns == null)
                    this.foreignKeyColumns = new CachedEnumerable<POCOGenerator.DbObjects.IForeignKeyColumn, ForeignKeyColumn>(this.foreignKey.ForeignKeyColumns, fkc => new ForeignKeyColumn(fkc, this));

                foreach (var foreignKeyColumn in this.foreignKeyColumns)
                {
                    yield return foreignKeyColumn;
                }
            }
        }

        public override string ToString()
        {
            return this.foreignKey.ToString();
        }
    }

    public sealed class ForeignKeyColumn
    {
        private readonly POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn;

        internal ForeignKeyColumn(POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn, ForeignKey foreignKey)
        {
            this.foreignKeyColumn = foreignKeyColumn;
            this.ForeignKey = foreignKey;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IForeignKeyColumn foreignKeyColumn)
        {
            return this.foreignKeyColumn == foreignKeyColumn;
        }

        public ForeignKey ForeignKey { get; private set; }

        private TableColumn foreignTableColumn;
        public TableColumn ForeignTableColumn
        {
            get
            {
                if (this.foreignTableColumn == null)
                    this.foreignTableColumn = this.ForeignKey.ForeignTable.TableColumns.First(c => c.InternalEquals(this.foreignKeyColumn.ForeignTableColumn));

                return this.foreignTableColumn;
            }
        }

        private TableColumn primaryTableColumn;
        public TableColumn PrimaryTableColumn
        {
            get
            {
                if (this.foreignKeyColumn.PrimaryTableColumn == null)
                    return null;

                if (this.primaryTableColumn == null)
                    this.primaryTableColumn = this.ForeignKey.PrimaryTable.TableColumns.First(c => c.InternalEquals(this.foreignKeyColumn.PrimaryTableColumn));

                return this.primaryTableColumn;
            }
        }

        public bool Is_Foreign_Column_PK { get { return this.foreignKeyColumn.Is_Foreign_Column_PK; } }
        public bool Is_Primary_Column_PK { get { return this.foreignKeyColumn.Is_Primary_Column_PK; } }
        public int Ordinal { get { return this.foreignKeyColumn.Ordinal; } }

        public override string ToString()
        {
            return this.foreignKeyColumn.ToString();
        }

        public string ToFullString()
        {
            return this.foreignKeyColumn.ToFullString();
        }
    }
}
