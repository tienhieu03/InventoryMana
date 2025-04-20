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
using BusinessLayer.DataModels;
using ZXing;
using System.Drawing.Imaging;
using System.IO;
using STOCK.PopUpForm;
using SharedControls;

namespace STOCK.Forms
{
    public partial class formPrintBarcode : MaterialForm
    {
        public formPrintBarcode()
        {
            InitializeComponent();
        }

        PRODUCT_CATEGORY _category;
        PRODUCT _product;

        private void formPrintBarcode_Load(object sender, EventArgs e)
        {
            _category = new PRODUCT_CATEGORY();
            _product = new PRODUCT();
            LoadCate();
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            LoadList();
        }

        void LoadList()
        {
            dgvList.DataSource = _product.getListByCategory(int.Parse(cboCategory.SelectedValue.ToString()));
        }

        private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        void LoadCate()
        {
            cboCategory.DataSource = _category.getAll();
            cboCategory.DisplayMember = "Category";
            cboCategory.ValueMember = "CategoryID";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public byte[] GenerateBarcode(string content)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 100,
                    Width = 250,
                    Margin = 1
                }
            };
            using (Bitmap bitmap = writer.Write(content))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        private void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Barcode", typeof(string));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("BarcodeImage", typeof(byte[]));

            // Check if any rows are selected
            if (dgvList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one row to print.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (DataGridViewRow row in dgvList.SelectedRows) // Iterate through selected rows only
            {
                if (!row.IsNewRow)
                {
                    string name = row.Cells["ProductName"].Value?.ToString() ?? string.Empty;
                    string barcode = row.Cells["Barcode"].Value?.ToString() ?? string.Empty;
                    decimal price = 0;
                    int stampNumber = 0;

                    // Safely get Price
                    if (row.Cells["Price"].Value != null && decimal.TryParse(row.Cells["Price"].Value.ToString(), out decimal tempPrice))
                    {
                        price = tempPrice;
                    }

                    // Safely get StampNumber (Assuming column name is "StampNumber")
                    // Make sure the "StampNumber" column exists in your dgvList
                    if (dgvList.Columns.Contains("StampNumber") && row.Cells["StampNumber"] != null && row.Cells["StampNumber"].Value != null && int.TryParse(row.Cells["StampNumber"].Value.ToString(), out int tempStampNumber))
                    {
                        stampNumber = tempStampNumber;
                    }
                    else
                    {
                        // Optional: Handle cases where StampNumber is missing or invalid
                        // For now, we skip rows with invalid StampNumber or if the column doesn't exist
                         if (stampNumber <= 0) continue; // Skip this row if StampNumber is not valid or zero/negative
                    }


                    if (!string.IsNullOrEmpty(barcode)) // Only process if barcode exists
                    {
                         // Assuming BarcodeHelper exists and works. If not, use the local GenerateBarcode method.
                         // byte[] barcodeImage = GenerateBarcode(barcode); 
                         byte[] barcodeImage = SharedControls.BarcodeHelper.GenerateBarcode(barcode); // Use SharedControls if BarcodeHelper is there

                         // Add the row 'stampNumber' times
                         for (int i = 0; i < stampNumber; i++)
                         {
                             dt.Rows.Add(name, barcode, price, barcodeImage);
                         }
                    }
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No valid barcodes selected or generated. Check selection and 'StampNumber' values.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gán vào RDLC
            var rdlcForm = new FormBarcodeRDLC(dt);
            rdlcForm.ShowDialog();
        }

        // Keep the local GenerateBarcode method in case BarcodeHelper is not accessible or intended
        // public byte[] GenerateBarcode(string content) ... (existing method)
    }
}
