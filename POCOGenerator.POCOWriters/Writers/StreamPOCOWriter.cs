using System;
using System.IO;
using System.Text;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class StreamPOCOWriter : POCOWriter, IWriter
    {
        private Stream stream;

        internal StreamPOCOWriter(Stream stream)
        {
            this.stream = stream;
        }

        private void StreamWrite(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            stream.Write(buffer, 0, buffer.Length);
        }

        private void StreamWriteLine(string text = null)
        {
            byte[] buffer = null;

            if (string.IsNullOrEmpty(text) == false)
            {
                buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
            }

            buffer = Encoding.UTF8.GetBytes(Environment.NewLine);
            stream.Write(buffer, 0, buffer.Length);
        }

        public void Clear()
        {
            stream.Position = 0;
            stream.SetLength(0);
            SnapshotClear();
        }

        public void Write(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            StreamWrite(text);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            StreamWriteLine();
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            StreamWriteLine(text);
            SnapshotWriteLine(text);
        }
    }
}
