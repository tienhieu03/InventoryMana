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
using MaterialSkin.Controls;

namespace UserManagement.FuncForm
{
    public partial class formFuncPermission : MaterialForm
    {
        public formFuncPermission()
        {
            InitializeComponent();
        }
        public int _userID;
        public string _cmpID;
        public string _dpID;
        SYS_USER _sysUser;
        SYS_RIGHT _sysRight;

        private void formFuncPermission_Load(object sender, EventArgs e)
        {
            _sysUser = new SYS_USER();
            _sysRight = new SYS_RIGHT();
            dgvUser.AutoGenerateColumns = false;
            dgvFunction.AutoGenerateColumns = false;
            
            // Đăng ký sự kiện CellMouseDown để hiển thị context menu khi chuột phải
            dgvFunction.CellMouseDown += dgvFunction_CellMouseDown;
            
            loadUsers();
            loadFuncByUser();
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
        void loadFuncByUser()
        {
            VIEW_FUNC_SYS_RIGHT _vFuncRight = new VIEW_FUNC_SYS_RIGHT();
            dgvFunction.DataSource = _vFuncRight.getFuncByUser(_userID);
            dgvFunction.ReadOnly = true;
            for (int i = 0; i < dgvUser.RowCount; i++)
            {
                if (int.Parse(dgvUser.Rows[i].Cells["UserID"].Value.ToString()) == _userID)
                {
                    dgvUser.ClearSelection();
                    dgvUser.Rows[i].Selected = true;
                    dgvUser.CurrentCell = dgvUser.Rows[i].Cells[0];
                }
            }
            dgvFunction.ClearSelection();
        }

        private void dgvUser_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Kiểm tra nếu không phải tiêu đề cột
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Lấy tên cột
                string columnName = dgvUser.Columns[e.ColumnIndex].Name;

                // Kiểm tra cột "IsGroup"
                if (columnName == "IsGroup")
                {
                    // Lấy giá trị của ô
                    object cellValue = dgvUser.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    if (cellValue != null && bool.TryParse(cellValue.ToString(), out bool isGroup))
                    {
                        Image img;

                        if (isGroup)
                        {
                            img = imageList1.Images[0];  // Ảnh đầu tiên trong ImageList
                        }
                        else
                        {
                            img = imageList1.Images[1];
                        }

                        // Vẽ ảnh vào ô
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                        // Tính toán vị trí để ảnh nằm ở giữa ô
                        int imgX = e.CellBounds.X + (e.CellBounds.Width - img.Width) / 2;
                        int imgY = e.CellBounds.Y + (e.CellBounds.Height - img.Height) / 2;

                        // Vẽ ảnh vào ô
                        e.Graphics.DrawImage(img, new Point(imgX, imgY));

                        // Đánh dấu đã vẽ xong, không cần vẽ nội dung mặc định
                        e.Handled = true;
                    }
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
                    _sysRight.update(_userID, funcCode, 0);
                }
            }
            loadFuncByUser();
        }

        private void mnViewOnly_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    string funcCode = dgvFunction.Rows[i].Cells["FUNC_CODE"].Value.ToString();
                    _sysRight.update(_userID, funcCode, 1);
                }
            }
            // Tải lại toàn bộ dữ liệu để hiển thị đồng bộ với DB
            loadFuncByUser();
        }

        private void mnFullFunction_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvFunction.RowCount; i++)
            {
                if (dgvFunction.Rows[i].Selected)
                {
                    string funcCode = dgvFunction.Rows[i].Cells["FUNC_CODE"].Value.ToString();
                    _sysRight.update(_userID, funcCode, 2);
                }
            }
            // Tải lại toàn bộ dữ liệu để hiển thị đồng bộ với DB
            loadFuncByUser();
        }

        private void dgvFunction_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Kiểm tra nếu là hàng hợp lệ (không phải header)
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
                loadFuncByUser();
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
    }
}
