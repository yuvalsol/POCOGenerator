using System;
using System.IO;
using System.Reflection;
using POCOGenerator;

namespace FileStreamDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "AdventureWorks2014.cs";
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);

            Console.WriteLine("File Name: {0}", fileName);
            Console.WriteLine("File Path: {0}", filePath);

            using (FileStream stream = File.Open(filePath, FileMode.Create))
            {
                IGenerator generator = GeneratorFactory.GetGenerator(stream);
                try { generator.Settings.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
                if (string.IsNullOrEmpty(generator.Settings.ConnectionString))
                    generator.Settings.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
                generator.Settings.RDBMS = RDBMS.SQLServer;
                generator.Settings.DatabaseObjects.Tables.IncludeAll = true;
                generator.Settings.POCO.CommentsWithoutNull = true;
                generator.Settings.ClassName.IncludeSchema = true;
                generator.Settings.ClassName.SchemaSeparator = "_";
                generator.Settings.ClassName.IgnoreDboSchema = true;

                GeneratorResults results = generator.Generate();
                PrintError(results, generator.Error);
            }

            if (File.Exists(filePath))
            {
                long length = new FileInfo(filePath).Length;
                Console.WriteLine("File {0} was saved successfully", fileName);
                Console.WriteLine("File size is {0} bytes", length);

                Console.WriteLine();
                Console.WriteLine("Delete file? [y/n]");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.KeyChar == 'y' || key.KeyChar == 'Y')
                {
                    Console.WriteLine();
                    Console.WriteLine("Deleting file...");
                    try
                    {
                        File.Delete(filePath);
                        Console.WriteLine("File {0} was deleted", fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: Filed to delete File {0}", fileName);
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: File {0} wasn't saved", fileName);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
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
