using System;
using POCOGenerator.Objects;

namespace POCOGenerator
{
    #region Interfaces

    public interface IGeneratingEventArgs
    {
    }

    public interface IObjectsGeneratingEventArgs : IGeneratingEventArgs
    {
    }

    public interface IObjectGeneratingEventArgs : IGeneratingEventArgs
    {
        IDbObject DbObject { get; }
        string ClassName { get; }
        string Error { get; }
        string Namespace { get; }
    }

    public interface IPOCOEventArgs
    {
        IDbObject DbObject { get; }
        string ClassName { get; }
        string Error { get; }
        string POCO { get; }
    }

    public interface IGeneratedEventArgs
    {
    }

    public interface IObjectGeneratedEventArgs : IGeneratedEventArgs
    {
        IDbObject DbObject { get; }
        string ClassName { get; }
        string Error { get; }
        string Namespace { get; }
    }

    public interface IObjectsGeneratedEventArgs : IGeneratedEventArgs
    {
    }

    public interface ISkipGenerating
    {
        bool Skip { get; set; }
    }

    public interface IStopGenerating
    {
        bool Stop { get; set; }
    }

    #endregion

    #region Server Built

    public sealed class ServerBuiltAsyncEventArgs : EventArgs
    {
        internal ServerBuiltAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerBuiltEventArgs : EventArgs, IStopGenerating
    {
        internal ServerBuiltEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
        public bool Stop { get; set; }
    }

    #endregion

    #region Server

    public sealed class ServerGeneratingAsyncEventArgs : EventArgs, IGeneratingEventArgs
    {
        internal ServerGeneratingAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerGeneratingEventArgs : EventArgs, IGeneratingEventArgs, IStopGenerating
    {
        internal ServerGeneratingEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ServerGeneratedAsyncEventArgs : EventArgs, IGeneratedEventArgs
    {
        internal ServerGeneratedAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerGeneratedEventArgs : EventArgs, IGeneratedEventArgs, IStopGenerating
    {
        internal ServerGeneratedEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
        public bool Stop { get; set; }
    }

    #endregion

    #region Database

    public sealed class DatabaseGeneratingAsyncEventArgs : EventArgs, IGeneratingEventArgs
    {
        internal DatabaseGeneratingAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
    }

    public sealed class DatabaseGeneratingEventArgs : EventArgs, IGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal DatabaseGeneratingEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class DatabaseGeneratedAsyncEventArgs : EventArgs, IGeneratedEventArgs
    {
        internal DatabaseGeneratedAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
    }

    public sealed class DatabaseGeneratedEventArgs : EventArgs, IGeneratedEventArgs, IStopGenerating
    {
        internal DatabaseGeneratedEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
        public bool Stop { get; set; }
    }

    #endregion

    #region Table

    public sealed class TablesGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal TablesGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal TableGeneratingAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TableGeneratingEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TablePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal TablePOCOAsyncEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class TablePOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal TablePOCOEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal TableGeneratedAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal TableGeneratedEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public IDbObject DbObject { get { return this.Table; } }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TablesGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal TablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal TablesGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Complex Type Table

    public sealed class ComplexTypeTablesGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ComplexTypeTablesGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ComplexTypeTablesGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ComplexTypeTablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal ComplexTypeTableGeneratingAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ComplexTypeTableGeneratingEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal ComplexTypeTablePOCOAsyncEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ComplexTypeTablePOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal ComplexTypeTablePOCOEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal ComplexTypeTableGeneratedAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal ComplexTypeTableGeneratedEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public IDbObject DbObject { get { return this.ComplexTypeTable; } }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablesGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ComplexTypeTablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ComplexTypeTablesGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ComplexTypeTablesGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region View

    public sealed class ViewsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ViewsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ViewsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ViewsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal ViewGeneratingAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ViewGeneratingEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal ViewPOCOAsyncEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ViewPOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal ViewPOCOEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal ViewGeneratedAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal ViewGeneratedEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public IDbObject DbObject { get { return this.View; } }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ViewsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ViewsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ViewsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Procedure

    public sealed class ProceduresGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal ProceduresGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ProceduresGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ProceduresGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal ProcedureGeneratingAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal ProcedureGeneratingEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedurePOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal ProcedurePOCOAsyncEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ProcedurePOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal ProcedurePOCOEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal ProcedureGeneratedAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal ProcedureGeneratedEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public IDbObject DbObject { get { return this.Procedure; } }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProceduresGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal ProceduresGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ProceduresGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal ProceduresGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Function

    public sealed class FunctionsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal FunctionsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class FunctionsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal FunctionsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal FunctionGeneratingAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal FunctionGeneratingEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal FunctionPOCOAsyncEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class FunctionPOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal FunctionPOCOEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal FunctionGeneratedAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal FunctionGeneratedEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public IDbObject DbObject { get { return this.Function; } }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal FunctionsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class FunctionsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal FunctionsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region TVP

    public sealed class TVPsGeneratingAsyncEventArgs : EventArgs, IObjectsGeneratingEventArgs
    {
        internal TVPsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class TVPsGeneratingEventArgs : EventArgs, IObjectsGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TVPsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratingAsyncEventArgs : EventArgs, IObjectGeneratingEventArgs
    {
        internal TVPGeneratingAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratingEventArgs : EventArgs, IObjectGeneratingEventArgs, ISkipGenerating, IStopGenerating
    {
        internal TVPGeneratingEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPPOCOAsyncEventArgs : EventArgs, IPOCOEventArgs
    {
        internal TVPPOCOAsyncEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class TVPPOCOEventArgs : EventArgs, IPOCOEventArgs, IStopGenerating
    {
        internal TVPPOCOEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratedAsyncEventArgs : EventArgs, IObjectGeneratedEventArgs
    {
        internal TVPGeneratedAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratedEventArgs : EventArgs, IObjectGeneratedEventArgs, IStopGenerating
    {
        internal TVPGeneratedEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public IDbObject DbObject { get { return this.TVP; } }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPsGeneratedAsyncEventArgs : EventArgs, IObjectsGeneratedEventArgs
    {
        internal TVPsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TVPsGeneratedEventArgs : EventArgs, IObjectsGeneratedEventArgs, IStopGenerating
    {
        internal TVPsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion
}
