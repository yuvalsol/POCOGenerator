using System;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using POCOGenerator;

namespace RichTextBoxDemo
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
        }

        private void DemoForm_Load(object sender, EventArgs e)
        {
            try { txtConnectionString.Text = File.ReadAllText("ConnectionString.txt"); } catch { }
            if (string.IsNullOrEmpty(txtConnectionString.Text))
                txtConnectionString.Text = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=AdventureWorks2014;Integrated Security=True";
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Clear();

            IGenerator generator = GeneratorWinFormsFactory.GetGenerator(txtPocoEditor);
            generator.Settings.ConnectionString = txtConnectionString.Text;
            generator.Settings.DatabaseObjects.Tables.IncludeAll = true;
            generator.Settings.POCO.CommentsWithoutNull = true;
            generator.Settings.ClassName.IncludeSchema = true;
            generator.Settings.ClassName.SchemaSeparator = "_";
            generator.Settings.ClassName.IgnoreDboSchema = true;
            generator.Settings.EFAnnotations.Enable = true;

            if (rdbLightTheme.Checked)
            {
                generator.Settings.SyntaxHighlight.Reset();
            }
            else if (rdbDarkTheme.Checked)
            {
                generator.Settings.SyntaxHighlight.Text = Color.FromArgb(255, 255, 255);
                generator.Settings.SyntaxHighlight.Keyword = Color.FromArgb(86, 156, 214);
                generator.Settings.SyntaxHighlight.UserType = Color.FromArgb(78, 201, 176);
                generator.Settings.SyntaxHighlight.String = Color.FromArgb(214, 157, 133);
                generator.Settings.SyntaxHighlight.Comment = Color.FromArgb(96, 139, 78);
                generator.Settings.SyntaxHighlight.Error = Color.FromArgb(255, 0, 0);
                generator.Settings.SyntaxHighlight.Background = Color.FromArgb(0, 0, 0);
            }

            GeneratorResults results = generator.Generate();

            AlertError(results);
        }

        private void AlertError(GeneratorResults results)
        {
            bool isError = (results & GeneratorResults.Error) == GeneratorResults.Error;
            bool isWarning = (results & GeneratorResults.Warning) == GeneratorResults.Warning;

            if (results == GeneratorResults.None || isWarning)
            {
                if (isWarning)
                    MessageBox.Show(this, string.Format("Warning Result: {0}", results), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (isError)
            {
                MessageBox.Show(this, string.Format("Error Result: {0}", results), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string text = txtPocoEditor.Text;
            if (string.IsNullOrEmpty(text))
                Clipboard.Clear();
            else
                Clipboard.SetText(text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            if (rdbLightTheme.Checked)
                txtPocoEditor.BackColor = Color.FromArgb(255, 255, 255);
            else if (rdbDarkTheme.Checked)
                txtPocoEditor.BackColor = Color.FromArgb(0, 0, 0);

            txtPocoEditor.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
