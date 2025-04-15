using System;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;
using DataLayer;
using STOCK.Forms;
using STOCK.Controls;
using SharedControls;

namespace STOCK.Controls
{
    public partial class DepartmentControl : UserControl
    {
        public DepartmentControl()
        {
            InitializeComponent();
            _right = 2; // Mặc định là full quyền nếu không truyền vào
        }
        
        public DepartmentControl(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            _user = user;
            _right = right;
        }
        
        DEPARTMENT _department;
        COMPANY _company;
        bool _add;
        string _departmentid;
        tb_SYS_USER _user;
        int _right;

        private void DepartmentControl_Load(object sender, EventArgs e)
        {
            _department = new DEPARTMENT();
            _company = new COMPANY();
            LoadCompany();
            ShowHideControls(true);
            _enable(false);
            txtId.Enabled = false;
            CboCp.SelectedIndexChanged += CboCp_SelectedIndexChanged;
            LoadDpByCp();
            
            // Cập nhật trạng thái các nút dựa trên quyền hạn
            UpdateButtonsByPermission();
        }
        
        // Phương thức cập nhật trạng thái các nút dựa trên quyền
        private void UpdateButtonsByPermission()
        {
            if (_right == 0) // Lock Function
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else if (_right == 1) // View Only
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
            else // Full Function (2)
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
        }

        void ShowHideControls(bool t)
        {
            btnAdd.Visible = t;
            btnEdit.Visible = t;
            btnDelete.Visible = t;
            btnSave.Visible = !t;
            btnCancel.Visible = !t;

        }
        void _enable(bool t)
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
        void ResetFields()
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
            gvList.AutoGenerateColumns = false;
            gvList.DataSource = _department.getAll(CboCp.SelectedValue.ToString());
            gvList.ReadOnly = true;
            gvList.ClearSelection();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to add records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _add = true;
            _enable(true);
            ResetFields();
            ShowHideControls(false);
            txtId.Enabled = true;
        }

        public void Alert(string msg, formNoti.enmType type)
        {
            formNoti frm = new formNoti();
            frm.showAlert(msg, type);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to edit records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (gvList.SelectedRows.Count == 0)
            {
                this.Alert("Select a record!", formNoti.enmType.error);
                return;
            }

            _add = false;
            _enable(true);
            ShowHideControls(false);
            txtId.Enabled = false;
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to delete records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (gvList.SelectedRows.Count > 0)
            {
                string departmentID = gvList.SelectedRows[0].Cells["DepartmentID"].Value?.ToString();

                if (string.IsNullOrEmpty(departmentID))
                {
                    this.Alert("Can not find Department", formNoti.enmType.error);
                    return;
                }

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _department.delete(departmentID);
                    LoadDpByCp();
                    this.Alert("Successfully", formNoti.enmType.success);
                }
            }
            else
            {
                this.Alert("Select a record!", formNoti.enmType.error);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("Department ID cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtId.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Department Name cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                // If Warehouse is checked, Symbol is required
                if (chkWarehouse.Checked && string.IsNullOrWhiteSpace(txtSymbol.Text))
                {
                    MessageBox.Show("Symbol is required for Warehouse departments!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSymbol.Focus();
                    return;
                }

                if (_add)
                {
                    // Check if department ID already exists
                    if (_department.getItem(txtId.Text) != null)
                    {
                        MessageBox.Show("Department ID already exists!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtId.Focus();
                        return;
                    }

                    tb_Department dp = new tb_Department()
                    {
                        DepartmentID = txtId.Text,
                        CompanyID = CboCp.SelectedValue.ToString(),
                        DepartmentName = txtName.Text,
                        DepartmentPhone = txtPhone.Text,
                        DepartmentAddress = txtAddress.Text,
                        DepartmentFax = txtFax.Text,
                        DepartmentEmail = txtEmail.Text,
                        Warehouse = chkWarehouse.Checked,
                        Symbol = txtSymbol.Text,
                        IsDisabled = chkDisable.Checked,
                        CreatedDate = DateTime.Now,  // Gán ngày tạo mới
                        DeletedDate = null,         // Mặc định NULL
                        UpdatedDate = null,
                        RestoredDate = null
                    };
                    _department.add(dp);
                }
                else
                {
                    tb_Department dp = _department.getItem(_departmentid);
                    bool wasDisabled = dp.IsDisabled ?? false;
                    dp.CompanyID = CboCp.SelectedValue.ToString();
                    dp.DepartmentName = txtName.Text;
                    dp.DepartmentPhone = txtPhone.Text;
                    dp.DepartmentAddress = txtAddress.Text;
                    dp.DepartmentFax = txtFax.Text;
                    dp.DepartmentEmail = txtEmail.Text;
                    dp.Warehouse = chkWarehouse.Checked;
                    dp.Symbol = txtSymbol.Text;
                    dp.IsDisabled = chkDisable.Checked;
                    dp.UpdatedDate = DateTime.Now;
                    if (wasDisabled && !chkDisable.Checked)
                    {
                        dp.RestoredDate = DateTime.Now;
                    }
                    _department.update(dp);
                }
                _add = false;
                LoadDpByCp();
                _enable(false);
                ShowHideControls(true);
                txtId.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ResetFields();
            ShowHideControls(true);
            txtId.Enabled = false;
            LoadDpByCp();
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

    }
}
