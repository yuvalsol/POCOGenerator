using System;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class ConsolePOCOWriter : POCOWriter, IWriter
    {
        internal ConsolePOCOWriter()
        {
        }

        public void Clear()
        {
            Console.Clear();
            SnapshotClear();
        }

        public void Write(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            Console.Write(text);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            Console.WriteLine();
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            Console.WriteLine(text);
            SnapshotWriteLine(text);
        }
    }
}
