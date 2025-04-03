using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialSkin.Controls;
using System.Windows.Forms;
using BusinessLayer.Utils;
using BusinessLayer;

namespace UserManagement.FuncForm
{
    public partial class formSysFuncRight : MaterialForm
    {
        public formSysFuncRight()
        {
            InitializeComponent();
        }
        public int _userID;
        public string _cmpID;
        public string _dpID;
        SYS_USER _sysUser;
        SYS_RIGHT _sysRight;
        private void formSysFuncRight_Load(object sender, EventArgs e)
        {
            gvUsers.AutoGenerateColumns = false;
            gvFunction.AutoGenerateColumns = false;
            _sysUser = new SYS_USER();
            _sysRight = new SYS_RIGHT();
            loadUsers();
            loadFuncByUser();
        }
        void loadUsers()
        {
            if (_cmpID == null && _dpID == null)
            {
                gvUsers.DataSource = _sysUser.getUserByDpFunc("DEMO01", "~");
                gvUsers.ReadOnly = true;
            }
            else
            {
                gvUsers.DataSource = _sysUser.getUserByDpFunc(_cmpID, _dpID);
                gvUsers.ReadOnly = true;
            }
        }

        private void gvUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string columnName = gvUsers.Columns[e.ColumnIndex].Name;

                if (columnName == "IsGroup")
                {
                    object cellValue = gvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    if (cellValue != null && bool.TryParse(cellValue.ToString(), out bool isGroup))
                    {
                        Image img;   
                        if (isGroup)
                        {
                            img = imageList1.Images["groups"];
                        }
                        else
                        {
                            img = imageList1.Images["user"];
                        }

                        e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                        int imgX = e.CellBounds.X + (e.CellBounds.Width - img.Width) / 2;
                        int imgY = e.CellBounds.Y + (e.CellBounds.Height - img.Height) / 2;

                        e.Graphics.DrawImage(img, new Point(imgX, imgY));

                        e.Handled = true;
                    }
                }
            }
        }
        void loadFuncByUser()
        {
            VIEW_FUNC_SYS_RIGHT _vFuncRight = new VIEW_FUNC_SYS_RIGHT();
            gvFunction.DataSource = _vFuncRight.getFuncByUser(_userID);
            gvFunction.ReadOnly = true;
            for (int i = 0; i < gvUsers.RowCount; i++)
            {
                if (int.Parse(gvUsers.Rows[i].Cells["UserID"].Value.ToString()) == _userID)
                {
                    gvUsers.ClearSelection();
                    gvUsers.Rows[i].Selected = true;
                    gvUsers.CurrentCell = gvUsers.Rows[i].Cells[0];
                }
            }
            gvFunction.ClearSelection();
        }
    }
}
