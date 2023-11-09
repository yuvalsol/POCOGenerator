using System;
using System.IO;
using POCOGenerator;

namespace ComplexTypesDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // execute script ComplexTypesDB.sql to create ComplexTypesDB database

            Console.ForegroundColor = ConsoleColor.Red;

            IGenerator generator = GeneratorFactory.GetConsoleColorGenerator();
            try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=ComplexTypesDB;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;
            generator.Settings.Tables.IncludeAll = true;

            GeneratorResults results = generator.Generate();
            PrintError(results, generator.Error);

            Console.WriteLine();
            Console.WriteLine("Press any key to re-generate with complex types");
            Console.ReadKey();

            generator.Settings.Reset();

            generator.Settings.POCO.ComplexTypes = true;

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
