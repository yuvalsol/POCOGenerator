using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using POCOGenerator.POCOWriters.Writers;

namespace POCOGenerator.POCOWriters
{
    public static class WriterFactory
    {
        public static Func<IWriter> GetCreateConsoleColorWriterHandler()
        {
            return () => { return new ConsoleColorPOCOWriter(); };
        }

        public static Func<IWriter> GetCreateConsoleWriterHandler()
        {
            return () => { return new ConsolePOCOWriter(); };
        }

        public static Func<IWriter> GetCreateEmptyWriterHandler()
        {
            return () => { return new EmptyPOCOWriter(); };
        }

        public static Func<IWriter> GetCreateWriterHandler(RichTextBox richTextBox)
        {
            return () => { return new RichTextBoxPOCOWriter(richTextBox); };
        }

        public static Func<IWriter> GetCreateWriterHandler(Stream stream)
        {
            return () => { return new StreamPOCOWriter(stream); };
        }

        public static Func<IWriter> GetCreateWriterHandler(StringBuilder stringBuilder)
        {
            return () => { return new StringBuilderPOCOWriter(stringBuilder); };
        }

        public static Func<IWriter> GetCreateWriterHandler(TextWriter textWriter)
        {
            return () => { return new TextWriterPOCOWriter(textWriter); };
        }
    }
}
