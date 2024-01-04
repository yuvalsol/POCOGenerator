using System;
using System.Data.Common;
using System.Windows.Forms;

namespace POCOGeneratorUI.ConnectionDialog
{
    internal partial class DataConnectionPropertiesDialog : Form
    {
        public DataConnectionPropertiesDialog(DbConnectionStringBuilder builder)
        {
            InitializeComponent();
            this.builder = builder;
        }

        private readonly DbConnectionStringBuilder builder;

        private void DataConnectionPropertiesDialog_Load(object sender, EventArgs e)
        {
            propertyGridConnectionProperties.SelectedObject = builder;
            txtConnectionString.Text = builder.ConnectionString;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void propertyGridConnectionProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            txtConnectionString.Text = builder.ConnectionString;
        }
    }
}
