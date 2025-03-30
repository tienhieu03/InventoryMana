namespace UserManagement
{
    partial class formMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.btnGroup = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple1 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroupSeparator1 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.btnUser = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupTriple2 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.kryptonRibbonGroup1 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonTab1 = new Krypton.Ribbon.KryptonRibbonTab();
            this.kryptonRibbon1 = new Krypton.Ribbon.KryptonRibbon();
            this.kryptonRibbonGroupSeparator2 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroupTriple3 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.btnUpdateInfor = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup2 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple4 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.btnFuntion = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroupSeparator3 = new Krypton.Ribbon.KryptonRibbonGroupSeparator();
            this.kryptonRibbonGroupTriple5 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.btnReport = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.kryptonRibbonGroup3 = new Krypton.Ribbon.KryptonRibbonGroup();
            this.kryptonRibbonGroupTriple6 = new Krypton.Ribbon.KryptonRibbonGroupTriple();
            this.btnExit = new Krypton.Ribbon.KryptonRibbonGroupButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gvUser = new System.Windows.Forms.DataGridView();
            this.pnlDepartment = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.IsDisable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsGroup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DepartmentID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUser)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGroup
            // 
            this.btnGroup.ImageLarge = ((System.Drawing.Image)(resources.GetObject("btnGroup.ImageLarge")));
            this.btnGroup.ImageSmall = ((System.Drawing.Image)(resources.GetObject("btnGroup.ImageSmall")));
            this.btnGroup.TextLine1 = "User Group";
            this.btnGroup.ToolTipValues.Image = ((System.Drawing.Image)(resources.GetObject("btnGroup.ToolTipValues.Image")));
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // kryptonRibbonGroupTriple1
            // 
            this.kryptonRibbonGroupTriple1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnGroup});
            // 
            // btnUser
            // 
            this.btnUser.ImageLarge = ((System.Drawing.Image)(resources.GetObject("btnUser.ImageLarge")));
            this.btnUser.TextLine1 = "User";
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // kryptonRibbonGroupTriple2
            // 
            this.kryptonRibbonGroupTriple2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnUser});
            // 
            // kryptonRibbonGroup1
            // 
            this.kryptonRibbonGroup1.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple1,
            this.kryptonRibbonGroupSeparator1,
            this.kryptonRibbonGroupTriple2,
            this.kryptonRibbonGroupSeparator2,
            this.kryptonRibbonGroupTriple3});
            this.kryptonRibbonGroup1.TextLine1 = "Account";
            // 
            // kryptonRibbonTab1
            // 
            this.kryptonRibbonTab1.Groups.AddRange(new Krypton.Ribbon.KryptonRibbonGroup[] {
            this.kryptonRibbonGroup1,
            this.kryptonRibbonGroup2,
            this.kryptonRibbonGroup3});
            this.kryptonRibbonTab1.Text = "Funtion";
            // 
            // kryptonRibbon1
            // 
            this.kryptonRibbon1.Name = "kryptonRibbon1";
            this.kryptonRibbon1.RibbonTabs.AddRange(new Krypton.Ribbon.KryptonRibbonTab[] {
            this.kryptonRibbonTab1});
            this.kryptonRibbon1.SelectedTab = this.kryptonRibbonTab1;
            this.kryptonRibbon1.Size = new System.Drawing.Size(1360, 143);
            this.kryptonRibbon1.TabIndex = 0;
            // 
            // kryptonRibbonGroupTriple3
            // 
            this.kryptonRibbonGroupTriple3.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnUpdateInfor});
            // 
            // btnUpdateInfor
            // 
            this.btnUpdateInfor.TextLine1 = "Update Infor";
            this.btnUpdateInfor.Click += new System.EventHandler(this.btnUpdateInfor_Click);
            // 
            // kryptonRibbonGroup2
            // 
            this.kryptonRibbonGroup2.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple4,
            this.kryptonRibbonGroupSeparator3,
            this.kryptonRibbonGroupTriple5});
            this.kryptonRibbonGroup2.TextLine1 = "Permissions";
            // 
            // kryptonRibbonGroupTriple4
            // 
            this.kryptonRibbonGroupTriple4.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnFuntion});
            // 
            // btnFuntion
            // 
            this.btnFuntion.TextLine1 = "Function";
            this.btnFuntion.Click += new System.EventHandler(this.btnFuntion_Click);
            // 
            // kryptonRibbonGroupTriple5
            // 
            this.kryptonRibbonGroupTriple5.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnReport});
            // 
            // btnReport
            // 
            this.btnReport.TextLine1 = "Report";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // kryptonRibbonGroup3
            // 
            this.kryptonRibbonGroup3.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupContainer[] {
            this.kryptonRibbonGroupTriple6});
            this.kryptonRibbonGroup3.TextLine1 = "System";
            // 
            // kryptonRibbonGroupTriple6
            // 
            this.kryptonRibbonGroupTriple6.Items.AddRange(new Krypton.Ribbon.KryptonRibbonGroupItem[] {
            this.btnExit});
            // 
            // btnExit
            // 
            this.btnExit.TextLine1 = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pnlDepartment);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 207);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1360, 28);
            this.panel1.TabIndex = 1;
            // 
            // gvUser
            // 
            this.gvUser.AllowUserToAddRows = false;
            this.gvUser.AllowUserToDeleteRows = false;
            this.gvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsDisable,
            this.IsGroup,
            this.UserID,
            this.UserName,
            this.FullName,
            this.CompanyID,
            this.DepartmentID});
            this.gvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvUser.Location = new System.Drawing.Point(3, 235);
            this.gvUser.Name = "gvUser";
            this.gvUser.ReadOnly = true;
            this.gvUser.RowHeadersVisible = false;
            this.gvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvUser.Size = new System.Drawing.Size(1360, 477);
            this.gvUser.TabIndex = 2;
            this.gvUser.DoubleClick += new System.EventHandler(this.gvUser_DoubleClick);
            // 
            // pnlDepartment
            // 
            this.pnlDepartment.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDepartment.Location = new System.Drawing.Point(1002, 0);
            this.pnlDepartment.Name = "pnlDepartment";
            this.pnlDepartment.Size = new System.Drawing.Size(358, 28);
            this.pnlDepartment.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(889, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 22);
            this.label1.TabIndex = 1;
            this.label1.Text = "Department";
            // 
            // IsDisable
            // 
            this.IsDisable.DataPropertyName = "IsDisable";
            this.IsDisable.HeaderText = "Disabled";
            this.IsDisable.Name = "IsDisable";
            this.IsDisable.ReadOnly = true;
            this.IsDisable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsDisable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // IsGroup
            // 
            this.IsGroup.DataPropertyName = "IsGroup";
            this.IsGroup.HeaderText = "Group";
            this.IsGroup.Name = "IsGroup";
            this.IsGroup.ReadOnly = true;
            this.IsGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "ID";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            this.UserID.Visible = false;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "User Name";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            // 
            // FullName
            // 
            this.FullName.DataPropertyName = "FullName";
            this.FullName.HeaderText = "Full Name";
            this.FullName.Name = "FullName";
            this.FullName.ReadOnly = true;
            // 
            // CompanyID
            // 
            this.CompanyID.DataPropertyName = "CompanyID";
            this.CompanyID.HeaderText = "Company ID";
            this.CompanyID.Name = "CompanyID";
            this.CompanyID.ReadOnly = true;
            this.CompanyID.Visible = false;
            // 
            // DepartmentID
            // 
            this.DepartmentID.DataPropertyName = "DepartmentID";
            this.DepartmentID.HeaderText = "Department ID";
            this.DepartmentID.Name = "DepartmentID";
            this.DepartmentID.ReadOnly = true;
            this.DepartmentID.Visible = false;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1366, 715);
            this.Controls.Add(this.gvUser);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.kryptonRibbon1);
            this.Name = "formMain";
            this.Text = "User Management";
            this.Load += new System.EventHandler(this.formMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonRibbon1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Krypton.Ribbon.KryptonRibbonGroupButton btnGroup;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple1;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator1;
        private Krypton.Ribbon.KryptonRibbonGroupButton btnUser;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple2;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup1;
        private Krypton.Ribbon.KryptonRibbonTab kryptonRibbonTab1;
        private Krypton.Ribbon.KryptonRibbon kryptonRibbon1;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator2;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple3;
        private Krypton.Ribbon.KryptonRibbonGroupButton btnUpdateInfor;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup2;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple4;
        private Krypton.Ribbon.KryptonRibbonGroupButton btnFuntion;
        private Krypton.Ribbon.KryptonRibbonGroupSeparator kryptonRibbonGroupSeparator3;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple5;
        private Krypton.Ribbon.KryptonRibbonGroupButton btnReport;
        private Krypton.Ribbon.KryptonRibbonGroup kryptonRibbonGroup3;
        private Krypton.Ribbon.KryptonRibbonGroupTriple kryptonRibbonGroupTriple6;
        private Krypton.Ribbon.KryptonRibbonGroupButton btnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlDepartment;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsDisable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DepartmentID;
    }
}

