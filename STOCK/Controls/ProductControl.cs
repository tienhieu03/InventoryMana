using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using BusinessLayer;
using BusinessLayer.DataModels;
using System.Xml.Linq;

namespace STOCK.Controls
{
    public partial class ProductControl: UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
        }

        tb_SYS_USER _user;
        int _right;
        bool _add;
        int _id;
        string _barcode;
        SUPPLIER _supplier;
        UNIT _unit;
        PRODUCT_CATEGORY _category;
        ORIGIN _origin;
        PRODUCT _product;

        private void CompanyControl_Load(object sender, EventArgs e)
        {
            _supplier = new SUPPLIER();
            _unit = new UNIT();
            _category = new PRODUCT_CATEGORY();
            _origin = new ORIGIN();
            _enable(false);
            ShowHideControls(true);
            loadData();
            gvList.ClearSelection();
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

        private void ProductControl_Load(object sender, EventArgs e)
        {
            loadData();
            ShowHideControls(true);
            _enable(false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _add = true;
            _enable(true);
            ResetFields();
            ShowHideControls(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(true);
            ShowHideControls(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                string companyID = gvList.SelectedRows[0].Cells["CompanyID"].Value?.ToString();

                if (string.IsNullOrEmpty(companyID))
                {
                    MessageBox.Show("Can not find Department ID!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _company.delete(companyID);
                    loadData(); // Load lại danh sách sau khi xóa
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
                tb_Company ct = new tb_Company()
                {
                    CompanyID = txtId.Text,
                    CompanyName = txtName.Text,
                    CompanyEmail = txtEmail.Text,
                    CompanyPhone = txtPhone.Text,
                    CompanyFax = txtFax.Text,
                    CompanyAddress = txtAddress.Text,
                    IsDisabled = chkDisable.Checked,
                    CreatedDate = DateTime.Now,  // Gán ngày tạo mới
                    DeletedDate = null,         // Mặc định NULL
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
