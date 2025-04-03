namespace UserManagement.FuncForm
{
    partial class formFuncPermission
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formFuncPermission));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.IsGroup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvFunction = new System.Windows.Forms.DataGridView();
            this.FUNC_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fIsGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnLockFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnViewOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnFullFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnSelectAllChildren = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunction)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvUser);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvFunction);
            this.splitContainer1.Size = new System.Drawing.Size(794, 423);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsGroup,
            this.UserName,
            this.FullName,
            this.UserID});
            this.dgvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUser.Location = new System.Drawing.Point(0, 0);
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            this.dgvUser.RowHeadersVisible = false;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(314, 423);
            this.dgvUser.TabIndex = 0;
            this.dgvUser.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvUser_CellPainting);
            this.dgvUser.Click += new System.EventHandler(this.dgvUser_Click);
            // 
            // IsGroup
            // 
            this.IsGroup.DataPropertyName = "IsGroup";
            this.IsGroup.HeaderText = "IsGroup";
            this.IsGroup.Name = "IsGroup";
            this.IsGroup.ReadOnly = true;
            // 
            // UserName
            // 
            this.UserName.DataPropertyName = "UserName";
            this.UserName.HeaderText = "User Name";
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FullName
            // 
            this.FullName.DataPropertyName = "FullName";
            this.FullName.HeaderText = "Full Name";
            this.FullName.Name = "FullName";
            this.FullName.ReadOnly = true;
            this.FullName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FullName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "UserID";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            this.UserID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UserID.Visible = false;
            // 
            // dgvFunction
            // 
            this.dgvFunction.AllowUserToAddRows = false;
            this.dgvFunction.AllowUserToDeleteRows = false;
            this.dgvFunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFunction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FUNC_CODE,
            this.fIsGroup,
            this.Description,
            this.Permission,
            this.Parent});
            this.dgvFunction.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFunction.Location = new System.Drawing.Point(0, 0);
            this.dgvFunction.Name = "dgvFunction";
            this.dgvFunction.ReadOnly = true;
            this.dgvFunction.RowHeadersVisible = false;
            this.dgvFunction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFunction.Size = new System.Drawing.Size(476, 423);
            this.dgvFunction.TabIndex = 1;
            this.dgvFunction.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFunction_CellFormatting);
            // 
            // FUNC_CODE
            // 
            this.FUNC_CODE.DataPropertyName = "FUNC_CODE";
            this.FUNC_CODE.HeaderText = "FUNC_CODE";
            this.FUNC_CODE.Name = "FUNC_CODE";
            this.FUNC_CODE.ReadOnly = true;
            this.FUNC_CODE.Visible = false;
            // 
            // fIsGroup
            // 
            this.fIsGroup.DataPropertyName = "IsGroup";
            this.fIsGroup.HeaderText = "IsGroup";
            this.fIsGroup.Name = "fIsGroup";
            this.fIsGroup.ReadOnly = true;
            this.fIsGroup.Visible = false;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Function";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Permission
            // 
            this.Permission.DataPropertyName = "Access";
            this.Permission.HeaderText = "Permission";
            this.Permission.Name = "Permission";
            this.Permission.ReadOnly = true;
            // 
            // Parent
            // 
            this.Parent.DataPropertyName = "Parent";
            this.Parent.HeaderText = "Parent";
            this.Parent.Name = "Parent";
            this.Parent.ReadOnly = true;
            this.Parent.Visible = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "groups");
            this.imageList1.Images.SetKeyName(1, "user");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnLockFunction,
            this.toolStripSeparator1,
            this.mnViewOnly,
            this.toolStripSeparator2,
            this.mnFullFunction,
            this.toolStripSeparator3,
            this.mnSelectAllChildren});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(218, 110);
            // 
            // mnLockFunction
            // 
            this.mnLockFunction.Name = "mnLockFunction";
            this.mnLockFunction.Size = new System.Drawing.Size(217, 22);
            this.mnLockFunction.Text = "Lock Function";
            this.mnLockFunction.Click += new System.EventHandler(this.mnLockFunction_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(214, 6);
            // 
            // mnViewOnly
            // 
            this.mnViewOnly.Name = "mnViewOnly";
            this.mnViewOnly.Size = new System.Drawing.Size(217, 22);
            this.mnViewOnly.Text = "View Only";
            this.mnViewOnly.Click += new System.EventHandler(this.mnViewOnly_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(214, 6);
            // 
            // mnFullFunction
            // 
            this.mnFullFunction.Name = "mnFullFunction";
            this.mnFullFunction.Size = new System.Drawing.Size(217, 22);
            this.mnFullFunction.Text = "Full Function";
            this.mnFullFunction.Click += new System.EventHandler(this.mnFullFunction_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // mnSelectAllChildren
            // 
            this.mnSelectAllChildren.Name = "mnSelectAllChildren";
            this.mnSelectAllChildren.Size = new System.Drawing.Size(217, 22);
            this.mnSelectAllChildren.Text = "Chọn tất cả chức năng con";
            this.mnSelectAllChildren.Click += new System.EventHandler(this.mnSelectAllChildren_Click);
            // 
            // formFuncPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Name = "formFuncPermission";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.Text = "formFuncPermission";
            this.Load += new System.EventHandler(this.formFuncPermission_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunction)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridView dgvFunction;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnLockFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnViewOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnFullFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNC_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn fIsGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permission;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnSelectAllChildren;
    }
}