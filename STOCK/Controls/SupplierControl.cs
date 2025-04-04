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

namespace STOCK.Controls
{
    public partial class SupplierControl : UserControl
    {
        public SupplierControl()
        {
            InitializeComponent();
            _right = 2; // Mặc định là full quyền nếu không truyền vào
        }
        
        public SupplierControl(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            _user = user;
            _right = right;
        }

        SUPPLIER _supplier;
        bool _add;
        int _id;
        tb_SYS_USER _user;
        int _right;

        private void SupplierControl_Load(object sender, EventArgs e)
        {
            gvList.ClearSelection();
            _add = false;
            _enable(false);
            _supplier = new SUPPLIER();
            ShowHideControls(true);
            loadData();
            txtId.Enabled = false;
            
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
        }

        void ResetFields()
        {
            txtId.Text = ""; // Xóa ID khi thêm mới
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
            gvList.DataSource = _supplier.getList();
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
            txtId.Enabled = false;
            ShowHideControls(false);
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
                MessageBox.Show("Please select a record to edit!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _add = false;
            _enable(true);
            ShowHideControls(false);
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
                DataGridViewRow row = gvList.SelectedRows[0];

                int supplierID = (int)row.Cells["SupplierID"].Value;

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _supplier.delete(supplierID);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_add)
            {
                tb_Supplier ct = new tb_Supplier()
                {
                    SupplierName = txtName.Text,
                    SupplierEmail = txtEmail.Text,
                    SupplierPhone = txtPhone.Text,
                    SupplierFax = txtFax.Text,
                    SupplierAddress = txtAddress.Text,
                    IsDisabled = chkDisable.Checked,
                    CreatedDate = DateTime.Now,  // Gán ngày tạo mới
                    DeletedDate = null,         // Mặc định NULL
                    UpdatedDate = null,
                    RestoredDate = null
                };
                _supplier.add(ct);
            }
            else
            {
                tb_Supplier ct = _supplier.getItem(_id);
                bool wasDisabled = ct.IsDisabled ?? false;
                ct.SupplierName = txtName.Text;
                ct.SupplierPhone = txtPhone.Text;
                ct.SupplierAddress = txtAddress.Text;
                ct.SupplierFax = txtFax.Text;
                ct.SupplierEmail = txtEmail.Text;
                ct.IsDisabled = chkDisable.Checked;
                ct.UpdatedDate = DateTime.Now;
                if (wasDisabled && !chkDisable.Checked)
                {
                    ct.RestoredDate = DateTime.Now;
                }
                _supplier.update(ct);
            }
            _add = false;
            _enable(false);
            loadData();
            ShowHideControls(true);
            gvList.ClearSelection();
            ResetFields();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ShowHideControls(true);
            gvList.ClearSelection();
        }

        private void gvList_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];

                _id = (int)row.Cells["SupplierID"].Value;
                txtId.Text = _id.ToString();
                txtName.Text = row.Cells["SupplierName"].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells["SupplierPhone"].Value?.ToString() ?? "";
                txtFax.Text = row.Cells["SupplierFax"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["SupplierEmail"].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells["SupplierAddress"].Value?.ToString() ?? "";

                // Kiểm tra null trước khi chuyển đổi sang boolean
                object isDisabledValue = row.Cells["IsDisabled"].Value;
                chkDisable.Checked = (isDisabledValue != DBNull.Value) && Convert.ToBoolean(isDisabledValue);
            }
        }
    }
}
