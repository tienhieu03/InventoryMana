using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer.Utils;
using BusinessLayer;

namespace UserManagement.UserControls
{
    public partial class UserPermissionControl : UserControl
    {

        public int UserID { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }

        SYS_RIGHT _sysRight;
        SYS_USER _sysUser;
        VIEW_FUNC_SYS_RIGHT _vFuncRight;
        public UserPermissionControl()
        {
            InitializeComponent();
            _sysRight = new SYS_RIGHT();
            _vFuncRight = new VIEW_FUNC_SYS_RIGHT();
        }

        public void LoadPermissions(int userId, string companyId, string departmentId)
        {
            UserID = userId;
            CompanyID = companyId;
            DepartmentID = departmentId;

            // Force a database refresh
            _vFuncRight = new VIEW_FUNC_SYS_RIGHT();

            // Completely clear the current view
            dgvFunction.DataSource = null;
            dgvFunction.Refresh();

            // Reapply the data with fresh data from the database
            dgvFunction.AutoGenerateColumns = false;
            dgvFunction.DataSource = _vFuncRight.getFuncByUser(UserID);
            dgvFunction.ReadOnly = true;
            dgvFunction.ClearSelection();
            dgvFunction.Refresh();

            // Display user name in the title label
            SYS_USER sysUser = new SYS_USER();
            var user = sysUser.getItem(UserID);
        }

        private void UserPermissionControl_Load(object sender, EventArgs e)
        {
            _sysUser = new SYS_USER();
            _sysRight = new SYS_RIGHT();
            dgvFunction.AutoGenerateColumns = false;

            // Đăng ký sự kiện CellMouseDown để hiển thị context menu khi chuột phải
            dgvFunction.CellMouseDown += dgvFunction_CellMouseDown;
        }

        private void dgvFunction_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy giá trị cột "ISGROUP"
                object cellValue = dgvFunction.Rows[e.RowIndex].Cells["fIsGroup"].Value;

                // Kiểm tra nếu giá trị là true
                if (cellValue != null && bool.TryParse(cellValue.ToString(), out bool isGroup) && isGroup)
                {
                    // Thay đổi màu nền, màu chữ và font cho hàng
                    dgvFunction.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                    dgvFunction.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                    dgvFunction.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Constantia", 11, FontStyle.Bold);
                }
            }
        }

        private void mnLockFunction_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    string funcCode = dgvFunction.Rows[i].Cells["FUNC_CODE"].Value.ToString();
                    _sysRight.update(UserID, funcCode, 0);
                }
            }
            LoadPermissions(UserID, CompanyID, DepartmentID);
        }

        private void mnViewOnly_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    string funcCode = dgvFunction.Rows[i].Cells["FUNC_CODE"].Value.ToString();
                    _sysRight.update(UserID, funcCode, 1);
                }
            }
            LoadPermissions(UserID, CompanyID, DepartmentID);
        }

        private void mnFullFunction_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    string funcCode = dgvFunction.Rows[i].Cells["FUNC_CODE"].Value.ToString();
                    _sysRight.update(UserID, funcCode, 2);
                }
            }
            LoadPermissions(UserID, CompanyID, DepartmentID);
        }
        
        /// <summary>
        /// Reloads permission data for the current user
        /// </summary>
        public void RefreshPermissions()
        {
            if (UserID > 0)
            {
                // Reload the function permissions grid
                dgvFunction.DataSource = null;
                dgvFunction.AutoGenerateColumns = false;
                dgvFunction.DataSource = _vFuncRight.getFuncByUser(UserID);
                dgvFunction.ClearSelection();
                dgvFunction.Refresh();
            }
        }

        private void mnSelectAllChildren_Click(object sender, EventArgs e)
        {
            // Lấy dòng hiện tại được chọn
            if (dgvFunction.SelectedRows.Count == 0)
                return;

            // Kiểm tra xem dòng được chọn có phải là nhóm không
            int currentRow = dgvFunction.SelectedRows[0].Index;
            bool isGroup = false;
            if (dgvFunction.Rows[currentRow].Cells["fIsGroup"].Value != null)
            {
                bool.TryParse(dgvFunction.Rows[currentRow].Cells["fIsGroup"].Value.ToString(), out isGroup);
            }

            if (isGroup)
            {
                // Lấy tên của nhóm chức năng
                string groupName = dgvFunction.Rows[currentRow].Cells["FUNC_CODE"].Value.ToString();

                // Chọn tất cả các chức năng con của nhóm
                for (int i = 0; i < dgvFunction.RowCount; i++)
                {
                    // Bỏ qua nếu là nhóm
                    if (dgvFunction.Rows[i].Cells["fIsGroup"].Value != null &&
                        bool.TryParse(dgvFunction.Rows[i].Cells["fIsGroup"].Value.ToString(), out bool rowIsGroup) &&
                        rowIsGroup)
                        continue;

                    // Lấy thông tin về chức năng cha của hàng hiện tại
                    if (dgvFunction.Rows[i].Cells["Parent"] != null &&
                        dgvFunction.Rows[i].Cells["Parent"].Value != null &&
                        dgvFunction.Rows[i].Cells["Parent"].Value.ToString() == groupName)
                    {
                        dgvFunction.Rows[i].Selected = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nhóm chức năng để thực hiện thao tác này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvFunction_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Kiểm tra nếu là chuột phải và nhấn vào hàng hợp lệ (không phải header hoặc vùng trống)
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                // Kiểm tra xem ô được nhấn có phải là một phần của selection hiện tại hay không
                bool cellAlreadySelected = dgvFunction.Rows[e.RowIndex].Selected;

                // Kiểm tra nếu là nhóm chức năng và nhấn Control
                bool isGroup = false;
                if (dgvFunction.Rows[e.RowIndex].Cells["fIsGroup"].Value != null)
                {
                    bool.TryParse(dgvFunction.Rows[e.RowIndex].Cells["fIsGroup"].Value.ToString(), out isGroup);
                }

                if (isGroup && ModifierKeys.HasFlag(Keys.Control))
                {
                    // Lấy tên của nhóm chức năng
                    string groupName = dgvFunction.Rows[e.RowIndex].Cells["FUNC_CODE"].Value.ToString();

                    // Chọn tất cả các chức năng con của nhóm
                    for (int i = 0; i < dgvFunction.RowCount; i++)
                    {
                        // Bỏ qua nếu là nhóm
                        if (dgvFunction.Rows[i].Cells["fIsGroup"].Value != null &&
                            bool.TryParse(dgvFunction.Rows[i].Cells["fIsGroup"].Value.ToString(), out bool rowIsGroup) &&
                            rowIsGroup)
                            continue;

                        // Lấy thông tin về chức năng cha của hàng hiện tại
                        if (dgvFunction.Rows[i].Cells["Parent"] != null &&
                            dgvFunction.Rows[i].Cells["Parent"].Value != null &&
                            dgvFunction.Rows[i].Cells["Parent"].Value.ToString() == groupName)
                        {
                            dgvFunction.Rows[i].Selected = true;
                        }
                    }
                }
                // Nếu ô chưa được chọn, thì chỉ chọn hàng được nhấn
                else if (!cellAlreadySelected)
                {
                    if (!ModifierKeys.HasFlag(Keys.Control) && !ModifierKeys.HasFlag(Keys.Shift))
                    {
                        dgvFunction.ClearSelection();
                    }
                    dgvFunction.Rows[e.RowIndex].Selected = true;
                }

                // Đặt cell hiện tại là cell được nhấn
                // Tìm cột visible đầu tiên để tránh lỗi "Current cell cannot be set to an invisible cell"
                DataGridViewCell visibleCell = null;
                foreach (DataGridViewCell cell in dgvFunction.Rows[e.RowIndex].Cells)
                {
                    if (cell.Visible && dgvFunction.Columns[cell.ColumnIndex].Visible)
                    {
                        visibleCell = cell;
                        break;
                    }
                }

                if (visibleCell != null)
                {
                    dgvFunction.CurrentCell = visibleCell;
                }

                // Hiển thị contextMenuStrip1 tại vị trí chuột
                contextMenuStrip1.Show(dgvFunction, dgvFunction.PointToClient(Cursor.Position));
            }
        }
    }
}
