namespace SharedControls
{
    partial class ucDepartment
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
            this.linkDepartment = new System.Windows.Forms.LinkLabel();
            this.txtDepartment = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // linkDepartment
            // 
            this.linkDepartment.AutoSize = true;
            this.linkDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkDepartment.Location = new System.Drawing.Point(9, 11);
            this.linkDepartment.Name = "linkDepartment";
            this.linkDepartment.Size = new System.Drawing.Size(167, 18);
            this.linkDepartment.TabIndex = 0;
            this.linkDepartment.TabStop = true;
            this.linkDepartment.Text = "<<Department List>>";
            this.linkDepartment.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDepartment_LinkClicked);
            // 
            // txtDepartment
            // 
            this.txtDepartment.Location = new System.Drawing.Point(12, 32);
            this.txtDepartment.Multiline = true;
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(521, 165);
            this.txtDepartment.TabIndex = 1;
            // 
            // ucDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.linkDepartment);
            this.Name = "ucDepartment";
            this.Size = new System.Drawing.Size(550, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkDepartment;
        public System.Windows.Forms.TextBox txtDepartment;
    }
}
