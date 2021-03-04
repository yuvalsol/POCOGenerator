using System;
using System.Windows.Forms;

namespace POCOGeneratorUI
{
    public partial class DbObjectsForm : Form
    {
        public DbObjectsForm(bool isSupportTableFunctions, bool isSupportTVPs, bool isEnableTables, bool isEnableViews, bool isEnableProcedures, bool isEnableFunctions, bool isEnableTVPs)
        {
            InitializeComponent();
            Init(isSupportTableFunctions, isSupportTVPs, isEnableTables, isEnableViews, isEnableProcedures, isEnableFunctions, isEnableTVPs);
        }

        private void Init(bool isSupportTableFunctions, bool isSupportTVPs, bool isEnableTables, bool isEnableViews, bool isEnableProcedures, bool isEnableFunctions, bool isEnableTVPs)
        {
            chkFunctions.Visible = isSupportTableFunctions;
            if (isSupportTableFunctions == false)
                this.Height -= 30;

            chkTVPs.Visible = isSupportTVPs;
            if (isSupportTVPs == false)
                this.Height -= 30;

            chkTables.Checked = isEnableTables;
            chkViews.Checked = isEnableViews;
            chkProcedures.Checked = isEnableProcedures;
            if (isSupportTableFunctions)
                chkFunctions.Checked = isEnableFunctions;
            if (isSupportTVPs)
                chkTVPs.Checked = isEnableTVPs;

            if ((isEnableTables || isEnableViews || isEnableProcedures || isEnableFunctions || isEnableTVPs) == false)
                chkTables.Checked = true;
        }

        private void DbObjectsForm_Shown(object sender, EventArgs e)
        {
            btnOK.Focus();
        }

        public bool IsEnableTables { get; private set; }
        public bool IsEnableViews { get; private set; }
        public bool IsEnableProcedures { get; private set; }
        public bool IsEnableFunctions { get; private set; }
        public bool IsEnableTVPs { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IsEnableTables = chkTables.Checked;
            IsEnableViews = chkViews.Checked;
            IsEnableProcedures = chkProcedures.Checked;
            IsEnableFunctions = chkFunctions.Visible && chkFunctions.Checked;
            IsEnableTVPs = chkTVPs.Visible && chkTVPs.Checked;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
