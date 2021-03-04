using System;
using System.Drawing;
using System.Windows.Forms;

namespace POCOGenerator.POCOWriters.Writers
{
    internal class RichTextBoxPOCOWriter : POCOWriter, IWriter, ISyntaxHighlight
    {
        private RichTextBox richTextBox;

        internal RichTextBoxPOCOWriter(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
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
            richTextBox.BackColor = this.Background;
            richTextBox.Clear();
            SnapshotClear();
        }

        private void AppendText(string text, Color color)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                richTextBox.Select(richTextBox.TextLength, 0);
                richTextBox.SelectionColor = color;
                richTextBox.SelectedText = text;
                richTextBox.SelectionColor = this.Text;
            }
        }

        public void Write(string text)
        {
            AppendText(text, this.Text);
            SnapshotWrite(text);
        }

        public void WriteKeyword(string text)
        {
            AppendText(text, this.Keyword);
            SnapshotWrite(text);
        }

        public void WriteUserType(string text)
        {
            AppendText(text, this.UserType);
            SnapshotWrite(text);
        }

        public void WriteString(string text)
        {
            AppendText(text, this.String);
            SnapshotWrite(text);
        }

        public void WriteComment(string text)
        {
            AppendText(text, this.Comment);
            SnapshotWrite(text);
        }

        public void WriteError(string text)
        {
            AppendText(text, this.Error);
            SnapshotWrite(text);
        }

        public void WriteLine()
        {
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine();
        }

        public void WriteLine(string text)
        {
            AppendText(text, this.Text);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }

        public void WriteLineKeyword(string text)
        {
            AppendText(text, this.Keyword);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }

        public void WriteLineUserType(string text)
        {
            AppendText(text, this.UserType);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }

        public void WriteLineString(string text)
        {
            AppendText(text, this.String);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }

        public void WriteLineComment(string text)
        {
            AppendText(text, this.Comment);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }

        public void WriteLineError(string text)
        {
            AppendText(text, this.Error);
            richTextBox.AppendText(Environment.NewLine);
            SnapshotWriteLine(text);
        }
    }
}
