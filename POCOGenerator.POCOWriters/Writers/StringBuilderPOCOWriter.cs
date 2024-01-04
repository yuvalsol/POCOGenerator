using System;
using System.Text;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class StringBuilderPOCOWriter : POCOWriter, IWriter
    {
        private readonly StringBuilder stringBuilder;

        internal StringBuilderPOCOWriter(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
        }

        public void Clear()
        {
            stringBuilder.Clear();
            SnapshotClear();
        }

        public void Write(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            stringBuilder.Append(text);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            stringBuilder.AppendLine();
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            stringBuilder.AppendLine(text);
            SnapshotWriteLine(text);
        }
    }
}
