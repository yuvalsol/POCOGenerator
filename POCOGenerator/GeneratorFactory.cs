using System;
using System.IO;
using System.Text;
using POCOGenerator.POCOWriters;

namespace POCOGenerator
{
    public static class GeneratorFactory
    {
        public static IGenerator GetConsoleColorGenerator()
        {
            return new Generator(WriterFactory.GetCreateConsoleColorWriterHandler());
        }

        public static IGenerator GetConsoleGenerator()
        {
            return new Generator(WriterFactory.GetCreateConsoleWriterHandler());
        }

        public static IGenerator GetGenerator()
        {
            return new Generator(WriterFactory.GetCreateEmptyWriterHandler());
        }

        public static IGenerator GetGenerator(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return new Generator(WriterFactory.GetCreateWriterHandler(stream));
        }

        public static IGenerator GetGenerator(StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException("stringBuilder");

            return new Generator(WriterFactory.GetCreateWriterHandler(stringBuilder));
        }

        public static IGenerator GetGenerator(TextWriter textWriter)
        {
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            return new Generator(WriterFactory.GetCreateWriterHandler(textWriter));
        }

        public static void RedirectToConsoleColor(this IGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateConsoleColorWriterHandler();
            }
        }

        public static void RedirectToConsole(this IGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateConsoleWriterHandler();
            }
        }

        public static void ClearOut(this IGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateEmptyWriterHandler();
            }
        }

        public static void RedirectTo(this IGenerator generator, Stream stream)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            if (stream == null)
                throw new ArgumentNullException("stream");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateWriterHandler(stream);
            }
        }

        public static void RedirectTo(this IGenerator generator, StringBuilder stringBuilder)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            if (stringBuilder == null)
                throw new ArgumentNullException("stringBuilder");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateWriterHandler(stringBuilder);
            }
        }

        public static void RedirectTo(this IGenerator generator, TextWriter textWriter)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateWriterHandler(textWriter);
            }
        }
    }
}
