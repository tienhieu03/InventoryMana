using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using MATERIAL.MyFunctions;

namespace STOCK.Forms
{
    public partial class formReportDepartment : Form
    {
        public formReportDepartment()
        {
            InitializeComponent();
        }

        public formReportDepartment(TextBox txtDepartment)
        {
            InitializeComponent();
            this._txtDepartment = txtDepartment;
        }

        TextBox _txtDepartment;

        DEPARTMENT _de;
        COMPANY _company;

        private void formReportDepartment_Load(object sender, EventArgs e)
        {
            _company = new COMPANY();
            _de = new DEPARTMENT();
            LoadCompany();
            LoadDepartment();
            cboCompany.SelectedIndexChanged += CboCompany_SelectedIndexChanged;
            cboCompany.SelectedValue = myFunctions._compid;
        }

        private void CboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }
        void LoadDepartment()
        {
            dgvList.DataSource = _de.getAll(cboCompany.SelectedValue.ToString());
            dgvList.ReadOnly = true;
        }
    }
}
