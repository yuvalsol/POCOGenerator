using System;
using System.IO;
using System.Windows.Forms;
using POCOGenerator.POCOWriters;

namespace POCOGenerator
{
    /// <summary>Creates instances of the POCO Generator, suitable for WinForms, and provides redirection to other WinForms output sources.</summary>
    public static class GeneratorWinFormsFactory
    {
        /// <summary>Gets a generator that writes to an instance of <see cref="RichTextBox" />.</summary>
        /// <param name="richTextBox">The instance of <see cref="RichTextBox" /> that the generator writes to.</param>
        /// <returns>The generator that writes to an instance of <see cref="RichTextBox" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="richTextBox" /> is <see langword="null" />.</exception>
        public static IGenerator GetGenerator(RichTextBox richTextBox)
        {
            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            return new Generator(WriterFactory.GetCreateWriterHandler(richTextBox));
        }

        /// <summary>Redirects the generator underline output source to an instance of <see cref="RichTextBox" />.</summary>
        /// <param name="generator">The generator to redirect its underline output source.</param>
        /// <param name="richTextBox">The instance of <see cref="RichTextBox" /> that the generator is redirected to.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="generator" /> is <see langword="null" /> or
        ///   <paramref name="richTextBox" /> is <see langword="null" />.
        /// </exception>
        public static void RedirectTo(this IGenerator generator, RichTextBox richTextBox)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            var g = (Generator)generator;
            lock (g.lockObject)
            {
                g.createWriter = WriterFactory.GetCreateWriterHandler(richTextBox);
            }
        }
    }
}
