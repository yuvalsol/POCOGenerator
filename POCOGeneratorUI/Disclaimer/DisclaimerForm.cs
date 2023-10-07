using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace POCOGeneratorUI.Disclaimer
{
    public partial class DisclaimerForm : Form
    {
        internal DisclaimerForm(string disclaimer)
        {
            InitializeComponent();
            this.BackColor = Color.White;
            warningIcon.Image = SystemIcons.Warning.ToBitmap();
            lblDisclaimer.Text = disclaimer;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}