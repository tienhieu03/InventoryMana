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
using MaterialSkin.Controls;

namespace STOCK.Forms
{
    public partial class formSetParam : MaterialForm
    {
        public formSetParam()
        {
            InitializeComponent();
        }
        COMPANY _company;
        DEPARTMENT _department;
        private void formSetParam_Load(object sender, EventArgs e)
        {
            _company = new COMPANY();
            _department = new DEPARTMENT();
            LoadCompany();
            cboCompany.SelectedValue = "DEMO01";
            cboCompany.SelectedIndexChanged += CboCompany_SelectedIndexChanged; ;
            LoadDepartment();
        }

        private void LoadDepartment()
        {
            cboDepartment.DataSource = _department.getAll(cboCompany.SelectedValue.ToString());
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentID";
            cboDepartment.SelectedIndex = -1;
        }

        private void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }

        private void CboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string macty = cboCompany.SelectedValue.ToString();
            string madvi = (cboDepartment.Text.Trim() == "") ? "~" : cboDepartment.SelectedValue.ToString();
            SYS_PARAM _sysParam = new SYS_PARAM(macty, madvi);
            _sysParam.SaveFile();
            MessageBox.Show("Xác lập đơn vị sử dụng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
