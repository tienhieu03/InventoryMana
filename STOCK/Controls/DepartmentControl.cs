using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;
using DataLayer;
using STOCK.Forms;
using STOCK.Controls;

namespace STOCK.Controls
{
    public partial class DepartmentControl : UserControl
    {
        public DepartmentControl()
        {
            InitializeComponent();

        }
        DEPARTMENT _department;
        COMPANY _company;
        bool _add;
        string _departmentid;

        private void ShowHideControls(bool t)
        {
            btnAdd.Visible = t;
            btnEdit.Visible = t;
            btnDelete.Visible = t;
            btnSave.Visible = !t;
            btnClear.Visible = !t;
        }
        private void _enable(bool t)
        {
            txtName.Enabled = t;
            txtPhone.Enabled = t;
            txtFax.Enabled = t;
            txtEmail.Enabled = t;
            txtAddress.Enabled = t;
            chkDisable.Enabled = t;
            chkWarehouse.Enabled = t;
            txtSymbol.Enabled = t;
        }
        private void ResetFields()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            chkDisable.Checked = false;
            chkWarehouse.Checked = false;
            txtSymbol.Text = "";
        }
        private void LoadCompany()
        {
            var companies = _company.getAll();
            if (companies == null || companies.Count == 0)
            {
                MessageBox.Show("No company data found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CboCp.DataSource = companies;
            CboCp.DisplayMember = "CompanyName";
            CboCp.ValueMember = "CompanyID";
        }

        private void LoadDpByCp()
        {
            gvList.DataSource = _department.getAll(CboCp.SelectedValue.ToString());
            gvList.ReadOnly = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _add = true;
            ResetFields();
            _enable(true);
            ShowHideControls(false);
            txtId.Enabled = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(true);
            ShowHideControls(false);
            txtId.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete this record?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                _department.delete(_departmentid);
            }
            LoadDpByCp();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_add)
            {
                tb_Department dp = new tb_Department()
                {
                    DepartmentID = txtId.Text,
                    CompanyID = CboCp.SelectedValue.ToString(),
                    DepartmentName = txtName.Text,
                    DepartmentPhone = txtPhone.Text,
                    DepartmentAddress = txtAddress.Text,
                    DepartmentFax = txtFax.Text,
                    DepartmentEmail = txtEmail.Text,
                    IsDisabled = chkDisable.Checked
                };
                _department.add(dp);
            }
            else
            {
                tb_Department dp = _department.getItem(_departmentid);
                dp.CompanyID = CboCp.SelectedValue.ToString();
                dp.DepartmentName = txtName.Text;
                dp.DepartmentPhone = txtPhone.Text;
                dp.DepartmentAddress = txtAddress.Text;
                dp.DepartmentFax = txtFax.Text;
                dp.DepartmentEmail = txtEmail.Text;
                dp.IsDisabled = chkDisable.Checked;
                _department.update(dp);
            }
            _add = false;
            LoadDpByCp();
            _enable(false);
            ShowHideControls(true);
            txtId.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ShowHideControls(true);
            txtId.Enabled = false;
            LoadDpByCp();
            ResetFields();
        }


        private void gvList_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];
                _departmentid = row.Cells["DepartmentID"].Value.ToString();
                CboCp.SelectedValue = row.Cells["CompanyID"].Value.ToString();
                txtId.Text = row.Cells["DepartmentID"].Value.ToString();
                txtName.Text = row.Cells["DepartmentName"].Value.ToString();
                txtPhone.Text = row.Cells["DepartmentPhone"].Value.ToString();
                txtFax.Text = row.Cells["DepartmentFax"].Value.ToString();
                txtEmail.Text = row.Cells["DepartmentEmail"].Value.ToString();
                txtAddress.Text = row.Cells["DepartmentAddress"].Value.ToString();
                txtSymbol.Text = row.Cells["Symbol"].Value.ToString();
                chkDisable.Checked = Convert.ToBoolean(row.Cells["IsDisabled"].Value);
                chkWarehouse.Checked = Convert.ToBoolean(row.Cells["Warehouse"].Value);
            }

        }

        private void CboCp_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDpByCp();
        }

        private void DepartmentControl_Load(object sender, EventArgs e)
        {
            MessageBox.Show("DepartmentControl Loaded!");
            _department = new DEPARTMENT();
            _company = new COMPANY();
            LoadCompany();
            ShowHideControls(true);
            _enable(false);
            txtId.Enabled = false;
            CboCp.SelectedIndexChanged += CboCp_SelectedIndexChanged;
            LoadDpByCp();
        }
    }
}
