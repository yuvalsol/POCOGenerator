using System;
using System.IO;
using System.Text;
using POCOGenerator.POCOWriters;

namespace POCOGenerator
{
    /// <summary>Creates instances of the POCO Generator and provides redirection to other output sources.</summary>
    public static class GeneratorFactory
    {
        #region Get Generator

        /// <summary>Gets an output-empty generator. The generator doesn't write to any underline output source.</summary>
        /// <returns>The output-empty generator.</returns>
        public static IGenerator GetGenerator()
        {
            return new Generator(WriterFactory.GetCreateOutputEmptyWriterHandler());
        }

        /// <summary>Gets a generator that writes to an instance of <see cref="StringBuilder" />.</summary>
        /// <param name="stringBuilder">The instance of <see cref="StringBuilder" /> that the generator writes to.</param>
        /// <returns>The generator that writes to an instance of <see cref="StringBuilder" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stringBuilder" /> is <see langword="null" />.</exception>
        public static IGenerator GetGenerator(StringBuilder stringBuilder)
        {
            if (stringBuilder == null)
                throw new ArgumentNullException("stringBuilder");

            return new Generator(WriterFactory.GetCreateWriterHandler(stringBuilder));
        }

        /// <summary>Gets a generator that writes to an instance of <see cref="TextWriter" />.</summary>
        /// <param name="textWriter">The instance of <see cref="TextWriter" /> that the generator writes to.</param>
        /// <returns>The generator that writes to an instance of <see cref="TextWriter" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="textWriter" /> is <see langword="null" />.</exception>
        public static IGenerator GetGenerator(TextWriter textWriter)
        {
            if (textWriter == null)
                throw new ArgumentNullException("textWriter");

            return new Generator(WriterFactory.GetCreateWriterHandler(textWriter));
        }

        /// <summary>Gets a generator that writes to an instance of <see cref="Stream" />.</summary>
        /// <param name="stream">The instance of <see cref="Stream" /> that the generator writes to.</param>
        /// <returns>The generator that writes to an instance of <see cref="Stream" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <see langword="null" />.</exception>
        public static IGenerator GetGenerator(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            return new Generator(WriterFactory.GetCreateWriterHandler(stream));
        }

        /// <summary>Gets a generator that writes to the <see cref="Console" />.</summary>
        /// <returns>The generator that writes to the <see cref="Console" />.</returns>
        public static IGenerator GetConsoleGenerator()
        {
            return new Generator(WriterFactory.GetCreateConsoleWriterHandler());
        }

        /// <summary>Gets a generator that writes to the <see cref="Console" /> with syntax highlight colors.</summary>
        /// <returns>The generator that writes to the <see cref="Console" /> with syntax highlight colors.</returns>
        public static IGenerator GetConsoleColorGenerator()
        {
            return new Generator(WriterFactory.GetCreateConsoleColorWriterHandler());
        }

        #endregion

        #region Redirect To

        /// <summary>Clears the generator underline output source.</summary>
        /// <param name="generator">The generator to clear its underline output source.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator" /> is <see langword="null" />.</exception>
        public static void RedirectToOutputEmpty(this IGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateOutputEmptyWriterHandler();
            }
        }

        /// <summary>Redirects the generator underline output source to an instance of <see cref="StringBuilder" />.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <param name="stringBuilder">The instance of <see cref="StringBuilder" /> that the generator is redirected to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator" /> is <see langword="null" /> or
        ///   <paramref name="stringBuilder" /> is <see langword="null" />.
        /// </exception>
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

        /// <summary>Redirects the generator underline output source to an instance of <see cref="TextWriter" />.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <param name="textWriter">The instance of <see cref="TextWriter" /> that the generator is redirected to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator" /> is <see langword="null" /> or
        ///   <paramref name="textWriter" /> is <see langword="null" />.
        /// </exception>
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

        /// <summary>Redirects the generator underline output source to an instance of <see cref="Stream" />.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <param name="stream">The instance of <see cref="Stream" /> that the generator is redirected to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator" /> is <see langword="null" /> or
        ///   <paramref name="stream" /> is <see langword="null" />.
        /// </exception>
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

        /// <summary>Redirects the generator underline output source to the <see cref="Console" />.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator" /> is <see langword="null" />.</exception>
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

        /// <summary>Redirects the generator underline output source to the <see cref="Console" /> with syntax highlight colors.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <exception cref="ArgumentNullException"><paramref name="generator" /> is <see langword="null" />.</exception>
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

        #endregion
    }
}
