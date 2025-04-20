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
using BusinessLayer.DataModels;
using DataLayer;
using MATERIAL.MyFunctions;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace STOCK.Controls
{
    public partial class CompanyStock : UserControl
    {
        public CompanyStock()
        {
            InitializeComponent();
        }
        public CompanyStock(tb_SYS_USER user, int right) // Renamed parameters for clarity
        {
            InitializeComponent();
            this._user = user; // Assign the passed user object to the class field
            this._right = right; // Assign the passed right value to the class field
        }
        tb_SYS_USER _user;
        int _right;
        INVENTORY _stock;
        COMPANY _company;
        List<obj_INVENTORY> _itemList = new List<obj_INVENTORY>();
        private void CompanyStock_Load(object sender, EventArgs e)
        {
            dgvStock.AutoGenerateColumns = false;
            _company = new COMPANY();
            _stock = new INVENTORY();
            LoadCompany();
            cboCompany.SelectedValue = myFunctions._compid;
            dtPeriod.Value = DateTime.Now;
            LoadStock(myFunctions._compid, DateTime.Now.Year, DateTime.Now.Month);
            _itemList = _stock.getCompanyStock(myFunctions._compid, dtPeriod.Value.Year, dtPeriod.Value.Month);
            txtSearch.KeyDown += txtSearch_KeyDown;
        }
        void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }
        void LoadStock(string companyId, int year, int period)
        {
            dgvStock.DataSource = _stock.getCompanyStock(companyId, year, period);
            dgvStock.ReadOnly = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            _export();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            LoadStock(cboCompany.SelectedValue.ToString(), dtPeriod.Value.Year, dtPeriod.Value.Month);
            _itemList = _stock.getCompanyStock(cboCompany.SelectedValue.ToString(), dtPeriod.Value.Year, dtPeriod.Value.Month);
        }
        void _export()
        {
            string nameFile = "";
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Excel 2000-2003 (.xls)|*.xls|Excel 2007 or higher (.xlsx)|*.xlsx";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                nameFile = saveFile.FileName;
            }
            if (string.IsNullOrEmpty(nameFile)) return;

            // Đảm bảo hiển thị ProgressBar trên luồng chính
            this.Invoke((MethodInvoker)delegate
            {
                progressBarExport.Visible = true;

            });

            // Get company name on the UI thread before starting the task
            string companyName = cboCompany.Text;

            Task.Run(() =>
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = null;

                try
                {
                    ws = wb.ActiveSheet;
                    //Sheets name - Use the captured companyName variable
                    ws.Name = "DM " + companyName;
                    ws.Range[ws.Cells[1, 1], ws.Cells[1, 12]].Merge();
                    //text padding
                    ws.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Ghi dữ liệu vào Excel
                    ws.Range[ws.Cells[1, 1], ws.Cells[1, 12]].BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
                    // Use the captured companyName variable
                    ws.Cells[1, 1].Value = "STOCK" + companyName.ToUpper();
                    ws.Cells[1, 1].Font.Size = 20;
                    ws.Cells[2, 1].Value = "BARCODE";
                    ws.Cells[2, 2].Value = "Product Name";
                    ws.Cells[2, 3].Value = "Unit";
                    ws.Cells[2, 4].Value = "BeginningQty";
                    ws.Cells[2, 5].Value = "ReceivedQty";
                    ws.Cells[2, 6].Value = "RecInnerQty";
                    ws.Cells[2, 7].Value = "OutInnerQty";
                    ws.Cells[2, 8].Value = "BulkQty";
                    ws.Cells[2, 9].Value = "IssuedQty";
                    ws.Cells[2, 10].Value = "FinalQty";
                    ws.Cells[2, 11].Value = "Value";
                    ws.Cells[2, 12].Value = "TotalValue";
                    ws.Cells[2, 13].Value = "PeriodYear";
                    ws.Cells[2, 14].Value = "Year";
                    ws.Cells[2, 15].Value = "Period";

                    //export data
                    for (int i = 0; i < _itemList.Count; i++) // Corrected loop condition
                    {
                        var item = _itemList[i]; // Access item directly by index
                        if (item != null) // Add null check for safety
                        {
                            ws.Cells[i + 3, 1].Value = item.BARCODE; // Start writing from row 3 (since header is row 2)
                            ws.Cells[i + 3, 2].Value = item.ProductName;
                            ws.Cells[i + 3, 3].Value = item.Unit;
                            ws.Cells[i + 3, 4].Value = item.BeginningQuantity;
                            ws.Cells[i + 3, 5].Value = item.ReceivedQuantity;
                            ws.Cells[i + 3, 6].Value = item.RecInnerQuantity;
                            ws.Cells[i + 3, 7].Value = item.OutInnerQuantity;
                            ws.Cells[i + 3, 8].Value = item.BulkQuantity;
                            ws.Cells[i + 3, 9].Value = item.IssuedQuantity;
                            ws.Cells[i + 3, 10].Value = item.FinalQuantity;
                            ws.Cells[i + 3, 11].Value = item.Value;
                            ws.Cells[i + 3, 12].Value = item.TotalValue;
                            ws.Cells[i + 3, 13].Value = item.PeriodYear;
                            ws.Cells[i + 3, 14].Value = item.Year;
                            ws.Cells[i + 3, 15].Value = item.Period;
                        }
                    }

                    // Adjust column widths for better readability (optional)
                    ws.Columns.AutoFit(); 

                    wb.SaveAs(nameFile);
                }
                catch (Exception ex)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                }
                finally
                {
                    wb.Close(true);
                    app.Quit();
                    releaseObject(wb);
                    releaseObject(app);

                    // Ẩn ProgressBar sau khi hoàn tất
                    this.Invoke((MethodInvoker)delegate
                    {

                        progressBarExport.Visible = false;
                        MessageBox.Show("Exported successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    });
                }
            });
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dgvStock.DataSource = _itemList;
            }
            else
            {
                var filteredList = _itemList.Where(item => item.ProductName != null && item.ProductName.ToLower().Contains(keyword)).ToList();
                dgvStock.DataSource = filteredList;
            }

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
