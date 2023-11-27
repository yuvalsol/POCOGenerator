using System;
using System.IO;
using System.Linq;
using POCOGenerator;
using POCOGenerator.Objects;

namespace DetailedServerTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            bool redirectToFile = true;

            if (redirectToFile)
                ToFile();
            else
                ToConsole();
        }

        private static void ToFile()
        {
            using (FileStream fs = File.Open(@"DetailedServerTreeDemo.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    TextWriter writer = Console.Out;
                    Console.SetOut(sw);
                    ToConsole(true);
                    Console.SetOut(writer);
                }
            }
        }

        private static void ToConsole(bool redirectToFile = false)
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;
            generator.Settings.IncludeAll = true;

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                Server server = e.Server;
                PrintServer(server);

                // do not generate classes
                e.Stop = true;
            };

            GeneratorResults results = generator.Generate();

            PrintError(results, generator.Error);

            if (redirectToFile == false)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to continue . . .");
                Console.ReadKey(true);
            }
        }

        private static void PrintError(GeneratorResults results, Exception Error)
        {
            bool isError = (results & GeneratorResults.Error) == GeneratorResults.Error;

            if (isError)
                Console.WriteLine("Error Result: {0}", results);

            if (Error != null)
            {
                Console.WriteLine("Error: {0}", Error.Message);
                Console.WriteLine("Error Stack Trace:");
                Console.WriteLine(Error.StackTrace);
            }
        }

        private const int INDENT_SIZE = 4;

        private static void PrintServer(Server server)
        {
            Console.WriteLine(server.ToStringWithVersion());
            PrintDatabases(server, INDENT_SIZE);
        }

        private static void PrintDatabases(Server server, int indent)
        {
            if (server.Databases.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Databases");

                indent += INDENT_SIZE;
                foreach (Database database in server.Databases)
                    PrintDatabase(database, indent);
            }
        }

        private static void PrintDatabase(Database database, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), database);

            indent += INDENT_SIZE;
            PrintTables(database, indent);
            PrintViews(database, indent);
            PrintProcedures(database, indent);
            PrintFunctions(database, indent);
            PrintTVPs(database, indent);
        }

        #region Tables

        private static void PrintTables(Database database, int indent)
        {
            if (database.Tables.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Tables");

                indent += INDENT_SIZE;
                foreach (Table table in database.Tables)
                    PrintTable(table, indent);
            }
        }

        private static void PrintTable(Table table, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), table);

            indent += INDENT_SIZE;
            PrintTableColumns(table, indent);
            PrintTablePrimaryKey(table, indent);
            PrintTableUniqueKeys(table, indent);
            PrintTableForeignKeys(table, indent);
            PrintTableIndexes(table, indent);
        }

        private static void PrintTableColumns(Table table, int indent)
        {
            if (table.TableColumns.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Columns");

                indent += INDENT_SIZE;
                foreach (TableColumn column in table.TableColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column.ToFullString());
            }
        }

        private static void PrintTablePrimaryKey(Table table, int indent)
        {
            if (table.PrimaryKey != null)
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Primary Key");

                indent += INDENT_SIZE;
                Console.WriteLine("{0}{1}", new string(' ', indent), table.PrimaryKey);

                indent += INDENT_SIZE;
                foreach (PrimaryKeyColumn column in table.PrimaryKey.PrimaryKeyColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column.TableColumn.ColumnName);
            }
        }

        private static void PrintTableUniqueKeys(Table table, int indent)
        {
            if (table.UniqueKeys.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Unique Keys");

                indent += INDENT_SIZE;
                foreach (UniqueKey uniqueKey in table.UniqueKeys)
                    PrintTableUniqueKey(uniqueKey, indent);
            }
        }

        private static void PrintTableUniqueKey(UniqueKey uniqueKey, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), uniqueKey);

            indent += INDENT_SIZE;
            foreach (UniqueKeyColumn column in uniqueKey.UniqueKeyColumns)
                Console.WriteLine("{0}{1}", new string(' ', indent), column.TableColumn.ColumnName);
        }

        private static void PrintTableForeignKeys(Table table, int indent)
        {
            if (table.ForeignKeys.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Foreign Keys");

                indent += INDENT_SIZE;
                foreach (ForeignKey foreignKey in table.ForeignKeys)
                    PrintTableForeignKey(foreignKey, indent);
            }
        }

        private static void PrintTableForeignKey(ForeignKey foreignKey, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), foreignKey);

            indent += INDENT_SIZE;
            foreach (ForeignKeyColumn column in foreignKey.ForeignKeyColumns)
                Console.WriteLine("{0}{1}.{2} -> {3}.{4}", new string(' ', indent), foreignKey.ForeignTable, column.ForeignTableColumn.ColumnName, foreignKey.PrimaryTable, column.PrimaryTableColumn.ColumnName);
        }

        private static void PrintTableIndexes(Table table, int indent)
        {
            if (table.Indexes.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Indexes");

                indent += INDENT_SIZE;
                foreach (TableIndex index in table.Indexes)
                    PrintTableIndex(index, indent);
            }
        }

        private static void PrintTableIndex(TableIndex index, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), index.ToFullString());

            indent += INDENT_SIZE;
            foreach (TableIndexColumn column in index.IndexColumns)
                Console.WriteLine("{0}{1}{2}", new string(' ', indent), column.TableColumn.ColumnName, (column.Is_Descending ? " (Desc)" : " (Asc)"));
        }

        #endregion

        #region Views

        private static void PrintViews(Database database, int indent)
        {
            if (database.Views.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Views");

                indent += INDENT_SIZE;
                foreach (View view in database.Views)
                    PrintView(view, indent);
            }
        }

        private static void PrintView(View view, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), view);

            indent += INDENT_SIZE;
            PrintViewColumns(view, indent);
            PrintViewIndexes(view, indent);
        }

        private static void PrintViewColumns(View view, int indent)
        {
            if (view.ViewColumns.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Columns");

                indent += INDENT_SIZE;
                foreach (ViewColumn column in view.ViewColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column.ToFullString());
            }
        }

        private static void PrintViewIndexes(View view, int indent)
        {
            if (view.Indexes.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Indexes");

                indent += INDENT_SIZE;
                foreach (ViewIndex index in view.Indexes)
                    PrintViewIndex(index, indent);
            }
        }

        private static void PrintViewIndex(ViewIndex index, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), index.ToFullString());

            indent += INDENT_SIZE;
            foreach (ViewIndexColumn column in index.IndexColumns)
                Console.WriteLine("{0}{1}{2}", new string(' ', indent), column.ViewColumn.ColumnName, (column.Is_Descending ? " (Desc)" : " (Asc)"));
        }

        #endregion

        #region Procedures

        private static void PrintProcedures(Database database, int indent)
        {
            if (database.Procedures.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Stored Procedures");

                indent += INDENT_SIZE;
                foreach (Procedure procedure in database.Procedures)
                    PrintProcedure(procedure, indent);
            }
        }

        private static void PrintProcedure(Procedure procedure, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), procedure);

            indent += INDENT_SIZE;
            PrintProcedureParameters(procedure, indent);
            PrintProcedureColumns(procedure, indent);
        }

        private static void PrintProcedureParameters(Procedure procedure, int indent)
        {
            if (procedure.ProcedureParameters.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Parameters");

                indent += INDENT_SIZE;
                foreach (ProcedureParameter parameter in procedure.ProcedureParameters)
                    Console.WriteLine("{0}{1}", new string(' ', indent), parameter);
            }
        }

        private static void PrintProcedureColumns(Procedure procedure, int indent)
        {
            if (procedure.ProcedureColumns.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Columns");

                indent += INDENT_SIZE;
                foreach (ProcedureColumn column in procedure.ProcedureColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column);
            }
        }

        #endregion

        #region Functions

        private static void PrintFunctions(Database database, int indent)
        {
            if (database.Functions.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Table-valued Functions");

                indent += INDENT_SIZE;
                foreach (Function function in database.Functions)
                    PrintFunction(function, indent);
            }
        }

        private static void PrintFunction(Function function, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), function);

            indent += INDENT_SIZE;
            PrintFunctionParameters(function, indent);
            PrintFunctionColumns(function, indent);
        }

        private static void PrintFunctionParameters(Function function, int indent)
        {
            if (function.FunctionParameters.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Parameters");

                indent += INDENT_SIZE;
                foreach (FunctionParameter parameter in function.FunctionParameters)
                    Console.WriteLine("{0}{1}", new string(' ', indent), parameter);
            }
        }

        private static void PrintFunctionColumns(Function function, int indent)
        {
            if (function.FunctionColumns.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Columns");

                indent += INDENT_SIZE;
                foreach (FunctionColumn column in function.FunctionColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column);
            }
        }

        #endregion

        #region TVPs

        private static void PrintTVPs(Database database, int indent)
        {
            if (database.TVPs.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "User-Defined Table Types");

                indent += INDENT_SIZE;
                foreach (TVP tvp in database.TVPs)
                    PrintTVP(tvp, indent);
            }
        }

        private static void PrintTVP(TVP tvp, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), tvp);

            indent += INDENT_SIZE;
            PrintTVPColumns(tvp, indent);
        }

        private static void PrintTVPColumns(TVP tvp, int indent)
        {
            if (tvp.TVPColumns.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Columns");

                indent += INDENT_SIZE;
                foreach (TVPColumn column in tvp.TVPColumns)
                    Console.WriteLine("{0}{1}", new string(' ', indent), column);
            }
        }

        #endregion
    }
}
