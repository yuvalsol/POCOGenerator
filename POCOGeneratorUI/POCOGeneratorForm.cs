using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using POCOGenerator;
using POCOGenerator.Objects;
using POCOGeneratorUI.ConnectionDialog;
using POCOGeneratorUI.Filtering;
using POCOGeneratorUI.TypesMapping;

namespace POCOGeneratorUI
{
    public partial class POCOGeneratorForm : Form
    {
        #region Form

        public POCOGeneratorForm()
        {
            InitializeComponent();
            GetControlsOriginalLocation();
            this.Text += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }

        private void POCOGeneratorForm_Load(object sender, EventArgs e)
        {
            LoadUISettings();
        }

        private void POCOGeneratorForm_Shown(object sender, EventArgs e)
        {
            ShowDisclaimer();
        }

        private void POCOGeneratorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerializeUISettings();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GroupBox_Paint(object sender, PaintEventArgs e)
        {
            (sender as GroupBox).DrawGroupBox(e.Graphics, this.BackColor, Color.Black, SystemColors.ActiveBorder, FontStyle.Bold);
        }

        #endregion

        #region Connect

        private RDBMS rdbms;
        private string connectionString;
        private IGenerator generator;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            RDBMS rdbms = this.rdbms;
            string connectionString = this.connectionString;

            Exception error = null;
            bool succeeded = GetConnectionString(ref rdbms, ref connectionString, ref error);
            if (succeeded)
            {
                if (rdbms != RDBMS.None && string.IsNullOrEmpty(connectionString) == false)
                {
                    IGenerator generator = GetGenerator(rdbms, connectionString);

                    if (generator != null)
                    {
                        this.rdbms = rdbms;
                        this.connectionString = connectionString;
                        this.generator = generator;

                        BuildServerTree();
                    }
                }
            }
            else
            {
                string errorMessage = "Failed to retrieve connection string.";
                if (error != null)
                    errorMessage += Environment.NewLine + error.Message;
                MessageBox.Show(this, errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool GetConnectionString(ref RDBMS rdbms, ref string connectionString, ref Exception error)
        {
            try
            {
                DataConnectionDialog dcd = new DataConnectionDialog(rdbms, connectionString);
                if (dcd.ShowDialog(this) == DialogResult.OK)
                {
                    rdbms = dcd.RDBMS;
                    connectionString = dcd.ConnectionString;
                }
                else
                {
                    rdbms = RDBMS.None;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex;
                return false;
            }
        }

        #endregion

        #region Generator

        private IGenerator GetGenerator(RDBMS rdbms, string connectionString)
        {
            bool isSupportTableFunctions = (rdbms == RDBMS.SQLServer);
            bool isSupportTVPs = (rdbms == RDBMS.SQLServer);

            DbObjectsForm dbObjectsForm = new DbObjectsForm(
                isSupportTableFunctions,
                isSupportTVPs,
                this.dbObjectsForm_IsEnableTables,
                this.dbObjectsForm_IsEnableViews,
                this.dbObjectsForm_IsEnableProcedures,
                this.dbObjectsForm_IsEnableFunctions,
                this.dbObjectsForm_IsEnableTVPs
            );

            if (dbObjectsForm.ShowDialog(this) == DialogResult.OK)
            {
                if (dbObjectsForm.IsEnableTables || dbObjectsForm.IsEnableViews || dbObjectsForm.IsEnableProcedures || dbObjectsForm.IsEnableFunctions || dbObjectsForm.IsEnableTVPs)
                {
                    IGenerator generator = GeneratorWinFormsFactory.GetGenerator(txtPocoEditor);
                    generator.Settings.ConnectionString = connectionString;
                    generator.Settings.RDBMS = rdbms;

                    generator.Settings.IncludeAll = false;
                    generator.Settings.Tables.IncludeAll = dbObjectsForm.IsEnableTables;
                    generator.Settings.Views.IncludeAll = dbObjectsForm.IsEnableViews;
                    generator.Settings.StoredProcedures.IncludeAll = dbObjectsForm.IsEnableProcedures;
                    generator.Settings.Functions.IncludeAll = dbObjectsForm.IsEnableFunctions;
                    generator.Settings.TVPs.IncludeAll = dbObjectsForm.IsEnableTVPs;

                    this.dbObjectsForm_IsEnableTables = dbObjectsForm.IsEnableTables;
                    this.dbObjectsForm_IsEnableViews = dbObjectsForm.IsEnableViews;
                    this.dbObjectsForm_IsEnableProcedures = dbObjectsForm.IsEnableProcedures;
                    this.dbObjectsForm_IsEnableFunctions = dbObjectsForm.IsEnableFunctions;
                    this.dbObjectsForm_IsEnableTVPs = dbObjectsForm.IsEnableTVPs;

                    return generator;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void SetGeneratorSettings()
        {
            // POCO
            this.generator.Settings.POCO.Properties = rdbProperties.Checked;
            this.generator.Settings.POCO.Fields = rdbFields.Checked;
            this.generator.Settings.POCO.VirtualProperties = chkVirtualProperties.Checked;
            this.generator.Settings.POCO.OverrideProperties = chkOverrideProperties.Checked;
            this.generator.Settings.POCO.PartialClass = chkPartialClass.Checked;
            this.generator.Settings.POCO.StructTypesNullable = chkStructTypesNullable.Checked;
            this.generator.Settings.POCO.Comments = chkComments.Checked;
            this.generator.Settings.POCO.CommentsWithoutNull = chkCommentsWithoutNull.Checked;
            this.generator.Settings.POCO.Using = chkUsing.Checked;
            this.generator.Settings.POCO.UsingInsideNamespace = chkUsingInsideNamespace.Checked;
            this.generator.Settings.POCO.Namespace = txtNamespace.Text;
            this.generator.Settings.POCO.WrapAroundEachClass = false;
            this.generator.Settings.POCO.Inherit = txtInherit.Text;
            this.generator.Settings.POCO.ColumnDefaults = chkColumnDefaults.Checked;
            this.generator.Settings.POCO.NewLineBetweenMembers = chkNewLineBetweenMembers.Checked;
            this.generator.Settings.POCO.EnumSQLTypeToString = rdbEnumSQLTypeToString.Checked;
            this.generator.Settings.POCO.EnumSQLTypeToEnumUShort = rdbEnumSQLTypeToEnumUShort.Checked;
            this.generator.Settings.POCO.EnumSQLTypeToEnumInt = rdbEnumSQLTypeToEnumInt.Checked;
            this.generator.Settings.POCO.Tab = "    ";

            // Navigation Properties
            this.generator.Settings.NavigationProperties.Enable = chkNavigationProperties.Checked;
            this.generator.Settings.NavigationProperties.VirtualNavigationProperties = chkVirtualNavigationProperties.Checked;
            this.generator.Settings.NavigationProperties.OverrideNavigationProperties = chkOverrideNavigationProperties.Checked;
            this.generator.Settings.NavigationProperties.ShowManyToManyJoinTable = chkShowManyToManyJoinTable.Checked;
            this.generator.Settings.NavigationProperties.Comments = chkNavigationPropertiesComments.Checked;
            this.generator.Settings.NavigationProperties.ListNavigationProperties = rdbListNavigationProperties.Checked;
            this.generator.Settings.NavigationProperties.ICollectionNavigationProperties = rdbICollectionNavigationProperties.Checked;
            this.generator.Settings.NavigationProperties.IEnumerableNavigationProperties = rdbIEnumerableNavigationProperties.Checked;

            // Class Name
            this.generator.Settings.ClassName.Singular = chkSingular.Checked;
            this.generator.Settings.ClassName.IncludeDB = chkIncludeDB.Checked;
            this.generator.Settings.ClassName.DBSeparator = txtDBSeparator.Text;
            this.generator.Settings.ClassName.IncludeSchema = chkIncludeSchema.Checked;
            this.generator.Settings.ClassName.IgnoreDboSchema = chkIgnoreDboSchema.Checked;
            this.generator.Settings.ClassName.SchemaSeparator = txtSchemaSeparator.Text;
            this.generator.Settings.ClassName.WordsSeparator = txtWordsSeparator.Text;
            this.generator.Settings.ClassName.CamelCase = chkCamelCase.Checked;
            this.generator.Settings.ClassName.UpperCase = chkUpperCase.Checked;
            this.generator.Settings.ClassName.LowerCase = chkLowerCase.Checked;
            this.generator.Settings.ClassName.Search = txtSearch.Text;
            this.generator.Settings.ClassName.Replace = txtReplace.Text;
            this.generator.Settings.ClassName.SearchIgnoreCase = chkSearchIgnoreCase.Checked;
            this.generator.Settings.ClassName.FixedClassName = txtFixedClassName.Text;
            this.generator.Settings.ClassName.Prefix = txtPrefix.Text;
            this.generator.Settings.ClassName.Suffix = txtSuffix.Text;

            // EF Annotations
            this.generator.Settings.EFAnnotations.Enable = chkEF.Checked;
            this.generator.Settings.EFAnnotations.Column = chkEFColumn.Checked;
            this.generator.Settings.EFAnnotations.Required = chkEFRequired.Checked;
            this.generator.Settings.EFAnnotations.RequiredWithErrorMessage = chkEFRequiredWithErrorMessage.Checked;
            this.generator.Settings.EFAnnotations.ConcurrencyCheck = chkEFConcurrencyCheck.Checked;
            this.generator.Settings.EFAnnotations.StringLength = chkEFStringLength.Checked;
            this.generator.Settings.EFAnnotations.Display = chkEFDisplay.Checked;
            this.generator.Settings.EFAnnotations.Description = chkEFDescription.Checked;
            this.generator.Settings.EFAnnotations.ComplexType = chkEFComplexType.Checked;
            this.generator.Settings.EFAnnotations.Index = chkEFIndex.Checked;
            this.generator.Settings.EFAnnotations.ForeignKeyAndInverseProperty = chkEFForeignKeyAndInverseProperty.Checked;
        }

        #endregion

        #region Build Server Tree

        private enum ImageType
        {
            Server,
            Database,
            Folder,
            Table,
            View,
            Procedure,
            Function,
            TVP,
            Column,
            PrimaryKey,
            ForeignKey,
            UniqueKey,
            Index
        }

        private void BuildServerTree()
        {
            try
            {
                txtPocoEditor.Clear();

                DisableServerTree();
                filters.Clear();
                trvServer.Nodes.Clear();

                SetStatusMessage("Building...");

                Server server = null;
                void serverBuilt(object sender, ServerBuiltEventArgs e)
                {
                    server = e.Server;
                    e.Stop = true;
                }

                this.generator.ServerBuilt += serverBuilt;

                GeneratorResults results = this.generator.Generate();

                this.generator.ServerBuilt -= serverBuilt;

                if (results != GeneratorResults.None)
                {
                    HandleGeneratorError(results);
                    return;
                }

                TreeNode serverNode = BuildServerNode(server);
                trvServer.Nodes.Add(serverNode);
                Application.DoEvents();

                foreach (var database in server.Databases)
                {
                    TreeNode databaseNode = BuildDatabaseNode(database);
                    serverNode.Nodes.AddSorted(databaseNode);

                    EnableServerTree();
                    serverNode.Expand();
                    DisableServerTree();
                    Application.DoEvents();

                    BuildDbObjectsNode(databaseNode, "Tables", database.Tables);
                    BuildDbObjectsNode(databaseNode, "Views", database.Views);
                    BuildDbObjectsNode(databaseNode, "Stored Procedures", database.Procedures);
                    BuildDbObjectsNode(databaseNode, "Table-valued Functions", database.Functions);
                    BuildDbObjectsNode(databaseNode, "User-Defined Table Types", database.TVPs);

                    EnableServerTree();
                    databaseNode.Expand();
                    DisableServerTree();
                    Application.DoEvents();
                }

                trvServer.SelectedNode = serverNode;

                EnableServerTree();
                Application.DoEvents();

                SetFormControls(this.generator.Support.SupportSchema, this.generator.Support.SupportTVPs, this.generator.Support.SupportEnumDataType);

                ClearStatus();

                trvServer.Focus();
            }
            catch (Exception ex)
            {
                SetStatusErrorMessage("Error. " + (ex.Message + (ex.InnerException != null ? " " + ex.InnerException.Message : string.Empty)).Replace(Environment.NewLine, " "));
            }
        }

        private void HandleGeneratorError(GeneratorResults results)
        {
            string errorMessage = "Error";
            if ((results & GeneratorResults.NoDbObjectsIncluded) == GeneratorResults.NoDbObjectsIncluded)
                errorMessage = "No Database Objects Included";
            else if ((results & GeneratorResults.ConnectionStringMissing) == GeneratorResults.ConnectionStringMissing)
                errorMessage = "Connection String Missing";
            else if ((results & GeneratorResults.ConnectionStringMalformed) == GeneratorResults.ConnectionStringMalformed)
                errorMessage = "Connection String Malformed";
            else if ((results & GeneratorResults.ConnectionStringNotMatchAnyRDBMS) == GeneratorResults.ConnectionStringNotMatchAnyRDBMS)
                errorMessage = "Connection String Not Match Any RDBMS";
            else if ((results & GeneratorResults.ConnectionStringMatchMoreThanOneRDBMS) == GeneratorResults.ConnectionStringMatchMoreThanOneRDBMS)
                errorMessage = "Connection String Match More Than One RDBMS";
            else if ((results & GeneratorResults.ServerNotResponding) == GeneratorResults.ServerNotResponding)
                errorMessage = "Server Not Responding";
            else if ((results & GeneratorResults.UnexpectedError) == GeneratorResults.UnexpectedError)
                errorMessage = "Unexpected Error";
            else if ((results & GeneratorResults.Error) == GeneratorResults.Error)
                errorMessage = "Error";
            else if ((results & GeneratorResults.Warning) == GeneratorResults.Warning)
                errorMessage = "Warning";

            if (this.generator.Error != null)
                errorMessage += ". " + Environment.NewLine + this.generator.Error.Message;

            errorMessage += ". " + Environment.NewLine + this.generator.Settings.ConnectionString;

            SetStatusErrorMessage(errorMessage.Replace(Environment.NewLine, " "));

            bool isWarning = ((results & GeneratorResults.Warning) == GeneratorResults.Warning);
            MessageBox.Show(this, errorMessage, (isWarning ? "Warning" : "Error"), MessageBoxButtons.OK, (isWarning ? MessageBoxIcon.Warning : MessageBoxIcon.Error));
        }

        private TreeNode BuildServerNode(Server server)
        {
            return new TreeNode(server.ToStringWithVersion())
            {
                Tag = server,
                ImageIndex = (int)ImageType.Server,
                SelectedImageIndex = (int)ImageType.Server
            };
        }

        private TreeNode BuildDatabaseNode(Database database)
        {
            TreeNode databaseNode = new TreeNode(database.ToString())
            {
                Tag = database,
                ImageIndex = (int)ImageType.Database,
                SelectedImageIndex = (int)ImageType.Database
            };
            if (database.Errors.Any())
                databaseNode.ForeColor = Color.Red;
            return databaseNode;
        }

        private void BuildDbObjectsNode(TreeNode databaseNode, string dbObjectsName, IEnumerable<IDbObject> dbObjects)
        {
            if (dbObjects.Any())
            {
                TreeNode node = new TreeNode(dbObjectsName)
                {
                    Tag = dbObjects,
                    ImageIndex = (int)ImageType.Folder,
                    SelectedImageIndex = (int)ImageType.Folder
                };
                databaseNode.Nodes.Add(node);
                node.ShowPlus();
            }
        }

        private void trvServer_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                return;
            }
            else if (e.Node.Tag == null)
            {
                return;
            }
            else if (e.Node.Tag is Server)
            {
                return;
            }
            else if (e.Node.Tag is Database)
            {
                return;
            }
            else if (e.Node.Tag is IEnumerable<Table>)
            {
                BuildDbGroup<Table>(e.Node, ImageType.Table, this.isCheckedTables);
            }
            else if (e.Node.Tag is IEnumerable<POCOGenerator.Objects.View>)
            {
                BuildDbGroup<POCOGenerator.Objects.View>(e.Node, ImageType.View, this.isCheckedViews);
            }
            else if (e.Node.Tag is IEnumerable<Procedure>)
            {
                BuildDbGroup<Procedure>(e.Node, ImageType.Procedure, this.isCheckedProcedures);
            }
            else if (e.Node.Tag is IEnumerable<Function>)
            {
                BuildDbGroup<Function>(e.Node, ImageType.Function, this.isCheckedFunctions);
            }
            else if (e.Node.Tag is IEnumerable<TVP>)
            {
                BuildDbGroup<TVP>(e.Node, ImageType.TVP, this.isCheckedTVPs);
            }
            else if (e.Node.Tag is Table)
            {
                Table table = e.Node.Tag as Table;
                BuildColumnsNode(e.Node, table.TableColumns);
                BuildPrimaryKeyNode(e.Node, table.PrimaryKey);
                BuildUniqueKeysNode(e.Node, table.UniqueKeys);
                BuildForeignKeysNode(e.Node, table.ForeignKeys);
                BuildIndexesNode(e.Node, table.Indexes);
            }
            else if (e.Node.Tag is POCOGenerator.Objects.View)
            {
                POCOGenerator.Objects.View view = e.Node.Tag as POCOGenerator.Objects.View;
                BuildColumnsNode(e.Node, view.ViewColumns);
                BuildIndexesNode(e.Node, view.Indexes);
            }
            else if (e.Node.Tag is Procedure)
            {
                Procedure procedure = e.Node.Tag as Procedure;
                BuildParametersNode(e.Node, procedure.ProcedureParameters);
                BuildColumnsNode(e.Node, procedure.ProcedureColumns);
            }
            else if (e.Node.Tag is Function)
            {
                Function function = e.Node.Tag as Function;
                BuildParametersNode(e.Node, function.FunctionParameters);
                BuildColumnsNode(e.Node, function.FunctionColumns);
            }
            else if (e.Node.Tag is TVP)
            {
                TVP tvp = e.Node.Tag as TVP;
                BuildColumnsNode(e.Node, tvp.TVPColumns);
            }
            else if (e.Node.Tag is IEnumerable<TableColumn>)
            {
                BuildTableColumns(e);
            }
            else if (e.Node.Tag is IEnumerable<PrimaryKey>)
            {
                BuildTablePrimaryKey(e.Node);
            }
            else if (e.Node.Tag is PrimaryKey)
            {
                BuildTablePrimaryKeyColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<UniqueKey>)
            {
                BuildTableUniqueKeys(e.Node);
            }
            else if (e.Node.Tag is UniqueKey)
            {
                BuildTableUniqueKeyColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<ForeignKey>)
            {
                BuildTableForeignKeys(e.Node);
            }
            else if (e.Node.Tag is ForeignKey)
            {
                BuildTableForeignKeyColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<TableIndex>)
            {
                BuildTableIndexes(e.Node);
            }
            else if (e.Node.Tag is TableIndex)
            {
                BuildTableIndexColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<ViewColumn>)
            {
                BuildViewColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<ViewIndex>)
            {
                BuildViewIndexes(e.Node);
            }
            else if (e.Node.Tag is ViewIndex)
            {
                BuildViewIndexColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<ProcedureParameter>)
            {
                BuildProcedureParameters(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<ProcedureColumn>)
            {
                BuildProcedureColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<FunctionParameter>)
            {
                BuildFunctionParameters(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<FunctionColumn>)
            {
                BuildFunctionColumns(e.Node);
            }
            else if (e.Node.Tag is IEnumerable<TVPColumn>)
            {
                BuildTVPColumns(e.Node);
            }
        }

        private void BuildDbGroup<T>(TreeNode node, ImageType imageType, bool isChecked) where T : IDbObject
        {
            string text = node.Text;
            node.Text += " (expanding...)";
            Application.DoEvents();

            var items = node.Tag as IEnumerable<T>;
            foreach (var item in items)
            {
                TreeNode itemNode = new TreeNode(item.ToString())
                {
                    Tag = item,
                    ImageIndex = (int)imageType,
                    SelectedImageIndex = (int)imageType
                };
                if (string.IsNullOrEmpty(item.Error) == false)
                    itemNode.ForeColor = Color.Red;
                node.Nodes.AddSorted(itemNode);
                itemNode.ShowPlus();
                if (isChecked)
                {
                    trvServer.AfterCheck -= trvServer_AfterCheck;
                    itemNode.Checked = true;
                    trvServer.AfterCheck += trvServer_AfterCheck;
                }
            }

            node.Text = text;
            Application.DoEvents();
        }

        private void BuildColumnsNode(TreeNode node, IEnumerable<IDbColumn> columns)
        {
            TreeNode columnsNode = new TreeNode("Columns")
            {
                Tag = columns,
                ImageIndex = (int)ImageType.Folder,
                SelectedImageIndex = (int)ImageType.Folder
            };
            node.Nodes.Add(columnsNode);
            if (columns.Any())
                columnsNode.ShowPlus();
        }

        private void BuildParametersNode(TreeNode node, IEnumerable<IDbParameter> parameters)
        {
            TreeNode parametersNode = new TreeNode("Parameters")
            {
                Tag = parameters,
                ImageIndex = (int)ImageType.Folder,
                SelectedImageIndex = (int)ImageType.Folder
            };
            node.Nodes.Add(parametersNode);
            if (parameters.Any())
                parametersNode.ShowPlus();
        }

        private void BuildPrimaryKeyNode(TreeNode node, PrimaryKey primaryKey)
        {
            if (primaryKey != null)
            {
                TreeNode primaryKeyNode = new TreeNode("Primary Key")
                {
                    Tag = new PrimaryKey[] { primaryKey },
                    ImageIndex = (int)ImageType.Folder,
                    SelectedImageIndex = (int)ImageType.Folder
                };
                node.Nodes.Add(primaryKeyNode);
                primaryKeyNode.ShowPlus();
            }
        }

        private void BuildUniqueKeysNode(TreeNode node, IEnumerable<UniqueKey> uniqueKeys)
        {
            if (uniqueKeys.Any())
            {
                TreeNode uniqueKeysNode = new TreeNode("Unique Keys")
                {
                    Tag = uniqueKeys,
                    ImageIndex = (int)ImageType.Folder,
                    SelectedImageIndex = (int)ImageType.Folder
                };
                node.Nodes.Add(uniqueKeysNode);
                uniqueKeysNode.ShowPlus();
            }
        }

        private void BuildForeignKeysNode(TreeNode node, IEnumerable<ForeignKey> foreignKeys)
        {
            if (foreignKeys.Any())
            {
                TreeNode foreignKeysNode = new TreeNode("Foreign Keys")
                {
                    Tag = foreignKeys,
                    ImageIndex = (int)ImageType.Folder,
                    SelectedImageIndex = (int)ImageType.Folder
                };
                node.Nodes.Add(foreignKeysNode);
                foreignKeysNode.ShowPlus();
            }
        }

        private void BuildIndexesNode(TreeNode node, IEnumerable<Index> indexes)
        {
            if (indexes.Any())
            {
                TreeNode indexesNode = new TreeNode("Indexes")
                {
                    Tag = indexes,
                    ImageIndex = (int)ImageType.Folder,
                    SelectedImageIndex = (int)ImageType.Folder
                };
                node.Nodes.Add(indexesNode);
                indexesNode.ShowPlus();
            }
        }

        private void BuildTableColumns(TreeViewCancelEventArgs e)
        {
            IEnumerable<TableColumn> tableColumns = e.Node.Tag as IEnumerable<TableColumn>;

            foreach (var column in tableColumns)
                e.Node.Nodes.Add(BuildTableColumnNode(column));
        }

        private TreeNode BuildTableColumnNode(TableColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)(column.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column)),
                SelectedImageIndex = (int)(column.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column))
            };
        }

        private void BuildTablePrimaryKey(TreeNode node)
        {
            PrimaryKey primaryKey = (node.Tag as IEnumerable<PrimaryKey>).First();

            TreeNode primaryKeyNode = BuildTablePrimaryKeyNode(primaryKey);
            node.Nodes.Add(primaryKeyNode);
            primaryKeyNode.ShowPlus();
        }

        private TreeNode BuildTablePrimaryKeyNode(PrimaryKey primaryKey)
        {
            return new TreeNode(primaryKey.ToString())
            {
                Tag = primaryKey,
                ImageIndex = (int)ImageType.PrimaryKey,
                SelectedImageIndex = (int)ImageType.PrimaryKey
            };
        }

        private void BuildTablePrimaryKeyColumns(TreeNode node)
        {
            PrimaryKey primaryKey = node.Tag as PrimaryKey;

            foreach (var column in primaryKey.PrimaryKeyColumns)
                node.Nodes.Add(BuildTablePrimaryKeyColumnNode(column));
        }

        private TreeNode BuildTablePrimaryKeyColumnNode(PrimaryKeyColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.PrimaryKey,
                SelectedImageIndex = (int)ImageType.PrimaryKey
            };
        }

        private void BuildTableUniqueKeys(TreeNode node)
        {
            IEnumerable<UniqueKey> uniqueKeys = node.Tag as IEnumerable<UniqueKey>;

            foreach (var uniqueKey in uniqueKeys)
            {
                TreeNode uniqueKeyNode = BuildTableUniqueKeyNode(uniqueKey);
                node.Nodes.Add(uniqueKeyNode);
                uniqueKeyNode.ShowPlus();
            }
        }

        private TreeNode BuildTableUniqueKeyNode(UniqueKey uniqueKey)
        {
            return new TreeNode(uniqueKey.ToString())
            {
                Tag = uniqueKey,
                ImageIndex = (int)ImageType.UniqueKey,
                SelectedImageIndex = (int)ImageType.UniqueKey
            };
        }

        private void BuildTableUniqueKeyColumns(TreeNode node)
        {
            UniqueKey uniqueKey = node.Tag as UniqueKey;

            foreach (var column in uniqueKey.UniqueKeyColumns)
                node.Nodes.Add(BuildTableUniqueKeyColumnNode(column));
        }

        private TreeNode BuildTableUniqueKeyColumnNode(UniqueKeyColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)(column.TableColumn.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.TableColumn.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column)),
                SelectedImageIndex = (int)(column.TableColumn.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.TableColumn.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column))
            };
        }

        private void BuildTableForeignKeys(TreeNode node)
        {
            IEnumerable<ForeignKey> foreignKeys = node.Tag as IEnumerable<ForeignKey>;

            foreach (var foreignKey in foreignKeys)
            {
                TreeNode foreignKeyNode = BuildTableForeignKeyNode(foreignKey);
                node.Nodes.Add(foreignKeyNode);
                foreignKeyNode.ShowPlus();
            }
        }

        private TreeNode BuildTableForeignKeyNode(ForeignKey foreignKey)
        {
            return new TreeNode(foreignKey.ToString())
            {
                Tag = foreignKey,
                ImageIndex = (int)ImageType.ForeignKey,
                SelectedImageIndex = (int)ImageType.ForeignKey
            };
        }

        private void BuildTableForeignKeyColumns(TreeNode node)
        {
            ForeignKey foreignKey = node.Tag as ForeignKey;

            foreach (var column in foreignKey.ForeignKeyColumns)
                node.Nodes.Add(BuildTableForeignKeyColumnNode(column, foreignKey));
        }

        private TreeNode BuildTableForeignKeyColumnNode(ForeignKeyColumn column, ForeignKey foreignKey)
        {
            return new TreeNode(string.Format("{0} {1} {2}.{3}", column.ToFullString(), char.ConvertFromUtf32(8594), foreignKey.PrimaryTable, column.PrimaryTableColumn))
            {
                Tag = column,
                ImageIndex = (int)ImageType.ForeignKey,
                SelectedImageIndex = (int)ImageType.ForeignKey
            };
        }

        private void BuildTableIndexes(TreeNode node)
        {
            IEnumerable<TableIndex> tableIndexes = node.Tag as IEnumerable<TableIndex>;

            foreach (var tableIndex in tableIndexes)
            {
                TreeNode tableIndexNode = BuildTableIndexNode(tableIndex);
                node.Nodes.Add(tableIndexNode);
                tableIndexNode.ShowPlus();
            }
        }

        private TreeNode BuildTableIndexNode(TableIndex tableIndex)
        {
            return new TreeNode(tableIndex.ToString())
            {
                Tag = tableIndex,
                ImageIndex = (int)ImageType.Index,
                SelectedImageIndex = (int)ImageType.Index
            };
        }

        private void BuildTableIndexColumns(TreeNode node)
        {
            TableIndex tableIndex = node.Tag as TableIndex;

            foreach (var column in tableIndex.IndexColumns)
                node.Nodes.Add(BuildTableIndexColumnNode(column));
        }

        private TreeNode BuildTableIndexColumnNode(TableIndexColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)(column.TableColumn.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.TableColumn.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column)),
                SelectedImageIndex = (int)(column.TableColumn.PrimaryKeyColumn != null ? ImageType.PrimaryKey : (column.TableColumn.ForeignKeyColumns.Any() ? ImageType.ForeignKey : ImageType.Column))
            };
        }

        private void BuildViewColumns(TreeNode node)
        {
            IEnumerable<ViewColumn> viewColumns = node.Tag as IEnumerable<ViewColumn>;

            foreach (var column in viewColumns)
                node.Nodes.Add(BuildViewColumnNode(column));
        }

        private TreeNode BuildViewColumnNode(ViewColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildViewIndexes(TreeNode node)
        {
            IEnumerable<ViewIndex> viewIndexes = node.Tag as IEnumerable<ViewIndex>;

            foreach (var viewIndex in viewIndexes)
            {
                TreeNode viewIndexNode = BuildViewIndexNode(viewIndex);
                node.Nodes.Add(viewIndexNode);
                viewIndexNode.ShowPlus();
            }
        }

        private TreeNode BuildViewIndexNode(ViewIndex viewIndex)
        {
            return new TreeNode(viewIndex.ToString())
            {
                Tag = viewIndex,
                ImageIndex = (int)ImageType.Index,
                SelectedImageIndex = (int)ImageType.Index
            };
        }

        private void BuildViewIndexColumns(TreeNode node)
        {
            ViewIndex viewIndex = node.Tag as ViewIndex;

            foreach (var column in viewIndex.IndexColumns)
                node.Nodes.Add(BuildViewIndexColumnNode(column));
        }

        private TreeNode BuildViewIndexColumnNode(ViewIndexColumn column)
        {
            return new TreeNode(column.ToFullString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildProcedureParameters(TreeNode node)
        {
            IEnumerable<ProcedureParameter> procedureParameters = node.Tag as IEnumerable<ProcedureParameter>;

            foreach (var parameter in procedureParameters)
                node.Nodes.Add(BuildProcedureParameterNode(parameter));
        }

        private TreeNode BuildProcedureParameterNode(ProcedureParameter parameter)
        {
            return new TreeNode(parameter.ToString())
            {
                Tag = parameter,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildProcedureColumns(TreeNode node)
        {
            IEnumerable<ProcedureColumn> procedureColumns = node.Tag as IEnumerable<ProcedureColumn>;

            foreach (var column in procedureColumns)
                node.Nodes.Add(BuildProcedureColumnNode(column));
        }

        private TreeNode BuildProcedureColumnNode(ProcedureColumn column)
        {
            return new TreeNode(column.ToString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildFunctionParameters(TreeNode node)
        {
            IEnumerable<FunctionParameter> functionParameters = node.Tag as IEnumerable<FunctionParameter>;

            foreach (var parameter in functionParameters)
                node.Nodes.Add(BuildFunctionParameterNode(parameter));
        }

        private TreeNode BuildFunctionParameterNode(FunctionParameter parameter)
        {
            return new TreeNode(parameter.ToString())
            {
                Tag = parameter,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildFunctionColumns(TreeNode node)
        {
            IEnumerable<FunctionColumn> functionColumns = node.Tag as IEnumerable<FunctionColumn>;

            foreach (var column in functionColumns)
                node.Nodes.Add(BuildFunctionColumnNode(column));
        }

        private TreeNode BuildFunctionColumnNode(FunctionColumn column)
        {
            return new TreeNode(column.ToString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        private void BuildTVPColumns(TreeNode node)
        {
            IEnumerable<TVPColumn> tvpColumns = node.Tag as IEnumerable<TVPColumn>;

            foreach (var column in tvpColumns)
                node.Nodes.Add(BuildTVPColumnNode(column));
        }

        private TreeNode BuildTVPColumnNode(TVPColumn column)
        {
            return new TreeNode(column.ToString())
            {
                Tag = column,
                ImageIndex = (int)ImageType.Column,
                SelectedImageIndex = (int)ImageType.Column
            };
        }

        #endregion

        #region Controls Appearance

        private Dictionary<Control, Point> controlsOriginalLocation;

        private void GetControlsOriginalLocation()
        {
            this.controlsOriginalLocation = new Control[]
            {
                lblWordsSeparator,
                txtWordsSeparator,
                lblWordsSeparatorDesc,
                chkCamelCase,
                chkUpperCase,
                chkLowerCase,
                lblSearch,
                txtSearch,
                lblReplace,
                txtReplace,
                chkSearchIgnoreCase,
                lblFixedName,
                txtFixedClassName,
                lblPrefix,
                txtPrefix,
                lblSuffix,
                txtSuffix
            }
            .ToDictionary(c => c, c => new Point(c.Location.X, c.Location.Y));
        }

        private void SetFormControls(bool isSupportSchema, bool isSupportTVPs, bool isSupportEnumDataType)
        {
            chkIncludeSchema.Visible = isSupportSchema;

            chkIgnoreDboSchema.Visible = isSupportSchema;

            if (isSupportSchema == false)
                txtSchemaSeparator.Text = string.Empty;
            lblSchemaSeparator.Visible = isSupportSchema;
            txtSchemaSeparator.Visible = isSupportSchema;

            if (isSupportSchema)
            {
                foreach (var item in this.controlsOriginalLocation)
                    item.Key.Location = item.Value;
            }
            else
            {
                foreach (var item in this.controlsOriginalLocation)
                    item.Key.Location = new Point(item.Value.X, item.Value.Y - 23);
            }

            if (isSupportTVPs)
                lblSingularDesc.Text = "(Tables, Views, TVPs)";
            else
                lblSingularDesc.Text = "(Tables, Views)";

            if (isSupportEnumDataType)
            {
                panelEnum.Visible = true;

                // deubg ????
            }
            else
            {
                SetRadioButton(rdbEnumSQLTypeToString, rdbEnumSQLTypeToString_CheckedChanged, true);
                SetRadioButton(rdbEnumSQLTypeToEnumUShort, rdbEnumSQLTypeToEnumUShort_CheckedChanged, false);
                SetRadioButton(rdbEnumSQLTypeToEnumInt, rdbEnumSQLTypeToEnumInt_CheckedChanged, false);

                panelEnum.Visible = false;
            }
        }

        #endregion

        #region Enable/Disable Server Tree

        private void EnableServerTree()
        {
            trvServer.BeforeCollapse -= trvServer_DisableEvent;
            trvServer.BeforeExpand -= trvServer_DisableEvent;
            trvServer.BeforeExpand += trvServer_BeforeExpand;
            trvServer.AfterCheck += trvServer_AfterCheck;
            trvServer.MouseUp += trvServer_MouseUp;
            trvServer.AfterSelect += trvServer_AfterSelect;
        }

        private void DisableServerTree()
        {
            trvServer.BeforeCollapse += trvServer_DisableEvent;
            trvServer.BeforeExpand -= trvServer_BeforeExpand;
            trvServer.BeforeExpand += trvServer_DisableEvent;
            trvServer.AfterCheck -= trvServer_AfterCheck;
            trvServer.MouseUp -= trvServer_MouseUp;
            trvServer.AfterSelect -= trvServer_AfterSelect;
        }

        private void trvServer_DisableEvent(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region Server Tree CheckBoxes

        private void trvServer_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool isDrawCheckBox =
                (e.Node.Tag != null) && (
                e.Node.Tag is Database ||
                e.Node.Tag is IEnumerable<Table> ||
                e.Node.Tag is IEnumerable<POCOGenerator.Objects.View> ||
                e.Node.Tag is IEnumerable<Procedure> ||
                e.Node.Tag is IEnumerable<Function> ||
                e.Node.Tag is IEnumerable<TVP> ||
                e.Node.Tag is Table ||
                e.Node.Tag is POCOGenerator.Objects.View ||
                e.Node.Tag is Procedure ||
                e.Node.Tag is Function ||
                e.Node.Tag is TVP);

            if (isDrawCheckBox == false)
                e.Node.HideCheckBox();

            e.DrawDefault = true;
        }

        private bool isCheckedDatabase;
        private bool isCheckedTables;
        private bool isCheckedViews;
        private bool isCheckedProcedures;
        private bool isCheckedFunctions;
        private bool isCheckedTVPs;

        private void trvServer_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag is Database)
                DatabaseAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is IEnumerable<Table>)
                TablesAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is IEnumerable<POCOGenerator.Objects.View>)
                ViewsAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is IEnumerable<Procedure>)
                ProceduresAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is IEnumerable<Function>)
                FunctionsAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is IEnumerable<TVP>)
                TVPsAfterCheck(e.Node, e.Node.Checked);
            else if (e.Node.Tag != null && e.Node.Tag is Table)
                CheckDbObjectNode(e.Node, e.Node.Checked, ref this.isCheckedTables);
            else if (e.Node.Tag != null && e.Node.Tag is POCOGenerator.Objects.View)
                CheckDbObjectNode(e.Node, e.Node.Checked, ref this.isCheckedViews);
            else if (e.Node.Tag != null && e.Node.Tag is Procedure)
                CheckDbObjectNode(e.Node, e.Node.Checked, ref this.isCheckedProcedures);
            else if (e.Node.Tag != null && e.Node.Tag is Function)
                CheckDbObjectNode(e.Node, e.Node.Checked, ref this.isCheckedFunctions);
            else if (e.Node.Tag != null && e.Node.Tag is TVP)
                CheckDbObjectNode(e.Node, e.Node.Checked, ref this.isCheckedTVPs);

            TreeNodeChecked();
        }

        private void DatabaseAfterCheck(TreeNode databaseNode, bool isChecked)
        {
            this.isCheckedDatabase = isChecked;
            this.isCheckedTables = isChecked;
            this.isCheckedViews = isChecked;
            this.isCheckedProcedures = isChecked;
            this.isCheckedFunctions = isChecked;
            this.isCheckedTVPs = isChecked;

            CheckDatabaseNode(databaseNode, isChecked);
        }

        private void CheckDatabaseNode(TreeNode databaseNode, bool isChecked)
        {
            if (databaseNode.Nodes.Count > 0)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                foreach (TreeNode dbObjectsNode in databaseNode.Nodes)
                {
                    dbObjectsNode.Checked = isChecked;
                    foreach (TreeNode node in dbObjectsNode.Nodes)
                        node.Checked = isChecked;
                }
                trvServer.AfterCheck += trvServer_AfterCheck;
            }
        }

        private void ChangeCheckDatabaseNode(TreeNode databaseNode)
        {
            if (this.isCheckedDatabase)
            {
                if (this.isCheckedTables == false || this.isCheckedViews == false || this.isCheckedProcedures == false || this.isCheckedFunctions == false || this.isCheckedTVPs == false)
                {
                    this.isCheckedDatabase = false;
                    trvServer.AfterCheck -= trvServer_AfterCheck;
                    databaseNode.Checked = false;
                    trvServer.AfterCheck += trvServer_AfterCheck;
                }
            }
            else
            {
                if (this.isCheckedTables && this.isCheckedViews && this.isCheckedProcedures && this.isCheckedFunctions && this.isCheckedTVPs)
                {
                    this.isCheckedDatabase = true;
                    trvServer.AfterCheck -= trvServer_AfterCheck;
                    databaseNode.Checked = true;
                    trvServer.AfterCheck += trvServer_AfterCheck;
                }
            }
        }

        private void TablesAfterCheck(TreeNode dbObjectsNode, bool isChecked)
        {
            this.isCheckedTables = isChecked;
            CheckDbObjectsNode(dbObjectsNode, isChecked);
            ChangeCheckDatabaseNode(dbObjectsNode.Parent);
        }

        private void ViewsAfterCheck(TreeNode dbObjectsNode, bool isChecked)
        {
            this.isCheckedViews = isChecked;
            CheckDbObjectsNode(dbObjectsNode, isChecked);
            ChangeCheckDatabaseNode(dbObjectsNode.Parent);
        }

        private void ProceduresAfterCheck(TreeNode dbObjectsNode, bool isChecked)
        {
            this.isCheckedProcedures = isChecked;
            CheckDbObjectsNode(dbObjectsNode, isChecked);
            ChangeCheckDatabaseNode(dbObjectsNode.Parent);
        }

        private void FunctionsAfterCheck(TreeNode dbObjectsNode, bool isChecked)
        {
            this.isCheckedFunctions = isChecked;
            CheckDbObjectsNode(dbObjectsNode, isChecked);
            ChangeCheckDatabaseNode(dbObjectsNode.Parent);
        }

        private void TVPsAfterCheck(TreeNode dbObjectsNode, bool isChecked)
        {
            this.isCheckedTVPs = isChecked;
            CheckDbObjectsNode(dbObjectsNode, isChecked);
            ChangeCheckDatabaseNode(dbObjectsNode.Parent);
        }

        private void CheckDbObjectsNode(TreeNode dbObjectsNode, bool isChecked)
        {
            if (dbObjectsNode.Nodes.Count > 0)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                foreach (TreeNode node in dbObjectsNode.Nodes)
                    node.Checked = isChecked;
                trvServer.AfterCheck += trvServer_AfterCheck;
            }
        }

        private void CheckDbObjectNode(TreeNode node, bool isChecked, ref bool isDbObjectsChecked)
        {
            if (isChecked)
            {
                foreach (TreeNode siblingNode in node.Parent.Nodes)
                {
                    if (siblingNode.Checked == false)
                        return;
                }

                isDbObjectsChecked = true;

                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Parent.Checked = true;
                trvServer.AfterCheck += trvServer_AfterCheck;

                ChangeCheckDatabaseNode(node.Parent.Parent);
            }
            else
            {
                if (isDbObjectsChecked)
                {
                    isDbObjectsChecked = false;

                    trvServer.AfterCheck -= trvServer_AfterCheck;
                    node.Parent.Checked = false;
                    trvServer.AfterCheck += trvServer_AfterCheck;

                    ChangeCheckDatabaseNode(node.Parent.Parent);
                }
            }
        }

        #endregion

        #region Generate POCOs

        private bool generatePOCOsFromCheckedTreeNodes;
        private TreeNode currentSelectedNode;
        private readonly List<Table> selectedTables = new List<Table>();
        private readonly List<POCOGenerator.Objects.View> selectedViews = new List<POCOGenerator.Objects.View>();
        private readonly List<Procedure> selectedProcedures = new List<Procedure>();
        private readonly List<Function> selectedFunctions = new List<Function>();
        private readonly List<TVP> selectedTVPs = new List<TVP>();

        private void GetSelectedDbObjects()
        {
            this.selectedTables.Clear();
            this.selectedViews.Clear();
            this.selectedProcedures.Clear();
            this.selectedFunctions.Clear();
            this.selectedTVPs.Clear();

            TreeNode serverNode = trvServer.Nodes[0];
            foreach (TreeNode databaseNode in serverNode.Nodes)
            {
                foreach (TreeNode dbObjectsNode in databaseNode.Nodes)
                {
                    if (dbObjectsNode.Tag is IEnumerable<Table>)
                        GetCheckedDbObjects<Table>(dbObjectsNode, this.selectedTables);
                    else if (dbObjectsNode.Tag is IEnumerable<POCOGenerator.Objects.View>)
                        GetCheckedDbObjects<POCOGenerator.Objects.View>(dbObjectsNode, this.selectedViews);
                    else if (dbObjectsNode.Tag is IEnumerable<Procedure>)
                        GetCheckedDbObjects<Procedure>(dbObjectsNode, this.selectedProcedures);
                    else if (dbObjectsNode.Tag is IEnumerable<Function>)
                        GetCheckedDbObjects<Function>(dbObjectsNode, this.selectedFunctions);
                    else if (dbObjectsNode.Tag is IEnumerable<TVP>)
                        GetCheckedDbObjects<TVP>(dbObjectsNode, this.selectedTVPs);
                }
            }
        }

        private void GetCheckedDbObjects<T>(TreeNode dbObjectsNode, List<T> lst) where T : IDbObject
        {
            if (dbObjectsNode.Checked)
            {
                lst.AddRange((IEnumerable<T>)dbObjectsNode.Tag);
            }
            else
            {
                foreach (TreeNode dbObjectNode in dbObjectsNode.Nodes)
                {
                    if (dbObjectNode.Checked)
                        lst.Add((T)dbObjectNode.Tag);
                }
            }
        }

        private void trvServer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNodeSelected();
        }

        private void TreeNodeSelected()
        {
            ClearStatus();

            if (this.generatePOCOsFromCheckedTreeNodes)
                return;

            GeneratorResults results = GeneratePOCOs();
            if (results == GeneratorResults.NoDbObjectsIncluded)
            {
                txtPocoEditor.Clear();
                PrintDatabaseErrors();
            }
        }

        private void PrintDatabaseErrors()
        {
            TreeNode selectedNode = trvServer.SelectedNode;
            if (selectedNode != null && selectedNode.Tag != null)
            {
                if (selectedNode.Tag is Database)
                {
                    Database database = (Database)selectedNode.Tag;
                    if (database.Errors.Any())
                    {
                        txtPocoEditor.Select(txtPocoEditor.TextLength, 0);
                        txtPocoEditor.SelectionColor = this.generator.Settings.SyntaxHighlight.Error;
                        txtPocoEditor.SelectedText = string.Join(Environment.NewLine + Environment.NewLine, database.Errors);
                        txtPocoEditor.SelectionColor = this.generator.Settings.SyntaxHighlight.Text;
                    }
                }
            }
        }

        private void POCOOptionChanged()
        {
            ClearStatus();

            GeneratorResults results = GeneratePOCOs(forceGeneratingPOCOs: true);
            if (results == GeneratorResults.NoDbObjectsIncluded)
                txtPocoEditor.Clear();
        }

        private void TreeNodeChecked()
        {
            ClearStatus();

            GetSelectedDbObjects();

            GeneratorResults results = GeneratePOCOs();
            if (results == GeneratorResults.NoDbObjectsIncluded)
                txtPocoEditor.Clear();
        }

        private void ExportPOCOs(Action<IGenerator> BeforeGeneratePOCOs = null, Action<IGenerator> AfterGeneratePOCOs = null)
        {
            GeneratePOCOs(false, true, BeforeGeneratePOCOs, AfterGeneratePOCOs);
        }

        private GeneratorResults GeneratePOCOs(bool forceGeneratingPOCOs = false, bool isExport = false, Action<IGenerator> BeforeGeneratePOCOs = null, Action<IGenerator> AfterGeneratePOCOs = null)
        {
            this.generatePOCOsFromCheckedTreeNodes =
                this.selectedTables.HasAny() ||
                this.selectedViews.HasAny() ||
                this.selectedProcedures.HasAny() ||
                this.selectedFunctions.HasAny() ||
                this.selectedTVPs.HasAny();

            List<Table> selectedTables = null;
            List<POCOGenerator.Objects.View> selectedViews = null;
            List<Procedure> selectedProcedures = null;
            List<Function> selectedFunctions = null;
            List<TVP> selectedTVPs = null;

            if (this.generatePOCOsFromCheckedTreeNodes)
            {
                selectedTables = new List<Table>(this.selectedTables);
                selectedViews = new List<POCOGenerator.Objects.View>(this.selectedViews);
                selectedProcedures = new List<Procedure>(this.selectedProcedures);
                selectedFunctions = new List<Function>(this.selectedFunctions);
                selectedTVPs = new List<TVP>(this.selectedTVPs);
            }
            else
            {
                TreeNode selectedNode = GetSelectedNode();

                if (selectedNode != null)
                {
                    if (isExport == false)
                        if (forceGeneratingPOCOs == false)
                            if (this.currentSelectedNode == selectedNode)
                                return GeneratorResults.None;

                    if (selectedNode.Tag is Table)
                    {
                        selectedTables = new List<Table>() { (Table)selectedNode.Tag };
                    }
                    else if (selectedNode.Tag is POCOGenerator.Objects.View)
                    {
                        selectedViews = new List<POCOGenerator.Objects.View>() { (POCOGenerator.Objects.View)selectedNode.Tag };
                    }
                    else if (selectedNode.Tag is Procedure)
                    {
                        selectedProcedures = new List<Procedure>() { (Procedure)selectedNode.Tag };
                    }
                    else if (selectedNode.Tag is Function)
                    {
                        selectedFunctions = new List<Function>() { (Function)selectedNode.Tag };
                    }
                    else if (selectedNode.Tag is TVP)
                    {
                        selectedTVPs = new List<TVP>() { (TVP)selectedNode.Tag };
                    }
                    else
                    {
                        if (isExport == false)
                            this.currentSelectedNode = null;

                        return GeneratorResults.NoDbObjectsIncluded;
                    }

                    if (isExport == false)
                        this.currentSelectedNode = selectedNode;
                }
                else
                {
                    if (isExport == false)
                        this.currentSelectedNode = null;

                    return GeneratorResults.NoDbObjectsIncluded;
                }
            }

            List<Database> selectedDatabases =
                (selectedTables.HasAny() ? selectedTables.Select(t => t.Database) : Enumerable.Empty<Database>())
                .Union(selectedViews.HasAny() ? selectedViews.Select(v => v.Database) : Enumerable.Empty<Database>())
                .Union(selectedProcedures.HasAny() ? selectedProcedures.Select(p => p.Database) : Enumerable.Empty<Database>())
                .Union(selectedFunctions.HasAny() ? selectedFunctions.Select(f => f.Database) : Enumerable.Empty<Database>())
                .Union(selectedTVPs.HasAny() ? selectedTVPs.Select(t => t.Database) : Enumerable.Empty<Database>())
                .Distinct()
                .ToList();

            void databaseGenerating(object sendeer, DatabaseGeneratingEventArgs e)
            {
                e.Skip = (selectedDatabases.Contains(e.Database) == false);
            }

            void tablesGenerating(object sender, TablesGeneratingEventArgs e)
            {
                e.Skip = selectedTables.IsNullOrEmpty();

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void tableGenerating(object sender, TableGeneratingEventArgs e)
            {
                e.Skip = (selectedTables.Contains(e.Table) == false);
            }

            void tableGenerated(object sender, TableGeneratedEventArgs e)
            {
                selectedTables.Remove(e.Table);

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void viewsGenerating(object sender, ViewsGeneratingEventArgs e)
            {
                e.Skip = selectedViews.IsNullOrEmpty();

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void viewGenerating(object sender, ViewGeneratingEventArgs e)
            {
                e.Skip = (selectedViews.Contains(e.View) == false);
            }

            void viewGenerated(object sender, ViewGeneratedEventArgs e)
            {
                selectedViews.Remove(e.View);

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void proceduresGenerating(object sender, ProceduresGeneratingEventArgs e)
            {
                e.Skip = selectedProcedures.IsNullOrEmpty();

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void procedureGenerating(object sender, ProcedureGeneratingEventArgs e)
            {
                e.Skip = (selectedProcedures.Contains(e.Procedure) == false);
            }

            void procedureGenerated(object sender, ProcedureGeneratedEventArgs e)
            {
                selectedProcedures.Remove(e.Procedure);

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void functionsGenerating(object sender, FunctionsGeneratingEventArgs e)
            {
                e.Skip = selectedFunctions.IsNullOrEmpty();

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void functionGenerating(object sender, FunctionGeneratingEventArgs e)
            {
                e.Skip = (selectedFunctions.Contains(e.Function) == false);
            }

            void functionGenerated(object sender, FunctionGeneratedEventArgs e)
            {
                selectedFunctions.Remove(e.Function);

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void tvpsGenerating(object sender, TVPsGeneratingEventArgs e)
            {
                e.Skip = selectedTVPs.IsNullOrEmpty();

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void tvpGenerating(object sender, TVPGeneratingEventArgs e)
            {
                e.Skip = (selectedTVPs.Contains(e.TVP) == false);
            }

            void tvpGenerated(object sender, TVPGeneratedEventArgs e)
            {
                selectedTVPs.Remove(e.TVP);

                e.Stop =
                    selectedTables.IsNullOrEmpty() &&
                    selectedViews.IsNullOrEmpty() &&
                    selectedProcedures.IsNullOrEmpty() &&
                    selectedFunctions.IsNullOrEmpty() &&
                    selectedTVPs.IsNullOrEmpty();
            }

            void databaseGenerated(object sendeer, DatabaseGeneratedEventArgs e)
            {
                selectedDatabases.Remove(e.Database);

                e.Stop = selectedDatabases.IsNullOrEmpty();
            }

            this.generator.DatabaseGenerating += databaseGenerating;
            this.generator.TablesGenerating += tablesGenerating;
            this.generator.TableGenerating += tableGenerating;
            this.generator.TableGenerated += tableGenerated;
            this.generator.ViewsGenerating += viewsGenerating;
            this.generator.ViewGenerating += viewGenerating;
            this.generator.ViewGenerated += viewGenerated;
            this.generator.ProceduresGenerating += proceduresGenerating;
            this.generator.ProcedureGenerating += procedureGenerating;
            this.generator.ProcedureGenerated += procedureGenerated;
            this.generator.FunctionsGenerating += functionsGenerating;
            this.generator.FunctionGenerating += functionGenerating;
            this.generator.FunctionGenerated += functionGenerated;
            this.generator.TVPsGenerating += tvpsGenerating;
            this.generator.TVPGenerating += tvpGenerating;
            this.generator.TVPGenerated += tvpGenerated;
            this.generator.DatabaseGenerated += databaseGenerated;

            SetGeneratorSettings();

            if (BeforeGeneratePOCOs != null)
                BeforeGeneratePOCOs(this.generator);

            GeneratorResults results = this.generator.GeneratePOCOs();

            if (AfterGeneratePOCOs != null)
                AfterGeneratePOCOs(this.generator);

            this.generator.DatabaseGenerating -= databaseGenerating;
            this.generator.TablesGenerating -= tablesGenerating;
            this.generator.TableGenerating -= tableGenerating;
            this.generator.TableGenerated -= tableGenerated;
            this.generator.ViewsGenerating -= viewsGenerating;
            this.generator.ViewGenerating -= viewGenerating;
            this.generator.ViewGenerated -= viewGenerated;
            this.generator.ProceduresGenerating -= proceduresGenerating;
            this.generator.ProcedureGenerating -= procedureGenerating;
            this.generator.ProcedureGenerated -= procedureGenerated;
            this.generator.FunctionsGenerating -= functionsGenerating;
            this.generator.FunctionGenerating -= functionGenerating;
            this.generator.FunctionGenerated -= functionGenerated;
            this.generator.TVPsGenerating -= tvpsGenerating;
            this.generator.TVPGenerating -= tvpGenerating;
            this.generator.TVPGenerated -= tvpGenerated;
            this.generator.DatabaseGenerated -= databaseGenerated;

            return results;
        }

        private TreeNode GetSelectedNode()
        {
            TreeNode selectedNode = trvServer.SelectedNode;

            if (selectedNode == null || selectedNode.Tag == null)
                return null;

            else if (selectedNode.Tag is Table)
                return selectedNode;
            else if (selectedNode.Tag is POCOGenerator.Objects.View)
                return selectedNode;
            else if (selectedNode.Tag is Procedure)
                return selectedNode;
            else if (selectedNode.Tag is Function)
                return selectedNode;
            else if (selectedNode.Tag is TVP)
                return selectedNode;

            else if (selectedNode.Tag is Server)
                return null;
            else if (selectedNode.Tag is Database)
                return null;
            else if (selectedNode.Tag is IEnumerable<Table>)
                return null;
            else if (selectedNode.Tag is IEnumerable<POCOGenerator.Objects.View>)
                return null;
            else if (selectedNode.Tag is IEnumerable<Procedure>)
                return null;
            else if (selectedNode.Tag is IEnumerable<Function>)
                return null;
            else if (selectedNode.Tag is IEnumerable<TVP>)
                return null;

            else if (selectedNode.Tag is IEnumerable<TableColumn>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is TableColumn)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<PrimaryKey>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is PrimaryKey)
                return selectedNode.Parent.Parent;
            else if (selectedNode.Tag is PrimaryKeyColumn)
                return selectedNode.Parent.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<UniqueKey>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is UniqueKey)
                return selectedNode.Parent.Parent;
            else if (selectedNode.Tag is UniqueKeyColumn)
                return selectedNode.Parent.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<ForeignKey>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is ForeignKey)
                return selectedNode.Parent.Parent;
            else if (selectedNode.Tag is ForeignKeyColumn)
                return selectedNode.Parent.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<TableIndex>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is TableIndex)
                return selectedNode.Parent.Parent;
            else if (selectedNode.Tag is TableIndexColumn)
                return selectedNode.Parent.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<ViewColumn>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is ViewColumn)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<ViewIndex>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is ViewIndex)
                return selectedNode.Parent.Parent;
            else if (selectedNode.Tag is ViewIndexColumn)
                return selectedNode.Parent.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<ProcedureParameter>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is ProcedureParameter)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<ProcedureColumn>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is ProcedureColumn)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<FunctionParameter>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is FunctionParameter)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<FunctionColumn>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is FunctionColumn)
                return selectedNode.Parent.Parent;

            else if (selectedNode.Tag is IEnumerable<TVPColumn>)
                return selectedNode.Parent;
            else if (selectedNode.Tag is TVPColumn)
                return selectedNode.Parent.Parent;

            else
                return null;
        }

        #endregion

        #region Context Menu

        private void trvServer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point point = new Point(e.X, e.Y);
                TreeNode node = trvServer.GetNodeAt(point);

                if (node != null)
                {
                    trvServer.SelectedNode = node;

                    if (node.Tag is Database)
                    {
                        removeFilterToolStripMenuItem.Visible = false;
                        filterSettingsToolStripMenuItem.Visible = false;
                        clearCheckBoxesToolStripMenuItem.Visible = true;
                        contextMenu.Show(trvServer, point);
                    }
                    else if (
                        node.Tag is IEnumerable<Table> ||
                        node.Tag is IEnumerable<POCOGenerator.Objects.View> ||
                        node.Tag is IEnumerable<Procedure> ||
                        node.Tag is IEnumerable<Function> ||
                        node.Tag is IEnumerable<TVP>)
                    {
                        // stop the user from filtering unless the node was expanded
                        // this prevents error when exporting
                        if (node.Nodes.Count > 0)
                        {
                            removeFilterToolStripMenuItem.Visible = true;
                            filterSettingsToolStripMenuItem.Visible = true;
                            clearCheckBoxesToolStripMenuItem.Visible = true;
                            removeFilterToolStripMenuItem.Enabled = filters.ContainsKey(node) && filters[node].IsEnabled;
                            contextMenu.Show(trvServer, point);
                        }
                        else
                        {
                            contextMenu.Hide();
                            contextMenuTable.Hide();
                        }
                    }
                    else if (node.Tag is Table)
                    {
                        string tableName = ((Table)node.Tag).ToString();
                        checkReferencedTablesToolStripMenuItem.Text = "Check Tables Referenced From " + tableName;
                        checkReferencingTablesToolStripMenuItem.Text = "Check Tables Referencing To " + tableName;
                        checkAccessibleTablesToolStripMenuItem.Text = "Check Recursively Tables Accessible From && To " + tableName;
                        contextMenuTable.Show(trvServer, point);
                    }
                    else
                    {
                        contextMenu.Hide();
                        contextMenuTable.Hide();
                    }
                }
                else
                {
                    contextMenu.Hide();
                    contextMenuTable.Hide();
                }
            }
        }

        #region Filters

        private readonly Dictionary<TreeNode, FilterSettings> filters = new Dictionary<TreeNode, FilterSettings>();

        private void removeFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            if (filters.ContainsKey(node) == false)
                return;

            FilterSettings filterSettings = filters[node];

            if (filterSettings.IsEnabled)
            {
                filters.Remove(node);
                node.ShowAll();
                node.Text = node.Text.Replace(" (filtered)", string.Empty);
            }
        }

        private void filterSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            FilterSettings filterSettings = null;
            if (filters.ContainsKey(node))
                filterSettings = filters[node];
            else
                filterSettings = new FilterSettings();

            FilterSettingsForm filterSettingsForm = new FilterSettingsForm(filterSettings, this.generator.Support.SupportSchema);

            if (filterSettingsForm.ShowDialog(this) == DialogResult.OK)
            {
                filterSettings.FilterName.FilterType = filterSettingsForm.FilterTypeName;
                filterSettings.FilterName.Filter = filterSettingsForm.FilterName;
                filterSettings.FilterSchema.FilterType = filterSettingsForm.FilterTypeSchema;
                filterSettings.FilterSchema.Filter = filterSettingsForm.FilterSchema;

                if (filterSettings.IsEnabled)
                {
                    if (filters.ContainsKey(node) == false)
                        filters.Add(node, filterSettings);

                    SetFilteredNodesVisibility(node, filterSettings);
                }
                else
                {
                    if (filters.ContainsKey(node))
                        filters.Remove(node);

                    node.ShowAll();
                    node.Text = node.Text.Replace(" (filtered)", string.Empty);
                }
            }
        }

        private void SetFilteredNodesVisibility(TreeNode parent, FilterSettings filterSettings)
        {
            List<TreeNode> toShow = new List<TreeNode>();
            List<TreeNode> toHide = new List<TreeNode>();

            bool isSupportSchema = this.generator.Support.SupportSchema;

            foreach (TreeNode node in parent.Nodes)
                (IsMatchFilter(node.Tag as IDbObject, filterSettings, isSupportSchema) ? toShow : toHide).Add(node);

            foreach (TreeNode node in toHide)
                node.Hide();

            foreach (TreeNode node in toShow)
                node.Show();

            if (parent.Text.Contains(" (filtered)") == false)
                parent.Text += " (filtered)";
        }

        private bool IsMatchFilter(IDbObject dbObject, FilterSettings filterSettings, bool isSupportSchema)
        {
            if (filterSettings.FilterName.IsEnabled)
            {
                if (filterSettings.FilterName.FilterType == FilterType.Equals)
                {
                    if ((string.Compare(dbObject.Name, filterSettings.FilterName.Filter, true) == 0) == false)
                        return false;
                }
                else if (filterSettings.FilterName.FilterType == FilterType.Contains)
                {
                    if ((dbObject.Name.IndexOf(filterSettings.FilterName.Filter, StringComparison.InvariantCultureIgnoreCase) != -1) == false)
                        return false;
                }
                else if (filterSettings.FilterName.FilterType == FilterType.Does_Not_Contain)
                {
                    if ((dbObject.Name.IndexOf(filterSettings.FilterName.Filter, StringComparison.InvariantCultureIgnoreCase) == -1) == false)
                        return false;
                }
            }

            if (isSupportSchema)
            {
                if (filterSettings.FilterSchema.IsEnabled)
                {
                    if (filterSettings.FilterSchema.FilterType == FilterType.Equals)
                    {
                        if ((string.Compare(dbObject.Schema, filterSettings.FilterSchema.Filter, true) == 0) == false)
                            return false;
                    }
                    else if (filterSettings.FilterSchema.FilterType == FilterType.Contains)
                    {
                        if ((dbObject.Schema.IndexOf(filterSettings.FilterSchema.Filter, StringComparison.InvariantCultureIgnoreCase) != -1) == false)
                            return false;
                    }
                    else if (filterSettings.FilterSchema.FilterType == FilterType.Does_Not_Contain)
                    {
                        if ((dbObject.Schema.IndexOf(filterSettings.FilterSchema.Filter, StringComparison.InvariantCultureIgnoreCase) == -1) == false)
                            return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Check Boxes

        private void clearCheckBoxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            if (node.Tag is Database)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                DatabaseAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
            else if (node.Tag is IEnumerable<Table>)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                TablesAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
            else if (node.Tag is IEnumerable<POCOGenerator.Objects.View>)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                ViewsAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
            else if (node.Tag is IEnumerable<Procedure>)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                ProceduresAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
            else if (node.Tag is IEnumerable<Function>)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                FunctionsAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
            else if (node.Tag is IEnumerable<TVP>)
            {
                trvServer.AfterCheck -= trvServer_AfterCheck;
                node.Checked = false;
                TVPsAfterCheck(node, false);
                trvServer.AfterCheck += trvServer_AfterCheck;
                TreeNodeChecked();
            }
        }

        #endregion

        #region Table-Connected Check Boxes

        private void checkReferencedTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            Table table = node.Tag as Table;
            if (table == null)
                return;

            trvServer.AfterCheck -= trvServer_AfterCheck;

            List<Table> primaryTables = table.ForeignKeys.Select(fk => fk.PrimaryTable).Where(pt => pt != null).ToList();

            List<TreeNode> toCheck = new List<TreeNode>();
            foreach (TreeNode n in node.Parent.Nodes)
            {
                if (primaryTables.Remove(((Table)n.Tag)))
                {
                    if (n.Checked == false)
                        toCheck.Add(n);

                    if (primaryTables.IsNullOrEmpty())
                        break;
                }
            }

            if (toCheck.HasAny())
            {
                foreach (TreeNode tableNode in toCheck)
                    tableNode.Checked = true;

                TreeNodeChecked();
            }

            trvServer.AfterCheck += trvServer_AfterCheck;
        }

        private void checkReferencingTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            Table table = node.Tag as Table;
            if (table == null)
                return;

            trvServer.AfterCheck -= trvServer_AfterCheck;

            List<TreeNode> toCheck = new List<TreeNode>();
            foreach (TreeNode n in node.Parent.Nodes)
            {
                if (((Table)n.Tag).ForeignKeys.Any(fk => fk.PrimaryTable == table))
                {
                    if (n.Checked == false)
                        toCheck.Add(n);
                }
            }

            if (toCheck.HasAny())
            {
                foreach (TreeNode tableNode in toCheck)
                    tableNode.Checked = true;

                TreeNodeChecked();
            }

            trvServer.AfterCheck += trvServer_AfterCheck;
        }

        private void checkAccessibleTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = trvServer.SelectedNode;
            if (node == null)
                return;

            Table table = node.Tag as Table;
            if (table == null)
                return;

            trvServer.AfterCheck -= trvServer_AfterCheck;

            List<Table> accessibleTables = new List<Table>() { table };

            int fromIndex = 0;
            int toIndex = 0;
            while (fromIndex <= toIndex)
            {
                while (fromIndex <= toIndex)
                {
                    for (int i = fromIndex; i <= toIndex; i++)
                    {
                        foreach (var fk in accessibleTables[i].ForeignKeys)
                        {
                            if (accessibleTables.Contains(fk.PrimaryTable) == false)
                                accessibleTables.Add(fk.PrimaryTable);
                        }
                    }

                    fromIndex = toIndex + 1;
                    toIndex = accessibleTables.Count - 1;
                }

                foreach (TreeNode n in node.Parent.Nodes)
                {
                    Table t = (Table)n.Tag;
                    if (accessibleTables.Contains(t) == false)
                    {
                        if (t.ForeignKeys.Any(fk => accessibleTables.Contains(fk.PrimaryTable)))
                            accessibleTables.Add(t);
                    }
                }

                toIndex = accessibleTables.Count - 1;
            }

            List<TreeNode> toCheck = new List<TreeNode>();
            foreach (TreeNode n in node.Parent.Nodes)
            {
                Table t = (Table)n.Tag;
                if (accessibleTables.Remove(t))
                {
                    if (n.Checked == false)
                        toCheck.Add(n);

                    if (accessibleTables.IsNullOrEmpty())
                        break;
                }
            }

            if (toCheck.HasAny())
            {
                foreach (TreeNode tableNode in toCheck)
                    tableNode.Checked = true;

                TreeNodeChecked();
            }

            trvServer.AfterCheck += trvServer_AfterCheck;
        }

        #endregion

        #endregion

        #region POCO Options

        #region Set Control

        private void SetCheckBox(CheckBox chk, EventHandler checkedChangedHandler, bool isChecked)
        {
            chk.CheckedChanged -= checkedChangedHandler;
            chk.Checked = isChecked;
            chk.CheckedChanged += checkedChangedHandler;
        }

        private void SetRadioButton(RadioButton rdb, EventHandler checkedChangedHandler, bool isChecked)
        {
            rdb.CheckedChanged -= checkedChangedHandler;
            rdb.Checked = isChecked;
            rdb.CheckedChanged += checkedChangedHandler;
        }

        private void SetTextBox(TextBox txt, EventHandler textChangedHandler, string text)
        {
            txt.TextChanged -= textChangedHandler;
            txt.Text = text;
            txt.TextChanged += textChangedHandler;
        }

        #endregion

        #region POCO

        private void rdbProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbProperties.Checked)
                POCOOptionChanged();
        }

        private void rdbFields_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFields.Checked)
                POCOOptionChanged();
        }

        private void chkVirtualProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVirtualProperties.Checked && chkOverrideProperties.Checked)
                SetCheckBox(chkOverrideProperties, chkOverrideProperties_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkOverrideProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVirtualProperties.Checked && chkOverrideProperties.Checked)
                SetCheckBox(chkVirtualProperties, chkVirtualProperties_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkPartialClass_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkStructTypesNullable_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkComments_CheckedChanged(object sender, EventArgs e)
        {
            if (chkComments.Checked && chkCommentsWithoutNull.Checked)
                SetCheckBox(chkCommentsWithoutNull, chkCommentsWithoutNull_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkCommentsWithoutNull_CheckedChanged(object sender, EventArgs e)
        {
            if (chkComments.Checked && chkCommentsWithoutNull.Checked)
                SetCheckBox(chkComments, chkComments_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkUsing_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkUsingInsideNamespace_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtInherit_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkColumnDefaults_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkNewLineBetweenMembers_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void rdbEnumSQLTypeToString_CheckedChanged(object sender, EventArgs e)
        {
            if (this.generator != null && this.generator.Support.SupportEnumDataType && rdbEnumSQLTypeToString.Checked)
                POCOOptionChanged();
        }

        private void rdbEnumSQLTypeToEnumUShort_CheckedChanged(object sender, EventArgs e)
        {
            if (this.generator != null && this.generator.Support.SupportEnumDataType && rdbEnumSQLTypeToEnumUShort.Checked)
                POCOOptionChanged();
        }

        private void rdbEnumSQLTypeToEnumInt_CheckedChanged(object sender, EventArgs e)
        {
            if (this.generator != null && this.generator.Support.SupportEnumDataType && rdbEnumSQLTypeToEnumInt.Checked)
                POCOOptionChanged();
        }

        #endregion

        #region Navigation Properties

        private void chkNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNavigationProperties.Checked == false)
            {
                SetCheckBox(chkVirtualNavigationProperties, chkVirtualNavigationProperties_CheckedChanged, false);
                SetCheckBox(chkOverrideNavigationProperties, chkOverrideNavigationProperties_CheckedChanged, false);
                SetCheckBox(chkShowManyToManyJoinTable, chkShowManyToManyJoinTable_CheckedChanged, false);
                SetCheckBox(chkNavigationPropertiesComments, chkNavigationPropertiesComments_CheckedChanged, false);
                SetCheckBox(chkEFForeignKeyAndInverseProperty, chkEFForeignKeyAndInverseProperty_CheckedChanged, false);
            }

            POCOOptionChanged();
        }

        private void chkVirtualNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVirtualNavigationProperties.Checked && chkNavigationProperties.Checked == false)
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);

            if (chkVirtualNavigationProperties.Checked && chkOverrideNavigationProperties.Checked)
                SetCheckBox(chkOverrideNavigationProperties, chkOverrideNavigationProperties_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkOverrideNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOverrideNavigationProperties.Checked && chkNavigationProperties.Checked == false)
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);

            if (chkVirtualNavigationProperties.Checked && chkOverrideNavigationProperties.Checked)
                SetCheckBox(chkVirtualNavigationProperties, chkVirtualNavigationProperties_CheckedChanged, false);

            POCOOptionChanged();
        }

        private void chkShowManyToManyJoinTable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowManyToManyJoinTable.Checked && chkNavigationProperties.Checked == false)
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);

            POCOOptionChanged();
        }

        private void chkNavigationPropertiesComments_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNavigationPropertiesComments.Checked && chkNavigationProperties.Checked == false)
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);

            POCOOptionChanged();
        }

        private void rdbListNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNavigationProperties.Checked && rdbListNavigationProperties.Checked)
                POCOOptionChanged();
        }

        private void rdbICollectionNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNavigationProperties.Checked && rdbICollectionNavigationProperties.Checked)
                POCOOptionChanged();
        }

        private void rdbIEnumerableNavigationProperties_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNavigationProperties.Checked && rdbIEnumerableNavigationProperties.Checked)
                POCOOptionChanged();
        }

        #endregion

        #region Class Name

        private void chkSingular_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkIncludeDB_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtDBSeparator_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkIncludeSchema_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkIgnoreDboSchema_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtSchemaSeparator_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtWordsSeparator_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkCamelCase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCamelCase.Checked)
            {
                SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, false);
                SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, false);
            }

            POCOOptionChanged();
        }

        private void chkUpperCase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpperCase.Checked)
            {
                SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, false);
                SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, false);
            }

            POCOOptionChanged();
        }

        private void chkLowerCase_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLowerCase.Checked)
            {
                SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, false);
                SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, false);
            }

            POCOOptionChanged();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtReplace_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void chkSearchIgnoreCase_CheckedChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtFixedClassName_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtPrefix_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        private void txtSuffix_TextChanged(object sender, EventArgs e)
        {
            POCOOptionChanged();
        }

        #endregion

        #region EF Annotations

        private void chkEF_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEF.Checked == false)
            {
                SetCheckBox(chkEFColumn, chkEFColumn_CheckedChanged, false);
                SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, false);
                SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, false);
                SetCheckBox(chkEFConcurrencyCheck, chkEFConcurrencyCheck_CheckedChanged, false);
                SetCheckBox(chkEFStringLength, chkEFStringLength_CheckedChanged, false);
                SetCheckBox(chkEFDisplay, chkEFDisplay_CheckedChanged, false);
                SetCheckBox(chkEFDescription, chkEFDescription_CheckedChanged, false);
                SetCheckBox(chkEFComplexType, chkEFComplexType_CheckedChanged, false);
                SetCheckBox(chkEFIndex, chkEFIndex_CheckedChanged, false);
                SetCheckBox(chkEFForeignKeyAndInverseProperty, chkEFForeignKeyAndInverseProperty_CheckedChanged, false);
            }

            POCOOptionChanged();
        }

        private void CheckEFCheckBox(bool otherEF)
        {
            if (otherEF && chkEF.Checked == false)
                SetCheckBox(chkEF, chkEF_CheckedChanged, true);
        }

        private void chkEFColumn_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFColumn.Checked);
            POCOOptionChanged();
        }

        private void chkEFRequired_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEFRequired.Checked && chkEFRequiredWithErrorMessage.Checked)
                SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, false);

            CheckEFCheckBox(chkEFRequired.Checked);

            POCOOptionChanged();
        }

        private void chkEFRequiredWithErrorMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEFRequired.Checked && chkEFRequiredWithErrorMessage.Checked)
                SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, false);

            CheckEFCheckBox(chkEFRequiredWithErrorMessage.Checked);

            POCOOptionChanged();
        }

        private void chkEFConcurrencyCheck_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFConcurrencyCheck.Checked);
            POCOOptionChanged();
        }

        private void chkEFStringLength_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFStringLength.Checked);
            POCOOptionChanged();
        }

        private void chkEFDisplay_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFDisplay.Checked);
            POCOOptionChanged();
        }

        private void chkEFDescription_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFDescription.Checked);
            POCOOptionChanged();
        }

        private void chkEFComplexType_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFComplexType.Checked);
            POCOOptionChanged();
        }

        private void chkEFIndex_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFIndex.Checked);
            POCOOptionChanged();
        }

        private void chkEFForeignKeyAndInverseProperty_CheckedChanged(object sender, EventArgs e)
        {
            CheckEFCheckBox(chkEFForeignKeyAndInverseProperty.Checked);

            if (chkEFForeignKeyAndInverseProperty.Checked && chkNavigationProperties.Checked == false)
                SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, true);

            POCOOptionChanged();
        }

        #endregion

        #endregion

        #region Export

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtFolder.Text))
                folderBrowserDialogExport.SelectedPath = txtFolder.Text;

            if (folderBrowserDialogExport.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                txtFolder.Text = folderBrowserDialogExport.SelectedPath;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (this.generator == null)
                return;

            ClearStatus();

            if (string.IsNullOrEmpty(txtFolder.Text))
            {
                SetStatusErrorMessage("Folder is empty.");
                return;
            }

            int selectedDbObjectsCount =
                this.selectedTables.Count +
                this.selectedViews.Count +
                this.selectedProcedures.Count +
                this.selectedFunctions.Count +
                this.selectedTVPs.Count;

            IDbObject dbObject = null;
            TreeNode selectedNode = GetSelectedNode();
            if (selectedNode != null)
            {
                if (selectedNode.Tag is Table)
                    dbObject = (Table)selectedNode.Tag;
                else if (selectedNode.Tag is POCOGenerator.Objects.View)
                    dbObject = (POCOGenerator.Objects.View)selectedNode.Tag;
                else if (selectedNode.Tag is Procedure)
                    dbObject = (Procedure)selectedNode.Tag;
                else if (selectedNode.Tag is Function)
                    dbObject = (Function)selectedNode.Tag;
                else if (selectedNode.Tag is TVP)
                    dbObject = (TVP)selectedNode.Tag;
            }

            if (selectedDbObjectsCount == 0 && dbObject == null)
                return;

            SetStatusMessage("Exporting...");

            try
            {
                if (rdbSingleFile.Checked)
                {
                    string fileName = Export_SingleFile_GetFileName(selectedDbObjectsCount, dbObject);
                    Export_SingleFile(fileName);
                }
                else if (rdbMultipleFiles.Checked)
                {
                    Export_MultipleFiles();
                }
                else if (rdbMultipleFilesRelativeNamespace.Checked)
                {
                    Export_MultipleFiles_RelativeNamespace();
                }
            }
            catch (Exception ex)
            {
                this.generator.RedirectTo(txtPocoEditor);

                SetStatusErrorMessage("Exporting failed. " + ex.Message);
            }
        }

        private void Export_SingleFile(string fileName)
        {
            string path = Path.GetFullPath(Path.Combine(txtFolder.Text, fileName));

            string folder = Path.GetDirectoryName(path);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);

            Exception exportError = null;

            try
            {
                using (var stream = File.Open(path, FileMode.Create))
                {
                    ExportPOCOs(
                        g => g.RedirectTo(stream),
                        g => g.RedirectTo(txtPocoEditor)
                    );
                }
            }
            catch (Exception ex)
            {
                this.generator.RedirectTo(txtPocoEditor);
                exportError = ex;
            }

            if (exportError != null)
            {
                SetStatusErrorMessage("Exporting failed. " + path);

                string errorMessage =
                    "Failed to export to file." +
                    Environment.NewLine +
                    string.Format("{0}: {1}", fileName, exportError.Message);
                MessageBox.Show(this, errorMessage, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SetStatusMessage("Exporting done. " + path);
            }
        }

        private string Export_SingleFile_GetFileName(int selectedDbObjectsCount, IDbObject dbObject)
        {
            string dbObjectName = null;

            if (selectedDbObjectsCount == 0 && dbObject != null)
            {
                dbObjectName = dbObject.ToString();
            }
            else if (selectedDbObjectsCount == 1)
            {
                if (this.selectedTables.HasSingle())
                    dbObjectName = this.selectedTables[0].ToString();
                else if (this.selectedViews.HasSingle())
                    dbObjectName = this.selectedViews[0].ToString();
                else if (this.selectedProcedures.HasSingle())
                    dbObjectName = this.selectedProcedures[0].ToString();
                else if (this.selectedFunctions.HasSingle())
                    dbObjectName = this.selectedFunctions[0].ToString();
                else if (this.selectedTVPs.HasSingle())
                    dbObjectName = this.selectedTVPs[0].ToString();
            }
            else if (selectedDbObjectsCount > 1)
            {
                var databases =
                    this.selectedTables.Select(x => x.Database).Union(
                    this.selectedViews.Select(x => x.Database)).Union(
                    this.selectedProcedures.Select(x => x.Database)).Union(
                    this.selectedFunctions.Select(x => x.Database)).Union(
                    this.selectedTVPs.Select(x => x.Database))
                    .Distinct()
                    .ToList();

                if (databases.HasSingle())
                    dbObjectName = databases[0].ToString();
            }

            if (string.IsNullOrEmpty(dbObjectName))
            {
                TreeNode serverNode = trvServer.Nodes[0];
                Server server = serverNode.Tag as Server;
                dbObjectName = server.ToString();
            }

            return string.Join("_", dbObjectName.Split(Path.GetInvalidFileNameChars())) + ".cs";
        }

        private void Export_MultipleFiles()
        {
            string folder = Path.GetFullPath(txtFolder.Text);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);

            int filesCount = 0;
            List<Tuple<string, Exception>> exportErrors = new List<Tuple<string, Exception>>();

            void tablePOCO(object sender1, TablePOCOEventArgs e1)
            {
                if (Export_MultipleFiles_WritePOCOToFile(e1.ClassName, e1.POCO, folder, exportErrors))
                    filesCount++;
            }

            void viewPOCO(object sender1, ViewPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_WritePOCOToFile(e1.ClassName, e1.POCO, folder, exportErrors))
                    filesCount++;
            }

            void procedurePOCO(object sender1, ProcedurePOCOEventArgs e1)
            {
                if (Export_MultipleFiles_WritePOCOToFile(e1.ClassName, e1.POCO, folder, exportErrors))
                    filesCount++;
            }

            void functionPOCO(object sender1, FunctionPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_WritePOCOToFile(e1.ClassName, e1.POCO, folder, exportErrors))
                    filesCount++;
            }

            void tvpPOCO(object sender1, TVPPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_WritePOCOToFile(e1.ClassName, e1.POCO, folder, exportErrors))
                    filesCount++;
            }

            ExportPOCOs(
                g =>
                {
                    g.Settings.POCO.WrapAroundEachClass = true;
                    g.ClearOut();
                    g.TablePOCO += tablePOCO;
                    g.ViewPOCO += viewPOCO;
                    g.ProcedurePOCO += procedurePOCO;
                    g.FunctionPOCO += functionPOCO;
                    g.TVPPOCO += tvpPOCO;
                },
                g =>
                {
                    g.TablePOCO -= tablePOCO;
                    g.ViewPOCO -= viewPOCO;
                    g.ProcedurePOCO -= procedurePOCO;
                    g.FunctionPOCO -= functionPOCO;
                    g.TVPPOCO -= tvpPOCO;
                    g.RedirectTo(txtPocoEditor);
                }
            );

            Export_MultipleFiles_SetExportMessage(exportErrors, folder, filesCount);
        }

        private bool Export_MultipleFiles_WritePOCOToFile(string className, string poco, string folder, List<Tuple<string, Exception>> exportErrors)
        {
            try
            {
                string fileName = string.Join("_", className.Split(Path.GetInvalidFileNameChars())) + ".cs";
                string path = Path.Combine(folder, fileName);
                File.WriteAllText(path, poco);
                return true;
            }
            catch (Exception ex)
            {
                exportErrors.Add(new Tuple<string, Exception>(className, ex));
                return false;
            }
        }

        private void Export_MultipleFiles_RelativeNamespace()
        {
            string folder = Path.GetFullPath(txtFolder.Text);
            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);

            string path = folder;
            int filesCount = 0;
            List<Tuple<string, Exception>> exportErrors = new List<Tuple<string, Exception>>();

            void serverGenerating(object sender1, ServerGeneratingEventArgs e1)
            {
                path = Path.Combine(
                    path,
                    string.Join("_", e1.Server.ToString().Split(Path.GetInvalidFileNameChars()))
                );

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void databaseGenerating(object sender1, DatabaseGeneratingEventArgs e1)
            {
                path = Path.Combine(
                    path,
                    string.Join("_", e1.Database.ToString().Split(Path.GetInvalidFileNameChars()))
                );

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void tablesGenerating(object sender1, TablesGeneratingEventArgs e1)
            {
                path = Path.Combine(path, "Tables");

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void viewsGenerating(object sender1, ViewsGeneratingEventArgs e1)
            {
                path = Path.Combine(path, "Views");

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void proceduresGenerating(object sender1, ProceduresGeneratingEventArgs e1)
            {
                path = Path.Combine(path, "Procedures");

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void functionsGenerating(object sender1, FunctionsGeneratingEventArgs e1)
            {
                path = Path.Combine(path, "Functions");

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void tvpsGenerating(object sender1, TVPsGeneratingEventArgs e1)
            {
                path = Path.Combine(path, "TVPs");

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
            }

            void tableGenerating(object sender1, TableGeneratingEventArgs e1)
            {
                e1.Namespace = Export_MultipleFiles_RelativeNamespace_GetNamespace(e1.Namespace, e1.Table.Database, "Tables", e1.Table.Schema);
            }

            void viewGenerating(object sender1, ViewGeneratingEventArgs e1)
            {
                e1.Namespace = Export_MultipleFiles_RelativeNamespace_GetNamespace(e1.Namespace, e1.View.Database, "Views", e1.View.Schema);
            }

            void procedureGenerating(object sender1, ProcedureGeneratingEventArgs e1)
            {
                e1.Namespace = Export_MultipleFiles_RelativeNamespace_GetNamespace(e1.Namespace, e1.Procedure.Database, "Procedures", e1.Procedure.Schema);
            }

            void functionGenerating(object sender1, FunctionGeneratingEventArgs e1)
            {
                e1.Namespace = Export_MultipleFiles_RelativeNamespace_GetNamespace(e1.Namespace, e1.Function.Database, "Functions", e1.Function.Schema);
            }

            void tvpGenerating(object sender1, TVPGeneratingEventArgs e1)
            {
                e1.Namespace = Export_MultipleFiles_RelativeNamespace_GetNamespace(e1.Namespace, e1.TVP.Database, "TVPs", e1.TVP.Schema);
            }

            void tablePOCO(object sender1, TablePOCOEventArgs e1)
            {
                if (Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(e1.ClassName, e1.POCO, e1.Table.Schema, path, exportErrors))
                    filesCount++;
            }

            void viewPOCO(object sender1, ViewPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(e1.ClassName, e1.POCO, e1.View.Schema, path, exportErrors))
                    filesCount++;
            }

            void procedurePOCO(object sender1, ProcedurePOCOEventArgs e1)
            {
                if (Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(e1.ClassName, e1.POCO, e1.Procedure.Schema, path, exportErrors))
                    filesCount++;
            }

            void functionPOCO(object sender1, FunctionPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(e1.ClassName, e1.POCO, e1.Function.Schema, path, exportErrors))
                    filesCount++;
            }

            void tvpPOCO(object sender1, TVPPOCOEventArgs e1)
            {
                if (Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(e1.ClassName, e1.POCO, e1.TVP.Schema, path, exportErrors))
                    filesCount++;
            }

            void tablesGenerated(object sender1, TablesGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void viewsGenerated(object sender1, ViewsGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void proceduresGenerated(object sender1, ProceduresGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void functionsGenerated(object sender1, FunctionsGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void tvpsGenerated(object sender1, TVPsGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void databaseGenerated(object sender1, DatabaseGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            void serverGenerated(object sender1, ServerGeneratedEventArgs e1)
            {
                path = Path.GetDirectoryName(path);
            }

            ExportPOCOs(
                g =>
                {
                    g.Settings.POCO.WrapAroundEachClass = true;
                    g.ClearOut();
                    g.ServerGenerating += serverGenerating;
                    g.DatabaseGenerating += databaseGenerating;
                    g.TablesGenerating += tablesGenerating;
                    g.ViewsGenerating += viewsGenerating;
                    g.ProceduresGenerating += proceduresGenerating;
                    g.FunctionsGenerating += functionsGenerating;
                    g.TVPsGenerating += tvpsGenerating;
                    g.TableGenerating += tableGenerating;
                    g.ViewGenerating += viewGenerating;
                    g.ProcedureGenerating += procedureGenerating;
                    g.FunctionGenerating += functionGenerating;
                    g.TVPGenerating += tvpGenerating;
                    g.TablePOCO += tablePOCO;
                    g.ViewPOCO += viewPOCO;
                    g.ProcedurePOCO += procedurePOCO;
                    g.FunctionPOCO += functionPOCO;
                    g.TVPPOCO += tvpPOCO;
                    g.TablesGenerated += tablesGenerated;
                    g.ViewsGenerated += viewsGenerated;
                    g.ProceduresGenerated += proceduresGenerated;
                    g.FunctionsGenerated += functionsGenerated;
                    g.TVPsGenerated += tvpsGenerated;
                    g.DatabaseGenerated += databaseGenerated;
                    g.ServerGenerated += serverGenerated;
                },
                g =>
                {
                    g.ServerGenerating -= serverGenerating;
                    g.DatabaseGenerating -= databaseGenerating;
                    g.TablesGenerating -= tablesGenerating;
                    g.ViewsGenerating -= viewsGenerating;
                    g.ProceduresGenerating -= proceduresGenerating;
                    g.FunctionsGenerating -= functionsGenerating;
                    g.TVPsGenerating -= tvpsGenerating;
                    g.TableGenerating -= tableGenerating;
                    g.ViewGenerating -= viewGenerating;
                    g.ProcedureGenerating -= procedureGenerating;
                    g.FunctionGenerating -= functionGenerating;
                    g.TVPGenerating -= tvpGenerating;
                    g.TablePOCO -= tablePOCO;
                    g.ViewPOCO -= viewPOCO;
                    g.ProcedurePOCO -= procedurePOCO;
                    g.FunctionPOCO -= functionPOCO;
                    g.TVPPOCO -= tvpPOCO;
                    g.TablesGenerated -= tablesGenerated;
                    g.ViewsGenerated -= viewsGenerated;
                    g.ProceduresGenerated -= proceduresGenerated;
                    g.FunctionsGenerated -= functionsGenerated;
                    g.TVPsGenerated -= tvpsGenerated;
                    g.DatabaseGenerated -= databaseGenerated;
                    g.ServerGenerated -= serverGenerated;
                    g.RedirectTo(txtPocoEditor);
                }
            );

            Export_MultipleFiles_SetExportMessage(exportErrors, folder, filesCount);
        }

        private string Export_MultipleFiles_RelativeNamespace_GetNamespace(string @namespace, Database database, string dbGroup, string schema)
        {
            if (string.IsNullOrEmpty(@namespace))
                @namespace = string.Format("{0}.{1}", database, dbGroup);
            else
                @namespace = string.Format("{0}.{1}.{2}", @namespace, database, dbGroup);
            if (string.IsNullOrEmpty(schema) == false)
                @namespace = string.Format("{0}.{1}", @namespace, schema);
            return @namespace;
        }

        private bool Export_MultipleFiles_RelativeNamespace_WritePOCOToFile(string className, string poco, string schema, string path, List<Tuple<string, Exception>> exportErrors)
        {
            try
            {
                schema = string.Join("_", (schema ?? string.Empty).Split(Path.GetInvalidFileNameChars()));

                path = Path.Combine(path, schema);

                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);

                string fileName = string.Join("_", className.Split(Path.GetInvalidFileNameChars())) + ".cs";
                path = Path.Combine(path, fileName);
                File.WriteAllText(path, poco);
                return true;
            }
            catch (Exception ex)
            {
                exportErrors.Add(new Tuple<string, Exception>(className, ex));
                return false;
            }
        }

        private void Export_MultipleFiles_SetExportMessage(List<Tuple<string, Exception>> exportErrors, string path, int filesCount)
        {
            if (exportErrors.HasAny())
            {
                SetStatusErrorMessage(
                    "Exporting failed. " +
                    (filesCount > 0 ? filesCount.ToString("N0") + " file" + (filesCount > 1 ? "s" : string.Empty) + ". " : string.Empty) +
                    path
                );

                string errorMessage =
                    string.Format("Failed to export {0:N0} file{1}.", exportErrors.Count, (exportErrors.HasAny() ? "s" : string.Empty)) +
                    Environment.NewLine +
                    string.Join(Environment.NewLine, exportErrors.Take(5).Select(x => string.Format("{0}: {1}", x.Item1, x.Item2.Message))) +
                    (exportErrors.Count > 5 ? Environment.NewLine + "and more." : string.Empty);
                MessageBox.Show(this, errorMessage, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SetStatusMessage(
                    "Exporting done. " +
                    (filesCount > 0 ? filesCount.ToString("N0") + " file" + (filesCount > 1 ? "s" : string.Empty) + ". " : string.Empty) +
                    path
                );
            }
        }

        #endregion

        #region Status Message

        private void SetStatusMessage(string message)
        {
            toolStripStatusLabel.Text = message;
            toolStripStatusLabel.ForeColor = Color.Black;
            Application.DoEvents();
        }

        private void SetStatusErrorMessage(string message)
        {
            toolStripStatusLabel.Text = message;
            toolStripStatusLabel.ForeColor = Color.Red;
            Application.DoEvents();
        }

        private void ClearStatus()
        {
            SetStatusMessage(string.Empty);
        }

        #endregion

        #region Copy

        private void btnCopy_Click(object sender, EventArgs e)
        {
            string text = txtPocoEditor.Text;
            if (string.IsNullOrEmpty(text))
                Clipboard.Clear();
            else
                Clipboard.SetText(text);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = txtPocoEditor.SelectedText;
            if (string.IsNullOrEmpty(text))
                Clipboard.Clear();
            else
                Clipboard.SetText(text);

            txtPocoEditor.Focus();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtPocoEditor.SelectAll();
            txtPocoEditor.Focus();
        }

        #endregion

        #region Type Mapping

        private TypesMappingForm SQLServerTypesMapping;
        private TypesMappingForm MySQLTypesMapping;

        private void btnTypeMapping_Click(object sender, EventArgs e)
        {
            if (this.rdbms == RDBMS.SQLServer || this.rdbms == RDBMS.None)
            {
                if (SQLServerTypesMapping == null)
                    SQLServerTypesMapping = new TypesMappingForm("SQL Server", GetSQLServerTypesMapping(), 465, 645);
                SQLServerTypesMapping.ShowDialog(this);
            }
            else if (this.rdbms == RDBMS.MySQL)
            {
                if (MySQLTypesMapping == null)
                    MySQLTypesMapping = new TypesMappingForm("MySQL", GetMySQLTypesMapping(), 576, 797);
                MySQLTypesMapping.ShowDialog(this);
            }
        }

        private static readonly Color textColor = Color.FromArgb(0, 0, 0);
        private static readonly Color keywordColor = Color.FromArgb(0, 0, 255);
        private static readonly Color userTypeColor = Color.FromArgb(43, 145, 175);

        private List<TypeMapping> GetSQLServerTypesMapping()
        {
            TypeMappingPart[] byteArray = new TypeMappingPart[] {
                new TypeMappingPart("byte", keywordColor),
                new TypeMappingPart("[]", textColor)
            };

            return new List<TypeMapping>()
            {
                new TypeMapping("bigint", keywordColor, "long", keywordColor),
                new TypeMapping("binary", keywordColor, byteArray),
                new TypeMapping("bit", keywordColor, "bool", keywordColor),
                new TypeMapping("char", keywordColor, "string", keywordColor),
                new TypeMapping("date", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("datetime", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("datetime2", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("datetimeoffset", keywordColor, "DateTimeOffset", userTypeColor),
                new TypeMapping("decimal", keywordColor, "decimal", keywordColor),
                new TypeMapping("filestream", keywordColor, byteArray),
                new TypeMapping("float", keywordColor, "double", keywordColor),

                new TypeMapping(
                    "geography", keywordColor,
                    new TypeMappingPart("Microsoft.SqlServer.Types.", textColor),
                    new TypeMappingPart("SqlGeography", userTypeColor)
                ),

                new TypeMapping(
                    "geometry", keywordColor,
                    new TypeMappingPart("Microsoft.SqlServer.Types.", textColor),
                    new TypeMappingPart("SqlGeometry", userTypeColor)
                ),

                new TypeMapping(
                    "hierarchyid", keywordColor,
                    new TypeMappingPart("Microsoft.SqlServer.Types.", textColor),
                    new TypeMappingPart("SqlHierarchyId", userTypeColor)
                ),

                new TypeMapping("image", keywordColor, byteArray),
                new TypeMapping("int", keywordColor, "int", keywordColor),
                new TypeMapping("money", keywordColor, "decimal", keywordColor),
                new TypeMapping("nchar", keywordColor, "string", keywordColor),
                new TypeMapping("ntext", keywordColor, "string", keywordColor),
                new TypeMapping("numeric", keywordColor, "decimal", keywordColor),
                new TypeMapping("nvarchar", keywordColor, "string", keywordColor),
                new TypeMapping("real", keywordColor, "float", keywordColor),
                new TypeMapping("rowversion", keywordColor, byteArray),
                new TypeMapping("smalldatetime", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("smallint", keywordColor, "short", keywordColor),
                new TypeMapping("smallmoney", keywordColor, "decimal", keywordColor),
                new TypeMapping("sql_variant", keywordColor, "object", keywordColor),
                new TypeMapping("text", keywordColor, "string", keywordColor),
                new TypeMapping("time", keywordColor, "TimeSpan", userTypeColor),
                new TypeMapping("timestamp", keywordColor, byteArray),
                new TypeMapping("tinyint", keywordColor, "byte", keywordColor),
                new TypeMapping("uniqueidentifier", keywordColor, "Guid", userTypeColor),
                new TypeMapping("varbinary", keywordColor, byteArray),
                new TypeMapping("varchar", keywordColor, "string", keywordColor),
                new TypeMapping("xml", keywordColor, "string", keywordColor),
                new TypeMapping("else", textColor, "object", keywordColor)
            };
        }

        private List<TypeMapping> GetMySQLTypesMapping()
        {
            TypeMappingPart[] byteArray = new TypeMappingPart[] {
                new TypeMappingPart("byte", keywordColor),
                new TypeMappingPart("[]", textColor)
            };

            return new List<TypeMapping>()
            {
                new TypeMapping("bigint", keywordColor, "long", keywordColor),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("bigint unsigned", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("serial", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("decimal", keywordColor) }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("binary", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("char byte", keywordColor)
                    },
                    byteArray
                ),

                new TypeMapping("bit", keywordColor, "bool", keywordColor),
                new TypeMapping("blob", keywordColor, byteArray),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("char", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("character", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("string", keywordColor) }
                ),

                new TypeMapping("date", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("datetime", keywordColor, "DateTime", userTypeColor),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("decimal", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("numeric", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("dec", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("fixed", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("decimal", keywordColor) }
                ),

                new TypeMapping("double", keywordColor, "double", keywordColor),
                new TypeMapping("double unsigned", keywordColor, "decimal", keywordColor),
                new TypeMapping("enum", keywordColor, "string", keywordColor),
                new TypeMapping("float", keywordColor, "float", keywordColor),
                new TypeMapping("float unsigned", keywordColor, "decimal", keywordColor),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("geometry", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("geometrycollection", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("geomcollection", keywordColor)
                    },
                    new TypeMappingPart[] {
                        new TypeMappingPart("System.Data.Spatial.", textColor),
                        new TypeMappingPart("DbGeometry", userTypeColor)
                    }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("int", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("integer", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("int", keywordColor) }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("int", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("integer unsigned", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("long", keywordColor) }
                ),

                new TypeMapping("json", keywordColor, "string", keywordColor),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("linestring", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("multilinestring", keywordColor)
                    },
                    new TypeMappingPart[] {
                        new TypeMappingPart("System.Data.Spatial.", textColor),
                        new TypeMappingPart("DbGeometry", userTypeColor)
                    }
                ),

                new TypeMapping("longblob", keywordColor, byteArray),
                new TypeMapping("longtext", keywordColor, "string", keywordColor),
                new TypeMapping("mediumblob", keywordColor, byteArray),
                new TypeMapping("mediumint", keywordColor, "int", keywordColor),
                new TypeMapping("mediumint unsigned", keywordColor, "int", keywordColor),
                new TypeMapping("mediumtext", keywordColor, "string", keywordColor),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("nchar", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("national char", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("string", keywordColor) }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("nvarchar", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("national varchar", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("string", keywordColor) }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("point", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("multipoint", keywordColor)
                    },
                    new TypeMappingPart[] {
                        new TypeMappingPart("System.Data.Spatial.", textColor),
                        new TypeMappingPart("DbGeometry", userTypeColor)
                    }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("polygon", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("multipolygon", keywordColor)
                    },
                    new TypeMappingPart[] {
                        new TypeMappingPart("System.Data.Spatial.", textColor),
                        new TypeMappingPart("DbGeometry", userTypeColor)
                    }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("real", keywordColor),
                        new TypeMappingPart(" (REAL_AS_FLOAT off)", textColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("double", keywordColor) }
                ),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("real", keywordColor),
                        new TypeMappingPart(" (REAL_AS_FLOAT on)", textColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("float", keywordColor) }
                ),

                new TypeMapping("set", keywordColor, "string", keywordColor),
                new TypeMapping("smallint", keywordColor, "short", keywordColor),
                new TypeMapping("smallint unsigned", keywordColor, "int", keywordColor),
                new TypeMapping("text", keywordColor, "string", keywordColor),
                new TypeMapping("time", keywordColor, "TimeSpan", userTypeColor),
                new TypeMapping("timestamp", keywordColor, "DateTime", userTypeColor),
                new TypeMapping("tinyblob", keywordColor, byteArray),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("tinyint(1)", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("bool", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("boolean", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("bool", keywordColor) }
                ),

                new TypeMapping("tinyint", keywordColor, "sbyte", keywordColor),
                new TypeMapping("tinyint unsigned", keywordColor, "byte", keywordColor),
                new TypeMapping("tinytext", keywordColor, "string", keywordColor),
                new TypeMapping("varbinary", keywordColor, byteArray),

                new TypeMapping(
                    new TypeMappingPart[] {
                        new TypeMappingPart("varchar", keywordColor),
                        new TypeMappingPart("/", textColor),
                        new TypeMappingPart("character varying", keywordColor)
                    },
                    new TypeMappingPart[] { new TypeMappingPart("string", keywordColor) }
                ),

                new TypeMapping("year", keywordColor, "short", keywordColor),
                new TypeMapping("else", textColor, "object", keywordColor)
            };
        }

        #endregion

        #region Disclaimer

        private void btnDisclaimer_Click(object sender, EventArgs e)
        {
            ShowDisclaimer();
        }

        private void ShowDisclaimer()
        {
            MessageBox.Show(this, Disclaimer.Message, "Disclaimer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region UI Settings

        private bool dbObjectsForm_IsEnableTables;
        private bool dbObjectsForm_IsEnableViews;
        private bool dbObjectsForm_IsEnableProcedures;
        private bool dbObjectsForm_IsEnableFunctions;
        private bool dbObjectsForm_IsEnableTVPs;

        private UISettings GetUISettings()
        {
            return new UISettings()
            {
                RDBMS = this.rdbms,
                ConnectionString = this.connectionString,

                SupportSchema = (this.generator != null ? this.generator.Support.SupportSchema : true),
                SupportTVPs = (this.generator != null ? this.generator.Support.SupportTVPs : true),
                SupportEnumDataType = (this.generator != null ? this.generator.Support.SupportEnumDataType : false),

                dbObjectsForm_IsEnableTables = this.dbObjectsForm_IsEnableTables,
                dbObjectsForm_IsEnableViews = this.dbObjectsForm_IsEnableViews,
                dbObjectsForm_IsEnableProcedures = this.dbObjectsForm_IsEnableProcedures,
                dbObjectsForm_IsEnableFunctions = this.dbObjectsForm_IsEnableFunctions,
                dbObjectsForm_IsEnableTVPs = this.dbObjectsForm_IsEnableTVPs,

                // POCO
                rdbProperties_Checked = rdbProperties.Checked,
                rdbFields_Checked = rdbFields.Checked,
                chkVirtualProperties_Checked = chkVirtualProperties.Checked,
                chkOverrideProperties_Checked = chkOverrideProperties.Checked,
                chkPartialClass_Checked = chkPartialClass.Checked,
                chkStructTypesNullable_Checked = chkStructTypesNullable.Checked,
                chkComments_Checked = chkComments.Checked,
                chkCommentsWithoutNull_Checked = chkCommentsWithoutNull.Checked,
                chkUsing_Checked = chkUsing.Checked,
                chkUsingInsideNamespace_Checked = chkUsingInsideNamespace.Checked,
                txtNamespace_Text = txtNamespace.Text,
                txtInherit_Text = txtInherit.Text,
                chkColumnDefaults_Checked = chkColumnDefaults.Checked,
                chkNewLineBetweenMembers_Checked = chkNewLineBetweenMembers.Checked,
                rdbEnumSQLTypeToString_Checked = rdbEnumSQLTypeToString.Checked,
                rdbEnumSQLTypeToEnumUShort_Checked = rdbEnumSQLTypeToEnumUShort.Checked,
                rdbEnumSQLTypeToEnumInt_Checked = rdbEnumSQLTypeToEnumInt.Checked,

                // Navigation Properties
                chkNavigationProperties_Checked = chkNavigationProperties.Checked,
                chkVirtualNavigationProperties_Checked = chkVirtualNavigationProperties.Checked,
                chkOverrideNavigationProperties_Checked = chkOverrideNavigationProperties.Checked,
                chkShowManyToManyJoinTable_Checked = chkShowManyToManyJoinTable.Checked,
                chkNavigationPropertiesComments_Checked = chkNavigationPropertiesComments.Checked,
                rdbListNavigationProperties_Checked = rdbListNavigationProperties.Checked,
                rdbICollectionNavigationProperties_Checked = rdbICollectionNavigationProperties.Checked,
                rdbIEnumerableNavigationProperties_Checked = rdbIEnumerableNavigationProperties.Checked,

                // Class Name
                chkSingular_Checked = chkSingular.Checked,
                chkIncludeDB_Checked = chkIncludeDB.Checked,
                txtDBSeparator_Text = txtDBSeparator.Text,
                chkIncludeSchema_Checked = chkIncludeSchema.Checked,
                chkIgnoreDboSchema_Checked = chkIgnoreDboSchema.Checked,
                txtSchemaSeparator_Text = txtSchemaSeparator.Text,
                txtWordsSeparator_Text = txtWordsSeparator.Text,
                chkCamelCase_Checked = chkCamelCase.Checked,
                chkUpperCase_Checked = chkUpperCase.Checked,
                chkLowerCase_Checked = chkLowerCase.Checked,
                txtSearch_Text = txtSearch.Text,
                txtReplace_Text = txtReplace.Text,
                chkSearchIgnoreCase_Checked = chkSearchIgnoreCase.Checked,
                txtFixedClassName_Text = txtFixedClassName.Text,
                txtPrefix_Text = txtPrefix.Text,
                txtSuffix_Text = txtSuffix.Text,

                // EF Annotations
                chkEF_Checked = chkEF.Checked,
                chkEFColumn_Checked = chkEFColumn.Checked,
                chkEFRequired_Checked = chkEFRequired.Checked,
                chkEFRequiredWithErrorMessage_Checked = chkEFRequiredWithErrorMessage.Checked,
                chkEFConcurrencyCheck_Checked = chkEFConcurrencyCheck.Checked,
                chkEFStringLength_Checked = chkEFStringLength.Checked,
                chkEFDisplay_Checked = chkEFDisplay.Checked,
                chkEFDescription_Checked = chkEFDescription.Checked,
                chkEFComplexType_Checked = chkEFComplexType.Checked,
                chkEFIndex_Checked = chkEFIndex.Checked,
                chkEFForeignKeyAndInverseProperty_Checked = chkEFForeignKeyAndInverseProperty.Checked,

                // Export To Files
                txtFolder_Text = txtFolder.Text,
                rdbSingleFile_Checked = rdbSingleFile.Checked,
                rdbMultipleFiles_Checked = rdbMultipleFiles.Checked,
                rdbMultipleFilesRelativeNamespace_Checked = rdbMultipleFilesRelativeNamespace.Checked
            };
        }

        private void LoadUISettings()
        {
            UISettings settings = DeserializeUISettings();
            if (settings == null)
                return;

            this.rdbms = settings.RDBMS;
            this.connectionString = settings.ConnectionString;

            this.dbObjectsForm_IsEnableTables = settings.dbObjectsForm_IsEnableTables;
            this.dbObjectsForm_IsEnableViews = settings.dbObjectsForm_IsEnableViews;
            this.dbObjectsForm_IsEnableProcedures = settings.dbObjectsForm_IsEnableProcedures;
            this.dbObjectsForm_IsEnableFunctions = settings.dbObjectsForm_IsEnableFunctions;
            this.dbObjectsForm_IsEnableTVPs = settings.dbObjectsForm_IsEnableTVPs;

            // POCO
            SetRadioButton(rdbProperties, rdbProperties_CheckedChanged, settings.rdbProperties_Checked);
            SetRadioButton(rdbFields, rdbFields_CheckedChanged, settings.rdbFields_Checked);
            SetCheckBox(chkVirtualProperties, chkVirtualProperties_CheckedChanged, settings.chkVirtualProperties_Checked);
            SetCheckBox(chkOverrideProperties, chkOverrideProperties_CheckedChanged, settings.chkOverrideProperties_Checked);
            SetCheckBox(chkPartialClass, chkPartialClass_CheckedChanged, settings.chkPartialClass_Checked);
            SetCheckBox(chkStructTypesNullable, chkStructTypesNullable_CheckedChanged, settings.chkStructTypesNullable_Checked);
            SetCheckBox(chkComments, chkComments_CheckedChanged, settings.chkComments_Checked);
            SetCheckBox(chkCommentsWithoutNull, chkCommentsWithoutNull_CheckedChanged, settings.chkCommentsWithoutNull_Checked);
            SetCheckBox(chkUsing, chkUsing_CheckedChanged, settings.chkUsing_Checked);
            SetCheckBox(chkUsingInsideNamespace, chkUsingInsideNamespace_CheckedChanged, settings.chkUsingInsideNamespace_Checked);
            SetTextBox(txtNamespace, txtNamespace_TextChanged, settings.txtNamespace_Text);
            SetTextBox(txtInherit, txtInherit_TextChanged, settings.txtInherit_Text);
            SetCheckBox(chkColumnDefaults, chkColumnDefaults_CheckedChanged, settings.chkColumnDefaults_Checked);
            SetCheckBox(chkNewLineBetweenMembers, chkNewLineBetweenMembers_CheckedChanged, settings.chkNewLineBetweenMembers_Checked);
            SetRadioButton(rdbEnumSQLTypeToString, rdbEnumSQLTypeToString_CheckedChanged, settings.rdbEnumSQLTypeToString_Checked);
            SetRadioButton(rdbEnumSQLTypeToEnumUShort, rdbEnumSQLTypeToEnumUShort_CheckedChanged, settings.rdbEnumSQLTypeToEnumUShort_Checked);
            SetRadioButton(rdbEnumSQLTypeToEnumInt, rdbEnumSQLTypeToEnumInt_CheckedChanged, settings.rdbEnumSQLTypeToEnumInt_Checked);

            // Navigation Properties
            SetCheckBox(chkNavigationProperties, chkNavigationProperties_CheckedChanged, settings.chkNavigationProperties_Checked);
            SetCheckBox(chkVirtualNavigationProperties, chkVirtualNavigationProperties_CheckedChanged, settings.chkVirtualNavigationProperties_Checked);
            SetCheckBox(chkOverrideNavigationProperties, chkOverrideNavigationProperties_CheckedChanged, settings.chkOverrideNavigationProperties_Checked);
            SetCheckBox(chkShowManyToManyJoinTable, chkShowManyToManyJoinTable_CheckedChanged, settings.chkShowManyToManyJoinTable_Checked);
            SetCheckBox(chkNavigationPropertiesComments, chkNavigationPropertiesComments_CheckedChanged, settings.chkNavigationPropertiesComments_Checked);
            SetRadioButton(rdbListNavigationProperties, rdbListNavigationProperties_CheckedChanged, settings.rdbListNavigationProperties_Checked);
            SetRadioButton(rdbICollectionNavigationProperties, rdbICollectionNavigationProperties_CheckedChanged, settings.rdbICollectionNavigationProperties_Checked);
            SetRadioButton(rdbIEnumerableNavigationProperties, rdbIEnumerableNavigationProperties_CheckedChanged, settings.rdbIEnumerableNavigationProperties_Checked);

            // Class Name
            SetCheckBox(chkSingular, chkSingular_CheckedChanged, settings.chkSingular_Checked);
            SetCheckBox(chkIncludeDB, chkIncludeDB_CheckedChanged, settings.chkIncludeDB_Checked);
            SetTextBox(txtDBSeparator, txtDBSeparator_TextChanged, settings.txtDBSeparator_Text);
            SetCheckBox(chkIncludeSchema, chkIncludeSchema_CheckedChanged, settings.chkIncludeSchema_Checked);
            SetCheckBox(chkIgnoreDboSchema, chkIgnoreDboSchema_CheckedChanged, settings.chkIgnoreDboSchema_Checked);
            SetTextBox(txtSchemaSeparator, txtSchemaSeparator_TextChanged, settings.txtSchemaSeparator_Text);
            SetTextBox(txtWordsSeparator, txtWordsSeparator_TextChanged, settings.txtWordsSeparator_Text);
            SetCheckBox(chkCamelCase, chkCamelCase_CheckedChanged, settings.chkCamelCase_Checked);
            SetCheckBox(chkUpperCase, chkUpperCase_CheckedChanged, settings.chkUpperCase_Checked);
            SetCheckBox(chkLowerCase, chkLowerCase_CheckedChanged, settings.chkLowerCase_Checked);
            SetTextBox(txtSearch, txtSearch_TextChanged, settings.txtSearch_Text);
            SetTextBox(txtReplace, txtReplace_TextChanged, settings.txtReplace_Text);
            SetCheckBox(chkSearchIgnoreCase, chkSearchIgnoreCase_CheckedChanged, settings.chkSearchIgnoreCase_Checked);
            SetTextBox(txtFixedClassName, txtFixedClassName_TextChanged, settings.txtFixedClassName_Text);
            SetTextBox(txtPrefix, txtPrefix_TextChanged, settings.txtPrefix_Text);
            SetTextBox(txtSuffix, txtSuffix_TextChanged, settings.txtSuffix_Text);

            // EF Annotations
            SetCheckBox(chkEF, chkEF_CheckedChanged, settings.chkEF_Checked);
            SetCheckBox(chkEFColumn, chkEFColumn_CheckedChanged, settings.chkEFColumn_Checked);
            SetCheckBox(chkEFRequired, chkEFRequired_CheckedChanged, settings.chkEFRequired_Checked);
            SetCheckBox(chkEFRequiredWithErrorMessage, chkEFRequiredWithErrorMessage_CheckedChanged, settings.chkEFRequiredWithErrorMessage_Checked);
            SetCheckBox(chkEFConcurrencyCheck, chkEFConcurrencyCheck_CheckedChanged, settings.chkEFConcurrencyCheck_Checked);
            SetCheckBox(chkEFStringLength, chkEFStringLength_CheckedChanged, settings.chkEFStringLength_Checked);
            SetCheckBox(chkEFDisplay, chkEFDisplay_CheckedChanged, settings.chkEFDisplay_Checked);
            SetCheckBox(chkEFDescription, chkEFDescription_CheckedChanged, settings.chkEFDescription_Checked);
            SetCheckBox(chkEFComplexType, chkEFComplexType_CheckedChanged, settings.chkEFComplexType_Checked);
            SetCheckBox(chkEFIndex, chkEFIndex_CheckedChanged, settings.chkEFIndex_Checked);
            SetCheckBox(chkEFForeignKeyAndInverseProperty, chkEFForeignKeyAndInverseProperty_CheckedChanged, settings.chkEFForeignKeyAndInverseProperty_Checked);

            // Export To Files
            txtFolder.Text = settings.txtFolder_Text;
            rdbSingleFile.Checked = settings.rdbSingleFile_Checked;
            rdbMultipleFiles.Checked = settings.rdbMultipleFiles_Checked;
            rdbMultipleFilesRelativeNamespace.Checked = settings.rdbMultipleFilesRelativeNamespace_Checked;

            SetFormControls(settings.SupportSchema, settings.SupportTVPs, settings.SupportEnumDataType);
        }

        private const string settingsFileName = "POCOGeneratorUI.settings";

        private void SerializeUISettings()
        {
            try
            {
                UISettings settings = GetUISettings();

                using (var fs = new FileStream(settingsFileName, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(fs, settings);
                }
            }
            catch
            {
            }
        }

        private UISettings DeserializeUISettings()
        {
            try
            {
                if (File.Exists(settingsFileName) == false)
                    return null;

                using (var fs = new FileStream(settingsFileName, FileMode.Open))
                {
                    return (UISettings)new BinaryFormatter().Deserialize(fs);
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
