using System;
using POCOGenerator.DbObjects;

namespace POCOGenerator.POCOIterators
{
    #region Interfaces

    public interface IStopGenerating
    {
        bool Stop { get; }
    }

    #endregion

    #region Server

    public sealed class ServerGeneratingAsyncEventArgs : EventArgs
    {
        internal ServerGeneratingAsyncEventArgs(IServer server)
        {
            this.Server = server;
        }

        public IServer Server { get; private set; }
    }

    public sealed class ServerGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ServerGeneratingEventArgs(IServer server)
        {
            this.Server = server;
        }

        public IServer Server { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ServerGeneratedAsyncEventArgs : EventArgs
    {
        internal ServerGeneratedAsyncEventArgs(IServer server)
        {
            this.Server = server;
        }

        public IServer Server { get; private set; }
    }

    public sealed class ServerGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ServerGeneratedEventArgs(IServer server)
        {
            this.Server = server;
        }

        public IServer Server { get; private set; }
        public bool Stop { get; set; }
    }

    #endregion

    #region Database

    public sealed class DatabaseGeneratingAsyncEventArgs : EventArgs
    {
        internal DatabaseGeneratingAsyncEventArgs(IDatabase database)
        {
            this.Database = database;
        }

        public IDatabase Database { get; private set; }
    }

    public sealed class DatabaseGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal DatabaseGeneratingEventArgs(IDatabase database)
        {
            this.Database = database;
        }

        public IDatabase Database { get; private set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class DatabaseGeneratedAsyncEventArgs : EventArgs
    {
        internal DatabaseGeneratedAsyncEventArgs(IDatabase database)
        {
            this.Database = database;
        }

        public IDatabase Database { get; private set; }
    }

    public sealed class DatabaseGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal DatabaseGeneratedEventArgs(IDatabase database)
        {
            this.Database = database;
        }

        public IDatabase Database { get; private set; }
        public bool Stop { get; set; }
    }

    #endregion

    #region Table

    public sealed class TablesGeneratingAsyncEventArgs : EventArgs
    {
        internal TablesGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal TablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratingAsyncEventArgs : EventArgs
    {
        internal TableGeneratingAsyncEventArgs(ITable table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public ITable Table { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal TableGeneratingEventArgs(ITable table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public ITable Table { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TablePOCOAsyncEventArgs : EventArgs
    {
        internal TablePOCOAsyncEventArgs(ITable table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public ITable Table { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class TablePOCOEventArgs : EventArgs, IStopGenerating
    {
        internal TablePOCOEventArgs(ITable table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public ITable Table { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratedAsyncEventArgs : EventArgs
    {
        internal TableGeneratedAsyncEventArgs(ITable table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public ITable Table { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal TableGeneratedEventArgs(ITable table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public ITable Table { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TablesGeneratedAsyncEventArgs : EventArgs
    {
        internal TablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal TablesGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Complex Type Table

    public sealed class ComplexTypeTablesGeneratingAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTablesGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ComplexTypeTablesGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ComplexTypeTablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratingAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratingAsyncEventArgs(IComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ComplexTypeTableGeneratingEventArgs(IComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablePOCOAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTablePOCOAsyncEventArgs(IComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class ComplexTypeTablePOCOEventArgs : EventArgs, IStopGenerating
    {
        internal ComplexTypeTablePOCOEventArgs(IComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratedAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratedAsyncEventArgs(IComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ComplexTypeTableGeneratedEventArgs(IComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public IComplexTypeTable ComplexTypeTable { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablesGeneratedAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ComplexTypeTablesGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ComplexTypeTablesGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region View

    public sealed class ViewsGeneratingAsyncEventArgs : EventArgs
    {
        internal ViewsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ViewsGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ViewsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratingAsyncEventArgs : EventArgs
    {
        internal ViewGeneratingAsyncEventArgs(IView view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public IView View { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ViewGeneratingEventArgs(IView view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public IView View { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewPOCOAsyncEventArgs : EventArgs
    {
        internal ViewPOCOAsyncEventArgs(IView view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public IView View { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class ViewPOCOEventArgs : EventArgs, IStopGenerating
    {
        internal ViewPOCOEventArgs(IView view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public IView View { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratedAsyncEventArgs : EventArgs
    {
        internal ViewGeneratedAsyncEventArgs(IView view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public IView View { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ViewGeneratedEventArgs(IView view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public IView View { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewsGeneratedAsyncEventArgs : EventArgs
    {
        internal ViewsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ViewsGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ViewsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Procedure

    public sealed class ProceduresGeneratingAsyncEventArgs : EventArgs
    {
        internal ProceduresGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class ProceduresGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ProceduresGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratingAsyncEventArgs : EventArgs
    {
        internal ProcedureGeneratingAsyncEventArgs(IProcedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public IProcedure Procedure { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal ProcedureGeneratingEventArgs(IProcedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public IProcedure Procedure { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedurePOCOAsyncEventArgs : EventArgs
    {
        internal ProcedurePOCOAsyncEventArgs(IProcedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public IProcedure Procedure { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class ProcedurePOCOEventArgs : EventArgs, IStopGenerating
    {
        internal ProcedurePOCOEventArgs(IProcedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public IProcedure Procedure { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratedAsyncEventArgs : EventArgs
    {
        internal ProcedureGeneratedAsyncEventArgs(IProcedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public IProcedure Procedure { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ProcedureGeneratedEventArgs(IProcedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public IProcedure Procedure { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProceduresGeneratedAsyncEventArgs : EventArgs
    {
        internal ProceduresGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ProceduresGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal ProceduresGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region Function

    public sealed class FunctionsGeneratingAsyncEventArgs : EventArgs
    {
        internal FunctionsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class FunctionsGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal FunctionsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratingAsyncEventArgs : EventArgs
    {
        internal FunctionGeneratingAsyncEventArgs(IFunction function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public IFunction Function { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal FunctionGeneratingEventArgs(IFunction function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public IFunction Function { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionPOCOAsyncEventArgs : EventArgs
    {
        internal FunctionPOCOAsyncEventArgs(IFunction function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public IFunction Function { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class FunctionPOCOEventArgs : EventArgs, IStopGenerating
    {
        internal FunctionPOCOEventArgs(IFunction function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public IFunction Function { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratedAsyncEventArgs : EventArgs
    {
        internal FunctionGeneratedAsyncEventArgs(IFunction function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public IFunction Function { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal FunctionGeneratedEventArgs(IFunction function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public IFunction Function { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionsGeneratedAsyncEventArgs : EventArgs
    {
        internal FunctionsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class FunctionsGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal FunctionsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion

    #region TVP

    public sealed class TVPsGeneratingAsyncEventArgs : EventArgs
    {
        internal TVPsGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class TVPsGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal TVPsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratingAsyncEventArgs : EventArgs
    {
        internal TVPGeneratingAsyncEventArgs(ITVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public ITVP TVP { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratingEventArgs : EventArgs, IStopGenerating
    {
        internal TVPGeneratingEventArgs(ITVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public ITVP TVP { get; private set; }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPPOCOAsyncEventArgs : EventArgs
    {
        internal TVPPOCOAsyncEventArgs(ITVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public ITVP TVP { get; private set; }
        public string POCO { get; private set; }
    }

    public sealed class TVPPOCOEventArgs : EventArgs, IStopGenerating
    {
        internal TVPPOCOEventArgs(ITVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public ITVP TVP { get; private set; }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratedAsyncEventArgs : EventArgs
    {
        internal TVPGeneratedAsyncEventArgs(ITVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public ITVP TVP { get; private set; }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal TVPGeneratedEventArgs(ITVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public ITVP TVP { get; private set; }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPsGeneratedAsyncEventArgs : EventArgs
    {
        internal TVPsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TVPsGeneratedEventArgs : EventArgs, IStopGenerating
    {
        internal TVPsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion
}
