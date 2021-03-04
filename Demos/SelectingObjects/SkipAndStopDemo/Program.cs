using System;
using POCOGenerator;
using POCOGenerator.Objects;

namespace SkipAndStopDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            IGenerator generator = GeneratorFactory.GetConsoleColorGenerator();
            generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;

            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";

            // select everything
            generator.Settings.IncludeAll = true;

            generator.TableGenerating += (object sender, TableGeneratingEventArgs e) =>
            {
                Table table = e.Table;

                // skip any table that is not under Sales schema
                if (table.Schema != "Sales")
                    e.Skip = true;
            };

            generator.TablesGenerated += (object sender, TablesGeneratedEventArgs e) =>
            {
                // stop the generator
                // views, procedures, functions and TVPs will not be generated
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
    }
}
