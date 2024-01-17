namespace POCOGeneratorUI
{
    partial class POCOGeneratorForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POCOGeneratorForm));
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.trvServer = new System.Windows.Forms.TreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.removeFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.filterSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearCheckBoxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageListDbObjects = new System.Windows.Forms.ImageList(this.components);
			this.txtPocoEditor = new System.Windows.Forms.RichTextBox();
			this.contextMenuPocoEditor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panelMain = new System.Windows.Forms.Panel();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnDisclaimer = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.grbPOCO = new System.Windows.Forms.GroupBox();
			this.chkFactoryPattern = new System.Windows.Forms.CheckBox();
			this.chkImmutableClass = new System.Windows.Forms.CheckBox();
			this.rdbEnumSQLTypeToEnumInt = new System.Windows.Forms.RadioButton();
			this.lblSQLEnum = new System.Windows.Forms.Label();
			this.chkComplexTypes = new System.Windows.Forms.CheckBox();
			this.chkUsingInsideNamespace = new System.Windows.Forms.CheckBox();
			this.rdbEnumSQLTypeToEnumUShort = new System.Windows.Forms.RadioButton();
			this.panelEnum = new System.Windows.Forms.Panel();
			this.btnResetPOCOSettings = new System.Windows.Forms.Button();
			this.rdbEnumSQLTypeToString = new System.Windows.Forms.RadioButton();
			this.panelProperties = new System.Windows.Forms.Panel();
			this.rdbProperties = new System.Windows.Forms.RadioButton();
			this.rdbFields = new System.Windows.Forms.RadioButton();
			this.chkVirtualProperties = new System.Windows.Forms.CheckBox();
			this.chkStructTypesNullable = new System.Windows.Forms.CheckBox();
			this.chkComments = new System.Windows.Forms.CheckBox();
			this.chkCommentsWithoutNull = new System.Windows.Forms.CheckBox();
			this.chkColumnDefaults = new System.Windows.Forms.CheckBox();
			this.chkNewLineBetweenMembers = new System.Windows.Forms.CheckBox();
			this.lblNamespace = new System.Windows.Forms.Label();
			this.chkOverrideProperties = new System.Windows.Forms.CheckBox();
			this.txtNamespace = new System.Windows.Forms.TextBox();
			this.txtInherit = new System.Windows.Forms.TextBox();
			this.chkUsing = new System.Windows.Forms.CheckBox();
			this.lblInherit = new System.Windows.Forms.Label();
			this.chkPartialClass = new System.Windows.Forms.CheckBox();
			this.grbNavigationProperties = new System.Windows.Forms.GroupBox();
			this.chkNavigationProperties = new System.Windows.Forms.CheckBox();
			this.chkVirtualNavigationProperties = new System.Windows.Forms.CheckBox();
			this.btnResetNavigationPropertiesSettings = new System.Windows.Forms.Button();
			this.panelNavigationProperties = new System.Windows.Forms.Panel();
			this.rdbIListNavigationProperties = new System.Windows.Forms.RadioButton();
			this.rdbIEnumerableNavigationProperties = new System.Windows.Forms.RadioButton();
			this.rdbICollectionNavigationProperties = new System.Windows.Forms.RadioButton();
			this.rdbListNavigationProperties = new System.Windows.Forms.RadioButton();
			this.chkNavigationPropertiesComments = new System.Windows.Forms.CheckBox();
			this.chkManyToManyJoinTable = new System.Windows.Forms.CheckBox();
			this.chkOverrideNavigationProperties = new System.Windows.Forms.CheckBox();
			this.grbExportToFiles = new System.Windows.Forms.GroupBox();
			this.grbFileName = new System.Windows.Forms.GroupBox();
			this.rdbFileNameDatabaseSchemaName = new System.Windows.Forms.RadioButton();
			this.rdbFileNameDatabaseName = new System.Windows.Forms.RadioButton();
			this.rdbFileNameSchemaName = new System.Windows.Forms.RadioButton();
			this.rdbFileNameName = new System.Windows.Forms.RadioButton();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.rdbSingleFile = new System.Windows.Forms.RadioButton();
			this.rdbMultipleFilesSingleFolder = new System.Windows.Forms.RadioButton();
			this.rdbMultipleFilesRelativeFolders = new System.Windows.Forms.RadioButton();
			this.btnFolder = new System.Windows.Forms.Button();
			this.txtFolder = new System.Windows.Forms.TextBox();
			this.btnExport = new System.Windows.Forms.Button();
			this.grbClassName = new System.Windows.Forms.GroupBox();
			this.chkSingular = new System.Windows.Forms.CheckBox();
			this.btnResetClassNameSettings = new System.Windows.Forms.Button();
			this.txtFixedClassName = new System.Windows.Forms.TextBox();
			this.chkCamelCase = new System.Windows.Forms.CheckBox();
			this.chkUpperCase = new System.Windows.Forms.CheckBox();
			this.chkLowerCase = new System.Windows.Forms.CheckBox();
			this.lblWordsSeparator = new System.Windows.Forms.Label();
			this.txtWordsSeparator = new System.Windows.Forms.TextBox();
			this.lblPrefix = new System.Windows.Forms.Label();
			this.txtPrefix = new System.Windows.Forms.TextBox();
			this.lblSuffix = new System.Windows.Forms.Label();
			this.txtSuffix = new System.Windows.Forms.TextBox();
			this.lblWordsSeparatorDesc = new System.Windows.Forms.Label();
			this.lblFixedName = new System.Windows.Forms.Label();
			this.chkIncludeDB = new System.Windows.Forms.CheckBox();
			this.lblDBSeparator = new System.Windows.Forms.Label();
			this.chkSearchIgnoreCase = new System.Windows.Forms.CheckBox();
			this.txtDBSeparator = new System.Windows.Forms.TextBox();
			this.txtReplace = new System.Windows.Forms.TextBox();
			this.chkIncludeSchema = new System.Windows.Forms.CheckBox();
			this.lblReplace = new System.Windows.Forms.Label();
			this.chkIgnoreDboSchema = new System.Windows.Forms.CheckBox();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.lblSchemaSeparator = new System.Windows.Forms.Label();
			this.lblSearch = new System.Windows.Forms.Label();
			this.txtSchemaSeparator = new System.Windows.Forms.TextBox();
			this.lblSingularDesc = new System.Windows.Forms.Label();
			this.grbEFAnnotations = new System.Windows.Forms.GroupBox();
			this.btnResetEFAnnotationsSettings = new System.Windows.Forms.Button();
			this.chkEF = new System.Windows.Forms.CheckBox();
			this.chkEFColumn = new System.Windows.Forms.CheckBox();
			this.chkEFConcurrencyCheck = new System.Windows.Forms.CheckBox();
			this.chkEFComplexType = new System.Windows.Forms.CheckBox();
			this.chkEFDescription = new System.Windows.Forms.CheckBox();
			this.chkEFIndex = new System.Windows.Forms.CheckBox();
			this.chkEFForeignKeyAndInverseProperty = new System.Windows.Forms.CheckBox();
			this.chkEFStringLength = new System.Windows.Forms.CheckBox();
			this.chkEFDisplay = new System.Windows.Forms.CheckBox();
			this.chkEFRequired = new System.Windows.Forms.CheckBox();
			this.chkEFRequiredWithErrorMessage = new System.Windows.Forms.CheckBox();
			this.lblEF = new System.Windows.Forms.Label();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnTypeMapping = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.folderBrowserDialogExport = new System.Windows.Forms.FolderBrowserDialog();
			this.contextMenuTable = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.checkReferencedTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkReferencingTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkAccessibleTablesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.contextMenu.SuspendLayout();
			this.contextMenuPocoEditor.SuspendLayout();
			this.panelMain.SuspendLayout();
			this.grbPOCO.SuspendLayout();
			this.panelProperties.SuspendLayout();
			this.grbNavigationProperties.SuspendLayout();
			this.panelNavigationProperties.SuspendLayout();
			this.grbExportToFiles.SuspendLayout();
			this.grbFileName.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.grbClassName.SuspendLayout();
			this.grbEFAnnotations.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.contextMenuTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.panelMain);
			this.splitContainer1.Size = new System.Drawing.Size(1039, 712);
			this.splitContainer1.SplitterDistance = 310;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.trvServer);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.txtPocoEditor);
			this.splitContainer2.Size = new System.Drawing.Size(1039, 310);
			this.splitContainer2.SplitterDistance = 400;
			this.splitContainer2.TabIndex = 0;
			// 
			// trvServer
			// 
			this.trvServer.BackColor = System.Drawing.Color.White;
			this.trvServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.trvServer.CheckBoxes = true;
			this.trvServer.ContextMenuStrip = this.contextMenu;
			this.trvServer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvServer.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
			this.trvServer.HideSelection = false;
			this.trvServer.ImageIndex = 0;
			this.trvServer.ImageList = this.imageListDbObjects;
			this.trvServer.Location = new System.Drawing.Point(0, 0);
			this.trvServer.Name = "trvServer";
			this.trvServer.SelectedImageIndex = 0;
			this.trvServer.Size = new System.Drawing.Size(400, 310);
			this.trvServer.TabIndex = 1;
			this.trvServer.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.trvServer_AfterCheck);
			this.trvServer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvServer_BeforeExpand);
			this.trvServer.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvServer_DrawNode);
			this.trvServer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvServer_AfterSelect);
			this.trvServer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trvServer_MouseUp);
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFilterToolStripMenuItem,
            this.filterSettingsToolStripMenuItem,
            this.clearCheckBoxesToolStripMenuItem});
			this.contextMenu.Name = "contextMenuServerTree";
			this.contextMenu.Size = new System.Drawing.Size(169, 70);
			// 
			// removeFilterToolStripMenuItem
			// 
			this.removeFilterToolStripMenuItem.Name = "removeFilterToolStripMenuItem";
			this.removeFilterToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.removeFilterToolStripMenuItem.Text = "Remove Filter";
			this.removeFilterToolStripMenuItem.Click += new System.EventHandler(this.removeFilterToolStripMenuItem_Click);
			// 
			// filterSettingsToolStripMenuItem
			// 
			this.filterSettingsToolStripMenuItem.Name = "filterSettingsToolStripMenuItem";
			this.filterSettingsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.filterSettingsToolStripMenuItem.Text = "Filter Settings";
			this.filterSettingsToolStripMenuItem.Click += new System.EventHandler(this.filterSettingsToolStripMenuItem_Click);
			// 
			// clearCheckBoxesToolStripMenuItem
			// 
			this.clearCheckBoxesToolStripMenuItem.Name = "clearCheckBoxesToolStripMenuItem";
			this.clearCheckBoxesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
			this.clearCheckBoxesToolStripMenuItem.Text = "Clear Checkboxes";
			this.clearCheckBoxesToolStripMenuItem.Click += new System.EventHandler(this.clearCheckBoxesToolStripMenuItem_Click);
			// 
			// imageListDbObjects
			// 
			this.imageListDbObjects.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDbObjects.ImageStream")));
			this.imageListDbObjects.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListDbObjects.Images.SetKeyName(0, "Server.gif");
			this.imageListDbObjects.Images.SetKeyName(1, "Database.gif");
			this.imageListDbObjects.Images.SetKeyName(2, "Folder.gif");
			this.imageListDbObjects.Images.SetKeyName(3, "Table.gif");
			this.imageListDbObjects.Images.SetKeyName(4, "View.gif");
			this.imageListDbObjects.Images.SetKeyName(5, "Procedure.gif");
			this.imageListDbObjects.Images.SetKeyName(6, "Function.gif");
			this.imageListDbObjects.Images.SetKeyName(7, "TVP.gif");
			this.imageListDbObjects.Images.SetKeyName(8, "Column.gif");
			this.imageListDbObjects.Images.SetKeyName(9, "PrimaryKey.gif");
			this.imageListDbObjects.Images.SetKeyName(10, "ForeignKey.gif");
			this.imageListDbObjects.Images.SetKeyName(11, "UniqueKey.gif");
			this.imageListDbObjects.Images.SetKeyName(12, "Index.gif");
			// 
			// txtPocoEditor
			// 
			this.txtPocoEditor.BackColor = System.Drawing.Color.White;
			this.txtPocoEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPocoEditor.ContextMenuStrip = this.contextMenuPocoEditor;
			this.txtPocoEditor.DetectUrls = false;
			this.txtPocoEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtPocoEditor.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.txtPocoEditor.Location = new System.Drawing.Point(0, 0);
			this.txtPocoEditor.Name = "txtPocoEditor";
			this.txtPocoEditor.ReadOnly = true;
			this.txtPocoEditor.Size = new System.Drawing.Size(635, 310);
			this.txtPocoEditor.TabIndex = 0;
			this.txtPocoEditor.Text = "";
			this.txtPocoEditor.WordWrap = false;
			// 
			// contextMenuPocoEditor
			// 
			this.contextMenuPocoEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem});
			this.contextMenuPocoEditor.Name = "contextMenuPocoEditor";
			this.contextMenuPocoEditor.Size = new System.Drawing.Size(123, 48);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// panelMain
			// 
			this.panelMain.Controls.Add(this.btnReset);
			this.panelMain.Controls.Add(this.btnDisclaimer);
			this.panelMain.Controls.Add(this.btnConnect);
			this.panelMain.Controls.Add(this.grbPOCO);
			this.panelMain.Controls.Add(this.grbNavigationProperties);
			this.panelMain.Controls.Add(this.grbExportToFiles);
			this.panelMain.Controls.Add(this.grbClassName);
			this.panelMain.Controls.Add(this.grbEFAnnotations);
			this.panelMain.Controls.Add(this.statusStrip);
			this.panelMain.Controls.Add(this.btnCopy);
			this.panelMain.Controls.Add(this.btnTypeMapping);
			this.panelMain.Controls.Add(this.btnClose);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(1039, 398);
			this.panelMain.TabIndex = 0;
			// 
			// btnReset
			// 
			this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReset.AutoSize = true;
			this.btnReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnReset.Location = new System.Drawing.Point(765, 342);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(45, 23);
			this.btnReset.TabIndex = 8;
			this.btnReset.Text = "Reset";
			this.toolTip.SetToolTip(this.btnReset, "Reset settings to their default values.");
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnDisclaimer
			// 
			this.btnDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisclaimer.AutoSize = true;
			this.btnDisclaimer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnDisclaimer.Location = new System.Drawing.Point(905, 342);
			this.btnDisclaimer.Name = "btnDisclaimer";
			this.btnDisclaimer.Size = new System.Drawing.Size(65, 23);
			this.btnDisclaimer.TabIndex = 10;
			this.btnDisclaimer.Text = "Disclaimer";
			this.toolTip.SetToolTip(this.btnDisclaimer, "Disclaimer message about POCO Generator.");
			this.btnDisclaimer.UseVisualStyleBackColor = true;
			this.btnDisclaimer.Click += new System.EventHandler(this.btnDisclaimer_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.AutoSize = true;
			this.btnConnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnConnect.Location = new System.Drawing.Point(703, 342);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(57, 23);
			this.btnConnect.TabIndex = 7;
			this.btnConnect.Text = "Connect";
			this.toolTip.SetToolTip(this.btnConnect, "Connect to server.");
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// grbPOCO
			// 
			this.grbPOCO.AutoSize = true;
			this.grbPOCO.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbPOCO.Controls.Add(this.chkFactoryPattern);
			this.grbPOCO.Controls.Add(this.chkImmutableClass);
			this.grbPOCO.Controls.Add(this.rdbEnumSQLTypeToEnumInt);
			this.grbPOCO.Controls.Add(this.lblSQLEnum);
			this.grbPOCO.Controls.Add(this.chkComplexTypes);
			this.grbPOCO.Controls.Add(this.chkUsingInsideNamespace);
			this.grbPOCO.Controls.Add(this.rdbEnumSQLTypeToEnumUShort);
			this.grbPOCO.Controls.Add(this.panelEnum);
			this.grbPOCO.Controls.Add(this.btnResetPOCOSettings);
			this.grbPOCO.Controls.Add(this.rdbEnumSQLTypeToString);
			this.grbPOCO.Controls.Add(this.panelProperties);
			this.grbPOCO.Controls.Add(this.chkVirtualProperties);
			this.grbPOCO.Controls.Add(this.chkStructTypesNullable);
			this.grbPOCO.Controls.Add(this.chkComments);
			this.grbPOCO.Controls.Add(this.chkCommentsWithoutNull);
			this.grbPOCO.Controls.Add(this.chkColumnDefaults);
			this.grbPOCO.Controls.Add(this.chkNewLineBetweenMembers);
			this.grbPOCO.Controls.Add(this.lblNamespace);
			this.grbPOCO.Controls.Add(this.chkOverrideProperties);
			this.grbPOCO.Controls.Add(this.txtNamespace);
			this.grbPOCO.Controls.Add(this.txtInherit);
			this.grbPOCO.Controls.Add(this.chkUsing);
			this.grbPOCO.Controls.Add(this.lblInherit);
			this.grbPOCO.Controls.Add(this.chkPartialClass);
			this.grbPOCO.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grbPOCO.Location = new System.Drawing.Point(12, 5);
			this.grbPOCO.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbPOCO.Name = "grbPOCO";
			this.grbPOCO.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbPOCO.Size = new System.Drawing.Size(285, 233);
			this.grbPOCO.TabIndex = 1;
			this.grbPOCO.TabStop = false;
			this.grbPOCO.Text = "POCO";
			this.grbPOCO.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// chkFactoryPattern
			// 
			this.chkFactoryPattern.AutoSize = true;
			this.chkFactoryPattern.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkFactoryPattern.Location = new System.Drawing.Point(129, 44);
			this.chkFactoryPattern.Name = "chkFactoryPattern";
			this.chkFactoryPattern.Size = new System.Drawing.Size(100, 17);
			this.chkFactoryPattern.TabIndex = 18;
			this.chkFactoryPattern.Text = "Factory Method";
			this.toolTip.SetToolTip(this.chkFactoryPattern, "Include a static \"factory method\" in the class that will create an instance of th" +
        "e class an populate the class properties/fields.");
			this.chkFactoryPattern.UseVisualStyleBackColor = true;
			this.chkFactoryPattern.CheckedChanged += new System.EventHandler(this.chkFactoryPattern_CheckedChanged);
			// 
			// chkImmutableClass
			// 
			this.chkImmutableClass.AutoSize = true;
			this.chkImmutableClass.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkImmutableClass.Location = new System.Drawing.Point(6, 44);
			this.chkImmutableClass.Name = "chkImmutableClass";
			this.chkImmutableClass.Size = new System.Drawing.Size(102, 17);
			this.chkImmutableClass.TabIndex = 17;
			this.chkImmutableClass.Text = "Immutable Class";
			this.toolTip.SetToolTip(this.chkImmutableClass, "Create properties as read-only, with a class constructor used to set the property" +
        " values. ");
			this.chkImmutableClass.UseVisualStyleBackColor = true;
			this.chkImmutableClass.CheckedChanged += new System.EventHandler(this.chkImmutableClass_CheckedChanged);
			// 
			// rdbEnumSQLTypeToEnumInt
			// 
			this.rdbEnumSQLTypeToEnumInt.AutoSize = true;
			this.rdbEnumSQLTypeToEnumInt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbEnumSQLTypeToEnumInt.Location = new System.Drawing.Point(201, 200);
			this.rdbEnumSQLTypeToEnumInt.Name = "rdbEnumSQLTypeToEnumInt";
			this.rdbEnumSQLTypeToEnumInt.Size = new System.Drawing.Size(65, 17);
			this.rdbEnumSQLTypeToEnumInt.TabIndex = 3;
			this.rdbEnumSQLTypeToEnumInt.TabStop = true;
			this.rdbEnumSQLTypeToEnumInt.Text = "enum int";
			this.toolTip.SetToolTip(this.rdbEnumSQLTypeToEnumInt, "Enum and set type column will be generated as enumeration\r\nof type System.Int32 (" +
        "int).");
			this.rdbEnumSQLTypeToEnumInt.UseVisualStyleBackColor = true;
			this.rdbEnumSQLTypeToEnumInt.CheckedChanged += new System.EventHandler(this.rdbEnumSQLTypeToEnumInt_CheckedChanged);
			// 
			// lblSQLEnum
			// 
			this.lblSQLEnum.AutoSize = true;
			this.lblSQLEnum.Location = new System.Drawing.Point(6, 202);
			this.lblSQLEnum.Name = "lblSQLEnum";
			this.lblSQLEnum.Size = new System.Drawing.Size(64, 13);
			this.lblSQLEnum.TabIndex = 0;
			this.lblSQLEnum.Text = "Enum Type:";
			this.toolTip.SetToolTip(this.lblSQLEnum, "Determines how enum and set type columns are generated.");
			// 
			// chkComplexTypes
			// 
			this.chkComplexTypes.AutoSize = true;
			this.chkComplexTypes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkComplexTypes.Location = new System.Drawing.Point(181, 182);
			this.chkComplexTypes.Name = "chkComplexTypes";
			this.chkComplexTypes.Size = new System.Drawing.Size(98, 17);
			this.chkComplexTypes.TabIndex = 14;
			this.chkComplexTypes.Text = "Complex Types";
			this.toolTip.SetToolTip(this.chkComplexTypes, "Reverse-engineer existing Entity Framework\'s complex types\r\nin the database.");
			this.chkComplexTypes.UseVisualStyleBackColor = true;
			this.chkComplexTypes.CheckedChanged += new System.EventHandler(this.chkComplexTypes_CheckedChanged);
			// 
			// chkUsingInsideNamespace
			// 
			this.chkUsingInsideNamespace.AutoSize = true;
			this.chkUsingInsideNamespace.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUsingInsideNamespace.Location = new System.Drawing.Point(73, 137);
			this.chkUsingInsideNamespace.Name = "chkUsingInsideNamespace";
			this.chkUsingInsideNamespace.Size = new System.Drawing.Size(142, 17);
			this.chkUsingInsideNamespace.TabIndex = 10;
			this.chkUsingInsideNamespace.Text = "using Inside Namespace";
			this.toolTip.SetToolTip(this.chkUsingInsideNamespace, "If a custom namespace is set (Namespace setting), the using\r\nstatements are place" +
        "d inside the namespace declaration.");
			this.chkUsingInsideNamespace.UseVisualStyleBackColor = true;
			this.chkUsingInsideNamespace.CheckedChanged += new System.EventHandler(this.chkUsingInsideNamespace_CheckedChanged);
			// 
			// rdbEnumSQLTypeToEnumUShort
			// 
			this.rdbEnumSQLTypeToEnumUShort.AutoSize = true;
			this.rdbEnumSQLTypeToEnumUShort.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbEnumSQLTypeToEnumUShort.Location = new System.Drawing.Point(118, 200);
			this.rdbEnumSQLTypeToEnumUShort.Name = "rdbEnumSQLTypeToEnumUShort";
			this.rdbEnumSQLTypeToEnumUShort.Size = new System.Drawing.Size(83, 17);
			this.rdbEnumSQLTypeToEnumUShort.TabIndex = 2;
			this.rdbEnumSQLTypeToEnumUShort.TabStop = true;
			this.rdbEnumSQLTypeToEnumUShort.Text = "enum ushort";
			this.toolTip.SetToolTip(this.rdbEnumSQLTypeToEnumUShort, "Enum type column will be generated as enumeration of type\r\nSystem.UInt16 (ushort)" +
        " and set type column will be generated as\r\nbitwise enumeration of type System.UI" +
        "nt64 (ulong).");
			this.rdbEnumSQLTypeToEnumUShort.UseVisualStyleBackColor = true;
			this.rdbEnumSQLTypeToEnumUShort.CheckedChanged += new System.EventHandler(this.rdbEnumSQLTypeToEnumUShort_CheckedChanged);
			// 
			// panelEnum
			// 
			this.panelEnum.AutoSize = true;
			this.panelEnum.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelEnum.Location = new System.Drawing.Point(6, 188);
			this.panelEnum.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.panelEnum.Name = "panelEnum";
			this.panelEnum.Size = new System.Drawing.Size(0, 0);
			this.panelEnum.TabIndex = 15;
			this.panelEnum.TabStop = true;
			// 
			// btnResetPOCOSettings
			// 
			this.btnResetPOCOSettings.AutoSize = true;
			this.btnResetPOCOSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnResetPOCOSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetPOCOSettings.Location = new System.Drawing.Point(261, 9);
			this.btnResetPOCOSettings.Margin = new System.Windows.Forms.Padding(0);
			this.btnResetPOCOSettings.Name = "btnResetPOCOSettings";
			this.btnResetPOCOSettings.Size = new System.Drawing.Size(21, 19);
			this.btnResetPOCOSettings.TabIndex = 16;
			this.btnResetPOCOSettings.Text = "R";
			this.toolTip.SetToolTip(this.btnResetPOCOSettings, "Reset POCO settings to their default values.");
			this.btnResetPOCOSettings.UseVisualStyleBackColor = true;
			this.btnResetPOCOSettings.Click += new System.EventHandler(this.btnResetPOCOSettings_Click);
			// 
			// rdbEnumSQLTypeToString
			// 
			this.rdbEnumSQLTypeToString.AutoSize = true;
			this.rdbEnumSQLTypeToString.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbEnumSQLTypeToString.Checked = true;
			this.rdbEnumSQLTypeToString.Location = new System.Drawing.Point(68, 200);
			this.rdbEnumSQLTypeToString.Name = "rdbEnumSQLTypeToString";
			this.rdbEnumSQLTypeToString.Size = new System.Drawing.Size(50, 17);
			this.rdbEnumSQLTypeToString.TabIndex = 1;
			this.rdbEnumSQLTypeToString.TabStop = true;
			this.rdbEnumSQLTypeToString.Text = "string";
			this.toolTip.SetToolTip(this.rdbEnumSQLTypeToString, "The data member type will be string for both enum\r\nand set type columns.");
			this.rdbEnumSQLTypeToString.UseVisualStyleBackColor = true;
			this.rdbEnumSQLTypeToString.CheckedChanged += new System.EventHandler(this.rdbEnumSQLTypeToString_CheckedChanged);
			// 
			// panelProperties
			// 
			this.panelProperties.AutoSize = true;
			this.panelProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelProperties.Controls.Add(this.rdbProperties);
			this.panelProperties.Controls.Add(this.rdbFields);
			this.panelProperties.Location = new System.Drawing.Point(6, 19);
			this.panelProperties.Name = "panelProperties";
			this.panelProperties.Size = new System.Drawing.Size(135, 20);
			this.panelProperties.TabIndex = 1;
			this.panelProperties.TabStop = true;
			// 
			// rdbProperties
			// 
			this.rdbProperties.AutoSize = true;
			this.rdbProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbProperties.Checked = true;
			this.rdbProperties.Location = new System.Drawing.Point(0, 0);
			this.rdbProperties.Name = "rdbProperties";
			this.rdbProperties.Size = new System.Drawing.Size(72, 17);
			this.rdbProperties.TabIndex = 1;
			this.rdbProperties.TabStop = true;
			this.rdbProperties.Text = "Properties";
			this.toolTip.SetToolTip(this.rdbProperties, "Data members are constructed as properties (getter & setter).");
			this.rdbProperties.UseVisualStyleBackColor = true;
			this.rdbProperties.CheckedChanged += new System.EventHandler(this.rdbProperties_CheckedChanged);
			// 
			// rdbFields
			// 
			this.rdbFields.AutoSize = true;
			this.rdbFields.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbFields.Location = new System.Drawing.Point(80, 0);
			this.rdbFields.Name = "rdbFields";
			this.rdbFields.Size = new System.Drawing.Size(52, 17);
			this.rdbFields.TabIndex = 2;
			this.rdbFields.TabStop = true;
			this.rdbFields.Text = "Fields";
			this.toolTip.SetToolTip(this.rdbFields, "Data members are constructed as fields.");
			this.rdbFields.UseVisualStyleBackColor = true;
			this.rdbFields.CheckedChanged += new System.EventHandler(this.rdbFields_CheckedChanged);
			// 
			// chkVirtualProperties
			// 
			this.chkVirtualProperties.AutoSize = true;
			this.chkVirtualProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkVirtualProperties.Location = new System.Drawing.Point(6, 68);
			this.chkVirtualProperties.Name = "chkVirtualProperties";
			this.chkVirtualProperties.Size = new System.Drawing.Size(105, 17);
			this.chkVirtualProperties.TabIndex = 3;
			this.chkVirtualProperties.Text = "Virtual Properties";
			this.toolTip.SetToolTip(this.chkVirtualProperties, "Add virtual modifier to properties.");
			this.chkVirtualProperties.UseVisualStyleBackColor = true;
			this.chkVirtualProperties.CheckedChanged += new System.EventHandler(this.chkVirtualProperties_CheckedChanged);
			// 
			// chkStructTypesNullable
			// 
			this.chkStructTypesNullable.AutoSize = true;
			this.chkStructTypesNullable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkStructTypesNullable.Location = new System.Drawing.Point(6, 91);
			this.chkStructTypesNullable.Name = "chkStructTypesNullable";
			this.chkStructTypesNullable.Size = new System.Drawing.Size(127, 17);
			this.chkStructTypesNullable.TabIndex = 5;
			this.chkStructTypesNullable.Text = "Struct Types Nullable";
			this.toolTip.SetToolTip(this.chkStructTypesNullable, "struct data members will be constructed as nullable (int?)\r\neven if they are not " +
        "nullable in the database.");
			this.chkStructTypesNullable.UseVisualStyleBackColor = true;
			this.chkStructTypesNullable.CheckedChanged += new System.EventHandler(this.chkStructTypesNullable_CheckedChanged);
			// 
			// chkComments
			// 
			this.chkComments.AutoSize = true;
			this.chkComments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkComments.Location = new System.Drawing.Point(6, 114);
			this.chkComments.Name = "chkComments";
			this.chkComments.Size = new System.Drawing.Size(75, 17);
			this.chkComments.TabIndex = 7;
			this.chkComments.Text = "Comments";
			this.toolTip.SetToolTip(this.chkComments, "Add a comment, to data members, about the original\r\ndatabase column type and whet" +
        "her the column is nullable.");
			this.chkComments.UseVisualStyleBackColor = true;
			this.chkComments.CheckedChanged += new System.EventHandler(this.chkComments_CheckedChanged);
			// 
			// chkCommentsWithoutNull
			// 
			this.chkCommentsWithoutNull.AutoSize = true;
			this.chkCommentsWithoutNull.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkCommentsWithoutNull.Location = new System.Drawing.Point(97, 114);
			this.chkCommentsWithoutNull.Name = "chkCommentsWithoutNull";
			this.chkCommentsWithoutNull.Size = new System.Drawing.Size(134, 17);
			this.chkCommentsWithoutNull.TabIndex = 8;
			this.chkCommentsWithoutNull.Text = "Comments Without null";
			this.toolTip.SetToolTip(this.chkCommentsWithoutNull, "Add a comment, to data members, about the original\r\ndatabase column type.");
			this.chkCommentsWithoutNull.UseVisualStyleBackColor = true;
			this.chkCommentsWithoutNull.CheckedChanged += new System.EventHandler(this.chkCommentsWithoutNull_CheckedChanged);
			// 
			// chkColumnDefaults
			// 
			this.chkColumnDefaults.AutoSize = true;
			this.chkColumnDefaults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkColumnDefaults.Location = new System.Drawing.Point(149, 91);
			this.chkColumnDefaults.Name = "chkColumnDefaults";
			this.chkColumnDefaults.Size = new System.Drawing.Size(103, 17);
			this.chkColumnDefaults.TabIndex = 6;
			this.chkColumnDefaults.Text = "Column Defaults";
			this.toolTip.SetToolTip(this.chkColumnDefaults, "Add data member initialization based on\r\nthe column\'s default value in the databa" +
        "se.");
			this.chkColumnDefaults.UseVisualStyleBackColor = true;
			this.chkColumnDefaults.CheckedChanged += new System.EventHandler(this.chkColumnDefaults_CheckedChanged);
			// 
			// chkNewLineBetweenMembers
			// 
			this.chkNewLineBetweenMembers.AutoSize = true;
			this.chkNewLineBetweenMembers.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkNewLineBetweenMembers.Location = new System.Drawing.Point(6, 182);
			this.chkNewLineBetweenMembers.Name = "chkNewLineBetweenMembers";
			this.chkNewLineBetweenMembers.Size = new System.Drawing.Size(162, 17);
			this.chkNewLineBetweenMembers.TabIndex = 13;
			this.chkNewLineBetweenMembers.Text = "New Line Between Members";
			this.toolTip.SetToolTip(this.chkNewLineBetweenMembers, "Add empty lines between POCO data members.");
			this.chkNewLineBetweenMembers.UseVisualStyleBackColor = true;
			this.chkNewLineBetweenMembers.CheckedChanged += new System.EventHandler(this.chkNewLineBetweenMembers_CheckedChanged);
			// 
			// lblNamespace
			// 
			this.lblNamespace.AutoSize = true;
			this.lblNamespace.Location = new System.Drawing.Point(6, 163);
			this.lblNamespace.Name = "lblNamespace";
			this.lblNamespace.Size = new System.Drawing.Size(64, 13);
			this.lblNamespace.TabIndex = 0;
			this.lblNamespace.Text = "Namespace";
			this.toolTip.SetToolTip(this.lblNamespace, "Wraps all the POCOs with a custom namespace.");
			// 
			// chkOverrideProperties
			// 
			this.chkOverrideProperties.AutoSize = true;
			this.chkOverrideProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkOverrideProperties.Location = new System.Drawing.Point(127, 68);
			this.chkOverrideProperties.Name = "chkOverrideProperties";
			this.chkOverrideProperties.Size = new System.Drawing.Size(116, 17);
			this.chkOverrideProperties.TabIndex = 4;
			this.chkOverrideProperties.Text = "Override Properties";
			this.toolTip.SetToolTip(this.chkOverrideProperties, "Add override modifier to properties.");
			this.chkOverrideProperties.UseVisualStyleBackColor = true;
			this.chkOverrideProperties.CheckedChanged += new System.EventHandler(this.chkOverrideProperties_CheckedChanged);
			// 
			// txtNamespace
			// 
			this.txtNamespace.Location = new System.Drawing.Point(70, 159);
			this.txtNamespace.Name = "txtNamespace";
			this.txtNamespace.Size = new System.Drawing.Size(75, 20);
			this.txtNamespace.TabIndex = 11;
			this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
			// 
			// txtInherit
			// 
			this.txtInherit.Location = new System.Drawing.Point(189, 159);
			this.txtInherit.Name = "txtInherit";
			this.txtInherit.Size = new System.Drawing.Size(75, 20);
			this.txtInherit.TabIndex = 12;
			this.txtInherit.TextChanged += new System.EventHandler(this.txtInherit_TextChanged);
			// 
			// chkUsing
			// 
			this.chkUsing.AutoSize = true;
			this.chkUsing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUsing.Location = new System.Drawing.Point(6, 137);
			this.chkUsing.Name = "chkUsing";
			this.chkUsing.Size = new System.Drawing.Size(51, 17);
			this.chkUsing.TabIndex = 9;
			this.chkUsing.Text = "using";
			this.toolTip.SetToolTip(this.chkUsing, "Add using statements at the beginning of all the POCOs.\r\nIf a custom namespace is" +
        " set (Namespace setting), the using\r\nstatements are placed outside the namespace" +
        " declaration.");
			this.chkUsing.UseVisualStyleBackColor = true;
			this.chkUsing.CheckedChanged += new System.EventHandler(this.chkUsing_CheckedChanged);
			// 
			// lblInherit
			// 
			this.lblInherit.AutoSize = true;
			this.lblInherit.Location = new System.Drawing.Point(153, 163);
			this.lblInherit.Name = "lblInherit";
			this.lblInherit.Size = new System.Drawing.Size(36, 13);
			this.lblInherit.TabIndex = 0;
			this.lblInherit.Text = "Inherit";
			this.toolTip.SetToolTip(this.lblInherit, "Add a comma-delimited list of inherit class and interfaces.");
			// 
			// chkPartialClass
			// 
			this.chkPartialClass.AutoSize = true;
			this.chkPartialClass.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkPartialClass.Location = new System.Drawing.Point(157, 21);
			this.chkPartialClass.Name = "chkPartialClass";
			this.chkPartialClass.Size = new System.Drawing.Size(83, 17);
			this.chkPartialClass.TabIndex = 2;
			this.chkPartialClass.Text = "Partial Class";
			this.toolTip.SetToolTip(this.chkPartialClass, "Add partial modifier to the class.");
			this.chkPartialClass.UseVisualStyleBackColor = true;
			this.chkPartialClass.CheckedChanged += new System.EventHandler(this.chkPartialClass_CheckedChanged);
			// 
			// grbNavigationProperties
			// 
			this.grbNavigationProperties.AutoSize = true;
			this.grbNavigationProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbNavigationProperties.Controls.Add(this.chkNavigationProperties);
			this.grbNavigationProperties.Controls.Add(this.chkVirtualNavigationProperties);
			this.grbNavigationProperties.Controls.Add(this.btnResetNavigationPropertiesSettings);
			this.grbNavigationProperties.Controls.Add(this.panelNavigationProperties);
			this.grbNavigationProperties.Controls.Add(this.chkNavigationPropertiesComments);
			this.grbNavigationProperties.Controls.Add(this.chkManyToManyJoinTable);
			this.grbNavigationProperties.Controls.Add(this.chkOverrideNavigationProperties);
			this.grbNavigationProperties.Location = new System.Drawing.Point(12, 268);
			this.grbNavigationProperties.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbNavigationProperties.MinimumSize = new System.Drawing.Size(285, 0);
			this.grbNavigationProperties.Name = "grbNavigationProperties";
			this.grbNavigationProperties.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbNavigationProperties.Size = new System.Drawing.Size(286, 102);
			this.grbNavigationProperties.TabIndex = 3;
			this.grbNavigationProperties.TabStop = false;
			this.grbNavigationProperties.Text = "Navigation Properties";
			this.grbNavigationProperties.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// chkNavigationProperties
			// 
			this.chkNavigationProperties.AutoSize = true;
			this.chkNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkNavigationProperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkNavigationProperties.Location = new System.Drawing.Point(6, 19);
			this.chkNavigationProperties.Name = "chkNavigationProperties";
			this.chkNavigationProperties.Size = new System.Drawing.Size(127, 17);
			this.chkNavigationProperties.TabIndex = 1;
			this.chkNavigationProperties.Text = "Navigation Properties";
			this.toolTip.SetToolTip(this.chkNavigationProperties, "Add navigation properties and constructor\r\ninitialization, if necessary.");
			this.chkNavigationProperties.UseVisualStyleBackColor = true;
			this.chkNavigationProperties.CheckedChanged += new System.EventHandler(this.chkNavigationProperties_CheckedChanged);
			// 
			// chkVirtualNavigationProperties
			// 
			this.chkVirtualNavigationProperties.AutoSize = true;
			this.chkVirtualNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkVirtualNavigationProperties.Location = new System.Drawing.Point(6, 44);
			this.chkVirtualNavigationProperties.Name = "chkVirtualNavigationProperties";
			this.chkVirtualNavigationProperties.Size = new System.Drawing.Size(55, 17);
			this.chkVirtualNavigationProperties.TabIndex = 3;
			this.chkVirtualNavigationProperties.Text = "Virtual";
			this.toolTip.SetToolTip(this.chkVirtualNavigationProperties, "Add virtual modifier to the navigation properties.");
			this.chkVirtualNavigationProperties.UseVisualStyleBackColor = true;
			this.chkVirtualNavigationProperties.CheckedChanged += new System.EventHandler(this.chkVirtualNavigationProperties_CheckedChanged);
			// 
			// btnResetNavigationPropertiesSettings
			// 
			this.btnResetNavigationPropertiesSettings.AutoSize = true;
			this.btnResetNavigationPropertiesSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnResetNavigationPropertiesSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetNavigationPropertiesSettings.Location = new System.Drawing.Point(261, 9);
			this.btnResetNavigationPropertiesSettings.Margin = new System.Windows.Forms.Padding(0);
			this.btnResetNavigationPropertiesSettings.Name = "btnResetNavigationPropertiesSettings";
			this.btnResetNavigationPropertiesSettings.Size = new System.Drawing.Size(21, 19);
			this.btnResetNavigationPropertiesSettings.TabIndex = 7;
			this.btnResetNavigationPropertiesSettings.Text = "R";
			this.toolTip.SetToolTip(this.btnResetNavigationPropertiesSettings, "Reset Navigation Properties settings to their default values.");
			this.btnResetNavigationPropertiesSettings.UseVisualStyleBackColor = true;
			this.btnResetNavigationPropertiesSettings.Click += new System.EventHandler(this.btnResetNavigationPropertiesSettings_Click);
			// 
			// panelNavigationProperties
			// 
			this.panelNavigationProperties.AutoSize = true;
			this.panelNavigationProperties.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panelNavigationProperties.Controls.Add(this.rdbIListNavigationProperties);
			this.panelNavigationProperties.Controls.Add(this.rdbIEnumerableNavigationProperties);
			this.panelNavigationProperties.Controls.Add(this.rdbICollectionNavigationProperties);
			this.panelNavigationProperties.Controls.Add(this.rdbListNavigationProperties);
			this.panelNavigationProperties.Location = new System.Drawing.Point(6, 69);
			this.panelNavigationProperties.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.panelNavigationProperties.Name = "panelNavigationProperties";
			this.panelNavigationProperties.Size = new System.Drawing.Size(270, 20);
			this.panelNavigationProperties.TabIndex = 6;
			this.panelNavigationProperties.TabStop = true;
			// 
			// rdbIListNavigationProperties
			// 
			this.rdbIListNavigationProperties.AutoSize = true;
			this.rdbIListNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbIListNavigationProperties.Location = new System.Drawing.Point(49, 0);
			this.rdbIListNavigationProperties.Name = "rdbIListNavigationProperties";
			this.rdbIListNavigationProperties.Size = new System.Drawing.Size(44, 17);
			this.rdbIListNavigationProperties.TabIndex = 2;
			this.rdbIListNavigationProperties.TabStop = true;
			this.rdbIListNavigationProperties.Text = "IList";
			this.toolTip.SetToolTip(this.rdbIListNavigationProperties, "Set the collection navigation property to type IList\r\nand for constructor initial" +
        "ization set to type List.");
			this.rdbIListNavigationProperties.UseVisualStyleBackColor = true;
			this.rdbIListNavigationProperties.CheckedChanged += new System.EventHandler(this.rdbIListNavigationProperties_CheckedChanged);
			// 
			// rdbIEnumerableNavigationProperties
			// 
			this.rdbIEnumerableNavigationProperties.AutoSize = true;
			this.rdbIEnumerableNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbIEnumerableNavigationProperties.Location = new System.Drawing.Point(183, 0);
			this.rdbIEnumerableNavigationProperties.Name = "rdbIEnumerableNavigationProperties";
			this.rdbIEnumerableNavigationProperties.Size = new System.Drawing.Size(84, 17);
			this.rdbIEnumerableNavigationProperties.TabIndex = 4;
			this.rdbIEnumerableNavigationProperties.TabStop = true;
			this.rdbIEnumerableNavigationProperties.Text = "IEnumerable";
			this.toolTip.SetToolTip(this.rdbIEnumerableNavigationProperties, "Set the collection navigation property to type IEnumerable\r\nand for constructor i" +
        "nitialization set to type List.");
			this.rdbIEnumerableNavigationProperties.UseVisualStyleBackColor = true;
			this.rdbIEnumerableNavigationProperties.CheckedChanged += new System.EventHandler(this.rdbIEnumerableNavigationProperties_CheckedChanged);
			// 
			// rdbICollectionNavigationProperties
			// 
			this.rdbICollectionNavigationProperties.AutoSize = true;
			this.rdbICollectionNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbICollectionNavigationProperties.Location = new System.Drawing.Point(101, 0);
			this.rdbICollectionNavigationProperties.Name = "rdbICollectionNavigationProperties";
			this.rdbICollectionNavigationProperties.Size = new System.Drawing.Size(74, 17);
			this.rdbICollectionNavigationProperties.TabIndex = 3;
			this.rdbICollectionNavigationProperties.TabStop = true;
			this.rdbICollectionNavigationProperties.Text = "ICollection";
			this.toolTip.SetToolTip(this.rdbICollectionNavigationProperties, "Set the collection navigation property to type ICollection\r\nand for constructor i" +
        "nitialization set to type HashSet.");
			this.rdbICollectionNavigationProperties.UseVisualStyleBackColor = true;
			this.rdbICollectionNavigationProperties.CheckedChanged += new System.EventHandler(this.rdbICollectionNavigationProperties_CheckedChanged);
			// 
			// rdbListNavigationProperties
			// 
			this.rdbListNavigationProperties.AutoSize = true;
			this.rdbListNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbListNavigationProperties.Checked = true;
			this.rdbListNavigationProperties.Location = new System.Drawing.Point(0, 0);
			this.rdbListNavigationProperties.Name = "rdbListNavigationProperties";
			this.rdbListNavigationProperties.Size = new System.Drawing.Size(41, 17);
			this.rdbListNavigationProperties.TabIndex = 1;
			this.rdbListNavigationProperties.TabStop = true;
			this.rdbListNavigationProperties.Text = "List";
			this.toolTip.SetToolTip(this.rdbListNavigationProperties, "Set the collection navigation property to type List\r\nand for constructor initiali" +
        "zation set to type List.");
			this.rdbListNavigationProperties.UseVisualStyleBackColor = true;
			this.rdbListNavigationProperties.CheckedChanged += new System.EventHandler(this.rdbListNavigationProperties_CheckedChanged);
			// 
			// chkNavigationPropertiesComments
			// 
			this.chkNavigationPropertiesComments.AutoSize = true;
			this.chkNavigationPropertiesComments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkNavigationPropertiesComments.Location = new System.Drawing.Point(141, 19);
			this.chkNavigationPropertiesComments.Name = "chkNavigationPropertiesComments";
			this.chkNavigationPropertiesComments.Size = new System.Drawing.Size(75, 17);
			this.chkNavigationPropertiesComments.TabIndex = 2;
			this.chkNavigationPropertiesComments.Text = "Comments";
			this.toolTip.SetToolTip(this.chkNavigationPropertiesComments, "Add a comment about the underline foreign key\r\nof the navigation property.");
			this.chkNavigationPropertiesComments.UseVisualStyleBackColor = true;
			this.chkNavigationPropertiesComments.CheckedChanged += new System.EventHandler(this.chkNavigationPropertiesComments_CheckedChanged);
			// 
			// chkManyToManyJoinTable
			// 
			this.chkManyToManyJoinTable.AutoSize = true;
			this.chkManyToManyJoinTable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkManyToManyJoinTable.Location = new System.Drawing.Point(131, 44);
			this.chkManyToManyJoinTable.Name = "chkManyToManyJoinTable";
			this.chkManyToManyJoinTable.Size = new System.Drawing.Size(149, 17);
			this.chkManyToManyJoinTable.TabIndex = 5;
			this.chkManyToManyJoinTable.Text = "Many-To-Many Join Table";
			this.toolTip.SetToolTip(this.chkManyToManyJoinTable, "In a Many-to-Many relationship, the join table is hidden\r\nby default. When this s" +
        "etting is enabled, the join table is\r\nforcefully rendered.");
			this.chkManyToManyJoinTable.UseVisualStyleBackColor = true;
			this.chkManyToManyJoinTable.CheckedChanged += new System.EventHandler(this.chkManyToManyJoinTable_CheckedChanged);
			// 
			// chkOverrideNavigationProperties
			// 
			this.chkOverrideNavigationProperties.AutoSize = true;
			this.chkOverrideNavigationProperties.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkOverrideNavigationProperties.Location = new System.Drawing.Point(63, 44);
			this.chkOverrideNavigationProperties.Name = "chkOverrideNavigationProperties";
			this.chkOverrideNavigationProperties.Size = new System.Drawing.Size(66, 17);
			this.chkOverrideNavigationProperties.TabIndex = 4;
			this.chkOverrideNavigationProperties.Text = "Override";
			this.toolTip.SetToolTip(this.chkOverrideNavigationProperties, "Add override modifier to the navigation properties.");
			this.chkOverrideNavigationProperties.UseVisualStyleBackColor = true;
			this.chkOverrideNavigationProperties.CheckedChanged += new System.EventHandler(this.chkOverrideNavigationProperties_CheckedChanged);
			// 
			// grbExportToFiles
			// 
			this.grbExportToFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.grbExportToFiles.AutoSize = true;
			this.grbExportToFiles.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbExportToFiles.Controls.Add(this.grbFileName);
			this.grbExportToFiles.Controls.Add(this.flowLayoutPanel1);
			this.grbExportToFiles.Controls.Add(this.btnFolder);
			this.grbExportToFiles.Controls.Add(this.txtFolder);
			this.grbExportToFiles.Controls.Add(this.btnExport);
			this.grbExportToFiles.Location = new System.Drawing.Point(703, 111);
			this.grbExportToFiles.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbExportToFiles.Name = "grbExportToFiles";
			this.grbExportToFiles.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbExportToFiles.Size = new System.Drawing.Size(325, 213);
			this.grbExportToFiles.TabIndex = 5;
			this.grbExportToFiles.TabStop = false;
			this.grbExportToFiles.Text = "Export to Files";
			this.grbExportToFiles.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// grbFileName
			// 
			this.grbFileName.AutoSize = true;
			this.grbFileName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbFileName.Controls.Add(this.rdbFileNameDatabaseSchemaName);
			this.grbFileName.Controls.Add(this.rdbFileNameDatabaseName);
			this.grbFileName.Controls.Add(this.rdbFileNameSchemaName);
			this.grbFileName.Controls.Add(this.rdbFileNameName);
			this.grbFileName.Location = new System.Drawing.Point(6, 125);
			this.grbFileName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbFileName.Name = "grbFileName";
			this.grbFileName.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbFileName.Size = new System.Drawing.Size(263, 75);
			this.grbFileName.TabIndex = 5;
			this.grbFileName.TabStop = false;
			this.grbFileName.Text = "File Name";
			this.grbFileName.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// rdbFileNameDatabaseSchemaName
			// 
			this.rdbFileNameDatabaseSchemaName.AutoSize = true;
			this.rdbFileNameDatabaseSchemaName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbFileNameDatabaseSchemaName.Location = new System.Drawing.Point(113, 42);
			this.rdbFileNameDatabaseSchemaName.Name = "rdbFileNameDatabaseSchemaName";
			this.rdbFileNameDatabaseSchemaName.Size = new System.Drawing.Size(144, 17);
			this.rdbFileNameDatabaseSchemaName.TabIndex = 4;
			this.rdbFileNameDatabaseSchemaName.Text = "Database.Schema.Name";
			this.toolTip.SetToolTip(this.rdbFileNameDatabaseSchemaName, "Exported file names are Database.Schema.Name.cs.\r\nApplicable when exporting to mu" +
        "ltiple files.");
			this.rdbFileNameDatabaseSchemaName.UseVisualStyleBackColor = true;
			// 
			// rdbFileNameDatabaseName
			// 
			this.rdbFileNameDatabaseName.AutoSize = true;
			this.rdbFileNameDatabaseName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbFileNameDatabaseName.Location = new System.Drawing.Point(3, 42);
			this.rdbFileNameDatabaseName.Name = "rdbFileNameDatabaseName";
			this.rdbFileNameDatabaseName.Size = new System.Drawing.Size(102, 17);
			this.rdbFileNameDatabaseName.TabIndex = 3;
			this.rdbFileNameDatabaseName.Text = "Database.Name";
			this.toolTip.SetToolTip(this.rdbFileNameDatabaseName, "Exported file names are Database.Name.cs.\r\nApplicable when exporting to multiple " +
        "files.");
			this.rdbFileNameDatabaseName.UseVisualStyleBackColor = true;
			// 
			// rdbFileNameSchemaName
			// 
			this.rdbFileNameSchemaName.AutoSize = true;
			this.rdbFileNameSchemaName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbFileNameSchemaName.Location = new System.Drawing.Point(64, 19);
			this.rdbFileNameSchemaName.Name = "rdbFileNameSchemaName";
			this.rdbFileNameSchemaName.Size = new System.Drawing.Size(95, 17);
			this.rdbFileNameSchemaName.TabIndex = 2;
			this.rdbFileNameSchemaName.Text = "Schema.Name";
			this.toolTip.SetToolTip(this.rdbFileNameSchemaName, "Exported file names are Schema.Name.cs.\r\nApplicable when exporting to multiple fi" +
        "les.");
			this.rdbFileNameSchemaName.UseVisualStyleBackColor = true;
			// 
			// rdbFileNameName
			// 
			this.rdbFileNameName.AutoSize = true;
			this.rdbFileNameName.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbFileNameName.Checked = true;
			this.rdbFileNameName.Location = new System.Drawing.Point(3, 19);
			this.rdbFileNameName.Name = "rdbFileNameName";
			this.rdbFileNameName.Size = new System.Drawing.Size(53, 17);
			this.rdbFileNameName.TabIndex = 1;
			this.rdbFileNameName.TabStop = true;
			this.rdbFileNameName.Text = "Name";
			this.toolTip.SetToolTip(this.rdbFileNameName, "Exported file names are Name.cs.\r\nApplicable when exporting to multiple files.");
			this.rdbFileNameName.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.rdbSingleFile);
			this.flowLayoutPanel1.Controls.Add(this.rdbMultipleFilesSingleFolder);
			this.flowLayoutPanel1.Controls.Add(this.rdbMultipleFilesRelativeFolders);
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 50);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(176, 69);
			this.flowLayoutPanel1.TabIndex = 4;
			// 
			// rdbSingleFile
			// 
			this.rdbSingleFile.AutoSize = true;
			this.rdbSingleFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbSingleFile.Checked = true;
			this.rdbSingleFile.Location = new System.Drawing.Point(3, 3);
			this.rdbSingleFile.Name = "rdbSingleFile";
			this.rdbSingleFile.Size = new System.Drawing.Size(73, 17);
			this.rdbSingleFile.TabIndex = 1;
			this.rdbSingleFile.TabStop = true;
			this.rdbSingleFile.Text = "Single File";
			this.toolTip.SetToolTip(this.rdbSingleFile, "All the POCOs are saved to one file.\r\nThe file name is the name of the database.");
			this.rdbSingleFile.UseVisualStyleBackColor = true;
			// 
			// rdbMultipleFilesSingleFolder
			// 
			this.rdbMultipleFilesSingleFolder.AutoSize = true;
			this.rdbMultipleFilesSingleFolder.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbMultipleFilesSingleFolder.Location = new System.Drawing.Point(3, 26);
			this.rdbMultipleFilesSingleFolder.Name = "rdbMultipleFilesSingleFolder";
			this.rdbMultipleFilesSingleFolder.Size = new System.Drawing.Size(155, 17);
			this.rdbMultipleFilesSingleFolder.TabIndex = 2;
			this.rdbMultipleFilesSingleFolder.Text = "Multiple Files - Single Folder";
			this.toolTip.SetToolTip(this.rdbMultipleFilesSingleFolder, "Each POCO is saved to its own file.\r\nAll the files are saved to the root folder.");
			this.rdbMultipleFilesSingleFolder.UseVisualStyleBackColor = true;
			// 
			// rdbMultipleFilesRelativeFolders
			// 
			this.rdbMultipleFilesRelativeFolders.AutoSize = true;
			this.rdbMultipleFilesRelativeFolders.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.rdbMultipleFilesRelativeFolders.Location = new System.Drawing.Point(3, 49);
			this.rdbMultipleFilesRelativeFolders.Name = "rdbMultipleFilesRelativeFolders";
			this.rdbMultipleFilesRelativeFolders.Size = new System.Drawing.Size(170, 17);
			this.rdbMultipleFilesRelativeFolders.TabIndex = 3;
			this.rdbMultipleFilesRelativeFolders.Text = "Multiple Files - Relative Folders";
			this.toolTip.SetToolTip(this.rdbMultipleFilesRelativeFolders, resources.GetString("rdbMultipleFilesRelativeFolders.ToolTip"));
			this.rdbMultipleFilesRelativeFolders.UseVisualStyleBackColor = true;
			// 
			// btnFolder
			// 
			this.btnFolder.AutoSize = true;
			this.btnFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnFolder.Location = new System.Drawing.Point(6, 19);
			this.btnFolder.Name = "btnFolder";
			this.btnFolder.Size = new System.Drawing.Size(46, 23);
			this.btnFolder.TabIndex = 1;
			this.btnFolder.Text = "Folder";
			this.toolTip.SetToolTip(this.btnFolder, "Select root folder for export files.");
			this.btnFolder.UseVisualStyleBackColor = true;
			this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
			// 
			// txtFolder
			// 
			this.txtFolder.Location = new System.Drawing.Point(60, 20);
			this.txtFolder.Name = "txtFolder";
			this.txtFolder.Size = new System.Drawing.Size(205, 20);
			this.txtFolder.TabIndex = 2;
			// 
			// btnExport
			// 
			this.btnExport.AutoSize = true;
			this.btnExport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnExport.Location = new System.Drawing.Point(272, 19);
			this.btnExport.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(47, 23);
			this.btnExport.TabIndex = 3;
			this.btnExport.Text = "Export";
			this.toolTip.SetToolTip(this.btnExport, "Export POCOs to files.");
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// grbClassName
			// 
			this.grbClassName.AutoSize = true;
			this.grbClassName.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbClassName.Controls.Add(this.chkSingular);
			this.grbClassName.Controls.Add(this.btnResetClassNameSettings);
			this.grbClassName.Controls.Add(this.txtFixedClassName);
			this.grbClassName.Controls.Add(this.chkCamelCase);
			this.grbClassName.Controls.Add(this.chkUpperCase);
			this.grbClassName.Controls.Add(this.chkLowerCase);
			this.grbClassName.Controls.Add(this.lblWordsSeparator);
			this.grbClassName.Controls.Add(this.txtWordsSeparator);
			this.grbClassName.Controls.Add(this.lblPrefix);
			this.grbClassName.Controls.Add(this.txtPrefix);
			this.grbClassName.Controls.Add(this.lblSuffix);
			this.grbClassName.Controls.Add(this.txtSuffix);
			this.grbClassName.Controls.Add(this.lblWordsSeparatorDesc);
			this.grbClassName.Controls.Add(this.lblFixedName);
			this.grbClassName.Controls.Add(this.chkIncludeDB);
			this.grbClassName.Controls.Add(this.lblDBSeparator);
			this.grbClassName.Controls.Add(this.chkSearchIgnoreCase);
			this.grbClassName.Controls.Add(this.txtDBSeparator);
			this.grbClassName.Controls.Add(this.txtReplace);
			this.grbClassName.Controls.Add(this.chkIncludeSchema);
			this.grbClassName.Controls.Add(this.lblReplace);
			this.grbClassName.Controls.Add(this.chkIgnoreDboSchema);
			this.grbClassName.Controls.Add(this.txtSearch);
			this.grbClassName.Controls.Add(this.lblSchemaSeparator);
			this.grbClassName.Controls.Add(this.lblSearch);
			this.grbClassName.Controls.Add(this.txtSchemaSeparator);
			this.grbClassName.Controls.Add(this.lblSingularDesc);
			this.grbClassName.Location = new System.Drawing.Point(305, 5);
			this.grbClassName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbClassName.Name = "grbClassName";
			this.grbClassName.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbClassName.Size = new System.Drawing.Size(390, 194);
			this.grbClassName.TabIndex = 2;
			this.grbClassName.TabStop = false;
			this.grbClassName.Text = "Class Name";
			this.grbClassName.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// chkSingular
			// 
			this.chkSingular.AutoSize = true;
			this.chkSingular.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkSingular.Location = new System.Drawing.Point(6, 19);
			this.chkSingular.Name = "chkSingular";
			this.chkSingular.Size = new System.Drawing.Size(64, 17);
			this.chkSingular.TabIndex = 1;
			this.chkSingular.Text = "Singular";
			this.toolTip.SetToolTip(this.chkSingular, "Change the class name from plural to singular.\r\nApplicable only for tables, views" +
        " & TVPs.");
			this.chkSingular.UseVisualStyleBackColor = true;
			this.chkSingular.CheckedChanged += new System.EventHandler(this.chkSingular_CheckedChanged);
			// 
			// btnResetClassNameSettings
			// 
			this.btnResetClassNameSettings.AutoSize = true;
			this.btnResetClassNameSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnResetClassNameSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetClassNameSettings.Location = new System.Drawing.Point(366, 9);
			this.btnResetClassNameSettings.Margin = new System.Windows.Forms.Padding(0);
			this.btnResetClassNameSettings.Name = "btnResetClassNameSettings";
			this.btnResetClassNameSettings.Size = new System.Drawing.Size(21, 19);
			this.btnResetClassNameSettings.TabIndex = 17;
			this.btnResetClassNameSettings.Text = "R";
			this.toolTip.SetToolTip(this.btnResetClassNameSettings, "Reset Class Name settings to their default values.");
			this.btnResetClassNameSettings.UseVisualStyleBackColor = true;
			this.btnResetClassNameSettings.Click += new System.EventHandler(this.btnResetClassNameSettings_Click);
			// 
			// txtFixedClassName
			// 
			this.txtFixedClassName.Location = new System.Drawing.Point(69, 161);
			this.txtFixedClassName.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.txtFixedClassName.Name = "txtFixedClassName";
			this.txtFixedClassName.Size = new System.Drawing.Size(75, 20);
			this.txtFixedClassName.TabIndex = 14;
			this.txtFixedClassName.TextChanged += new System.EventHandler(this.txtFixedClassName_TextChanged);
			// 
			// chkCamelCase
			// 
			this.chkCamelCase.AutoSize = true;
			this.chkCamelCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkCamelCase.Location = new System.Drawing.Point(6, 115);
			this.chkCamelCase.Name = "chkCamelCase";
			this.chkCamelCase.Size = new System.Drawing.Size(79, 17);
			this.chkCamelCase.TabIndex = 8;
			this.chkCamelCase.Text = "CamelCase";
			this.toolTip.SetToolTip(this.chkCamelCase, "Change class name to camel case.");
			this.chkCamelCase.UseVisualStyleBackColor = true;
			this.chkCamelCase.CheckedChanged += new System.EventHandler(this.chkCamelCase_CheckedChanged);
			// 
			// chkUpperCase
			// 
			this.chkUpperCase.AutoSize = true;
			this.chkUpperCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkUpperCase.Location = new System.Drawing.Point(93, 115);
			this.chkUpperCase.Name = "chkUpperCase";
			this.chkUpperCase.Size = new System.Drawing.Size(94, 17);
			this.chkUpperCase.TabIndex = 9;
			this.chkUpperCase.Text = "UPPER CASE";
			this.toolTip.SetToolTip(this.chkUpperCase, "Change class name to upper case.");
			this.chkUpperCase.UseVisualStyleBackColor = true;
			this.chkUpperCase.CheckedChanged += new System.EventHandler(this.chkUpperCase_CheckedChanged);
			// 
			// chkLowerCase
			// 
			this.chkLowerCase.AutoSize = true;
			this.chkLowerCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkLowerCase.Location = new System.Drawing.Point(195, 115);
			this.chkLowerCase.Name = "chkLowerCase";
			this.chkLowerCase.Size = new System.Drawing.Size(77, 17);
			this.chkLowerCase.TabIndex = 10;
			this.chkLowerCase.Text = "lower case";
			this.toolTip.SetToolTip(this.chkLowerCase, "Change class name to lower case.");
			this.chkLowerCase.UseVisualStyleBackColor = true;
			this.chkLowerCase.CheckedChanged += new System.EventHandler(this.chkLowerCase_CheckedChanged);
			// 
			// lblWordsSeparator
			// 
			this.lblWordsSeparator.AutoSize = true;
			this.lblWordsSeparator.Location = new System.Drawing.Point(6, 94);
			this.lblWordsSeparator.Name = "lblWordsSeparator";
			this.lblWordsSeparator.Size = new System.Drawing.Size(87, 13);
			this.lblWordsSeparator.TabIndex = 0;
			this.lblWordsSeparator.Text = "Words Separator";
			this.toolTip.SetToolTip(this.lblWordsSeparator, "Add a separator between words.\r\nWord are text between underscores or in a camel c" +
        "ase.");
			// 
			// txtWordsSeparator
			// 
			this.txtWordsSeparator.Location = new System.Drawing.Point(93, 90);
			this.txtWordsSeparator.Name = "txtWordsSeparator";
			this.txtWordsSeparator.Size = new System.Drawing.Size(45, 20);
			this.txtWordsSeparator.TabIndex = 7;
			this.txtWordsSeparator.TextChanged += new System.EventHandler(this.txtWordsSeparator_TextChanged);
			// 
			// lblPrefix
			// 
			this.lblPrefix.AutoSize = true;
			this.lblPrefix.Location = new System.Drawing.Point(152, 165);
			this.lblPrefix.Name = "lblPrefix";
			this.lblPrefix.Size = new System.Drawing.Size(33, 13);
			this.lblPrefix.TabIndex = 0;
			this.lblPrefix.Text = "Prefix";
			this.toolTip.SetToolTip(this.lblPrefix, "Add prefix text to the class name.");
			// 
			// txtPrefix
			// 
			this.txtPrefix.Location = new System.Drawing.Point(185, 161);
			this.txtPrefix.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.txtPrefix.Name = "txtPrefix";
			this.txtPrefix.Size = new System.Drawing.Size(75, 20);
			this.txtPrefix.TabIndex = 15;
			this.txtPrefix.TextChanged += new System.EventHandler(this.txtPrefix_TextChanged);
			// 
			// lblSuffix
			// 
			this.lblSuffix.AutoSize = true;
			this.lblSuffix.Location = new System.Drawing.Point(268, 165);
			this.lblSuffix.Name = "lblSuffix";
			this.lblSuffix.Size = new System.Drawing.Size(33, 13);
			this.lblSuffix.TabIndex = 0;
			this.lblSuffix.Text = "Suffix";
			this.toolTip.SetToolTip(this.lblSuffix, "Add suffix text to the class name.");
			// 
			// txtSuffix
			// 
			this.txtSuffix.Location = new System.Drawing.Point(301, 161);
			this.txtSuffix.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.txtSuffix.Name = "txtSuffix";
			this.txtSuffix.Size = new System.Drawing.Size(75, 20);
			this.txtSuffix.TabIndex = 16;
			this.txtSuffix.TextChanged += new System.EventHandler(this.txtSuffix_TextChanged);
			// 
			// lblWordsSeparatorDesc
			// 
			this.lblWordsSeparatorDesc.AutoSize = true;
			this.lblWordsSeparatorDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
			this.lblWordsSeparatorDesc.Location = new System.Drawing.Point(146, 94);
			this.lblWordsSeparatorDesc.Name = "lblWordsSeparatorDesc";
			this.lblWordsSeparatorDesc.Size = new System.Drawing.Size(214, 13);
			this.lblWordsSeparatorDesc.TabIndex = 0;
			this.lblWordsSeparatorDesc.Text = "(Words between _ and words in CamelCase)";
			// 
			// lblFixedName
			// 
			this.lblFixedName.AutoSize = true;
			this.lblFixedName.Location = new System.Drawing.Point(6, 165);
			this.lblFixedName.Name = "lblFixedName";
			this.lblFixedName.Size = new System.Drawing.Size(63, 13);
			this.lblFixedName.TabIndex = 0;
			this.lblFixedName.Text = "Fixed Name";
			this.toolTip.SetToolTip(this.lblFixedName, "Set the name of the class to a fixed name.");
			// 
			// chkIncludeDB
			// 
			this.chkIncludeDB.AutoSize = true;
			this.chkIncludeDB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkIncludeDB.Location = new System.Drawing.Point(6, 44);
			this.chkIncludeDB.Name = "chkIncludeDB";
			this.chkIncludeDB.Size = new System.Drawing.Size(79, 17);
			this.chkIncludeDB.TabIndex = 2;
			this.chkIncludeDB.Text = "Include DB";
			this.toolTip.SetToolTip(this.chkIncludeDB, "Add the database name.");
			this.chkIncludeDB.UseVisualStyleBackColor = true;
			this.chkIncludeDB.CheckedChanged += new System.EventHandler(this.chkIncludeDB_CheckedChanged);
			// 
			// lblDBSeparator
			// 
			this.lblDBSeparator.AutoSize = true;
			this.lblDBSeparator.Location = new System.Drawing.Point(93, 46);
			this.lblDBSeparator.Name = "lblDBSeparator";
			this.lblDBSeparator.Size = new System.Drawing.Size(71, 13);
			this.lblDBSeparator.TabIndex = 0;
			this.lblDBSeparator.Text = "DB Separator";
			this.toolTip.SetToolTip(this.lblDBSeparator, "Add a separator after the database name.");
			// 
			// chkSearchIgnoreCase
			// 
			this.chkSearchIgnoreCase.AutoSize = true;
			this.chkSearchIgnoreCase.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkSearchIgnoreCase.Location = new System.Drawing.Point(260, 138);
			this.chkSearchIgnoreCase.Name = "chkSearchIgnoreCase";
			this.chkSearchIgnoreCase.Size = new System.Drawing.Size(83, 17);
			this.chkSearchIgnoreCase.TabIndex = 13;
			this.chkSearchIgnoreCase.Text = "Ignore Case";
			this.toolTip.SetToolTip(this.chkSearchIgnoreCase, "Enable case-insensitive search.");
			this.chkSearchIgnoreCase.UseVisualStyleBackColor = true;
			this.chkSearchIgnoreCase.CheckedChanged += new System.EventHandler(this.chkSearchIgnoreCase_CheckedChanged);
			// 
			// txtDBSeparator
			// 
			this.txtDBSeparator.Location = new System.Drawing.Point(164, 42);
			this.txtDBSeparator.Name = "txtDBSeparator";
			this.txtDBSeparator.Size = new System.Drawing.Size(45, 20);
			this.txtDBSeparator.TabIndex = 3;
			this.txtDBSeparator.TextChanged += new System.EventHandler(this.txtDBSeparator_TextChanged);
			// 
			// txtReplace
			// 
			this.txtReplace.Location = new System.Drawing.Point(177, 136);
			this.txtReplace.Name = "txtReplace";
			this.txtReplace.Size = new System.Drawing.Size(75, 20);
			this.txtReplace.TabIndex = 12;
			this.txtReplace.TextChanged += new System.EventHandler(this.txtReplace_TextChanged);
			// 
			// chkIncludeSchema
			// 
			this.chkIncludeSchema.AutoSize = true;
			this.chkIncludeSchema.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkIncludeSchema.Location = new System.Drawing.Point(6, 69);
			this.chkIncludeSchema.Name = "chkIncludeSchema";
			this.chkIncludeSchema.Size = new System.Drawing.Size(103, 17);
			this.chkIncludeSchema.TabIndex = 4;
			this.chkIncludeSchema.Text = "Include Schema";
			this.toolTip.SetToolTip(this.chkIncludeSchema, "Add the schema name.");
			this.chkIncludeSchema.UseVisualStyleBackColor = true;
			this.chkIncludeSchema.CheckedChanged += new System.EventHandler(this.chkIncludeSchema_CheckedChanged);
			// 
			// lblReplace
			// 
			this.lblReplace.AutoSize = true;
			this.lblReplace.Location = new System.Drawing.Point(130, 140);
			this.lblReplace.Name = "lblReplace";
			this.lblReplace.Size = new System.Drawing.Size(47, 13);
			this.lblReplace.TabIndex = 0;
			this.lblReplace.Text = "Replace";
			this.toolTip.SetToolTip(this.lblReplace, "Search and replace on the class name.\r\nSearch is case-sensitive.");
			// 
			// chkIgnoreDboSchema
			// 
			this.chkIgnoreDboSchema.AutoSize = true;
			this.chkIgnoreDboSchema.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkIgnoreDboSchema.Location = new System.Drawing.Point(117, 69);
			this.chkIgnoreDboSchema.Name = "chkIgnoreDboSchema";
			this.chkIgnoreDboSchema.Size = new System.Drawing.Size(119, 17);
			this.chkIgnoreDboSchema.TabIndex = 5;
			this.chkIgnoreDboSchema.Text = "Ignore dbo Schema";
			this.toolTip.SetToolTip(this.chkIgnoreDboSchema, "If the schema name is dbo,\r\ndon\'t add the schema name.");
			this.chkIgnoreDboSchema.UseVisualStyleBackColor = true;
			this.chkIgnoreDboSchema.CheckedChanged += new System.EventHandler(this.chkIgnoreDboSchema_CheckedChanged);
			// 
			// txtSearch
			// 
			this.txtSearch.Location = new System.Drawing.Point(47, 136);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(75, 20);
			this.txtSearch.TabIndex = 11;
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
			// 
			// lblSchemaSeparator
			// 
			this.lblSchemaSeparator.AutoSize = true;
			this.lblSchemaSeparator.Location = new System.Drawing.Point(244, 71);
			this.lblSchemaSeparator.Name = "lblSchemaSeparator";
			this.lblSchemaSeparator.Size = new System.Drawing.Size(95, 13);
			this.lblSchemaSeparator.TabIndex = 0;
			this.lblSchemaSeparator.Text = "Schema Separator";
			this.toolTip.SetToolTip(this.lblSchemaSeparator, "Add a separator after the schema name.");
			// 
			// lblSearch
			// 
			this.lblSearch.AutoSize = true;
			this.lblSearch.Location = new System.Drawing.Point(6, 140);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(41, 13);
			this.lblSearch.TabIndex = 0;
			this.lblSearch.Text = "Search";
			this.toolTip.SetToolTip(this.lblSearch, "Search and replace on the class name.\r\nSearch is case-sensitive.");
			// 
			// txtSchemaSeparator
			// 
			this.txtSchemaSeparator.Location = new System.Drawing.Point(339, 67);
			this.txtSchemaSeparator.Name = "txtSchemaSeparator";
			this.txtSchemaSeparator.Size = new System.Drawing.Size(45, 20);
			this.txtSchemaSeparator.TabIndex = 6;
			this.txtSchemaSeparator.TextChanged += new System.EventHandler(this.txtSchemaSeparator_TextChanged);
			// 
			// lblSingularDesc
			// 
			this.lblSingularDesc.AutoSize = true;
			this.lblSingularDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.lblSingularDesc.Location = new System.Drawing.Point(78, 21);
			this.lblSingularDesc.Name = "lblSingularDesc";
			this.lblSingularDesc.Size = new System.Drawing.Size(107, 13);
			this.lblSingularDesc.TabIndex = 0;
			this.lblSingularDesc.Text = "(Tables, Views, TVPs)";
			// 
			// grbEFAnnotations
			// 
			this.grbEFAnnotations.AutoSize = true;
			this.grbEFAnnotations.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.grbEFAnnotations.Controls.Add(this.btnResetEFAnnotationsSettings);
			this.grbEFAnnotations.Controls.Add(this.chkEF);
			this.grbEFAnnotations.Controls.Add(this.chkEFColumn);
			this.grbEFAnnotations.Controls.Add(this.chkEFConcurrencyCheck);
			this.grbEFAnnotations.Controls.Add(this.chkEFComplexType);
			this.grbEFAnnotations.Controls.Add(this.chkEFDescription);
			this.grbEFAnnotations.Controls.Add(this.chkEFIndex);
			this.grbEFAnnotations.Controls.Add(this.chkEFForeignKeyAndInverseProperty);
			this.grbEFAnnotations.Controls.Add(this.chkEFStringLength);
			this.grbEFAnnotations.Controls.Add(this.chkEFDisplay);
			this.grbEFAnnotations.Controls.Add(this.chkEFRequired);
			this.grbEFAnnotations.Controls.Add(this.chkEFRequiredWithErrorMessage);
			this.grbEFAnnotations.Controls.Add(this.lblEF);
			this.grbEFAnnotations.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grbEFAnnotations.Location = new System.Drawing.Point(305, 207);
			this.grbEFAnnotations.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbEFAnnotations.MinimumSize = new System.Drawing.Size(390, 0);
			this.grbEFAnnotations.Name = "grbEFAnnotations";
			this.grbEFAnnotations.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.grbEFAnnotations.Size = new System.Drawing.Size(390, 124);
			this.grbEFAnnotations.TabIndex = 4;
			this.grbEFAnnotations.TabStop = false;
			this.grbEFAnnotations.Text = "EF Annotations";
			this.grbEFAnnotations.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBox_Paint);
			// 
			// btnResetEFAnnotationsSettings
			// 
			this.btnResetEFAnnotationsSettings.AutoSize = true;
			this.btnResetEFAnnotationsSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnResetEFAnnotationsSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnResetEFAnnotationsSettings.Location = new System.Drawing.Point(366, 9);
			this.btnResetEFAnnotationsSettings.Margin = new System.Windows.Forms.Padding(0);
			this.btnResetEFAnnotationsSettings.Name = "btnResetEFAnnotationsSettings";
			this.btnResetEFAnnotationsSettings.Size = new System.Drawing.Size(21, 19);
			this.btnResetEFAnnotationsSettings.TabIndex = 12;
			this.btnResetEFAnnotationsSettings.Text = "R";
			this.toolTip.SetToolTip(this.btnResetEFAnnotationsSettings, "Reset EF Annotations settings to their default values.");
			this.btnResetEFAnnotationsSettings.UseVisualStyleBackColor = true;
			this.btnResetEFAnnotationsSettings.Click += new System.EventHandler(this.btnResetEFAnnotationsSettings_Click);
			// 
			// chkEF
			// 
			this.chkEF.AutoSize = true;
			this.chkEF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEF.Location = new System.Drawing.Point(6, 19);
			this.chkEF.Name = "chkEF";
			this.chkEF.Size = new System.Drawing.Size(39, 17);
			this.chkEF.TabIndex = 1;
			this.chkEF.Text = "EF";
			this.toolTip.SetToolTip(this.chkEF, "Add Table, Key, MaxLength, Timestamp and\r\nDatabaseGenerated attributes to data me" +
        "mbers.");
			this.chkEF.UseVisualStyleBackColor = true;
			this.chkEF.CheckedChanged += new System.EventHandler(this.chkEF_CheckedChanged);
			// 
			// chkEFColumn
			// 
			this.chkEFColumn.AutoSize = true;
			this.chkEFColumn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFColumn.Location = new System.Drawing.Point(6, 44);
			this.chkEFColumn.Name = "chkEFColumn";
			this.chkEFColumn.Size = new System.Drawing.Size(61, 17);
			this.chkEFColumn.TabIndex = 2;
			this.chkEFColumn.Text = "Column";
			this.toolTip.SetToolTip(this.chkEFColumn, "Add Column attribute with the\r\ndatabase column\'s name and type.");
			this.chkEFColumn.UseVisualStyleBackColor = true;
			this.chkEFColumn.CheckedChanged += new System.EventHandler(this.chkEFColumn_CheckedChanged);
			// 
			// chkEFConcurrencyCheck
			// 
			this.chkEFConcurrencyCheck.AutoSize = true;
			this.chkEFConcurrencyCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFConcurrencyCheck.Location = new System.Drawing.Point(6, 69);
			this.chkEFConcurrencyCheck.Name = "chkEFConcurrencyCheck";
			this.chkEFConcurrencyCheck.Size = new System.Drawing.Size(117, 17);
			this.chkEFConcurrencyCheck.TabIndex = 5;
			this.chkEFConcurrencyCheck.Text = "ConcurrencyCheck";
			this.toolTip.SetToolTip(this.chkEFConcurrencyCheck, "Add ConcurrencyCheck attribute to Timestamp\r\nand RowVersion properties.");
			this.chkEFConcurrencyCheck.UseVisualStyleBackColor = true;
			this.chkEFConcurrencyCheck.CheckedChanged += new System.EventHandler(this.chkEFConcurrencyCheck_CheckedChanged);
			// 
			// chkEFComplexType
			// 
			this.chkEFComplexType.AutoSize = true;
			this.chkEFComplexType.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFComplexType.Location = new System.Drawing.Point(6, 94);
			this.chkEFComplexType.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.chkEFComplexType.Name = "chkEFComplexType";
			this.chkEFComplexType.Size = new System.Drawing.Size(90, 17);
			this.chkEFComplexType.TabIndex = 9;
			this.chkEFComplexType.Text = "ComplexType";
			this.toolTip.SetToolTip(this.chkEFComplexType, "Add ComplexType attribute to complex types.");
			this.chkEFComplexType.UseVisualStyleBackColor = true;
			this.chkEFComplexType.CheckedChanged += new System.EventHandler(this.chkEFComplexType_CheckedChanged);
			// 
			// chkEFDescription
			// 
			this.chkEFDescription.AutoSize = true;
			this.chkEFDescription.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFDescription.Location = new System.Drawing.Point(293, 69);
			this.chkEFDescription.Name = "chkEFDescription";
			this.chkEFDescription.Size = new System.Drawing.Size(79, 17);
			this.chkEFDescription.TabIndex = 8;
			this.chkEFDescription.Text = "Description";
			this.toolTip.SetToolTip(this.chkEFDescription, "Add Description attribute to table and columns.");
			this.chkEFDescription.UseVisualStyleBackColor = true;
			this.chkEFDescription.CheckedChanged += new System.EventHandler(this.chkEFDescription_CheckedChanged);
			// 
			// chkEFIndex
			// 
			this.chkEFIndex.AutoSize = true;
			this.chkEFIndex.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFIndex.Location = new System.Drawing.Point(104, 94);
			this.chkEFIndex.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.chkEFIndex.Name = "chkEFIndex";
			this.chkEFIndex.Size = new System.Drawing.Size(80, 17);
			this.chkEFIndex.TabIndex = 10;
			this.chkEFIndex.Text = "Index (EF6)";
			this.toolTip.SetToolTip(this.chkEFIndex, "Add Index attribute to data members that are part of an index.\r\nIf the index is u" +
        "nique or clustered, the corresponding properties\r\nare set accordingly. Index att" +
        "ribute is applicable starting from EF6.");
			this.chkEFIndex.UseVisualStyleBackColor = true;
			this.chkEFIndex.CheckedChanged += new System.EventHandler(this.chkEFIndex_CheckedChanged);
			// 
			// chkEFForeignKeyAndInverseProperty
			// 
			this.chkEFForeignKeyAndInverseProperty.AutoSize = true;
			this.chkEFForeignKeyAndInverseProperty.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFForeignKeyAndInverseProperty.Location = new System.Drawing.Point(192, 94);
			this.chkEFForeignKeyAndInverseProperty.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.chkEFForeignKeyAndInverseProperty.Name = "chkEFForeignKeyAndInverseProperty";
			this.chkEFForeignKeyAndInverseProperty.Size = new System.Drawing.Size(165, 17);
			this.chkEFForeignKeyAndInverseProperty.TabIndex = 11;
			this.chkEFForeignKeyAndInverseProperty.Text = "ForeignKey && InverseProperty";
			this.toolTip.SetToolTip(this.chkEFForeignKeyAndInverseProperty, "Add ForeignKey and InverseProperty attributes\r\nto navigation properties.");
			this.chkEFForeignKeyAndInverseProperty.UseVisualStyleBackColor = true;
			this.chkEFForeignKeyAndInverseProperty.CheckedChanged += new System.EventHandler(this.chkEFForeignKeyAndInverseProperty_CheckedChanged);
			// 
			// chkEFStringLength
			// 
			this.chkEFStringLength.AutoSize = true;
			this.chkEFStringLength.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFStringLength.Location = new System.Drawing.Point(131, 69);
			this.chkEFStringLength.Name = "chkEFStringLength";
			this.chkEFStringLength.Size = new System.Drawing.Size(86, 17);
			this.chkEFStringLength.TabIndex = 6;
			this.chkEFStringLength.Text = "StringLength";
			this.toolTip.SetToolTip(this.chkEFStringLength, "Add StringLength attribute to string properties.");
			this.chkEFStringLength.UseVisualStyleBackColor = true;
			this.chkEFStringLength.CheckedChanged += new System.EventHandler(this.chkEFStringLength_CheckedChanged);
			// 
			// chkEFDisplay
			// 
			this.chkEFDisplay.AutoSize = true;
			this.chkEFDisplay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFDisplay.Location = new System.Drawing.Point(225, 69);
			this.chkEFDisplay.Name = "chkEFDisplay";
			this.chkEFDisplay.Size = new System.Drawing.Size(60, 17);
			this.chkEFDisplay.TabIndex = 7;
			this.chkEFDisplay.Text = "Display";
			this.toolTip.SetToolTip(this.chkEFDisplay, "Add Display attribute.");
			this.chkEFDisplay.UseVisualStyleBackColor = true;
			this.chkEFDisplay.CheckedChanged += new System.EventHandler(this.chkEFDisplay_CheckedChanged);
			// 
			// chkEFRequired
			// 
			this.chkEFRequired.AutoSize = true;
			this.chkEFRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFRequired.Location = new System.Drawing.Point(75, 44);
			this.chkEFRequired.Name = "chkEFRequired";
			this.chkEFRequired.Size = new System.Drawing.Size(69, 17);
			this.chkEFRequired.TabIndex = 3;
			this.chkEFRequired.Text = "Required";
			this.toolTip.SetToolTip(this.chkEFRequired, "Add Required attribute to properties that are not nullable.");
			this.chkEFRequired.UseVisualStyleBackColor = true;
			this.chkEFRequired.CheckedChanged += new System.EventHandler(this.chkEFRequired_CheckedChanged);
			// 
			// chkEFRequiredWithErrorMessage
			// 
			this.chkEFRequiredWithErrorMessage.AutoSize = true;
			this.chkEFRequiredWithErrorMessage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkEFRequiredWithErrorMessage.Location = new System.Drawing.Point(152, 44);
			this.chkEFRequiredWithErrorMessage.Name = "chkEFRequiredWithErrorMessage";
			this.chkEFRequiredWithErrorMessage.Size = new System.Drawing.Size(159, 17);
			this.chkEFRequiredWithErrorMessage.TabIndex = 4;
			this.chkEFRequiredWithErrorMessage.Text = "Required with ErrorMessage";
			this.toolTip.SetToolTip(this.chkEFRequiredWithErrorMessage, "Add Required attribute to properties that are not nullable\r\nand also add an error" +
        " message.");
			this.chkEFRequiredWithErrorMessage.UseVisualStyleBackColor = true;
			this.chkEFRequiredWithErrorMessage.CheckedChanged += new System.EventHandler(this.chkEFRequiredWithErrorMessage_CheckedChanged);
			// 
			// lblEF
			// 
			this.lblEF.AutoSize = true;
			this.lblEF.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.lblEF.Location = new System.Drawing.Point(53, 21);
			this.lblEF.Name = "lblEF";
			this.lblEF.Size = new System.Drawing.Size(278, 13);
			this.lblEF.TabIndex = 0;
			this.lblEF.Text = "(Table, Key, MaxLength, Timestamp, DatabaseGenerated)";
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 376);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1039, 22);
			this.statusStrip.TabIndex = 9;
			this.statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopy.AutoSize = true;
			this.btnCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnCopy.Location = new System.Drawing.Point(987, 5);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(41, 23);
			this.btnCopy.TabIndex = 6;
			this.btnCopy.Text = "Copy";
			this.toolTip.SetToolTip(this.btnCopy, "Copy generated POCOs.");
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnTypeMapping
			// 
			this.btnTypeMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTypeMapping.AutoSize = true;
			this.btnTypeMapping.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnTypeMapping.Location = new System.Drawing.Point(815, 342);
			this.btnTypeMapping.Name = "btnTypeMapping";
			this.btnTypeMapping.Size = new System.Drawing.Size(85, 23);
			this.btnTypeMapping.TabIndex = 9;
			this.btnTypeMapping.Text = "Type Mapping";
			this.toolTip.SetToolTip(this.btnTypeMapping, "Mapping from RDBMS data types to .NET data types.");
			this.btnTypeMapping.UseVisualStyleBackColor = true;
			this.btnTypeMapping.Click += new System.EventHandler(this.btnTypeMapping_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.AutoSize = true;
			this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(985, 342);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(43, 23);
			this.btnClose.TabIndex = 11;
			this.btnClose.Text = "Close";
			this.toolTip.SetToolTip(this.btnClose, "Close POCO Generator.");
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// contextMenuTable
			// 
			this.contextMenuTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkReferencedTablesToolStripMenuItem,
            this.checkReferencingTablesToolStripMenuItem,
            this.checkAccessibleTablesToolStripMenuItem});
			this.contextMenuTable.Name = "contextMenuTable";
			this.contextMenuTable.Size = new System.Drawing.Size(376, 70);
			// 
			// checkReferencedTablesToolStripMenuItem
			// 
			this.checkReferencedTablesToolStripMenuItem.Name = "checkReferencedTablesToolStripMenuItem";
			this.checkReferencedTablesToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
			this.checkReferencedTablesToolStripMenuItem.Text = "Check Tables Referenced From This Table";
			this.checkReferencedTablesToolStripMenuItem.Click += new System.EventHandler(this.checkReferencedTablesToolStripMenuItem_Click);
			// 
			// checkReferencingTablesToolStripMenuItem
			// 
			this.checkReferencingTablesToolStripMenuItem.Name = "checkReferencingTablesToolStripMenuItem";
			this.checkReferencingTablesToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
			this.checkReferencingTablesToolStripMenuItem.Text = "Check Tables Referencing To This Table";
			this.checkReferencingTablesToolStripMenuItem.Click += new System.EventHandler(this.checkReferencingTablesToolStripMenuItem_Click);
			// 
			// checkAccessibleTablesToolStripMenuItem
			// 
			this.checkAccessibleTablesToolStripMenuItem.Name = "checkAccessibleTablesToolStripMenuItem";
			this.checkAccessibleTablesToolStripMenuItem.Size = new System.Drawing.Size(375, 22);
			this.checkAccessibleTablesToolStripMenuItem.Text = "Check Recursively Tables Accessible From && To This Table";
			this.checkAccessibleTablesToolStripMenuItem.Click += new System.EventHandler(this.checkAccessibleTablesToolStripMenuItem_Click);
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 1500;
			// 
			// POCOGeneratorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(1039, 712);
			this.Controls.Add(this.splitContainer1);
			this.MinimumSize = new System.Drawing.Size(1055, 751);
			this.Name = "POCOGeneratorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "POCO Generator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.POCOGeneratorForm_FormClosing);
			this.Load += new System.EventHandler(this.POCOGeneratorForm_Load);
			this.Shown += new System.EventHandler(this.POCOGeneratorForm_Shown);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.contextMenu.ResumeLayout(false);
			this.contextMenuPocoEditor.ResumeLayout(false);
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.grbPOCO.ResumeLayout(false);
			this.grbPOCO.PerformLayout();
			this.panelProperties.ResumeLayout(false);
			this.panelProperties.PerformLayout();
			this.grbNavigationProperties.ResumeLayout(false);
			this.grbNavigationProperties.PerformLayout();
			this.panelNavigationProperties.ResumeLayout(false);
			this.panelNavigationProperties.PerformLayout();
			this.grbExportToFiles.ResumeLayout(false);
			this.grbExportToFiles.PerformLayout();
			this.grbFileName.ResumeLayout(false);
			this.grbFileName.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.grbClassName.ResumeLayout(false);
			this.grbClassName.PerformLayout();
			this.grbEFAnnotations.ResumeLayout(false);
			this.grbEFAnnotations.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.contextMenuTable.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView trvServer;
        private System.Windows.Forms.ImageList imageListDbObjects;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox txtPocoEditor;
        private System.Windows.Forms.ContextMenuStrip contextMenuPocoEditor;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdbProperties;
        private System.Windows.Forms.RadioButton rdbFields;
        private System.Windows.Forms.CheckBox chkVirtualProperties;
        private System.Windows.Forms.CheckBox chkStructTypesNullable;
        private System.Windows.Forms.CheckBox chkComments;
        private System.Windows.Forms.CheckBox chkCommentsWithoutNull;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TextBox txtFixedClassName;
        private System.Windows.Forms.CheckBox chkLowerCase;
        private System.Windows.Forms.CheckBox chkUpperCase;
        private System.Windows.Forms.CheckBox chkCamelCase;
        private System.Windows.Forms.TextBox txtWordsSeparator;
        private System.Windows.Forms.Label lblWordsSeparator;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogExport;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnTypeMapping;
        private System.Windows.Forms.Label lblWordsSeparatorDesc;
        private System.Windows.Forms.Label lblFixedName;
        private System.Windows.Forms.TextBox txtDBSeparator;
        private System.Windows.Forms.Label lblDBSeparator;
        private System.Windows.Forms.CheckBox chkIncludeDB;
        private System.Windows.Forms.TextBox txtSchemaSeparator;
        private System.Windows.Forms.Label lblSchemaSeparator;
        private System.Windows.Forms.CheckBox chkIgnoreDboSchema;
        private System.Windows.Forms.CheckBox chkIncludeSchema;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label lblNamespace;
        private System.Windows.Forms.CheckBox chkUsing;
        private System.Windows.Forms.CheckBox chkSingular;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.CheckBox chkPartialClass;
        private System.Windows.Forms.CheckBox chkEF;
        private System.Windows.Forms.CheckBox chkEFRequired;
        private System.Windows.Forms.CheckBox chkEFColumn;
        private System.Windows.Forms.Label lblSingularDesc;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterSettingsToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkSearchIgnoreCase;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox chkEFDisplay;
        private System.Windows.Forms.CheckBox chkEFStringLength;
        private System.Windows.Forms.CheckBox chkEFConcurrencyCheck;
        private System.Windows.Forms.CheckBox chkNewLineBetweenMembers;
        private System.Windows.Forms.CheckBox chkEFRequiredWithErrorMessage;
        private System.Windows.Forms.Label lblEF;
        private System.Windows.Forms.CheckBox chkEFComplexType;
        private System.Windows.Forms.CheckBox chkEFIndex;
        private System.Windows.Forms.ToolStripMenuItem clearCheckBoxesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuTable;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.CheckBox chkNavigationProperties;
        private System.Windows.Forms.CheckBox chkVirtualNavigationProperties;
        private System.Windows.Forms.Panel panelNavigationProperties;
        private System.Windows.Forms.RadioButton rdbIEnumerableNavigationProperties;
        private System.Windows.Forms.RadioButton rdbICollectionNavigationProperties;
        private System.Windows.Forms.RadioButton rdbListNavigationProperties;
        private System.Windows.Forms.CheckBox chkEFForeignKeyAndInverseProperty;
        private System.Windows.Forms.CheckBox chkNavigationPropertiesComments;
        private System.Windows.Forms.CheckBox chkManyToManyJoinTable;
        private System.Windows.Forms.TextBox txtInherit;
        private System.Windows.Forms.Label lblInherit;
        private System.Windows.Forms.CheckBox chkEFDescription;
        private System.Windows.Forms.CheckBox chkOverrideProperties;
        private System.Windows.Forms.CheckBox chkOverrideNavigationProperties;
        private System.Windows.Forms.CheckBox chkColumnDefaults;
        private System.Windows.Forms.GroupBox grbEFAnnotations;
        private System.Windows.Forms.GroupBox grbClassName;
        private System.Windows.Forms.GroupBox grbNavigationProperties;
        private System.Windows.Forms.GroupBox grbExportToFiles;
        private System.Windows.Forms.GroupBox grbPOCO;
        private System.Windows.Forms.Panel panelEnum;
        private System.Windows.Forms.RadioButton rdbEnumSQLTypeToString;
        private System.Windows.Forms.RadioButton rdbEnumSQLTypeToEnumUShort;
        private System.Windows.Forms.Label lblSQLEnum;
        private System.Windows.Forms.RadioButton rdbEnumSQLTypeToEnumInt;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ToolStripMenuItem checkReferencedTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkReferencingTablesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAccessibleTablesToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdbSingleFile;
        private System.Windows.Forms.RadioButton rdbMultipleFilesRelativeFolders;
        private System.Windows.Forms.RadioButton rdbMultipleFilesSingleFolder;
        private System.Windows.Forms.CheckBox chkUsingInsideNamespace;
        private System.Windows.Forms.Button btnDisclaimer;
        private System.Windows.Forms.CheckBox chkComplexTypes;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdbFileNameName;
        private System.Windows.Forms.RadioButton rdbFileNameSchemaName;
        private System.Windows.Forms.RadioButton rdbFileNameDatabaseName;
        private System.Windows.Forms.RadioButton rdbFileNameDatabaseSchemaName;
        private System.Windows.Forms.GroupBox grbFileName;
        private System.Windows.Forms.Button btnResetPOCOSettings;
        private System.Windows.Forms.Button btnResetNavigationPropertiesSettings;
        private System.Windows.Forms.Button btnResetClassNameSettings;
        private System.Windows.Forms.Button btnResetEFAnnotationsSettings;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.RadioButton rdbIListNavigationProperties;
        private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.CheckBox chkImmutableClass;
		private System.Windows.Forms.CheckBox chkFactoryPattern;
	}
}