using System;
using System.Windows.Forms;

namespace POCOGeneratorUI.Filtering
{
    public partial class FilterSettingsForm : Form
    {
        public FilterSettingsForm(FilterSettings filterSettings, bool isSupportSchema)
        {
            InitializeComponent();

            SetFilterSettingsForm();
            SetSchemaSupport(isSupportSchema);
            SetFilter(filterSettings);
        }

        private class FilterItem
        {
            public FilterType FilterType { get; private set; }

            public FilterItem(FilterType filterType)
            {
                FilterType = filterType;
            }

            public override string ToString()
            {
                return FilterType.ToString().Replace('_', ' ');
            }
        }

        private void SetFilterSettingsForm()
        {
            ddlFilterName.Items.Add(new FilterItem(FilterType.Equals));
            ddlFilterName.Items.Add(new FilterItem(FilterType.Contains));
            ddlFilterName.Items.Add(new FilterItem(FilterType.Does_Not_Contain));

            ddlFilterSchema.Items.Add(new FilterItem(FilterType.Equals));
            ddlFilterSchema.Items.Add(new FilterItem(FilterType.Contains));
            ddlFilterSchema.Items.Add(new FilterItem(FilterType.Does_Not_Contain));

            ClearFilter();
        }

        private void SetSchemaSupport(bool isSupportSchema)
        {
            lblFilterSchema.Visible = isSupportSchema;
            ddlFilterSchema.Visible = isSupportSchema;
            txtFilterSchema.Visible = isSupportSchema;
            int height = 150;
            if (isSupportSchema == false)
                height -= 50;
            this.Height = height;
        }

        private void SetFilter(FilterSettings filterSettings)
        {
            SetFilterName(filterSettings.FilterName.FilterType, filterSettings.FilterName.Filter);
            SetFilterSchema(filterSettings.FilterSchema.FilterType, filterSettings.FilterSchema.Filter);
        }

        private void ClearFilter()
        {
            SetFilterName(FilterType.Contains, null);
            SetFilterSchema(FilterType.Contains, null);
        }

        private void SetFilterName(FilterType filterType, string value)
        {
            ddlFilterName.SelectedIndex = (int)filterType;
            txtFilterName.Text = value;
        }

        private void SetFilterSchema(FilterType filterType, string value)
        {
            ddlFilterSchema.SelectedIndex = (int)filterType;
            txtFilterSchema.Text = value;
        }

        public FilterType FilterTypeName
        {
            get { return ((FilterItem)ddlFilterName.SelectedItem).FilterType; }
        }

        public string FilterName
        {
            get { return txtFilterName.Text; }
        }

        public FilterType FilterTypeSchema
        {
            get { return ((FilterItem)ddlFilterSchema.SelectedItem).FilterType; }
        }

        public string FilterSchema
        {
            get { return txtFilterSchema.Text; }
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            ClearFilter();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FilterSettingsForm_Shown(object sender, EventArgs e)
        {
            txtFilterName.Focus();
        }
    }
}
