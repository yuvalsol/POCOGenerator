using System;
using System.Windows.Forms;
using POCOGenerator.POCOWriters;

namespace POCOGenerator
{
    public static class GeneratorWinFormsFactory
    {
        public static IGenerator GetGenerator(RichTextBox richTextBox)
        {
            if (richTextBox == null)
                throw new ArgumentNullException("richTextBox");

            return new Generator(WriterFactory.GetCreateWriterHandler(richTextBox));
        }

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
