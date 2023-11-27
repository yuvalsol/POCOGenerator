using System;
using System.Drawing;
using System.IO;
using POCOGenerator;

namespace ConsoleColorDarkThemeDemo
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
            generator.Settings.DatabaseObjects.Tables.IncludeAll = true;
            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.IgnoreDboSchema = true;

            generator.Settings.SyntaxHighlight.Text = Color.FromArgb(255, 255, 255);
            generator.Settings.SyntaxHighlight.Keyword = Color.FromArgb(86, 156, 214);
            generator.Settings.SyntaxHighlight.UserType = Color.FromArgb(78, 201, 176);
            generator.Settings.SyntaxHighlight.String = Color.FromArgb(214, 157, 133);
            generator.Settings.SyntaxHighlight.Comment = Color.FromArgb(96, 139, 78);
            generator.Settings.SyntaxHighlight.Error = Color.FromArgb(255, 0, 0);
            generator.Settings.SyntaxHighlight.Background = Color.FromArgb(0, 0, 0);

            GeneratorResults results = generator.Generate();

            PrintError(results, generator.Error);

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
