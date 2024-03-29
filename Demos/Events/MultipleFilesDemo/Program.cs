﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using POCOGenerator;
using POCOGenerator.Objects;

namespace MultipleFilesDemo
{
    class Program
    {
        static void Main()
        {
            IGenerator generator = GeneratorFactory.GetGenerator();
            try { generator.Settings.Connection.ConnectionString = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(generator.Settings.Connection.ConnectionString))
                generator.Settings.Connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
            generator.Settings.DatabaseObjects.IncludeAll = true;
            generator.Settings.POCO.Using = true;
            generator.Settings.POCO.Namespace = "MultipleFilesDemo";
            generator.Settings.POCO.CommentsWithoutNull = true;

            // wrap using & namespace around each class
            generator.Settings.POCO.WrapAroundEachClass = true;

            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("Root Path:");
            Console.WriteLine(root);
            Console.WriteLine();

            string path = root;
            int indent = 0;
            // key: schema, value: files count
            OrderedDictionary filesCount = new OrderedDictionary(StringComparer.OrdinalIgnoreCase);

            #region Server

            // create server and namespace folders
            generator.ServerGenerating += (object sender, ServerGeneratingEventArgs e) =>
            {
                // create server folder

                string server = e.Server.ToString();
                string folder = string.Join("_", server.Split(Path.GetInvalidFileNameChars()));

                path = Path.Combine(path, folder);

                if (Directory.Exists(path))
                {
                    try { Directory.Delete(path, true); }
                    catch { }
                }

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                Console.WriteLine("{0}", folder);

                // create namespace folder

                if (sender is IGenerator g)
                {
                    string @namespace = g.Settings.POCO.Namespace;

                    if (string.IsNullOrEmpty(@namespace) == false)
                    {
                        folder = string.Join("_", @namespace.Split(Path.GetInvalidFileNameChars()));

                        path = Path.Combine(path, folder);

                        if (Directory.Exists(path) == false)
                            Directory.CreateDirectory(path);

                        indent += INDENT_SIZE;
                        Console.WriteLine("{0}{1}", new string(' ', indent), folder);
                    }
                }
            };

            #region Database

            // create database folder
            generator.DatabaseGenerating += (object sender, DatabaseGeneratingEventArgs e) =>
            {
                string database = e.Database.ToString();
                string folder = string.Join("_", database.Split(Path.GetInvalidFileNameChars()));

                path = Path.Combine(path, folder);

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                indent += INDENT_SIZE;
                Console.WriteLine("{0}{1}", new string(' ', indent), folder);
            };

            #region Tables

            // create Tables folder
            generator.TablesGenerating += (object sender, TablesGeneratingEventArgs e) =>
            {
                DbGroupGenerating("Tables", ref path, ref indent, filesCount);
            };

            // get namespace for table
            generator.TableGenerating += (object sender, TableGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.Table.Database, "Tables", e.Table.Schema);
            };

            // save table poco
            generator.TablePOCO += (object sender, TablePOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.Table.Schema, path, filesCount);
            };

            // take the path one step up once all the tables are written
            generator.TablesGenerated += (object sender, TablesGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            #region ComplexTypeTables

            // create Tables folder for complex type tables
            generator.ComplexTypeTablesGenerating += (object sender, ComplexTypeTablesGeneratingEventArgs e) =>
            {
                DbGroupGenerating("Tables", ref path, ref indent, filesCount);
            };

            // get namespace for complex type table
            generator.ComplexTypeTableGenerating += (object sender, ComplexTypeTableGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.ComplexTypeTable.Database, "Tables", e.ComplexTypeTable.Schema);
            };

            // save complex type table poco
            generator.ComplexTypeTablePOCO += (object sender, ComplexTypeTablePOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.ComplexTypeTable.Schema, path, filesCount);
            };

            // take the path one step up once all the complex type tables are written
            generator.ComplexTypeTablesGenerated += (object sender, ComplexTypeTablesGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            #region Views

            // create Views folder
            generator.ViewsGenerating += (object sender, ViewsGeneratingEventArgs e) =>
            {
                DbGroupGenerating("Views", ref path, ref indent, filesCount);
            };

            // get namespace for view
            generator.ViewGenerating += (object sender, ViewGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.View.Database, "Views", e.View.Schema);
            };

            // save view poco
            generator.ViewPOCO += (object sender, ViewPOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.View.Schema, path, filesCount);
            };

            // take the path one step up once all the views are written
            generator.ViewsGenerated += (object sender, ViewsGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            #region Procedures

            // create Procedures folder
            generator.ProceduresGenerating += (object sender, ProceduresGeneratingEventArgs e) =>
            {
                DbGroupGenerating("Procedures", ref path, ref indent, filesCount);
            };

            // get namespace for procedure
            generator.ProcedureGenerating += (object sender, ProcedureGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.Procedure.Database, "Procedures", e.Procedure.Schema);
            };

            // save procedure poco
            generator.ProcedurePOCO += (object sender, ProcedurePOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.Procedure.Schema, path, filesCount);
            };

            // take the path one step up once all the procedures are written
            generator.ProceduresGenerated += (object sender, ProceduresGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            #region Functions

            // create Functions folder
            generator.FunctionsGenerating += (object sender, FunctionsGeneratingEventArgs e) =>
            {
                DbGroupGenerating("Functions", ref path, ref indent, filesCount);
            };

            // get namespace for function
            generator.FunctionGenerating += (object sender, FunctionGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.Function.Database, "Functions", e.Function.Schema);
            };

            // save function poco
            generator.FunctionPOCO += (object sender, FunctionPOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.Function.Schema, path, filesCount);
            };

            // take the path one step up once all the functions are written
            generator.FunctionsGenerated += (object sender, FunctionsGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            #region TVPs

            // create TVPs folder
            generator.TVPsGenerating += (object sender, TVPsGeneratingEventArgs e) =>
            {
                DbGroupGenerating("TVPs", ref path, ref indent, filesCount);
            };

            // get namespace for tvp
            generator.TVPGenerating += (object sender, TVPGeneratingEventArgs e) =>
            {
                e.Namespace = GetNamespace(e.Namespace, e.TVP.Database, "TVPs", e.TVP.Schema);
            };

            // save tvp poco
            generator.TVPPOCO += (object sender, TVPPOCOEventArgs e) =>
            {
                DbObjectPOCO(e.ClassName, e.POCO, e.TVP.Schema, path, filesCount);
            };

            // take the path one step up once all the tvps are written
            generator.TVPsGenerated += (object sender, TVPsGeneratedEventArgs e) =>
            {
                DbGroupGenerated(ref path, ref indent, filesCount);
            };

            #endregion

            // take the path one step up once the database is written
            generator.DatabaseGenerated += (object sender, DatabaseGeneratedEventArgs e) =>
            {
                path = Path.GetDirectoryName(path);
                indent -= INDENT_SIZE;
            };

            #endregion

            // take the path one step up once the server is written
            generator.ServerGenerated += (object sender, ServerGeneratedEventArgs e) =>
            {
                path = Path.GetDirectoryName(path);
            };

            #endregion

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

        private const int INDENT_SIZE = 4;

        private static void DbGroupGenerating(string dbGroup, ref string path, ref int indent, OrderedDictionary filesCount)
        {
            path = Path.Combine(path, dbGroup);

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            indent += INDENT_SIZE;
            Console.WriteLine("{0}{1}", new string(' ', indent), dbGroup);

            filesCount.Clear();
        }

        private static string GetNamespace(string @namespace, Database database, string dbGroup, string schema)
        {
            string fullNamespace = string.Format("{0}.{1}", database, dbGroup);
            if (string.IsNullOrEmpty(@namespace) == false)
                fullNamespace = string.Format("{0}.{1}", @namespace, fullNamespace);
            if (string.IsNullOrEmpty(schema) == false)
                fullNamespace = string.Format("{0}.{1}", fullNamespace, schema);
            return fullNamespace;
        }

        private static void DbObjectPOCO(string className, string poco, string schema, string path, OrderedDictionary filesCount)
        {
            schema = string.Join("_", (schema ?? string.Empty).Split(Path.GetInvalidFileNameChars()));

            path = Path.Combine(path, schema);

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            string fileName = string.Join("_", className.Split(Path.GetInvalidFileNameChars())) + ".cs";

            path = Path.Combine(path, fileName);
            File.WriteAllText(path, poco);

            if (filesCount.Contains(schema))
                filesCount[schema] = (int)filesCount[schema] + 1;
            else
                filesCount.Add(schema, 1);
        }

        private static void DbGroupGenerated(ref string path, ref int indent, OrderedDictionary filesCount)
        {
            path = Path.GetDirectoryName(path);

            if (filesCount.Count > 0)
            {
                indent += INDENT_SIZE;

                foreach (DictionaryEntry item in filesCount)
                {
                    string schema = (string)item.Key;
                    int count = (int)item.Value;
                    Console.WriteLine("{0}{1}: {2} {3}", new string(' ', indent), schema, count, (count == 1 ? "file" : "files"));
                }

                indent -= INDENT_SIZE;
            }

            indent -= INDENT_SIZE;
        }
    }
}
