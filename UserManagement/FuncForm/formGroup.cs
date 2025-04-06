using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using BusinessLayer;
using DataLayer;
using BusinessLayer.Utils;

namespace UserManagement.FuncForm
{
    public partial class formGroup : MaterialForm
    {
        public formGroup()
        {
            InitializeComponent();
        }
        formMain objMain=(formMain)Application.OpenForms["formMain"];
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

        private void formGroup_Load(object sender, EventArgs e)
        {

            _sysUser = new SYS_USER();
            if (!_add)
            {
                var user = _sysUser.getItem(_userID);
                txtGroup.Text = user.UserName;
                _companyID = user.CompanyID;
                _departmentID = user.DepartmentID;
                txtDescrip.Text = user.FullName;
                txtGroup.ReadOnly = true;
                loadUserInGroup(_userID);
            }
            else
            {
                txtDescrip.Text = "";
                txtGroup.Text = "";
            }
        }
        public void loadUserInGroup(int idGroup)
        {
            _vUserInGroup = new VIEW_USER_IN_GROUPS();
            gvMember.DataSource = _vUserInGroup.getUserInGroup(_departmentID, _companyID, idGroup);
            gvMember.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtGroup.Text.Trim() == "")
            {
                bool checkedUser = _sysUser.checkUserExist(_companyID, _departmentID, txtGroup.Text.Trim());
                if (checkedUser)
                {
                    MessageBox.Show("Please enter group name", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroup.SelectAll();
                    txtGroup.Focus();
                    return;
                }
            }
            saveData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void saveData()
        {
            if (_add)
            {
                bool checkedUser = _sysUser.checkUserExist(_companyID, _departmentID, txtGroup.Text.Trim());
                if (checkedUser)
                {
                    MessageBox.Show("Group already exists please check again!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroup.SelectAll();
                    txtGroup.Focus();
                    return;
                }
                _user = new tb_SYS_USER();
                _user.UserName = txtGroup.Text.Trim();
                _user.FullName = txtDescrip.Text.Trim();
                _user.IsGroup = true;
                _user.IsDisable = false;
                _user.CompanyID = _companyID;
                _user.DepartmentID = _departmentID;
                _sysUser.add(_user);
                
                // Lấy ID của nhóm vừa thêm
                _userID = _sysUser.getItem(_companyID, _departmentID, txtGroup.Text.Trim()).UserID;
                _add = false;
                
                // Reload danh sách thành viên trong nhóm
                loadUserInGroup(_userID);
                
                MessageBox.Show("Group added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _user = _sysUser.getItem(_userID);
                _user.FullName = txtDescrip.Text;
                _sysUser.update(_user);
                
                // Reload danh sách thành viên trong nhóm
                loadUserInGroup(_userID);
                
                MessageBox.Show("Group updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Reload danh sách user trong form chính
            objMain.LoadUser(_companyID, _departmentID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formShowMembers frm = new formShowMembers();
            frm._idGroup = _userID;
            frm._companyID = _companyID;
            frm._departmentID = _departmentID;
            frm.ShowDialog();
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvMember.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = gvMember.SelectedRows[0];
                    int userId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                    
                    _sysGroup = new SYS_GROUP();
                    _sysGroup.delGroup(userId, _userID);
                    
                    loadUserInGroup(_userID);
                    MessageBox.Show("Member removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please select a member to remove from the group.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
