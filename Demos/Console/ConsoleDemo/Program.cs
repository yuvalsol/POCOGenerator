using System;
using POCOGenerator;

namespace ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator = GeneratorFactory.GetConsoleGenerator();
            generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;
            generator.Settings.Tables.IncludeAll = true;
            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.IgnoreDboSchema = true;

            GeneratorResults results = generator.Generate();

            PrintError(results, generator.Error);
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
