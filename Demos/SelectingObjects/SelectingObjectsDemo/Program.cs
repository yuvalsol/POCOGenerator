using System;
using System.IO;
using POCOGenerator;
using POCOGenerator.Objects;

namespace SelectingObjectsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            try { generator.Settings.Connection.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.Connection.ConnectionString))
                generator.Settings.Connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";

            // database object is selected when:
            // 1. explicitly included: Settings.IncludeAll, Settings.Tables.IncludeAll, Settings.Tables.Include.Add(), ...
            // 2. not explicitly excluded. it doesn't appear in any excluding setting: Settings.Tables.ExcludeAll, Settings.Tables.Exclude.Add(), ...

            // select all the tables under HumanResources & Purchasing schemas
            // and select table Production.Product
            generator.Settings.DatabaseObjects.Tables.Include.Add("HumanResources.*");
            generator.Settings.DatabaseObjects.Tables.Include.Add("Purchasing.*");
            generator.Settings.DatabaseObjects.Tables.Include.Add("Production.Product");

            // select all views except views under Production & Sales schemas
            // and except view Person.vAdditionalContactInfo
            generator.Settings.DatabaseObjects.Views.IncludeAll = true;
            generator.Settings.DatabaseObjects.Views.Exclude.Add("Production.*");
            generator.Settings.DatabaseObjects.Views.Exclude.Add("Sales.*");
            generator.Settings.DatabaseObjects.Views.Exclude.Add("Person.vAdditionalContactInfo");

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                foreach (Database database in e.Server.Databases)
                {
                    Console.WriteLine("Tables:");
                    Console.WriteLine("-------");
                    foreach (Table table in database.Tables)
                        Console.WriteLine(table);
                    Console.WriteLine();

                    Console.WriteLine("Views:");
                    Console.WriteLine("------");
                    foreach (View view in database.Views)
                        Console.WriteLine(view);
                    Console.WriteLine();
                }

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
    }
}
