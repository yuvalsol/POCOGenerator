using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a database table.</summary>
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

        /// <summary>Gets a value indicating whether this table is a many-to-many join table.</summary>
        /// <value>
        ///   <c>true</c> if this table is a many-to-many join table; otherwise, <c>false</c>.</value>
        public bool IsJoinTable { get { return this.table.IsJoinTable; } }

        internal string ClassName { get { return this.table.ClassName; } }

        /// <summary>Gets the error message that occurred during the generating process of this table.</summary>
        /// <value>The error message that occurred during the generating process of this table.</value>
        public string Error { get { return this.table.Error?.Message; } }

        /// <summary>Gets the database that this table belongs to.</summary>
        /// <value>The database that this table belongs to.</value>
        public Database Database { get; private set; }

        /// <summary>Gets the collection of database columns that belong to this table.</summary>
        /// <value>Collection of database columns.</value>
        public IEnumerable<IDbColumn> Columns
        {
            get
            {
                return this.TableColumns;
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITableColumn, TableColumn> tableColumns;
        /// <summary>Gets the columns of the table.</summary>
        /// <value>The columns of the table.</value>
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
        /// <summary>Gets the primary key of the table.
        /// <para>Returns <see langword="null" /> if the table has no primary key.</para></summary>
        /// <value>The primary key of the table.</value>
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
        /// <summary>Gets the unique keys of the table.</summary>
        /// <value>The unique keys of the table.</value>
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
        /// <summary>Gets the foreign keys of the table.</summary>
        /// <value>The foreign keys of the table.</value>
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
        /// <summary>Gets the navigation properties of the table.</summary>
        /// <value>The navigation properties of the table.</value>
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
        /// <summary>Gets the indexes of the table.</summary>
        /// <value>The indexes of the table.</value>
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
        /// <summary>Gets the complex types of the table.</summary>
        /// <value>The complex types of the table.</value>
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

        /// <summary>Gets the name of the table.</summary>
        /// <value>The name of the table.</value>
        public string Name { get { return this.table.Name; } }

        /// <summary>Gets the schema of the table.
        /// <para>Returns <see langword="null" /> if the RDBMS doesn't support schema.</para></summary>
        /// <value>The schema of the table.</value>
        /// <seealso cref="Support.SupportSchema" />
        public string Schema
        {
            get
            {
                if (this.table is POCOGenerator.DbObjects.ISchema schema)
                    return schema.Schema;
                return null;
            }
        }

        /// <summary>Gets the description of the table.</summary>
        /// <value>The description of the table.</value>
        public string Description { get { return this.table.Description; } }

        /// <summary>Returns a string that represents this table.</summary>
        /// <returns>A string that represents this table.</returns>
        public override string ToString()
        {
            return this.table.ToString();
        }
    }
}
