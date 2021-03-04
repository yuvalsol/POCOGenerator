using System;
using System.Collections.Generic;
using POCOGenerator.DbObjects;

namespace POCOGenerator.POCOIterators
{
    public interface IDbIterator
    {
        void Iterate(IEnumerable<IDbObjectTraverse> dbObjects);

        // in order of execution
        event EventHandler<ServerGeneratingAsyncEventArgs> ServerGeneratingAsync;
        event EventHandler<ServerGeneratingEventArgs> ServerGenerating;
        event EventHandler<DatabaseGeneratingAsyncEventArgs> DatabaseGeneratingAsync;
        event EventHandler<DatabaseGeneratingEventArgs> DatabaseGenerating;
        event EventHandler<TablesGeneratingAsyncEventArgs> TablesGeneratingAsync;
        event EventHandler<TablesGeneratingEventArgs> TablesGenerating;
        event EventHandler<TableGeneratingAsyncEventArgs> TableGeneratingAsync;
        event EventHandler<TableGeneratingEventArgs> TableGenerating;
        event EventHandler<TablePOCOAsyncEventArgs> TablePOCOAsync;
        event EventHandler<TablePOCOEventArgs> TablePOCO;
        event EventHandler<TableGeneratedAsyncEventArgs> TableGeneratedAsync;
        event EventHandler<TableGeneratedEventArgs> TableGenerated;
        event EventHandler<TablesGeneratedAsyncEventArgs> TablesGeneratedAsync;
        event EventHandler<TablesGeneratedEventArgs> TablesGenerated;
        event EventHandler<ViewsGeneratingAsyncEventArgs> ViewsGeneratingAsync;
        event EventHandler<ViewsGeneratingEventArgs> ViewsGenerating;
        event EventHandler<ViewGeneratingAsyncEventArgs> ViewGeneratingAsync;
        event EventHandler<ViewGeneratingEventArgs> ViewGenerating;
        event EventHandler<ViewPOCOAsyncEventArgs> ViewPOCOAsync;
        event EventHandler<ViewPOCOEventArgs> ViewPOCO;
        event EventHandler<ViewGeneratedAsyncEventArgs> ViewGeneratedAsync;
        event EventHandler<ViewGeneratedEventArgs> ViewGenerated;
        event EventHandler<ViewsGeneratedAsyncEventArgs> ViewsGeneratedAsync;
        event EventHandler<ViewsGeneratedEventArgs> ViewsGenerated;
        event EventHandler<ProceduresGeneratingAsyncEventArgs> ProceduresGeneratingAsync;
        event EventHandler<ProceduresGeneratingEventArgs> ProceduresGenerating;
        event EventHandler<ProcedureGeneratingAsyncEventArgs> ProcedureGeneratingAsync;
        event EventHandler<ProcedureGeneratingEventArgs> ProcedureGenerating;
        event EventHandler<ProcedurePOCOAsyncEventArgs> ProcedurePOCOAsync;
        event EventHandler<ProcedurePOCOEventArgs> ProcedurePOCO;
        event EventHandler<ProcedureGeneratedAsyncEventArgs> ProcedureGeneratedAsync;
        event EventHandler<ProcedureGeneratedEventArgs> ProcedureGenerated;
        event EventHandler<ProceduresGeneratedAsyncEventArgs> ProceduresGeneratedAsync;
        event EventHandler<ProceduresGeneratedEventArgs> ProceduresGenerated;
        event EventHandler<FunctionsGeneratingAsyncEventArgs> FunctionsGeneratingAsync;
        event EventHandler<FunctionsGeneratingEventArgs> FunctionsGenerating;
        event EventHandler<FunctionGeneratingAsyncEventArgs> FunctionGeneratingAsync;
        event EventHandler<FunctionGeneratingEventArgs> FunctionGenerating;
        event EventHandler<FunctionPOCOAsyncEventArgs> FunctionPOCOAsync;
        event EventHandler<FunctionPOCOEventArgs> FunctionPOCO;
        event EventHandler<FunctionGeneratedAsyncEventArgs> FunctionGeneratedAsync;
        event EventHandler<FunctionGeneratedEventArgs> FunctionGenerated;
        event EventHandler<FunctionsGeneratedAsyncEventArgs> FunctionsGeneratedAsync;
        event EventHandler<FunctionsGeneratedEventArgs> FunctionsGenerated;
        event EventHandler<TVPsGeneratingAsyncEventArgs> TVPsGeneratingAsync;
        event EventHandler<TVPsGeneratingEventArgs> TVPsGenerating;
        event EventHandler<TVPGeneratingAsyncEventArgs> TVPGeneratingAsync;
        event EventHandler<TVPGeneratingEventArgs> TVPGenerating;
        event EventHandler<TVPPOCOAsyncEventArgs> TVPPOCOAsync;
        event EventHandler<TVPPOCOEventArgs> TVPPOCO;
        event EventHandler<TVPGeneratedAsyncEventArgs> TVPGeneratedAsync;
        event EventHandler<TVPGeneratedEventArgs> TVPGenerated;
        event EventHandler<TVPsGeneratedAsyncEventArgs> TVPsGeneratedAsync;
        event EventHandler<TVPsGeneratedEventArgs> TVPsGenerated;
        event EventHandler<DatabaseGeneratedAsyncEventArgs> DatabaseGeneratedAsync;
        event EventHandler<DatabaseGeneratedEventArgs> DatabaseGenerated;
        event EventHandler<ServerGeneratedAsyncEventArgs> ServerGeneratedAsync;
        event EventHandler<ServerGeneratedEventArgs> ServerGenerated;
    }
}
