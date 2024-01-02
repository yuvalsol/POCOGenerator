using System;
using POCOGenerator.Objects;

namespace POCOGenerator
{
    #region Interfaces

    /// <summary>Provides data for generating event.</summary>
    public interface IGeneratingEventArgs
    {
    }

    /// <summary>Provides data for objects group generating event.</summary>
    public interface IObjectsGeneratingEventArgs : IGeneratingEventArgs
    {
    }

    /// <summary>Provides data for object generating event.</summary>
    public interface IObjectGeneratingEventArgs : IGeneratingEventArgs
    {
        /// <summary>Gets the database object that this event argument provides data about.</summary>
        /// <value>The database object.</value>
        IDbObject DbObject { get; }

        /// <summary>Gets the POCO class name of the database object that this event argument provides data about.</summary>
        /// <value>The POCO class name of the database object.</value>
        string ClassName { get; }

        /// <summary>Gets the error message that occurred during the generating process of the database object that this event argument provides data about.</summary>
        /// <value>The error message that occurred during the generating process of the database object.</value>
        string Error { get; }

        /// <summary>Gets the namespace of the database object that this event argument provides data about.</summary>
        /// <value>The namespace of the database object.</value>
        string Namespace { get; }
    }

    /// <summary>Provides data for POCO generated event.</summary>
    public interface IPOCOEventArgs
    {
        /// <summary>Gets the database object that this event argument provides data about.</summary>
        /// <value>The database object.</value>
        IDbObject DbObject { get; }

        /// <summary>Gets the POCO class name of the database object that this event argument provides data about.</summary>
        /// <value>The POCO class name of the database object.</value>
        string ClassName { get; }

        /// <summary>Gets the error message that occurred during the generating process of the database object that this event argument provides data about.</summary>
        /// <value>The error message that occurred during the generating process of the database object.</value>
        string Error { get; }

        /// <summary>Gets the POCO class text of the database object that this event argument provides data about.</summary>
        /// <value>The POCO class text of the database object.</value>
        string POCO { get; }
    }

    /// <summary>Provides data for generated event.</summary>
    public interface IGeneratedEventArgs
    {
    }

    /// <summary>Provides data for objects group generated event.</summary>
    public interface IObjectsGeneratedEventArgs : IGeneratedEventArgs
    {
    }

    /// <summary>Provides data for object generated event.</summary>
    public interface IObjectGeneratedEventArgs : IGeneratedEventArgs
    {
        /// <summary>Gets the database object that this event argument provides data about.</summary>
        /// <value>The database object.</value>
        IDbObject DbObject { get; }

        /// <summary>Gets the POCO class name of the database object that this event argument provides data about.</summary>
        /// <value>The POCO class name of the database object.</value>
        string ClassName { get; }

        /// <summary>Gets the error message that occurred during the generating process of the database object that this event argument provides data about.</summary>
        /// <value>The error message that occurred during the generating process of the database object.</value>
        string Error { get; }

        /// <summary>Gets the namespace of the database object that this event argument provides data about.</summary>
        /// <value>The namespace of the database object.</value>
        string Namespace { get; }
    }

    /// <summary>Provides data for server generating or server generated event.</summary>
    public interface IServerEventArgs
    {
        /// <summary>Gets the server that this event argument provides data about.</summary>
        /// <value>The server.</value>
        Server Server { get; }
    }

    /// <summary>Provides data for database generating or database generated event.</summary>
    public interface IDatabaseEventArgs
    {
        /// <summary>Gets the database that this event argument provides data about.</summary>
        /// <value>The database.</value>
        Database Database { get; }
    }

    /// <summary>Provides data for table generating or table generated event.</summary>
    public interface ITableEventArgs
    {
        /// <summary>Gets the table that this event argument provides data about.</summary>
        /// <value>The table.</value>
        Table Table { get; }
    }

    /// <summary>Provides data for complex type generating or complex type generated event.</summary>
    public interface IComplexTypeTableEventArgs
    {
        /// <summary>Gets the complex type that this event argument provides data about.</summary>
        /// <value>The complex type.</value>
        ComplexTypeTable ComplexTypeTable { get; }
    }

    /// <summary>Provides data for view generating or view generated event.</summary>
    public interface IViewEventArgs
    {
        /// <summary>Gets the view that this event argument provides data about.</summary>
        /// <value>The view.</value>
        View View { get; }
    }

    /// <summary>Provides data for stored procedure generating or stored procedure generated event.</summary>
    public interface IProcedureEventArgs
    {
        /// <summary>Gets the stored procedure that this event argument provides data about.</summary>
        /// <value>The stored procedure.</value>
        Procedure Procedure { get; }
    }

    /// <summary>Provides data for table-valued function generating or table-valued function generated event.</summary>
    public interface IFunctionEventArgs
    {
        /// <summary>Gets the table-valued function that this event argument provides data about.</summary>
        /// <value>The table-valued function.</value>
        Function Function { get; }
    }

    /// <summary>Provides data for TVP generating or TVP generated event.</summary>
    public interface ITVPEventArgs
    {
        /// <summary>Gets the TVP that this event argument provides data about.</summary>
        /// <value>The TVP.</value>
        TVP TVP { get; }
    }

    /// <summary>Provides data about the namespace of the current database object.</summary>
    public interface INamespaceGenerating
    {
        /// <summary>Gets or sets the namespace of the current database object.</summary>
        /// <value>The namespace of the current database object.</value>
        string Namespace { get; set; }
    }

    /// <summary>Provides data about whether to skip the processing of the current database object.</summary>
    public interface ISkipGenerating
    {
        /// <summary>Gets or sets a value indicating whether to skip processing the current database object.</summary>
        /// <value>
        ///   <c>true</c> to skip processing the current database object; otherwise, <c>false</c>.</value>
        bool Skip { get; set; }
    }

    /// <summary>Provides data about whether to stop the generator.</summary>
    public interface IStopGenerating
    {
        /// <summary>Gets or sets a value indicating whether to stop the generator.</summary>
        /// <value>
        ///   <c>true</c> to stop the generator; otherwise, <c>false</c>.</value>
        bool Stop { get; set; }
    }

    #endregion

    #region Server Built

    /// <summary>Provides data for <see cref="IGenerator.ServerBuiltAsync"/> event.</summary>
    public sealed class ServerBuiltAsyncEventArgs : EventArgs, IServerEventArgs
    {
        internal ServerBuiltAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ServerBuilt"/> event.</summary>
    public sealed class ServerBuiltEventArgs : EventArgs, IServerEventArgs, IStopGenerating
    {
        internal ServerBuiltEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Server

    /// <summary>Provides data for <see cref="IGenerator.ServerGeneratingAsync"/> event.</summary>
    public sealed class ServerGeneratingAsyncEventArgs : EventArgs, IGeneratingEventArgs, IServerEventArgs
    {
        internal ServerGeneratingAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ServerGenerating"/> event.</summary>
    public sealed class ServerGeneratingEventArgs : EventArgs, IGeneratingEventArgs, IServerEventArgs, IStopGenerating
    {
        internal ServerGeneratingEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ServerGeneratedAsync"/> event.</summary>
    public sealed class ServerGeneratedAsyncEventArgs : EventArgs, IGeneratedEventArgs, IServerEventArgs
    {
        internal ServerGeneratedAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ServerGenerated"/> event.</summary>
    public sealed class ServerGeneratedEventArgs : EventArgs, IGeneratedEventArgs, IServerEventArgs, IStopGenerating
    {
        internal ServerGeneratedEventArgs(Server server)
        {
            this.Server = server;
        }

        /// <inheritdoc />
        public Server Server { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Database

    /// <summary>Provides data for <see cref="IGenerator.DatabaseGeneratingAsync"/> event.</summary>
    public sealed class DatabaseGeneratingAsyncEventArgs : EventArgs, IGeneratingEventArgs, IDatabaseEventArgs
    {
        internal DatabaseGeneratingAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        /// <inheritdoc />
        public Database Database { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.DatabaseGenerating"/> event.</summary>
    public sealed class DatabaseGeneratingEventArgs : EventArgs, IGeneratingEventArgs, IDatabaseEventArgs, ISkipGenerating, IStopGenerating
    {
        internal DatabaseGeneratingEventArgs(Database database)
        {
            this.Database = database;
        }

        /// <inheritdoc />
        public Database Database { get; private set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.DatabaseGeneratedAsync"/> event.</summary>
    public sealed class DatabaseGeneratedAsyncEventArgs : EventArgs, IGeneratedEventArgs, IDatabaseEventArgs
    {
        internal DatabaseGeneratedAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        /// <inheritdoc />
        public Database Database { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.DatabaseGenerated"/> event.</summary>
    public sealed class DatabaseGeneratedEventArgs : EventArgs, IGeneratedEventArgs, IDatabaseEventArgs, IStopGenerating
    {
        internal DatabaseGeneratedEventArgs(Database database)
        {
            this.Database = database;
        }

        /// <inheritdoc />
        public Database Database { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Table

    /// <summary>Provides data for <see cref="IGenerator.TablesGeneratingAsync"/> event.</summary>
    public sealed class TablesGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal TablesGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.TablesGenerating"/> event.</summary>
    public sealed class TablesGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TablesGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TableGeneratingAsync"/> event.</summary>
    public sealed class TableGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, ITableEventArgs
    {
        internal TableGeneratingAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TableGenerating"/> event.</summary>
    public sealed class TableGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ITableEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal TableGeneratingEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TablePOCOAsync"/> event.</summary>
    public sealed class TablePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, ITableEventArgs
    {
        internal TablePOCOAsyncEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TablePOCO"/> event.</summary>
    public sealed class TablePOCOEventArgs : EventArgs, IPOCOEventArgs, ITableEventArgs, IStopGenerating
    {
        internal TablePOCOEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TableGeneratedAsync"/> event.</summary>
    public sealed class TableGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, ITableEventArgs
    {
        internal TableGeneratedAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TableGenerated"/> event.</summary>
    public sealed class TableGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, ITableEventArgs, IStopGenerating
    {
        internal TableGeneratedEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Table Table { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Table; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Table.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Table.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TablesGeneratedAsync"/> event.</summary>
    public sealed class TablesGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal TablesGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.TablesGenerated"/> event.</summary>
    public sealed class TablesGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal TablesGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Complex Type Table

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablesGeneratingAsync"/> event.</summary>
    public sealed class ComplexTypeTablesGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ComplexTypeTablesGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablesGenerating"/> event.</summary>
    public sealed class ComplexTypeTablesGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ComplexTypeTablesGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTableGeneratingAsync"/> event.</summary>
    public sealed class ComplexTypeTableGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, IComplexTypeTableEventArgs
    {
        internal ComplexTypeTableGeneratingAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTableGenerating"/> event.</summary>
    public sealed class ComplexTypeTableGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, IComplexTypeTableEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal ComplexTypeTableGeneratingEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablePOCOAsync"/> event.</summary>
    public sealed class ComplexTypeTablePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, IComplexTypeTableEventArgs
    {
        internal ComplexTypeTablePOCOAsyncEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablePOCO"/> event.</summary>
    public sealed class ComplexTypeTablePOCOEventArgs : EventArgs, IPOCOEventArgs, IComplexTypeTableEventArgs, IStopGenerating
    {
        internal ComplexTypeTablePOCOEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTableGeneratedAsync"/> event.</summary>
    public sealed class ComplexTypeTableGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, IComplexTypeTableEventArgs
    {
        internal ComplexTypeTableGeneratedAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTableGenerated"/> event.</summary>
    public sealed class ComplexTypeTableGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IComplexTypeTableEventArgs, IStopGenerating
    {
        internal ComplexTypeTableGeneratedEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public ComplexTypeTable ComplexTypeTable { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        /// <inheritdoc />
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.ComplexTypeTable.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablesGeneratedAsync"/> event.</summary>
    public sealed class ComplexTypeTablesGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ComplexTypeTablesGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ComplexTypeTablesGenerated"/> event.</summary>
    public sealed class ComplexTypeTablesGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ComplexTypeTablesGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region View

    /// <summary>Provides data for <see cref="IGenerator.ViewsGeneratingAsync"/> event.</summary>
    public sealed class ViewsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ViewsGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewsGenerating"/> event.</summary>
    public sealed class ViewsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ViewsGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewGeneratingAsync"/> event.</summary>
    public sealed class ViewGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, IViewEventArgs
    {
        internal ViewGeneratingAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewGenerating"/> event.</summary>
    public sealed class ViewGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, IViewEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal ViewGeneratingEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewPOCOAsync"/> event.</summary>
    public sealed class ViewPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, IViewEventArgs
    {
        internal ViewPOCOAsyncEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewPOCO"/> event.</summary>
    public sealed class ViewPOCOEventArgs : EventArgs, IPOCOEventArgs, IViewEventArgs, IStopGenerating
    {
        internal ViewPOCOEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewGeneratedAsync"/> event.</summary>
    public sealed class ViewGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, IViewEventArgs
    {
        internal ViewGeneratedAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewGenerated"/> event.</summary>
    public sealed class ViewGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IViewEventArgs, IStopGenerating
    {
        internal ViewGeneratedEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public View View { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.View; } }
        /// <inheritdoc />
        public string ClassName { get { return this.View.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.View.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewsGeneratedAsync"/> event.</summary>
    public sealed class ViewsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ViewsGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ViewsGenerated"/> event.</summary>
    public sealed class ViewsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ViewsGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Procedure

    /// <summary>Provides data for <see cref="IGenerator.ProceduresGeneratingAsync"/> event.</summary>
    public sealed class ProceduresGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ProceduresGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProceduresGenerating"/> event.</summary>
    public sealed class ProceduresGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ProceduresGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedureGeneratingAsync"/> event.</summary>
    public sealed class ProcedureGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, IProcedureEventArgs
    {
        internal ProcedureGeneratingAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedureGenerating"/> event.</summary>
    public sealed class ProcedureGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, IProcedureEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal ProcedureGeneratingEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedurePOCOAsync"/> event.</summary>
    public sealed class ProcedurePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, IProcedureEventArgs
    {
        internal ProcedurePOCOAsyncEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedurePOCO"/> event.</summary>
    public sealed class ProcedurePOCOEventArgs : EventArgs, IPOCOEventArgs, IProcedureEventArgs, IStopGenerating
    {
        internal ProcedurePOCOEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedureGeneratedAsync"/> event.</summary>
    public sealed class ProcedureGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, IProcedureEventArgs
    {
        internal ProcedureGeneratedAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProcedureGenerated" /> event.</summary>
    public sealed class ProcedureGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IProcedureEventArgs, IStopGenerating
    {
        internal ProcedureGeneratedEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Procedure Procedure { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Procedure; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Procedure.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Procedure.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProceduresGeneratedAsync"/> event.</summary>
    public sealed class ProceduresGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ProceduresGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.ProceduresGenerated"/> event.</summary>
    public sealed class ProceduresGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ProceduresGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region Function

    /// <summary>Provides data for <see cref="IGenerator.FunctionsGeneratingAsync"/> event.</summary>
    public sealed class FunctionsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal FunctionsGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionsGenerating"/> event.</summary>
    public sealed class FunctionsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal FunctionsGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionGeneratingAsync"/> event.</summary>
    public sealed class FunctionGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, IFunctionEventArgs
    {
        internal FunctionGeneratingAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionGenerating"/> event.</summary>
    public sealed class FunctionGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, IFunctionEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal FunctionGeneratingEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionPOCOAsync"/> event.</summary>
    public sealed class FunctionPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, IFunctionEventArgs
    {
        internal FunctionPOCOAsyncEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionPOCO"/> event.</summary>
    public sealed class FunctionPOCOEventArgs : EventArgs, IPOCOEventArgs, IFunctionEventArgs, IStopGenerating
    {
        internal FunctionPOCOEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionGeneratedAsync"/> event.</summary>
    public sealed class FunctionGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, IFunctionEventArgs
    {
        internal FunctionGeneratedAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionGenerated"/> event.</summary>
    public sealed class FunctionGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IFunctionEventArgs, IStopGenerating
    {
        internal FunctionGeneratedEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public Function Function { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.Function; } }
        /// <inheritdoc />
        public string ClassName { get { return this.Function.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.Function.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionsGeneratedAsync"/> event.</summary>
    public sealed class FunctionsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal FunctionsGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.FunctionsGenerated"/> event.</summary>
    public sealed class FunctionsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal FunctionsGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion

    #region TVP

    /// <summary>Provides data for <see cref="IGenerator.TVPsGeneratingAsync"/> event.</summary>
    public sealed class TVPsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal TVPsGeneratingAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPsGenerating"/> event.</summary>
    public sealed class TVPsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TVPsGeneratingEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPGeneratingAsync"/> event.</summary>
    public sealed class TVPGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs, ITVPEventArgs
    {
        internal TVPGeneratingAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPGenerating"/> event.</summary>
    public sealed class TVPGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ITVPEventArgs, INamespaceGenerating, ISkipGenerating, IStopGenerating
    {
        internal TVPGeneratingEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc cref="INamespaceGenerating.Namespace" />
        public string Namespace { get; set; }
        /// <inheritdoc />
        public bool Skip { get; set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPPOCOAsync"/> event.</summary>
    public sealed class TVPPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs, ITVPEventArgs
    {
        internal TVPPOCOAsyncEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPPOCO"/> event.</summary>
    public sealed class TVPPOCOEventArgs : EventArgs, IPOCOEventArgs, ITVPEventArgs, IStopGenerating
    {
        internal TVPPOCOEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc />
        public string POCO { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPGeneratedAsync"/> event.</summary>
    public sealed class TVPGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs, ITVPEventArgs
    {
        internal TVPGeneratedAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPGenerated"/> event.</summary>
    public sealed class TVPGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, ITVPEventArgs, IStopGenerating
    {
        internal TVPGeneratedEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        /// <inheritdoc />
        public TVP TVP { get; private set; }
        /// <inheritdoc />
        public IDbObject DbObject { get { return this.TVP; } }
        /// <inheritdoc />
        public string ClassName { get { return this.TVP.ClassName; } }
        /// <inheritdoc />
        public string Error { get { return this.TVP.Error; } }
        /// <inheritdoc />
        public string Namespace { get; private set; }
        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPsGeneratedAsync"/> event.</summary>
    public sealed class TVPsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal TVPsGeneratedAsyncEventArgs()
        {
        }
    }

    /// <summary>Provides data for <see cref="IGenerator.TVPsGenerated" /> event.</summary>
    public sealed class TVPsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal TVPsGeneratedEventArgs()
        {
        }

        /// <inheritdoc />
        public bool Stop { get; set; }
    }

    #endregion
}
