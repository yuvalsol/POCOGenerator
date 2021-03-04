using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using POCOGenerator.DbObjects;
using POCOGenerator.POCOWriters;
using POCOGenerator.Utils;

namespace POCOGenerator.POCOIterators
{
    public abstract class DbIterator : IDbIterator
    {
        #region Constructor

        protected IWriter writer;
        protected IDbSupport support;
        protected IDbIteratorSettings settings;

        public DbIterator(IWriter writer, IDbSupport support, IDbIteratorSettings settings)
        {
            this.writer = writer;
            this.support = support;
            this.settings = settings;
        }

        #endregion

        #region Events

        // in order of execution
        public event EventHandler<ServerGeneratingAsyncEventArgs> ServerGeneratingAsync;
        public event EventHandler<ServerGeneratingEventArgs> ServerGenerating;
        public event EventHandler<DatabaseGeneratingAsyncEventArgs> DatabaseGeneratingAsync;
        public event EventHandler<DatabaseGeneratingEventArgs> DatabaseGenerating;
        public event EventHandler<TablesGeneratingAsyncEventArgs> TablesGeneratingAsync;
        public event EventHandler<TablesGeneratingEventArgs> TablesGenerating;
        public event EventHandler<TableGeneratingAsyncEventArgs> TableGeneratingAsync;
        public event EventHandler<TableGeneratingEventArgs> TableGenerating;
        public event EventHandler<TablePOCOAsyncEventArgs> TablePOCOAsync;
        public event EventHandler<TablePOCOEventArgs> TablePOCO;
        public event EventHandler<TableGeneratedAsyncEventArgs> TableGeneratedAsync;
        public event EventHandler<TableGeneratedEventArgs> TableGenerated;
        public event EventHandler<TablesGeneratedAsyncEventArgs> TablesGeneratedAsync;
        public event EventHandler<TablesGeneratedEventArgs> TablesGenerated;
        public event EventHandler<ViewsGeneratingAsyncEventArgs> ViewsGeneratingAsync;
        public event EventHandler<ViewsGeneratingEventArgs> ViewsGenerating;
        public event EventHandler<ViewGeneratingAsyncEventArgs> ViewGeneratingAsync;
        public event EventHandler<ViewGeneratingEventArgs> ViewGenerating;
        public event EventHandler<ViewPOCOAsyncEventArgs> ViewPOCOAsync;
        public event EventHandler<ViewPOCOEventArgs> ViewPOCO;
        public event EventHandler<ViewGeneratedAsyncEventArgs> ViewGeneratedAsync;
        public event EventHandler<ViewGeneratedEventArgs> ViewGenerated;
        public event EventHandler<ViewsGeneratedAsyncEventArgs> ViewsGeneratedAsync;
        public event EventHandler<ViewsGeneratedEventArgs> ViewsGenerated;
        public event EventHandler<ProceduresGeneratingAsyncEventArgs> ProceduresGeneratingAsync;
        public event EventHandler<ProceduresGeneratingEventArgs> ProceduresGenerating;
        public event EventHandler<ProcedureGeneratingAsyncEventArgs> ProcedureGeneratingAsync;
        public event EventHandler<ProcedureGeneratingEventArgs> ProcedureGenerating;
        public event EventHandler<ProcedurePOCOAsyncEventArgs> ProcedurePOCOAsync;
        public event EventHandler<ProcedurePOCOEventArgs> ProcedurePOCO;
        public event EventHandler<ProcedureGeneratedAsyncEventArgs> ProcedureGeneratedAsync;
        public event EventHandler<ProcedureGeneratedEventArgs> ProcedureGenerated;
        public event EventHandler<ProceduresGeneratedAsyncEventArgs> ProceduresGeneratedAsync;
        public event EventHandler<ProceduresGeneratedEventArgs> ProceduresGenerated;
        public event EventHandler<FunctionsGeneratingAsyncEventArgs> FunctionsGeneratingAsync;
        public event EventHandler<FunctionsGeneratingEventArgs> FunctionsGenerating;
        public event EventHandler<FunctionGeneratingAsyncEventArgs> FunctionGeneratingAsync;
        public event EventHandler<FunctionGeneratingEventArgs> FunctionGenerating;
        public event EventHandler<FunctionPOCOAsyncEventArgs> FunctionPOCOAsync;
        public event EventHandler<FunctionPOCOEventArgs> FunctionPOCO;
        public event EventHandler<FunctionGeneratedAsyncEventArgs> FunctionGeneratedAsync;
        public event EventHandler<FunctionGeneratedEventArgs> FunctionGenerated;
        public event EventHandler<FunctionsGeneratedAsyncEventArgs> FunctionsGeneratedAsync;
        public event EventHandler<FunctionsGeneratedEventArgs> FunctionsGenerated;
        public event EventHandler<TVPsGeneratingAsyncEventArgs> TVPsGeneratingAsync;
        public event EventHandler<TVPsGeneratingEventArgs> TVPsGenerating;
        public event EventHandler<TVPGeneratingAsyncEventArgs> TVPGeneratingAsync;
        public event EventHandler<TVPGeneratingEventArgs> TVPGenerating;
        public event EventHandler<TVPPOCOAsyncEventArgs> TVPPOCOAsync;
        public event EventHandler<TVPPOCOEventArgs> TVPPOCO;
        public event EventHandler<TVPGeneratedAsyncEventArgs> TVPGeneratedAsync;
        public event EventHandler<TVPGeneratedEventArgs> TVPGenerated;
        public event EventHandler<TVPsGeneratedAsyncEventArgs> TVPsGeneratedAsync;
        public event EventHandler<TVPsGeneratedEventArgs> TVPsGenerated;
        public event EventHandler<DatabaseGeneratedAsyncEventArgs> DatabaseGeneratedAsync;
        public event EventHandler<DatabaseGeneratedEventArgs> DatabaseGenerated;
        public event EventHandler<ServerGeneratedAsyncEventArgs> ServerGeneratedAsync;
        public event EventHandler<ServerGeneratedEventArgs> ServerGenerated;

        #endregion

        #region Iterate

        public void Iterate(IEnumerable<IDbObjectTraverse> dbObjects)
        {
            Clear();

            if (dbObjects == null || dbObjects.Any() == false)
                return;

            bool isExistDbObject = (dbObjects.Any(o => o.Error == null));

            string namespaceOffset = ItereatorStart(isExistDbObject, dbObjects);

            bool isFirstDbObject = true;

            var servers = dbObjects.GroupBy(x => x.Database.Server).OrderBy(x => x.Key.ToString());
            foreach (var server in servers)
            {
                this.ServerGeneratingAsync.RaiseAsync(this, () => new ServerGeneratingAsyncEventArgs(server.Key));

                var argsServerGenerating = this.ServerGenerating.Raise(this, () => new ServerGeneratingEventArgs(server.Key));
                if (argsServerGenerating != null && argsServerGenerating.Stop)
                {
                    IteratorEnd(isExistDbObject);
                    return;
                }

                var databases = server.GroupBy(x => x.Database).OrderBy(x => x.Key.ToString());
                foreach (var database in databases)
                {
                    this.DatabaseGeneratingAsync.RaiseAsync(this, () => new DatabaseGeneratingAsyncEventArgs(database.Key));

                    var argsDatabaseGenerating = this.DatabaseGenerating.Raise(this, () => new DatabaseGeneratingEventArgs(database.Key));
                    if (argsDatabaseGenerating != null && argsDatabaseGenerating.Stop)
                    {
                        IteratorEnd(isExistDbObject);
                        return;
                    }

                    if (argsDatabaseGenerating == null || argsDatabaseGenerating.Skip == false)
                    {
                        var tables = database.Where(x => x.DbObjectType == DbObjectType.Table).OrderBy(x => x.ToString());
                        if (tables.Any())
                        {
                            this.TablesGeneratingAsync.RaiseAsync(this, () => new TablesGeneratingAsyncEventArgs());

                            var argsTablesGenerating = this.TablesGenerating.Raise(this, () => new TablesGeneratingEventArgs());
                            if (argsTablesGenerating != null && argsTablesGenerating.Stop)
                            {
                                IteratorEnd(isExistDbObject);
                                return;
                            }

                            if (argsTablesGenerating == null || argsTablesGenerating.Skip == false)
                            {
                                foreach (IDbObjectTraverse table in tables)
                                {
                                    // don't write join table
                                    if (((ITable)table).IsJoinTable && settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable == false)
                                        continue;

                                    bool stop = WriteDbObject(dbObjects, table, namespaceOffset, RaiseTableGeneratingEvent, RaiseTableGeneratedEvent, ref isFirstDbObject);
                                    if (stop)
                                    {
                                        IteratorEnd(isExistDbObject);
                                        return;
                                    }
                                }

                                this.TablesGeneratedAsync.RaiseAsync(this, () => new TablesGeneratedAsyncEventArgs());

                                var argsTablesGenerated = this.TablesGenerated.Raise(this, () => new TablesGeneratedEventArgs());
                                if (argsTablesGenerated != null && argsTablesGenerated.Stop)
                                {
                                    IteratorEnd(isExistDbObject);
                                    return;
                                }
                            }
                        }

                        var views = database.Where(x => x.DbObjectType == DbObjectType.View).OrderBy(x => x.ToString());
                        if (views.Any())
                        {
                            this.ViewsGeneratingAsync.RaiseAsync(this, () => new ViewsGeneratingAsyncEventArgs());

                            var argsViewsGenerating = this.ViewsGenerating.Raise(this, () => new ViewsGeneratingEventArgs());
                            if (argsViewsGenerating != null && argsViewsGenerating.Stop)
                            {
                                IteratorEnd(isExistDbObject);
                                return;
                            }

                            if (argsViewsGenerating == null || argsViewsGenerating.Skip == false)
                            {
                                foreach (IDbObjectTraverse view in views)
                                {
                                    bool stop = WriteDbObject(dbObjects, view, namespaceOffset, RaiseViewGeneratingEvent, RaiseViewGeneratedEvent, ref isFirstDbObject);
                                    if (stop)
                                    {
                                        IteratorEnd(isExistDbObject);
                                        return;
                                    }
                                }

                                this.ViewsGeneratedAsync.RaiseAsync(this, () => new ViewsGeneratedAsyncEventArgs());

                                var argsViewsGenerated = this.ViewsGenerated.Raise(this, () => new ViewsGeneratedEventArgs());
                                if (argsViewsGenerated != null && argsViewsGenerated.Stop)
                                {
                                    IteratorEnd(isExistDbObject);
                                    return;
                                }
                            }
                        }

                        var procedures = database.Where(x => x.DbObjectType == DbObjectType.Procedure).OrderBy(x => x.ToString());
                        if (procedures.Any())
                        {
                            this.ProceduresGeneratingAsync.RaiseAsync(this, () => new ProceduresGeneratingAsyncEventArgs());

                            var argsProceduresGenerating = this.ProceduresGenerating.Raise(this, () => new ProceduresGeneratingEventArgs());
                            if (argsProceduresGenerating != null && argsProceduresGenerating.Stop)
                            {
                                IteratorEnd(isExistDbObject);
                                return;
                            }

                            if (argsProceduresGenerating == null || argsProceduresGenerating.Skip == false)
                            {
                                foreach (IDbObjectTraverse procedure in procedures)
                                {
                                    bool stop = WriteDbObject(dbObjects, procedure, namespaceOffset, RaiseProcedureGeneratingEvent, RaiseProcedureGeneratedEvent, ref isFirstDbObject);
                                    if (stop)
                                    {
                                        IteratorEnd(isExistDbObject);
                                        return;
                                    }
                                }

                                this.ProceduresGeneratedAsync.RaiseAsync(this, () => new ProceduresGeneratedAsyncEventArgs());

                                var argsProceduresGenerated = this.ProceduresGenerated.Raise(this, () => new ProceduresGeneratedEventArgs());
                                if (argsProceduresGenerated != null && argsProceduresGenerated.Stop)
                                {
                                    IteratorEnd(isExistDbObject);
                                    return;
                                }
                            }
                        }

                        var functions = database.Where(x => x.DbObjectType == DbObjectType.Function).OrderBy(x => x.ToString());
                        if (functions.Any())
                        {
                            this.FunctionsGeneratingAsync.RaiseAsync(this, () => new FunctionsGeneratingAsyncEventArgs());

                            var argsFunctionsGenerating = this.FunctionsGenerating.Raise(this, () => new FunctionsGeneratingEventArgs());
                            if (argsFunctionsGenerating != null && argsFunctionsGenerating.Stop)
                            {
                                IteratorEnd(isExistDbObject);
                                return;
                            }

                            if (argsFunctionsGenerating == null || argsFunctionsGenerating.Skip == false)
                            {
                                foreach (IDbObjectTraverse function in functions)
                                {
                                    bool stop = WriteDbObject(dbObjects, function, namespaceOffset, RaiseFunctionGeneratingEvent, RaiseFunctionGeneratedEvent, ref isFirstDbObject);
                                    if (stop)
                                    {
                                        IteratorEnd(isExistDbObject);
                                        return;
                                    }
                                }

                                this.FunctionsGeneratedAsync.RaiseAsync(this, () => new FunctionsGeneratedAsyncEventArgs());

                                var argsFunctionsGenerated = this.FunctionsGenerated.Raise(this, () => new FunctionsGeneratedEventArgs());
                                if (argsFunctionsGenerated != null && argsFunctionsGenerated.Stop)
                                {
                                    IteratorEnd(isExistDbObject);
                                    return;
                                }
                            }
                        }

                        var tvps = database.Where(x => x.DbObjectType == DbObjectType.TVP).OrderBy(x => x.ToString());
                        if (tvps.Any())
                        {
                            this.TVPsGeneratingAsync.RaiseAsync(this, () => new TVPsGeneratingAsyncEventArgs());

                            var argsTVPsGenerating = this.TVPsGenerating.Raise(this, () => new TVPsGeneratingEventArgs());
                            if (argsTVPsGenerating != null && argsTVPsGenerating.Stop)
                            {
                                IteratorEnd(isExistDbObject);
                                return;
                            }

                            if (argsTVPsGenerating == null || argsTVPsGenerating.Skip == false)
                            {
                                foreach (IDbObjectTraverse tvp in tvps)
                                {
                                    bool stop = WriteDbObject(dbObjects, tvp, namespaceOffset, RaiseTVPGeneratingEvent, RaiseTVPGeneratedEvent, ref isFirstDbObject);
                                    if (stop)
                                    {
                                        IteratorEnd(isExistDbObject);
                                        return;
                                    }
                                }

                                this.TVPsGeneratedAsync.RaiseAsync(this, () => new TVPsGeneratedAsyncEventArgs());

                                var argsTVPsGenerated = this.TVPsGenerated.Raise(this, () => new TVPsGeneratedEventArgs());
                                if (argsTVPsGenerated != null && argsTVPsGenerated.Stop)
                                {
                                    IteratorEnd(isExistDbObject);
                                    return;
                                }
                            }
                        }

                        this.DatabaseGeneratedAsync.RaiseAsync(this, () => new DatabaseGeneratedAsyncEventArgs(database.Key));

                        var argsDatabaseGenerated = this.DatabaseGenerated.Raise(this, () => new DatabaseGeneratedEventArgs(database.Key));
                        if (argsDatabaseGenerated != null && argsDatabaseGenerated.Stop)
                        {
                            IteratorEnd(isExistDbObject);
                            return;
                        }
                    }
                }

                this.ServerGeneratedAsync.RaiseAsync(this, () => new ServerGeneratedAsyncEventArgs(server.Key));

                var argsServerGenerated = this.ServerGenerated.Raise(this, () => new ServerGeneratedEventArgs(server.Key));
                if (argsServerGenerated != null && argsServerGenerated.Stop)
                {
                    IteratorEnd(isExistDbObject);
                    return;
                }
            }

            IteratorEnd(isExistDbObject);
        }

        protected virtual string ItereatorStart(bool isExistDbObject, IEnumerable<IDbObjectTraverse> dbObjects)
        {
            string namespaceOffset = string.Empty;

            if (isExistDbObject)
            {
                if (settings.POCOIteratorSettings.WrapAroundEachClass == false)
                {
                    // Using
                    if (settings.POCOIteratorSettings.UsingInsideNamespace == false)
                        WriteUsing(dbObjects, namespaceOffset);

                    // Namespace Start
                    namespaceOffset = WriteNamespaceStart(settings.POCOIteratorSettings.Namespace);

                    // Using
                    if (settings.POCOIteratorSettings.UsingInsideNamespace)
                        WriteUsing(dbObjects, namespaceOffset);
                }
            }

            return namespaceOffset;
        }

        protected virtual void IteratorEnd(bool isExistDbObject)
        {
            if (isExistDbObject)
            {
                if (settings.POCOIteratorSettings.WrapAroundEachClass == false)
                {
                    // Namespace End
                    WriteNamespaceEnd(settings.POCOIteratorSettings.Namespace);
                }
            }
        }

        protected virtual bool WriteDbObject(
            IEnumerable<IDbObjectTraverse> dbObjects,
            IDbObjectTraverse dbObject,
            string namespaceOffset,
            RaiseGeneratingEventHandler RaiseGeneratingEvent,
            RaiseGeneratedEventHandler RaiseGeneratedEvent,
            ref bool isFirstDbObject)
        {
            // Class Name
            string className = GetClassName(
                dbObject.Database.ToString(),
                (dbObject is ISchema ? ((ISchema)dbObject).Schema : null),
                dbObject.Name,
                dbObject.DbObjectType
            );
            dbObject.ClassName = className;

            string @namespace = settings.POCOIteratorSettings.Namespace;
            bool skip = false;
            bool stop = RaiseGeneratingEvent(dbObject, ref @namespace, ref skip);
            if (stop)
                return true;
            if (skip)
                return false;

            if (isFirstDbObject == false)
                writer.WriteLine();

            isFirstDbObject = false;

            if (dbObject.Error != null)
            {
                // Write Error
                WriteError(dbObject, namespaceOffset);
            }
            else
            {
                // Write Class
                WriteDbObjectClass(dbObjects, dbObject, @namespace, namespaceOffset, className);
            }

            return RaiseGeneratedEvent(dbObject, @namespace);
        }

        protected virtual void WriteDbObjectClass(IEnumerable<IDbObjectTraverse> dbObjects, IDbObjectTraverse dbObject, string @namespace, string namespaceOffset, string className)
        {
            // Enums
            List<IEnumColumn> enumColumns = GetEnumColumns(dbObject);

            // Navigation Properties
            List<INavigationProperty> navigationProperties = GetNavigationProperties(dbObject);

            if (settings.POCOIteratorSettings.WrapAroundEachClass)
            {
                // Using
                if (settings.POCOIteratorSettings.UsingInsideNamespace == false)
                    WriteUsing(dbObjects, namespaceOffset);

                // Namespace Start
                namespaceOffset = WriteNamespaceStart(@namespace);

                // Using
                if (settings.POCOIteratorSettings.UsingInsideNamespace)
                    WriteUsing(dbObjects, namespaceOffset);
            }

            // Class Attributes
            WriteClassAttributes(dbObject, namespaceOffset);

            // Class Start
            WriteClassStart(className, dbObject, namespaceOffset);

            // Constructor
            WriteConstructor(className, enumColumns, navigationProperties, dbObject, namespaceOffset);

            // Columns
            if (dbObject.Columns.HasAny())
            {
                var columns = dbObject.Columns.OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
                var lastColumn = columns.Last();
                foreach (IColumn column in columns)
                    WriteColumn(column, column == lastColumn, dbObject, namespaceOffset);
            }

            // Enums
            if (settings.POCOIteratorSettings.EnumSQLTypeToString == false && (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort || settings.POCOIteratorSettings.EnumSQLTypeToEnumInt))
            {
                if (enumColumns.HasAny())
                {
                    var lastEnumColumn = enumColumns.Last();
                    foreach (IEnumColumn enumColumn in enumColumns)
                        WriteEnum(enumColumn, enumColumn == lastEnumColumn, dbObject, namespaceOffset);
                }
            }

            // Navigation Properties
            WriteNavigationProperties(navigationProperties, dbObject, namespaceOffset);

            // Class End
            WriteClassEnd(dbObject, namespaceOffset);

            if (settings.POCOIteratorSettings.WrapAroundEachClass)
            {
                // Namespace End
                WriteNamespaceEnd(@namespace);
            }
        }

        protected delegate bool RaiseGeneratingEventHandler(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip);

        protected virtual bool RaiseTableGeneratingEvent(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip)
        {
            string argsNamespace = @namespace;

            this.TableGeneratingAsync.RaiseAsync(this, () => new TableGeneratingAsyncEventArgs((ITable)dbObject, argsNamespace));

            var argsTable = this.TableGenerating.Raise(this, () => new TableGeneratingEventArgs((ITable)dbObject, argsNamespace));

            if (argsTable != null)
            {
                if (argsTable.Stop)
                    return true;
                skip = argsTable.Skip;
                if (skip)
                    return false;
                @namespace = argsTable.Namespace;
            }

            if (this.TablePOCO != null || this.TablePOCOAsync != null)
                writer.StartSnapshot();

            return false;
        }

        protected virtual bool RaiseViewGeneratingEvent(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip)
        {
            string argsNamespace = @namespace;

            this.ViewGeneratingAsync.RaiseAsync(this, () => new ViewGeneratingAsyncEventArgs((IView)dbObject, argsNamespace));

            var argsView = this.ViewGenerating.Raise(this, () => new ViewGeneratingEventArgs((IView)dbObject, argsNamespace));

            if (argsView != null)
            {
                if (argsView.Stop)
                    return true;
                skip = argsView.Skip;
                if (skip)
                    return false;
                @namespace = argsView.Namespace;
            }

            if (this.ViewPOCO != null || this.ViewPOCOAsync != null)
                writer.StartSnapshot();

            return false;
        }

        protected virtual bool RaiseProcedureGeneratingEvent(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip)
        {
            string argsNamespace = @namespace;

            this.ProcedureGeneratingAsync.RaiseAsync(this, () => new ProcedureGeneratingAsyncEventArgs((IProcedure)dbObject, argsNamespace));

            var argsProcedure = this.ProcedureGenerating.Raise(this, () => new ProcedureGeneratingEventArgs((IProcedure)dbObject, argsNamespace));

            if (argsProcedure != null)
            {
                if (argsProcedure.Stop)
                    return true;
                skip = argsProcedure.Skip;
                if (skip)
                    return false;
                @namespace = argsProcedure.Namespace;
            }

            if (this.ProcedurePOCO != null || this.ProcedurePOCOAsync != null)
                writer.StartSnapshot();

            return false;
        }

        protected virtual bool RaiseFunctionGeneratingEvent(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip)
        {
            string argsNamespace = @namespace;

            this.FunctionGeneratingAsync.RaiseAsync(this, () => new FunctionGeneratingAsyncEventArgs((IFunction)dbObject, argsNamespace));

            var argsFunction = this.FunctionGenerating.Raise(this, () => new FunctionGeneratingEventArgs((IFunction)dbObject, argsNamespace));

            if (argsFunction != null)
            {
                if (argsFunction.Stop)
                    return true;
                skip = argsFunction.Skip;
                if (skip)
                    return false;
                @namespace = argsFunction.Namespace;
            }

            if (this.FunctionPOCO != null || this.FunctionPOCOAsync != null)
                writer.StartSnapshot();

            return false;
        }

        protected virtual bool RaiseTVPGeneratingEvent(IDbObjectTraverse dbObject, ref string @namespace, ref bool skip)
        {
            string argsNamespace = @namespace;

            this.TVPGeneratingAsync.RaiseAsync(this, () => new TVPGeneratingAsyncEventArgs((ITVP)dbObject, argsNamespace));

            var argsTVP = this.TVPGenerating.Raise(this, () => new TVPGeneratingEventArgs((ITVP)dbObject, argsNamespace));

            if (argsTVP != null)
            {
                if (argsTVP.Stop)
                    return true;
                skip = argsTVP.Skip;
                if (skip)
                    return false;
                @namespace = argsTVP.Namespace;
            }

            if (this.TVPPOCO != null || this.TVPPOCOAsync != null)
                writer.StartSnapshot();

            return false;
        }

        protected delegate bool RaiseGeneratedEventHandler(IDbObjectTraverse dbObject, string @namespace);

        protected virtual bool RaiseTableGeneratedEvent(IDbObjectTraverse dbObject, string @namespace)
        {
            if (this.TablePOCO != null || this.TablePOCOAsync != null)
            {
                string poco = writer.EndSnapshot().ToString().Trim();

                this.TablePOCOAsync.RaiseAsync(this, () => new TablePOCOAsyncEventArgs((ITable)dbObject, poco));

                var argsPOCO = this.TablePOCO.Raise(this, () => new TablePOCOEventArgs((ITable)dbObject, poco));
                if (argsPOCO != null && argsPOCO.Stop)
                    return true;
            }

            this.TableGeneratedAsync.RaiseAsync(this, () => new TableGeneratedAsyncEventArgs((ITable)dbObject, @namespace));

            var argsTable = this.TableGenerated.Raise(this, () => new TableGeneratedEventArgs((ITable)dbObject, @namespace));
            if (argsTable != null && argsTable.Stop)
                return true;

            return false;
        }

        protected virtual bool RaiseViewGeneratedEvent(IDbObjectTraverse dbObject, string @namespace)
        {
            if (this.ViewPOCO != null || this.ViewPOCOAsync != null)
            {
                string poco = writer.EndSnapshot().ToString().Trim();

                this.ViewPOCOAsync.RaiseAsync(this, () => new ViewPOCOAsyncEventArgs((IView)dbObject, poco));

                var argsPOCO = this.ViewPOCO.Raise(this, () => new ViewPOCOEventArgs((IView)dbObject, poco));
                if (argsPOCO != null && argsPOCO.Stop)
                    return true;
            }

            this.ViewGeneratedAsync.RaiseAsync(this, () => new ViewGeneratedAsyncEventArgs((IView)dbObject, @namespace));

            var argsView = this.ViewGenerated.Raise(this, () => new ViewGeneratedEventArgs((IView)dbObject, @namespace));
            if (argsView != null && argsView.Stop)
                return true;

            return false;
        }

        protected virtual bool RaiseProcedureGeneratedEvent(IDbObjectTraverse dbObject, string @namespace)
        {
            if (this.ProcedurePOCO != null || this.ProcedurePOCOAsync != null)
            {
                string poco = writer.EndSnapshot().ToString().Trim();

                this.ProcedurePOCOAsync.RaiseAsync(this, () => new ProcedurePOCOAsyncEventArgs((IProcedure)dbObject, poco));

                var argsPOCO = this.ProcedurePOCO.Raise(this, () => new ProcedurePOCOEventArgs((IProcedure)dbObject, poco));
                if (argsPOCO != null && argsPOCO.Stop)
                    return true;
            }

            this.ProcedureGeneratedAsync.RaiseAsync(this, () => new ProcedureGeneratedAsyncEventArgs((IProcedure)dbObject, @namespace));

            var argsProcedure = this.ProcedureGenerated.Raise(this, () => new ProcedureGeneratedEventArgs((IProcedure)dbObject, @namespace));
            if (argsProcedure != null && argsProcedure.Stop)
                return true;

            return false;
        }

        protected virtual bool RaiseFunctionGeneratedEvent(IDbObjectTraverse dbObject, string @namespace)
        {
            if (this.FunctionPOCO != null || this.FunctionPOCOAsync != null)
            {
                string poco = writer.EndSnapshot().ToString().Trim();

                this.FunctionPOCOAsync.RaiseAsync(this, () => new FunctionPOCOAsyncEventArgs((IFunction)dbObject, poco));

                var argsPOCO = this.FunctionPOCO.Raise(this, () => new FunctionPOCOEventArgs((IFunction)dbObject, poco));
                if (argsPOCO != null && argsPOCO.Stop)
                    return true;
            }

            this.FunctionGeneratedAsync.RaiseAsync(this, () => new FunctionGeneratedAsyncEventArgs((IFunction)dbObject, @namespace));

            var argsFunction = this.FunctionGenerated.Raise(this, () => new FunctionGeneratedEventArgs((IFunction)dbObject, @namespace));
            if (argsFunction != null && argsFunction.Stop)
                return true;

            return false;
        }

        protected virtual bool RaiseTVPGeneratedEvent(IDbObjectTraverse dbObject, string @namespace)
        {
            if (this.TVPPOCO != null || this.TVPPOCOAsync != null)
            {
                string poco = writer.EndSnapshot().ToString().Trim();

                this.TVPPOCOAsync.RaiseAsync(this, () => new TVPPOCOAsyncEventArgs((ITVP)dbObject, poco));

                var argsPOCO = this.TVPPOCO.Raise(this, () => new TVPPOCOEventArgs((ITVP)dbObject, poco));
                if (argsPOCO != null && argsPOCO.Stop)
                    return true;
            }

            this.TVPGeneratedAsync.RaiseAsync(this, () => new TVPGeneratedAsyncEventArgs((ITVP)dbObject, @namespace));

            var argsTVP = this.TVPGenerated.Raise(this, () => new TVPGeneratedEventArgs((ITVP)dbObject, @namespace));
            if (argsTVP != null && argsTVP.Stop)
                return true;

            return false;
        }

        #endregion

        #region Clear

        protected virtual void Clear()
        {
            writer.Clear();
        }

        #endregion

        #region Schema

        protected virtual string DefaultSchema { get { return support.DefaultSchema; } }

        #endregion

        #region Using

        protected virtual void WriteUsing(IEnumerable<IDbObjectTraverse> dbObjects, string namespaceOffset)
        {
            if (settings.POCOIteratorSettings.Using)
            {
                WriteUsingClause(dbObjects, namespaceOffset);
                WriteEFUsingClause(dbObjects, namespaceOffset);
                writer.WriteLine();
            }
        }

        protected virtual void WriteUsingClause(IEnumerable<IDbObjectTraverse> dbObjects, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.WriteKeyword("using");
            writer.WriteLine(" System;");

            if (settings.NavigationPropertiesIteratorSettings.Enable)
            {
                writer.Write(namespaceOffset);
                writer.WriteKeyword("using");
                writer.WriteLine(" System.Collections.Generic;");
            }

            if (HasSpecialSQLTypes(dbObjects))
                WriteSpecialSQLTypesUsingClause(namespaceOffset);
        }

        protected virtual bool HasSpecialSQLTypes(IEnumerable<IDbObjectTraverse> dbObjects)
        {
            if (dbObjects.HasAny())
            {
                foreach (var dbObject in dbObjects)
                {
                    if (dbObject.Columns.HasAny())
                    {
                        foreach (IColumn column in dbObject.Columns)
                        {
                            string dataTypeName = (column.DataTypeName ?? string.Empty).ToLower();
                            if (IsSQLTypeRDBMSSpecificType(dataTypeName))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        protected abstract void WriteSpecialSQLTypesUsingClause(string namespaceOffset);

        #region EF

        protected virtual void WriteEFUsingClause(IEnumerable<IDbObjectTraverse> dbObjects, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable)
            {
                if (dbObjects.HasAny())
                {
                    if (dbObjects.Any(o => o.DbObjectType == DbObjectType.Table))
                    {
                        if (settings.EFAnnotationsIteratorSettings.Description)
                        {
                            writer.Write(namespaceOffset);
                            writer.WriteKeyword("using");
                            writer.WriteLine(" System.ComponentModel;");
                        }

                        writer.Write(namespaceOffset);
                        writer.WriteKeyword("using");
                        writer.WriteLine(" System.ComponentModel.DataAnnotations;");

                        writer.Write(namespaceOffset);
                        writer.WriteKeyword("using");
                        writer.WriteLine(" System.ComponentModel.DataAnnotations.Schema;");
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Namespace Start

        protected virtual string WriteNamespaceStart(string @namespace)
        {
            string namespaceOffset = string.Empty;

            if (string.IsNullOrEmpty(@namespace) == false)
            {
                WriteNamespaceStartClause(@namespace);
                namespaceOffset = settings.POCOIteratorSettings.Tab;
            }

            return namespaceOffset;
        }

        protected virtual void WriteNamespaceStartClause(string @namespace)
        {
            writer.WriteKeyword("namespace");
            writer.Write(" ");
            writer.WriteLine(@namespace);
            writer.WriteLine("{");
        }

        #endregion

        #region Error

        protected virtual void WriteError(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.WriteLineError("/*");

            writer.Write(namespaceOffset);
            writer.WriteLineError(string.Format("{0}.{1}", dbObject.Database.ToString(), dbObject.ToString()));

            Exception currentError = dbObject.Error;
            while (currentError != null)
            {
                writer.Write(namespaceOffset);
                writer.WriteLineError(currentError.Message);
                currentError = currentError.InnerException;
            }

            writer.Write(namespaceOffset);
            writer.WriteLineError("*/");
        }

        #endregion

        #region Class Attributes

        protected virtual void WriteClassAttributes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteEFClassAttributes(dbObject, namespaceOffset);
        }

        #region EF

        protected virtual void WriteEFClassAttributes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable)
            {
                if (dbObject.DbObjectType == DbObjectType.Table)
                {
                    WriteEFTable(dbObject, namespaceOffset);
                }

                if (settings.EFAnnotationsIteratorSettings.Description && dbObject is IDescription)
                {
                    IDescription descObject = (IDescription)dbObject;
                    if (string.IsNullOrEmpty(descObject.Description) == false)
                        WriteEFDescription(descObject.Description, false, namespaceOffset);
                }
            }
        }

        protected virtual void WriteEFTable(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write("[");
            writer.WriteUserType("Table");
            writer.Write("(");
            writer.WriteString("\"");

            if (dbObject is ISchema)
            {
                string schema = ((ISchema)dbObject).Schema;
                if (string.IsNullOrEmpty(schema) == false)
                {
                    if (schema != DefaultSchema)
                    {
                        writer.WriteString(schema);
                        writer.WriteString(".");
                    }
                }
            }

            writer.WriteString(dbObject.Name);
            writer.WriteString("\"");
            writer.WriteLine(")]");
        }

        #endregion

        #endregion

        #region Class Name

        protected virtual string GetClassName(string database, string schema, string name, DbObjectType dbObjectType)
        {
            string className = null;

            // prefix
            if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.Prefix) == false)
                className += settings.ClassNameIteratorSettings.Prefix;

            if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.FixedClassName))
            {
                if (settings.ClassNameIteratorSettings.IncludeDB)
                {
                    // database
                    if (settings.ClassNameIteratorSettings.CamelCase || string.IsNullOrEmpty(settings.ClassNameIteratorSettings.WordsSeparator) == false)
                        className += NameHelper.TransformName(database, settings.ClassNameIteratorSettings.WordsSeparator, settings.ClassNameIteratorSettings.CamelCase, settings.ClassNameIteratorSettings.UpperCase, settings.ClassNameIteratorSettings.LowerCase);
                    else if (settings.ClassNameIteratorSettings.UpperCase)
                        className += database.ToUpper();
                    else if (settings.ClassNameIteratorSettings.LowerCase)
                        className += database.ToLower();
                    else
                        className += database;

                    // db separator
                    if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.DBSeparator) == false)
                        className += settings.ClassNameIteratorSettings.DBSeparator;
                }

                if (settings.ClassNameIteratorSettings.IncludeSchema)
                {
                    if (string.IsNullOrEmpty(schema) == false)
                    {
                        if (settings.ClassNameIteratorSettings.IgnoreDboSchema == false || schema != DefaultSchema)
                        {
                            // schema
                            if (settings.ClassNameIteratorSettings.CamelCase || string.IsNullOrEmpty(settings.ClassNameIteratorSettings.WordsSeparator) == false)
                                className += NameHelper.TransformName(schema, settings.ClassNameIteratorSettings.WordsSeparator, settings.ClassNameIteratorSettings.CamelCase, settings.ClassNameIteratorSettings.UpperCase, settings.ClassNameIteratorSettings.LowerCase);
                            else if (settings.ClassNameIteratorSettings.UpperCase)
                                className += schema.ToUpper();
                            else if (settings.ClassNameIteratorSettings.LowerCase)
                                className += schema.ToLower();
                            else
                                className += schema;

                            // schema separator
                            if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.SchemaSeparator) == false)
                                className += settings.ClassNameIteratorSettings.SchemaSeparator;
                        }
                    }
                }

                // name
                if (settings.ClassNameIteratorSettings.Singular)
                {
                    if (dbObjectType == DbObjectType.Table || dbObjectType == DbObjectType.View || dbObjectType == DbObjectType.TVP)
                        name = NameHelper.GetSingularName(name);
                }

                if (settings.ClassNameIteratorSettings.CamelCase || string.IsNullOrEmpty(settings.ClassNameIteratorSettings.WordsSeparator) == false)
                    className += NameHelper.TransformName(name, settings.ClassNameIteratorSettings.WordsSeparator, settings.ClassNameIteratorSettings.CamelCase, settings.ClassNameIteratorSettings.UpperCase, settings.ClassNameIteratorSettings.LowerCase);
                else if (settings.ClassNameIteratorSettings.UpperCase)
                    className += name.ToUpper();
                else if (settings.ClassNameIteratorSettings.LowerCase)
                    className += name.ToLower();
                else
                    className += name;

                if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.Search) == false)
                {
                    if (settings.ClassNameIteratorSettings.SearchIgnoreCase)
                        className = Regex.Replace(className, settings.ClassNameIteratorSettings.Search, settings.ClassNameIteratorSettings.Replace ?? string.Empty, RegexOptions.IgnoreCase);
                    else
                        className = className.Replace(settings.ClassNameIteratorSettings.Search, settings.ClassNameIteratorSettings.Replace ?? string.Empty);
                }
            }
            else
            {
                // fixed name
                className += settings.ClassNameIteratorSettings.FixedClassName;
            }

            // postfix
            if (string.IsNullOrEmpty(settings.ClassNameIteratorSettings.Suffix) == false)
                className += settings.ClassNameIteratorSettings.Suffix;

            return className;
        }

        #endregion

        #region Class Start

        protected virtual void WriteClassStart(string className, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.WriteKeyword("public");
            writer.Write(" ");
            if (settings.POCOIteratorSettings.PartialClass)
            {
                writer.WriteKeyword("partial");
                writer.Write(" ");
            }
            writer.WriteKeyword("class");
            writer.Write(" ");
            writer.WriteUserType(className);
            if (string.IsNullOrEmpty(settings.POCOIteratorSettings.Inherit) == false)
            {
                writer.Write(" : ");
                string[] inherit = settings.POCOIteratorSettings.Inherit.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                writer.WriteUserType(inherit[0]);
                for (int i = 1; i < inherit.Length; i++)
                {
                    writer.Write(", ");
                    writer.WriteUserType(inherit[i]);
                }
            }
            writer.WriteLine();

            writer.Write(namespaceOffset);
            writer.WriteLine("{");
        }

        #endregion

        #region Class Constructor

        protected virtual void WriteConstructor(string className, List<IEnumColumn> enumColumns, List<INavigationProperty> navigationProperties, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            List<ITableColumn> tableColumnsWithColumnDefault = GetTableColumnsWithColumnDefaultConstructor(dbObject);
            bool isConstructorHasColumnDefaults = tableColumnsWithColumnDefault.HasAny();

            List<ITableColumn> tableColumnsWithEnum = GetTableColumnsWithEnumConstructor(dbObject, enumColumns);
            bool isConstructorHasEnumColumns = tableColumnsWithEnum.HasAny();

            if (isConstructorHasColumnDefaults && isConstructorHasEnumColumns)
            {
                tableColumnsWithColumnDefault.RemoveAll(c => tableColumnsWithEnum.Contains(c));
                isConstructorHasColumnDefaults = tableColumnsWithColumnDefault.HasAny();
            }

            bool isConstructorHasNavigationProperties = IsConstructorHasNavigationProperties(dbObject, navigationProperties);

            if (isConstructorHasColumnDefaults || isConstructorHasEnumColumns || isConstructorHasNavigationProperties)
            {
                WriteConstructorStart(className, dbObject, namespaceOffset);

                if (isConstructorHasColumnDefaults && isConstructorHasEnumColumns == false)
                {
                    foreach (var column in tableColumnsWithColumnDefault.OrderBy(c => c.ColumnOrdinal ?? 0))
                        WriteColumnDefaultConstructorInitialization(column, namespaceOffset);
                }
                else if (isConstructorHasEnumColumns && isConstructorHasColumnDefaults == false)
                {
                    foreach (var column in tableColumnsWithEnum.OrderBy(c => c.ColumnOrdinal ?? 0))
                        WriteEnumConstructorInitialization(column, namespaceOffset);
                }
                else if (isConstructorHasColumnDefaults && isConstructorHasEnumColumns)
                {
                    var lst1 = tableColumnsWithColumnDefault.Select(column => new { column, IsColumnDefault = true });
                    var lst2 = tableColumnsWithEnum.Select(column => new { column, IsColumnDefault = false });
                    foreach (var item in lst1.Union(lst2).OrderBy(i => i.column.ColumnOrdinal ?? 0))
                    {
                        if (item.IsColumnDefault)
                            WriteColumnDefaultConstructorInitialization(item.column, namespaceOffset);
                        else
                            WriteEnumConstructorInitialization(item.column, namespaceOffset);
                    }
                }

                if ((isConstructorHasColumnDefaults || isConstructorHasEnumColumns) && isConstructorHasNavigationProperties)
                    writer.WriteLine();

                if (isConstructorHasNavigationProperties)
                {
                    foreach (var np in navigationProperties.Where(p => p.IsCollection))
                        WriteNavigationPropertyConstructorInitialization(np, namespaceOffset);
                }

                WriteConstructorEnd(dbObject, namespaceOffset);
                writer.WriteLine();
            }
        }

        protected virtual void WriteConstructorStart(string className, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("public");
            writer.Write(" ");
            writer.Write(className);
            writer.WriteLine("()");

            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteLine("{");
        }

        protected virtual void WriteConstructorEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteLine("}");
        }

        #region Class Constructor - Column Defaults

        protected virtual List<ITableColumn> GetTableColumnsWithColumnDefaultConstructor(IDbObjectTraverse dbObject)
        {
            return GetColumnDefaults_NotNull(dbObject);
        }

        protected virtual List<ITableColumn> GetColumnDefaults_NotNull(IDbObjectTraverse dbObject)
        {
            if (settings.POCOIteratorSettings.ColumnDefaults && dbObject.DbObjectType == DbObjectType.Table)
            {
                ITable table = (ITable)dbObject;
                if (table.TableColumns.HasAny())
                    return table.TableColumns.Where(c => c.ColumnDefault != null).ToList();
            }

            return null;
        }

        protected virtual List<ITableColumn> GetColumnDefaults_NotNullOrEmpty(IDbObjectTraverse dbObject)
        {
            if (settings.POCOIteratorSettings.ColumnDefaults && dbObject.DbObjectType == DbObjectType.Table)
            {
                ITable table = (ITable)dbObject;
                if (table.TableColumns.HasAny())
                    return table.TableColumns.Where(c => string.IsNullOrEmpty(c.ColumnDefault) == false).ToList();
            }

            return null;
        }

        protected virtual void WriteColumnDefaultConstructorInitialization(ITableColumn column, string namespaceOffset)
        {
            string dataTypeName = (column.DataTypeName ?? string.Empty).ToLower();
            string cleanColumnDefault = CleanColumnDefault(column.ColumnDefault);

            if (IsSQLTypeMappedToBool(dataTypeName, column.IsUnsigned, column.NumericPrecision))
            {
                if (IsSQLTypeMappedToBoolTrue(dataTypeName, column.IsUnsigned, column.NumericPrecision, cleanColumnDefault)) { WriteColumnDefaultConstructorInitializationBool(true, column, namespaceOffset); }
                else if (IsSQLTypeMappedToBoolFalse(dataTypeName, column.IsUnsigned, column.NumericPrecision, cleanColumnDefault)) { WriteColumnDefaultConstructorInitializationBool(false, column, namespaceOffset); }
                else { WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset); }
            }
            else if (IsSQLTypeMappedToByte(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationByte(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToSByte(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationSByte(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToShort(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationShort(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToUShort(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationUShort(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToInt(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationInt(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToUInt(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationUInt(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToLong(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationLong(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToULong(dataTypeName, column.IsUnsigned, column.NumericPrecision)) { WriteColumnDefaultConstructorInitializationULong(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToFloat(dataTypeName, column.IsUnsigned, column.NumericPrecision, column.NumericScale)) { WriteColumnDefaultConstructorInitializationFloat(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToDouble(dataTypeName, column.IsUnsigned, column.NumericPrecision, column.NumericScale)) { WriteColumnDefaultConstructorInitializationDouble(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToDecimal(dataTypeName, column.IsUnsigned, column.NumericPrecision, column.NumericScale)) { WriteColumnDefaultConstructorInitializationDecimal(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToDateTime(dataTypeName, column.DateTimePrecision))
            {
                if (IsColumnDefaultNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTime_Now(column, namespaceOffset);
                else if (IsColumnDefaultUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTime_UtcNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTime_OffsetNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTime_OffsetUtcNow(column, namespaceOffset);
                else
                    WriteColumnDefaultConstructorInitializationDateTime(column, namespaceOffset, cleanColumnDefault);
            }
            else if (IsSQLTypeMappedToTimeSpan(dataTypeName, column.DateTimePrecision))
            {
                if (IsColumnDefaultNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationTimeSpan_Now(column, namespaceOffset);
                else if (IsColumnDefaultUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationTimeSpan_UtcNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationTimeSpan_OffsetNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationTimeSpan_OffsetUtcNow(column, namespaceOffset);
                else
                    WriteColumnDefaultConstructorInitializationTimeSpan(column, namespaceOffset, cleanColumnDefault);
            }
            else if (IsSQLTypeMappedToDateTimeOffset(dataTypeName, column.DateTimePrecision))
            {
                if (IsColumnDefaultNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTimeOffset_Now(column, namespaceOffset);
                else if (IsColumnDefaultUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTimeOffset_UtcNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTimeOffset_OffsetNow(column, namespaceOffset);
                else if (IsColumnDefaultOffsetUtcNow(cleanColumnDefault))
                    WriteColumnDefaultConstructorInitializationDateTimeOffset_OffsetUtcNow(column, namespaceOffset);
                else
                    WriteColumnDefaultConstructorInitializationDateTimeOffset(column, namespaceOffset, cleanColumnDefault);
            }
            else if (IsSQLTypeMappedToString(dataTypeName, column.StringPrecision)) { WriteColumnDefaultConstructorInitializationString(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToByteArray(dataTypeName)) { WriteColumnDefaultConstructorInitializationByteArray(cleanColumnDefault, column, namespaceOffset); }
            else if (IsSQLTypeMappedToGuid(dataTypeName) && IsSQLTypeMappedToGuidNewGuid(dataTypeName, cleanColumnDefault)) { WriteColumnDefaultConstructorInitializationNewGuid(column, namespaceOffset); }
            else if (IsSQLTypeRDBMSSpecificType(dataTypeName)) { WriteColumnDefaultConstructorInitializationRDBMSSpecificType(cleanColumnDefault, column, namespaceOffset); }
            else { WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset); }
        }

        protected virtual bool IsSQLTypeMappedToBool(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToBoolTrue(string dataTypeName, bool isUnsigned, int? numericPrecision, string cleanColumnDefault) { return false; }
        protected virtual bool IsSQLTypeMappedToBoolFalse(string dataTypeName, bool isUnsigned, int? numericPrecision, string cleanColumnDefault) { return false; }
        protected virtual bool IsSQLTypeMappedToByte(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToSByte(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToShort(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToUShort(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToInt(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToUInt(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToLong(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToULong(string dataTypeName, bool isUnsigned, int? numericPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToFloat(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale) { return false; }
        protected virtual bool IsSQLTypeMappedToDouble(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale) { return false; }
        protected virtual bool IsSQLTypeMappedToDecimal(string dataTypeName, bool isUnsigned, int? numericPrecision, int? numericScale) { return false; }
        protected virtual bool IsSQLTypeMappedToDateTime(string dataTypeName, int? dateTimePrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToTimeSpan(string dataTypeName, int? dateTimePrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToDateTimeOffset(string dataTypeName, int? dateTimePrecision) { return false; }
        protected virtual bool IsColumnDefaultNow(string cleanColumnDefault) { return false; }
        protected virtual bool IsColumnDefaultUtcNow(string cleanColumnDefault) { return false; }
        protected virtual bool IsColumnDefaultOffsetNow(string cleanColumnDefault) { return false; }
        protected virtual bool IsColumnDefaultOffsetUtcNow(string cleanColumnDefault) { return false; }
        protected virtual bool IsSQLTypeMappedToString(string dataTypeName, int? stringPrecision) { return false; }
        protected virtual bool IsSQLTypeMappedToByteArray(string dataTypeName) { return false; }
        protected virtual bool IsSQLTypeMappedToGuid(string dataTypeName) { return false; }
        protected virtual bool IsSQLTypeMappedToGuidNewGuid(string dataTypeName, string cleanColumnDefault) { return false; }
        protected virtual bool IsSQLTypeRDBMSSpecificType(string dataTypeName) { return false; }

        protected virtual string CleanColumnDefault(string columnDefault)
        {
            if (columnDefault.StartsWith("('") && columnDefault.EndsWith("')"))
                columnDefault = columnDefault.Substring(2, columnDefault.Length - 4);
            else if (columnDefault.StartsWith("(N'") && columnDefault.EndsWith("')"))
                columnDefault = columnDefault.Substring(3, columnDefault.Length - 5);
            else if (columnDefault.StartsWith("((") && columnDefault.EndsWith("))"))
                columnDefault = columnDefault.Substring(2, columnDefault.Length - 4);
            return columnDefault;
        }

        protected virtual void WriteColumnDefaultConstructorInitializationStart(ITableColumn column, string namespaceOffset, bool isComment = false)
        {
            string cleanColumnName = NameHelper.CleanName(column.ColumnName);
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            if (isComment)
            {
                writer.WriteComment("/* this.");
                writer.WriteComment(cleanColumnName);
                writer.WriteComment(" = ");
            }
            else
            {
                writer.WriteKeyword("this");
                writer.Write(".");
                writer.Write(cleanColumnName);
                writer.Write(" = ");
            }
        }

        protected virtual void WriteColumnDefaultConstructorInitializationEnd(bool isComment = false)
        {
            if (isComment)
                writer.WriteLineComment("; */");
            else
                writer.WriteLine(";");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationBool(bool value, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            writer.WriteKeyword(value.ToString().ToLower());
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationByte(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationSByte(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationShort(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationUShort(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationInt(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationUInt(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset, "u");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationLong(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset, "l");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationULong(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset, "ul");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationFloat(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset, "f");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDouble(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDecimal(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationNumber(columnDefault, column, namespaceOffset, "m");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationNumber(string columnDefault, ITableColumn column, string namespaceOffset, string suffix = null)
        {
            columnDefault = CleanNumberDefault(columnDefault);
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            writer.Write(columnDefault);
            if (string.IsNullOrEmpty(suffix) == false)
                writer.Write(suffix);
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual string CleanNumberDefault(string columnDefault)
        {
            return columnDefault.Replace("(", string.Empty).Replace(")", string.Empty);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime_Now(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTime_Now();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime_UtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTime_UtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime_OffsetNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTime_OffsetNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime_OffsetUtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTime_OffsetUtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTime(ITableColumn column, string namespaceOffset, string cleanColumnDefault)
        {
            DateTime dateTime;
            if (DateTime.TryParse(cleanColumnDefault, out dateTime))
            {
                WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                WriteDateTime(dateTime);
                WriteColumnDefaultConstructorInitializationEnd();
            }
            else
            {
                WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset);
            }
        }

        protected virtual void WriteColumnDefaultConstructorInitializationTimeSpan_Now(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteTimeSpan_Now();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationTimeSpan_UtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteTimeSpan_UtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationTimeSpan_OffsetNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteTimeSpan_OffsetNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationTimeSpan_OffsetUtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteTimeSpan_OffsetUtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected static readonly Regex regexTimeSpan = new Regex(@"^(?<hh>-?\d+)\:(?<mm>\d{2})\:(?<ss>\d{2})(?:\.(?<ms>\d+))?$", RegexOptions.Compiled);

        protected virtual void WriteColumnDefaultConstructorInitializationTimeSpan(ITableColumn column, string namespaceOffset, string cleanColumnDefault)
        {
            Match match = regexTimeSpan.Match(cleanColumnDefault);
            if (match.Success)
            {
                int hours = int.Parse(match.Groups["hh"].Value);
                int minutes = int.Parse(match.Groups["mm"].Value);
                int seconds = int.Parse(match.Groups["ss"].Value);
                int milliseconds = 0;
                if (match.Groups["ms"].Success)
                    milliseconds = int.Parse(match.Groups["ms"].Value);

                WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                WriteTimeSpan(hours, minutes, seconds, milliseconds);
                WriteColumnDefaultConstructorInitializationEnd();
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(cleanColumnDefault, out dateTime))
                {
                    WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                    WriteTimeSpan(dateTime);
                    WriteColumnDefaultConstructorInitializationEnd();
                }
                else
                {
                    WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset);
                }
            }
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeOffset_Now(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTimeOffset_Now();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeOffset_UtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTimeOffset_UtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeOffset_OffsetNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTimeOffset_OffsetNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeOffset_OffsetUtcNow(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            WriteDateTimeOffset_OffsetUtcNow();
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationDateTimeOffset(ITableColumn column, string namespaceOffset, string cleanColumnDefault)
        {
            DateTimeOffset dateTimeOffset;
            if (DateTimeOffset.TryParse(cleanColumnDefault, out dateTimeOffset))
            {
                WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                WriteDateTimeOffset(dateTimeOffset);
                WriteColumnDefaultConstructorInitializationEnd();
            }
            else
            {
                DateTime dateTime;
                if (DateTime.TryParse(cleanColumnDefault, out dateTime))
                {
                    WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                    WriteDateTimeOffset(dateTime);
                    WriteColumnDefaultConstructorInitializationEnd();
                }
                else
                {
                    WriteColumnDefaultConstructorInitializationComment(column.ColumnDefault, column, namespaceOffset);
                }
            }
        }

        protected virtual void WriteDateTime_Now()
        {
            writer.WriteUserType("DateTime");
            writer.Write(".Now");
        }

        protected virtual void WriteDateTime_UtcNow()
        {
            writer.WriteUserType("DateTime");
            writer.Write(".UtcNow");
        }

        protected virtual void WriteDateTime_OffsetNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".Now.DateTime");
        }

        protected virtual void WriteDateTime_OffsetUtcNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".UtcNow.UtcDateTime");
        }

        protected virtual void WriteDateTime(DateTime dateTime)
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("DateTime");
            writer.Write("(");
            writer.Write(dateTime.Year.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTime.Month.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTime.Day.ToString(CultureInfo.InvariantCulture));
            if (dateTime.Hour != 0 || dateTime.Minute != 0 || dateTime.Second != 0 || dateTime.Millisecond != 0)
            {
                writer.Write(", ");
                writer.Write(dateTime.Hour.ToString(CultureInfo.InvariantCulture));
                writer.Write(", ");
                writer.Write(dateTime.Minute.ToString(CultureInfo.InvariantCulture));
                writer.Write(", ");
                writer.Write(dateTime.Second.ToString(CultureInfo.InvariantCulture));
                if (dateTime.Millisecond != 0)
                {
                    writer.Write(", ");
                    writer.Write(dateTime.Millisecond.ToString(CultureInfo.InvariantCulture));
                }
            }
            writer.Write(")");
        }

        protected virtual void WriteTimeSpan_Now()
        {
            writer.WriteUserType("DateTime");
            writer.Write(".Now.TimeOfDay");
        }

        protected virtual void WriteTimeSpan_UtcNow()
        {
            writer.WriteUserType("DateTime");
            writer.Write(".UtcNow.TimeOfDay");
        }

        protected virtual void WriteTimeSpan_OffsetNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".Now.DateTime.TimeOfDay");
        }

        protected virtual void WriteTimeSpan_OffsetUtcNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".UtcNow.UtcDateTime.TimeOfDay");
        }

        protected virtual void WriteTimeSpan(TimeSpan timeSpan)
        {
            WriteTimeSpan(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        protected virtual void WriteTimeSpan(int hours, int minutes, int seconds)
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("TimeSpan");
            writer.Write("(");
            writer.Write(hours.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(minutes.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(seconds.ToString(CultureInfo.InvariantCulture));
            writer.Write(")");
        }

        protected virtual void WriteTimeSpan(int hours, int minutes, int seconds, int milliseconds)
        {
            if (milliseconds == 0)
            {
                WriteTimeSpan(hours, minutes, seconds);
            }
            else
            {
                writer.WriteKeyword("new ");
                writer.WriteUserType("TimeSpan");
                writer.Write("(0, ");
                writer.Write(hours.ToString(CultureInfo.InvariantCulture));
                writer.Write(", ");
                writer.Write(minutes.ToString(CultureInfo.InvariantCulture));
                writer.Write(", ");
                writer.Write(seconds.ToString(CultureInfo.InvariantCulture));
                writer.Write(", ");
                writer.Write(milliseconds.ToString(CultureInfo.InvariantCulture));
                writer.Write(")");
            }
        }

        protected virtual void WriteTimeSpan(DateTime dateTime)
        {
            WriteDateTime(dateTime);
            writer.Write(".TimeOfDay");
        }

        protected virtual void WriteDateTimeOffset_Now()
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("DateTimeOffset");
            writer.Write("(");
            writer.WriteUserType("DateTime");
            writer.Write(".Now)");
        }

        protected virtual void WriteDateTimeOffset_UtcNow()
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("DateTimeOffset");
            writer.Write("(");
            writer.WriteUserType("DateTime");
            writer.Write(".UtcNow)");
        }

        protected virtual void WriteDateTimeOffset_OffsetNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".Now");
        }

        protected virtual void WriteDateTimeOffset_OffsetUtcNow()
        {
            writer.WriteUserType("DateTimeOffset");
            writer.Write(".UtcNow");
        }

        protected virtual void WriteDateTimeOffset(DateTimeOffset dateTimeOffset)
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("DateTimeOffset");
            writer.Write("(");
            writer.Write(dateTimeOffset.Year.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Month.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Day.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Hour.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Minute.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Second.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            writer.Write(dateTimeOffset.Millisecond.ToString(CultureInfo.InvariantCulture));
            writer.Write(", ");
            WriteTimeSpan(dateTimeOffset.Offset);
            writer.Write(")");
        }

        protected virtual void WriteDateTimeOffset(DateTime dateTime)
        {
            writer.WriteKeyword("new ");
            writer.WriteUserType("DateTimeOffset");
            writer.Write("(");
            WriteDateTime(dateTime);
            writer.Write(")");
        }

        protected virtual void WriteColumnDefaultConstructorInitializationString(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            columnDefault = CleanStringDefault(columnDefault);
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            writer.WriteString("\"");
            writer.WriteString(columnDefault);
            writer.WriteString("\"");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual string CleanStringDefault(string columnDefault)
        {
            return columnDefault.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        protected abstract void WriteColumnDefaultConstructorInitializationByteArray(string columnDefault, ITableColumn column, string namespaceOffset);

        protected virtual void WriteColumnDefaultConstructorInitializationByteArray_Hex(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            columnDefault = CleanBinaryDefault(columnDefault);
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            writer.WriteUserType("BitConverter");
            writer.Write(".GetBytes(");
            writer.WriteUserType("Convert");
            writer.Write(".ToInt64(");
            writer.WriteString("\"");
            if (columnDefault.StartsWith("0x") == false)
                writer.WriteString("0x");
            writer.WriteString(columnDefault);
            writer.WriteString("\"");
            writer.Write(", 16)");
            writer.Write(")");
            WriteColumnDefaultConstructorInitializationEnd();

            string cleanColumnName = NameHelper.CleanName(column.ColumnName);
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("if");
            writer.Write(" (");
            writer.WriteUserType("BitConverter");
            writer.WriteLine(".IsLittleEndian)");
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteUserType("Array");
            writer.Write(".Reverse(");
            writer.WriteKeyword("this");
            writer.Write(".");
            writer.Write(cleanColumnName);
            writer.WriteLine(");");
        }

        protected virtual string CleanBinaryDefault(string columnDefault)
        {
            return columnDefault.Replace("(", string.Empty).Replace(")", string.Empty);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationByteArray_String(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationByteArray_Hex("0x" + BitConverter.ToString(Encoding.UTF8.GetBytes(columnDefault)).Replace("-", string.Empty), column, namespaceOffset);
        }

        protected virtual void WriteColumnDefaultConstructorInitializationNewGuid(ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
            writer.WriteUserType("Guid");
            writer.Write(".NewGuid()");
            WriteColumnDefaultConstructorInitializationEnd();
        }

        protected virtual void WriteColumnDefaultConstructorInitializationRDBMSSpecificType(string columnDefault, ITableColumn column, string namespaceOffset)
        {
        }

        protected virtual void WriteColumnDefaultConstructorInitializationComment(string columnDefault, ITableColumn column, string namespaceOffset)
        {
            WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset, true);
            writer.WriteComment(columnDefault);
            WriteColumnDefaultConstructorInitializationEnd(true);
        }

        #endregion

        #region Class Constructor - Enum Columns

        protected virtual List<ITableColumn> GetTableColumnsWithEnumConstructor(IDbObjectTraverse dbObject, List<IEnumColumn> enumColumns)
        {
            return null;
        }

        protected virtual void WriteEnumConstructorInitialization(ITableColumn column, string namespaceOffset)
        {
            IEnumColumn enumColumn = column as IEnumColumn;
            if (enumColumn == null)
                return;

            string cleanColumnName = NameHelper.CleanName(enumColumn.Column.ColumnName);

            if (enumColumn.IsEnumDataType)
                WriteEnumDataTypeConstructorInitialization(column, namespaceOffset, enumColumn, cleanColumnName);
            else if (enumColumn.IsSetDataType)
                WriteSetDataTypeConstructorInitialization(column, namespaceOffset, enumColumn, cleanColumnName);
        }

        protected virtual void WriteEnumDataTypeConstructorInitialization(ITableColumn column, string namespaceOffset, IEnumColumn enumColumn, string cleanColumnName)
        {
            string literal = GetEnumDataTypeLiteralConstructorInitialization(enumColumn);

            if (string.IsNullOrEmpty(literal) == false)
            {
                string cleanLiteral = NameHelper.CleanEnumLiteral(literal);

                WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                WriteEnumName(cleanColumnName);
                writer.Write(".");
                writer.Write(cleanLiteral);
                WriteColumnDefaultConstructorInitializationEnd();
            }
        }

        protected virtual void WriteSetDataTypeConstructorInitialization(ITableColumn column, string namespaceOffset, IEnumColumn enumColumn, string cleanColumnName)
        {
            List<string> literals = GetSetDataTypeLiteralsConstructorInitialization(enumColumn);

            if (literals.HasAny())
            {
                WriteColumnDefaultConstructorInitializationStart(column, namespaceOffset);
                for (int i = 0; i < literals.Count; i++)
                {
                    string literal = literals[i];
                    string cleanLiteral = NameHelper.CleanEnumLiteral(literal);

                    if (i > 0)
                        writer.Write(" | ");
                    WriteEnumName(cleanColumnName);
                    writer.Write(".");
                    writer.Write(cleanLiteral);
                }
                WriteColumnDefaultConstructorInitializationEnd();
            }
        }

        protected virtual string GetEnumDataTypeLiteralConstructorInitialization(IEnumColumn enumColumn)
        {
            return null;
        }

        protected virtual List<string> GetSetDataTypeLiteralsConstructorInitialization(IEnumColumn enumColumn)
        {
            return null;
        }

        #endregion

        #region Class Constructor - Navigation Properties

        protected virtual bool IsConstructorHasNavigationProperties(IDbObjectTraverse dbObject, List<INavigationProperty> navigationProperties)
        {
            return
                IsNavigableObject(dbObject) &&
                navigationProperties.HasAny(p => p.IsCollection);
        }

        protected virtual void WriteNavigationPropertyConstructorInitialization(INavigationProperty navigationProperty, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("this");
            writer.Write(".");
            writer.Write(navigationProperty.ToString());
            writer.Write(" = ");
            writer.WriteKeyword("new");
            writer.Write(" ");
            writer.WriteUserType(settings.NavigationPropertiesIteratorSettings.ICollectionNavigationProperties ? "HashSet" : "List");
            writer.Write("<");
            writer.WriteUserType(navigationProperty.ClassName);
            writer.WriteLine(">();");
        }

        #endregion

        #endregion

        #region Column Attributes

        protected virtual void WriteColumnAttributes(IColumn column, string cleanColumnName, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteEFColumnAttributes(column, cleanColumnName, dbObject, namespaceOffset);
        }

        #region EF

        protected virtual void WriteEFColumnAttributes(IColumn column, string cleanColumnName, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable)
            {
                if (dbObject.DbObjectType == DbObjectType.Table)
                {
                    ITableColumn tableColumn = (ITableColumn)column;

                    // Primary Key
                    WriteEFPrimaryKeyAttribute(tableColumn, dbObject, namespaceOffset);

                    // Index
                    WriteEFIndexAttribute(tableColumn, namespaceOffset);

                    // Column
                    WriteEFColumnAttribute(tableColumn, dbObject, cleanColumnName, namespaceOffset);

                    // MaxLength
                    WriteEFMaxLengthAttribute(tableColumn, namespaceOffset);

                    // StringLength
                    WriteEFStringLengthAttribute(tableColumn, namespaceOffset);

                    // Timestamp
                    WriteEFTimestampAttribute(tableColumn, namespaceOffset);

                    // ConcurrencyCheck
                    WriteEFConcurrencyCheckAttribute(tableColumn, namespaceOffset);

                    // DatabaseGenerated Identity
                    WriteEFDatabaseGeneratedIdentityAttribute(tableColumn, namespaceOffset);

                    // DatabaseGenerated Computed
                    WriteEFDatabaseGeneratedComputedAttribute(tableColumn, namespaceOffset);

                    // Required
                    WriteEFRequiredAttribute(tableColumn, namespaceOffset);

                    // Display
                    WriteEFDisplayAttribute(tableColumn, namespaceOffset);
                }
                else if (dbObject.DbObjectType == DbObjectType.View)
                {
                    ITableColumn tableColumn = (ITableColumn)column;

                    // Index
                    WriteEFIndexAttribute(tableColumn, namespaceOffset);
                }

                // Description
                WriteEFDescriptionAttribute(column, namespaceOffset);
            }
        }

        protected virtual void WriteEFPrimaryKeyAttribute(ITableColumn tableColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            bool isPrimaryKey = (tableColumn.PrimaryKeyColumn != null);

            if (isPrimaryKey)
            {
                bool isCompositePrimaryKey = IsCompositePrimaryKey(dbObject);

                if (isCompositePrimaryKey)
                    WriteEFCompositePrimaryKey(tableColumn.ColumnName, tableColumn.DataTypeName, tableColumn.PrimaryKeyColumn.Ordinal, namespaceOffset);
                else
                    WriteEFPrimaryKey(namespaceOffset);
            }
        }

        protected virtual void WriteEFIndexAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Index && tableColumn.IndexColumns.HasAny())
            {
                foreach (var indexColumn in tableColumn.IndexColumns.OrderBy(ic => ic.Index.Name))
                {
                    bool isCompositeIndex = (indexColumn.Index.IndexColumns.Count > 1);
                    if (isCompositeIndex)
                        WriteEFCompositeIndex(indexColumn.Index.Name, indexColumn.Index.Is_Unique, indexColumn.Index.Is_Clustered, indexColumn.Is_Descending, indexColumn.Ordinal, namespaceOffset);
                    else
                        WriteEFIndex(indexColumn.Index.Name, indexColumn.Index.Is_Unique, indexColumn.Index.Is_Clustered, indexColumn.Is_Descending, namespaceOffset);
                }
            }
        }

        protected virtual void WriteEFColumnAttribute(ITableColumn tableColumn, IDbObjectTraverse dbObject, string cleanColumnName, string namespaceOffset)
        {
            bool isPrimaryKey = (tableColumn.PrimaryKeyColumn != null);
            bool isCompositePrimaryKey = IsCompositePrimaryKey(dbObject);

            if ((settings.EFAnnotationsIteratorSettings.Column && (isPrimaryKey == false || isCompositePrimaryKey == false)) ||
                (tableColumn.ColumnName != cleanColumnName))
            {
                //debug
                if (tableColumn is ComplexType == false)
                {
                    WriteEFColumn(tableColumn.ColumnName, tableColumn.DataTypeName, namespaceOffset);
                }
            }
        }

        protected virtual void WriteEFMaxLengthAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (IsEFAttributeMaxLength(tableColumn.DataTypeName))
                WriteEFMaxLength(tableColumn.StringPrecision, namespaceOffset);
        }

        protected virtual void WriteEFStringLengthAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.StringLength)
            {
                if (IsEFAttributeStringLength(tableColumn.DataTypeName))
                {
                    if (tableColumn.StringPrecision > 0)
                        WriteEFStringLength(tableColumn.StringPrecision.Value, namespaceOffset);
                }
            }
        }

        protected virtual void WriteEFTimestampAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (IsEFAttributeTimestamp(tableColumn.DataTypeName))
                WriteEFTimestamp(namespaceOffset);
        }

        protected virtual void WriteEFConcurrencyCheckAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.ConcurrencyCheck)
            {
                if (IsEFAttributeConcurrencyCheck(tableColumn.DataTypeName))
                    WriteEFConcurrencyCheck(namespaceOffset);
            }
        }

        protected virtual void WriteEFDatabaseGeneratedIdentityAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (tableColumn.IsIdentity)
                WriteEFDatabaseGeneratedIdentity(namespaceOffset);
        }

        protected virtual void WriteEFDatabaseGeneratedComputedAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (tableColumn.IsComputed)
                WriteEFDatabaseGeneratedComputed(namespaceOffset);
        }

        protected virtual void WriteEFRequiredAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Required || settings.EFAnnotationsIteratorSettings.RequiredWithErrorMessage)
            {
                if (tableColumn.IsNullable == false)
                {
                    string display = null;
                    if (settings.EFAnnotationsIteratorSettings.RequiredWithErrorMessage)
                        display = GetEFDisplay(tableColumn.ColumnName);

                    WriteEFRequired(display, namespaceOffset);
                }
            }
        }

        protected virtual void WriteEFDisplayAttribute(ITableColumn tableColumn, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Display)
            {
                string display = GetEFDisplay(tableColumn.ColumnName);
                WriteEFDisplay(display, namespaceOffset);
            }
        }

        protected virtual void WriteEFDescriptionAttribute(IColumn column, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Description && column is IDescription)
            {
                IDescription descObject = (IDescription)column;
                if (string.IsNullOrEmpty(descObject.Description) == false)
                    WriteEFDescription(descObject.Description, true, namespaceOffset);
            }
        }

        protected abstract bool IsEFAttributeMaxLength(string dataTypeName);
        protected abstract bool IsEFAttributeStringLength(string dataTypeName);
        protected abstract bool IsEFAttributeTimestamp(string dataTypeName);
        protected abstract bool IsEFAttributeConcurrencyCheck(string dataTypeName);

        protected virtual bool IsCompositePrimaryKey(IDbObjectTraverse dbObject)
        {
            if (dbObject.Columns.HasAny())
                return (dbObject.Columns.Count(c => c is ITableColumn && ((ITableColumn)c).PrimaryKeyColumn != null) > 1);
            return false;
        }

        protected virtual void WriteEFPrimaryKey(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Key");
            writer.WriteLine("]");
        }

        protected virtual void WriteEFCompositePrimaryKey(string columnName, string dataTypeName, byte ordinal, string namespaceOffset)
        {
            WriteEFPrimaryKey(namespaceOffset);

            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Column");
            writer.Write("(");

            if (settings.EFAnnotationsIteratorSettings.Column)
            {
                writer.Write("Name = ");
                writer.WriteString("\"");
                writer.WriteString(columnName);
                writer.WriteString("\"");
                writer.Write(", TypeName = ");
                writer.WriteString("\"");
                writer.WriteString(dataTypeName);
                writer.WriteString("\"");
                writer.Write(", ");
            }

            writer.Write("Order = ");
            writer.Write(ordinal.ToString());
            writer.WriteLine(")]");
        }

        protected virtual void WriteEFIndex(string indexName, bool isUnique, bool isClustered, bool isDescending, string namespaceOffset)
        {
            WriteEFIndexSortOrderError(indexName, isDescending, namespaceOffset);
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Index");
            writer.Write("(");
            writer.WriteString("\"");
            writer.WriteString(indexName);
            writer.WriteString("\"");
            if (isUnique)
            {
                writer.Write(", IsUnique = ");
                writer.WriteKeyword("true");
            }
            if (isClustered)
            {
                writer.Write(", IsClustered = ");
                writer.WriteKeyword("true");
            }
            writer.WriteLine(")]");
        }

        protected virtual void WriteEFCompositeIndex(string indexName, bool isUnique, bool isClustered, bool isDescending, byte ordinal, string namespaceOffset)
        {
            WriteEFIndexSortOrderError(indexName, isDescending, namespaceOffset);
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Index");
            writer.Write("(");
            writer.WriteString("\"");
            writer.WriteString(indexName);
            writer.WriteString("\"");
            writer.Write(", ");
            writer.Write(ordinal.ToString());
            if (isUnique)
            {
                writer.Write(", IsUnique = ");
                writer.WriteKeyword("true");
            }
            if (isClustered)
            {
                writer.Write(", IsClustered = ");
                writer.WriteKeyword("true");
            }
            writer.WriteLine(")]");
        }

        protected virtual void WriteEFIndexSortOrderError(string indexName, bool isDescending, string namespaceOffset)
        {
            if (isDescending)
            {
                writer.Write(namespaceOffset);
                writer.Write(settings.POCOIteratorSettings.Tab);
                writer.WriteError("/* ");
                writer.WriteError(indexName);
                writer.WriteLineError(". Sort order is Descending. Index doesn't support sort order. */");
            }
        }

        protected virtual void WriteEFColumn(string columnName, string dataTypeName, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Column");
            writer.Write("(Name = ");
            writer.WriteString("\"");
            writer.WriteString(columnName);
            writer.WriteString("\"");
            writer.Write(", TypeName = ");
            writer.WriteString("\"");
            writer.WriteString(dataTypeName);
            writer.WriteString("\"");
            writer.WriteLine(")]");
        }

        protected virtual void WriteEFMaxLength(int? stringPrecision, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("MaxLength");
            if (stringPrecision >= 0)
            {
                writer.Write("(");
                writer.Write(stringPrecision.ToString());
                writer.Write(")");
            }
            writer.WriteLine("]");
        }

        protected virtual void WriteEFStringLength(int stringPrecision, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("StringLength");
            writer.Write("(");
            writer.Write(stringPrecision.ToString());
            writer.Write(")");
            writer.WriteLine("]");
        }

        protected virtual void WriteEFTimestamp(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Timestamp");
            writer.WriteLine("]");
        }

        protected virtual void WriteEFConcurrencyCheck(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("ConcurrencyCheck");
            writer.WriteLine("]");
        }

        protected virtual void WriteEFDatabaseGeneratedIdentity(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("DatabaseGenerated");
            writer.Write("(");
            writer.WriteUserType("DatabaseGeneratedOption");
            writer.WriteLine(".Identity)]");
        }

        protected virtual void WriteEFDatabaseGeneratedComputed(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("DatabaseGenerated");
            writer.Write("(");
            writer.WriteUserType("DatabaseGeneratedOption");
            writer.WriteLine(".Computed)]");
        }

        protected static readonly Regex regexDisplay1 = new Regex("[^0-9a-zA-Z]", RegexOptions.Compiled);
        protected static readonly Regex regexDisplay2 = new Regex("([^A-Z]|^)(([A-Z\\s]*)($|[A-Z]))", RegexOptions.Compiled);
        protected static readonly Regex regexDisplay3 = new Regex("\\s{2,}", RegexOptions.Compiled);

        protected virtual string GetEFDisplay(string columnName)
        {
            string display = columnName;
            display = regexDisplay1.Replace(display, " ");
            display = regexDisplay2.Replace(display, "$1 $3 $4");
            display = display.Trim();
            display = regexDisplay3.Replace(display, " ");
            return display;
        }

        protected virtual void WriteEFRequired(string display, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Required");
            if (settings.EFAnnotationsIteratorSettings.RequiredWithErrorMessage)
                WriteEFRequiredErrorMessage(display);
            writer.WriteLine("]");
        }

        protected virtual void WriteEFRequiredErrorMessage(string display)
        {
            writer.Write("(ErrorMessage = ");
            writer.WriteString("\"");
            writer.WriteString(display);
            writer.WriteString(" is required");
            writer.WriteString("\"");
            writer.Write(")");
        }

        protected virtual void WriteEFDisplay(string display, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Display");
            writer.Write("(Name = ");
            writer.WriteString("\"");
            writer.WriteString(display);
            writer.WriteString("\"");
            writer.WriteLine(")]");
        }

        protected virtual void WriteEFDescription(string description, bool writeTab, string namespaceOffset)
        {
            if (string.IsNullOrEmpty(description) == false)
            {
                writer.Write(namespaceOffset);
                if (writeTab)
                    writer.Write(settings.POCOIteratorSettings.Tab);
                writer.Write("[");
                writer.WriteUserType("Description");
                writer.Write("(");
                writer.WriteString("\"");
                writer.WriteString(NameHelper.Escape(description));
                writer.WriteString("\"");
                writer.WriteLine(")]");
            }
        }

        #endregion

        #endregion

        #region Column

        protected virtual void WriteColumn(IColumn column, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable && settings.EFAnnotationsIteratorSettings.ComplexType && dbObject.DbObjectType == DbObjectType.Table)
                WriteEFComplexTypeColumn(column, isLastColumn, dbObject, namespaceOffset);
            else
                WriteDbColumn(column, isLastColumn, dbObject, namespaceOffset);
        }

        protected virtual void WriteDbColumn(IColumn column, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            string cleanColumnName = NameHelper.CleanName(column.ColumnName);

            WriteColumnAttributes(column, cleanColumnName, dbObject, namespaceOffset);

            WriteColumnStart(namespaceOffset);

            WriteColumnDataType(column);

            WriteColumnName(cleanColumnName);

            WriteColumnEnd();

            WriteColumnComments(column);

            writer.WriteLine();

            if (settings.POCOIteratorSettings.NewLineBetweenMembers && isLastColumn == false)
                writer.WriteLine();
        }

        protected virtual void WriteColumnStart(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("public");
            writer.Write(" ");

            if (settings.POCOIteratorSettings.Properties && settings.POCOIteratorSettings.VirtualProperties)
            {
                writer.WriteKeyword("virtual");
                writer.Write(" ");
            }
            else if (settings.POCOIteratorSettings.Properties && settings.POCOIteratorSettings.OverrideProperties)
            {
                writer.WriteKeyword("override");
                writer.Write(" ");
            }
        }

        protected virtual void WriteColumnDataType(IColumn column)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable && settings.EFAnnotationsIteratorSettings.ComplexType && column is ComplexType)
                WriteEFComplexTypeColumnDataType(column);
            else
                WriteDbColumnDataType(column);
        }

        protected abstract void WriteDbColumnDataType(IColumn column);

        protected virtual void WriteColumnName(string columnName)
        {
            writer.Write(" ");
            writer.Write(columnName);
        }

        protected virtual void WriteColumnEnd()
        {
            if (settings.POCOIteratorSettings.Properties)
            {
                writer.Write(" { ");
                writer.WriteKeyword("get");
                writer.Write("; ");
                writer.WriteKeyword("set");
                writer.Write("; }");
            }
            else if (settings.POCOIteratorSettings.Fields)
            {
                writer.Write(";");
            }
        }

        protected virtual void WriteColumnComments(IColumn column)
        {
            if (settings.POCOIteratorSettings.Comments || settings.POCOIteratorSettings.CommentsWithoutNull)
            {
                writer.Write(" ");
                writer.WriteComment("//");
                writer.WriteComment(" ");
                writer.WriteComment(column.DataTypeDisplay);
                writer.WriteComment(column.Precision ?? string.Empty);

                if (settings.POCOIteratorSettings.CommentsWithoutNull == false)
                {
                    writer.WriteComment(",");
                    writer.WriteComment(" ");
                    writer.WriteComment((column.IsNullable ? "null" : "not null"));
                }
            }
        }

        #region EF

        protected List<string> complexTypeNames;
        protected List<ComplexTypeColumn> complexTypeColumns;

        protected virtual void WriteEFComplexTypeColumn(IColumn column, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            string columnName = column.ColumnName.Trim();
            int index = columnName.IndexOf('_');
            if (index != -1 && index != 0 && index != columnName.Length - 1)
            {
                string complexTypeName = NameHelper.CleanName(columnName.Substring(0, index));
                string complexTypeColumnName = columnName.Substring(index + 1);

                if (complexTypeNames == null)
                    complexTypeNames = new List<string>();
                if (complexTypeNames.Contains(complexTypeName) == false)
                {
                    ComplexType complexType = new ComplexType(complexTypeName, column.ColumnOrdinal);
                    WriteDbColumn(complexType, isLastColumn, dbObject, namespaceOffset);
                    complexTypeNames.Add(complexTypeName);
                }

                ComplexTypeColumn complexTypeColumn = new ComplexTypeColumn(complexTypeName, complexTypeColumnName, (ITableColumn)column);
                if (complexTypeColumns == null)
                    complexTypeColumns = new List<ComplexTypeColumn>();
                complexTypeColumns.Add(complexTypeColumn);
            }
            else
            {
                WriteDbColumn(column, isLastColumn, dbObject, namespaceOffset);
            }
        }

        protected virtual void WriteEFComplexTypeColumnDataType(IColumn column)
        {
            writer.WriteUserType(column.DataTypeDisplay);
        }

        #endregion

        #region Column Data Types

        protected virtual void WriteColumnBool(bool isNullable)
        {
            writer.WriteKeyword("bool");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnByte(bool isNullable)
        {
            writer.WriteKeyword("byte");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnByteArray()
        {
            writer.WriteKeyword("byte");
            writer.Write("[]");
        }

        protected virtual void WriteColumnDateTime(bool isNullable)
        {
            writer.WriteUserType("DateTime");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnDateTimeOffset(bool isNullable)
        {
            writer.WriteUserType("DateTimeOffset");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnDecimal(bool isNullable)
        {
            writer.WriteKeyword("decimal");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnDouble(bool isNullable)
        {
            writer.WriteKeyword("double");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnFloat(bool isNullable)
        {
            writer.WriteKeyword("float");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnGuid(bool isNullable)
        {
            writer.WriteUserType("Guid");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnInt(bool isNullable)
        {
            writer.WriteKeyword("int");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnLong(bool isNullable)
        {
            writer.WriteKeyword("long");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnObject()
        {
            writer.WriteKeyword("object");
        }

        protected virtual void WriteColumnSByte(bool isNullable)
        {
            writer.WriteKeyword("sbyte");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnShort(bool isNullable)
        {
            writer.WriteKeyword("short");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnString()
        {
            writer.WriteKeyword("string");
        }

        protected virtual void WriteColumnTimeSpan(bool isNullable)
        {
            writer.WriteUserType("TimeSpan");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnUInt(bool isNullable)
        {
            writer.WriteKeyword("uint");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnULong(bool isNullable)
        {
            writer.WriteKeyword("ulong");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnUShort(bool isNullable)
        {
            writer.WriteKeyword("ushort");
            if (isNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        protected virtual void WriteColumnEnum(IEnumColumn enumColumn)
        {
            string cleanColumnName = NameHelper.CleanName(enumColumn.Column.ColumnName);
            WriteEnumName(cleanColumnName);
            if (enumColumn.Column.IsNullable || settings.POCOIteratorSettings.StructTypesNullable)
                writer.Write("?");
        }

        #endregion

        #endregion

        #region Enums

        protected virtual List<IEnumColumn> GetEnumColumns(IDbObjectTraverse dbObject)
        {
            if (support.IsSupportEnumDataType)
            {
                if (settings.POCOIteratorSettings.EnumSQLTypeToString == false && (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort || settings.POCOIteratorSettings.EnumSQLTypeToEnumInt))
                {
                    if (dbObject.Columns != null && dbObject.Columns.Any(c => c is IEnumColumn))
                    {
                        return dbObject.Columns
                            .Where(c => c is IEnumColumn)
                            .Cast<IEnumColumn>()
                            .Where(c => c.IsEnumDataType || c.IsSetDataType)
                            .OrderBy<IEnumColumn, int>(c => c.Column.ColumnOrdinal ?? 0)
                            .ToList();
                    }
                }
            }

            return null;
        }

        protected virtual void WriteEnum(IEnumColumn enumColumn, bool isLastColumn, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.WriteLine();

            string cleanColumnName = NameHelper.CleanName(enumColumn.Column.ColumnName);

            List<string> literals = enumColumn.EnumLiterals.Select(name => NameHelper.CleanEnumLiteral(name)).ToList();

            if (enumColumn.IsEnumDataType)
                WriteEnumDeclaration(cleanColumnName, literals, namespaceOffset);
            else if (enumColumn.IsSetDataType)
                WriteSetDeclaration(cleanColumnName, literals, namespaceOffset);
        }

        protected virtual void WriteEnumDeclaration(string columnName, List<string> literals, string namespaceOffset)
        {
            WriteEnumStart(columnName, false, namespaceOffset);

            for (int i = 0; i < literals.Count; i++)
            {
                string literal = literals[i];
                string literalValue = (i + 1).ToString();
                bool isLastLiteral = (i < literals.Count - 1);
                WriteEnumLiteral(literal, literalValue, isLastLiteral, namespaceOffset);
            }

            WriteEnumEnd(namespaceOffset);
        }

        protected virtual void WriteSetDeclaration(string columnName, List<string> literals, string namespaceOffset)
        {
            WriteEnumFlags(namespaceOffset);
            WriteEnumStart(columnName, true, namespaceOffset);

            for (int i = 0; i < literals.Count; i++)
            {
                string literal = literals[i];

                string literalValue = "1";
                if (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort)
                    literalValue += "ul";

                if (i > 0)
                {
                    literalValue += " << ";
                    literalValue += i.ToString();
                }

                bool isLastLiteral = (i < literals.Count - 1);

                WriteEnumLiteral(literal, literalValue, isLastLiteral, namespaceOffset);
            }

            WriteEnumEnd(namespaceOffset);
        }

        protected virtual void WriteEnumFlags(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("Flags");
            writer.Write("]");
            writer.WriteLine();
        }

        protected virtual void WriteEnumStart(string columnName, bool isSetDataType, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("public");
            writer.Write(" ");
            writer.WriteKeyword("enum");
            writer.Write(" ");
            WriteEnumName(columnName);
            writer.Write(" : ");
            if (settings.POCOIteratorSettings.EnumSQLTypeToEnumUShort)
            {
                if (isSetDataType)
                    writer.WriteKeyword("ulong");
                else
                    writer.WriteKeyword("ushort");
            }
            else if (settings.POCOIteratorSettings.EnumSQLTypeToEnumInt)
            {
                writer.WriteKeyword("int");
            }
            writer.WriteLine();

            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("{");
            writer.WriteLine();
        }

        protected virtual void WriteEnumName(string columnName)
        {
            WriteEnumNamePrefix();
            writer.WriteUserType(columnName);
            WriteEnumNameSuffix();
        }

        protected virtual void WriteEnumNamePrefix()
        {
        }

        protected virtual void WriteEnumNameSuffix()
        {
            writer.WriteUserType("_Values");
        }

        protected virtual void WriteEnumLiteral(string literal, string literalValue, bool isLastLiteral, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write(literal);
            writer.Write(" = ");
            writer.Write(literalValue);
            if (isLastLiteral)
                writer.Write(",");
            writer.WriteLine();
        }

        protected virtual void WriteEnumEnd(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("}");
            writer.WriteLine();
        }

        #endregion

        #region Navigation Properties

        protected virtual bool IsNavigableObject(IDbObjectTraverse dbObject)
        {
            return
                settings.NavigationPropertiesIteratorSettings.Enable &&
                dbObject.DbObjectType == DbObjectType.Table;
        }

        #region Get Navigation Properties

        protected virtual List<INavigationProperty> GetNavigationProperties(IDbObjectTraverse dbObject)
        {
            List<INavigationProperty> navigationProperties = null;

            if (IsNavigableObject(dbObject))
            {
                ITable table = (ITable)dbObject;

                if (table.ForeignKeys.HasAny())
                {
                    navigationProperties = new List<INavigationProperty>();

                    foreach (var fk in table.ForeignKeys)
                    {
                        if ((settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable &&
                            fk.NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOn) ||
                            (settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable == false &&
                            fk.NavigationPropertyFromForeignToPrimary.IsVisibleWhenShowManyToManyJoinTableIsOff))
                        {
                            string className = GetClassName(
                                dbObject.Database.ToString(),
                                (fk.PrimaryTable is ISchema ? ((ISchema)fk.PrimaryTable).Schema : null),
                                fk.PrimaryTable.Name,
                                dbObject.DbObjectType
                            );

                            fk.NavigationPropertyFromForeignToPrimary.ClassName = className;
                            navigationProperties.Add(fk.NavigationPropertyFromForeignToPrimary);
                        }
                    }
                }

                if (table.PrimaryForeignKeys.HasAny())
                {
                    if (navigationProperties == null)
                        navigationProperties = new List<INavigationProperty>();

                    foreach (var fk in table.PrimaryForeignKeys)
                    {
                        if ((settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable &&
                            fk.NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOn) ||
                            (settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable == false &&
                            fk.NavigationPropertyFromPrimaryToForeign.IsVisibleWhenShowManyToManyJoinTableIsOff))
                        {
                            string className = GetClassName(
                                dbObject.Database.ToString(),
                                (fk.ForeignTable is ISchema ? ((ISchema)fk.ForeignTable).Schema : null),
                                fk.ForeignTable.Name,
                                dbObject.DbObjectType
                            );

                            fk.NavigationPropertyFromPrimaryToForeign.ClassName = className;
                            navigationProperties.Add(fk.NavigationPropertyFromPrimaryToForeign);
                        }

                        if (fk.VirtualNavigationProperties.HasAny())
                        {
                            foreach (var vnp in fk.VirtualNavigationProperties)
                            {
                                if ((settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable &&
                                    vnp.IsVisibleWhenShowManyToManyJoinTableIsOn) ||
                                    (settings.NavigationPropertiesIteratorSettings.ShowManyToManyJoinTable == false &&
                                    vnp.IsVisibleWhenShowManyToManyJoinTableIsOff))
                                {
                                    var vfk = vnp.ForeignKey;

                                    string className = GetClassName(
                                        dbObject.Database.ToString(),
                                        (vfk.ForeignTable is ISchema ? ((ISchema)vfk.ForeignTable).Schema : null),
                                        vfk.ForeignTable.Name,
                                        dbObject.DbObjectType
                                    );

                                    vnp.ClassName = className;
                                    navigationProperties.Add(vnp);
                                }
                            }
                        }
                    }
                }

                // rename duplicates
                RenameDuplicateNavigationProperties(navigationProperties);
            }

            SetNavigationPropertiesMultipleRelationships(navigationProperties);

            return navigationProperties;
        }

        protected static readonly Regex regexEndNumber = new Regex("(\\d+)$", RegexOptions.Compiled);

        protected virtual void RenameDuplicateNavigationProperties(List<INavigationProperty> navigationProperties)
        {
            if (navigationProperties.HasAny())
            {
                // groups of navigation properties with the same name
                var npGroups1 = navigationProperties.GroupBy(p => p.ToString()).Where(g => g.Count() > 1);

                // if the original column name ended with a number, then assign that number to the property name
                foreach (var npGroup in npGroups1)
                {
                    foreach (var np in npGroup)
                    {
                        string columnName = (
                            np.IsFromForeignToPrimary ?
                            np.ForeignKey.ForeignKeyColumns[0].PrimaryTableColumn.ColumnName :
                            np.ForeignKey.ForeignKeyColumns[0].ForeignTableColumn.ColumnName
                        );

                        var match = regexEndNumber.Match(columnName);
                        if (match.Success)
                            np.RenamedPropertyName = np.ToString() + match.Value;
                    }
                }

                // if there are still duplicate property names, then rename them with a running number suffix
                var npGroups2 = navigationProperties.GroupBy(p => p.ToString()).Where(g => g.Count() > 1);
                foreach (var npGroup in npGroups2)
                {
                    int suffix = 1;
                    foreach (var np in npGroup.Skip(1))
                        np.RenamedPropertyName = np.ToString() + (suffix++);
                }
            }
        }

        #endregion

        #region Write Navigation Properties

        protected virtual void WriteNavigationProperties(List<INavigationProperty> navigationProperties, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (IsNavigableObject(dbObject))
            {
                if (navigationProperties.HasAny())
                {
                    if (settings.POCOIteratorSettings.NewLineBetweenMembers == false)
                        writer.WriteLine();

                    foreach (var np in navigationProperties)
                        WriteNavigationProperty(np, dbObject, namespaceOffset);
                }
            }
        }

        protected virtual void WriteNavigationProperty(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.POCOIteratorSettings.NewLineBetweenMembers)
                writer.WriteLine();

            WriteNavigationPropertyComments(navigationProperty, dbObject, namespaceOffset);

            WriteNavigationPropertyAttributes(navigationProperty, dbObject, namespaceOffset);

            if (navigationProperty.IsCollection)
                WriteNavigationPropertyCollection(navigationProperty, dbObject, namespaceOffset);
            else
                WriteNavigationPropertySingular(navigationProperty, dbObject, namespaceOffset);
        }

        protected virtual void WriteNavigationPropertyComments(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.NavigationPropertiesIteratorSettings.Comments)
            {
                if (navigationProperty.IsVirtualNavigationProperty == false)
                {
                    foreach (var fkc in navigationProperty.ForeignKey.ForeignKeyColumns)
                    {
                        writer.Write(namespaceOffset);
                        writer.Write(settings.POCOIteratorSettings.Tab);
                        writer.WriteComment("// ");
                        if (navigationProperty.ForeignKey.ForeignTable is ISchema)
                        {
                            writer.WriteComment(((ISchema)navigationProperty.ForeignKey.ForeignTable).Schema);
                            writer.WriteComment(".");
                        }
                        writer.WriteComment(navigationProperty.ForeignKey.ForeignTable.Name);
                        writer.WriteComment(".");
                        writer.WriteComment(fkc.ForeignTableColumn.ColumnName);
                        writer.WriteComment(" -> ");
                        if (navigationProperty.ForeignKey.PrimaryTable is ISchema)
                        {
                            writer.WriteComment(((ISchema)navigationProperty.ForeignKey.PrimaryTable).Schema);
                            writer.WriteComment(".");
                        }
                        writer.WriteComment(navigationProperty.ForeignKey.PrimaryTable.Name);
                        writer.WriteComment(".");
                        writer.WriteComment(fkc.PrimaryTableColumn.ColumnName);
                        writer.WriteComment(" (");
                        writer.WriteComment(navigationProperty.ForeignKey.Name);
                        writer.WriteComment(")");
                        writer.WriteLine();
                    }
                }
            }
        }

        protected virtual void WriteNavigationPropertyAttributes(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteEFNavigationPropertyAttributes(navigationProperty, dbObject, namespaceOffset);
        }

        protected virtual void WriteNavigationPropertySingular(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteNavigationPropertyStart(namespaceOffset);
            writer.WriteUserType(navigationProperty.ClassName);
            writer.Write(" ");
            writer.Write(navigationProperty.ToString());
            WriteNavigationPropertyEnd();
            writer.WriteLine();
        }

        protected virtual void WriteNavigationPropertyCollection(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            WriteNavigationPropertyStart(namespaceOffset);
            if (settings.NavigationPropertiesIteratorSettings.ListNavigationProperties)
                writer.WriteUserType("List");
            else if (settings.NavigationPropertiesIteratorSettings.ICollectionNavigationProperties)
                writer.WriteUserType("ICollection");
            else if (settings.NavigationPropertiesIteratorSettings.IEnumerableNavigationProperties)
                writer.WriteUserType("IEnumerable");
            writer.Write("<");
            writer.WriteUserType(navigationProperty.ClassName);
            writer.Write("> ");
            writer.Write(navigationProperty.ToString());
            WriteNavigationPropertyEnd();
            writer.WriteLine();
        }

        protected virtual void WriteNavigationPropertyStart(string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.WriteKeyword("public");
            writer.Write(" ");

            if (settings.POCOIteratorSettings.Properties && settings.NavigationPropertiesIteratorSettings.VirtualNavigationProperties)
            {
                writer.WriteKeyword("virtual");
                writer.Write(" ");
            }
            else if (settings.POCOIteratorSettings.Properties && settings.NavigationPropertiesIteratorSettings.OverrideNavigationProperties)
            {
                writer.WriteKeyword("override");
                writer.Write(" ");
            }
        }

        protected virtual void WriteNavigationPropertyEnd()
        {
            if (settings.POCOIteratorSettings.Properties)
            {
                writer.Write(" { ");
                writer.WriteKeyword("get");
                writer.Write("; ");
                writer.WriteKeyword("set");
                writer.Write("; }");
            }
            else if (settings.POCOIteratorSettings.Fields)
            {
                writer.Write(";");
            }
        }

        #endregion

        #region EF

        protected virtual void SetNavigationPropertiesMultipleRelationships(List<INavigationProperty> navigationProperties)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable)
            {
                if (navigationProperties.HasAny())
                {
                    var multipleRels = navigationProperties
                        .GroupBy(np => new { np.ForeignKey.ForeignTable, np.ForeignKey.PrimaryTable })
                        .Where(g => g.Count() > 1)
                        .SelectMany(g => g);

                    foreach (var np in multipleRels)
                        np.HasMultipleRelationships = true;
                }
            }
        }

        protected virtual void WriteEFNavigationPropertyAttributes(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable)
            {
                if (settings.EFAnnotationsIteratorSettings.ForeignKeyAndInverseProperty)
                {
                    if (IsNavigableObject(dbObject))
                    {
                        if (navigationProperty.IsFromForeignToPrimary)
                            WriteNavigationPropertyForeignKeyAttribute(navigationProperty, dbObject, namespaceOffset);

                        if (navigationProperty.IsFromForeignToPrimary == false && navigationProperty.HasMultipleRelationships)
                            WriteNavigationPropertyInversePropertyAttribute(navigationProperty, dbObject, namespaceOffset);
                    }
                }
            }
        }

        protected virtual void WriteNavigationPropertyForeignKeyAttribute(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("ForeignKey");
            writer.Write("(");
            writer.WriteString("\"");
            if (navigationProperty.HasMultipleRelationships)
                writer.WriteString(string.Join(", ", navigationProperty.ForeignKey.ForeignKeyColumns.Select(c => c.ForeignTableColumn.ColumnName)));
            else
                writer.WriteString(string.Join(", ", navigationProperty.ForeignKey.ForeignKeyColumns.Select(c => c.PrimaryTableColumn.ColumnName)));
            writer.WriteString("\"");
            writer.WriteLine(")]");
        }

        protected virtual void WriteNavigationPropertyInversePropertyAttribute(INavigationProperty navigationProperty, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("InverseProperty");
            writer.Write("(");
            writer.WriteString("\"");
            writer.WriteString(navigationProperty.InverseProperty.ToString());
            writer.WriteString("\"");
            writer.WriteLine(")]");
        }

        #endregion

        #endregion

        #region Class End

        protected virtual void WriteClassEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (settings.EFAnnotationsIteratorSettings.Enable && settings.EFAnnotationsIteratorSettings.ComplexType && dbObject.DbObjectType == DbObjectType.Table)
                WriteEFComplexTypeClassEnd(dbObject, namespaceOffset);

            WriteDbClassEnd(dbObject, namespaceOffset);
        }

        protected virtual void WriteDbClassEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.Write(namespaceOffset);
            writer.WriteLine("}");
        }

        #region EF

        protected virtual void WriteEFComplexTypeClassEnd(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            if (complexTypeColumns.HasAny())
                WriteComplexTypes(dbObject, namespaceOffset);
        }

        protected virtual void WriteComplexTypes(IDbObjectTraverse dbObject, string namespaceOffset)
        {
            var complexTypes = complexTypeColumns.GroupBy(x => x.ComplexTypeName);
            foreach (var columns in complexTypes)
                WriteComplexType(columns.Key, columns, dbObject, namespaceOffset);
        }

        protected virtual void WriteComplexType(string complexTypeName, IEnumerable<ComplexTypeColumn> complexTypeColumns, IDbObjectTraverse dbObject, string namespaceOffset)
        {
            writer.WriteLine();

            // Class Attribute
            writer.Write(namespaceOffset);
            writer.Write(settings.POCOIteratorSettings.Tab);
            writer.Write("[");
            writer.WriteUserType("ComplexType");
            writer.WriteLine("]");

            namespaceOffset += settings.POCOIteratorSettings.Tab;

            // Class Start
            WriteClassStart(complexTypeName, dbObject, namespaceOffset);

            // Columns
            var columns = complexTypeColumns.OrderBy<IColumn, int>(c => c.ColumnOrdinal ?? 0);
            var lastColumn = columns.Last();
            foreach (IColumn column in columns)
                WriteDbColumn(column, column == lastColumn, dbObject, namespaceOffset);

            // Class End
            WriteDbClassEnd(dbObject, namespaceOffset);
        }

        #endregion

        #endregion

        #region Namespace End

        protected virtual void WriteNamespaceEnd(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace) == false)
                writer.WriteLine("}");
        }

        #endregion
    }
}
