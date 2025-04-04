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
    public partial class formShowMembers : MaterialForm
    {
        public formShowMembers()
        {
            InitializeComponent();
        }

        public string _companyID;
        public string _departmentID;
        public int _idGroup;
        SYS_GROUP _sysGroup;
        VIEW_USER_NOTIN_GROUP _vNotGroup;


        formGroup objGroup = (formGroup)Application.OpenForms["formGroup"];
        private void formShowMembers_Load(object sender, EventArgs e)
        {
            _sysGroup = new SYS_GROUP();
            _vNotGroup = new VIEW_USER_NOTIN_GROUP();
            loadUserNotInGroup();
        }

        void loadUserNotInGroup()
        {
            gvMember.AutoGenerateColumns = false;
            gvMember.DataSource = _vNotGroup.getUserNotInGroup(_departmentID, _companyID);
            gvMember.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (gvMember.RowCount > 0 && gvMember.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = gvMember.SelectedRows[0];
                int userId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                
                tb_SYS_GROUP gr = new tb_SYS_GROUP();
                gr.Groups = _idGroup;
                gr.Member = userId;
                _sysGroup.add(gr);
                
                objGroup.loadUserInGroup(_idGroup);
                
                MessageBox.Show("Đã thêm thành viên vào nhóm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thành viên cần thêm vào nhóm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
