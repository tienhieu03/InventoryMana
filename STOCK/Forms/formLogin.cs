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
using MATERIAL.MyFunctions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DataLayer;

namespace STOCK.Forms
{
    public partial class formLogin : MaterialForm
    {
        public formLogin()
        {
            InitializeComponent();
        }

        SYS_PARAM _sysParam;
        SYS_USER _sysUser;
        DEPARTMENT _dp;

        private void formLogin_Load(object sender, EventArgs e)
        {
            _sysUser = new SYS_USER();
            _dp = new DEPARTMENT();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open("sysparam.ini", FileMode.Open, FileAccess.Read);
            _sysParam = (SYS_PARAM)bf.Deserialize(fs);
            fs.Close();
            myFunctions._compid = _sysParam.companyId;
            myFunctions._dpid = _sysParam.departmentId;
            if (myFunctions._dpid == "~")
                myFunctions._dpname = "";
            else
                myFunctions._dpname = _dp.getItem(myFunctions._dpid).DepartmentName;
        }


        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Enter User Name", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool us = _sysUser.checkUserExist(_sysParam.companyId, _sysParam.departmentId, txtUsername.Text.Trim());
            if (!us)
            {
                MessageBox.Show("User Name does not exist.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string pass = Encryptor.Encrypt(txtPassword.Text, "qwertyuiop!@#$", true);
            tb_SYS_USER user = _sysUser.getItem(txtUsername.Text.Trim(), _sysParam.companyId, _sysParam.departmentId);
            if (user.Password.Equals(pass))
            {
                Main frm = new Main(user);
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Incorrect Password", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Login_Click(sender, e);
            }
        }

        private void Exit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Exit_Click(sender, e);
            }
        }
    }
}
