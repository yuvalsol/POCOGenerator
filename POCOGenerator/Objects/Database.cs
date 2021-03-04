using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    public sealed class Database
    {
        private readonly POCOGenerator.DbObjects.IDatabase database;
        private readonly DatabaseAccessibleObjects databaseAccessibleObjects;

        internal Database(POCOGenerator.DbObjects.IDatabase database, Server server, DatabaseAccessibleObjects databaseAccessibleObjects)
        {
            this.database = database;
            this.Server = server;
            this.databaseAccessibleObjects = databaseAccessibleObjects;
        }

        internal bool InternalEquals(POCOGenerator.DbObjects.IDatabase database)
        {
            return this.database == database;
        }

        public Server Server { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.ITable, Table> tables;
        public IEnumerable<Table> Tables
        {
            get
            {
                if (this.database.Tables.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.Tables.IsNullOrEmpty())
                    yield break;

                if (this.tables == null)
                {
                    var source = this.database.Tables.Intersect(this.databaseAccessibleObjects.Tables).ToList();
                    this.tables = new CachedEnumerable<POCOGenerator.DbObjects.ITable, Table>(source, t => new Table(t, this));
                }

                foreach (var table in this.tables)
                {
                    yield return table;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITable, Table> accessibleTables;
        internal IEnumerable<Table> AccessibleTables
        {
            get
            {
                if (this.database.Tables.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.AccessibleTables.IsNullOrEmpty())
                    yield break;

                if (this.accessibleTables == null)
                {
                    var source = this.database.Tables.Intersect(this.databaseAccessibleObjects.AccessibleTables).ToList();
                    this.accessibleTables = new CachedEnumerable<POCOGenerator.DbObjects.ITable, Table>(source, t => new Table(t, this));
                }

                foreach (var table in this.accessibleTables)
                {
                    yield return table;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IView, View> views;
        public IEnumerable<View> Views
        {
            get
            {
                if (this.database.Views.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.Views.IsNullOrEmpty())
                    yield break;

                if (this.views == null)
                {
                    var source = this.database.Views.Intersect(this.databaseAccessibleObjects.Views).ToList();
                    this.views = new CachedEnumerable<POCOGenerator.DbObjects.IView, View>(source, v => new View(v, this));
                }

                foreach (var view in this.views)
                {
                    yield return view;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IProcedure, Procedure> procedures;
        public IEnumerable<Procedure> Procedures
        {
            get
            {
                if (this.database.Procedures.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.Procedures.IsNullOrEmpty())
                    yield break;

                if (this.procedures == null)
                {
                    var source = this.database.Procedures.Intersect(this.databaseAccessibleObjects.Procedures).ToList();
                    this.procedures = new CachedEnumerable<POCOGenerator.DbObjects.IProcedure, Procedure>(source, p => new Procedure(p, this));
                }

                foreach (var procedure in this.procedures)
                {
                    yield return procedure;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IFunction, Function> functions;
        public IEnumerable<Function> Functions
        {
            get
            {
                if (this.database.Functions.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.Functions.IsNullOrEmpty())
                    yield break;

                if (this.functions == null)
                {
                    var source = this.database.Functions.Intersect(this.databaseAccessibleObjects.Functions).ToList();
                    this.functions = new CachedEnumerable<POCOGenerator.DbObjects.IFunction, Function>(source, f => new Function(f, this));
                }

                foreach (var function in this.functions)
                {
                    yield return function;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.ITVP, TVP> tvps;
        public IEnumerable<TVP> TVPs
        {
            get
            {
                if (this.database.TVPs.IsNullOrEmpty())
                    yield break;

                if (this.databaseAccessibleObjects == null || this.databaseAccessibleObjects.TVPs.IsNullOrEmpty())
                    yield break;

                if (this.tvps == null)
                {
                    var source = this.database.TVPs.Intersect(this.databaseAccessibleObjects.TVPs).ToList();
                    this.tvps = new CachedEnumerable<POCOGenerator.DbObjects.ITVP, TVP>(source, t => new TVP(t, this));
                }

                foreach (var tvp in this.tvps)
                {
                    yield return tvp;
                }
            }
        }

        private CachedEnumerable<Exception, string> errors;
        public IEnumerable<string> Errors
        {
            get
            {
                if (this.database.Errors.IsNullOrEmpty())
                    yield break;

                if (this.errors == null)
                {
                    this.errors = new CachedEnumerable<Exception, string>(this.database.Errors, ex =>
                    {
                        string errorMessage = ex.Message;
#if DEBUG
                        if (string.IsNullOrEmpty(ex.StackTrace) == false)
                            errorMessage += Environment.NewLine + ex.StackTrace;

                        while (ex.InnerException != null)
                        {
                            errorMessage += Environment.NewLine + ex.InnerException.Message;
                            if (string.IsNullOrEmpty(ex.InnerException.StackTrace) == false)
                                errorMessage += Environment.NewLine + ex.InnerException.StackTrace;
                            ex = ex.InnerException;
                        }
#endif

                        return errorMessage;
                    });
                }

                foreach (var error in this.errors)
                {
                    yield return error;
                }
            }
        }

        public string Name { get { return this.database.Name; } }

        public string Description { get { return this.database.Description; } }

        public override string ToString()
        {
            return this.database.ToString();
        }
    }
}
