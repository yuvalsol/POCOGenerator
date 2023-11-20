using System;
using System.IO;
using POCOGenerator;
using POCOGenerator.Objects;

namespace WildcardsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.RDBMS = RDBMS.SQLServer;

            // all the tables under Sales schema
            generator.Settings.Tables.Include.Add("Sales.*");

            // HumanResources.Employee but not HumanResources.EmployeeDepartmentHistory
            // or HumanResources.EmployeePayHistory
            generator.Settings.Tables.Include.Add("Employe?");

            generator.ServerBuilt += (object sender, ServerBuiltEventArgs e) =>
            {
                foreach (Database database in e.Server.Databases)
                {
                    foreach (Table table in database.Tables)
                        Console.WriteLine(table);
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
