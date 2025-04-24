namespace STOCK.Forms
{
    partial class formReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formReport));
            this.splReport = new System.Windows.Forms.SplitContainer();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lstList = new System.Windows.Forms.ListBox();
            this.btnCancel = new SharedControls.CustomButton();
            this.btnProceed = new SharedControls.CustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.splReport)).BeginInit();
            this.splReport.Panel1.SuspendLayout();
            this.splReport.Panel2.SuspendLayout();
            this.splReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // splReport
            // 
            this.splReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splReport.Location = new System.Drawing.Point(3, 24);
            this.splReport.Name = "splReport";
            // 
            // splReport.Panel1
            // 
            this.splReport.Panel1.Controls.Add(this.lstList);
            // 
            // splReport.Panel2
            // 
            this.splReport.Panel2.Controls.Add(this.btnCancel);
            this.splReport.Panel2.Controls.Add(this.btnProceed);
            this.splReport.Size = new System.Drawing.Size(834, 553);
            this.splReport.SplitterDistance = 277;
            this.splReport.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lstList
            // 
            this.lstList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstList.FormattingEnabled = true;
            this.lstList.Location = new System.Drawing.Point(0, 0);
            this.lstList.Name = "lstList";
            this.lstList.Size = new System.Drawing.Size(277, 553);
            this.lstList.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnCancel.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.btnCancel.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnCancel.BorderRadius = 20;
            this.btnCancel.BorderSize = 0;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(331, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 56);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextColor = System.Drawing.Color.White;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnProceed
            // 
            this.btnProceed.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnProceed.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.btnProceed.BorderColor = System.Drawing.Color.PaleVioletRed;
            this.btnProceed.BorderRadius = 20;
            this.btnProceed.BorderSize = 0;
            this.btnProceed.FlatAppearance.BorderSize = 0;
            this.btnProceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProceed.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProceed.ForeColor = System.Drawing.Color.White;
            this.btnProceed.Image = ((System.Drawing.Image)(resources.GetObject("btnProceed.Image")));
            this.btnProceed.Location = new System.Drawing.Point(78, 407);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(138, 56);
            this.btnProceed.TabIndex = 0;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnProceed.TextColor = System.Drawing.Color.White;
            this.btnProceed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProceed.UseVisualStyleBackColor = false;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // formReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 580);
            this.Controls.Add(this.splReport);
            this.FormStyle = MaterialSkin.Controls.MaterialForm.FormStyles.ActionBar_None;
            this.Name = "formReport";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "formReport";
            this.Load += new System.EventHandler(this.formReport_Load);
            this.splReport.Panel1.ResumeLayout(false);
            this.splReport.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splReport)).EndInit();
            this.splReport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splReport;
        private System.Windows.Forms.ImageList imageList1;
        private SharedControls.CustomButton btnProceed;
        private SharedControls.CustomButton btnCancel;
        private System.Windows.Forms.ListBox lstList;
    }
}