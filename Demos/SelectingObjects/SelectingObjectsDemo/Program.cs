using System;
using POCOGenerator;
using POCOGenerator.Objects;

namespace SelectingObjectsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;

            // db object is selected when:
            // 1. explicitly included: Settings.IncludeAll, Settings.Tables.IncludeAll, Settings.Tables.Include.Add(), ...
            // 2. not explicitly excluded. it doesn't appear in any excluding setting: Settings.Tables.ExcludeAll, Settings.Tables.Exclude.Add(), ...

            // select all the tables under HumanResources & Purchasing schema
            // and select table Production.Product
            generator.Settings.Tables.Include.Add("HumanResources.*");
            generator.Settings.Tables.Include.Add("Purchasing.*");
            generator.Settings.Tables.Include.Add("Production.Product");

            // select all views except view under Production & Sales schema
            // and except view Person.vAdditionalContactInfo
            generator.Settings.Views.IncludeAll = true;
            generator.Settings.Views.Exclude.Add("Production.*");
            generator.Settings.Views.Exclude.Add("Sales.*");
            generator.Settings.Views.Exclude.Add("Person.vAdditionalContactInfo");

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                foreach (Database database in e.Server.Databases)
                {
                    Console.WriteLine("Tables:");
                    Console.WriteLine("-------");
                    foreach (Table table in database.Tables)
                        Console.WriteLine(table);
                    Console.WriteLine();

                    Console.WriteLine("View:");
                    Console.WriteLine("-----");
                    foreach (View view in database.Views)
                        Console.WriteLine(view);
                    Console.WriteLine();
                }

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
    }
}
