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

namespace STOCK.Forms
{
    public partial class formReportViewer : Form
    {
        private LocalReport _report;

        // Constructor to accept the LocalReport object
        public formReportViewer(LocalReport report)
        {
            InitializeComponent();
            _report = report;
        }

        private void reportViewer_Load(object sender, EventArgs e)
        {
            if (_report != null)
            {
                // Assign the report definition and data sources to the viewer
                this.reportViewer1.LocalReport.ReportEmbeddedResource = _report.ReportEmbeddedResource;
                this.reportViewer1.LocalReport.DataSources.Clear();
                foreach (var ds in _report.DataSources)
                {
                    this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ds.Name, ds.Value));
                }

                // Refresh the report viewer to display the report
                this.reportViewer1.RefreshReport();
            }
            else
            {
                MessageBox.Show("Report data is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

        }
    }
}
