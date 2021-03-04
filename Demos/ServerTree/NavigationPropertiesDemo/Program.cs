using System;
using System.IO;
using System.Linq;
using POCOGenerator;
using POCOGenerator.Objects;

namespace NavigationPropertiesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            bool redirectToFile = false;

            if (redirectToFile)
                ToFile();
            else
                Run();
        }

        private static void ToFile()
        {
            using (FileStream fs = File.Open(@"C:\NavigationPropertiesDemo.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    TextWriter writer = Console.Out;
                    Console.SetOut(sw);
                    Run();
                    Console.SetOut(writer);
                }
            }
        }

        private static void Run()
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;
            generator.Settings.Tables.IncludeAll = true;
            generator.Settings.NavigationProperties.ShowManyToManyJoinTable = false;

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                Server server = e.Server;
                bool showManyToManyJoinTable = generator.Settings.NavigationProperties.ShowManyToManyJoinTable;

                Console.WriteLine("Show Many-To-Many Join Table = {0}", showManyToManyJoinTable);
                Console.WriteLine();

                PrintServer(server, showManyToManyJoinTable);

                // do not generate classes
                e.Stop = true;
            };

            GeneratorResults results = generator.Generate();

            PrintError(results, generator.Error);
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

        private const int INDENT_SIZE = 3;

        private static void PrintServer(Server server, bool showManyToManyJoinTable)
        {
            Console.WriteLine(server.ToStringWithVersion());
            PrintDatabases(server, showManyToManyJoinTable, INDENT_SIZE);
        }

        private static void PrintDatabases(Server server, bool showManyToManyJoinTable, int indent)
        {
            if (server.Databases.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Databases");

                indent += INDENT_SIZE;
                foreach (Database database in server.Databases)
                    PrintDatabase(database, showManyToManyJoinTable, indent);
            }
        }

        private static void PrintDatabase(Database database, bool showManyToManyJoinTable, int indent)
        {
            Console.WriteLine("{0}{1}", new string(' ', indent), database);

            indent += INDENT_SIZE;
            PrintTables(database, showManyToManyJoinTable, indent);
        }

        private static void PrintTables(Database database, bool showManyToManyJoinTable, int indent)
        {
            if (database.Tables.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Tables");

                indent += INDENT_SIZE;
                foreach (Table table in database.Tables)
                    PrintTable(table, showManyToManyJoinTable, indent);
            }
        }

        private static void PrintTable(Table table, bool showManyToManyJoinTable, int indent)
        {
            // don't print join table
            if (table.IsJoinTable && showManyToManyJoinTable == false)
                return;

            Console.WriteLine("{0}{1}", new string(' ', indent), table);

            indent += INDENT_SIZE;
            PrintTableColumns(table, indent);
            PrintTablePrimaryKey(table, indent);
            PrintTableForeignKeys(table, indent);
            PrintTableNavigationProperties(table, showManyToManyJoinTable, indent);
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

        private static void PrintTableNavigationProperties(Table table, bool showManyToManyJoinTable, int indent)
        {
            if (table.NavigationProperties.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Navigation Properties");

                indent += INDENT_SIZE;
                foreach (NavigationProperty navigationProperty in table.NavigationProperties)
                    PrintTableNavigationProperty(navigationProperty, showManyToManyJoinTable, indent);
            }
        }

        private static void PrintTableNavigationProperty(NavigationProperty navigationProperty, bool showManyToManyJoinTable, int indent)
        {
            if ((showManyToManyJoinTable && navigationProperty.IsVisibleWhenShowManyToManyJoinTableIsOn) ||
                (showManyToManyJoinTable == false && navigationProperty.IsVisibleWhenShowManyToManyJoinTableIsOff))
            {
                PrintNavigationProperty(navigationProperty, indent);
                PrintInverseProperty(navigationProperty, indent);
            }
        }

        private static void PrintNavigationProperty(NavigationProperty navigationProperty, int indent)
        {
            Console.WriteLine(
                new string(' ', indent) +
                (navigationProperty.IsCollection ? "CollectionOf(" : string.Empty) +
                navigationProperty.ToTable +
                (navigationProperty.IsCollection ? ")" : string.Empty) +
                " " +
                navigationProperty.PropertyName +
                (navigationProperty.IsVirtualNavigationProperty ? " [Virtual Navigation Property]" : " [" + navigationProperty.ForeignKey + "]")
            );
        }

        private static void PrintInverseProperty(NavigationProperty navigationProperty, int indent)
        {
            NavigationProperty inverseProperty = navigationProperty.InverseProperty;

            if (inverseProperty != null)
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Inverse Property");
                indent += INDENT_SIZE;

                PrintNavigationProperty(inverseProperty, indent);
            }
        }
    }
}
