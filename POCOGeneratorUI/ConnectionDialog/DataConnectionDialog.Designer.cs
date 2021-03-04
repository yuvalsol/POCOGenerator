namespace POCOGeneratorUI.ConnectionDialog
{
    partial class DataConnectionDialog
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
            this.lblRDBMS = new System.Windows.Forms.Label();
            this.ddlRDBMS = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.ddlServers = new System.Windows.Forms.ComboBox();
            this.btnRefreshServers = new System.Windows.Forms.Button();
            this.rdbWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.rdbServerAuthentication = new System.Windows.Forms.RadioButton();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.ddlDatabases = new System.Windows.Forms.ComboBox();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConnectionProperties = new System.Windows.Forms.Button();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnRefreshDatabases = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRDBMS
            // 
            this.lblRDBMS.AutoSize = true;
            this.lblRDBMS.Location = new System.Drawing.Point(12, 9);
            this.lblRDBMS.Name = "lblRDBMS";
            this.lblRDBMS.Size = new System.Drawing.Size(49, 13);
            this.lblRDBMS.TabIndex = 0;
            this.lblRDBMS.Text = "RDBMS:";
            // 
            // ddlRDBMS
            // 
            this.ddlRDBMS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRDBMS.FormattingEnabled = true;
            this.ddlRDBMS.Location = new System.Drawing.Point(12, 25);
            this.ddlRDBMS.Name = "ddlRDBMS";
            this.ddlRDBMS.Size = new System.Drawing.Size(115, 21);
            this.ddlRDBMS.TabIndex = 1;
            this.ddlRDBMS.SelectedIndexChanged += new System.EventHandler(this.ddlRDBMS_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(12, 62);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 13);
            this.lblServer.TabIndex = 2;
            this.lblServer.Text = "Server:";
            // 
            // ddlServers
            // 
            this.ddlServers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlServers.FormattingEnabled = true;
            this.ddlServers.Location = new System.Drawing.Point(12, 78);
            this.ddlServers.Name = "ddlServers";
            this.ddlServers.Size = new System.Drawing.Size(310, 21);
            this.ddlServers.TabIndex = 3;
            this.ddlServers.TextChanged += new System.EventHandler(this.ddlServers_TextChanged);
            // 
            // btnRefreshServers
            // 
            this.btnRefreshServers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshServers.AutoSize = true;
            this.btnRefreshServers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefreshServers.Location = new System.Drawing.Point(328, 77);
            this.btnRefreshServers.Name = "btnRefreshServers";
            this.btnRefreshServers.Size = new System.Drawing.Size(54, 23);
            this.btnRefreshServers.TabIndex = 4;
            this.btnRefreshServers.Text = "Refresh";
            this.btnRefreshServers.UseVisualStyleBackColor = true;
            this.btnRefreshServers.Click += new System.EventHandler(this.btnRefreshServers_Click);
            // 
            // rdbWindowsAuthentication
            // 
            this.rdbWindowsAuthentication.AutoSize = true;
            this.rdbWindowsAuthentication.Checked = true;
            this.rdbWindowsAuthentication.Location = new System.Drawing.Point(12, 115);
            this.rdbWindowsAuthentication.Name = "rdbWindowsAuthentication";
            this.rdbWindowsAuthentication.Size = new System.Drawing.Size(140, 17);
            this.rdbWindowsAuthentication.TabIndex = 5;
            this.rdbWindowsAuthentication.TabStop = true;
            this.rdbWindowsAuthentication.Text = "Windows Authentication";
            this.rdbWindowsAuthentication.UseVisualStyleBackColor = true;
            this.rdbWindowsAuthentication.CheckedChanged += new System.EventHandler(this.rdbAuthentication_CheckedChanged);
            // 
            // rdbServerAuthentication
            // 
            this.rdbServerAuthentication.AutoSize = true;
            this.rdbServerAuthentication.Location = new System.Drawing.Point(12, 138);
            this.rdbServerAuthentication.Name = "rdbServerAuthentication";
            this.rdbServerAuthentication.Size = new System.Drawing.Size(127, 17);
            this.rdbServerAuthentication.TabIndex = 6;
            this.rdbServerAuthentication.Text = "Server Authentication";
            this.rdbServerAuthentication.UseVisualStyleBackColor = true;
            this.rdbServerAuthentication.CheckedChanged += new System.EventHandler(this.rdbAuthentication_CheckedChanged);
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(104, 161);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(278, 20);
            this.txtUserName.TabIndex = 7;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(104, 187);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(278, 20);
            this.txtPassword.TabIndex = 8;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Enabled = false;
            this.lblUserName.Location = new System.Drawing.Point(28, 165);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(63, 13);
            this.lblUserName.TabIndex = 9;
            this.lblUserName.Text = "User Name:";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Enabled = false;
            this.lblPassword.Location = new System.Drawing.Point(28, 191);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Password:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Enabled = false;
            this.lblDatabase.Location = new System.Drawing.Point(12, 223);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(56, 13);
            this.lblDatabase.TabIndex = 11;
            this.lblDatabase.Text = "Database:";
            // 
            // ddlDatabases
            // 
            this.ddlDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDatabases.Enabled = false;
            this.ddlDatabases.FormattingEnabled = true;
            this.ddlDatabases.Location = new System.Drawing.Point(12, 239);
            this.ddlDatabases.Name = "ddlDatabases";
            this.ddlDatabases.Size = new System.Drawing.Size(310, 21);
            this.ddlDatabases.TabIndex = 12;
            this.ddlDatabases.TextChanged += new System.EventHandler(this.ddlDatabases_TextChanged);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTestConnection.AutoSize = true;
            this.btnTestConnection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTestConnection.Location = new System.Drawing.Point(12, 387);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(95, 23);
            this.btnTestConnection.TabIndex = 13;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.AutoSize = true;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(294, 387);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(32, 23);
            this.btnOK.TabIndex = 14;
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
            this.btnCancel.Location = new System.Drawing.Point(332, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConnectionProperties
            // 
            this.btnConnectionProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectionProperties.AutoSize = true;
            this.btnConnectionProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnConnectionProperties.Location = new System.Drawing.Point(261, 266);
            this.btnConnectionProperties.Name = "btnConnectionProperties";
            this.btnConnectionProperties.Size = new System.Drawing.Size(121, 23);
            this.btnConnectionProperties.TabIndex = 17;
            this.btnConnectionProperties.Text = "Connection Properties";
            this.btnConnectionProperties.UseVisualStyleBackColor = true;
            this.btnConnectionProperties.Click += new System.EventHandler(this.btnConnectionProperties_Click);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(12, 311);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ReadOnly = true;
            this.txtConnectionString.Size = new System.Drawing.Size(370, 60);
            this.txtConnectionString.TabIndex = 18;
            // 
            // btnRefreshDatabases
            // 
            this.btnRefreshDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDatabases.AutoSize = true;
            this.btnRefreshDatabases.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRefreshDatabases.Enabled = false;
            this.btnRefreshDatabases.Location = new System.Drawing.Point(328, 238);
            this.btnRefreshDatabases.Name = "btnRefreshDatabases";
            this.btnRefreshDatabases.Size = new System.Drawing.Size(54, 23);
            this.btnRefreshDatabases.TabIndex = 19;
            this.btnRefreshDatabases.Text = "Refresh";
            this.btnRefreshDatabases.UseVisualStyleBackColor = true;
            this.btnRefreshDatabases.Click += new System.EventHandler(this.btnRefreshDatabases_Click);
            // 
            // DataConnectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 422);
            this.Controls.Add(this.btnRefreshDatabases);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.btnConnectionProperties);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.ddlDatabases);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.rdbServerAuthentication);
            this.Controls.Add(this.rdbWindowsAuthentication);
            this.Controls.Add(this.btnRefreshServers);
            this.Controls.Add(this.ddlServers);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.ddlRDBMS);
            this.Controls.Add(this.lblRDBMS);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataConnectionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Connection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRDBMS;
        private System.Windows.Forms.ComboBox ddlRDBMS;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.ComboBox ddlServers;
        private System.Windows.Forms.Button btnRefreshServers;
        private System.Windows.Forms.RadioButton rdbWindowsAuthentication;
        private System.Windows.Forms.RadioButton rdbServerAuthentication;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ComboBox ddlDatabases;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConnectionProperties;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnRefreshDatabases;
    }
}