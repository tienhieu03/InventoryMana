namespace STOCK.Controls
{
    partial class CompanyStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompanyStock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonToolStrip1 = new Krypton.Toolkit.KryptonToolStrip();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.MaskedTextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.cboCompany = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtPeriod = new System.Windows.Forms.DateTimePicker();
            this.customPanel1 = new SharedControls.CustomPanel();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.BARCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BeginningQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceivedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecInnerQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutInnerQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BulkQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IssuedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FinalQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBarExport = new System.Windows.Forms.ProgressBar();
            this.kryptonToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonToolStrip1
            // 
            this.kryptonToolStrip1.AutoSize = false;
            this.kryptonToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.kryptonToolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.kryptonToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.kryptonToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.kryptonToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExport});
            this.kryptonToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.kryptonToolStrip1.Name = "kryptonToolStrip1";
            this.kryptonToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.kryptonToolStrip1.Size = new System.Drawing.Size(948, 35);
            this.kryptonToolStrip1.TabIndex = 2;
            this.kryptonToolStrip1.Text = "kryptonToolStrip1";
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(93, 32);
            this.btnExport.Text = "Export Excel";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(948, 465);
            this.splitContainer1.SplitterDistance = 62;
            this.splitContainer1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.txtSearch);
            this.panel2.Controls.Add(this.btnView);
            this.panel2.Controls.Add(this.cboCompany);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dtPeriod);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(948, 62);
            this.panel2.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(749, 36);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(126, 23);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "Find";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(268, 37);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(447, 20);
            this.txtSearch.TabIndex = 5;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Location = new System.Drawing.Point(749, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(126, 24);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cboCompany
            // 
            this.cboCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCompany.FormattingEnabled = true;
            this.cboCompany.Location = new System.Drawing.Point(268, 9);
            this.cboCompany.Name = "cboCompany";
            this.cboCompany.Size = new System.Drawing.Size(447, 24);
            this.cboCompany.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(52, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Company/Department";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(52, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Period";
            // 
            // dtPeriod
            // 
            this.dtPeriod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPeriod.Location = new System.Drawing.Point(112, 36);
            this.dtPeriod.Name = "dtPeriod";
            this.dtPeriod.Size = new System.Drawing.Size(131, 22);
            this.dtPeriod.TabIndex = 0;
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.White;
            this.customPanel1.BorderRadius = 30;
            this.customPanel1.Controls.Add(this.progressBarExport);
            this.customPanel1.Controls.Add(this.dgvStock);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.ForeColor = System.Drawing.Color.Black;
            this.customPanel1.GradientAngle = 90F;
            this.customPanel1.GradientBottomColor = System.Drawing.Color.CadetBlue;
            this.customPanel1.GradientTopColor = System.Drawing.Color.DodgerBlue;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.customPanel1.Size = new System.Drawing.Size(948, 399);
            this.customPanel1.TabIndex = 6;
            // 
            // dgvStock
            // 
            this.dgvStock.AllowUserToAddRows = false;
            this.dgvStock.AllowUserToDeleteRows = false;
            this.dgvStock.AllowUserToResizeColumns = false;
            this.dgvStock.AllowUserToResizeRows = false;
            this.dgvStock.BackgroundColor = System.Drawing.Color.White;
            this.dgvStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStock.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvStock.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(145)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvStock.ColumnHeadersHeight = 35;
            this.dgvStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BARCODE,
            this.ProductName,
            this.Unit,
            this.BeginningQuantity,
            this.ReceivedQuantity,
            this.RecInnerQuantity,
            this.OutInnerQuantity,
            this.BulkQuantity,
            this.IssuedQuantity,
            this.FinalQuantity,
            this.Value,
            this.TotalValue});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvStock.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStock.EnableHeadersVisualStyles = false;
            this.dgvStock.Location = new System.Drawing.Point(4, 0);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.ReadOnly = true;
            this.dgvStock.RowHeadersVisible = false;
            this.dgvStock.Size = new System.Drawing.Size(940, 384);
            this.dgvStock.TabIndex = 0;
            // 
            // BARCODE
            // 
            this.BARCODE.DataPropertyName = "BARCODE";
            this.BARCODE.HeaderText = "BARCODE";
            this.BARCODE.Name = "BARCODE";
            this.BARCODE.ReadOnly = true;
            this.BARCODE.Width = 150;
            // 
            // ProductName
            // 
            this.ProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "Unit";
            this.Unit.HeaderText = "Unit";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Width = 50;
            // 
            // BeginningQuantity
            // 
            this.BeginningQuantity.DataPropertyName = "BeginningQuantity";
            this.BeginningQuantity.HeaderText = "First Qty";
            this.BeginningQuantity.Name = "BeginningQuantity";
            this.BeginningQuantity.ReadOnly = true;
            this.BeginningQuantity.Width = 80;
            // 
            // ReceivedQuantity
            // 
            this.ReceivedQuantity.DataPropertyName = "ReceivedQuantity";
            this.ReceivedQuantity.HeaderText = "Rec Qty";
            this.ReceivedQuantity.Name = "ReceivedQuantity";
            this.ReceivedQuantity.ReadOnly = true;
            this.ReceivedQuantity.Width = 80;
            // 
            // RecInnerQuantity
            // 
            this.RecInnerQuantity.DataPropertyName = "RecInnerQuantity";
            this.RecInnerQuantity.HeaderText = "RecInnerQty";
            this.RecInnerQuantity.Name = "RecInnerQuantity";
            this.RecInnerQuantity.ReadOnly = true;
            // 
            // OutInnerQuantity
            // 
            this.OutInnerQuantity.DataPropertyName = "OutInnerQuantity";
            this.OutInnerQuantity.HeaderText = "OutInnerQty";
            this.OutInnerQuantity.Name = "OutInnerQuantity";
            this.OutInnerQuantity.ReadOnly = true;
            // 
            // BulkQuantity
            // 
            this.BulkQuantity.DataPropertyName = "BulkQuantity";
            this.BulkQuantity.HeaderText = "Bulk Qty";
            this.BulkQuantity.Name = "BulkQuantity";
            this.BulkQuantity.ReadOnly = true;
            this.BulkQuantity.Width = 80;
            // 
            // IssuedQuantity
            // 
            this.IssuedQuantity.DataPropertyName = "IssuedQuantity";
            this.IssuedQuantity.HeaderText = "Issued Qty";
            this.IssuedQuantity.Name = "IssuedQuantity";
            this.IssuedQuantity.ReadOnly = true;
            this.IssuedQuantity.Width = 80;
            // 
            // FinalQuantity
            // 
            this.FinalQuantity.DataPropertyName = "FinalQuantity";
            this.FinalQuantity.HeaderText = "Final Qty";
            this.FinalQuantity.Name = "FinalQuantity";
            this.FinalQuantity.ReadOnly = true;
            this.FinalQuantity.Width = 80;
            // 
            // Value
            // 
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // TotalValue
            // 
            this.TotalValue.DataPropertyName = "TotalValue";
            this.TotalValue.HeaderText = "TotalValue";
            this.TotalValue.Name = "TotalValue";
            this.TotalValue.ReadOnly = true;
            // 
            // progressBarExport
            // 
            this.progressBarExport.Location = new System.Drawing.Point(425, 162);
            this.progressBarExport.Name = "progressBarExport";
            this.progressBarExport.Size = new System.Drawing.Size(100, 23);
            this.progressBarExport.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarExport.TabIndex = 52;
            this.progressBarExport.Visible = false;
            // 
            // CompanyStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.kryptonToolStrip1);
            this.Name = "CompanyStock";
            this.Size = new System.Drawing.Size(948, 500);
            this.Load += new System.EventHandler(this.CompanyStock_Load);
            this.kryptonToolStrip1.ResumeLayout(false);
            this.kryptonToolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonToolStrip kryptonToolStrip1;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.MaskedTextBox txtSearch;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cboCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtPeriod;
        private SharedControls.CustomPanel customPanel1;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn BARCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn BeginningQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceivedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecInnerQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutInnerQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn BulkQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn IssuedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn FinalQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalValue;
        private System.Windows.Forms.ProgressBar progressBarExport;
    }
}
