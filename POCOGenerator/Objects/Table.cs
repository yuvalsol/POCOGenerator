using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class Table : IDbObject
    {
        private readonly POCOGenerator.DbObjects.ITable table;

        internal Table(POCOGenerator.DbObjects.ITable table, Database database)
        {
            this.table = table;
            this.Database = database;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.ITable table)
        {
            return this.table == table;
        }

        public bool IsJoinTable { get { return this.table.IsJoinTable; } }

        internal string ClassName { get { return this.table.ClassName; } }
        public string Error { get { return (this.table.Error != null ? this.table.Error.Message : null); } }

        public Database Database { get; private set; }

        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.TableColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn> tableColumns;
        public IEnumerable<TableColumn> TableColumns
        {
            get
            {
                if (this.table.TableColumns.IsNullOrEmpty())
                    yield break;

                if (this.tableColumns == null)
                    this.tableColumns = new CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn>(this.table.TableColumns, c => new TableColumn(c, this));

                foreach (var tableColumn in this.tableColumns)
                {
                    yield return tableColumn;
                }
            }
        }

        private PrimaryKey primaryKey;
        public PrimaryKey PrimaryKey
        {
            get
            {
                if (this.table.PrimaryKey == null)
                    return null;

                if (this.primaryKey == null)
                    this.primaryKey = new PrimaryKey(this.table.PrimaryKey, this);

                return this.primaryKey;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IUniqueKey, UniqueKey> uniqueKeys;
        public IEnumerable<UniqueKey> UniqueKeys
        {
            get
            {
                if (this.table.UniqueKeys.IsNullOrEmpty())
                    yield break;

                if (this.uniqueKeys == null)
                    this.uniqueKeys = new CachedEnumerable<POCOGenerator.DbObjects.IUniqueKey, UniqueKey>(this.table.UniqueKeys, uk => new UniqueKey(uk, this));

                foreach (var uniqueKey in this.uniqueKeys)
                {
                    yield return uniqueKey;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IForeignKey, ForeignKey> foreignKeys;
        public IEnumerable<ForeignKey> ForeignKeys
        {
            get
            {
                if (this.table.ForeignKeys.IsNullOrEmpty())
                    yield break;

                if (this.foreignKeys == null)
                    this.foreignKeys = new CachedEnumerable<POCOGenerator.DbObjects.IForeignKey, ForeignKey>(this.table.ForeignKeys, fk => new ForeignKey(fk, this));

                foreach (var foreignKey in this.foreignKeys)
                {
                    yield return foreignKey;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty> navigationPropertiesSingular;
        private CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty> navigationPropertiesCollection;
        private CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty> virtualNavigationProperties;
        public IEnumerable<NavigationProperty> NavigationProperties
        {
            get
            {
                if (this.table.ForeignKeys.IsNullOrEmpty() && this.table.PrimaryForeignKeys.IsNullOrEmpty())
                    yield break;

                if (this.table.ForeignKeys.HasAny())
                {
                    if (this.navigationPropertiesSingular == null)
                    {
                        this.navigationPropertiesSingular = new CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty>(
                            this.table.ForeignKeys.Select(fk => fk.NavigationPropertyFromForeignToPrimary),
                            np => new NavigationProperty(np, this)
                        );
                    }

                    foreach (var navigationProperty in this.navigationPropertiesSingular)
                    {
                        yield return navigationProperty;
                    }
                }

                if (this.table.PrimaryForeignKeys.HasAny())
                {
                    if (this.navigationPropertiesCollection == null)
                    {
                        this.navigationPropertiesCollection = new CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty>(
                            this.table.PrimaryForeignKeys.Select(fk => fk.NavigationPropertyFromPrimaryToForeign),
                            np => new NavigationProperty(np, this)
                        );
                    }

                    foreach (var navigationProperty in this.navigationPropertiesCollection)
                    {
                        yield return navigationProperty;
                    }

                    if (this.virtualNavigationProperties == null)
                    {
                        this.virtualNavigationProperties = new CachedEnumerable<POCOGenerator.DbObjects.INavigationProperty, NavigationProperty>(
                            this.table.PrimaryForeignKeys.Where(fk => fk.VirtualNavigationProperties.HasAny()).SelectMany(fk => fk.VirtualNavigationProperties),
                            np => new NavigationProperty(np, this)
                        );
                    }

                    foreach (var virtualNavigationProperty in this.virtualNavigationProperties)
                    {
                        yield return virtualNavigationProperty;
                    }
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IIndex, TableIndex> indexes;
        public IEnumerable<TableIndex> Indexes
        {
            get
            {
                if (this.table.Indexes.IsNullOrEmpty())
                    yield break;

                if (this.indexes == null)
                    this.indexes = new CachedEnumerable<POCOGenerator.DbObjects.IIndex, TableIndex>(this.table.Indexes, i => new TableIndex(i, this));

                foreach (var index in this.indexes)
                {
                    yield return index;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTable, ComplexTypeTable> complexTypeTables;
        public IEnumerable<ComplexTypeTable> ComplexTypeTables
        {
            get
            {
                if (this.table.ComplexTypeTables.IsNullOrEmpty())
                    yield break;

                if (this.complexTypeTables == null)
                {
                    this.complexTypeTables = new CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTable, ComplexTypeTable>(
                        this.table.ComplexTypeTables,
                         ctt1 => this.Database.ComplexTypeTables.First(ctt2 => ctt2.InternalEquals(ctt1))
                    );
                }

                foreach (var complexTypeTable in this.complexTypeTables)
                {
                    yield return complexTypeTable;
                }
            }
        }

        public string Name { get { return this.table.Name; } }

        public string Schema
        {
            get
            {
                if (this.table is POCOGenerator.DbObjects.ISchema)
                    return ((POCOGenerator.DbObjects.ISchema)this.table).Schema;
                return null;
            }
        }

        public string Description { get { return this.table.Description; } }

        public override string ToString()
        {
            return this.table.ToString();
        }
    }
}
