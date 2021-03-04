using System;
using System.Drawing;
using Console = Colorful.Console;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class ConsoleColorPOCOWriter : POCOWriter, IWriter, ISyntaxHighlight
    {
        internal ConsoleColorPOCOWriter()
        {
        }

        public Color Text { get; set; }
        public Color Keyword { get; set; }
        public Color UserType { get; set; }
        public Color String { get; set; }
        public Color Comment { get; set; }
        public Color Error { get; set; }
        public Color Background { get; set; }

        public void Clear()
        {
            Console.BackgroundColor = this.Background;
            Console.Clear();
            SnapshotClear();
        }

        public void Write(string text)
        {
            Console.Write(text, this.Text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            Console.Write(text, this.Keyword);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            Console.Write(text, this.UserType);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            Console.Write(text, this.String);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            Console.Write(text, this.Comment);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            Console.Write(text, this.Error);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            Console.WriteLine();
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text, this.Text);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            Console.WriteLine(text, this.Keyword);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            Console.WriteLine(text, this.UserType);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            Console.WriteLine(text, this.String);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            Console.WriteLine(text, this.Comment);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            Console.WriteLine(text, this.Error);
            SnapshotWriteLine(text);
        }
    }
}
