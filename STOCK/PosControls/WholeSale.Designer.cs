namespace POS.PosControls
{
    partial class WholeSale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WholeSale));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonToolStrip1 = new Krypton.Toolkit.KryptonToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.tabInvoice = new System.Windows.Forms.TabControl();
            this.pageList = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cboDepartment = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboCompany = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtTill = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.customPanel1 = new SharedControls.CustomPanel();
            this.gvList = new System.Windows.Forms.DataGridView();
            this.pageDetail = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.cboCustomer = new System.Windows.Forms.ComboBox();
            this.txtDiscount = new System.Windows.Forms.MaskedTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblDelete = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboExportUnit = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.gvDetail = new System.Windows.Forms.DataGridView();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BARCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuantityDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QRCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CmdDeleteRow = new System.Windows.Forms.ToolStripMenuItem();
            this.CmdDeleteDetail = new System.Windows.Forms.ToolStripMenuItem();
            this.CmdImport = new System.Windows.Forms.ToolStripMenuItem();
            this.DeletedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceNo2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonToolStrip1.SuspendLayout();
            this.tabInvoice.SuspendLayout();
            this.pageList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).BeginInit();
            this.pageDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
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
            this.btnAdd,
            this.btnEdit,
            this.btnDelete,
            this.btnSave,
            this.btnCancel,
            this.btnPrint});
            this.kryptonToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.kryptonToolStrip1.Name = "kryptonToolStrip1";
            this.kryptonToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.kryptonToolStrip1.Size = new System.Drawing.Size(1264, 43);
            this.kryptonToolStrip1.TabIndex = 11;
            this.kryptonToolStrip1.Text = "kryptonToolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 40);
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(59, 40);
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 40);
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 40);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(77, 40);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(63, 40);
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // tabInvoice
            // 
            this.tabInvoice.Controls.Add(this.pageList);
            this.tabInvoice.Controls.Add(this.pageDetail);
            this.tabInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInvoice.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabInvoice.Location = new System.Drawing.Point(0, 43);
            this.tabInvoice.Margin = new System.Windows.Forms.Padding(4);
            this.tabInvoice.Name = "tabInvoice";
            this.tabInvoice.SelectedIndex = 0;
            this.tabInvoice.Size = new System.Drawing.Size(1264, 572);
            this.tabInvoice.TabIndex = 12;
            // 
            // pageList
            // 
            this.pageList.Controls.Add(this.splitContainer1);
            this.pageList.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageList.Location = new System.Drawing.Point(4, 28);
            this.pageList.Margin = new System.Windows.Forms.Padding(4);
            this.pageList.Name = "pageList";
            this.pageList.Padding = new System.Windows.Forms.Padding(4);
            this.pageList.Size = new System.Drawing.Size(1256, 540);
            this.pageList.TabIndex = 0;
            this.pageList.Text = "List";
            this.pageList.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cboDepartment);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.cboCompany);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.dtTill);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.dtFrom);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(1248, 532);
            this.splitContainer1.SplitterDistance = 112;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 3;
            // 
            // cboDepartment
            // 
            this.cboDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDepartment.FormattingEnabled = true;
            this.cboDepartment.Location = new System.Drawing.Point(265, 79);
            this.cboDepartment.Margin = new System.Windows.Forms.Padding(4);
            this.cboDepartment.Name = "cboDepartment";
            this.cboDepartment.Size = new System.Drawing.Size(692, 27);
            this.cboDepartment.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(161, 81);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Department";
            // 
            // cboCompany
            // 
            this.cboCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCompany.FormattingEnabled = true;
            this.cboCompany.Location = new System.Drawing.Point(265, 46);
            this.cboCompany.Margin = new System.Windows.Forms.Padding(4);
            this.cboCompany.Name = "cboCompany";
            this.cboCompany.Size = new System.Drawing.Size(692, 27);
            this.cboCompany.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(161, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Company";
            // 
            // dtTill
            // 
            this.dtTill.CustomFormat = "";
            this.dtTill.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTill.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTill.Location = new System.Drawing.Point(692, 6);
            this.dtTill.Margin = new System.Windows.Forms.Padding(4);
            this.dtTill.Name = "dtTill";
            this.dtTill.Size = new System.Drawing.Size(265, 26);
            this.dtTill.TabIndex = 3;
            this.dtTill.ValueChanged += new System.EventHandler(this.dtTill_ValueChanged);
            this.dtTill.Leave += new System.EventHandler(this.dtTill_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(616, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Till";
            // 
            // dtFrom
            // 
            this.dtFrom.CustomFormat = "";
            this.dtFrom.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(265, 6);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(265, 26);
            this.dtFrom.TabIndex = 1;
            this.dtFrom.ValueChanged += new System.EventHandler(this.dtFrom_ValueChanged);
            this.dtFrom.Leave += new System.EventHandler(this.dtFrom_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "From";
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.White;
            this.customPanel1.BorderRadius = 30;
            this.customPanel1.Controls.Add(this.gvList);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.ForeColor = System.Drawing.Color.Black;
            this.customPanel1.GradientAngle = 90F;
            this.customPanel1.GradientBottomColor = System.Drawing.Color.CadetBlue;
            this.customPanel1.GradientTopColor = System.Drawing.Color.DodgerBlue;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Padding = new System.Windows.Forms.Padding(5, 0, 5, 18);
            this.customPanel1.Size = new System.Drawing.Size(1248, 415);
            this.customPanel1.TabIndex = 8;
            // 
            // gvList
            // 
            this.gvList.AllowUserToAddRows = false;
            this.gvList.AllowUserToDeleteRows = false;
            this.gvList.AllowUserToResizeColumns = false;
            this.gvList.AllowUserToResizeRows = false;
            this.gvList.BackgroundColor = System.Drawing.Color.White;
            this.gvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gvList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(145)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvList.ColumnHeadersHeight = 35;
            this.gvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeletedBy,
            this.InvoiceID,
            this.InvoiceNo,
            this.Date,
            this.InvoiceNo2,
            this.Day2,
            this.Quantity,
            this.TotalPrice,
            this.Description,
            this.Status});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvList.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvList.EnableHeadersVisualStyles = false;
            this.gvList.Location = new System.Drawing.Point(5, 0);
            this.gvList.Margin = new System.Windows.Forms.Padding(4);
            this.gvList.Name = "gvList";
            this.gvList.ReadOnly = true;
            this.gvList.RowHeadersVisible = false;
            this.gvList.RowHeadersWidth = 51;
            this.gvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvList.Size = new System.Drawing.Size(1238, 397);
            this.gvList.TabIndex = 0;
            // 
            // pageDetail
            // 
            this.pageDetail.Controls.Add(this.splitContainer2);
            this.pageDetail.Location = new System.Drawing.Point(4, 28);
            this.pageDetail.Margin = new System.Windows.Forms.Padding(4);
            this.pageDetail.Name = "pageDetail";
            this.pageDetail.Padding = new System.Windows.Forms.Padding(4);
            this.pageDetail.Size = new System.Drawing.Size(1256, 540);
            this.pageDetail.TabIndex = 1;
            this.pageDetail.Text = "Detail";
            this.pageDetail.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(4, 4);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.cboCustomer);
            this.splitContainer2.Panel1.Controls.Add(this.txtDiscount);
            this.splitContainer2.Panel1.Controls.Add(this.label11);
            this.splitContainer2.Panel1.Controls.Add(this.lblDelete);
            this.splitContainer2.Panel1.Controls.Add(this.txtNote);
            this.splitContainer2.Panel1.Controls.Add(this.label10);
            this.splitContainer2.Panel1.Controls.Add(this.label9);
            this.splitContainer2.Panel1.Controls.Add(this.cboExportUnit);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            this.splitContainer2.Panel1.Controls.Add(this.cboStatus);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            this.splitContainer2.Panel1.Controls.Add(this.txtInvoiceNo);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.dtDate);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gvDetail);
            this.splitContainer2.Size = new System.Drawing.Size(1248, 532);
            this.splitContainer2.SplitterDistance = 146;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 0;
            // 
            // cboCustomer
            // 
            this.cboCustomer.FormattingEnabled = true;
            this.cboCustomer.Location = new System.Drawing.Point(644, 78);
            this.cboCustomer.Margin = new System.Windows.Forms.Padding(4);
            this.cboCustomer.Name = "cboCustomer";
            this.cboCustomer.Size = new System.Drawing.Size(311, 27);
            this.cboCustomer.TabIndex = 18;
            // 
            // txtDiscount
            // 
            this.txtDiscount.Location = new System.Drawing.Point(995, 79);
            this.txtDiscount.Margin = new System.Windows.Forms.Padding(4);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(53, 26);
            this.txtDiscount.TabIndex = 17;
            this.txtDiscount.Leave += new System.EventHandler(this.txtDiscount_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(976, 39);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 19);
            this.label11.TabIndex = 16;
            this.label11.Text = "Discount (%)";
            // 
            // lblDelete
            // 
            this.lblDelete.AutoSize = true;
            this.lblDelete.ForeColor = System.Drawing.Color.Red;
            this.lblDelete.Location = new System.Drawing.Point(11, 7);
            this.lblDelete.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(63, 19);
            this.lblDelete.TabIndex = 14;
            this.lblDelete.Text = "Deleted";
            this.lblDelete.Visible = false;
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(52, 113);
            this.txtNote.Margin = new System.Windows.Forms.Padding(4);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(1123, 26);
            this.txtNote.TabIndex = 13;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(33, 113);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 19);
            this.label10.TabIndex = 12;
            this.label10.Text = "Note";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(559, 81);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 19);
            this.label9.TabIndex = 10;
            this.label9.Text = "Customer";
            // 
            // cboExportUnit
            // 
            this.cboExportUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExportUnit.FormattingEnabled = true;
            this.cboExportUnit.Location = new System.Drawing.Point(105, 75);
            this.cboExportUnit.Margin = new System.Windows.Forms.Padding(4);
            this.cboExportUnit.Name = "cboExportUnit";
            this.cboExportUnit.Size = new System.Drawing.Size(377, 27);
            this.cboExportUnit.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 79);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 19);
            this.label8.TabIndex = 8;
            this.label8.Text = "Export Unit";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(703, 36);
            this.cboStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(252, 27);
            this.cboStatus.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(640, 39);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 19);
            this.label7.TabIndex = 6;
            this.label7.Text = "Status";
            // 
            // txtInvoiceNo
            // 
            this.txtInvoiceNo.Location = new System.Drawing.Point(436, 32);
            this.txtInvoiceNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.ReadOnly = true;
            this.txtInvoiceNo.Size = new System.Drawing.Size(179, 26);
            this.txtInvoiceNo.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(340, 38);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 19);
            this.label6.TabIndex = 4;
            this.label6.Text = "Invoice No";
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "";
            this.dtDate.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDate.Location = new System.Drawing.Point(80, 32);
            this.dtDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(239, 26);
            this.dtDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 36);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "From";
            // 
            // gvDetail
            // 
            this.gvDetail.AllowUserToAddRows = false;
            this.gvDetail.AllowUserToDeleteRows = false;
            this.gvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.BARCODE,
            this.ProductName,
            this.Unit,
            this.QuantityDetail,
            this.Price,
            this.DiscountAmount,
            this.SubTotal,
            this.QRCODE,
            this.ProductID});
            this.gvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDetail.Location = new System.Drawing.Point(0, 0);
            this.gvDetail.Margin = new System.Windows.Forms.Padding(4);
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.ReadOnly = true;
            this.gvDetail.RowHeadersVisible = false;
            this.gvDetail.RowHeadersWidth = 51;
            this.gvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDetail.Size = new System.Drawing.Size(1248, 381);
            this.gvDetail.TabIndex = 0;
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            this.STT.HeaderText = "STT";
            this.STT.MinimumWidth = 6;
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 125;
            // 
            // BARCODE
            // 
            this.BARCODE.DataPropertyName = "BARCODE";
            this.BARCODE.HeaderText = "BARCODE";
            this.BARCODE.MinimumWidth = 6;
            this.BARCODE.Name = "BARCODE";
            this.BARCODE.ReadOnly = true;
            this.BARCODE.Width = 125;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.MinimumWidth = 6;
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 150;
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "Unit";
            this.Unit.HeaderText = "Unit";
            this.Unit.MinimumWidth = 6;
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Width = 125;
            // 
            // QuantityDetail
            // 
            this.QuantityDetail.DataPropertyName = "QuantityDetail";
            this.QuantityDetail.HeaderText = "Quantity";
            this.QuantityDetail.MinimumWidth = 6;
            this.QuantityDetail.Name = "QuantityDetail";
            this.QuantityDetail.ReadOnly = true;
            this.QuantityDetail.Width = 125;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Price";
            this.Price.MinimumWidth = 6;
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Width = 125;
            // 
            // DiscountAmount
            // 
            this.DiscountAmount.DataPropertyName = "DiscountAmount";
            this.DiscountAmount.HeaderText = "Discount";
            this.DiscountAmount.MinimumWidth = 6;
            this.DiscountAmount.Name = "DiscountAmount";
            this.DiscountAmount.ReadOnly = true;
            this.DiscountAmount.Width = 125;
            // 
            // SubTotal
            // 
            this.SubTotal.DataPropertyName = "SubTotal";
            this.SubTotal.HeaderText = "Total Price";
            this.SubTotal.MinimumWidth = 6;
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.ReadOnly = true;
            this.SubTotal.Width = 125;
            // 
            // QRCODE
            // 
            this.QRCODE.DataPropertyName = "QRCODE";
            this.QRCODE.HeaderText = "QRCODE";
            this.QRCODE.MinimumWidth = 6;
            this.QRCODE.Name = "QRCODE";
            this.QRCODE.ReadOnly = true;
            this.QRCODE.Visible = false;
            this.QRCODE.Width = 125;
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            this.ProductID.HeaderText = "ProductID";
            this.ProductID.MinimumWidth = 6;
            this.ProductID.Name = "ProductID";
            this.ProductID.ReadOnly = true;
            this.ProductID.Visible = false;
            this.ProductID.Width = 125;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmdDeleteRow,
            this.CmdDeleteDetail,
            this.CmdImport});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(167, 76);
            // 
            // CmdDeleteRow
            // 
            this.CmdDeleteRow.Name = "CmdDeleteRow";
            this.CmdDeleteRow.Size = new System.Drawing.Size(166, 24);
            this.CmdDeleteRow.Text = "Delete Row";
            this.CmdDeleteRow.Click += new System.EventHandler(this.CmdDeleteRow_Click);
            // 
            // CmdDeleteDetail
            // 
            this.CmdDeleteDetail.Name = "CmdDeleteDetail";
            this.CmdDeleteDetail.Size = new System.Drawing.Size(166, 24);
            this.CmdDeleteDetail.Text = "Delete Detail";
            this.CmdDeleteDetail.Click += new System.EventHandler(this.CmdDeleteDetail_Click);
            // 
            // CmdImport
            // 
            this.CmdImport.Name = "CmdImport";
            this.CmdImport.Size = new System.Drawing.Size(166, 24);
            this.CmdImport.Text = "Import Excel";
            this.CmdImport.Click += new System.EventHandler(this.CmdImport_Click);
            // 
            // DeletedBy
            // 
            this.DeletedBy.DataPropertyName = "DeletedBy";
            this.DeletedBy.HeaderText = "DEL";
            this.DeletedBy.MinimumWidth = 6;
            this.DeletedBy.Name = "DeletedBy";
            this.DeletedBy.ReadOnly = true;
            this.DeletedBy.Width = 50;
            // 
            // InvoiceID
            // 
            this.InvoiceID.DataPropertyName = "InvoiceID";
            this.InvoiceID.HeaderText = "InvoiceID";
            this.InvoiceID.MinimumWidth = 6;
            this.InvoiceID.Name = "InvoiceID";
            this.InvoiceID.ReadOnly = true;
            this.InvoiceID.Visible = false;
            this.InvoiceID.Width = 125;
            // 
            // InvoiceNo
            // 
            this.InvoiceNo.DataPropertyName = "Invoice";
            this.InvoiceNo.HeaderText = "Invoice No";
            this.InvoiceNo.MinimumWidth = 6;
            this.InvoiceNo.Name = "InvoiceNo";
            this.InvoiceNo.ReadOnly = true;
            this.InvoiceNo.Width = 170;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Day";
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Width = 125;
            // 
            // InvoiceNo2
            // 
            this.InvoiceNo2.DataPropertyName = "Invoice2";
            this.InvoiceNo2.HeaderText = "InvoiceNo2";
            this.InvoiceNo2.MinimumWidth = 6;
            this.InvoiceNo2.Name = "InvoiceNo2";
            this.InvoiceNo2.ReadOnly = true;
            this.InvoiceNo2.Visible = false;
            this.InvoiceNo2.Width = 125;
            // 
            // Day2
            // 
            this.Day2.DataPropertyName = "Day2";
            this.Day2.HeaderText = "Receive Date";
            this.Day2.MinimumWidth = 6;
            this.Day2.Name = "Day2";
            this.Day2.ReadOnly = true;
            this.Day2.Visible = false;
            this.Day2.Width = 125;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.MinimumWidth = 6;
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 80;
            // 
            // TotalPrice
            // 
            this.TotalPrice.DataPropertyName = "TotalPrice";
            this.TotalPrice.HeaderText = "Total Price";
            this.TotalPrice.MinimumWidth = 6;
            this.TotalPrice.Name = "TotalPrice";
            this.TotalPrice.ReadOnly = true;
            this.TotalPrice.Width = 170;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "Status";
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 125;
            // 
            // WholeSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabInvoice);
            this.Controls.Add(this.kryptonToolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WholeSale";
            this.Size = new System.Drawing.Size(1264, 615);
            this.Load += new System.EventHandler(this.WholeSale_Load);
            this.kryptonToolStrip1.ResumeLayout(false);
            this.kryptonToolStrip1.PerformLayout();
            this.tabInvoice.ResumeLayout(false);
            this.pageList.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvList)).EndInit();
            this.pageDetail.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonToolStrip kryptonToolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.TabControl tabInvoice;
        private System.Windows.Forms.TabPage pageList;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ComboBox cboDepartment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboCompany;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtTill;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvList;
        private System.Windows.Forms.TabPage pageDetail;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MaskedTextBox txtDiscount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.MaskedTextBox txtNote;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboExportUnit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox txtInvoiceNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView gvDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn BARCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn QuantityDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn QRCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem CmdDeleteRow;
        private System.Windows.Forms.ToolStripMenuItem CmdDeleteDetail;
        private System.Windows.Forms.ToolStripMenuItem CmdImport;
        private System.Windows.Forms.ComboBox cboCustomer;
        private SharedControls.CustomPanel customPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeletedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceID;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNo2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
    }
}
