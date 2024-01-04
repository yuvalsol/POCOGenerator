using System;
using System.IO;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class TextWriterPOCOWriter : POCOWriter, IWriter
    {
        private readonly TextWriter textWriter;

        internal TextWriterPOCOWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public void Clear()
        {
            textWriter.Flush();
            SnapshotClear();
        }

        public void Write(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            textWriter.Write(text);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            textWriter.WriteLine();
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            textWriter.WriteLine(text);
            SnapshotWriteLine(text);
        }
    }
}
