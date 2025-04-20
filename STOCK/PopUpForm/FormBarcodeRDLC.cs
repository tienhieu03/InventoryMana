using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using STOCK;

namespace STOCK.PopUpForm
{
    public partial class FormBarcodeRDLC : Form
    {
        public FormBarcodeRDLC(DataTable dt)
        {
            InitializeComponent();

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            // Ensure the path includes the subdirectory where the RDLC file is copied.
            reportViewer1.LocalReport.ReportPath = @"RDLCReport\PrintBarcode.rdlc"; 
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void FormBarcodeRDLC_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
    }

}
