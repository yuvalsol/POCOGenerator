namespace POCOGeneratorUI.TypesMapping
{
    partial class TypesMappingForm
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
            this.txtTypeMappingEditor = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txtTypeMappingEditor
            // 
            this.txtTypeMappingEditor.BackColor = System.Drawing.Color.White;
            this.txtTypeMappingEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTypeMappingEditor.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtTypeMappingEditor.Location = new System.Drawing.Point(0, 0);
            this.txtTypeMappingEditor.Name = "txtTypeMappingEditor";
            this.txtTypeMappingEditor.ReadOnly = true;
            this.txtTypeMappingEditor.Size = new System.Drawing.Size(449, 607);
            this.txtTypeMappingEditor.TabIndex = 0;
            this.txtTypeMappingEditor.Text = "";
            // 
            // TypesMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 607);
            this.Controls.Add(this.txtTypeMappingEditor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TypesMappingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Types Mapping";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtTypeMappingEditor;
    }
}