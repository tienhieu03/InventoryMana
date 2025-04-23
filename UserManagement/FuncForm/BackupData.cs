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
using POS;

namespace UserManagement.FuncForm
{
    public partial class BackupData : MaterialForm
    {
        public BackupData()
        {
            InitializeComponent();
        }
        
        private void BackupData_Load(object sender, EventArgs e)
        {
            txtURL.Enabled = false;
            btnBackup.Enabled = false;
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // Get backup path, or generate one if empty
                string backupPath = txtURL.Text.Trim();
                if (string.IsNullOrEmpty(backupPath))
                {
                    // Generate automatic filename with timestamp
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                    string defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    backupPath = Path.Combine(defaultDirectory, $"Inventory-Management-{timestamp}.bak");
                    txtURL.Text = backupPath;
                }

                // Ensure directory exists and is writable
                string directory = Path.GetDirectoryName(backupPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    if (!Directory.Exists(directory))
                    {
                        try
                        {
                            Directory.CreateDirectory(directory);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Cannot create directory: {ex.Message}",
                                "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Test write permission
                    try
                    {
                        string testFile = Path.Combine(directory, "test_write.tmp");
                        File.WriteAllText(testFile, "Test");
                        File.Delete(testFile);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("You don't have write permissions to the selected folder. Please select another location.",
                            "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Execute the backup with the standardized filename
                ExecuteBackup(backupPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during backup: {ex.Message}",
                    "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteBackup(string backupPath)
        {
            // Get database connection with proper credentials
            string connectionString = GetConnectionString();

            // Execute the backup
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string databaseName = ExtractDatabaseName(connectionString);
                string backupQuery = $"BACKUP DATABASE [{databaseName}] TO DISK = N'{backupPath}' WITH FORMAT, INIT, NAME = N'{databaseName}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                connection.Open();
                using (SqlCommand command = new SqlCommand(backupQuery, connection))
                {
                    command.CommandTimeout = 300; // 5 minutes timeout for large databases
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Database backup created successfully!",
                    "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetConnectionString()
        {
            try
            {
                // Read connection info from connectdb.dba file
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream fs = File.Open("connectdb.dba", FileMode.Open, FileAccess.Read))
                {
                    connect cp = (connect)bf.Deserialize(fs);

                    // Decrypt connection info
                    string servername = Encryptor.Decrypt(cp.servername, "qwertyuiop!@#$", true);
                    string username = Encryptor.Decrypt(cp.username, "qwertyuiop!@#$", true);
                    string password = Encryptor.Decrypt(cp.passwd, "qwertyuiop!@#$", true);
                    string database = Encryptor.Decrypt(cp.database, "qwertyuiop!@#$", true);

                    // Build connection string with explicit SQL credentials
                    return $"Data Source={servername};Initial Catalog={database};" +
                           $"User ID={username};Password={password};Integrated Security=False;";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading connection information: {ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw; // Re-throw to be caught by the calling function
            }
        }

        private string ExtractDatabaseName(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string suggestedFilename = $"Inventory-Management-{timestamp}.bak";

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                saveDialog.Title = "Select Backup Location";
                saveDialog.DefaultExt = "bak";
                saveDialog.AddExtension = true;
                saveDialog.FileName = suggestedFilename;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    txtURL.Text = saveDialog.FileName;
                    btnBackup.Enabled = true;
                }
            }
        }
    }
}
