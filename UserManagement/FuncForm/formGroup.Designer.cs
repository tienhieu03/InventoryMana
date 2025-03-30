namespace UserManagement.FuncForm
{
    partial class formGroup
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
            this.tabGroup = new System.Windows.Forms.TabControl();
            this.pageGroup = new System.Windows.Forms.TabPage();
            this.txtDescrip = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGroup = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pageMember = new System.Windows.Forms.TabPage();
            this.btnDel = new MaterialSkin.Controls.MaterialButton();
            this.btnAdd = new MaterialSkin.Controls.MaterialButton();
            this.gvMember = new System.Windows.Forms.DataGridView();
            this.UserID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.btnExit = new MaterialSkin.Controls.MaterialButton();
            this.tabGroup.SuspendLayout();
            this.pageGroup.SuspendLayout();
            this.pageMember.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).BeginInit();
            this.SuspendLayout();
            // 
            // tabGroup
            // 
            this.tabGroup.Controls.Add(this.pageGroup);
            this.tabGroup.Controls.Add(this.pageMember);
            this.tabGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabGroup.Location = new System.Drawing.Point(3, 64);
            this.tabGroup.Name = "tabGroup";
            this.tabGroup.SelectedIndex = 0;
            this.tabGroup.Size = new System.Drawing.Size(564, 225);
            this.tabGroup.TabIndex = 0;
            // 
            // pageGroup
            // 
            this.pageGroup.Controls.Add(this.txtDescrip);
            this.pageGroup.Controls.Add(this.label2);
            this.pageGroup.Controls.Add(this.txtGroup);
            this.pageGroup.Controls.Add(this.label1);
            this.pageGroup.Location = new System.Drawing.Point(4, 22);
            this.pageGroup.Name = "pageGroup";
            this.pageGroup.Padding = new System.Windows.Forms.Padding(3);
            this.pageGroup.Size = new System.Drawing.Size(556, 199);
            this.pageGroup.TabIndex = 0;
            this.pageGroup.Text = "Group Information";
            this.pageGroup.UseVisualStyleBackColor = true;
            // 
            // txtDescrip
            // 
            this.txtDescrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrip.Location = new System.Drawing.Point(216, 84);
            this.txtDescrip.Name = "txtDescrip";
            this.txtDescrip.Size = new System.Drawing.Size(187, 22);
            this.txtDescrip.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(126, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description";
            // 
            // txtGroup
            // 
            this.txtGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroup.Location = new System.Drawing.Point(216, 46);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(187, 22);
            this.txtGroup.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(126, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Group Name";
            // 
            // pageMember
            // 
            this.pageMember.Controls.Add(this.btnDel);
            this.pageMember.Controls.Add(this.btnAdd);
            this.pageMember.Controls.Add(this.gvMember);
            this.pageMember.Location = new System.Drawing.Point(4, 22);
            this.pageMember.Name = "pageMember";
            this.pageMember.Padding = new System.Windows.Forms.Padding(3);
            this.pageMember.Size = new System.Drawing.Size(556, 199);
            this.pageMember.TabIndex = 1;
            this.pageMember.Text = "Member";
            this.pageMember.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDel.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnDel.Depth = 0;
            this.btnDel.HighEmphasis = true;
            this.btnDel.Icon = null;
            this.btnDel.Location = new System.Drawing.Point(479, 157);
            this.btnDel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnDel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnDel.Name = "btnDel";
            this.btnDel.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnDel.Size = new System.Drawing.Size(64, 36);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "Del";
            this.btnDel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnDel.UseAccentColor = false;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnAdd.Depth = 0;
            this.btnAdd.HighEmphasis = true;
            this.btnAdd.Icon = null;
            this.btnAdd.Location = new System.Drawing.Point(397, 157);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnAdd.Size = new System.Drawing.Size(64, 36);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add";
            this.btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAdd.UseAccentColor = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gvMember
            // 
            this.gvMember.AllowUserToAddRows = false;
            this.gvMember.AllowUserToDeleteRows = false;
            this.gvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UserID,
            this.UserName,
            this.FullName});
            this.gvMember.Dock = System.Windows.Forms.DockStyle.Top;
            this.gvMember.Location = new System.Drawing.Point(3, 3);
            this.gvMember.Name = "gvMember";
            this.gvMember.ReadOnly = true;
            this.gvMember.RowHeadersVisible = false;
            this.gvMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMember.Size = new System.Drawing.Size(550, 150);
            this.gvMember.TabIndex = 0;
            // 
            // UserID
            // 
            this.UserID.DataPropertyName = "UserID";
            this.UserID.HeaderText = "User ID";
            this.UserID.Name = "UserID";
            this.UserID.ReadOnly = true;
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
            // btnSave
            // 
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnSave.Depth = 0;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(404, 308);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnSave.Size = new System.Drawing.Size(64, 36);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExit.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btnExit.Depth = 0;
            this.btnExit.HighEmphasis = true;
            this.btnExit.Icon = null;
            this.btnExit.Location = new System.Drawing.Point(486, 308);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnExit.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnExit.Name = "btnExit";
            this.btnExit.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btnExit.Size = new System.Drawing.Size(64, 36);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnExit.UseAccentColor = false;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // formGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 353);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabGroup);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formGroup";
            this.Text = "User Group";
            this.Load += new System.EventHandler(this.formGroup_Load);
            this.tabGroup.ResumeLayout(false);
            this.pageGroup.ResumeLayout(false);
            this.pageGroup.PerformLayout();
            this.pageMember.ResumeLayout(false);
            this.pageMember.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabGroup;
        private System.Windows.Forms.TabPage pageGroup;
        private System.Windows.Forms.MaskedTextBox txtDescrip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage pageMember;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private MaterialSkin.Controls.MaterialButton btnExit;
        private MaterialSkin.Controls.MaterialButton btnDel;
        private MaterialSkin.Controls.MaterialButton btnAdd;
        private System.Windows.Forms.DataGridView gvMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserID;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
    }
}