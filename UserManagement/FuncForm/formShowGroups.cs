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
using BusinessLayer.Utils;
using DataLayer;
using BusinessLayer;

namespace UserManagement.FuncForm
{
    public partial class formShowGroups : MaterialForm
    {
        public formShowGroups()
        {
            InitializeComponent();
        }

        public string _companyID;
        public string _departmentID;
        public int _userID;
        SYS_GROUP _sysGroup;
        VIEW_USER_IN_GROUPS _vUserInGroup;

        formUser objUser = (formUser)Application.OpenForms["formUser"];

        private void formShowGroups_Load(object sender, EventArgs e)
        {
            _sysGroup = new SYS_GROUP();
            _vUserInGroup = new VIEW_USER_IN_GROUPS();
            loadGroup();
        }
        void loadGroup()
        {
            gvGroup.DataSource = _vUserInGroup.getGroupsByDonVi(_companyID, _departmentID);
            gvGroup.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvGroup.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = gvGroup.SelectedRows[0];
                    int groupId = Convert.ToInt32(selectedRow.Cells["UserID"].Value);
                    
                    if (_vUserInGroup.checkGroupsByUser(_userID, groupId))
                    {
                        MessageBox.Show("Người dùng đã tồn tại trong nhóm này. Vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    tb_SYS_GROUP gr = new tb_SYS_GROUP();
                    gr.Groups = groupId;
                    gr.Member = _userID;
                    _sysGroup.add(gr);
                    objUser.loadGroupByUser(_userID);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn nhóm để thêm người dùng vào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }
    }
}
