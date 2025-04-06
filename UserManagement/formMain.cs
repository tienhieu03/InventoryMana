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
using UserManagement.FuncForm;
using USERMANAGEMENT.MyComments;

namespace UserManagement
{
    public partial class formMain : MaterialForm
    {
        public formMain()
        {
            InitializeComponent();
        }
        MyTreeViewCombo _treeView;
        COMPANY _company;
        DEPARTMENT _department;
        SYS_USER _sysUser;
        bool _isRoot;
        string _companyID;
        string _departmentID;
        private void formMain_Load(object sender, EventArgs e)
        {
            // Đảm bảo DataGridView không tự động tạo cột ngay từ khi form được tải
            gvUser.AutoGenerateColumns = false;
            
            _company = new COMPANY();
            _department = new DEPARTMENT();
            _sysUser = new SYS_USER();
            _isRoot = true;
            LoadTreeView();
            LoadUser(_companyID,"~");
        }
        public void LoadUser(string cpid, string dpid)
        {
            _sysUser = new SYS_USER();
            
            // Đảm bảo DataGridView không tự động tạo cột
            gvUser.AutoGenerateColumns = false;
            
            // Gán dữ liệu
            gvUser.DataSource = _sysUser.getUserByDp(cpid, dpid);
            gvUser.ReadOnly = true;
        }
        void LoadTreeView()
        {
            try
            {
                _treeView = new MyTreeViewCombo(pnlDepartment.Width, 300);
                _treeView.Font = new Font("Arial", 10, FontStyle.Bold);
                var lstCp = _company.getAll();
                
                if (lstCp == null || lstCp.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu công ty.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                foreach (var item in lstCp)
                {
                    TreeNode parentNode = new TreeNode();
                    parentNode.Text = item.CompanyID + " - " + item.CompanyName;
                    parentNode.Tag = item.CompanyID;
                    parentNode.Name = item.CompanyID;
                    _treeView.TreeView.Nodes.Add(parentNode);
                    
                    var departmentList = _department.getAll(item.CompanyID);
                    if (departmentList != null && departmentList.Count > 0)
                    {
                        foreach (var dv in departmentList)
                        {
                            TreeNode childNode = new TreeNode();
                            childNode.Text = dv.DepartmentID + " - " + dv.DepartmentName;
                            childNode.Tag = dv.CompanyID + "." + dv.DepartmentID;
                            childNode.Name = dv.CompanyID + "." + dv.DepartmentID;
                            _treeView.TreeView.Nodes[parentNode.Name].Nodes.Add(childNode);
                        }
                    }
                }
                
                _treeView.TreeView.ExpandAll();
                
                pnlDepartment.Controls.Clear();
                
                pnlDepartment.Controls.Add(_treeView);
                _treeView.Width = pnlDepartment.Width;
                _treeView.Height = pnlDepartment.Height;
                _treeView.TreeView.AfterSelect += TreeView_AfterSelect;
                _treeView.TreeView.Click += TreeView_Click;
                
                Console.WriteLine($"Loaded {lstCp.Count} companies into TreeView");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error in LoadTreeView: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void TreeView_Click(object sender, EventArgs e)
        {
            _treeView.dropDown.Focus();
            _treeView.SelectAll();
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _treeView.Text = _treeView.TreeView.SelectedNode.Text;
            if (_treeView.TreeView.SelectedNode.Parent == null)
            {
                _isRoot = true;
                _companyID = _treeView.TreeView.SelectedNode.Tag.ToString();
                _departmentID = "~";
            }
            else
            {
                _isRoot = false;
                _companyID = _treeView.TreeView.SelectedNode.Name.Substring(0, 4);
                _departmentID = _treeView.TreeView.SelectedNode.Name.Substring(5);
            }
            LoadUser(_companyID, _departmentID);
            _treeView.dropDown.Close();
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            if (_treeView.Text == "")
            {
                MessageBox.Show("Please choose department.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            formGroup frm = new formGroup();
            frm._add = true;
            frm._companyID = _companyID;
            frm._departmentID = _departmentID;
            frm.ShowDialog();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            if (_treeView.Text == "")
            {
                MessageBox.Show("Please choose department.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            formUser frm = new formUser();
            frm._add = true;
            frm._companyID = _companyID;
            frm._departmentID = _departmentID;
            frm.ShowDialog();
        }

        private void btnUpdateInfor_Click(object sender, EventArgs e)
        {
            if (gvUser.RowCount > 0 && gvUser.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvUser.SelectedRows[0];
                bool isGroup = Convert.ToBoolean(selectedRow.Cells["IsGroup"].Value);
                int userId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                
                if (isGroup)
                {
                    formGroup frm = new formGroup();
                    frm._add = false;
                    frm._userID = userId;
                    frm.ShowDialog();
                }
                else
                {
                    formUser frm = new formUser();
                    frm._add = false;
                    frm._userID = userId;
                    frm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng cần cập nhật thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnFuntion_Click(object sender, EventArgs e)
        {
            if (gvUser.RowCount > 0 && gvUser.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvUser.SelectedRows[0];

                formFuncPermission frm = new formFuncPermission();
                frm._userID = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                frm._cmpID = _companyID;
                frm._dpID = _departmentID;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng cần phân quyền chức năng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (gvUser.RowCount > 0 && gvUser.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvUser.SelectedRows[0];

                formReportPer frm = new formReportPer();
                frm._userID = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                frm._cmpID = _companyID;
                frm._dpID = _departmentID;
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng cần phân quyền báo cáo", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvUser_DoubleClick(object sender, EventArgs e)
        {
            if (gvUser.RowCount > 0 && gvUser.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvUser.SelectedRows[0];
                
                // Kiểm tra xem nếu là nhóm
                bool isGroup = Convert.ToBoolean(selectedRow.Cells["IsGroup"].Value);
                
                if (isGroup)
                {
                    formGroup frm = new formGroup();
                    frm._add = false;
                    frm._userID = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                    frm.ShowDialog();
                }
                else
                {
                    formUser frm = new formUser();
                    frm._add = false;
                    frm._userID = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                    frm.ShowDialog();
                }
            }
        }
    }
}
