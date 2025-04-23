namespace UserManagement.UserControls
{
    partial class UserPermissionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserPermissionControl));
            this.customPanel2 = new SharedControls.CustomPanel();
            this.dgvFunction = new System.Windows.Forms.DataGridView();
            this.FUNC_CODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fIsGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Permission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Parent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnLockFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnViewOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnFullFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnSelectAllChildren = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.customPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunction)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel2
            // 
            this.customPanel2.BackColor = System.Drawing.Color.White;
            this.customPanel2.BorderRadius = 30;
            this.customPanel2.Controls.Add(this.dgvFunction);
            this.customPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel2.ForeColor = System.Drawing.Color.Black;
            this.customPanel2.GradientAngle = 90F;
            this.customPanel2.GradientBottomColor = System.Drawing.Color.CadetBlue;
            this.customPanel2.GradientTopColor = System.Drawing.Color.DodgerBlue;
            this.customPanel2.Location = new System.Drawing.Point(0, 0);
            this.customPanel2.Name = "customPanel2";
            this.customPanel2.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.customPanel2.Size = new System.Drawing.Size(525, 437);
            this.customPanel2.TabIndex = 8;
            // 
            // dgvFunction
            // 
            this.dgvFunction.AllowUserToAddRows = false;
            this.dgvFunction.AllowUserToDeleteRows = false;
            this.dgvFunction.AllowUserToResizeColumns = false;
            this.dgvFunction.AllowUserToResizeRows = false;
            this.dgvFunction.BackgroundColor = System.Drawing.Color.White;
            this.dgvFunction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFunction.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvFunction.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(145)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFunction.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFunction.ColumnHeadersHeight = 35;
            this.dgvFunction.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FUNC_CODE,
            this.fIsGroup,
            this.Description,
            this.Permission,
            this.Parent});
            this.dgvFunction.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFunction.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFunction.EnableHeadersVisualStyles = false;
            this.dgvFunction.Location = new System.Drawing.Point(4, 0);
            this.dgvFunction.Name = "dgvFunction";
            this.dgvFunction.ReadOnly = true;
            this.dgvFunction.RowHeadersVisible = false;
            this.dgvFunction.RowHeadersWidth = 51;
            this.dgvFunction.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFunction.Size = new System.Drawing.Size(517, 422);
            this.dgvFunction.TabIndex = 1;
            this.dgvFunction.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvFunction_CellFormatting);
            this.dgvFunction.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFunction_CellMouseDown);
            // 
            // FUNC_CODE
            // 
            this.FUNC_CODE.DataPropertyName = "FUNC_CODE";
            this.FUNC_CODE.HeaderText = "FUNC_CODE";
            this.FUNC_CODE.MinimumWidth = 6;
            this.FUNC_CODE.Name = "FUNC_CODE";
            this.FUNC_CODE.ReadOnly = true;
            this.FUNC_CODE.Visible = false;
            this.FUNC_CODE.Width = 125;
            // 
            // fIsGroup
            // 
            this.fIsGroup.DataPropertyName = "IsGroup";
            this.fIsGroup.HeaderText = "IsGroup";
            this.fIsGroup.MinimumWidth = 6;
            this.fIsGroup.Name = "fIsGroup";
            this.fIsGroup.ReadOnly = true;
            this.fIsGroup.Visible = false;
            this.fIsGroup.Width = 125;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Function";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 170;
            // 
            // Permission
            // 
            this.Permission.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Permission.DataPropertyName = "Access";
            this.Permission.HeaderText = "Permission";
            this.Permission.MinimumWidth = 6;
            this.Permission.Name = "Permission";
            this.Permission.ReadOnly = true;
            // 
            // Parent
            // 
            this.Parent.DataPropertyName = "Parent";
            this.Parent.HeaderText = "Parent";
            this.Parent.MinimumWidth = 6;
            this.Parent.Name = "Parent";
            this.Parent.ReadOnly = true;
            this.Parent.Visible = false;
            this.Parent.Width = 125;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnLockFunction,
            this.toolStripSeparator1,
            this.mnViewOnly,
            this.toolStripSeparator2,
            this.mnFullFunction,
            this.toolStripSeparator3,
            this.mnSelectAllChildren});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(213, 110);
            // 
            // mnLockFunction
            // 
            this.mnLockFunction.Name = "mnLockFunction";
            this.mnLockFunction.Size = new System.Drawing.Size(212, 22);
            this.mnLockFunction.Text = "Lock Function";
            this.mnLockFunction.Click += new System.EventHandler(this.mnLockFunction_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
            // 
            // mnViewOnly
            // 
            this.mnViewOnly.Name = "mnViewOnly";
            this.mnViewOnly.Size = new System.Drawing.Size(212, 22);
            this.mnViewOnly.Text = "View Only";
            this.mnViewOnly.Click += new System.EventHandler(this.mnViewOnly_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
            // 
            // mnFullFunction
            // 
            this.mnFullFunction.Name = "mnFullFunction";
            this.mnFullFunction.Size = new System.Drawing.Size(212, 22);
            this.mnFullFunction.Text = "Full Access";
            this.mnFullFunction.Click += new System.EventHandler(this.mnFullFunction_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(209, 6);
            // 
            // mnSelectAllChildren
            // 
            this.mnSelectAllChildren.Name = "mnSelectAllChildren";
            this.mnSelectAllChildren.Size = new System.Drawing.Size(212, 22);
            this.mnSelectAllChildren.Text = "Choose All Child Function";
            this.mnSelectAllChildren.Click += new System.EventHandler(this.mnSelectAllChildren_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "groups");
            this.imageList1.Images.SetKeyName(1, "user");
            // 
            // UserPermissionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.customPanel2);
            this.Name = "UserPermissionControl";
            this.Size = new System.Drawing.Size(525, 437);
            this.Load += new System.EventHandler(this.UserPermissionControl_Load);
            this.customPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFunction)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SharedControls.CustomPanel customPanel2;
        private System.Windows.Forms.DataGridView dgvFunction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNC_CODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn fIsGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Permission;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnLockFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnViewOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnFullFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnSelectAllChildren;
        private System.Windows.Forms.ImageList imageList1;
    }
}
