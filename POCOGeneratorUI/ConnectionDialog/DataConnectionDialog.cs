using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Linq;
using System.Windows.Forms;
using POCOGenerator;

namespace POCOGeneratorUI.ConnectionDialog
{
    internal partial class DataConnectionDialog : Form
    {
        private DbConnectionStringBuilder builder;

        private IRDBMSHandler Handler
        {
            get
            {
                return (IRDBMSHandler)ddlRDBMS.SelectedItem;
            }
        }

        public DataConnectionDialog(RDBMS rdbms = RDBMS.SQLServer, string connectionString = null)
        {
            InitializeComponent();

            DisableEvents();

            BindDropDownRDBMS();

            if (rdbms != RDBMS.None)
                ddlRDBMS.SelectedValue = rdbms;

            ddlRDBMSCurrentIndex = ddlRDBMS.SelectedIndex;

            if (string.IsNullOrEmpty(connectionString))
            {
                builder = Handler.GetConnectionStringBuilder();
            }
            else
            {
                try
                {
                    builder = Handler.GetConnectionStringBuilder(connectionString);
                }
                catch
                {
                    builder = Handler.GetConnectionStringBuilder();
                }

                SetControlsFromConnectionStringBuilder();
            }

            EnableEvents();
        }

        private void DisableEvents()
        {
            ddlRDBMS.SelectedIndexChanged -= ddlRDBMS_SelectedIndexChanged;
            ddlServers.TextChanged -= ddlServers_TextChanged;
            rdbWindowsAuthentication.CheckedChanged -= rdbAuthentication_CheckedChanged;
            rdbServerAuthentication.CheckedChanged -= rdbAuthentication_CheckedChanged;
            txtUserName.TextChanged -= txtUserName_TextChanged;
            txtPassword.TextChanged -= txtPassword_TextChanged;
            ddlDatabases.TextChanged -= ddlDatabases_TextChanged;
        }

        private void EnableEvents()
        {
            ddlRDBMS.SelectedIndexChanged += ddlRDBMS_SelectedIndexChanged;
            ddlServers.TextChanged += ddlServers_TextChanged;
            rdbWindowsAuthentication.CheckedChanged += rdbAuthentication_CheckedChanged;
            rdbServerAuthentication.CheckedChanged += rdbAuthentication_CheckedChanged;
            txtUserName.TextChanged += txtUserName_TextChanged;
            txtPassword.TextChanged += txtPassword_TextChanged;
            ddlDatabases.TextChanged += ddlDatabases_TextChanged;
        }

        private void BindDropDownRDBMS()
        {
            ddlRDBMS.SelectedIndexChanged -= ddlRDBMS_SelectedIndexChanged;

            ddlRDBMS.DataSource = new BindingSource(RDBMSHandlerFactory.GetRDBMSHandlers().ToArray(), null);
            ddlRDBMS.ValueMember = "RDBMS";

            ddlRDBMS.SelectedIndexChanged += ddlRDBMS_SelectedIndexChanged;
        }

        private int ddlRDBMSCurrentIndex;

        private void ddlRDBMS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRDBMSCurrentIndex != ddlRDBMS.SelectedIndex)
            {
                builder = Handler.GetConnectionStringBuilder();
                Clear();
                ddlRDBMSCurrentIndex = ddlRDBMS.SelectedIndex;
            }
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

        private void Clear()
        {
            ddlServers.DataSource = null;
            ddlServers.Items.Clear();
            ddlServers.Text = null;

            txtUserName.Text = null;
            txtPassword.Text = null;

            ddlDatabases.DataSource = null;
            ddlDatabases.Items.Clear();
            ddlDatabases.Text = null;

            txtConnectionString.Text = null;

            SetAuthenticationControls();
            SetDatabaseControls();
        }

        private void ddlServers_TextChanged(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;

            SetDatabaseControls();
        }

        private void rdbAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdb = sender as RadioButton;
            if (rdb.Checked)
            {
                SetAuthenticationControls();
                txtConnectionString.Text = ConnectionString;
            }
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;
        }

        private void ddlDatabases_TextChanged(object sender, EventArgs e)
        {
            txtConnectionString.Text = ConnectionString;
        }

        private void SetConnectionStringBuilder()
        {
            try
            {
                Handler.SetConnectionStringBuilder(
                    builder,
                    ddlServers.Text,
                    rdbWindowsAuthentication.Checked,
                    (rdbServerAuthentication.Checked ? txtUserName.Text : null),
                    (rdbServerAuthentication.Checked ? txtPassword.Text : null),
                    ddlDatabases.Text
                );
            }
            catch
            {
                builder.Clear();
            }
        }

        private void SetControlsFromConnectionStringBuilder()
        {
            try
            {
                Clear();

                var handler = Handler;

                ddlServers.Text = handler.GetServer(builder);
                ddlDatabases.Text = handler.GetDatabase(builder);

                if (handler.GetIntegratedSecurity(builder))
                {
                    rdbWindowsAuthentication.Checked = true;
                    txtUserName.Text = null;
                    txtPassword.Text = null;
                }
                else
                {
                    rdbServerAuthentication.Checked = true;
                    txtUserName.Text = handler.GetUserID(builder);
                    txtPassword.Text = handler.GetPassword(builder);
                }

                txtConnectionString.Text = builder.ConnectionString;

                SetAuthenticationControls();
                SetDatabaseControls();
            }
            catch
            {
                Clear();
            }
        }

        private void SetAuthenticationControls()
        {
            lblUserName.Enabled = rdbServerAuthentication.Checked;
            txtUserName.Enabled = rdbServerAuthentication.Checked;
            lblPassword.Enabled = rdbServerAuthentication.Checked;
            txtPassword.Enabled = rdbServerAuthentication.Checked;
        }

        private void SetDatabaseControls()
        {
            bool enabled = string.IsNullOrEmpty(ddlServers.Text) == false;
            ddlDatabases.Enabled = enabled;
            lblDatabase.Enabled = enabled;
            btnRefreshDatabases.Enabled = enabled;
        }

        public RDBMS RDBMS
        {
            get
            {
                return Handler.RDBMS;
            }
        }

        public string ConnectionString
        {
            get
            {
                SetConnectionStringBuilder();
                return builder.ConnectionString;
            }
        }

        private void btnConnectionProperties_Click(object sender, EventArgs e)
        {
            var builder = Handler.GetConnectionStringBuilder(ConnectionString);
            if (new DataConnectionPropertiesDialog(builder).ShowDialog(this) == DialogResult.OK)
            {
                this.builder = builder;
                DisableEvents();
                SetControlsFromConnectionStringBuilder();
                EnableEvents();
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                    return;

                this.Cursor = Cursors.WaitCursor;

                var handler = Handler;
                var builder = handler.GetConnectionStringBuilder(connectionString);
                handler.SetConnectionTimeout(builder, 30);
                bool succeeded = handler.TestConnection(builder.ConnectionString);
                MessageBox.Show(this, (succeeded ? "Connection Test Succeeded" : "Connection Test Failed"), "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnRefreshServers_Click(object sender, EventArgs e)
        {
            try
            {
                if (Handler.RDBMS == RDBMS.SQLServer)
                {
                    this.Cursor = Cursors.WaitCursor;

                    DataTable dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
                    if (dataSources == null)
                        return;

                    List<string> servers = new List<string>();
                    foreach (DataRow row in dataSources.Rows)
                    {
                        string ServerName = row["ServerName"] as string;
                        if (string.IsNullOrEmpty(ServerName) == false)
                        {
                            string InstanceName = row["InstanceName"] as string;
                            if (string.IsNullOrEmpty(InstanceName))
                                servers.Add(ServerName);
                            else
                                servers.Add(ServerName + @"\" + InstanceName);
                        }
                    }

                    if (servers.Count > 0)
                    {
                        int serverIndex = -1;
                        string currentServer = ddlServers.Text;
                        if (string.IsNullOrEmpty(currentServer) == false)
                        {
                            for (int i = 0; i < servers.Count; i++)
                            {
                                var server = servers[i];
                                if (string.Compare(server, currentServer, true) == 0)
                                {
                                    serverIndex = i;
                                    break;
                                }
                            }
                        }

                        DisableEvents();

                        ddlServers.DataSource = null;
                        ddlServers.Items.Clear();
                        ddlServers.DataSource = servers;

                        if (serverIndex != -1)
                            ddlServers.SelectedIndex = serverIndex;
                        else if (string.IsNullOrEmpty(currentServer) == false)
                            ddlServers.Text = currentServer;
                        else
                            ddlServers.Text = null;

                        txtConnectionString.Text = ConnectionString;

                        SetDatabaseControls();

                        EnableEvents();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnRefreshDatabases_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                    return;

                this.Cursor = Cursors.WaitCursor;

                var handler = Handler;
                var builder = handler.GetConnectionStringBuilder(connectionString);
                handler.SetConnectionTimeout(builder, 30);
                handler.SetDatabase(builder, null);
                string[] databases = handler.GetDatabases(builder.ConnectionString);
                if (databases == null)
                    return;

                if (databases.Length > 0)
                {
                    int databaseIndex = -1;
                    string currentDatabase = ddlDatabases.Text;
                    if (string.IsNullOrEmpty(currentDatabase) == false)
                    {
                        for (int i = 0; i < databases.Length; i++)
                        {
                            var database = databases[i];
                            if (string.Compare(database, currentDatabase, true) == 0)
                            {
                                databaseIndex = i;
                                break;
                            }
                        }
                    }

                    EnableEvents();

                    ddlDatabases.DataSource = null;
                    ddlDatabases.Items.Clear();
                    ddlDatabases.DataSource = databases;

                    if (databaseIndex != -1)
                        ddlDatabases.SelectedIndex = databaseIndex;
                    else if (string.IsNullOrEmpty(currentDatabase) == false)
                        ddlDatabases.Text = currentDatabase;
                    else
                        ddlDatabases.Text = null;

                    txtConnectionString.Text = ConnectionString;

                    DisableEvents();
                }
            }
            catch
            {
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}