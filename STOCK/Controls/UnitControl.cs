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
    public partial class UnitControl: UserControl
    {
        public UnitControl()
        {
            InitializeComponent();
            _right = 2; // Mặc định là full quyền nếu không truyền vào
        }
        
        public UnitControl(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            _user = user;
            _right = right;
        }
        
        UNIT _unit;
        bool _add;
        int _id;
        tb_SYS_USER _user;
        int _right;
        
        private void UnitControl_Load(object sender, EventArgs e)
        {
            gvList.ClearSelection();
            _add = false;
            _enable(false);
            _unit = new UNIT();
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
            chkDisable.Enabled = t;
        }

        void ResetFields()
        {
            txtId.Text = ""; // Xóa ID khi thêm mới
            txtName.Text = "";
            chkDisable.Checked = false;
        }

        void loadData()
        {
            gvList.AutoGenerateColumns = false;
            gvList.DataSource = _unit.getList();
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

                int unitID = (int)row.Cells["UnitID"].Value;

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _unit.delete(unitID);
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
                tb_Unit ct = new tb_Unit()
                {
                    UnitName = txtName.Text,
                    IsDisabled = chkDisable.Checked,
                    CreatedDate = DateTime.Now,  // Gán ngày tạo mới
                    DeletedDate = null,         // Mặc định NULL
                    UpdatedDate = null,
                    RestoredDate = null
                };
                _unit.add(ct);
            }
            else
            {
                tb_Unit ct = _unit.getItem(_id);
                bool wasDisabled = ct.IsDisabled ?? false;
                ct.UnitName = txtName.Text;
                ct.IsDisabled = chkDisable.Checked;
                ct.UpdatedDate = DateTime.Now;
                if (wasDisabled && !chkDisable.Checked)
                {
                    ct.RestoredDate = DateTime.Now;
                }
                _unit.update(ct);
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

                _id = (int)row.Cells["UnitID"].Value;
                txtId.Text = _id.ToString();
                txtName.Text = row.Cells["UnitName"].Value?.ToString() ?? "";
                chkDisable.Checked = Convert.ToBoolean(row.Cells["IsDisabled"].Value);
            }
        }
    }
}
