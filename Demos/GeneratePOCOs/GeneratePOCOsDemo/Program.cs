using System;
using System.IO;
using POCOGenerator;

namespace GeneratePOCOsDemo
{
    class Program
    {
        static void Main()
        {
            IGenerator generator = GeneratorFactory.GetConsoleGenerator();
            try { generator.Settings.Connection.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.Connection.ConnectionString))
                generator.Settings.Connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";

            generator.Settings.DatabaseObjects.Tables.Include.Add("Sales.Store");

            // settings for the first run
            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.IgnoreDboSchema = true;

            // first run
            GeneratorResults results = generator.Generate();
            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to re-generate with navigation properties");
            Console.WriteLine("GeneratePOCOs() doesn't query the database a second time");
            Console.ReadKey(true);

            // settings reset also clears the list of included database objects ("Sales.Store")
            // but not the list of objects that were previously constructed
            generator.Settings.Reset();

            // settings for the second run
            generator.Settings.NavigationProperties.Enable = true;
            generator.Settings.NavigationProperties.VirtualNavigationProperties = true;
            generator.Settings.NavigationProperties.IEnumerableNavigationProperties = true;

            // this line has no effect on GeneratePOCOs() (but would for Generate())
            // because GeneratePOCOs() skips calling the database
            generator.Settings.DatabaseObjects.Tables.IncludeAll = true;

            // second run
            results = generator.GeneratePOCOs();
            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }

        private static void PrintError(GeneratorResults results, Exception Error)
        {
            bool isError = (results & GeneratorResults.Error) == GeneratorResults.Error;
            bool isWarning = (results & GeneratorResults.Warning) == GeneratorResults.Warning;

            if (results != GeneratorResults.None)
                Console.WriteLine();

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
