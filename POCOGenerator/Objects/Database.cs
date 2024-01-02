using System;
using System.Collections.Generic;
using System.Linq;

namespace POCOGenerator.Objects
{
    /// <summary>Represents a RDBMS database.</summary>
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

        /// <summary>Gets the server that this database belongs to.</summary>
        /// <value>The server that this database belongs to.</value>
        public Server Server { get; private set; }

        private CachedEnumerable<POCOGenerator.DbObjects.ITable, Table> tables;
        /// <summary>Gets the collection of tables that belong to this database.</summary>
        /// <value>Collection of tables.</value>
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

        private CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTable, ComplexTypeTable> complexTypeTables;
        internal IEnumerable<ComplexTypeTable> ComplexTypeTables
        {
            get
            {
                if (this.database.Tables.IsNullOrEmpty())
                    yield break;

                if (this.complexTypeTables == null)
                {
                    var tables = this.database.Tables.Intersect(this.databaseAccessibleObjects.Tables);
                    var source = tables.Where(t => t.ComplexTypeTables.HasAny()).SelectMany(t => t.ComplexTypeTables).Distinct().OrderBy(t => t.Name).ToList();
                    this.complexTypeTables = new CachedEnumerable<POCOGenerator.DbObjects.IComplexTypeTable, ComplexTypeTable>(source, t => new ComplexTypeTable(t, this));
                }

                foreach (var complexTypeTable in this.complexTypeTables)
                {
                    yield return complexTypeTable;
                }
            }
        }

        private CachedEnumerable<POCOGenerator.DbObjects.IView, View> views;
        /// <summary>Gets the collection of views that belong to this database.</summary>
        /// <value>Collection of views.</value>
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
        /// <summary>Gets the collection of stored procedures that belong to this database.</summary>
        /// <value>Collection of stored procedures.</value>
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
        /// <summary>Gets the collection of table-valued functions that belong to this database.</summary>
        /// <value>Collection of table-valued functions.</value>
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
        /// <summary>Gets the collection of user-defined table types that belong to this database.</summary>
        /// <value>Collection of user-defined table types.</value>
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
        /// <summary>Gets the collection of error messages that occurred during the generating process of this database.</summary>
        /// <value>Collection of error messages.</value>
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

        /// <summary>Gets the name of the database.</summary>
        /// <value>The name of the database.</value>
        public string Name { get { return this.database.Name; } }

        /// <summary>Gets the description of the database.</summary>
        /// <value>The description of the database.</value>
        public string Description { get { return this.database.Description; } }

        /// <summary>Returns a string that represents this database.</summary>
        /// <returns>A string that represents this database.</returns>
        public override string ToString()
        {
            return this.database.ToString();
        }
    }
}
