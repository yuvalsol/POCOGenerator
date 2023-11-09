using System;
using System.IO;
using POCOGenerator;
using POCOGenerator.Objects;

namespace EventsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;
            generator.Settings.IncludeAll = true;
            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.WordsSeparator = "_";

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                Server server = e.Server;
                e.Stop = false;
            };

            generator.ServerGenerating += (object sender, ServerGeneratingEventArgs e) =>
            {
                Server server = e.Server;
                e.Stop = false;
            };

            generator.DatabaseGenerating += (object sender, DatabaseGeneratingEventArgs e) =>
            {
                Database database = e.Database;
                e.Skip = false;
                e.Stop = false;
            };

            generator.TablesGenerating += (object sender, TablesGeneratingEventArgs e) =>
            {
                e.Skip = false;
                e.Stop = false;
            };

            generator.TableGenerating += (object sender, TableGeneratingEventArgs e) =>
            {
                Table table = e.Table;
                string className = e.ClassName;
                string error = e.Error;
                e.Namespace = e.Namespace;
                e.Skip = false;
                e.Stop = false;
            };

            generator.TablePOCO += (object sender, TablePOCOEventArgs e) =>
            {
                Table table = e.Table;
                string className = e.ClassName;
                string error = e.Error;
                string poco = e.POCO;
                e.Stop = false;

                Console.WriteLine(poco);
                Console.WriteLine();
            };

            generator.TableGenerated += (object sender, TableGeneratedEventArgs e) =>
            {
                Table table = e.Table;
                string className = e.ClassName;
                string error = e.Error;
                string @namespace = e.Namespace;
                e.Stop = false;
            };

            generator.TablesGenerated += (object sender, TablesGeneratedEventArgs e) =>
            {
                e.Stop = false;
            };

            generator.ViewsGenerating += (object sender, ViewsGeneratingEventArgs e) =>
            {
                e.Skip = false;
                e.Stop = false;
            };

            generator.ViewGenerating += (object sender, ViewGeneratingEventArgs e) =>
            {
                View view = e.View;
                string className = e.ClassName;
                string error = e.Error;
                e.Namespace = e.Namespace;
                e.Skip = false;
                e.Stop = false;
            };

            generator.ViewPOCO += (object sender, ViewPOCOEventArgs e) =>
            {
                View view = e.View;
                string className = e.ClassName;
                string error = e.Error;
                string poco = e.POCO;
                e.Stop = false;

                Console.WriteLine(poco);
                Console.WriteLine();
            };

            generator.ViewGenerated += (object sender, ViewGeneratedEventArgs e) =>
            {
                View view = e.View;
                string className = e.ClassName;
                string error = e.Error;
                string @namespace = e.Namespace;
                e.Stop = false;
            };

            generator.ViewsGenerated += (object sender, ViewsGeneratedEventArgs e) =>
            {
                e.Stop = false;
            };

            generator.ProceduresGenerating += (object sender, ProceduresGeneratingEventArgs e) =>
            {
                e.Skip = false;
                e.Stop = false;
            };

            generator.ProcedureGenerating += (object sender, ProcedureGeneratingEventArgs e) =>
            {
                Procedure procedure = e.Procedure;
                string className = e.ClassName;
                string error = e.Error;
                e.Namespace = e.Namespace;
                e.Skip = false;
                e.Stop = false;
            };

            generator.ProcedurePOCO += (object sender, ProcedurePOCOEventArgs e) =>
            {
                Procedure procedure = e.Procedure;
                string className = e.ClassName;
                string error = e.Error;
                string poco = e.POCO;
                e.Stop = false;

                Console.WriteLine(poco);
                Console.WriteLine();
            };

            generator.ProcedureGenerated += (object sender, ProcedureGeneratedEventArgs e) =>
            {
                Procedure procedure = e.Procedure;
                string className = e.ClassName;
                string error = e.Error;
                string @namespace = e.Namespace;
                e.Stop = false;
            };

            generator.ProceduresGenerated += (object sender, ProceduresGeneratedEventArgs e) =>
            {
                e.Stop = false;
            };

            generator.FunctionsGenerating += (object sender, FunctionsGeneratingEventArgs e) =>
            {
                e.Skip = false;
                e.Stop = false;
            };

            generator.FunctionGenerating += (object sender, FunctionGeneratingEventArgs e) =>
            {
                Function function = e.Function;
                string className = e.ClassName;
                string error = e.Error;
                e.Namespace = e.Namespace;
                e.Skip = false;
                e.Stop = false;
            };

            generator.FunctionPOCO += (object sender, FunctionPOCOEventArgs e) =>
            {
                Function function = e.Function;
                string className = e.ClassName;
                string error = e.Error;
                string poco = e.POCO;
                e.Stop = false;

                Console.WriteLine(poco);
                Console.WriteLine();
            };

            generator.FunctionGenerated += (object sender, FunctionGeneratedEventArgs e) =>
            {
                Function function = e.Function;
                string className = e.ClassName;
                string error = e.Error;
                string @namespace = e.Namespace;
                e.Stop = false;
            };

            generator.FunctionsGenerated += (object sender, FunctionsGeneratedEventArgs e) =>
            {
                e.Stop = false;
            };

            generator.TVPsGenerating += (object sender, TVPsGeneratingEventArgs e) =>
            {
                e.Skip = false;
                e.Stop = false;
            };

            generator.TVPGenerating += (object sender, TVPGeneratingEventArgs e) =>
            {
                TVP tvp = e.TVP;
                string className = e.ClassName;
                string error = e.Error;
                e.Namespace = e.Namespace;
                e.Skip = false;
                e.Stop = false;
            };

            generator.TVPPOCO += (object sender, TVPPOCOEventArgs e) =>
            {
                TVP tvp = e.TVP;
                string className = e.ClassName;
                string error = e.Error;
                string poco = e.POCO;
                e.Stop = false;

                Console.WriteLine(poco);
                Console.WriteLine();
            };

            generator.TVPGenerated += (object sender, TVPGeneratedEventArgs e) =>
            {
                TVP tvp = e.TVP;
                string className = e.ClassName;
                string error = e.Error;
                string @namespace = e.Namespace;
                e.Stop = false;
            };

            generator.TVPsGenerated += (object sender, TVPsGeneratedEventArgs e) =>
            {
                e.Stop = false;
            };

            generator.DatabaseGenerated += (object sender, DatabaseGeneratedEventArgs e) =>
            {
                Database database = e.Database;
                e.Stop = false;
            };

            generator.ServerGenerated += (object sender, ServerGeneratedEventArgs e) =>
            {
                Server server = e.Server;
                e.Stop = false;
            };

            GeneratorResults results = generator.Generate();

            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }

        private static void PrintError(GeneratorResults results, Exception Error)
        {
            bool isError = (results & GeneratorResults.Error) == GeneratorResults.Error;
            bool isWarning = (results & GeneratorResults.Warning) == GeneratorResults.Warning;

            if (isError)
                Console.WriteLine("Error Result: {0}", results);
            else if (isWarning)
                Console.WriteLine("Warning Result: {0}", results);

            if (Error != null)
            {
                Console.WriteLine("Error: {0}", Error.Message);
                Console.WriteLine("Error Stack Trace:");
                Console.WriteLine(Error.StackTrace);
            }
        }
    }
}
