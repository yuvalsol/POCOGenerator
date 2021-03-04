using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POCOGeneratorUI.TypesMapping
{
    public partial class TypesMappingForm : Form
    {
        internal TypesMappingForm(string rdbmsName, List<TypeMapping> mappings, int width, int height)
        {
            InitializeComponent();
            InitForm(rdbmsName, mappings, width, height);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitForm(string rdbmsName, List<TypeMapping> mappings, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            txtTypeMappingEditor.Clear();
            txtTypeMappingEditor.Lines = null;
            Application.DoEvents();

            int lengthFrom = mappings.Max(m => m.FromType.Sum(x => x.Text.Length));
            int lengthTo = mappings.Max(m => m.ToType.Sum(x => x.Text.Length));

            string lineFrom = new string('─', lengthFrom);
            string lineTo = new string('─', lengthTo);
            string lineTop = string.Format("┌─{0}─┬─{1}─┐", lineFrom, lineTo);
            string lineMiddle = string.Format("├─{0}─┼─{1}─┤", lineFrom, lineTo);
            string lineBottom = string.Format("└─{0}─┴─{1}─┘", lineFrom, lineTo);

            string formatFrom = string.Format("{{0, -{0}}}", lengthFrom);
            string formatTo = string.Format("{{0, -{0}}}", lengthTo);

            txtTypeMappingEditor.AppendText(lineTop);
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText("│ ");
            AppendHeader(string.Format(formatFrom, rdbmsName.PadLeft(rdbmsName.Length + ((lengthFrom - rdbmsName.Length) / 2))));
            txtTypeMappingEditor.AppendText(" │ ");
            AppendHeader(string.Format(formatTo, ".NET".PadLeft(".NET".Length + ((lengthTo - ".NET".Length) / 2))));
            txtTypeMappingEditor.AppendText(" │");
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            txtTypeMappingEditor.AppendText(lineMiddle);
            txtTypeMappingEditor.AppendText(Environment.NewLine);

            foreach (var mapping in mappings)
            {
                txtTypeMappingEditor.AppendText("│ ");
                AppendParts(mapping.FromType, formatFrom, lengthFrom);
                txtTypeMappingEditor.AppendText(" │ ");
                AppendParts(mapping.ToType, formatTo, lengthTo);
                txtTypeMappingEditor.AppendText(" │");
                txtTypeMappingEditor.AppendText(Environment.NewLine);
            }

            txtTypeMappingEditor.AppendText(lineBottom);
        }

        private void AppendParts(IEnumerable<TypeMappingPart> mappingParts, string format, int length)
        {
            int count = mappingParts.Count();
            if (count == 1)
            {
                var part = mappingParts.Last();
                AppendText(string.Format(format, part.Text), part.SyntaxColor);
            }
            else if (count > 1)
            {
                var parts = mappingParts.TakeWhile((m, i) => i < count - 1);
                foreach (var part in parts)
                    AppendText(part.Text, part.SyntaxColor);
                var lengthUsed = parts.Sum(m => m.Text.Length);
                var lastPart = mappingParts.Last();
                AppendText(string.Format(string.Format("{{0, -{0}}}", length - lengthUsed), lastPart.Text), lastPart.SyntaxColor);
            }
        }

        private void AppendHeader(string text)
        {
            txtTypeMappingEditor.Select(txtTypeMappingEditor.TextLength, 0);
            txtTypeMappingEditor.SelectionFont = new Font(txtTypeMappingEditor.Font, FontStyle.Bold);
            txtTypeMappingEditor.SelectedText = text;
        }

        private void AppendText(string text, Color color)
        {
            txtTypeMappingEditor.Select(txtTypeMappingEditor.TextLength, 0);
            txtTypeMappingEditor.SelectionColor = color;
            txtTypeMappingEditor.SelectedText = text;
            txtTypeMappingEditor.SelectionColor = Color.Black;
        }
    }
}