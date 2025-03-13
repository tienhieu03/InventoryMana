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
using BusinessLayer.DataModels;

namespace STOCK.Controls
{
    public partial class Product_CategoryControl: UserControl
    {
        public Product_CategoryControl()
        {
            InitializeComponent();
        }
        PRODUCT_CATEGORY _category;
        bool _add;
        int _id;
        private void Product_CategoryControl_Load(object sender, EventArgs e)
        {
            gvList.ClearSelection();
            _add = false;
            _enable(false);
            _category = new PRODUCT_CATEGORY();
            ShowHideControls(true);
            loadData();
            txtId.Enabled = false;
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
            txtName.Enabled = t;
        }

        private void ResetFields()
        {
            txtId.Text = ""; // Xóa ID khi thêm mới
            txtName.Text = "";
        }

        void loadData()
        {
            gvList.AutoGenerateColumns = false;
            gvList.DataSource = _category.getAll();
            gvList.ReadOnly = true;
            gvList.ClearSelection();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _add = true;
            _enable(true);
            ResetFields();
            txtId.Enabled = false;
            ShowHideControls(false);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
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
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];

                int originID = (int)row.Cells["CategoryID"].Value;

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _category.delete(originID);
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
                tb_ProductCategory ct = new tb_ProductCategory()
                {
                    Category = txtName.Text,
                    CreatedDate = DateTime.Now,  // Gán ngày tạo mới
                    UpdatedDate = null,
                };
                _category.add(ct);
            }
            else
            {
                tb_ProductCategory ct = _category.getItem(_id);
                ct.Category = txtName.Text;

                _category.update(ct);
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

                _id = (int)row.Cells["CategoryID"].Value;
                txtId.Text = _id.ToString();
                txtName.Text = row.Cells["Category"].Value?.ToString() ?? "";
            }
        }
    }
}
