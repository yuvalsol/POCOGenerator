using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using POCOGenerator.Db.DbObjects;
using POCOGenerator.DbObjects;
using POCOGenerator.Utils;

namespace POCOGenerator.Db
{
    public abstract class DbHelper : IDbHelper
    {
        #region Constructor

        protected string ConnectionString;
        public virtual IDbSupport Support { get; protected set; }

        public DbHelper(string connectionString, IDbSupport support)
        {
            this.ConnectionString = connectionString;
            this.Support = support;
        }

        #endregion

        #region Connection, Command & Parameter

        protected abstract DbConnection GetConnection(string connectionString);

        protected abstract IDbCommand GetCommand();

        protected abstract IDataParameter GetParameter(IProcedureParameter parameter, IDatabase database);

        #endregion

        #region Server

        public virtual void BuildServerSchema(
            IServer server,
            string initialDatabase,
            bool isEnableTables,
            bool isEnableViews,
            bool isEnableStoredProcedures,
            bool isEnableFunctions,
            bool isEnableTVPs)
        {
            string serverVersion = GetServerVersion();

            POCOGenerator.DbObjects.Version version;
            if (POCOGenerator.DbObjects.Version.TryParse(serverVersion, out version))
            {
                server.Version = version;
                Support.Version = version;
            }

            GetServerConfiguration(server);

            server.Databases = GetDatabases(initialDatabase);

            RemoveSystemDatabases(server);

            foreach (var database in server.Databases)
            {
                database.Server = server;
                BuildDatabaseSchema(
                    database,
                    isEnableTables,
                    isEnableViews,
                    isEnableStoredProcedures,
                    isEnableFunctions,
                    isEnableTVPs
                );
            }

            SortDbObjects(server);
        }

        protected abstract string GetServerVersion();

        protected virtual void GetServerConfiguration(IServer server)
        {
        }

        protected virtual void SortDbObjects(IServer server)
        {
            if (server.Databases.HasAny())
            {
                server.Databases.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                foreach (var database in server.Databases)
                {
                    SortTables(database.Tables);
                    SortTables(database.Views);
                    SortProcedures(database.Procedures);
                    SortProcedures(database.Functions);
                    SortTVPs(database.TVPs);
                }
            }
        }

        protected virtual void SortTables<T>(List<T> tables) where T : ITable
        {
            if (tables.HasAny())
            {
                tables.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                foreach (var table in tables)
                {
                    if (table.TableColumns.HasAny())
                    {
                        table.TableColumns.Sort((x, y) => (x.ColumnOrdinal ?? 0).CompareTo(y.ColumnOrdinal ?? 0));

                        foreach (var tableColumn in table.TableColumns)
                        {
                            if (tableColumn.UniqueKeyColumns.HasAny())
                                tableColumn.UniqueKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));

                            if (tableColumn.ForeignKeyColumns.HasAny())
                                tableColumn.ForeignKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));

                            if (tableColumn.PrimaryForeignKeyColumns.HasAny())
                                tableColumn.PrimaryForeignKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));

                            if (tableColumn.IndexColumns.HasAny())
                                tableColumn.IndexColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                        }
                    }

                    if (table.PrimaryKey != null)
                    {
                        if (table.PrimaryKey.PrimaryKeyColumns.HasAny())
                            table.PrimaryKey.PrimaryKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                    }

                    if (table.UniqueKeys.HasAny())
                    {
                        table.UniqueKeys.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                        foreach (var uniqueKey in table.UniqueKeys)
                        {
                            if (uniqueKey.UniqueKeyColumns.HasAny())
                                uniqueKey.UniqueKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                        }
                    }

                    if (table.ForeignKeys.HasAny())
                    {
                        table.ForeignKeys.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                        foreach (var foreignKey in table.ForeignKeys)
                        {
                            if (foreignKey.ForeignKeyColumns.HasAny())
                                foreignKey.ForeignKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                        }
                    }

                    if (table.PrimaryForeignKeys.HasAny())
                    {
                        table.PrimaryForeignKeys.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                        // this is probably redundant
                        foreach (var primaryForeignKey in table.PrimaryForeignKeys)
                        {
                            if (primaryForeignKey.ForeignKeyColumns.HasAny())
                                primaryForeignKey.ForeignKeyColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                        }
                    }

                    if (table.Indexes.HasAny())
                    {
                        table.Indexes.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                        foreach (var index in table.Indexes)
                        {
                            if (index.IndexColumns.HasAny())
                                index.IndexColumns.Sort((x, y) => x.Ordinal.CompareTo(y.Ordinal));
                        }
                    }
                }
            }
        }

        protected virtual void SortProcedures<T>(List<T> procedures) where T : IProcedure
        {
            if (procedures.HasAny())
            {
                procedures.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                foreach (var procedure in procedures)
                {
                    if (procedure.ProcedureParameters.HasAny())
                        procedure.ProcedureParameters.Sort((x, y) => (x.ParameterOrdinal ?? 0).CompareTo(y.ParameterOrdinal ?? 0));

                    if (procedure.ProcedureColumns.HasAny())
                        procedure.ProcedureColumns.Sort((x, y) => (x.ColumnOrdinal ?? 0).CompareTo(y.ColumnOrdinal ?? 0));
                }
            }
        }

        protected virtual void SortTVPs(List<ITVP> tvps)
        {
            if (tvps.HasAny())
            {
                tvps.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                foreach (var tvp in tvps)
                {
                    if (tvp.TVPColumns.HasAny())
                        tvp.TVPColumns.Sort((x, y) => (x.ColumnOrdinal ?? 0).CompareTo(y.ColumnOrdinal ?? 0));
                }
            }
        }

        #endregion

        #region Databases

        protected virtual List<IDatabase> GetDatabases(string initialDatabase = null)
        {
            try
            {
                using (DbConnection connection = GetConnection(ConnectionString))
                {
                    connection.Open();
                    return
                        GetSchemaDatabases(connection)
                        .Where(d => string.IsNullOrEmpty(initialDatabase) || string.Compare(d.ToString(), initialDatabase, true) == 0)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get databases.", ex);
            }
        }

        protected abstract IEnumerable<IDatabase> GetSchemaDatabases(DbConnection connection);

        protected virtual void RemoveSystemDatabases(IServer server)
        {
        }

        protected virtual void BuildDatabaseSchema(
            IDatabase database,
            bool isEnableTables,
            bool isEnableViews,
            bool isEnableStoredProcedures,
            bool isEnableFunctions,
            bool isEnableTVPs)
        {
            database.Errors = new List<Exception>();

            List<ISystemObject> systemObjects = null;
            try
            {
                systemObjects = GetSystemObjects(database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(new Exception("Failed to get system objects.", ex));
            }

            List<IInternalKey> primaryKeysInternal = null;
            List<IInternalKey> uniqueKeysInternal = null;
            List<IInternalForeignKey> foreignKeysInternal = null;
            if (isEnableTables)
            {
                try
                {
                    var keys = GetKeys(database);
                    if (keys != null)
                    {
                        primaryKeysInternal = keys.Item1;
                        uniqueKeysInternal = keys.Item2;
                        foreignKeysInternal = keys.Item3;
                    }
                }
                catch (Exception ex)
                {
                    database.Errors.Add(new Exception("Failed to get primary, unique & foreign keys.", ex));
                }
            }

            List<IInternalIndex> indexesInternal = null;
            if (isEnableTables || isEnableViews)
            {
                try
                {
                    indexesInternal = GetIndexes(database);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(new Exception("Failed to get index columns.", ex));
                }
            }

            List<IIdentityColumn> identityColumns = null;
            if (isEnableTables)
            {
                try
                {
                    identityColumns = GetIdentityColumns(database);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(new Exception("Failed to get identity columns.", ex));
                }
            }

            List<IComputedColumn> computedColumns = null;
            if (isEnableTables)
            {
                try
                {
                    computedColumns = GetComputedColumns(database);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(new Exception("Failed to get computed columns.", ex));
                }
            }

            if (Support.IsSupportTVPs)
            {
                if (isEnableTVPs || isEnableStoredProcedures || (Support.IsSupportTableFunctions && isEnableFunctions))
                {
                    try
                    {
                        bool isBuildSchemaTVPsForOtherObjectTypes = (isEnableTVPs == false);
                        BuildSchemaTVPs(database, systemObjects);
                    }
                    catch (Exception ex)
                    {
                        database.Errors.Add(ex);
                    }
                }
            }

            if (isEnableTables)
            {
                try
                {
                    BuildSchemaTables(database, primaryKeysInternal, uniqueKeysInternal, foreignKeysInternal, indexesInternal, identityColumns, computedColumns, systemObjects);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(ex);
                }
            }

            if (isEnableViews)
            {
                try
                {
                    BuildSchemaViews(database, indexesInternal, systemObjects);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(ex);
                }
            }

            if (isEnableStoredProcedures)
            {
                try
                {
                    BuildSchemaProcedures(database, systemObjects);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(ex);
                }
            }

            if (Support.IsSupportTableFunctions)
            {
                if (isEnableFunctions)
                {
                    try
                    {
                        BuildSchemaFunctions(database, systemObjects);
                    }
                    catch (Exception ex)
                    {
                        database.Errors.Add(ex);
                    }
                }
            }

            if (Support.IsSupportTVPs)
            {
                if (isEnableTVPs == false && (isEnableStoredProcedures || (Support.IsSupportTableFunctions && isEnableFunctions)))
                {
                    database.TVPs = null;
                }
            }

            if (isEnableTables)
            {
                try
                {
                    BuildNavigationProperties(database);
                }
                catch (Exception ex)
                {
                    database.Errors.Add(ex);
                }
            }

            try
            {
                List<IDbObjectDescription> descriptions = GetDbObjectDescriptions(database);
                SetDbObjectDescriptions(database, descriptions);
            }
            catch (Exception ex)
            {
                database.Errors.Add(new Exception("Failed to get descriptions.", ex));
            }
        }

        #endregion

        #region System Objects

        protected virtual List<ISystemObject> GetSystemObjects(IDatabase database)
        {
            return null;
        }

        #endregion

        #region Descriptions

        protected abstract List<IDbObjectDescription> GetDbObjectDescriptions(IDatabase database);

        protected virtual void SetDbObjectDescriptions(IDatabase database, List<IDbObjectDescription> descriptions)
        {
            if (descriptions.HasAny())
            {
                database.Description = descriptions.Where(d =>
                    d.ObjectType == DbObjectType.Database &&
                    d.Name == database.ToString()
                ).Select(d => d.Description).FirstOrDefault();

                if (database.TVPs.HasAny())
                {
                    foreach (ITVP tvp in database.TVPs)
                    {
                        tvp.Description = descriptions.Where(d =>
                            d.ObjectType == DbObjectType.TVP &&
                            (d is ISchema == false || tvp is ISchema == false || ((ISchema)d).Schema == ((ISchema)tvp).Schema) &&
                            d.Name == tvp.Name
                        ).Select(d => d.Description).FirstOrDefault();

                        if (tvp.TVPColumns.HasAny())
                        {
                            foreach (ITVPColumn column in tvp.TVPColumns)
                            {
                                column.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.TVPColumn &&
                                    (d is ISchema == false || tvp is ISchema == false || ((ISchema)d).Schema == ((ISchema)tvp).Schema) &&
                                    d.Name == tvp.Name &&
                                    d.Minor_Name == column.ColumnName
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }
                    }
                }

                if (database.Tables.HasAny())
                {
                    foreach (ITable table in database.Tables)
                    {
                        table.Description = descriptions.Where(d =>
                            d.ObjectType == DbObjectType.Table &&
                            (d is ISchema == false || table is ISchema == false || ((ISchema)d).Schema == ((ISchema)table).Schema) &&
                            d.Name == table.Name
                        ).Select(d => d.Description).FirstOrDefault();

                        if (table.TableColumns.HasAny())
                        {
                            foreach (ITableColumn column in table.TableColumns)
                            {
                                column.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.Column &&
                                    (d is ISchema == false || table is ISchema == false || ((ISchema)d).Schema == ((ISchema)table).Schema) &&
                                    d.Name == table.Name &&
                                    d.Minor_Name == column.ColumnName
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }

                        if (table.Indexes.HasAny())
                        {
                            foreach (Index index in table.Indexes)
                            {
                                index.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.Index &&
                                    (d is ISchema == false || table is ISchema == false || ((ISchema)d).Schema == ((ISchema)table).Schema) &&
                                    d.Name == table.Name &&
                                    d.Minor_Name == index.Name
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }
                    }
                }

                if (database.Views.HasAny())
                {
                    foreach (IView view in database.Views)
                    {
                        view.Description = descriptions.Where(d =>
                            d.ObjectType == DbObjectType.View &&
                            (d is ISchema == false || view is ISchema == false || ((ISchema)d).Schema == ((ISchema)view).Schema) &&
                            d.Name == view.Name
                        ).Select(d => d.Description).FirstOrDefault();

                        if (view.TableColumns.HasAny())
                        {
                            foreach (ITableColumn column in view.TableColumns)
                            {
                                column.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.Column &&
                                    (d is ISchema == false || view is ISchema == false || ((ISchema)d).Schema == ((ISchema)view).Schema) &&
                                    d.Name == view.Name &&
                                    d.Minor_Name == column.ColumnName
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }

                        if (view.Indexes.HasAny())
                        {
                            foreach (Index index in view.Indexes)
                            {
                                index.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.Index &&
                                    (d is ISchema == false || view is ISchema == false || ((ISchema)d).Schema == ((ISchema)view).Schema) &&
                                    d.Name == view.Name &&
                                    d.Minor_Name == index.Name
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }
                    }
                }

                if (database.Procedures.HasAny())
                {
                    foreach (IProcedure procedure in database.Procedures)
                    {
                        procedure.Description = descriptions.Where(d =>
                            d.ObjectType == DbObjectType.Procedure &&
                            (d is ISchema == false || procedure is ISchema == false || ((ISchema)d).Schema == ((ISchema)procedure).Schema) &&
                            d.Name == procedure.Name
                        ).Select(d => d.Description).FirstOrDefault();

                        if (procedure.ProcedureParameters.HasAny())
                        {
                            foreach (IProcedureParameter parameter in procedure.ProcedureParameters)
                            {
                                parameter.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.ProcedureParameter &&
                                    (d is ISchema == false || procedure is ISchema == false || ((ISchema)d).Schema == ((ISchema)procedure).Schema) &&
                                    d.Name == procedure.Name &&
                                    d.Minor_Name == parameter.ParameterName
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }
                    }
                }

                if (database.Functions.HasAny())
                {
                    foreach (IFunction function in database.Functions)
                    {
                        function.Description = descriptions.Where(d =>
                            d.ObjectType == DbObjectType.Function &&
                            (d is ISchema == false || function is ISchema == false || ((ISchema)d).Schema == ((ISchema)function).Schema) &&
                            d.Name == function.Name
                        ).Select(d => d.Description).FirstOrDefault();

                        if (function.ProcedureParameters.HasAny())
                        {
                            foreach (IProcedureParameter parameter in function.ProcedureParameters)
                            {
                                parameter.Description = descriptions.Where(d =>
                                    d.ObjectType == DbObjectType.ProcedureParameter &&
                                    (d is ISchema == false || function is ISchema == false || ((ISchema)d).Schema == ((ISchema)function).Schema) &&
                                    d.Name == function.Name &&
                                    d.Minor_Name == parameter.ParameterName
                                ).Select(d => d.Description).FirstOrDefault();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Primary, Unique & Foreign Keys

        protected abstract Tuple<List<IInternalKey>, List<IInternalKey>, List<IInternalForeignKey>> GetKeys(IDatabase database);

        #endregion

        #region Indexes

        protected abstract List<IInternalIndex> GetIndexes(IDatabase database);

        #endregion

        #region Identity Columns

        protected abstract List<IIdentityColumn> GetIdentityColumns(IDatabase database);

        #endregion

        #region Computed Columns

        protected abstract List<IComputedColumn> GetComputedColumns(IDatabase database);

        #endregion

        #region TVPs

        protected virtual void BuildSchemaTVPs(IDatabase database, List<ISystemObject> systemObjects = null)
        {
            try
            {
                database.TVPs = GetTVPs(database);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get TVPs.", ex);
            }

            RemoveSystemObjectsFromTVPs(database, systemObjects);

            foreach (var tvp in database.TVPs)
            {
                tvp.Database = database;
                GetTVPSchema(tvp);
            }
        }

        protected virtual List<ITVP> GetTVPs(IDatabase database)
        {
            return new List<ITVP>(0);
        }

        protected virtual void RemoveSystemObjectsFromTVPs(IDatabase database, List<ISystemObject> systemObjects)
        {
        }

        protected virtual void GetTVPSchema(ITVP tvp)
        {
            try
            {
                tvp.TVPColumns = GetTVPColumns(tvp);
            }
            catch (Exception ex)
            {
                tvp.Error = ex;
            }

            if (tvp.TVPColumns != null)
                tvp.TVPColumns.ForEach(c => c.TVP = tvp);
        }

        protected virtual List<ITVPColumn> GetTVPColumns(ITVP tvp)
        {
            return new List<ITVPColumn>(0);
        }

        protected virtual DataTable GetTVPDataTable(ITVP tvp)
        {
            return null;
        }

        #endregion

        #region Tables

        protected virtual void BuildSchemaTables(
            IDatabase database,
            List<IInternalKey> primaryKeysInternal,
            List<IInternalKey> uniqueKeysInternal,
            List<IInternalForeignKey> foreignKeysInternal,
            List<IInternalIndex> indexesInternal,
            List<IIdentityColumn> identityColumns,
            List<IComputedColumn> computedColumns,
            List<ISystemObject> systemObjects = null)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Tables = GetSchemaTables(connection, database.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get tables.", ex);
                }
            }

            RemoveSystemObjectsFromTables(database, systemObjects);

            foreach (var table in database.Tables)
            {
                table.Database = database;
                GetTableSchema(table, primaryKeysInternal, uniqueKeysInternal, indexesInternal, identityColumns, computedColumns);
            }

            SetDatabaseForeignKeys(database, foreignKeysInternal);
        }

        protected abstract IEnumerable<ITable> GetSchemaTables(DbConnection connection, string database);

        protected virtual void RemoveSystemObjectsFromTables(IDatabase database, List<ISystemObject> systemObjects)
        {
        }

        protected virtual void GetTableSchema(
            ITable table,
            List<IInternalKey> primaryKeysInternal,
            List<IInternalKey> uniqueKeysInternal,
            List<IInternalIndex> indexesInternal,
            List<IIdentityColumn> identityColumns,
            List<IComputedColumn> computedColumns)
        {
            string connectionString = table.Database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    table.TableColumns = GetSchemaTableColumns(connection, table).ToList();

                    SetPrimaryKeys(table, primaryKeysInternal);

                    SetUniqueKeys(table, uniqueKeysInternal);

                    SetIndexes(table, indexesInternal, true, false);

                    SetIdentityAndComputedColumns(table, identityColumns, computedColumns);
                }
                catch (Exception ex)
                {
                    table.Error = ex;
                }
            }

            if (table.TableColumns != null)
                table.TableColumns.ForEach(c => c.Table = table);
        }

        protected abstract IEnumerable<ITableColumn> GetSchemaTableColumns(DbConnection connection, ITable table);

        protected virtual void SetPrimaryKeys(ITable table, List<IInternalKey> primaryKeysInternal)
        {
            if (primaryKeysInternal.HasAny())
            {
                var primaryKeyColumns = primaryKeysInternal.Where(pk =>
                    (pk is ISchema == false || table is ISchema == false || ((ISchema)pk).Schema == ((ISchema)table).Schema) &&
                    pk.Table_Name == table.Name
                ).ToList();

                if (primaryKeyColumns.HasAny())
                {
                    var pk = new PrimaryKey()
                    {
                        Name = primaryKeyColumns.First().Name,
                        Table = table
                    };

                    table.PrimaryKey = pk;

                    pk.PrimaryKeyColumns = table.TableColumns.Join(primaryKeyColumns, c => c.ColumnName, pkc => pkc.Column_Name, (c, pkc) =>
                    {
                        return c.PrimaryKeyColumn = new PrimaryKeyColumn()
                        {
                            PrimaryKey = table.PrimaryKey,
                            TableColumn = c,
                            Ordinal = pkc.Ordinal
                        };
                    })
                    .ToList();
                }
            }
        }

        protected virtual void SetUniqueKeys(ITable table, List<IInternalKey> uniqueKeysInternal)
        {
            if (uniqueKeysInternal.HasAny())
            {
                var uniqueKeys = uniqueKeysInternal.Where(uk =>
                    (uk is ISchema == false || table is ISchema == false || ((ISchema)uk).Schema == ((ISchema)table).Schema) &&
                    uk.Table_Name == table.Name
                )
                .GroupBy(ukc => new { ukc.Name })
                .ToList();

                if (uniqueKeys.HasAny())
                {
                    table.UniqueKeys = uniqueKeys.Select(uniqueKey =>
                    {
                        var uk = new UniqueKey()
                        {
                            Name = uniqueKey.Key.Name,
                            Table = table
                        };

                        uk.UniqueKeyColumns = table.TableColumns.Join(uniqueKey, c => c.ColumnName, ukc => ukc.Column_Name, (c, ukc) =>
                        {
                            var uniqueKeyColumn = new UniqueKeyColumn()
                            {
                                UniqueKey = uk,
                                TableColumn = c,
                                Ordinal = ukc.Ordinal
                            };

                            if (c.UniqueKeyColumns == null)
                                c.UniqueKeyColumns = new List<IUniqueKeyColumn>();
                            c.UniqueKeyColumns.Add(uniqueKeyColumn);

                            return uniqueKeyColumn;
                        })
                        .Cast<IUniqueKeyColumn>()
                        .ToList();

                        return uk;
                    })
                    .Cast<IUniqueKey>()
                    .ToList();
                }
            }
        }

        protected virtual void SetIndexes(ITable table, List<IInternalIndex> indexesInternal, bool isTableIndex, bool isViewIndex)
        {
            if (indexesInternal.HasAny())
            {
                var indexes = indexesInternal.Where(i =>
                    i.Is_Table_Index == isTableIndex &&
                    i.Is_View_Index == isViewIndex &&
                    (i is ISchema == false || table is ISchema == false || ((ISchema)i).Schema == ((ISchema)table).Schema) &&
                    i.Table_Name == table.Name
                )
                .GroupBy(ic => new
                {
                    ic.Name,
                    ic.Is_Unique,
                    ic.Is_Clustered
                })
                .ToList();

                if (indexes.HasAny())
                {
                    table.Indexes = indexes.Select(index =>
                    {
                        var i = new Index()
                        {
                            Name = index.Key.Name,
                            Is_Unique = index.Key.Is_Unique,
                            Is_Clustered = index.Key.Is_Clustered,
                            Table = table
                        };

                        i.IndexColumns = table.TableColumns.Join(index, c => c.ColumnName, ic => ic.Column_Name, (c, ic) =>
                        {
                            var indexColumn = new IndexColumn()
                            {
                                Index = i,
                                TableColumn = c,
                                Ordinal = ic.Ordinal,
                                Is_Descending = ic.Is_Descending
                            };

                            if (c.IndexColumns == null)
                                c.IndexColumns = new List<IIndexColumn>();
                            c.IndexColumns.Add(indexColumn);

                            return indexColumn;
                        })
                        .Cast<IIndexColumn>()
                       .ToList();

                        return i;
                    })
                    .Cast<IIndex>()
                    .ToList();
                }
            }
        }

        protected virtual void SetIdentityAndComputedColumns(ITable table, List<IIdentityColumn> identityColumns, List<IComputedColumn> computedColumns)
        {
            if (identityColumns.HasAny() || computedColumns.HasAny())
            {
                foreach (ITableColumn column in table.TableColumns)
                {
                    if (identityColumns.HasAny())
                    {
                        column.IsIdentity = identityColumns.Exists(ic =>
                            (ic is ISchema == false || table is ISchema == false || ((ISchema)ic).Schema == ((ISchema)table).Schema) &&
                            ic.Table_Name == table.Name &&
                            ic.Column_Name == column.ColumnName
                        );
                    }

                    if (computedColumns.HasAny())
                    {
                        column.IsComputed = computedColumns.Exists(cc =>
                            (cc is ISchema == false || table is ISchema == false || ((ISchema)cc).Schema == ((ISchema)table).Schema) &&
                            cc.Table_Name == table.Name &&
                            cc.Column_Name == column.ColumnName
                        );
                    }
                }
            }
        }

        protected virtual void SetDatabaseForeignKeys(IDatabase database, List<IInternalForeignKey> foreignKeysInternal)
        {
            foreach (var table in database.Tables)
            {
                SetForeignKeys(table, foreignKeysInternal);
            }
        }

        protected virtual void SetForeignKeys(ITable table, List<IInternalForeignKey> foreignKeysInternal)
        {
            if (foreignKeysInternal.HasAny())
            {
                var foreignKeys = foreignKeysInternal.Where(fk =>
                    (fk is IInternalForeignKeySchema == false || table is ISchema == false || ((IInternalForeignKeySchema)fk).Foreign_Schema == ((ISchema)table).Schema) &&
                    fk.Foreign_Table == table.Name
                )
                .GroupBy(fkc => new
                {
                    fkc.Name,
                    fkc.Is_One_To_One,
                    fkc.Is_One_To_Many,
                    fkc.Is_Many_To_Many,
                    fkc.Is_Many_To_Many_Complete,
                    fkc.Is_Cascade_Delete,
                    fkc.Is_Cascade_Update,
                    Primary_Schema = (fkc is ForeignKeySchemaInternal ? ((ForeignKeySchemaInternal)fkc).Primary_Schema : null),
                    fkc.Primary_Table
                })
                .ToList();

                foreach (var foreignKey in foreignKeys)
                {
                    ITable primaryTable = table.Database.Tables.FirstOrDefault(pt =>
                        (foreignKey.Key.Primary_Schema == null || pt is ISchema == false || foreignKey.Key.Primary_Schema == ((ISchema)pt).Schema) &&
                        foreignKey.Key.Primary_Table == pt.Name
                    );

                    if (primaryTable == null)
                        continue;

                    var fk = new ForeignKey()
                    {
                        Name = foreignKey.Key.Name,
                        Is_One_To_One = foreignKey.Key.Is_One_To_One,
                        Is_One_To_Many = foreignKey.Key.Is_One_To_Many,
                        Is_Many_To_Many = foreignKey.Key.Is_Many_To_Many,
                        Is_Many_To_Many_Complete = foreignKey.Key.Is_Many_To_Many_Complete,
                        Is_Cascade_Delete = foreignKey.Key.Is_Cascade_Delete,
                        Is_Cascade_Update = foreignKey.Key.Is_Cascade_Update,
                        ForeignTable = table,
                        PrimaryTable = primaryTable,
                    };

                    fk.ForeignKeyColumns = table.TableColumns.Join(foreignKey, c => c.ColumnName, fkc => fkc.Foreign_Column, (c, fkc) =>
                    {
                        ITableColumn primaryTableColumn = primaryTable.TableColumns.First(pc => pc.ColumnName == fkc.Primary_Column);

                        var foreignKeyColumn = new ForeignKeyColumn()
                        {
                            ForeignKey = fk,
                            ForeignTableColumn = c,
                            PrimaryTableColumn = primaryTableColumn,
                            Is_Foreign_Column_PK = fkc.Is_Foreign_Column_PK,
                            Is_Primary_Column_PK = fkc.Is_Primary_Column_PK,
                            Ordinal = fkc.Ordinal
                        };

                        if (c.ForeignKeyColumns == null)
                            c.ForeignKeyColumns = new List<IForeignKeyColumn>();
                        c.ForeignKeyColumns.Add(foreignKeyColumn);

                        if (primaryTableColumn.PrimaryForeignKeyColumns == null)
                            primaryTableColumn.PrimaryForeignKeyColumns = new List<IForeignKeyColumn>();
                        primaryTableColumn.PrimaryForeignKeyColumns.Add(foreignKeyColumn);

                        return foreignKeyColumn;
                    })
                    .Cast<IForeignKeyColumn>()
                    .ToList();

                    if (table.ForeignKeys == null)
                        table.ForeignKeys = new List<IForeignKey>();
                    table.ForeignKeys.Add(fk);

                    if (primaryTable.PrimaryForeignKeys == null)
                        primaryTable.PrimaryForeignKeys = new List<IForeignKey>();
                    primaryTable.PrimaryForeignKeys.Add(fk);
                }
            }
        }

        #endregion

        #region Views

        protected virtual void BuildSchemaViews(IDatabase database, List<IInternalIndex> indexesInternal, List<ISystemObject> systemObjects = null)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Views = GetSchemaViews(connection, database.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get views.", ex);
                }
            }

            RemoveSystemObjectsFromViews(database, systemObjects);

            foreach (var view in database.Views)
            {
                view.Database = database;
                GetViewSchema(view, indexesInternal);
            }
        }

        protected abstract IEnumerable<IView> GetSchemaViews(DbConnection connection, string database);

        protected virtual void RemoveSystemObjectsFromViews(IDatabase database, List<ISystemObject> systemObjects)
        {
        }

        protected virtual void GetViewSchema(IView view, List<IInternalIndex> indexesInternal)
        {
            string connectionString = view.Database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    view.TableColumns = GetSchemaViewColumns(connection, view).ToList();

                    SetIndexes(view, indexesInternal, false, true);
                }
                catch (Exception ex)
                {
                    view.Error = ex;
                }
            }

            if (view.TableColumns != null)
                view.TableColumns.ForEach(c => c.Table = view);
        }

        protected abstract IEnumerable<ITableColumn> GetSchemaViewColumns(DbConnection connection, IView view);

        #endregion

        #region Procedures

        protected virtual void BuildSchemaProcedures(IDatabase database, List<ISystemObject> systemObjects = null)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Procedures = GetSchemaProcedures(connection, database.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get procedures.", ex);
                }
            }

            RemoveSystemObjectsFromProcedures(database, systemObjects);

            foreach (var procedure in database.Procedures)
            {
                procedure.Database = database;
                GetProcedureSchema(procedure);
            }
        }

        protected abstract IEnumerable<IProcedure> GetSchemaProcedures(DbConnection connection, string database);

        protected virtual void RemoveSystemObjectsFromProcedures(IDatabase database, List<ISystemObject> systemObjects)
        {
        }

        protected virtual void GetProcedureSchema(IProcedure procedure)
        {
            string connectionString = procedure.Database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    procedure.ProcedureParameters = GetSchemaProcedureParameters(connection, procedure).ToList();
                    procedure.ProcedureColumns = GetProcedureSchema(procedure, connection);
                }
                catch (Exception ex)
                {
                    GetProcedureSchemaSecondTry(procedure, connection, ex);
                }
            }

            if (procedure.ProcedureParameters != null)
                procedure.ProcedureParameters.ForEach(p => p.Procedure = procedure);

            if (procedure.ProcedureColumns != null)
                procedure.ProcedureColumns.ForEach(c => c.Procedure = procedure);
        }

        protected abstract IEnumerable<IProcedureParameter> GetSchemaProcedureParameters(DbConnection connection, IProcedure procedure);

        protected virtual List<IProcedureColumn> GetProcedureSchema(IProcedure procedure, DbConnection connection)
        {
            using (IDbCommand command = GetCommand())
            {
                command.Connection = connection;
                command.CommandText = GetProcedureCommandText(procedure);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                foreach (IProcedureParameter parameter in procedure.ProcedureParameters)
                {
                    if (parameter.IsResult == false)
                        command.Parameters.Add(GetParameter(parameter, procedure.Database));
                }

                return GetProcedureSchema(command);
            }
        }

        protected abstract string GetProcedureCommandText(IProcedure procedure);

        protected virtual List<IProcedureColumn> GetProcedureSchema(IDbCommand command)
        {
            using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
            {
                DataTable schemaTable = reader.GetSchemaTable();
                if (schemaTable != null)
                    return GetSchemaProcedureColumns(schemaTable).ToList();
                else
                    return null;
            }
        }

        protected abstract IEnumerable<IProcedureColumn> GetSchemaProcedureColumns(DataTable schemaTable);

        protected virtual void GetProcedureSchemaSecondTry(IProcedure procedure, DbConnection connection, Exception ex)
        {
            procedure.Error = ex;
        }

        #endregion

        #region Functions

        protected virtual void BuildSchemaFunctions(IDatabase database, List<ISystemObject> systemObjects = null)
        {
            string connectionString = database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Functions = GetSchemaFunctions(connection, database.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get functions.", ex);
                }
            }

            RemoveSystemObjectsFromFunctions(database, systemObjects);

            List<IFunction> scalarFunctions = new List<IFunction>();

            foreach (var function in database.Functions)
            {
                function.Database = database;
                bool isScalarFunction = GetFunctionSchema(function);
                if (isScalarFunction)
                    scalarFunctions.Add(function);
            }

            database.Functions = database.Functions.Except(scalarFunctions).ToList();
        }

        protected virtual IEnumerable<IFunction> GetSchemaFunctions(DbConnection connection, string database)
        {
            return new IFunction[0];
        }

        protected virtual void RemoveSystemObjectsFromFunctions(IDatabase database, List<ISystemObject> systemObjects)
        {
        }

        protected virtual bool GetFunctionSchema(IFunction function)
        {
            bool isScalarFunction = true;

            string connectionString = function.Database.GetDatabaseConnectionString(ConnectionString);
            using (DbConnection connection = GetConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    function.ProcedureParameters = GetSchemaFunctionParameters(connection, function).ToList();

                    isScalarFunction = function.ProcedureParameters.Where(param => param.IsResult).HasSingle();
                    if (isScalarFunction == false)
                        function.ProcedureColumns = GetFunctionSchema(function, connection);
                }
                catch (Exception ex)
                {
                    function.Error = ex;
                }
            }

            if (isScalarFunction == false)
            {
                if (function.ProcedureParameters != null)
                    function.ProcedureParameters.ForEach(p => p.Procedure = function);

                if (function.ProcedureColumns != null)
                    function.ProcedureColumns.ForEach(c => c.Procedure = function);
            }

            return isScalarFunction;
        }

        protected virtual IEnumerable<IProcedureParameter> GetSchemaFunctionParameters(DbConnection connection, IFunction function)
        {
            return new IProcedureParameter[0];
        }

        protected virtual List<IProcedureColumn> GetFunctionSchema(IFunction function, DbConnection connection)
        {
            using (IDbCommand command = GetCommand())
            {
                command.Connection = connection;
                command.CommandText = GetFunctionCommandText(function);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 60;

                foreach (IProcedureParameter parameter in function.ProcedureParameters)
                {
                    if (parameter.IsResult == false)
                        command.Parameters.Add(GetParameter(parameter, function.Database));
                }

                using (IDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    DataTable schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                        return GetSchemaFunctionColumns(schemaTable).ToList();
                    else
                        return null;
                }
            }
        }

        protected virtual string GetFunctionCommandText(IFunction function)
        {
            return null;
        }

        protected virtual IEnumerable<IProcedureColumn> GetSchemaFunctionColumns(DataTable schemaTable)
        {
            return new IProcedureColumn[0];
        }

        #endregion

        #region Navigation Properties

        protected virtual void BuildNavigationProperties(IDatabase database)
        {
            if (database.Tables.HasAny())
            {
                foreach (ITable table in database.Tables)
                {
                    if (table.ForeignKeys.HasAny())
                    {
                        foreach (ForeignKey fk in table.ForeignKeys)
                        {
                            fk.NavigationPropertyFromForeignToPrimary = GetNavigationPropertyFromForeignToPrimary(fk);
                            fk.NavigationPropertyFromPrimaryToForeign = GetNavigationPropertyFromPrimaryToForeign(fk);

                            ((NavigationProperty)fk.NavigationPropertyFromPrimaryToForeign).InverseProperty = fk.NavigationPropertyFromForeignToPrimary;
                        }
                    }
                }

                BuildVirtualNavigationProperties(database);
            }
        }

        protected virtual void BuildVirtualNavigationProperties(IDatabase database)
        {
            if (database.Tables.HasAny())
            {
                var virtualNPs =

                    // all tables
                    database.Tables

                    // all the foreign keys
                    .Where(table => table.ForeignKeys.HasAny())
                    .SelectMany(table => table.ForeignKeys)

                    // the fk is part of a many-to-many relationship
                    .Where(fk => fk.Is_Many_To_Many)

                    // complete many-to-many relationship
                    // all the columns in the join table are part of a pk of other tables. (otherwise the join table is treated as one-to-many relationship)
                    .Where(fk => fk.Is_Many_To_Many_Complete)

                    // the foreign table is the join table
                    .GroupBy(fk => fk.ForeignTable)

                    // pair every fk with any other fk (in the same group of foreign keys that have the same join table)
                    .SelectMany(g => g.SelectMany(fk => g.Except(new IForeignKey[] { fk }), (thisFK, otherFK) => new { thisFK, otherFK }))

                    // before: otherFK.PrimaryTable <-- otherFK -- join table (thisFK.ForeignTable) -- thisFK --> thisFK.PrimaryTable
                    // after:  otherFK.PrimaryTable                        -- virtualFK -->                       thisFK.PrimaryTable
                    .Select(item =>
                    {
                        item.thisFK.ForeignTable.IsJoinTable = true;

                        var virtualFK = new ForeignKey();
                        virtualFK.IsVirtualForeignKey = true;
                        virtualFK.Name = string.Empty;
                        virtualFK.Is_One_To_One = false;
                        virtualFK.Is_One_To_Many = false;
                        virtualFK.Is_Many_To_Many = true;
                        virtualFK.Is_Many_To_Many_Complete = false;
                        virtualFK.Is_Cascade_Delete = false;
                        virtualFK.Is_Cascade_Update = false;

                        virtualFK.ForeignTable = item.otherFK.PrimaryTable;
                        virtualFK.PrimaryTable = item.thisFK.PrimaryTable;

                        // dummy foreign key column
                        // the columns don't have to be the same data type
                        var fkc = item.otherFK.ForeignKeyColumns.First();
                        var pkc = item.thisFK.ForeignKeyColumns.First();
                        virtualFK.ForeignKeyColumns = new List<IForeignKeyColumn>()
                        {
                            new ForeignKeyColumn()
                            {
                                IsVirtualForeignKeyColumn = true,
                                ForeignKey = virtualFK,
                                ForeignTableColumn = fkc.PrimaryTableColumn,
                                PrimaryTableColumn = pkc.PrimaryTableColumn,
                                Is_Foreign_Column_PK = true,
                                Is_Primary_Column_PK = true,
                                Ordinal = 1
                            }
                        };

                        // the concrete non-virtual navigation properties are visible when ShowManyToManyJoinTable is on
                        // they are not visible when ShowManyToManyJoinTable is off

                        // ShowManyToManyJoinTable = true -> show join table navigation properties
                        // ShowManyToManyJoinTable = false -> hide join table navigation properties

                        NavigationProperty otherFK_NavigationPropertyFromForeignToPrimary = (NavigationProperty)item.otherFK.NavigationPropertyFromForeignToPrimary;
                        otherFK_NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOn = true;
                        otherFK_NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOff = false;

                        NavigationProperty otherFK_NavigationPropertyFromPrimaryToForeign = (NavigationProperty)item.otherFK.NavigationPropertyFromPrimaryToForeign;
                        otherFK_NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOn = true;
                        otherFK_NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOff = false;

                        NavigationProperty thisFK_NavigationPropertyFromForeignToPrimary = (NavigationProperty)item.thisFK.NavigationPropertyFromForeignToPrimary;
                        thisFK_NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOn = true;
                        thisFK_NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOff = false;

                        NavigationProperty thisFK_NavigationPropertyFromPrimaryToForeign = (NavigationProperty)item.thisFK.NavigationPropertyFromPrimaryToForeign;
                        thisFK_NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOn = true;
                        thisFK_NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOff = false;

                        return new { item.thisFK, virtualFK };
                    })

                    // virtual navigation property from primary to foreign
                    .Select(item =>
                    {
                        var virtualNP = (NavigationProperty)GetNavigationPropertyFromPrimaryToForeign(item.virtualFK, true);
                        var virtualInverseProperty = (NavigationProperty)GetNavigationPropertyFromForeignToPrimary(item.virtualFK, true);

                        // the virtual navigation properties are visible when ShowManyToManyJoinTable is off
                        // they are not visible when ShowManyToManyJoinTable is on

                        // ShowManyToManyJoinTable = true -> hide virtual navigation properties
                        // ShowManyToManyJoinTable = false -> show virtual navigation properties

                        virtualNP.IsVisibleWhenShowManyToManyJoinTableIsOn = false;
                        virtualNP.IsVisibleWhenShowManyToManyJoinTableIsOff = true;

                        virtualInverseProperty.IsVisibleWhenShowManyToManyJoinTableIsOn = false;
                        virtualInverseProperty.IsVisibleWhenShowManyToManyJoinTableIsOff = true;

                        return new
                        {
                            item.thisFK,
                            virtualNP,
                            virtualInverseProperty
                        };
                    })

                    .GroupBy(item => new { item.thisFK })
                    .ToList();

                foreach (var group in virtualNPs)
                {
                    foreach (var item in group)
                    {
                        var virtualNP = item.virtualNP;
                        var virtualInverseProperty = item.virtualInverseProperty;
                        var virtualFK = (ForeignKey)virtualNP.ForeignKey;

                        virtualFK.NavigationPropertyFromPrimaryToForeign = virtualNP;
                        virtualFK.NavigationPropertyFromForeignToPrimary = virtualInverseProperty;
                        virtualNP.InverseProperty = virtualInverseProperty;
                    }

                    var thisFK = (ForeignKey)group.Key.thisFK;
                    thisFK.VirtualNavigationProperties = group.Select(x => x.virtualNP).Cast<INavigationProperty>().ToList();
                }
            }
        }

        protected virtual INavigationProperty GetNavigationPropertyFromForeignToPrimary(IForeignKey fk, bool isVirtualNavigationProperty = false)
        {
            return new NavigationProperty()
            {
                ForeignKey = fk,
                IsFromForeignToPrimary = true,
                IsCollection = false,
                PropertyName = GetNavigationPropertyPrimaryPropertyName(fk),
                IsVirtualNavigationProperty = isVirtualNavigationProperty,
                IsVisibleWhenShowManyToManyJoinTableIsOn = true,
                IsVisibleWhenShowManyToManyJoinTableIsOff = true
            };
        }

        protected virtual INavigationProperty GetNavigationPropertyFromPrimaryToForeign(IForeignKey fk, bool isVirtualNavigationProperty = false)
        {
            return new NavigationProperty()
            {
                ForeignKey = fk,
                IsFromForeignToPrimary = false,
                IsCollection = (fk.Is_One_To_One == false),
                PropertyName = GetNavigationPropertyForeignPropertyName(fk, fk.Is_One_To_One == false),
                IsVirtualNavigationProperty = isVirtualNavigationProperty,
                IsVisibleWhenShowManyToManyJoinTableIsOn = true,
                IsVisibleWhenShowManyToManyJoinTableIsOff = true
            };
        }

        #region Navigation Property Name

        protected virtual string GetNavigationPropertyPrimaryPropertyName(IForeignKey fk, bool isCollection = false)
        {
            string propertyName = fk.PrimaryTable.Name;

            string primaryColumn = fk.ForeignKeyColumns[0].PrimaryTableColumn.ColumnName;

            // ef convention <navigation property Name>_<primary key property name of navigation property type>
            string foreignColumn = fk.ForeignKeyColumns[0].ForeignTableColumn.ColumnName;
            if (foreignColumn.EndsWith("_" + primaryColumn))
                propertyName = foreignColumn.Substring(0, foreignColumn.LastIndexOf("_" + primaryColumn));

            propertyName = NameHelper.AddNamePrefix(propertyName, primaryColumn);

            if (NameHelper.IsNameVerb(propertyName))
            {
                propertyName = NameHelper.ConjugateNameVerbToPastParticiple(propertyName);
            }
            else
            {
                if (isCollection)
                    propertyName = NameHelper.GetPluralName(propertyName);
                else
                    propertyName = NameHelper.GetSingularName(propertyName);
            }

            propertyName = NameHelper.TransformName(propertyName);
            propertyName = NameHelper.CleanName(propertyName);
            return propertyName;
        }

        protected virtual string GetNavigationPropertyForeignPropertyName(IForeignKey fk, bool isCollection = true)
        {
            string propertyName = fk.ForeignTable.Name;

            string foreignColumn = fk.ForeignKeyColumns[0].ForeignTableColumn.ColumnName;
            propertyName = NameHelper.AddNamePrefix(propertyName, foreignColumn);

            if (NameHelper.IsNameVerb(propertyName))
            {
                propertyName = NameHelper.ConjugateNameVerbToPastParticiple(propertyName);
            }
            else
            {
                if (isCollection)
                    propertyName = NameHelper.GetPluralName(propertyName);
                else
                    propertyName = NameHelper.GetSingularName(propertyName);
            }

            propertyName = NameHelper.TransformName(propertyName);
            propertyName = NameHelper.CleanName(propertyName);
            return propertyName;
        }

        #endregion

        #endregion

        #region Helper Methods

        protected static string GetScript(Type type, string scriptName)
        {
            using (var stream = type.Assembly.GetManifestResourceStream(string.Format("{0}.Scripts.{1}", type.Namespace, scriptName)))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
