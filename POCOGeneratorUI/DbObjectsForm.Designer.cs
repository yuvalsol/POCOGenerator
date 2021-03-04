namespace POCOGeneratorUI
{
    partial class DbObjectsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DbObjectsForm));
            this.chkTables = new System.Windows.Forms.CheckBox();
            this.chkViews = new System.Windows.Forms.CheckBox();
            this.chkProcedures = new System.Windows.Forms.CheckBox();
            this.chkFunctions = new System.Windows.Forms.CheckBox();
            this.chkTVPs = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkTables
            // 
            this.chkTables.AutoSize = true;
            this.chkTables.Checked = true;
            this.chkTables.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTables.Location = new System.Drawing.Point(12, 12);
            this.chkTables.Name = "chkTables";
            this.chkTables.Size = new System.Drawing.Size(58, 17);
            this.chkTables.TabIndex = 0;
            this.chkTables.Text = "Tables";
            this.chkTables.UseVisualStyleBackColor = true;
            // 
            // chkViews
            // 
            this.chkViews.AutoSize = true;
            this.chkViews.Location = new System.Drawing.Point(12, 37);
            this.chkViews.Name = "chkViews";
            this.chkViews.Size = new System.Drawing.Size(54, 17);
            this.chkViews.TabIndex = 1;
            this.chkViews.Text = "Views";
            this.chkViews.UseVisualStyleBackColor = true;
            // 
            // chkStoredProcedures
            // 
            this.chkProcedures.AutoSize = true;
            this.chkProcedures.Location = new System.Drawing.Point(12, 62);
            this.chkProcedures.Name = "chkStoredProcedures";
            this.chkProcedures.Size = new System.Drawing.Size(114, 17);
            this.chkProcedures.TabIndex = 2;
            this.chkProcedures.Text = "Stored Procedures";
            this.chkProcedures.UseVisualStyleBackColor = true;
            // 
            // chkTableFunctions
            // 
            this.chkFunctions.AutoSize = true;
            this.chkFunctions.Location = new System.Drawing.Point(12, 87);
            this.chkFunctions.Name = "chkTableFunctions";
            this.chkFunctions.Size = new System.Drawing.Size(137, 17);
            this.chkFunctions.TabIndex = 3;
            this.chkFunctions.Text = "Table-valued Functions";
            this.chkFunctions.UseVisualStyleBackColor = true;
            // 
            // chkTVPs
            // 
            this.chkTVPs.AutoSize = true;
            this.chkTVPs.Location = new System.Drawing.Point(12, 112);
            this.chkTVPs.Name = "chkTVPs";
            this.chkTVPs.Size = new System.Drawing.Size(150, 17);
            this.chkTVPs.TabIndex = 4;
            this.chkTVPs.Text = "User-Defined Table Types";
            this.chkTVPs.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(104, 147);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(32, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(142, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DbObjectsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(204, 182);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkTVPs);
            this.Controls.Add(this.chkFunctions);
            this.Controls.Add(this.chkProcedures);
            this.Controls.Add(this.chkViews);
            this.Controls.Add(this.chkTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbObjectsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Objects";
            this.Shown += new System.EventHandler(this.DbObjectsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkTables;
        private System.Windows.Forms.CheckBox chkViews;
        private System.Windows.Forms.CheckBox chkProcedures;
        private System.Windows.Forms.CheckBox chkFunctions;
        private System.Windows.Forms.CheckBox chkTVPs;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}