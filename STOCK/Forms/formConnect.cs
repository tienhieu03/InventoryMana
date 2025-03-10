using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using MaterialSkin.Controls;
using Krypton.Toolkit;

namespace STOCK.Forms
{
    public partial class formConnect: MaterialForm
    {
        public formConnect()
        {
            InitializeComponent();
        }
        SqlConnection GetCon(string server, string username, string pass, string database)
        {
            return new SqlConnection("Data Source=" + server + "; Initial Catalog=" + database + "; User ID=" + username + "; Password=" + pass + ";");
        }

        private void checkBtn_Click(object sender, EventArgs e)
        {
            SqlConnection con = GetCon(txtServer.Text, txtUsername.Text, txtPassword.Text, cboDatabase.Text);
            try
            {
                con.Open();
                MessageBox.Show("Connection successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Connection Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string enCryptServ = Encryptor.Encrypt(txtServer.Text, "qwertyuiop!@#$", true);
            string enCryptUser = Encryptor.Encrypt(txtUsername.Text, "qwertyuiop!@#$", true);
            string enCryptPass = Encryptor.Encrypt(txtPassword.Text, "qwertyuiop!@#$", true);
            string enCryptDB = Encryptor.Encrypt(cboDatabase.Text, "qwertyuiop!@#$", true);
            connect cn = new connect(enCryptServ, enCryptUser, enCryptPass, enCryptDB);
            cn.SaveFile();
            MessageBox.Show("Connection saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //SaveFileDialog sf = new SaveFileDialog();
            //sf.Title = "Choose where to save";
            //sf.Filter = "Text Files (*.dba)|*.dba| AllFiles(*.*)|*.*";
            //if (sf.ShowDialog()==DialogResult.OK)
            //{
            //    connect cn = new connect(enCryptServ, enCryptUser, enCryptPass, enCryptDB);
            //    cn.ConnectData(sf.FileName);
            //    MessageBox.Show("Connection saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            SqlConnection con = GetCon(txtServer.Text, txtUsername.Text, txtPassword.Text, cboDatabase.Text);
            try
            {
                con.Open();
                MessageBox.Show("Connection successful", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Connection Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboDatabase_DropDown(object sender, EventArgs e)
        {
            KryptonComboBox cboDatabase = (KryptonComboBox)sender;
            cboDatabase.Items.Clear();
            try
            {
                string Conn = "server=" + txtServer.Text + ";User Id=" + txtUsername.Text + "; password=" + txtPassword.Text + ";";
                using (SqlConnection con = new SqlConnection(Conn))
                {
                    con.Open();
                    string sql = "SELECT NAME FROM SYS.DATABASES WHERE NAME NOT IN ('master', 'tempdb', 'model', 'msdb')";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    using (IDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cboDatabase.Items.Add(dr[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
