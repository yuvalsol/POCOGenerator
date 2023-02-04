using System;
using POCOGenerator.Objects;

namespace POCOGenerator
{
    #region Server

    public sealed class ServerBuiltAsyncEventArgs : EventArgs
    {
        internal ServerBuiltAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerBuiltEventArgs : EventArgs
    {
        internal ServerBuiltEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ServerGeneratingAsyncEventArgs : EventArgs
    {
        internal ServerGeneratingAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerGeneratingEventArgs : EventArgs
    {
        internal ServerGeneratingEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ServerGeneratedAsyncEventArgs : EventArgs
    {
        internal ServerGeneratedAsyncEventArgs(Server server)
        {
            this.Server = server;
        }

        public Server Server { get; private set; }
    }

    public sealed class ServerGeneratedEventArgs : EventArgs
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

    public sealed class DatabaseGeneratingAsyncEventArgs : EventArgs
    {
        internal DatabaseGeneratingAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
    }

    public sealed class DatabaseGeneratingEventArgs : EventArgs
    {
        internal DatabaseGeneratingEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class DatabaseGeneratedAsyncEventArgs : EventArgs
    {
        internal DatabaseGeneratedAsyncEventArgs(Database database)
        {
            this.Database = database;
        }

        public Database Database { get; private set; }
    }

    public sealed class DatabaseGeneratedEventArgs : EventArgs
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

    public sealed class TablesGeneratingAsyncEventArgs : EventArgs
    {
        internal TablesGeneratingAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratingEventArgs : EventArgs
    {
        internal TablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratingAsyncEventArgs : EventArgs
    {
        internal TableGeneratingAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratingEventArgs : EventArgs
    {
        internal TableGeneratingEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TablePOCOAsyncEventArgs : EventArgs
    {
        internal TablePOCOAsyncEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class TablePOCOEventArgs : EventArgs
    {
        internal TablePOCOEventArgs(Table table, string poco)
        {
            this.Table = table;
            this.POCO = poco;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TableGeneratedAsyncEventArgs : EventArgs
    {
        internal TableGeneratedAsyncEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TableGeneratedEventArgs : EventArgs
    {
        internal TableGeneratedEventArgs(Table table, string @namespace)
        {
            this.Table = table;
            this.Namespace = @namespace;
        }

        public Table Table { get; private set; }
        public string ClassName { get { return this.Table.ClassName; } }
        public string Error { get { return this.Table.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TablesGeneratedAsyncEventArgs : EventArgs
    {
        internal TablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TablesGeneratedEventArgs : EventArgs
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

    public sealed class ComplexTypeTablesGeneratingEventArgs : EventArgs
    {
        internal ComplexTypeTablesGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratingAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratingAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratingEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratingEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablePOCOAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTablePOCOAsyncEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ComplexTypeTablePOCOEventArgs : EventArgs
    {
        internal ComplexTypeTablePOCOEventArgs(ComplexTypeTable complexTypeTable, string poco)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.POCO = poco;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTableGeneratedAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratedAsyncEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ComplexTypeTableGeneratedEventArgs : EventArgs
    {
        internal ComplexTypeTableGeneratedEventArgs(ComplexTypeTable complexTypeTable, string @namespace)
        {
            this.ComplexTypeTable = complexTypeTable;
            this.Namespace = @namespace;
        }

        public ComplexTypeTable ComplexTypeTable { get; private set; }
        public string ClassName { get { return this.ComplexTypeTable.ClassName; } }
        public string Error { get { return this.ComplexTypeTable.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ComplexTypeTablesGeneratedAsyncEventArgs : EventArgs
    {
        internal ComplexTypeTablesGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ComplexTypeTablesGeneratedEventArgs : EventArgs
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

    public sealed class ViewsGeneratingEventArgs : EventArgs
    {
        internal ViewsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratingAsyncEventArgs : EventArgs
    {
        internal ViewGeneratingAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratingEventArgs : EventArgs
    {
        internal ViewGeneratingEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewPOCOAsyncEventArgs : EventArgs
    {
        internal ViewPOCOAsyncEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ViewPOCOEventArgs : EventArgs
    {
        internal ViewPOCOEventArgs(View view, string poco)
        {
            this.View = view;
            this.POCO = poco;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewGeneratedAsyncEventArgs : EventArgs
    {
        internal ViewGeneratedAsyncEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ViewGeneratedEventArgs : EventArgs
    {
        internal ViewGeneratedEventArgs(View view, string @namespace)
        {
            this.View = view;
            this.Namespace = @namespace;
        }

        public View View { get; private set; }
        public string ClassName { get { return this.View.ClassName; } }
        public string Error { get { return this.View.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ViewsGeneratedAsyncEventArgs : EventArgs
    {
        internal ViewsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ViewsGeneratedEventArgs : EventArgs
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

    public sealed class ProceduresGeneratingEventArgs : EventArgs
    {
        internal ProceduresGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratingAsyncEventArgs : EventArgs
    {
        internal ProcedureGeneratingAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratingEventArgs : EventArgs
    {
        internal ProcedureGeneratingEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedurePOCOAsyncEventArgs : EventArgs
    {
        internal ProcedurePOCOAsyncEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class ProcedurePOCOEventArgs : EventArgs
    {
        internal ProcedurePOCOEventArgs(Procedure procedure, string poco)
        {
            this.Procedure = procedure;
            this.POCO = poco;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProcedureGeneratedAsyncEventArgs : EventArgs
    {
        internal ProcedureGeneratedAsyncEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class ProcedureGeneratedEventArgs : EventArgs
    {
        internal ProcedureGeneratedEventArgs(Procedure procedure, string @namespace)
        {
            this.Procedure = procedure;
            this.Namespace = @namespace;
        }

        public Procedure Procedure { get; private set; }
        public string ClassName { get { return this.Procedure.ClassName; } }
        public string Error { get { return this.Procedure.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class ProceduresGeneratedAsyncEventArgs : EventArgs
    {
        internal ProceduresGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class ProceduresGeneratedEventArgs : EventArgs
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

    public sealed class FunctionsGeneratingEventArgs : EventArgs
    {
        internal FunctionsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratingAsyncEventArgs : EventArgs
    {
        internal FunctionGeneratingAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratingEventArgs : EventArgs
    {
        internal FunctionGeneratingEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionPOCOAsyncEventArgs : EventArgs
    {
        internal FunctionPOCOAsyncEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class FunctionPOCOEventArgs : EventArgs
    {
        internal FunctionPOCOEventArgs(Function function, string poco)
        {
            this.Function = function;
            this.POCO = poco;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionGeneratedAsyncEventArgs : EventArgs
    {
        internal FunctionGeneratedAsyncEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class FunctionGeneratedEventArgs : EventArgs
    {
        internal FunctionGeneratedEventArgs(Function function, string @namespace)
        {
            this.Function = function;
            this.Namespace = @namespace;
        }

        public Function Function { get; private set; }
        public string ClassName { get { return this.Function.ClassName; } }
        public string Error { get { return this.Function.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class FunctionsGeneratedAsyncEventArgs : EventArgs
    {
        internal FunctionsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class FunctionsGeneratedEventArgs : EventArgs
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

    public sealed class TVPsGeneratingEventArgs : EventArgs
    {
        internal TVPsGeneratingEventArgs()
        {
        }

        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratingAsyncEventArgs : EventArgs
    {
        internal TVPGeneratingAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratingEventArgs : EventArgs
    {
        internal TVPGeneratingEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; set; }
        public bool Skip { get; set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPPOCOAsyncEventArgs : EventArgs
    {
        internal TVPPOCOAsyncEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string POCO { get; private set; }
    }

    public sealed class TVPPOCOEventArgs : EventArgs
    {
        internal TVPPOCOEventArgs(TVP tvp, string poco)
        {
            this.TVP = tvp;
            this.POCO = poco;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string POCO { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPGeneratedAsyncEventArgs : EventArgs
    {
        internal TVPGeneratedAsyncEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
    }

    public sealed class TVPGeneratedEventArgs : EventArgs
    {
        internal TVPGeneratedEventArgs(TVP tvp, string @namespace)
        {
            this.TVP = tvp;
            this.Namespace = @namespace;
        }

        public TVP TVP { get; private set; }
        public string ClassName { get { return this.TVP.ClassName; } }
        public string Error { get { return this.TVP.Error; } }
        public string Namespace { get; private set; }
        public bool Stop { get; set; }
    }

    public sealed class TVPsGeneratedAsyncEventArgs : EventArgs
    {
        internal TVPsGeneratedAsyncEventArgs()
        {
        }
    }

    public sealed class TVPsGeneratedEventArgs : EventArgs
    {
        internal TVPsGeneratedEventArgs()
        {
        }

        public bool Stop { get; set; }
    }

    #endregion
}
