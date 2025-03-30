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
using DataLayer;
using MaterialSkin.Controls;

namespace UserManagement.FuncForm
{
    public partial class formUser : MaterialForm
    {
        public formUser()
        {
            InitializeComponent();
        }

        formMain objMain = (formMain)Application.OpenForms["formMain"];
        public string _companyID;
        public string _departmentID;
        public int _userID;
        public string _username;
        public string _fullname;
        public bool _add;
        SYS_USER _sysUser;
        tb_SYS_USER _user;
        SYS_GROUP _sysGroup;
        VIEW_USER_IN_GROUPS _vUserInGroup;
        private void formUser_Load(object sender, EventArgs e)
        {
            _sysUser = new SYS_USER();
            _sysGroup = new SYS_GROUP();
            if (!_add)
            {
                var user = _sysUser.getItem(_userID);
                txtUsername.Text = user.UserName;
                _companyID = user.CompanyID;
                _departmentID = user.DepartmentID;
                txtFullName.Text = user.FullName;
                chkDisable.Checked = user.IsDisable.Value;
                txtUsername.ReadOnly = true;
                
                try
                {
                    // Giải mã mật khẩu
                    txtPass.Text = Encryptor.Decrypt(user.Password, "qwertyuiop!@#$", true);
                    txtRepass.Text = Encryptor.Decrypt(user.Password, "qwertyuiop!@#$", true);
                }
                catch (FormatException ex)
                {
                    // Xử lý lỗi khi chuỗi mật khẩu không phải định dạng Base64 hợp lệ
                    Console.WriteLine($"Lỗi giải mã mật khẩu: {ex.Message}");
                    
                    // Hiển thị mật khẩu trống để người dùng có thể nhập mật khẩu mới
                    txtPass.Text = "";
                    txtRepass.Text = "";
                    
                    MessageBox.Show("Không thể giải mã mật khẩu. Vui lòng nhập mật khẩu mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                loadGroupByUser(_userID);
            }
            else
            {
                txtFullName.Text = "";
                txtPass.Text = "";
                txtRepass.Text = "";
                chkDisable.Checked = false;
            }
        }

        public void loadGroupByUser(int idUser)
        {
            _vUserInGroup = new VIEW_USER_IN_GROUPS();

            // Đảm bảo không tự động tạo cột
            gvMember.AutoGenerateColumns = false;

            // Gán dữ liệu
            gvMember.DataSource = _vUserInGroup.getGroupsByUser(_companyID, _departmentID, idUser);
            gvMember.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Chưa nhập tên người dùng. Tên người dùng nhập không dấu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.SelectAll();
                txtUsername.Focus();
                return;
            }
            if (!txtPass.Text.Equals(txtRepass.Text))//Ngài hãy lãnh đạo chúng toioke em
            {
                MessageBox.Show("Mật khẩu không trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.SelectAll();
                txtUsername.Focus();
                return;
            }
            saveData();
        }

        void saveData()
        {
            if (_add)
            {
                bool checkedUser = _sysUser.checkUserExist(_companyID, _departmentID, txtUsername.Text.Trim());
                if (checkedUser)
                {
                    MessageBox.Show("Nhóm đã tồn tại. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.SelectAll();
                    txtUsername.Focus();
                    return;
                }
                _user = new tb_SYS_USER();
                _user.UserName = txtUsername.Text.Trim();
                _user.FullName = txtFullName.Text;
                _user.Password = Encryptor.Encrypt(txtPass.Text.Trim(), "qwertyuiop!@#$", true);
                _user.IsGroup = false;
                _user.IsDisable = false;
                _user.CompanyID = _companyID;
                _user.DepartmentID = _departmentID;
                _sysUser.add(_user);
            }
            else
            {
                _user = _sysUser.getItem(_userID);
                _user.FullName = txtFullName.Text;
                _user.Password = Encryptor.Encrypt(txtPass.Text.Trim(), "qwertyuiop!@#$", true);
                _user.IsGroup = false;
                _user.IsDisable = chkDisable.Checked;
                _user.CompanyID = _companyID;
                _user.DepartmentID = _departmentID;
                _sysUser.update(_user);
            }
            //objMain.loadUser(_companyID, _departmentID);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formShowGroups frm = new formShowGroups();
            frm._userID = _userID;
            frm._companyID = _companyID;
            frm._departmentID = _departmentID;
            frm.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (gvMember.RowCount > 0 && gvMember.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvMember.SelectedRows[0];
                int idUser = Convert.ToInt32(selectedRow.Cells["UserID"].Value);

                _sysGroup.delGroup(_userID, idUser);
                loadGroupByUser(_userID);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thành viên cần xóa khỏi nhóm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
