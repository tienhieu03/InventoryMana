namespace UserManagement.FuncForm
{
    partial class formSysFuncRight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formSysFuncRight));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gvUsers = new System.Windows.Forms.DataGridView();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsGroup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gvFunction = new System.Windows.Forms.DataGridView();
            this.FUNC_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnLockFunc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnViewOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnFullFunc = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFunction)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.gvUsers);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gvFunction);
            this.splitContainer1.Size = new System.Drawing.Size(794, 423);
            this.splitContainer1.SplitterDistance = 338;
            this.splitContainer1.TabIndex = 0;
            // 
            // gvUsers
            // 
            this.gvUsers.AllowUserToAddRows = false;
            this.gvUsers.AllowUserToDeleteRows = false;
            this.gvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserID,
            this.IsGroup,
            this.UserName,
            this.FullName});
            this.gvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvUsers.Location = new System.Drawing.Point(0, 0);
            this.gvUsers.Name = "gvUsers";
            this.gvUsers.ReadOnly = true;
            this.gvUsers.RowHeadersVisible = false;
            this.gvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvUsers.Size = new System.Drawing.Size(338, 423);
            this.gvUsers.TabIndex = 0;
            this.gvUsers.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gvUsers_CellPainting);
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "UserID";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
            this.UserID.Visible = false;
            // 
            // IsGroup
            // 
            this.IsGroup.DataPropertyName = "IsGroup";
            this.IsGroup.HeaderText = "IsGroup";
            this.IsGroup.Name = "IsGroup";
            this.IsGroup.ReadOnly = true;
            this.IsGroup.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsGroup.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
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
            this.FullName.HeaderText = "FullName";
            this.FullName.Name = "FullName";
            this.FullName.ReadOnly = true;
            // 
            // gvFunction
            // 
            this.gvFunction.AllowUserToAddRows = false;
            this.gvFunction.AllowUserToDeleteRows = false;
            this.gvFunction.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvFunction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FUNC_CODE,
            this.Description,
            this.Permissions});
            this.gvFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvFunction.Location = new System.Drawing.Point(0, 0);
            this.gvFunction.Name = "gvFunction";
            this.gvFunction.ReadOnly = true;
            this.gvFunction.RowHeadersVisible = false;
            this.gvFunction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvFunction.Size = new System.Drawing.Size(452, 423);
            this.gvFunction.TabIndex = 1;
            // 
            // FUNC_CODE
            // 
            this.FUNC_CODE.DataPropertyName = "Permissions";
            this.FUNC_CODE.HeaderText = "FUNC_CODE";
            this.FUNC_CODE.Name = "FUNC_CODE";
            this.FUNC_CODE.ReadOnly = true;
            this.FUNC_CODE.Visible = false;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Permissions
            // 
            this.Permissions.HeaderText = "Permissions";
            this.Permissions.Name = "Permissions";
            this.Permissions.ReadOnly = true;
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
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnLockFunc,
            this.toolStripSeparator1,
            this.mnViewOnly,
            this.toolStripSeparator2,
            this.mnFullFunc});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 104);
            // 
            // mnLockFunc
            // 
            this.mnLockFunc.Name = "mnLockFunc";
            this.mnLockFunc.Size = new System.Drawing.Size(180, 22);
            this.mnLockFunc.Text = "Lock Function";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnViewOnly
            // 
            this.mnViewOnly.Name = "mnViewOnly";
            this.mnViewOnly.Size = new System.Drawing.Size(180, 22);
            this.mnViewOnly.Text = "View Only";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // mnFullFunc
            // 
            this.mnFullFunc.Name = "mnFullFunc";
            this.mnFullFunc.Size = new System.Drawing.Size(180, 22);
            this.mnFullFunc.Text = "Full Function";
            // 
            // formSysFuncRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Name = "formSysFuncRight";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.Text = "formSysFuncRight";
            this.Load += new System.EventHandler(this.formSysFuncRight_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFunction)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gvUsers;
        private System.Windows.Forms.DataGridView gvFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNC_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnLockFunc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnViewOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnFullFunc;
    }
}