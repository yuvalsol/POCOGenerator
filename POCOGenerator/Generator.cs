using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using POCOGenerator.DbFactories;
using POCOGenerator.DbHandlers;
using POCOGenerator.DbObjects;
using POCOGenerator.POCOIterators;
using POCOGenerator.POCOWriters;

namespace POCOGenerator
{
    internal class Generator : IGenerator
    {
        internal readonly object lockObject = new object();

        internal Generator(Func<IWriter> createWriter)
        {
            this.createWriter = createWriter;
            this.settings = new GeneratorSettings(this.lockObject);
        }

        #region Settings

        private GeneratorSettings settings;
        private GeneratorSettings settingsInternal;

        public Settings Settings
        {
            get
            {
                lock (this.lockObject)
                {
                    return this.settings;
                }
            }
        }

        #endregion

        private bool isServerBuilt;
        private IDbHandler dbHandler;
        private IDbHelper dbHelper;
        internal Func<IWriter> createWriter;
        private Func<IWriter> createWriterInternal;
        private POCOGenerator.Objects.Server proxyServer;
        private List<IDbObjectTraverse> dbObjectsToGenerate;

        public Support Support { get; private set; }

        public Exception Error { get; private set; }

        public GeneratorResults Generate()
        {
            try
            {
                // clear the generator error
                this.Error = null;

                // copy the settings and event to internal counterparts
                SetInternalSettingsAndEvents();

                // clear data members
                this.isServerBuilt = false;
                this.dbHandler = null;
                this.dbHelper = null;
                this.Support = null;
                this.proxyServer = null;
                this.dbObjectsToGenerate = null;

                // validate connection string
                string serverName = null;
                string instanceName = null;
                string userId = null;
                string initialDatabase = null;
                GeneratorResults results = ValidateConnectionString(ref this.dbHandler, ref serverName, ref instanceName, ref userId, ref initialDatabase);
                if (results != GeneratorResults.None)
                    return results;

                // create new server
                IServer server = this.dbHandler.GetServer();
                server.ServerName = serverName;
                server.InstanceName = instanceName;
                server.UserId = userId;

                // determine which object types to generate and which ones to skip, based on the user's settings
                bool isEnableTables = IsEnableDbObjects(this.settingsInternal.IncludeAll, this.settingsInternal.Tables.IncludeAll, this.settingsInternal.Tables.ExcludeAll, this.settingsInternal.Tables.Include);
                bool isEnableViews = IsEnableDbObjects(this.settingsInternal.IncludeAll, this.settingsInternal.Views.IncludeAll, this.settingsInternal.Views.ExcludeAll, this.settingsInternal.Views.Include);
                bool isEnableStoredProcedures = IsEnableDbObjects(this.settingsInternal.IncludeAll, this.settingsInternal.StoredProcedures.IncludeAll, this.settingsInternal.StoredProcedures.ExcludeAll, this.settingsInternal.StoredProcedures.Include);
                bool isEnableFunctions = IsEnableDbObjects(this.settingsInternal.IncludeAll, this.settingsInternal.Functions.IncludeAll, this.settingsInternal.Functions.ExcludeAll, this.settingsInternal.Functions.Include);
                bool isEnableTVPs = IsEnableDbObjects(this.settingsInternal.IncludeAll, this.settingsInternal.TVPs.IncludeAll, this.settingsInternal.TVPs.ExcludeAll, this.settingsInternal.TVPs.Include);

                // get db helper
                this.dbHelper = this.dbHandler.GetDbHelper(this.settingsInternal.ConnectionString);

                // create generator support
                this.Support = new GeneratorSupport(this.dbHelper.Support);

                // disable function and tvp rendering, if the db doesn't support it
                isEnableFunctions &= this.dbHelper.Support.IsSupportTableFunctions;
                isEnableTVPs &= this.dbHelper.Support.IsSupportTVPs;

                // build server schema
                this.dbHelper.BuildServerSchema(
                    server,
                    initialDatabase,
                    isEnableTables,
                    isEnableViews,
                    isEnableStoredProcedures,
                    isEnableFunctions,
                    isEnableTVPs
                );

                // get all the accessible objects, including the ones that are not listed in the user's settings
                // tables that are accessible through foreign keys or complex types
                List<DatabaseAccessibleObjects> databasesAccessibleObjects = GetDatabasesAccessibleObjects(server);

                // set the db objects to generate
                SetDbObjectsToGenerate(databasesAccessibleObjects);

                // create proxy server
                this.proxyServer = new POCOGenerator.Objects.Server(server, databasesAccessibleObjects);

                // the server is built
                this.isServerBuilt = true;

                // fire server built async event
                this.serverBuiltAsyncInternal.RaiseAsync(this, () => new ServerBuiltAsyncEventArgs(this.proxyServer));

                // fire server built sync event
                var args = this.serverBuiltInternal.Raise(this, () => new ServerBuiltEventArgs(this.proxyServer));

                // stop the generator if the event args Stop was set to true
                if (args != null && args.Stop)
                    return GeneratorResults.None;

                // generate classes
                return IterateDbObjects();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                return GeneratorResults.UnexpectedError;
            }
            finally
            {
                ClearInternalSettingsAndEvents();
            }
        }

        public GeneratorResults GeneratePOCOs()
        {
            if (this.isServerBuilt)
            {
                try
                {
                    // clear the generator error
                    this.Error = null;

                    // copy the settings and event to internal counterparts
                    SetInternalSettingsAndEvents();

                    // generate classes
                    return IterateDbObjects();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    return GeneratorResults.UnexpectedError;
                }
                finally
                {
                    ClearInternalSettingsAndEvents();
                }
            }
            else
            {
                return Generate();
            }
        }

        private GeneratorResults IterateDbObjects()
        {
            // create writer
            IWriter writer = this.createWriterInternal();

            // set the writer syntax colors
            if (writer is ISyntaxHighlight)
            {
                var syntaxHighlight = (ISyntaxHighlight)writer;
                syntaxHighlight.Text = this.settingsInternal.SyntaxHighlight.Text;
                syntaxHighlight.Keyword = this.settingsInternal.SyntaxHighlight.Keyword;
                syntaxHighlight.UserType = this.settingsInternal.SyntaxHighlight.UserType;
                syntaxHighlight.String = this.settingsInternal.SyntaxHighlight.String;
                syntaxHighlight.Comment = this.settingsInternal.SyntaxHighlight.Comment;
                syntaxHighlight.Error = this.settingsInternal.SyntaxHighlight.Error;
                syntaxHighlight.Background = this.settingsInternal.SyntaxHighlight.Background;
            }

            // create iterator
            IDbIterator iterator = this.dbHandler.GetIterator(writer, this.dbHelper.Support, this.settingsInternal);

            // set iterator events
            SetPOCOIteratorEvents(iterator, this.proxyServer);

            // generate classes
            iterator.Iterate(this.dbObjectsToGenerate);

            if (this.dbObjectsToGenerate.IsNullOrEmpty())
                return GeneratorResults.NoDbObjectsIncluded;

            return GeneratorResults.None;
        }

        private void SetInternalSettingsAndEvents()
        {
            lock (this.lockObject)
            {
                this.settingsInternal = (GeneratorSettings)this.settings.Clone();

                this.createWriterInternal = this.createWriter;

                this.serverBuiltAsyncInternal = this.serverBuiltAsync;
                this.serverBuiltInternal = this.serverBuilt;
                this.serverGeneratingAsyncInternal = this.serverGeneratingAsync;
                this.serverGeneratingInternal = this.serverGenerating;
                this.databaseGeneratingAsyncInternal = this.databaseGeneratingAsync;
                this.databaseGeneratingInternal = this.databaseGenerating;
                this.tablesGeneratingAsyncInternal = this.tablesGeneratingAsync;
                this.tablesGeneratingInternal = this.tablesGenerating;
                this.tableGeneratingAsyncInternal = this.tableGeneratingAsync;
                this.tableGeneratingInternal = this.tableGenerating;
                this.tablePOCOAsyncInternal = this.tablePOCOAsync;
                this.tablePOCOInternal = this.tablePOCO;
                this.tableGeneratedAsyncInternal = this.tableGeneratedAsync;
                this.tableGeneratedInternal = this.tableGenerated;
                this.tablesGeneratedAsyncInternal = this.tablesGeneratedAsync;
                this.tablesGeneratedInternal = this.tablesGenerated;
                this.complexTypeTablesGeneratingAsyncInternal = this.complexTypeTablesGeneratingAsync;
                this.complexTypeTablesGeneratingInternal = this.complexTypeTablesGenerating;
                this.complexTypeTableGeneratingAsyncInternal = this.complexTypeTableGeneratingAsync;
                this.complexTypeTableGeneratingInternal = this.complexTypeTableGenerating;
                this.complexTypeTablePOCOAsyncInternal = this.complexTypeTablePOCOAsync;
                this.complexTypeTablePOCOInternal = this.complexTypeTablePOCO;
                this.complexTypeTableGeneratedAsyncInternal = this.complexTypeTableGeneratedAsync;
                this.complexTypeTableGeneratedInternal = this.complexTypeTableGenerated;
                this.complexTypeTablesGeneratedAsyncInternal = this.complexTypeTablesGeneratedAsync;
                this.complexTypeTablesGeneratedInternal = this.complexTypeTablesGenerated;
                this.viewsGeneratingAsyncInternal = this.viewsGeneratingAsync;
                this.viewsGeneratingInternal = this.viewsGenerating;
                this.viewGeneratingAsyncInternal = this.viewGeneratingAsync;
                this.viewGeneratingInternal = this.viewGenerating;
                this.viewPOCOAsyncInternal = this.viewPOCOAsync;
                this.viewPOCOInternal = this.viewPOCO;
                this.viewGeneratedAsyncInternal = this.viewGeneratedAsync;
                this.viewGeneratedInternal = this.viewGenerated;
                this.viewsGeneratedAsyncInternal = this.viewsGeneratedAsync;
                this.viewsGeneratedInternal = this.viewsGenerated;
                this.proceduresGeneratingAsyncInternal = this.proceduresGeneratingAsync;
                this.proceduresGeneratingInternal = this.proceduresGenerating;
                this.procedureGeneratingAsyncInternal = this.procedureGeneratingAsync;
                this.procedureGeneratingInternal = this.procedureGenerating;
                this.procedurePOCOAsyncInternal = this.procedurePOCOAsync;
                this.procedurePOCOInternal = this.procedurePOCO;
                this.procedureGeneratedAsyncInternal = this.procedureGeneratedAsync;
                this.procedureGeneratedInternal = this.procedureGenerated;
                this.proceduresGeneratedAsyncInternal = this.proceduresGeneratedAsync;
                this.proceduresGeneratedInternal = this.proceduresGenerated;
                this.functionsGeneratingAsyncInternal = this.functionsGeneratingAsync;
                this.functionsGeneratingInternal = this.functionsGenerating;
                this.functionGeneratingAsyncInternal = this.functionGeneratingAsync;
                this.functionGeneratingInternal = this.functionGenerating;
                this.functionPOCOAsyncInternal = this.functionPOCOAsync;
                this.functionPOCOInternal = this.functionPOCO;
                this.functionGeneratedAsyncInternal = this.functionGeneratedAsync;
                this.functionGeneratedInternal = this.functionGenerated;
                this.functionsGeneratedAsyncInternal = this.functionsGeneratedAsync;
                this.functionsGeneratedInternal = this.functionsGenerated;
                this.tvpsGeneratingAsyncInternal = this.tvpsGeneratingAsync;
                this.tvpsGeneratingInternal = this.tvpsGenerating;
                this.tvpGeneratingAsyncInternal = this.tvpGeneratingAsync;
                this.tvpGeneratingInternal = this.tvpGenerating;
                this.tvpPOCOAsyncInternal = this.tvpPOCOAsync;
                this.tvpPOCOInternal = this.tvpPOCO;
                this.tvpGeneratedAsyncInternal = this.tvpGeneratedAsync;
                this.tvpGeneratedInternal = this.tvpGenerated;
                this.tvpsGeneratedAsyncInternal = this.tvpsGeneratedAsync;
                this.tvpsGeneratedInternal = this.tvpsGenerated;
                this.databaseGeneratedAsyncInternal = this.databaseGeneratedAsync;
                this.databaseGeneratedInternal = this.databaseGenerated;
                this.serverGeneratedAsyncInternal = this.serverGeneratedAsync;
                this.serverGeneratedInternal = this.serverGenerated;
            }
        }

        private void ClearInternalSettingsAndEvents()
        {
            this.settingsInternal = null;

            this.createWriterInternal = null;

            this.serverBuiltAsyncInternal = null;
            this.serverBuiltInternal = null;
            this.serverGeneratingAsyncInternal = null;
            this.serverGeneratingInternal = null;
            this.databaseGeneratingAsyncInternal = null;
            this.databaseGeneratingInternal = null;
            this.tablesGeneratingAsyncInternal = null;
            this.tablesGeneratingInternal = null;
            this.tableGeneratingAsyncInternal = null;
            this.tableGeneratingInternal = null;
            this.tablePOCOAsyncInternal = null;
            this.tablePOCOInternal = null;
            this.tableGeneratedAsyncInternal = null;
            this.tableGeneratedInternal = null;
            this.tablesGeneratedAsyncInternal = null;
            this.tablesGeneratedInternal = null;
            this.complexTypeTablesGeneratingAsyncInternal = null;
            this.complexTypeTablesGeneratingInternal = null;
            this.complexTypeTableGeneratingAsyncInternal = null;
            this.complexTypeTableGeneratingInternal = null;
            this.complexTypeTablePOCOAsyncInternal = null;
            this.complexTypeTablePOCOInternal = null;
            this.complexTypeTableGeneratedAsyncInternal = null;
            this.complexTypeTableGeneratedInternal = null;
            this.complexTypeTablesGeneratedAsyncInternal = null;
            this.complexTypeTablesGeneratedInternal = null;
            this.viewsGeneratingAsyncInternal = null;
            this.viewsGeneratingInternal = null;
            this.viewGeneratingAsyncInternal = null;
            this.viewGeneratingInternal = null;
            this.viewPOCOAsyncInternal = null;
            this.viewPOCOInternal = null;
            this.viewGeneratedAsyncInternal = null;
            this.viewGeneratedInternal = null;
            this.viewsGeneratedAsyncInternal = null;
            this.viewsGeneratedInternal = null;
            this.proceduresGeneratingAsyncInternal = null;
            this.proceduresGeneratingInternal = null;
            this.procedureGeneratingAsyncInternal = null;
            this.procedureGeneratingInternal = null;
            this.procedurePOCOAsyncInternal = null;
            this.procedurePOCOInternal = null;
            this.procedureGeneratedAsyncInternal = null;
            this.procedureGeneratedInternal = null;
            this.proceduresGeneratedAsyncInternal = null;
            this.proceduresGeneratedInternal = null;
            this.functionsGeneratingAsyncInternal = null;
            this.functionsGeneratingInternal = null;
            this.functionGeneratingAsyncInternal = null;
            this.functionGeneratingInternal = null;
            this.functionPOCOAsyncInternal = null;
            this.functionPOCOInternal = null;
            this.functionGeneratedAsyncInternal = null;
            this.functionGeneratedInternal = null;
            this.functionsGeneratedAsyncInternal = null;
            this.functionsGeneratedInternal = null;
            this.tvpsGeneratingAsyncInternal = null;
            this.tvpsGeneratingInternal = null;
            this.tvpGeneratingAsyncInternal = null;
            this.tvpGeneratingInternal = null;
            this.tvpPOCOAsyncInternal = null;
            this.tvpPOCOInternal = null;
            this.tvpGeneratedAsyncInternal = null;
            this.tvpGeneratedInternal = null;
            this.tvpsGeneratedAsyncInternal = null;
            this.tvpsGeneratedInternal = null;
            this.databaseGeneratedAsyncInternal = null;
            this.databaseGeneratedInternal = null;
            this.serverGeneratedAsyncInternal = null;
            this.serverGeneratedInternal = null;
        }

        private IDbHandler GetDbHandler(RDBMS rdbms)
        {
            if (rdbms == RDBMS.SQLServer)
                return DbFactory.Instance.SQLServerHandler;
            else if (rdbms == RDBMS.MySQL)
                return DbFactory.Instance.MySQLHandler;
            return null;
        }

        private GeneratorResults ValidateConnectionString(ref IDbHandler dbHandler, ref string serverName, ref string instanceName, ref string userId, ref string initialDatabase)
        {
            if (string.IsNullOrEmpty(this.settingsInternal.ConnectionString))
                return GeneratorResults.ConnectionStringMissing;

            if (this.settingsInternal.RDBMS == RDBMS.None)
            {
                var items = Enum.GetValues(typeof(RDBMS))
                    .Cast<RDBMS>()
                    .Where(rdbms => rdbms != RDBMS.None)
                    .Where(rdbms => GetDbHandler(rdbms).GetConnectionStringParser().Validate(this.settingsInternal.ConnectionString))
                    .ToArray();

                if (items.Length == 0)
                {
                    return GeneratorResults.ConnectionStringNotMatchAnyRDBMS;
                }
                else if (items.Length == 1)
                {
                    this.settingsInternal.RDBMS = items[0];
                    dbHandler = GetDbHandler(this.settingsInternal.RDBMS);

                    if (dbHandler.GetConnectionStringParser().Ping(this.settingsInternal.ConnectionString) == false)
                    {
                        dbHandler = null;
                        return GeneratorResults.ServerNotResponding;
                    }
                }
                else if (items.Length > 1)
                {
                    items = items.Where(rdbms => GetDbHandler(rdbms).GetConnectionStringParser().Ping(this.settingsInternal.ConnectionString)).ToArray();
                    if (items.Length == 0)
                    {
                        return GeneratorResults.ConnectionStringNotMatchAnyRDBMS;
                    }
                    else if (items.Length == 1)
                    {
                        this.settingsInternal.RDBMS = items[0];
                        dbHandler = GetDbHandler(this.settingsInternal.RDBMS);
                    }
                    else if (items.Length > 1)
                    {
                        return GeneratorResults.ConnectionStringMatchMoreThanOneRDBMS;
                    }
                }
            }
            else
            {
                dbHandler = GetDbHandler(this.settingsInternal.RDBMS);

                if (dbHandler.GetConnectionStringParser().Validate(this.settingsInternal.ConnectionString) == false)
                {
                    dbHandler = null;
                    return GeneratorResults.ConnectionStringMalformed;
                }

                if (dbHandler.GetConnectionStringParser().Ping(this.settingsInternal.ConnectionString) == false)
                {
                    dbHandler = null;
                    return GeneratorResults.ServerNotResponding;
                }
            }

            this.settingsInternal.ConnectionString = dbHandler.GetConnectionStringParser().Fix(this.settingsInternal.ConnectionString);

            bool integratedSecurity = false;
            dbHandler.GetConnectionStringParser().Parse(this.settingsInternal.ConnectionString, ref serverName, ref initialDatabase, ref userId, ref integratedSecurity);

            if (string.IsNullOrEmpty(serverName))
                return GeneratorResults.ConnectionStringMalformed;

            int index = serverName.LastIndexOf("\\");
            if (index != -1)
            {
                instanceName = serverName.Substring(index + 1);
                serverName = serverName.Substring(0, index);
            }

            if (integratedSecurity)
                userId = WindowsIdentity.GetCurrent().Name;

            return GeneratorResults.None;
        }

        private bool IsEnableDbObjects(bool isIncludeAll, bool isIncludeAllDbObjects, bool isExcludeAllDbObjects, IList<string> includeDbObjects)
        {
            return
                (isIncludeAll || isIncludeAllDbObjects || includeDbObjects.HasAny()) &&
                (isExcludeAllDbObjects == false);
        }

        #region Accessible Db Objects

        private List<DatabaseAccessibleObjects> GetDatabasesAccessibleObjects(IServer server)
        {
            List<DatabaseAccessibleObjects> databasesAccessibleObjects = server.Databases.Select(database => new DatabaseAccessibleObjects()
            {
                Database = database,
                Tables = GetDatabaseObjects(
                    database.Tables,
                    this.settingsInternal.IncludeAll,
                    this.settingsInternal.Tables.IncludeAll,
                    this.settingsInternal.Tables.Include,
                    this.settingsInternal.Tables.ExcludeAll,
                    this.settingsInternal.Tables.Exclude
                ),
                Views = GetDatabaseObjects(
                    database.Views,
                    this.settingsInternal.IncludeAll,
                    this.settingsInternal.Views.IncludeAll,
                    this.settingsInternal.Views.Include,
                    this.settingsInternal.Views.ExcludeAll,
                    this.settingsInternal.Views.Exclude
                ),
                Procedures = GetDatabaseObjects(
                    database.Procedures,
                    this.settingsInternal.IncludeAll,
                    this.settingsInternal.StoredProcedures.IncludeAll,
                    this.settingsInternal.StoredProcedures.Include,
                    this.settingsInternal.StoredProcedures.ExcludeAll,
                    this.settingsInternal.StoredProcedures.Exclude
                ),
                Functions = GetDatabaseObjects(
                    database.Functions,
                    this.settingsInternal.IncludeAll,
                    this.settingsInternal.Functions.IncludeAll,
                    this.settingsInternal.Functions.Include,
                    this.settingsInternal.Functions.ExcludeAll,
                    this.settingsInternal.Functions.Exclude
                ),
                TVPs = GetDatabaseObjects(
                    database.TVPs,
                    this.settingsInternal.IncludeAll,
                    this.settingsInternal.TVPs.IncludeAll,
                    this.settingsInternal.TVPs.Include,
                    this.settingsInternal.TVPs.ExcludeAll,
                    this.settingsInternal.TVPs.Exclude
                ),
            })
            .Where(d =>
                d.Tables.HasAny() ||
                d.Views.HasAny() ||
                d.Procedures.HasAny() ||
                d.Functions.HasAny() ||
                d.TVPs.HasAny())
            .ToList();

            SetDatabaseAccessibleTables(databasesAccessibleObjects);

            databasesAccessibleObjects.Sort((x, y) => x.Database.ToString().CompareTo(y.Database.ToString()));

            foreach (var item in databasesAccessibleObjects)
            {
                if (item.Tables.HasAny())
                    item.Tables.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                if (item.AccessibleTables.HasAny())
                    item.AccessibleTables.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                if (item.Views.HasAny())
                    item.Views.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                if (item.Procedures.HasAny())
                    item.Procedures.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                if (item.Functions.HasAny())
                    item.Functions.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

                if (item.TVPs.HasAny())
                    item.TVPs.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
            }

            return databasesAccessibleObjects;
        }

        private List<T> GetDatabaseObjects<T>(
            List<T> dbObjects,
            bool isIncludeAll,
            bool isIncludeAllDbObjects,
            IList<string> includeDbObjects,
            bool isExcludeAllDbObjects,
            IList<string> excludeDbObjects) where T : IDbObjectTraverse
        {
            if (dbObjects.HasAny())
            {
                if (isExcludeAllDbObjects == false)
                {
                    if (isIncludeAll || isIncludeAllDbObjects)
                    {
                        if (excludeDbObjects.IsNullOrEmpty())
                        {
                            return dbObjects.ToList();
                        }
                        else
                        {
                            var excludePatterns = excludeDbObjects.Select(eo => "^" + Regex.Escape(eo).Replace(@"\*", ".*?").Replace(@"\?", ".") + "$").ToArray();
                            return dbObjects.Where(o => excludePatterns.All(pattern => Regex.IsMatch(o.ToString(), pattern) == false && Regex.IsMatch(o.Name, pattern) == false)).ToList();
                        }
                    }
                    else if (includeDbObjects.HasAny())
                    {
                        var includePatterns = includeDbObjects.Select(io => "^" + Regex.Escape(io).Replace(@"\*", ".*?").Replace(@"\?", ".") + "$").ToArray();
                        var includedObjects = dbObjects.Where(o => includePatterns.Any(pattern => Regex.IsMatch(o.ToString(), pattern) || Regex.IsMatch(o.Name, pattern)));

                        if (excludeDbObjects.IsNullOrEmpty())
                        {
                            return includedObjects.ToList();
                        }
                        else
                        {
                            var excludePatterns = excludeDbObjects.Select(eo => "^" + Regex.Escape(eo).Replace(@"\*", ".*?").Replace(@"\?", ".") + "$").ToArray();
                            return includedObjects.Where(o => excludePatterns.All(pattern => Regex.IsMatch(o.ToString(), pattern) == false && Regex.IsMatch(o.Name, pattern) == false)).ToList();
                        }
                    }
                }
            }

            return null;
        }

        private void SetDatabaseAccessibleTables(List<DatabaseAccessibleObjects> databasesAccessibleObjects)
        {
            foreach (var item in databasesAccessibleObjects)
            {
                if (item.Tables.HasAny())
                {
                    item.AccessibleTables = new List<ITable>(item.Tables);

                    int fromIndex = 0;
                    int tablesCount = item.AccessibleTables.Count;
                    bool isContinue = true;

                    while (isContinue)
                    {
                        for (int i = fromIndex; i < tablesCount; i++)
                        {
                            ITable table = item.AccessibleTables[i];

                            if (table.ForeignKeys.HasAny())
                            {
                                foreach (var foreignKey in table.ForeignKeys)
                                {
                                    if (item.AccessibleTables.Contains(foreignKey.PrimaryTable) == false)
                                        item.AccessibleTables.Add(foreignKey.PrimaryTable);
                                }
                            }

                            if (table.ComplexTypeTables.HasAny())
                            {
                                foreach (var complexTypeTable in table.ComplexTypeTables)
                                {
                                    foreach (var t in complexTypeTable.Tables)
                                    {
                                        if (item.AccessibleTables.Contains(t) == false)
                                            item.AccessibleTables.Add(t);
                                    }
                                }
                            }
                        }

                        isContinue = (tablesCount < item.AccessibleTables.Count);

                        fromIndex = tablesCount;
                        tablesCount = item.AccessibleTables.Count;
                    }

                    item.AccessibleTables.RemoveRange(0, item.Tables.Count);

                    if (item.AccessibleTables.IsNullOrEmpty())
                        item.AccessibleTables = null;
                }
            }
        }

        private void SetDbObjectsToGenerate(List<DatabaseAccessibleObjects> databasesAccessibleObjects)
        {
            if (databasesAccessibleObjects.HasAny())
            {
                this.dbObjectsToGenerate = new List<IDbObjectTraverse>();

                foreach (var item in databasesAccessibleObjects)
                {
                    if (item.Tables.HasAny())
                        this.dbObjectsToGenerate.AddRange(item.Tables);

                    if (item.Views.HasAny())
                        this.dbObjectsToGenerate.AddRange(item.Views);

                    if (item.Procedures.HasAny())
                        this.dbObjectsToGenerate.AddRange(item.Procedures);

                    if (item.Functions.HasAny())
                        this.dbObjectsToGenerate.AddRange(item.Functions);

                    if (item.TVPs.HasAny())
                        this.dbObjectsToGenerate.AddRange(item.TVPs);
                }
            }
            else
            {
                this.dbObjectsToGenerate = null;
            }
        }

        #endregion

        #region Events

        // in order of execution
        private event EventHandler<ServerBuiltAsyncEventArgs> serverBuiltAsync;
        private event EventHandler<ServerBuiltEventArgs> serverBuilt;
        private event EventHandler<ServerGeneratingAsyncEventArgs> serverGeneratingAsync;
        private event EventHandler<ServerGeneratingEventArgs> serverGenerating;
        private event EventHandler<DatabaseGeneratingAsyncEventArgs> databaseGeneratingAsync;
        private event EventHandler<DatabaseGeneratingEventArgs> databaseGenerating;
        private event EventHandler<TablesGeneratingAsyncEventArgs> tablesGeneratingAsync;
        private event EventHandler<TablesGeneratingEventArgs> tablesGenerating;
        private event EventHandler<TableGeneratingAsyncEventArgs> tableGeneratingAsync;
        private event EventHandler<TableGeneratingEventArgs> tableGenerating;
        private event EventHandler<TablePOCOAsyncEventArgs> tablePOCOAsync;
        private event EventHandler<TablePOCOEventArgs> tablePOCO;
        private event EventHandler<TableGeneratedAsyncEventArgs> tableGeneratedAsync;
        private event EventHandler<TableGeneratedEventArgs> tableGenerated;
        private event EventHandler<TablesGeneratedAsyncEventArgs> tablesGeneratedAsync;
        private event EventHandler<TablesGeneratedEventArgs> tablesGenerated;
        private event EventHandler<ComplexTypeTablesGeneratingAsyncEventArgs> complexTypeTablesGeneratingAsync;
        private event EventHandler<ComplexTypeTablesGeneratingEventArgs> complexTypeTablesGenerating;
        private event EventHandler<ComplexTypeTableGeneratingAsyncEventArgs> complexTypeTableGeneratingAsync;
        private event EventHandler<ComplexTypeTableGeneratingEventArgs> complexTypeTableGenerating;
        private event EventHandler<ComplexTypeTablePOCOAsyncEventArgs> complexTypeTablePOCOAsync;
        private event EventHandler<ComplexTypeTablePOCOEventArgs> complexTypeTablePOCO;
        private event EventHandler<ComplexTypeTableGeneratedAsyncEventArgs> complexTypeTableGeneratedAsync;
        private event EventHandler<ComplexTypeTableGeneratedEventArgs> complexTypeTableGenerated;
        private event EventHandler<ComplexTypeTablesGeneratedAsyncEventArgs> complexTypeTablesGeneratedAsync;
        private event EventHandler<ComplexTypeTablesGeneratedEventArgs> complexTypeTablesGenerated;
        private event EventHandler<ViewsGeneratingAsyncEventArgs> viewsGeneratingAsync;
        private event EventHandler<ViewsGeneratingEventArgs> viewsGenerating;
        private event EventHandler<ViewGeneratingAsyncEventArgs> viewGeneratingAsync;
        private event EventHandler<ViewGeneratingEventArgs> viewGenerating;
        private event EventHandler<ViewPOCOAsyncEventArgs> viewPOCOAsync;
        private event EventHandler<ViewPOCOEventArgs> viewPOCO;
        private event EventHandler<ViewGeneratedAsyncEventArgs> viewGeneratedAsync;
        private event EventHandler<ViewGeneratedEventArgs> viewGenerated;
        private event EventHandler<ViewsGeneratedAsyncEventArgs> viewsGeneratedAsync;
        private event EventHandler<ViewsGeneratedEventArgs> viewsGenerated;
        private event EventHandler<ProceduresGeneratingAsyncEventArgs> proceduresGeneratingAsync;
        private event EventHandler<ProceduresGeneratingEventArgs> proceduresGenerating;
        private event EventHandler<ProcedureGeneratingAsyncEventArgs> procedureGeneratingAsync;
        private event EventHandler<ProcedureGeneratingEventArgs> procedureGenerating;
        private event EventHandler<ProcedurePOCOAsyncEventArgs> procedurePOCOAsync;
        private event EventHandler<ProcedurePOCOEventArgs> procedurePOCO;
        private event EventHandler<ProcedureGeneratedAsyncEventArgs> procedureGeneratedAsync;
        private event EventHandler<ProcedureGeneratedEventArgs> procedureGenerated;
        private event EventHandler<ProceduresGeneratedAsyncEventArgs> proceduresGeneratedAsync;
        private event EventHandler<ProceduresGeneratedEventArgs> proceduresGenerated;
        private event EventHandler<FunctionsGeneratingAsyncEventArgs> functionsGeneratingAsync;
        private event EventHandler<FunctionsGeneratingEventArgs> functionsGenerating;
        private event EventHandler<FunctionGeneratingAsyncEventArgs> functionGeneratingAsync;
        private event EventHandler<FunctionGeneratingEventArgs> functionGenerating;
        private event EventHandler<FunctionPOCOAsyncEventArgs> functionPOCOAsync;
        private event EventHandler<FunctionPOCOEventArgs> functionPOCO;
        private event EventHandler<FunctionGeneratedAsyncEventArgs> functionGeneratedAsync;
        private event EventHandler<FunctionGeneratedEventArgs> functionGenerated;
        private event EventHandler<FunctionsGeneratedAsyncEventArgs> functionsGeneratedAsync;
        private event EventHandler<FunctionsGeneratedEventArgs> functionsGenerated;
        private event EventHandler<TVPsGeneratingAsyncEventArgs> tvpsGeneratingAsync;
        private event EventHandler<TVPsGeneratingEventArgs> tvpsGenerating;
        private event EventHandler<TVPGeneratingAsyncEventArgs> tvpGeneratingAsync;
        private event EventHandler<TVPGeneratingEventArgs> tvpGenerating;
        private event EventHandler<TVPPOCOAsyncEventArgs> tvpPOCOAsync;
        private event EventHandler<TVPPOCOEventArgs> tvpPOCO;
        private event EventHandler<TVPGeneratedAsyncEventArgs> tvpGeneratedAsync;
        private event EventHandler<TVPGeneratedEventArgs> tvpGenerated;
        private event EventHandler<TVPsGeneratedAsyncEventArgs> tvpsGeneratedAsync;
        private event EventHandler<TVPsGeneratedEventArgs> tvpsGenerated;
        private event EventHandler<DatabaseGeneratedAsyncEventArgs> databaseGeneratedAsync;
        private event EventHandler<DatabaseGeneratedEventArgs> databaseGenerated;
        private event EventHandler<ServerGeneratedAsyncEventArgs> serverGeneratedAsync;
        private event EventHandler<ServerGeneratedEventArgs> serverGenerated;

        // in order of execution
        private event EventHandler<ServerBuiltAsyncEventArgs> serverBuiltAsyncInternal;
        private event EventHandler<ServerBuiltEventArgs> serverBuiltInternal;
        private event EventHandler<ServerGeneratingAsyncEventArgs> serverGeneratingAsyncInternal;
        private event EventHandler<ServerGeneratingEventArgs> serverGeneratingInternal;
        private event EventHandler<DatabaseGeneratingAsyncEventArgs> databaseGeneratingAsyncInternal;
        private event EventHandler<DatabaseGeneratingEventArgs> databaseGeneratingInternal;
        private event EventHandler<TablesGeneratingAsyncEventArgs> tablesGeneratingAsyncInternal;
        private event EventHandler<TablesGeneratingEventArgs> tablesGeneratingInternal;
        private event EventHandler<TableGeneratingAsyncEventArgs> tableGeneratingAsyncInternal;
        private event EventHandler<TableGeneratingEventArgs> tableGeneratingInternal;
        private event EventHandler<TablePOCOAsyncEventArgs> tablePOCOAsyncInternal;
        private event EventHandler<TablePOCOEventArgs> tablePOCOInternal;
        private event EventHandler<TableGeneratedAsyncEventArgs> tableGeneratedAsyncInternal;
        private event EventHandler<TableGeneratedEventArgs> tableGeneratedInternal;
        private event EventHandler<TablesGeneratedAsyncEventArgs> tablesGeneratedAsyncInternal;
        private event EventHandler<TablesGeneratedEventArgs> tablesGeneratedInternal;
        private event EventHandler<ComplexTypeTablesGeneratingAsyncEventArgs> complexTypeTablesGeneratingAsyncInternal;
        private event EventHandler<ComplexTypeTablesGeneratingEventArgs> complexTypeTablesGeneratingInternal;
        private event EventHandler<ComplexTypeTableGeneratingAsyncEventArgs> complexTypeTableGeneratingAsyncInternal;
        private event EventHandler<ComplexTypeTableGeneratingEventArgs> complexTypeTableGeneratingInternal;
        private event EventHandler<ComplexTypeTablePOCOAsyncEventArgs> complexTypeTablePOCOAsyncInternal;
        private event EventHandler<ComplexTypeTablePOCOEventArgs> complexTypeTablePOCOInternal;
        private event EventHandler<ComplexTypeTableGeneratedAsyncEventArgs> complexTypeTableGeneratedAsyncInternal;
        private event EventHandler<ComplexTypeTableGeneratedEventArgs> complexTypeTableGeneratedInternal;
        private event EventHandler<ComplexTypeTablesGeneratedAsyncEventArgs> complexTypeTablesGeneratedAsyncInternal;
        private event EventHandler<ComplexTypeTablesGeneratedEventArgs> complexTypeTablesGeneratedInternal;
        private event EventHandler<ViewsGeneratingAsyncEventArgs> viewsGeneratingAsyncInternal;
        private event EventHandler<ViewsGeneratingEventArgs> viewsGeneratingInternal;
        private event EventHandler<ViewGeneratingAsyncEventArgs> viewGeneratingAsyncInternal;
        private event EventHandler<ViewGeneratingEventArgs> viewGeneratingInternal;
        private event EventHandler<ViewPOCOAsyncEventArgs> viewPOCOAsyncInternal;
        private event EventHandler<ViewPOCOEventArgs> viewPOCOInternal;
        private event EventHandler<ViewGeneratedAsyncEventArgs> viewGeneratedAsyncInternal;
        private event EventHandler<ViewGeneratedEventArgs> viewGeneratedInternal;
        private event EventHandler<ViewsGeneratedAsyncEventArgs> viewsGeneratedAsyncInternal;
        private event EventHandler<ViewsGeneratedEventArgs> viewsGeneratedInternal;
        private event EventHandler<ProceduresGeneratingAsyncEventArgs> proceduresGeneratingAsyncInternal;
        private event EventHandler<ProceduresGeneratingEventArgs> proceduresGeneratingInternal;
        private event EventHandler<ProcedureGeneratingAsyncEventArgs> procedureGeneratingAsyncInternal;
        private event EventHandler<ProcedureGeneratingEventArgs> procedureGeneratingInternal;
        private event EventHandler<ProcedurePOCOAsyncEventArgs> procedurePOCOAsyncInternal;
        private event EventHandler<ProcedurePOCOEventArgs> procedurePOCOInternal;
        private event EventHandler<ProcedureGeneratedAsyncEventArgs> procedureGeneratedAsyncInternal;
        private event EventHandler<ProcedureGeneratedEventArgs> procedureGeneratedInternal;
        private event EventHandler<ProceduresGeneratedAsyncEventArgs> proceduresGeneratedAsyncInternal;
        private event EventHandler<ProceduresGeneratedEventArgs> proceduresGeneratedInternal;
        private event EventHandler<FunctionsGeneratingAsyncEventArgs> functionsGeneratingAsyncInternal;
        private event EventHandler<FunctionsGeneratingEventArgs> functionsGeneratingInternal;
        private event EventHandler<FunctionGeneratingAsyncEventArgs> functionGeneratingAsyncInternal;
        private event EventHandler<FunctionGeneratingEventArgs> functionGeneratingInternal;
        private event EventHandler<FunctionPOCOAsyncEventArgs> functionPOCOAsyncInternal;
        private event EventHandler<FunctionPOCOEventArgs> functionPOCOInternal;
        private event EventHandler<FunctionGeneratedAsyncEventArgs> functionGeneratedAsyncInternal;
        private event EventHandler<FunctionGeneratedEventArgs> functionGeneratedInternal;
        private event EventHandler<FunctionsGeneratedAsyncEventArgs> functionsGeneratedAsyncInternal;
        private event EventHandler<FunctionsGeneratedEventArgs> functionsGeneratedInternal;
        private event EventHandler<TVPsGeneratingAsyncEventArgs> tvpsGeneratingAsyncInternal;
        private event EventHandler<TVPsGeneratingEventArgs> tvpsGeneratingInternal;
        private event EventHandler<TVPGeneratingAsyncEventArgs> tvpGeneratingAsyncInternal;
        private event EventHandler<TVPGeneratingEventArgs> tvpGeneratingInternal;
        private event EventHandler<TVPPOCOAsyncEventArgs> tvpPOCOAsyncInternal;
        private event EventHandler<TVPPOCOEventArgs> tvpPOCOInternal;
        private event EventHandler<TVPGeneratedAsyncEventArgs> tvpGeneratedAsyncInternal;
        private event EventHandler<TVPGeneratedEventArgs> tvpGeneratedInternal;
        private event EventHandler<TVPsGeneratedAsyncEventArgs> tvpsGeneratedAsyncInternal;
        private event EventHandler<TVPsGeneratedEventArgs> tvpsGeneratedInternal;
        private event EventHandler<DatabaseGeneratedAsyncEventArgs> databaseGeneratedAsyncInternal;
        private event EventHandler<DatabaseGeneratedEventArgs> databaseGeneratedInternal;
        private event EventHandler<ServerGeneratedAsyncEventArgs> serverGeneratedAsyncInternal;
        private event EventHandler<ServerGeneratedEventArgs> serverGeneratedInternal;

        // in order of execution
        public event EventHandler<ServerBuiltAsyncEventArgs> ServerBuiltAsync { add { lock (this.lockObject) { this.serverBuiltAsync += value; } } remove { lock (this.lockObject) { this.serverBuiltAsync -= value; } } }
        public event EventHandler<ServerBuiltEventArgs> ServerBuilt { add { lock (this.lockObject) { this.serverBuilt += value; } } remove { lock (this.lockObject) { this.serverBuilt -= value; } } }
        public event EventHandler<ServerGeneratingAsyncEventArgs> ServerGeneratingAsync { add { lock (this.lockObject) { this.serverGeneratingAsync += value; } } remove { lock (this.lockObject) { this.serverGeneratingAsync -= value; } } }
        public event EventHandler<ServerGeneratingEventArgs> ServerGenerating { add { lock (this.lockObject) { this.serverGenerating += value; } } remove { lock (this.lockObject) { this.serverGenerating -= value; } } }
        public event EventHandler<DatabaseGeneratingAsyncEventArgs> DatabaseGeneratingAsync { add { lock (this.lockObject) { this.databaseGeneratingAsync += value; } } remove { lock (this.lockObject) { this.databaseGeneratingAsync -= value; } } }
        public event EventHandler<DatabaseGeneratingEventArgs> DatabaseGenerating { add { lock (this.lockObject) { this.databaseGenerating += value; } } remove { lock (this.lockObject) { this.databaseGenerating -= value; } } }
        public event EventHandler<TablesGeneratingAsyncEventArgs> TablesGeneratingAsync { add { lock (this.lockObject) { this.tablesGeneratingAsync += value; } } remove { lock (this.lockObject) { this.tablesGeneratingAsync -= value; } } }
        public event EventHandler<TablesGeneratingEventArgs> TablesGenerating { add { lock (this.lockObject) { this.tablesGenerating += value; } } remove { lock (this.lockObject) { this.tablesGenerating -= value; } } }
        public event EventHandler<TableGeneratingAsyncEventArgs> TableGeneratingAsync { add { lock (this.lockObject) { this.tableGeneratingAsync += value; } } remove { lock (this.lockObject) { this.tableGeneratingAsync -= value; } } }
        public event EventHandler<TableGeneratingEventArgs> TableGenerating { add { lock (this.lockObject) { this.tableGenerating += value; } } remove { lock (this.lockObject) { this.tableGenerating -= value; } } }
        public event EventHandler<TablePOCOAsyncEventArgs> TablePOCOAsync { add { lock (this.lockObject) { this.tablePOCOAsync += value; } } remove { lock (this.lockObject) { this.tablePOCOAsync -= value; } } }
        public event EventHandler<TablePOCOEventArgs> TablePOCO { add { lock (this.lockObject) { this.tablePOCO += value; } } remove { lock (this.lockObject) { this.tablePOCO -= value; } } }
        public event EventHandler<TableGeneratedAsyncEventArgs> TableGeneratedAsync { add { lock (this.lockObject) { this.tableGeneratedAsync += value; } } remove { lock (this.lockObject) { this.tableGeneratedAsync -= value; } } }
        public event EventHandler<TableGeneratedEventArgs> TableGenerated { add { lock (this.lockObject) { this.tableGenerated += value; } } remove { lock (this.lockObject) { this.tableGenerated -= value; } } }
        public event EventHandler<TablesGeneratedAsyncEventArgs> TablesGeneratedAsync { add { lock (this.lockObject) { this.tablesGeneratedAsync += value; } } remove { lock (this.lockObject) { this.tablesGeneratedAsync -= value; } } }
        public event EventHandler<TablesGeneratedEventArgs> TablesGenerated { add { lock (this.lockObject) { this.tablesGenerated += value; } } remove { lock (this.lockObject) { this.tablesGenerated -= value; } } }
        public event EventHandler<ComplexTypeTablesGeneratingAsyncEventArgs> ComplexTypeTablesGeneratingAsync { add { lock (this.lockObject) { this.complexTypeTablesGeneratingAsync += value; } } remove { lock (this.lockObject) { this.complexTypeTablesGeneratingAsync -= value; } } }
        public event EventHandler<ComplexTypeTablesGeneratingEventArgs> ComplexTypeTablesGenerating { add { lock (this.lockObject) { this.complexTypeTablesGenerating += value; } } remove { lock (this.lockObject) { this.complexTypeTablesGenerating -= value; } } }
        public event EventHandler<ComplexTypeTableGeneratingAsyncEventArgs> ComplexTypeTableGeneratingAsync { add { lock (this.lockObject) { this.complexTypeTableGeneratingAsync += value; } } remove { lock (this.lockObject) { this.complexTypeTableGeneratingAsync -= value; } } }
        public event EventHandler<ComplexTypeTableGeneratingEventArgs> ComplexTypeTableGenerating { add { lock (this.lockObject) { this.complexTypeTableGenerating += value; } } remove { lock (this.lockObject) { this.complexTypeTableGenerating -= value; } } }
        public event EventHandler<ComplexTypeTablePOCOAsyncEventArgs> ComplexTypeTablePOCOAsync { add { lock (this.lockObject) { this.complexTypeTablePOCOAsync += value; } } remove { lock (this.lockObject) { this.complexTypeTablePOCOAsync -= value; } } }
        public event EventHandler<ComplexTypeTablePOCOEventArgs> ComplexTypeTablePOCO { add { lock (this.lockObject) { this.complexTypeTablePOCO += value; } } remove { lock (this.lockObject) { this.complexTypeTablePOCO -= value; } } }
        public event EventHandler<ComplexTypeTableGeneratedAsyncEventArgs> ComplexTypeTableGeneratedAsync { add { lock (this.lockObject) { this.complexTypeTableGeneratedAsync += value; } } remove { lock (this.lockObject) { this.complexTypeTableGeneratedAsync -= value; } } }
        public event EventHandler<ComplexTypeTableGeneratedEventArgs> ComplexTypeTableGenerated { add { lock (this.lockObject) { this.complexTypeTableGenerated += value; } } remove { lock (this.lockObject) { this.complexTypeTableGenerated -= value; } } }
        public event EventHandler<ComplexTypeTablesGeneratedAsyncEventArgs> ComplexTypeTablesGeneratedAsync { add { lock (this.lockObject) { this.complexTypeTablesGeneratedAsync += value; } } remove { lock (this.lockObject) { this.complexTypeTablesGeneratedAsync -= value; } } }
        public event EventHandler<ComplexTypeTablesGeneratedEventArgs> ComplexTypeTablesGenerated { add { lock (this.lockObject) { this.complexTypeTablesGenerated += value; } } remove { lock (this.lockObject) { this.complexTypeTablesGenerated -= value; } } }
        public event EventHandler<ViewsGeneratingAsyncEventArgs> ViewsGeneratingAsync { add { lock (this.lockObject) { this.viewsGeneratingAsync += value; } } remove { lock (this.lockObject) { this.viewsGeneratingAsync -= value; } } }
        public event EventHandler<ViewsGeneratingEventArgs> ViewsGenerating { add { lock (this.lockObject) { this.viewsGenerating += value; } } remove { lock (this.lockObject) { this.viewsGenerating -= value; } } }
        public event EventHandler<ViewGeneratingAsyncEventArgs> ViewGeneratingAsync { add { lock (this.lockObject) { this.viewGeneratingAsync += value; } } remove { lock (this.lockObject) { this.viewGeneratingAsync -= value; } } }
        public event EventHandler<ViewGeneratingEventArgs> ViewGenerating { add { lock (this.lockObject) { this.viewGenerating += value; } } remove { lock (this.lockObject) { this.viewGenerating -= value; } } }
        public event EventHandler<ViewPOCOAsyncEventArgs> ViewPOCOAsync { add { lock (this.lockObject) { this.viewPOCOAsync += value; } } remove { lock (this.lockObject) { this.viewPOCOAsync -= value; } } }
        public event EventHandler<ViewPOCOEventArgs> ViewPOCO { add { lock (this.lockObject) { this.viewPOCO += value; } } remove { lock (this.lockObject) { this.viewPOCO -= value; } } }
        public event EventHandler<ViewGeneratedAsyncEventArgs> ViewGeneratedAsync { add { lock (this.lockObject) { this.viewGeneratedAsync += value; } } remove { lock (this.lockObject) { this.viewGeneratedAsync -= value; } } }
        public event EventHandler<ViewGeneratedEventArgs> ViewGenerated { add { lock (this.lockObject) { this.viewGenerated += value; } } remove { lock (this.lockObject) { this.viewGenerated -= value; } } }
        public event EventHandler<ViewsGeneratedAsyncEventArgs> ViewsGeneratedAsync { add { lock (this.lockObject) { this.viewsGeneratedAsync += value; } } remove { lock (this.lockObject) { this.viewsGeneratedAsync -= value; } } }
        public event EventHandler<ViewsGeneratedEventArgs> ViewsGenerated { add { lock (this.lockObject) { this.viewsGenerated += value; } } remove { lock (this.lockObject) { this.viewsGenerated -= value; } } }
        public event EventHandler<ProceduresGeneratingAsyncEventArgs> ProceduresGeneratingAsync { add { lock (this.lockObject) { this.proceduresGeneratingAsync += value; } } remove { lock (this.lockObject) { this.proceduresGeneratingAsync -= value; } } }
        public event EventHandler<ProceduresGeneratingEventArgs> ProceduresGenerating { add { lock (this.lockObject) { this.proceduresGenerating += value; } } remove { lock (this.lockObject) { this.proceduresGenerating -= value; } } }
        public event EventHandler<ProcedureGeneratingAsyncEventArgs> ProcedureGeneratingAsync { add { lock (this.lockObject) { this.procedureGeneratingAsync += value; } } remove { lock (this.lockObject) { this.procedureGeneratingAsync -= value; } } }
        public event EventHandler<ProcedureGeneratingEventArgs> ProcedureGenerating { add { lock (this.lockObject) { this.procedureGenerating += value; } } remove { lock (this.lockObject) { this.procedureGenerating -= value; } } }
        public event EventHandler<ProcedurePOCOAsyncEventArgs> ProcedurePOCOAsync { add { lock (this.lockObject) { this.procedurePOCOAsync += value; } } remove { lock (this.lockObject) { this.procedurePOCOAsync -= value; } } }
        public event EventHandler<ProcedurePOCOEventArgs> ProcedurePOCO { add { lock (this.lockObject) { this.procedurePOCO += value; } } remove { lock (this.lockObject) { this.procedurePOCO -= value; } } }
        public event EventHandler<ProcedureGeneratedAsyncEventArgs> ProcedureGeneratedAsync { add { lock (this.lockObject) { this.procedureGeneratedAsync += value; } } remove { lock (this.lockObject) { this.procedureGeneratedAsync -= value; } } }
        public event EventHandler<ProcedureGeneratedEventArgs> ProcedureGenerated { add { lock (this.lockObject) { this.procedureGenerated += value; } } remove { lock (this.lockObject) { this.procedureGenerated -= value; } } }
        public event EventHandler<ProceduresGeneratedAsyncEventArgs> ProceduresGeneratedAsync { add { lock (this.lockObject) { this.proceduresGeneratedAsync += value; } } remove { lock (this.lockObject) { this.proceduresGeneratedAsync -= value; } } }
        public event EventHandler<ProceduresGeneratedEventArgs> ProceduresGenerated { add { lock (this.lockObject) { this.proceduresGenerated += value; } } remove { lock (this.lockObject) { this.proceduresGenerated -= value; } } }
        public event EventHandler<FunctionsGeneratingAsyncEventArgs> FunctionsGeneratingAsync { add { lock (this.lockObject) { this.functionsGeneratingAsync += value; } } remove { lock (this.lockObject) { this.functionsGeneratingAsync -= value; } } }
        public event EventHandler<FunctionsGeneratingEventArgs> FunctionsGenerating { add { lock (this.lockObject) { this.functionsGenerating += value; } } remove { lock (this.lockObject) { this.functionsGenerating -= value; } } }
        public event EventHandler<FunctionGeneratingAsyncEventArgs> FunctionGeneratingAsync { add { lock (this.lockObject) { this.functionGeneratingAsync += value; } } remove { lock (this.lockObject) { this.functionGeneratingAsync -= value; } } }
        public event EventHandler<FunctionGeneratingEventArgs> FunctionGenerating { add { lock (this.lockObject) { this.functionGenerating += value; } } remove { lock (this.lockObject) { this.functionGenerating -= value; } } }
        public event EventHandler<FunctionPOCOAsyncEventArgs> FunctionPOCOAsync { add { lock (this.lockObject) { this.functionPOCOAsync += value; } } remove { lock (this.lockObject) { this.functionPOCOAsync -= value; } } }
        public event EventHandler<FunctionPOCOEventArgs> FunctionPOCO { add { lock (this.lockObject) { this.functionPOCO += value; } } remove { lock (this.lockObject) { this.functionPOCO -= value; } } }
        public event EventHandler<FunctionGeneratedAsyncEventArgs> FunctionGeneratedAsync { add { lock (this.lockObject) { this.functionGeneratedAsync += value; } } remove { lock (this.lockObject) { this.functionGeneratedAsync -= value; } } }
        public event EventHandler<FunctionGeneratedEventArgs> FunctionGenerated { add { lock (this.lockObject) { this.functionGenerated += value; } } remove { lock (this.lockObject) { this.functionGenerated -= value; } } }
        public event EventHandler<FunctionsGeneratedAsyncEventArgs> FunctionsGeneratedAsync { add { lock (this.lockObject) { this.functionsGeneratedAsync += value; } } remove { lock (this.lockObject) { this.functionsGeneratedAsync -= value; } } }
        public event EventHandler<FunctionsGeneratedEventArgs> FunctionsGenerated { add { lock (this.lockObject) { this.functionsGenerated += value; } } remove { lock (this.lockObject) { this.functionsGenerated -= value; } } }
        public event EventHandler<TVPsGeneratingAsyncEventArgs> TVPsGeneratingAsync { add { lock (this.lockObject) { this.tvpsGeneratingAsync += value; } } remove { lock (this.lockObject) { this.tvpsGeneratingAsync -= value; } } }
        public event EventHandler<TVPsGeneratingEventArgs> TVPsGenerating { add { lock (this.lockObject) { this.tvpsGenerating += value; } } remove { lock (this.lockObject) { this.tvpsGenerating -= value; } } }
        public event EventHandler<TVPGeneratingAsyncEventArgs> TVPGeneratingAsync { add { lock (this.lockObject) { this.tvpGeneratingAsync += value; } } remove { lock (this.lockObject) { this.tvpGeneratingAsync -= value; } } }
        public event EventHandler<TVPGeneratingEventArgs> TVPGenerating { add { lock (this.lockObject) { this.tvpGenerating += value; } } remove { lock (this.lockObject) { this.tvpGenerating -= value; } } }
        public event EventHandler<TVPPOCOAsyncEventArgs> TVPPOCOAsync { add { lock (this.lockObject) { this.tvpPOCOAsync += value; } } remove { lock (this.lockObject) { this.tvpPOCOAsync -= value; } } }
        public event EventHandler<TVPPOCOEventArgs> TVPPOCO { add { lock (this.lockObject) { this.tvpPOCO += value; } } remove { lock (this.lockObject) { this.tvpPOCO -= value; } } }
        public event EventHandler<TVPGeneratedAsyncEventArgs> TVPGeneratedAsync { add { lock (this.lockObject) { this.tvpGeneratedAsync += value; } } remove { lock (this.lockObject) { this.tvpGeneratedAsync -= value; } } }
        public event EventHandler<TVPGeneratedEventArgs> TVPGenerated { add { lock (this.lockObject) { this.tvpGenerated += value; } } remove { lock (this.lockObject) { this.tvpGenerated -= value; } } }
        public event EventHandler<TVPsGeneratedAsyncEventArgs> TVPsGeneratedAsync { add { lock (this.lockObject) { this.tvpsGeneratedAsync += value; } } remove { lock (this.lockObject) { this.tvpsGeneratedAsync -= value; } } }
        public event EventHandler<TVPsGeneratedEventArgs> TVPsGenerated { add { lock (this.lockObject) { this.tvpsGenerated += value; } } remove { lock (this.lockObject) { this.tvpsGenerated -= value; } } }
        public event EventHandler<DatabaseGeneratedAsyncEventArgs> DatabaseGeneratedAsync { add { lock (this.lockObject) { this.databaseGeneratedAsync += value; } } remove { lock (this.lockObject) { this.databaseGeneratedAsync -= value; } } }
        public event EventHandler<DatabaseGeneratedEventArgs> DatabaseGenerated { add { lock (this.lockObject) { this.databaseGenerated += value; } } remove { lock (this.lockObject) { this.databaseGenerated -= value; } } }
        public event EventHandler<ServerGeneratedAsyncEventArgs> ServerGeneratedAsync { add { lock (this.lockObject) { this.serverGeneratedAsync += value; } } remove { lock (this.lockObject) { this.serverGeneratedAsync -= value; } } }
        public event EventHandler<ServerGeneratedEventArgs> ServerGenerated { add { lock (this.lockObject) { this.serverGenerated += value; } } remove { lock (this.lockObject) { this.serverGenerated -= value; } } }

        private void SetPOCOIteratorEvents(IDbIterator iterator, POCOGenerator.Objects.Server proxyServer)
        {
            if (this.serverGeneratingAsyncInternal != null)
            {
                iterator.ServerGeneratingAsync += (sender, e) =>
                {
                    this.serverGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ServerGeneratingAsyncEventArgs(
                            proxyServer
                        )
                    );
                };
            }

            if (this.serverGeneratingInternal != null)
            {
                iterator.ServerGenerating += (sender, e) =>
                {
                    var args = new ServerGeneratingEventArgs(
                        proxyServer
                    );
                    this.serverGeneratingInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.databaseGeneratingAsyncInternal != null)
            {
                iterator.DatabaseGeneratingAsync += (sender, e) =>
                {
                    this.databaseGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new DatabaseGeneratingAsyncEventArgs(
                            proxyServer.Databases.First(d => d.InternalEquals(e.Database))
                        )
                    );
                };
            }

            if (this.databaseGeneratingInternal != null)
            {
                iterator.DatabaseGenerating += (sender, e) =>
                {
                    var args = new DatabaseGeneratingEventArgs(
                        proxyServer.Databases.First(d => d.InternalEquals(e.Database))
                    );
                    this.databaseGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.tablesGeneratingAsyncInternal != null)
            {
                iterator.TablesGeneratingAsync += (sender, e) =>
                {
                    this.tablesGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new TablesGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.tablesGeneratingInternal != null)
            {
                iterator.TablesGenerating += (sender, e) =>
                {
                    var args = new TablesGeneratingEventArgs();
                    this.tablesGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.tableGeneratingAsyncInternal != null)
            {
                iterator.TableGeneratingAsync += (sender, e) =>
                {
                    this.tableGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new TableGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.tableGeneratingInternal != null)
            {
                iterator.TableGenerating += (sender, e) =>
                {
                    var args = new TableGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                        e.Namespace
                    );
                    this.tableGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.tablePOCOAsyncInternal != null)
            {
                iterator.TablePOCOAsync += (sender, e) =>
                {
                    this.tablePOCOAsyncInternal.RaiseAsync(
                        this,
                        new TablePOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.tablePOCOInternal != null)
            {
                iterator.TablePOCO += (sender, e) =>
                {
                    var args = new TablePOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                        e.POCO
                    );
                    this.tablePOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.tableGeneratedAsyncInternal != null)
            {
                iterator.TableGeneratedAsync += (sender, e) =>
                {
                    this.tableGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new TableGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.tableGeneratedInternal != null)
            {
                iterator.TableGenerated += (sender, e) =>
                {
                    var args = new TableGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Tables).First(t => t.InternalEquals(e.Table)),
                        e.Namespace
                    );
                    this.tableGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.tablesGeneratedAsyncInternal != null)
            {
                iterator.TablesGeneratedAsync += (sender, e) =>
                {
                    this.tablesGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new TablesGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.tablesGeneratedInternal != null)
            {
                iterator.TablesGenerated += (sender, e) =>
                {
                    var args = new TablesGeneratedEventArgs();
                    this.tablesGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.complexTypeTablesGeneratingAsyncInternal != null)
            {
                iterator.ComplexTypeTablesGeneratingAsync += (sender, e) =>
                {
                    this.complexTypeTablesGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ComplexTypeTablesGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.complexTypeTablesGeneratingInternal != null)
            {
                iterator.ComplexTypeTablesGenerating += (sender, e) =>
                {
                    var args = new ComplexTypeTablesGeneratingEventArgs();
                    this.complexTypeTablesGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.complexTypeTableGeneratingAsyncInternal != null)
            {
                iterator.ComplexTypeTableGeneratingAsync += (sender, e) =>
                {
                    this.complexTypeTableGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ComplexTypeTableGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.complexTypeTableGeneratingInternal != null)
            {
                iterator.ComplexTypeTableGenerating += (sender, e) =>
                {
                    var args = new ComplexTypeTableGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                        e.Namespace
                    );
                    this.complexTypeTableGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.complexTypeTablePOCOAsyncInternal != null)
            {
                iterator.ComplexTypeTablePOCOAsync += (sender, e) =>
                {
                    this.complexTypeTablePOCOAsyncInternal.RaiseAsync(
                        this,
                        new ComplexTypeTablePOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.complexTypeTablePOCOInternal != null)
            {
                iterator.ComplexTypeTablePOCO += (sender, e) =>
                {
                    var args = new ComplexTypeTablePOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                        e.POCO
                    );
                    this.complexTypeTablePOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.complexTypeTableGeneratedAsyncInternal != null)
            {
                iterator.ComplexTypeTableGeneratedAsync += (sender, e) =>
                {
                    this.complexTypeTableGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ComplexTypeTableGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.complexTypeTableGeneratedInternal != null)
            {
                iterator.ComplexTypeTableGenerated += (sender, e) =>
                {
                    var args = new ComplexTypeTableGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.ComplexTypeTables).First(t => t.InternalEquals(e.ComplexTypeTable)),
                        e.Namespace
                    );
                    this.complexTypeTableGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.complexTypeTablesGeneratedAsyncInternal != null)
            {
                iterator.ComplexTypeTablesGeneratedAsync += (sender, e) =>
                {
                    this.complexTypeTablesGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ComplexTypeTablesGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.complexTypeTablesGeneratedInternal != null)
            {
                iterator.ComplexTypeTablesGenerated += (sender, e) =>
                {
                    var args = new ComplexTypeTablesGeneratedEventArgs();
                    this.complexTypeTablesGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.viewsGeneratingAsyncInternal != null)
            {
                iterator.ViewsGeneratingAsync += (sender, e) =>
                {
                    this.viewsGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ViewsGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.viewsGeneratingInternal != null)
            {
                iterator.ViewsGenerating += (sender, e) =>
                {
                    var args = new ViewsGeneratingEventArgs();
                    this.viewsGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.viewGeneratingAsyncInternal != null)
            {
                iterator.ViewGeneratingAsync += (sender, e) =>
                {
                    this.viewGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ViewGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.viewGeneratingInternal != null)
            {
                iterator.ViewGenerating += (sender, e) =>
                {
                    var args = new ViewGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                        e.Namespace
                    );
                    this.viewGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.viewPOCOAsyncInternal != null)
            {
                iterator.ViewPOCOAsync += (sender, e) =>
                {
                    this.viewPOCOAsyncInternal.RaiseAsync(
                        this,
                        new ViewPOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.viewPOCOInternal != null)
            {
                iterator.ViewPOCO += (sender, e) =>
                {
                    var args = new ViewPOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                        e.POCO
                    );
                    this.viewPOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.viewGeneratedAsyncInternal != null)
            {
                iterator.ViewGeneratedAsync += (sender, e) =>
                {
                    this.viewGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ViewGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.viewGeneratedInternal != null)
            {
                iterator.ViewGenerated += (sender, e) =>
                {
                    var args = new ViewGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Views).First(v => v.InternalEquals(e.View)),
                        e.Namespace
                    );
                    this.viewGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.viewsGeneratedAsyncInternal != null)
            {
                iterator.ViewsGeneratedAsync += (sender, e) =>
                {
                    this.viewsGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ViewsGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.viewsGeneratedInternal != null)
            {
                iterator.ViewsGenerated += (sender, e) =>
                {
                    var args = new ViewsGeneratedEventArgs();
                    this.viewsGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.proceduresGeneratingAsyncInternal != null)
            {
                iterator.ProceduresGeneratingAsync += (sender, e) =>
                {
                    this.proceduresGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ProceduresGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.proceduresGeneratingInternal != null)
            {
                iterator.ProceduresGenerating += (sender, e) =>
                {
                    var args = new ProceduresGeneratingEventArgs();
                    this.proceduresGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.procedureGeneratingAsyncInternal != null)
            {
                iterator.ProcedureGeneratingAsync += (sender, e) =>
                {
                    this.procedureGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new ProcedureGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.procedureGeneratingInternal != null)
            {
                iterator.ProcedureGenerating += (sender, e) =>
                {
                    var args = new ProcedureGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                        e.Namespace
                    );
                    this.procedureGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.procedurePOCOAsyncInternal != null)
            {
                iterator.ProcedurePOCOAsync += (sender, e) =>
                {
                    this.procedurePOCOAsyncInternal.RaiseAsync(
                        this,
                        new ProcedurePOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.procedurePOCOInternal != null)
            {
                iterator.ProcedurePOCO += (sender, e) =>
                {
                    var args = new ProcedurePOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                        e.POCO
                    );
                    this.procedurePOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.procedureGeneratedAsyncInternal != null)
            {
                iterator.ProcedureGeneratedAsync += (sender, e) =>
                {
                    this.procedureGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ProcedureGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.procedureGeneratedInternal != null)
            {
                iterator.ProcedureGenerated += (sender, e) =>
                {
                    var args = new ProcedureGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Procedures).First(p => p.InternalEquals(e.Procedure)),
                        e.Namespace
                    );
                    this.procedureGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.proceduresGeneratedAsyncInternal != null)
            {
                iterator.ProceduresGeneratedAsync += (sender, e) =>
                {
                    this.proceduresGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ProceduresGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.proceduresGeneratedInternal != null)
            {
                iterator.ProceduresGenerated += (sender, e) =>
                {
                    var args = new ProceduresGeneratedEventArgs();
                    this.proceduresGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.functionsGeneratingAsyncInternal != null)
            {
                iterator.FunctionsGeneratingAsync += (sender, e) =>
                {
                    this.functionsGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new FunctionsGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.functionsGeneratingInternal != null)
            {
                iterator.FunctionsGenerating += (sender, e) =>
                {
                    var args = new FunctionsGeneratingEventArgs();
                    this.functionsGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.functionGeneratingAsyncInternal != null)
            {
                iterator.FunctionGeneratingAsync += (sender, e) =>
                {
                    this.functionGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new FunctionGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.functionGeneratingInternal != null)
            {
                iterator.FunctionGenerating += (sender, e) =>
                {
                    var args = new FunctionGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                        e.Namespace
                    );
                    this.functionGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.functionPOCOAsyncInternal != null)
            {
                iterator.FunctionPOCOAsync += (sender, e) =>
                {
                    this.functionPOCOAsyncInternal.RaiseAsync(
                        this,
                        new FunctionPOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.functionPOCOInternal != null)
            {
                iterator.FunctionPOCO += (sender, e) =>
                {
                    var args = new FunctionPOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                        e.POCO
                    );
                    this.functionPOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.functionGeneratedAsyncInternal != null)
            {
                iterator.FunctionGeneratedAsync += (sender, e) =>
                {
                    this.functionGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new FunctionGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.functionGeneratedInternal != null)
            {
                iterator.FunctionGenerated += (sender, e) =>
                {
                    var args = new FunctionGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.Functions).First(f => f.InternalEquals(e.Function)),
                        e.Namespace
                    );
                    this.functionGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.functionsGeneratedAsyncInternal != null)
            {
                iterator.FunctionsGeneratedAsync += (sender, e) =>
                {
                    this.functionsGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new FunctionsGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.functionsGeneratedInternal != null)
            {
                iterator.FunctionsGenerated += (sender, e) =>
                {
                    var args = new FunctionsGeneratedEventArgs();
                    this.functionsGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.tvpsGeneratingAsyncInternal != null)
            {
                iterator.TVPsGeneratingAsync += (sender, e) =>
                {
                    this.tvpsGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new TVPsGeneratingAsyncEventArgs()
                    );
                };
            }

            if (this.tvpsGeneratingInternal != null)
            {
                iterator.TVPsGenerating += (sender, e) =>
                {
                    var args = new TVPsGeneratingEventArgs();
                    this.tvpsGeneratingInternal.Raise(this, args);
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.tvpGeneratingAsyncInternal != null)
            {
                iterator.TVPGeneratingAsync += (sender, e) =>
                {
                    this.tvpGeneratingAsyncInternal.RaiseAsync(
                        this,
                        new TVPGeneratingAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.tvpGeneratingInternal != null)
            {
                iterator.TVPGenerating += (sender, e) =>
                {
                    var args = new TVPGeneratingEventArgs(
                        proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                        e.Namespace
                    );
                    this.tvpGeneratingInternal.Raise(this, args);
                    e.Namespace = args.Namespace;
                    e.Skip = args.Skip;
                    e.Stop = args.Stop;
                };
            }

            if (this.tvpPOCOAsyncInternal != null)
            {
                iterator.TVPPOCOAsync += (sender, e) =>
                {
                    this.tvpPOCOAsyncInternal.RaiseAsync(
                        this,
                        new TVPPOCOAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                            e.POCO
                        )
                    );
                };
            }

            if (this.tvpPOCOInternal != null)
            {
                iterator.TVPPOCO += (sender, e) =>
                {
                    var args = new TVPPOCOEventArgs(
                        proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                        e.POCO
                    );
                    this.tvpPOCOInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.tvpGeneratedAsyncInternal != null)
            {
                iterator.TVPGeneratedAsync += (sender, e) =>
                {
                    this.tvpGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new TVPGeneratedAsyncEventArgs(
                            proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                            e.Namespace
                        )
                    );
                };
            }

            if (this.tvpGeneratedInternal != null)
            {
                iterator.TVPGenerated += (sender, e) =>
                {
                    var args = new TVPGeneratedEventArgs(
                        proxyServer.Databases.SelectMany(d => d.TVPs).First(t => t.InternalEquals(e.TVP)),
                        e.Namespace
                    );
                    this.tvpGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.tvpsGeneratedAsyncInternal != null)
            {
                iterator.TVPsGeneratedAsync += (sender, e) =>
                {
                    this.tvpsGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new TVPsGeneratedAsyncEventArgs()
                    );
                };
            }

            if (this.tvpsGeneratedInternal != null)
            {
                iterator.TVPsGenerated += (sender, e) =>
                {
                    var args = new TVPsGeneratedEventArgs();
                    this.tvpsGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.databaseGeneratedAsyncInternal != null)
            {
                iterator.DatabaseGeneratedAsync += (sender, e) =>
                {
                    this.databaseGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new DatabaseGeneratedAsyncEventArgs(
                            proxyServer.Databases.First(d => d.InternalEquals(e.Database))
                        )
                    );
                };
            }

            if (this.databaseGeneratedInternal != null)
            {
                iterator.DatabaseGenerated += (sender, e) =>
                {
                    var args = new DatabaseGeneratedEventArgs(
                        proxyServer.Databases.First(d => d.InternalEquals(e.Database))
                    );
                    this.databaseGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }

            if (this.serverGeneratedAsyncInternal != null)
            {
                iterator.ServerGeneratedAsync += (sender, e) =>
                {
                    this.serverGeneratedAsyncInternal.RaiseAsync(
                        this,
                        new ServerGeneratedAsyncEventArgs(
                            proxyServer
                        )
                    );
                };
            }

            if (this.serverGeneratedInternal != null)
            {
                iterator.ServerGenerated += (sender, e) =>
                {
                    var args = new ServerGeneratedEventArgs(
                        proxyServer
                    );
                    this.serverGeneratedInternal.Raise(this, args);
                    e.Stop = args.Stop;
                };
            }
        }

        #endregion

        #region Disclaimer

        public string Disclaimer
        {
            get
            {
                return POCOGenerator.Disclaimer.Message;
            }
        }

        #endregion
    }
}
