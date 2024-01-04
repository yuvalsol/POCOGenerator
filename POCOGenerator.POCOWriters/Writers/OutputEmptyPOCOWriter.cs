using System;
using System.Text;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class OutputEmptyPOCOWriter : POCOWriter, IWriter
    {
        internal OutputEmptyPOCOWriter()
        {
        }

        public void Clear()
        {
            SnapshotClear();
        }

        public void Write(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            SnapshotWriteLine(text);
        }
    }
}
