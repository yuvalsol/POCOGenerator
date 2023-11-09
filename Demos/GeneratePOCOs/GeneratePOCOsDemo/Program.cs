using System;
using System.IO;
using POCOGenerator;

namespace GeneratePOCOsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            IGenerator generator = GeneratorFactory.GetConsoleColorGenerator();
            try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;

            generator.Settings.Tables.Include.Add("Sales.Store");

            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.IgnoreDboSchema = true;

            GeneratorResults results = generator.Generate();
            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to re-generate with navigation properties");
            Console.WriteLine("GeneratePOCOs() doesn't query the database a second time");
            Console.ReadKey();

            generator.Settings.Reset();

            generator.Settings.NavigationProperties.Enable = true;
            generator.Settings.NavigationProperties.VirtualNavigationProperties = true;
            generator.Settings.NavigationProperties.IEnumerableNavigationProperties = true;

            // this line has no effect on GeneratePOCOs()
            generator.Settings.Tables.IncludeAll = true;

            results = generator.GeneratePOCOs();
            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey();
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
