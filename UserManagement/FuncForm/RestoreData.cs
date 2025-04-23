using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using MaterialSkin.Controls;

namespace UserManagement.FuncForm
{
    public partial class RestoreData : MaterialForm
    {
        private SqlConnection con;
        public RestoreData()
        {
            InitializeComponent();
        }

        private void RestoreData_Load(object sender, EventArgs e)
        {
            txtURL.Enabled = false;
            btnRestore.Enabled = false;

            try
            {
                // Khởi tạo connection
                con = new SqlConnection(GetConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetConnectionString()
        {
            try
            {
                // Đọc thông tin kết nối từ file connectdb.dba
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read))
                {
                    connect cp = (connect)bf.Deserialize(fs);

                    // Giải mã thông tin kết nối
                    string servername = Encryptor.Decrypt(cp.servername, "qwertyuiop!@#$", true);
                    string username = Encryptor.Decrypt(cp.username, "qwertyuiop!@#$", true);
                    string password = Encryptor.Decrypt(cp.passwd, "qwertyuiop!@#$", true);
                    string database = Encryptor.Decrypt(cp.database, "qwertyuiop!@#$", true);

                    // Tạo chuỗi kết nối với thông tin xác thực SQL
                    return $"Data Source={servername};Initial Catalog={database};" +
                           $"User ID={username};Password={password};Integrated Security=False;";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đọc thông tin kết nối: {ex.Message}",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Ném lại ngoại lệ để hàm gọi xử lý
            }
        }

        private string ExtractDatabaseName(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }

        private void ExecuteRestore()
        {
            string database = ExtractDatabaseName(con.ConnectionString);

            try
            {
                Cursor = Cursors.WaitCursor;

                // Hiển thị thông báo đang xử lý
                Label lblWaiting = new Label();
                lblWaiting.Text = "Đang khôi phục dữ liệu, vui lòng chờ...";
                lblWaiting.AutoSize = true;
                lblWaiting.Location = new Point(20, this.Height - 60);
                lblWaiting.Font = new Font(lblWaiting.Font, FontStyle.Bold);
                lblWaiting.ForeColor = Color.Blue;
                this.Controls.Add(lblWaiting);
                this.Refresh();

                if (con.State != ConnectionState.Open)
                    con.Open();

                // Đặt cơ sở dữ liệu sang chế độ single user
                string sql1 = $"ALTER DATABASE [{database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                using (SqlCommand cmd1 = new SqlCommand(sql1, con))
                {
                    cmd1.ExecuteNonQuery();
                }

                // Khôi phục cơ sở dữ liệu từ file backup
                string sql2 = $"USE MASTER RESTORE DATABASE [{database}] FROM DISK ='{txtURL.Text}' WITH REPLACE";
                using (SqlCommand cmd2 = new SqlCommand(sql2, con))
                {
                    cmd2.CommandTimeout = 300; // 5 phút cho cơ sở dữ liệu lớn
                    cmd2.ExecuteNonQuery();
                }

                // Đặt cơ sở dữ liệu về chế độ multi user
                string sql3 = $"ALTER DATABASE [{database}] SET MULTI_USER";
                using (SqlCommand cmd3 = new SqlCommand(sql3, con))
                {
                    cmd3.ExecuteNonQuery();
                }

                if (con.State == ConnectionState.Open)
                    con.Close();

                this.Controls.Remove(lblWaiting);
                lblWaiting.Dispose();

                Cursor = Cursors.Default;
                MessageBox.Show("Khôi phục dữ liệu thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRestore.Enabled = false;
            }
            catch (Exception ex)
            {
                try
                {
                    // Cố gắng đặt cơ sở dữ liệu về chế độ multi user nếu có lỗi
                    if (con.State == ConnectionState.Open)
                    {
                        using (SqlCommand cmd = new SqlCommand($"ALTER DATABASE [{database}] SET MULTI_USER", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                }
                catch { /* Bỏ qua lỗi khi cố gắng khôi phục */ }

                Cursor = Cursors.Default;
                MessageBox.Show($"Khôi phục dữ liệu không thành công: {ex.Message}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRestore.Enabled = false;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // Kiểm tra file backup có tồn tại không
            if (!File.Exists(txtURL.Text))
            {
                MessageBox.Show("File backup không tồn tại. Vui lòng chọn file hợp lệ.",
                    "Lỗi file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác nhận khôi phục
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn khôi phục cơ sở dữ liệu từ file backup này?\n\n" +
                "CẢNH BÁO: Dữ liệu hiện tại sẽ bị thay thế hoàn toàn.\n" +
                "Tất cả kết nối đến cơ sở dữ liệu sẽ bị ngắt.",
                "Xác nhận khôi phục", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            ExecuteRestore();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                openFileDialog.Title = "Chọn file backup để khôi phục";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtURL.Text = openFileDialog.FileName;
                    btnRestore.Enabled = true;
                }
            }
        }
    }
}
