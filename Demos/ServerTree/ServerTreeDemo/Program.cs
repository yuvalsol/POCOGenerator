using System;
using System.IO;
using System.Linq;
using POCOGenerator;
using POCOGenerator.Objects;

namespace ServerTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            bool redirectToFile = false;

            if (redirectToFile)
                ToFile();
            else
                ToConsole();
        }

        private static void ToFile()
        {
            using (FileStream fs = File.Open(@"ServerTreeDemo.txt", FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    TextWriter writer = Console.Out;
                    Console.SetOut(sw);
                    ToConsole();
                    Console.SetOut(writer);
                }
            }
        }

        private static void ToConsole()
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

            Console.WriteLine();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
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
            Console.WriteLine(server);
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

        private static void PrintTables(Database database, int indent)
        {
            if (database.Tables.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Tables");

                indent += INDENT_SIZE;
                foreach (Table table in database.Tables)
                    Console.WriteLine("{0}{1}", new string(' ', indent), table);
            }
        }

        private static void PrintViews(Database database, int indent)
        {
            if (database.Views.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Views");

                indent += INDENT_SIZE;
                foreach (View view in database.Views)
                    Console.WriteLine("{0}{1}", new string(' ', indent), view);
            }
        }

        private static void PrintProcedures(Database database, int indent)
        {
            if (database.Procedures.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Stored Procedures");

                indent += INDENT_SIZE;
                foreach (Procedure procedure in database.Procedures)
                    Console.WriteLine("{0}{1}", new string(' ', indent), procedure);
            }
        }

        private static void PrintFunctions(Database database, int indent)
        {
            if (database.Functions.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "Table-valued Functions");

                indent += INDENT_SIZE;
                foreach (Function function in database.Functions)
                    Console.WriteLine("{0}{1}", new string(' ', indent), function);
            }
        }

        private static void PrintTVPs(Database database, int indent)
        {
            if (database.TVPs.Any())
            {
                Console.WriteLine("{0}{1}", new string(' ', indent), "User-Defined Table Types");

                indent += INDENT_SIZE;
                foreach (TVP tvp in database.TVPs)
                    Console.WriteLine("{0}{1}", new string(' ', indent), tvp);
            }
        }
    }
}
