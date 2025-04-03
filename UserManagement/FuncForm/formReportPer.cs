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
using BusinessLayer.Utils;
using MaterialSkin.Controls;

namespace UserManagement.FuncForm
{
    public partial class formReportPer : MaterialForm
    {
        public formReportPer()
        {
            InitializeComponent();
        }
        public int _userID;
        public string _cmpID;
        public string _dpID;
        SYS_USER _sysUser;
        SYS_RIGHT_REP _sysRightRep;
        private void formReportPer_Load(object sender, EventArgs e)
        {
            dgvFunction.AutoGenerateColumns = false;
            dgvUser.AutoGenerateColumns = false;
            _sysUser = new SYS_USER();
            _sysRightRep = new SYS_RIGHT_REP();
            loadUsers();
            loadRepByUser();
        }
        void loadUsers()
        {
            if (_cmpID == null && _dpID == null)
            {
                dgvUser.DataSource = _sysUser.getUserByDpFunc("DEMO01", "~");
                dgvUser.ReadOnly = true;
            }
            else
            {
                dgvUser.DataSource = _sysUser.getUserByDpFunc(_cmpID, _dpID);
                dgvUser.ReadOnly = true;
            }
        }
        void loadRepByUser()
        {
            VIEW_REP_SYS_RIGHT_REP _vFuncRight = new VIEW_REP_SYS_RIGHT_REP();
            dgvFunction.DataSource = _vFuncRight.getRepByUser(_userID);
            dgvFunction.ReadOnly = true;
            for (int i = 0; i < dgvUser.RowCount; i++)
            {
                if (int.Parse(dgvUser.Rows[i].Cells["UserID"].Value.ToString()) == _userID)
                {
                    dgvUser.ClearSelection();
                    dgvUser.Rows[i].Selected = true;
                    
                    // Tìm cột visible đầu tiên để tránh lỗi "Current cell cannot be set to an invisible cell"
                    DataGridViewCell visibleCell = null;
                    foreach (DataGridViewCell cell in dgvUser.Rows[i].Cells)
                    {
                        if (cell.Visible && dgvUser.Columns[cell.ColumnIndex].Visible)
                        {
                            visibleCell = cell;
                            break;
                        }
                    }
                    
                    if (visibleCell != null)
                    {
                        dgvUser.CurrentCell = visibleCell;
                    }
                }
            }
            dgvFunction.ClearSelection();
        }

        private void mnLockFunction_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    int repCode = Convert.ToInt32(dgvFunction.Rows[i].Cells["REP_CODE"].Value);
                    _sysRightRep.update(_userID, repCode, false);
                }
            }
            // Tải lại toàn bộ dữ liệu để hiển thị đồng bộ với DB
            loadRepByUser();
        }

        private void mnFullFunction_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    int repCode = Convert.ToInt32(dgvFunction.Rows[i].Cells["REP_CODE"].Value);
                    _sysRightRep.update(_userID, repCode, true);
                }
            }
            // Tải lại toàn bộ dữ liệu để hiển thị đồng bộ với DB
            loadRepByUser();
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

        private void dgvUser_Click(object sender, EventArgs e)
        {
            if (dgvUser.CurrentRow != null)
            {
                _userID = int.Parse(dgvUser.CurrentRow.Cells["UserID"].Value.ToString());
                loadRepByUser();
            }
        }
    }
}
