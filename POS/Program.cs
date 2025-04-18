using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using STOCK.Forms;

namespace POS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (File.Exists("connectdb.dba"))
            {
                string conStr = "";
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read);
                connect cp = (connect)bf.Deserialize(fs);
                string deCryptServ = Encryptor.Decrypt(cp.servername, "qwertyuiop!@#$", true);
                string deCryptUser = Encryptor.Decrypt(cp.Username, "qwertyuiop!@#$", true);
                string deCryptPass = Encryptor.Decrypt(cp.passwd, "qwertyuiop!@#$", true);
                string deCryptDB = Encryptor.Decrypt(cp.database, "qwertyuiop!@#$", true);
                conStr += "Data Source=" + deCryptServ + ";Initial Catalog=" + deCryptDB + ";User ID=" + deCryptUser + ";Password=" + deCryptPass + ";";
                connoi = conStr;
                SqlConnection con = new SqlConnection(conStr);

                try
                {
                    con.Open();

                }
                catch
                {
                    MessageBox.Show("Connection failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fs.Close();
                }
                con.Close();
                fs.Close();
                if (File.Exists("sysparam.ini"))
                {
                    Application.Run(new formLogin());
                }
                else
                {
                    Application.Run(new formSetParam());
                }
            }
            else
            {
                Application.Run(new formConnect());
            }
        }
        public static string connoi = "";
    }
}
