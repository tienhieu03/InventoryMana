using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer;
using SharedControls;

namespace STOCK.Controls
{
    public partial class CompanyControl: UserControl
    {
        public CompanyControl()
        {
            InitializeComponent();
            _right = 2;
        }
        
        public CompanyControl(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            _user = user;
            _right = right;
        }

        COMPANY _company;
        bool _add;
        string _id;
        tb_SYS_USER _user;
        int _right;

        private void CompanyControl_Load(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            _company = new COMPANY();
            ShowHideControls(true);
            loadData();
            gvList.ClearSelection();
            
            // Kiểm tra quyền và cập nhật hiển thị các nút tương ứng
            UpdateButtonsByPermission();
        }
        
        // Cập nhật hiển thị các nút dựa trên quyền hạn
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
            else // Full Function
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

        private void _enable(bool t)
        {
            txtId.Enabled = t;
            txtName.Enabled = t;
            txtPhone.Enabled = t;
            txtFax.Enabled = t;
            txtEmail.Enabled = t;
            txtAddress.Enabled = t;
            chkDisable.Enabled = t;
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
        }
        void loadData()
        {
            gvList.AutoGenerateColumns = false;
            gvList.DataSource = _company.getAll();
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
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to edit records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _add = false;
            _enable(true);
            ShowHideControls(false);
        }

        public void Alert(string msg, formNoti.enmType type)
        {
            formNoti frm = new formNoti();
            frm.showAlert(msg, type);
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
                string companyID = gvList.SelectedRows[0].Cells["CompanyID"].Value?.ToString();

                if (string.IsNullOrEmpty(companyID))
                {
                    this.Alert("Nothing Found", formNoti.enmType.error);
                }

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _company.delete(companyID);
                    loadData();
                    this.Alert("Deleted successfully", formNoti.enmType.success);
                }
            }
            else
            {
                this.Alert("Select a record", formNoti.enmType.error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_add)
            {
                tb_Company ct = new tb_Company() {
                    CompanyID = txtId.Text,
                    CompanyName = txtName.Text,
                    CompanyEmail = txtEmail.Text,
                    CompanyPhone = txtPhone.Text,
                    CompanyFax = txtFax.Text,
                    CompanyAddress = txtAddress.Text,
                    IsDisabled = chkDisable.Checked,
                    CreatedBy = _user.UserID,
                    CreatedDate = DateTime.Now,
                    DeletedDate = null,
                    UpdatedDate = null,
                    RestoredDate = null
                };
                _company.add(ct);
            }
            else
            {
                tb_Company ct = _company.getItem(_id);
                bool wasDisabled = ct.IsDisabled ?? false;
                ct.CompanyID = txtId.Text;
                ct.CompanyName = txtName.Text;
                ct.CompanyPhone = txtPhone.Text;
                ct.CompanyAddress = txtAddress.Text;
                ct.CompanyFax = txtFax.Text;
                ct.CompanyEmail = txtEmail.Text;
                ct.IsDisabled = chkDisable.Checked;
                ct.UpdatedDate = DateTime.Now;
                if (wasDisabled && !chkDisable.Checked)
                {
                    ct.RestoredDate = DateTime.Now;
                }
                _company.update(ct);
            }
            _add = false;
            _enable(false);
            loadData();
            ShowHideControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ShowHideControls(true);
            txtId.Enabled = false;
        }

        private void gvList_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];

                _id = row.Cells["CompanyID"].Value?.ToString();

                txtId.Text = row.Cells["CompanyID"].Value?.ToString() ?? "";
                txtName.Text = row.Cells["CompanyName"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["CompanyPhone"].Value?.ToString() ?? "";
                txtFax.Text = row.Cells["CompanyFax"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["CompanyEmail"].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells["CompanyAddress"].Value?.ToString() ?? "";

                // Kiểm tra null trước khi chuyển đổi sang boolean
                object isDisabledValue = row.Cells["IsDisabled"].Value;
                chkDisable.Checked = (isDisabledValue != null) && Convert.ToBoolean(isDisabledValue);
            }
        }

    }
}
