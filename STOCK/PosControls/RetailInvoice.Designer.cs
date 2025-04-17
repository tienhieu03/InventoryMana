﻿﻿﻿namespace STOCK.Controls
{
    partial class RetailInvoice
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetailInvoice));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvRetail = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.customPanel1 = new SharedControls.CustomPanel();
            this.txtTotalPrice = new MaterialSkin.Controls.MaterialMaskedTextBox();
            this.txtTotalQuantity = new MaterialSkin.Controls.MaterialMaskedTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDiscount = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.txtBarcode = new System.Windows.Forms.MaskedTextBox();
            this.BARCODE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiscountAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRetail
            // 
            this.dgvRetail.AllowUserToAddRows = false;
            this.dgvRetail.AllowUserToDeleteRows = false;
            this.dgvRetail.AllowUserToResizeColumns = false;
            this.dgvRetail.AllowUserToResizeRows = false;
            this.dgvRetail.BackgroundColor = System.Drawing.Color.White;
            this.dgvRetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRetail.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(145)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRetail.ColumnHeadersHeight = 35;
            this.dgvRetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BARCODE,
            this.ProductID,
            this.ProductName,
            this.Unit,
            this.Quantity,
            this.Price,
            this.DiscountAmount,
            this.SubTotal});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRetail.EnableHeadersVisualStyles = false;
            this.dgvRetail.Location = new System.Drawing.Point(4, 0);
            this.dgvRetail.Name = "dgvRetail";
            this.dgvRetail.ReadOnly = true;
            this.dgvRetail.Size = new System.Drawing.Size(940, 281);
            this.dgvRetail.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.customPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtTotalPrice);
            this.splitContainer1.Panel2.Controls.Add(this.txtTotalQuantity);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnDiscount);
            this.splitContainer1.Panel2.Controls.Add(this.btnReturn);
            this.splitContainer1.Panel2.Controls.Add(this.btnSave);
            this.splitContainer1.Panel2.Controls.Add(this.btnPrint);
            this.splitContainer1.Panel2.Controls.Add(this.txtBarcode);
            this.splitContainer1.Size = new System.Drawing.Size(948, 500);
            this.splitContainer1.SplitterDistance = 296;
            this.splitContainer1.TabIndex = 1;
            // 
            // customPanel1
            // 
            this.customPanel1.BackColor = System.Drawing.Color.White;
            this.customPanel1.BorderRadius = 30;
            this.customPanel1.Controls.Add(this.dgvRetail);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.ForeColor = System.Drawing.Color.Black;
            this.customPanel1.GradientAngle = 90F;
            this.customPanel1.GradientBottomColor = System.Drawing.Color.CadetBlue;
            this.customPanel1.GradientTopColor = System.Drawing.Color.DodgerBlue;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 15);
            this.customPanel1.Size = new System.Drawing.Size(948, 296);
            this.customPanel1.TabIndex = 7;
            // 
            // txtTotalPrice
            // 
            this.txtTotalPrice.AllowPromptAsInput = true;
            this.txtTotalPrice.AnimateReadOnly = false;
            this.txtTotalPrice.AsciiOnly = false;
            this.txtTotalPrice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtTotalPrice.BeepOnError = false;
            this.txtTotalPrice.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtTotalPrice.Depth = 0;
            this.txtTotalPrice.Enabled = false;
            this.txtTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTotalPrice.HidePromptOnLeave = false;
            this.txtTotalPrice.HideSelection = true;
            this.txtTotalPrice.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Default;
            this.txtTotalPrice.LeadingIcon = null;
            this.txtTotalPrice.Location = new System.Drawing.Point(792, 11);
            this.txtTotalPrice.Mask = "";
            this.txtTotalPrice.MaxLength = 32767;
            this.txtTotalPrice.MouseState = MaterialSkin.MouseState.OUT;
            this.txtTotalPrice.Name = "txtTotalPrice";
            this.txtTotalPrice.PasswordChar = '\0';
            this.txtTotalPrice.PrefixSuffixText = null;
            this.txtTotalPrice.PromptChar = '_';
            this.txtTotalPrice.ReadOnly = false;
            this.txtTotalPrice.RejectInputOnFirstFailure = false;
            this.txtTotalPrice.ResetOnPrompt = true;
            this.txtTotalPrice.ResetOnSpace = true;
            this.txtTotalPrice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTotalPrice.SelectedText = "";
            this.txtTotalPrice.SelectionLength = 0;
            this.txtTotalPrice.SelectionStart = 0;
            this.txtTotalPrice.ShortcutsEnabled = true;
            this.txtTotalPrice.Size = new System.Drawing.Size(126, 48);
            this.txtTotalPrice.SkipLiterals = true;
            this.txtTotalPrice.TabIndex = 9;
            this.txtTotalPrice.TabStop = false;
            this.txtTotalPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtTotalPrice.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtTotalPrice.TrailingIcon = null;
            this.txtTotalPrice.UseSystemPasswordChar = false;
            this.txtTotalPrice.ValidatingType = null;
            // 
            // txtTotalQuantity
            // 
            this.txtTotalQuantity.AllowPromptAsInput = true;
            this.txtTotalQuantity.AnimateReadOnly = false;
            this.txtTotalQuantity.AsciiOnly = false;
            this.txtTotalQuantity.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.txtTotalQuantity.BeepOnError = false;
            this.txtTotalQuantity.CutCopyMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtTotalQuantity.Depth = 0;
            this.txtTotalQuantity.Enabled = false;
            this.txtTotalQuantity.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTotalQuantity.HidePromptOnLeave = false;
            this.txtTotalQuantity.HideSelection = true;
            this.txtTotalQuantity.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Default;
            this.txtTotalQuantity.LeadingIcon = null;
            this.txtTotalQuantity.Location = new System.Drawing.Point(674, 11);
            this.txtTotalQuantity.Mask = "";
            this.txtTotalQuantity.MaxLength = 32767;
            this.txtTotalQuantity.MouseState = MaterialSkin.MouseState.OUT;
            this.txtTotalQuantity.Name = "txtTotalQuantity";
            this.txtTotalQuantity.PasswordChar = '\0';
            this.txtTotalQuantity.PrefixSuffixText = null;
            this.txtTotalQuantity.PromptChar = '_';
            this.txtTotalQuantity.ReadOnly = false;
            this.txtTotalQuantity.RejectInputOnFirstFailure = false;
            this.txtTotalQuantity.ResetOnPrompt = true;
            this.txtTotalQuantity.ResetOnSpace = true;
            this.txtTotalQuantity.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTotalQuantity.SelectedText = "";
            this.txtTotalQuantity.SelectionLength = 0;
            this.txtTotalQuantity.SelectionStart = 0;
            this.txtTotalQuantity.ShortcutsEnabled = true;
            this.txtTotalQuantity.Size = new System.Drawing.Size(112, 48);
            this.txtTotalQuantity.SkipLiterals = true;
            this.txtTotalQuantity.TabIndex = 8;
            this.txtTotalQuantity.TabStop = false;
            this.txtTotalQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtTotalQuantity.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtTotalQuantity.TrailingIcon = null;
            this.txtTotalQuantity.UseSystemPasswordChar = false;
            this.txtTotalQuantity.ValidatingType = null;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 93);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnDiscount
            // 
            this.btnDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiscount.Location = new System.Drawing.Point(674, 93);
            this.btnDiscount.Name = "btnDiscount";
            this.btnDiscount.Size = new System.Drawing.Size(112, 53);
            this.btnDiscount.TabIndex = 4;
            this.btnDiscount.Text = "Discount";
            this.btnDiscount.UseVisualStyleBackColor = true;
            this.btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReturn.Location = new System.Drawing.Point(792, 93);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(126, 53);
            this.btnReturn.TabIndex = 3;
            this.btnReturn.Text = "Return Product";
            this.btnReturn.UseVisualStyleBackColor = true;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(556, 93);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 53);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.Location = new System.Drawing.Point(436, 93);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(114, 53);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print Bill";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // txtBarcode
            // 
            this.txtBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Location = new System.Drawing.Point(104, 96);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(294, 53);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // BARCODE
            // 
            this.BARCODE.DataPropertyName = "BARCODE";
            this.BARCODE.HeaderText = "BARCODE";
            this.BARCODE.Name = "BARCODE";
            this.BARCODE.ReadOnly = true;
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            this.ProductID.HeaderText = "ProductID";
            this.ProductID.Name = "ProductID";
            this.ProductID.ReadOnly = true;
            this.ProductID.Visible = false;
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
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            this.Quantity.Width = 80;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            dataGridViewCellStyle2.Format = "#,0 ₫";
            this.Price.DefaultCellStyle = dataGridViewCellStyle2;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // DiscountAmount
            // 
            this.DiscountAmount.DataPropertyName = "DiscountAmount";
            this.DiscountAmount.HeaderText = "Discount";
            this.DiscountAmount.Name = "DiscountAmount";
            this.DiscountAmount.ReadOnly = true;
            this.DiscountAmount.Width = 80;
            // 
            // SubTotal
            // 
            this.SubTotal.DataPropertyName = "SubTotal";
            dataGridViewCellStyle3.Format = "#,0 ₫";
            this.SubTotal.DefaultCellStyle = dataGridViewCellStyle3;
            this.SubTotal.HeaderText = "Sub Total";
            this.SubTotal.Name = "SubTotal";
            this.SubTotal.ReadOnly = true;
            // 
            // RetailInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RetailInvoice";
            this.Size = new System.Drawing.Size(948, 500);
            this.Load += new System.EventHandler(this.RetailInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRetail)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private SharedControls.CustomPanel customPanel1;
        private System.Windows.Forms.MaskedTextBox txtBarcode;
        private System.Windows.Forms.Button btnDiscount;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialMaskedTextBox txtTotalPrice;
        private MaterialSkin.Controls.MaterialMaskedTextBox txtTotalQuantity;
        public System.Windows.Forms.DataGridView dgvRetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn BARCODE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiscountAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubTotal;
    }
}
