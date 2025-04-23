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
using DataLayer;

namespace STOCK.Forms
{
    public partial class formPassword : Form
    {
        public formPassword()
        {
            InitializeComponent();
        }

        public formPassword(tb_SYS_USER user)
        {
            InitializeComponent();
            this._user = user;
        }

        tb_SYS_USER _user;
        SYS_USER _sysUser;

        private void formPassword_Load(object sender, EventArgs e)
        {
            _sysUser = new SYS_USER();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtNewPass.Text.Equals(txtRetype.Text))
            {
                MessageBox.Show("Password does not match", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var us = _sysUser.getItem(_user.UserID);
            if (us.Password == Encryptor.Encrypt(txtCurPass.Text, "qwertyuiop!@#$", true))
            {
                us.Password = Encryptor.Encrypt(txtNewPass.Text, "qwertyuiop!@#$", true);
                _sysUser.update(us);
                MessageBox.Show("Change password successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show("Current password is wrong", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }
    }
}
