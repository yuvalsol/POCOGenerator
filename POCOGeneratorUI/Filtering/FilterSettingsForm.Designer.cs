namespace POCOGeneratorUI.Filtering
{
    partial class FilterSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterSettingsForm));
            this.lblFilterName = new System.Windows.Forms.Label();
            this.lblFilterSchema = new System.Windows.Forms.Label();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.txtFilterSchema = new System.Windows.Forms.TextBox();
            this.ddlFilterName = new System.Windows.Forms.ComboBox();
            this.ddlFilterSchema = new System.Windows.Forms.ComboBox();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFilterName
            // 
            this.lblFilterName.AutoSize = true;
            this.lblFilterName.Location = new System.Drawing.Point(12, 9);
            this.lblFilterName.Name = "lblFilterName";
            this.lblFilterName.Size = new System.Drawing.Size(35, 13);
            this.lblFilterName.TabIndex = 0;
            this.lblFilterName.Text = "Name";
            // 
            // lblFilterSchema
            // 
            this.lblFilterSchema.AutoSize = true;
            this.lblFilterSchema.Location = new System.Drawing.Point(12, 46);
            this.lblFilterSchema.Name = "lblFilterSchema";
            this.lblFilterSchema.Size = new System.Drawing.Size(46, 13);
            this.lblFilterSchema.TabIndex = 0;
            this.lblFilterSchema.Text = "Schema";
            // 
            // txtFilterName
            // 
            this.txtFilterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilterName.Location = new System.Drawing.Point(211, 5);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(180, 20);
            this.txtFilterName.TabIndex = 2;
            // 
            // txtFilterSchema
            // 
            this.txtFilterSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilterSchema.Location = new System.Drawing.Point(211, 42);
            this.txtFilterSchema.Name = "txtFilterSchema";
            this.txtFilterSchema.Size = new System.Drawing.Size(180, 20);
            this.txtFilterSchema.TabIndex = 4;
            // 
            // ddlFilterName
            // 
            this.ddlFilterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFilterName.FormattingEnabled = true;
            this.ddlFilterName.Location = new System.Drawing.Point(74, 5);
            this.ddlFilterName.Name = "ddlFilterName";
            this.ddlFilterName.Size = new System.Drawing.Size(121, 21);
            this.ddlFilterName.TabIndex = 1;
            // 
            // ddlFilterSchema
            // 
            this.ddlFilterSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFilterSchema.FormattingEnabled = true;
            this.ddlFilterSchema.Location = new System.Drawing.Point(74, 42);
            this.ddlFilterSchema.Name = "ddlFilterSchema";
            this.ddlFilterSchema.Size = new System.Drawing.Size(121, 21);
            this.ddlFilterSchema.TabIndex = 3;
            // 
            // btnClearFilter
            // 
            this.btnClearFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearFilter.AutoSize = true;
            this.btnClearFilter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClearFilter.Location = new System.Drawing.Point(60, 81);
            this.btnClearFilter.Name = "btnClearFilter";
            this.btnClearFilter.Size = new System.Drawing.Size(66, 23);
            this.btnClearFilter.TabIndex = 6;
            this.btnClearFilter.Text = "Clear Filter";
            this.btnClearFilter.UseVisualStyleBackColor = true;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(12, 81);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(32, 23);
            this.btnOK.TabIndex = 5;
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
            this.btnCancel.Location = new System.Drawing.Point(341, 81);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FilterSettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(404, 112);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClearFilter);
            this.Controls.Add(this.ddlFilterSchema);
            this.Controls.Add(this.ddlFilterName);
            this.Controls.Add(this.txtFilterSchema);
            this.Controls.Add(this.txtFilterName);
            this.Controls.Add(this.lblFilterSchema);
            this.Controls.Add(this.lblFilterName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Settings";
            this.Shown += new System.EventHandler(this.FilterSettingsForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilterName;
        private System.Windows.Forms.Label lblFilterSchema;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.TextBox txtFilterSchema;
        private System.Windows.Forms.ComboBox ddlFilterName;
        private System.Windows.Forms.ComboBox ddlFilterSchema;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}