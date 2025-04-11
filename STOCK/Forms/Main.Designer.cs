namespace STOCK.Forms
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.flpMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.imgListMenu = new System.Windows.Forms.ImageList(this.components);
            this.btnCountStock = new System.Windows.Forms.Button();
            this.pnlSidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.LightGray;
            this.pnlSidebar.Controls.Add(this.flpMenu);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(240, 648);
            this.pnlSidebar.TabIndex = 0;
            // 
            // flpMenu
            // 
            this.flpMenu.AutoScroll = true;
            this.flpMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.Location = new System.Drawing.Point(0, 0);
            this.flpMenu.Name = "flpMenu";
            this.flpMenu.Size = new System.Drawing.Size(240, 648);
            this.flpMenu.TabIndex = 0;
            this.flpMenu.WrapContents = false;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(243, 64);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1120, 648);
            this.pnlMain.TabIndex = 1;
            // 
            // imgListMenu
            // 
            this.imgListMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListMenu.ImageStream")));
            this.imgListMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListMenu.Images.SetKeyName(0, "DASHBOARD");
            this.imgListMenu.Images.SetKeyName(1, "INVOICE");
            this.imgListMenu.Images.SetKeyName(2, "MANAGEMENT");
            // 
            // btnCountStock
            // 
            this.btnCountStock.BackColor = System.Drawing.Color.Transparent;
            this.btnCountStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCountStock.Image = ((System.Drawing.Image)(resources.GetObject("btnCountStock.Image")));
            this.btnCountStock.Location = new System.Drawing.Point(1312, 28);
            this.btnCountStock.Name = "btnCountStock";
            this.btnCountStock.Size = new System.Drawing.Size(48, 30);
            this.btnCountStock.TabIndex = 2;
            this.btnCountStock.UseVisualStyleBackColor = false;
            this.btnCountStock.Click += new System.EventHandler(this.btnCountStock_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1366, 715);
            this.Controls.Add(this.btnCountStock);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSidebar);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inventory Management";
            this.Load += new System.EventHandler(this.Main_Load);
            this.pnlSidebar.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.FlowLayoutPanel flpMenu;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ImageList imgListMenu;
        private System.Windows.Forms.Button btnCountStock;
    }
}