using System;

namespace POCOGenerator
{
    /// <summary>Represents the POCO Generator.</summary>
    public interface IGenerator
    {
        /// <summary>Gets the settings that determine the behavior of the generator.</summary>
        /// <value>The generator settings.</value>
        Settings Settings { get; }

        /// <summary>Gets the RDBMS support that provides information about the RDBMS capabilities.</summary>
        /// <value>The RDBMS support.</value>
        Support Support { get; }

        /// <summary>Gets the unexpected error that the generator encountered.
        /// <para>Returns <see langword="null" /> if the generator didn't encountered any unexpected error.</para></summary>
        /// <value>The unexpected error that the generator encountered.</value>
        Exception Error { get; }

        /// <summary>Gets the disclaimer message about POCO Generator.</summary>
        /// <value>The disclaimer message about POCO Generator.</value>
        string Disclaimer { get; }

        /// <summary>Connects to the RDBMS server and generates POCO classes from selected database objects.</summary>
        /// <returns>The result code that indicates whether the generator finished running successfully or not.</returns>
        GeneratorResults Generate();

        /// <summary>Generates POCO classes from previously built class objects without connecting to the RDBMS server again.
        /// <para>Falls back to <see cref="Generate"/> if no class objects were built previously.</para></summary>
        /// <returns>The result code that indicates whether the generator finished running successfully or not.</returns>
        GeneratorResults GeneratePOCOs();

        // in order of execution
        /// <summary>Occurs asynchronously when the server has been built.</summary>
        event EventHandler<ServerBuiltAsyncEventArgs> ServerBuiltAsync;
        /// <summary>Occurs when the server has been built.</summary>
        event EventHandler<ServerBuiltEventArgs> ServerBuilt;
        /// <summary>Occurs asynchronously when the server is about to be generated.</summary>
        event EventHandler<ServerGeneratingAsyncEventArgs> ServerGeneratingAsync;
        /// <summary>Occurs when the server is about to be generated.</summary>
        event EventHandler<ServerGeneratingEventArgs> ServerGenerating;
        /// <summary>Occurs asynchronously when the database is about to be generated.</summary>
        event EventHandler<DatabaseGeneratingAsyncEventArgs> DatabaseGeneratingAsync;
        /// <summary>Occurs when the database is about to be generated.</summary>
        event EventHandler<DatabaseGeneratingEventArgs> DatabaseGenerating;
        /// <summary>Occurs asynchronously when the tables are about to be generated.</summary>
        event EventHandler<TablesGeneratingAsyncEventArgs> TablesGeneratingAsync;
        /// <summary>Occurs when the tables are about to be generated.</summary>
        event EventHandler<TablesGeneratingEventArgs> TablesGenerating;
        /// <summary>Occurs asynchronously when the table is about to be generated.</summary>
        event EventHandler<TableGeneratingAsyncEventArgs> TableGeneratingAsync;
        /// <summary>Occurs when the table is about to be generated.</summary>
        event EventHandler<TableGeneratingEventArgs> TableGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the table has been generated.</summary>
        event EventHandler<TablePOCOAsyncEventArgs> TablePOCOAsync;
        /// <summary>Occurs when the POCO class text of the table has been generated.</summary>
        event EventHandler<TablePOCOEventArgs> TablePOCO;
        /// <summary>Occurs asynchronously when the table has been generated.</summary>
        event EventHandler<TableGeneratedAsyncEventArgs> TableGeneratedAsync;
        /// <summary>Occurs when the table has been generated.</summary>
        event EventHandler<TableGeneratedEventArgs> TableGenerated;
        /// <summary>Occurs asynchronously when the tables have been generated.</summary>
        event EventHandler<TablesGeneratedAsyncEventArgs> TablesGeneratedAsync;
        /// <summary>Occurs when the tables have been generated.</summary>
        event EventHandler<TablesGeneratedEventArgs> TablesGenerated;
        /// <summary>Occurs asynchronously when the complex types are about to be generated.</summary>
        event EventHandler<ComplexTypeTablesGeneratingAsyncEventArgs> ComplexTypeTablesGeneratingAsync;
        /// <summary>Occurs when the complex types are about to be generated.</summary>
        event EventHandler<ComplexTypeTablesGeneratingEventArgs> ComplexTypeTablesGenerating;
        /// <summary>Occurs asynchronously when the complex type is about to be generated.</summary>
        event EventHandler<ComplexTypeTableGeneratingAsyncEventArgs> ComplexTypeTableGeneratingAsync;
        /// <summary>Occurs when the complex type is about to be generated.</summary>
        event EventHandler<ComplexTypeTableGeneratingEventArgs> ComplexTypeTableGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the complex type has been generated.</summary>
        event EventHandler<ComplexTypeTablePOCOAsyncEventArgs> ComplexTypeTablePOCOAsync;
        /// <summary>Occurs when the POCO class text of the complex type has been generated.</summary>
        event EventHandler<ComplexTypeTablePOCOEventArgs> ComplexTypeTablePOCO;
        /// <summary>Occurs asynchronously when the complex type has been generated.</summary>
        event EventHandler<ComplexTypeTableGeneratedAsyncEventArgs> ComplexTypeTableGeneratedAsync;
        /// <summary>Occurs when the complex type has been generated.</summary>
        event EventHandler<ComplexTypeTableGeneratedEventArgs> ComplexTypeTableGenerated;
        /// <summary>Occurs asynchronously when the complex types have been generated.</summary>
        event EventHandler<ComplexTypeTablesGeneratedAsyncEventArgs> ComplexTypeTablesGeneratedAsync;
        /// <summary>Occurs when the complex types have been generated.</summary>
        event EventHandler<ComplexTypeTablesGeneratedEventArgs> ComplexTypeTablesGenerated;
        /// <summary>Occurs asynchronously when the views are about to be generated.</summary>
        event EventHandler<ViewsGeneratingAsyncEventArgs> ViewsGeneratingAsync;
        /// <summary>Occurs when the views are about to be generated.</summary>
        event EventHandler<ViewsGeneratingEventArgs> ViewsGenerating;
        /// <summary>Occurs asynchronously when the view is about to be generated.</summary>
        event EventHandler<ViewGeneratingAsyncEventArgs> ViewGeneratingAsync;
        /// <summary>Occurs when the view is about to be generated.</summary>
        event EventHandler<ViewGeneratingEventArgs> ViewGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the view has been generated.</summary>
        event EventHandler<ViewPOCOAsyncEventArgs> ViewPOCOAsync;
        /// <summary>Occurs when the POCO class text of the view has been generated.</summary>
        event EventHandler<ViewPOCOEventArgs> ViewPOCO;
        /// <summary>Occurs asynchronously when the view has been generated.</summary>
        event EventHandler<ViewGeneratedAsyncEventArgs> ViewGeneratedAsync;
        /// <summary>Occurs when the view has been generated.</summary>
        event EventHandler<ViewGeneratedEventArgs> ViewGenerated;
        /// <summary>Occurs asynchronously when the views have been generated.</summary>
        event EventHandler<ViewsGeneratedAsyncEventArgs> ViewsGeneratedAsync;
        /// <summary>Occurs when the views have been generated.</summary>
        event EventHandler<ViewsGeneratedEventArgs> ViewsGenerated;
        /// <summary>Occurs asynchronously when the stored procedures are about to be generated.</summary>
        event EventHandler<ProceduresGeneratingAsyncEventArgs> ProceduresGeneratingAsync;
        /// <summary>Occurs when the stored procedures are about to be generated.</summary>
        event EventHandler<ProceduresGeneratingEventArgs> ProceduresGenerating;
        /// <summary>Occurs asynchronously when the stored procedure is about to be generated.</summary>
        event EventHandler<ProcedureGeneratingAsyncEventArgs> ProcedureGeneratingAsync;
        /// <summary>Occurs when the stored procedure is about to be generated.</summary>
        event EventHandler<ProcedureGeneratingEventArgs> ProcedureGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the stored procedure has been generated.</summary>
        event EventHandler<ProcedurePOCOAsyncEventArgs> ProcedurePOCOAsync;
        /// <summary>Occurs when the POCO class text of the stored procedure has been generated.</summary>
        event EventHandler<ProcedurePOCOEventArgs> ProcedurePOCO;
        /// <summary>Occurs asynchronously when the stored procedure has been generated.</summary>
        event EventHandler<ProcedureGeneratedAsyncEventArgs> ProcedureGeneratedAsync;
        /// <summary>Occurs when the stored procedure has been generated.</summary>
        event EventHandler<ProcedureGeneratedEventArgs> ProcedureGenerated;
        /// <summary>Occurs asynchronously when the stored procedures have been generated.</summary>
        event EventHandler<ProceduresGeneratedAsyncEventArgs> ProceduresGeneratedAsync;
        /// <summary>Occurs when the stored procedures have been generated.</summary>
        event EventHandler<ProceduresGeneratedEventArgs> ProceduresGenerated;
        /// <summary>Occurs asynchronously when the functions are about to be generated.</summary>
        event EventHandler<FunctionsGeneratingAsyncEventArgs> FunctionsGeneratingAsync;
        /// <summary>Occurs when the functions are about to be generated.</summary>
        event EventHandler<FunctionsGeneratingEventArgs> FunctionsGenerating;
        /// <summary>Occurs asynchronously when the function is about to be generated.</summary>
        event EventHandler<FunctionGeneratingAsyncEventArgs> FunctionGeneratingAsync;
        /// <summary>Occurs when the function is about to be generated.</summary>
        event EventHandler<FunctionGeneratingEventArgs> FunctionGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the function has been generated.</summary>
        event EventHandler<FunctionPOCOAsyncEventArgs> FunctionPOCOAsync;
        /// <summary>Occurs when the POCO class text of the function has been generated.</summary>
        event EventHandler<FunctionPOCOEventArgs> FunctionPOCO;
        /// <summary>Occurs asynchronously when the function has been generated.</summary>
        event EventHandler<FunctionGeneratedAsyncEventArgs> FunctionGeneratedAsync;
        /// <summary>Occurs when the function has been generated.</summary>
        event EventHandler<FunctionGeneratedEventArgs> FunctionGenerated;
        /// <summary>Occurs asynchronously when the functions have been generated.</summary>
        event EventHandler<FunctionsGeneratedAsyncEventArgs> FunctionsGeneratedAsync;
        /// <summary>Occurs when the functions have been generated.</summary>
        event EventHandler<FunctionsGeneratedEventArgs> FunctionsGenerated;
        /// <summary>Occurs asynchronously when the TVPs are about to be generated.</summary>
        event EventHandler<TVPsGeneratingAsyncEventArgs> TVPsGeneratingAsync;
        /// <summary>Occurs when the TVPs are about to be generated.</summary>
        event EventHandler<TVPsGeneratingEventArgs> TVPsGenerating;
        /// <summary>Occurs asynchronously when the TVP is about to be generated.</summary>
        event EventHandler<TVPGeneratingAsyncEventArgs> TVPGeneratingAsync;
        /// <summary>Occurs when the TVP is about to be generated.</summary>
        event EventHandler<TVPGeneratingEventArgs> TVPGenerating;
        /// <summary>Occurs asynchronously when the POCO class text of the TVP has been generated.</summary>
        event EventHandler<TVPPOCOAsyncEventArgs> TVPPOCOAsync;
        /// <summary>Occurs when the POCO class text of the TVP has been generated.</summary>
        event EventHandler<TVPPOCOEventArgs> TVPPOCO;
        /// <summary>Occurs asynchronously when the TVP has been generated.</summary>
        event EventHandler<TVPGeneratedAsyncEventArgs> TVPGeneratedAsync;
        /// <summary>Occurs when the TVP has been generated.</summary>
        event EventHandler<TVPGeneratedEventArgs> TVPGenerated;
        /// <summary>Occurs asynchronously when the TVPs have been generated.</summary>
        event EventHandler<TVPsGeneratedAsyncEventArgs> TVPsGeneratedAsync;
        /// <summary>Occurs when the TVPs have been generated.</summary>
        event EventHandler<TVPsGeneratedEventArgs> TVPsGenerated;
        /// <summary>Occurs asynchronously when the database has been generated.</summary>
        event EventHandler<DatabaseGeneratedAsyncEventArgs> DatabaseGeneratedAsync;
        /// <summary>Occurs when the database has been generated.</summary>
        event EventHandler<DatabaseGeneratedEventArgs> DatabaseGenerated;
        /// <summary>Occurs asynchronously when the server has been generated.</summary>
        event EventHandler<ServerGeneratedAsyncEventArgs> ServerGeneratedAsync;
        /// <summary>Occurs when the server has been generated.</summary>
        event EventHandler<ServerGeneratedEventArgs> ServerGenerated;
    }
}
