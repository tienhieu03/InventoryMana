using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using BusinessLayer;
using BusinessLayer.DataModels;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using BusinessLayer.Utils;
using Excel = Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MaterialSkin.Controls;

namespace STOCK.Controls
{
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
            _right = 2; // Mặc định là full quyền nếu không truyền vào
        }

        public ProductControl(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            _user = user;
            _right = right;
        }

        tb_SYS_USER _user;
        int _right;
        bool _add;
        int _productID;
        SUPPLIER _supplier;
        UNIT _unit;
        PRODUCT_CATEGORY _category;
        ORIGIN _origin;
        SYS_SEQ _sysSeq;
        tb_SYS_SEQ _seq;
        PRODUCT _product;
        List<obj_PRODUCT> _lstPD=new List<obj_PRODUCT>();
        void loadCategory()
        {
            cboCategory.ComboBox.DataSource = _category.getAll();
            cboCategory.ComboBox.DisplayMember = "Category";
            cboCategory.ComboBox.ValueMember = "CategoryID";
        }

        void loadSupplier()
        {
            cboSupplier.DataSource = _supplier.getList();
            cboSupplier.DisplayMember = "SupplierName";
            cboSupplier.ValueMember = "SupplierID";
        }

        void loadUnit()
        {
            cboUnit.DataSource = _unit.getList();
            cboUnit.DisplayMember = "UnitName";
            cboUnit.ValueMember = "UnitID";
        }

        void loadOrigin()
        {
            cboOrigin.DataSource = _origin.getAll();
            cboOrigin.DisplayMember = "OriginName";
            cboOrigin.ValueMember = "OriginID";
        }


        void ShowHideControls(bool t)
        {
            btnAdd.Visible = t;
            btnEdit.Visible = t;
            btnDelete.Visible = t;
            btnSave.Visible = !t;
            btnCancel.Visible = !t;
        }

        void _enable(bool t)
        {
            txtName.Enabled = t;
            txtShortName.Enabled = t;
            rtxtDetail.Enabled = t;
            nudPrice.Enabled = t;
            cboSupplier.Enabled = t;
            cboUnit.Enabled = t;
            cboOrigin.Enabled = t;
            chkDisable.Enabled = t;
        }

        void ResetFields()
        {
            txtBarcode.Text = "";
            txtQrCode.Text = "";
            txtName.Text = "";
            txtShortName.Text = "";
            rtxtDetail.Text = "";
            nudPrice.Value = 0;
            chkDisable.Checked = false;
        }

        private void loadData()
        {
            gvList.AutoGenerateColumns = false;
            if (cboCategory.ComboBox.SelectedValue != null)
            {
                int categoryId;
                if (int.TryParse(cboCategory.ComboBox.SelectedValue.ToString(), out categoryId))
                {
                    gvList.DataSource = _product.getListByCategory(categoryId);
                    _lstPD = _product.getListByCategoryFull(categoryId);
                }
            }
            gvList.ReadOnly = true;
            gvList.ClearSelection();
        }


        private void ProductControl_Load(object sender, EventArgs e)
        {
            _supplier = new SUPPLIER();
            _unit = new UNIT();
            _category = new PRODUCT_CATEGORY();
            _origin = new ORIGIN();
            _product = new PRODUCT();
            _sysSeq = new SYS_SEQ();
            _enable(false);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ShowHideControls(true);
            loadCategory();
            loadData();

            loadSupplier();
            loadUnit();
            loadOrigin();

            nudPrice.DecimalPlaces = 0;

            cboCategory.SelectedIndexChanged += cboCategory_SelectedIndexChanged;
            gvList.CellFormatting += gvList_CellFormatting;
            gvList.ClearSelection();
            
            // Cập nhật trạng thái các nút dựa trên quyền hạn
            UpdateButtonsByPermission();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to add records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _add = true;
            _enable(true);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ResetFields();
            ShowHideControls(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to delete records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (gvList.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(gvList.SelectedRows[0].Cells["ProductID"].Value);

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _product.delete(productID);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_right < 2) // Nếu không có quyền Full Function
            {
                MessageBox.Show("You do not have permission to edit records.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            _add = false;
            _enable(true);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ShowHideControls(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_add)
            {
                tb_Product pd = new tb_Product();
                _seq = _sysSeq.getItem("PD@" + DateTime.Now.Year.ToString() + "@" + cboCategory.ComboBox.SelectedValue.ToString());
                if (_seq == null)
                {
                    _seq = new tb_SYS_SEQ();
                    _seq.SEQNAME = "PD@" + DateTime.Now.Year.ToString() + "@" + cboCategory.ComboBox.SelectedValue.ToString();
                    _seq.SEQVALUE = 1;
                    _sysSeq.add(_seq);
                }
                pd.BARCODE = BarcodeEAN13.BuildEan13(DateTime.Now.Year.ToString() + cboCategory.ComboBox.SelectedValue.ToString() + _seq.SEQVALUE.Value.ToString("0000000"));
                pd.ProductName = txtName.Text;
                pd.ShortName = txtShortName.Text;
                pd.QRCODE = txtQrCode.Text;//sửa
                pd.CategoryID = int.Parse(cboCategory.ComboBox.SelectedValue.ToString());
                pd.Description = rtxtDetail.Text;
                pd.SupplierID = int.Parse(cboSupplier.SelectedValue.ToString());
                pd.OriginID = int.Parse(cboOrigin.SelectedValue.ToString());
                pd.Unit = cboUnit.Text;
                pd.IsDisabled = chkDisable.Checked;
                pd.CreatedDate = DateTime.Now;
                pd.Price = float.Parse(nudPrice.Value.ToString());
                 _product.add(pd);
                txtBarcode.Text = pd.BARCODE;
                _sysSeq.udpate(_seq);
                MessageBox.Show(pd.BARCODE);

            }
            else
            {
                tb_Product pd = _product.getItem(_productID);
                bool wasDisabled = pd.IsDisabled ?? false;
                pd.ProductName = txtName.Text;
                pd.ShortName = txtShortName.Text;
                pd.CategoryID = int.Parse(cboCategory.ComboBox.SelectedValue.ToString());
                pd.Description = rtxtDetail.Text;
                pd.SupplierID = int.Parse(cboSupplier.SelectedValue.ToString());
                pd.OriginID = int.Parse(cboOrigin.SelectedValue.ToString());
                pd.Unit = cboUnit.Text;
                pd.IsDisabled = chkDisable.Checked;
                pd.UpdatedDate = DateTime.Now;
                pd.Price = float.Parse(nudPrice.Value.ToString());
                if (wasDisabled && !chkDisable.Checked)
                {
                    pd.RestoredDate = DateTime.Now;
                }
                txtBarcode.Text = pd.BARCODE;
                _product.update(pd);
            }
            _add = false;
            _enable(false);
            loadData();
            ResetFields();
            ShowHideControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ShowHideControls(true);
            ResetFields();
            txtBarcode.Enabled = false;
            //txtQrCode.Enabled = false;
        }

        private void gvList_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];

                _productID = (int)row.Cells["ProductID"].Value;
                cboCategory.ComboBox.SelectedValue = row.Cells["CategoryID"].Value;
                txtBarcode.Text = row.Cells["BARCODE"].Value.ToString();
                txtQrCode.Text = row.Cells["QRCODE"].Value.ToString();
                txtName.Text = row.Cells["ProductName"].Value.ToString();
                txtShortName.Text = row.Cells["ShortName"].Value.ToString();
                rtxtDetail.Text = row.Cells["Description"].Value.ToString();
                cboSupplier.SelectedValue = row.Cells["SupplierID"].Value;
                cboUnit.Text = row.Cells["Unit"].Value?.ToString();
                cboOrigin.SelectedValue = row.Cells["OriginID"].Value;
                nudPrice.Value = Convert.ToDecimal(row.Cells["Price"].Value);

                object isDisabledValue = row.Cells["IsDisabled"].Value;
                chkDisable.Checked = (isDisabledValue != null) && Convert.ToBoolean(isDisabledValue);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            _export();
        }

        private void ToggleButtons(bool isEnabled)
        {
            btnAdd.Enabled = isEnabled;
            btnEdit.Enabled = isEnabled;
            btnDelete.Enabled = isEnabled;
            btnSave.Enabled = isEnabled;
            btnCancel.Enabled = isEnabled;
            btnExport.Enabled = isEnabled;
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
                ToggleButtons(false);

            });

            Task.Run(() =>
            {
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = null;

                try
                {
                    ws = (Excel.Worksheet)wb.Sheets[1];
                    string categoryText = "";

                    // Đọc dữ liệu từ UI trên luồng chính
                    this.Invoke((MethodInvoker)delegate
                    {
                        categoryText = cboCategory.Text;
                    });

                    // Đặt tên sheet và định dạng Excel
                    this.Invoke((MethodInvoker)delegate
                    {
                        ws.Name = "DS" + categoryText;
                        ws.Range[ws.Cells[1, 1], ws.Cells[1, 13]].Merge();
                        ws.Cells[1, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        ws.Range[ws.Cells[1, 1], ws.Cells[1, 13]].BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
                        ws.Cells[1, 1].Value = "CATEGORY: " + categoryText.ToUpper();
                        ws.Cells[1, 1].Font.Size = 20;
                    });

                    // Lấy danh sách sản phẩm từ UI
                    List<obj_PRODUCT> productList = new List<obj_PRODUCT>();
                    this.Invoke((MethodInvoker)delegate
                    {
                        productList = _lstPD.ToList();
                    });

                    // Ghi dữ liệu vào Excel
                    ws.Cells[2, 1].Value = "BARCODE";
                    ws.Cells[2, 2].Value = "QRCODE";
                    ws.Cells[2, 3].Value = "ProductName";
                    ws.Cells[2, 4].Value = "ShortName";
                    ws.Cells[2, 5].Value = "Unit";
                    ws.Cells[2, 6].Value = "Price";
                    ws.Cells[2, 7].Value = "Description";
                    ws.Cells[2, 8].Value = "CategoryID";
                    ws.Cells[2, 9].Value = "Category";
                    ws.Cells[2, 10].Value = "SupplierID";
                    ws.Cells[2, 11].Value = "SupplierName";
                    ws.Cells[2, 12].Value = "OriginID";
                    ws.Cells[2, 13].Value = "OriginName";

                    for (int i = 0; i < productList.Count; i++)
                    {
                        ws.Cells[i + 3, 1].Value = productList[i].BARCODE;
                        ws.Cells[i + 3, 2].Value = productList[i].QRCODE;
                        ws.Cells[i + 3, 3].Value = productList[i].ProductName;
                        ws.Cells[i + 3, 4].Value = productList[i].ShortName;
                        ws.Cells[i + 3, 5].Value = productList[i].Unit;
                        ws.Cells[i + 3, 6].Value = productList[i].Price;
                        ws.Cells[i + 3, 7].Value = productList[i].Description;
                        ws.Cells[i + 3, 8].Value = productList[i].CategoryID;
                        ws.Cells[i + 3, 9].Value = productList[i].Category;
                        ws.Cells[i + 3, 10].Value = productList[i].SupplierID;
                        ws.Cells[i + 3, 11].Value = productList[i].SupplierName;
                        ws.Cells[i + 3, 12].Value = productList[i].OriginID;
                        ws.Cells[i + 3, 13].Value = productList[i].OriginName;
                    }

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

                        ToggleButtons(true);
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

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void gvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gvList.Columns[e.ColumnIndex].Name == "Price" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("N0");
                    e.FormattingApplied = true;
                }
            }
        }

        // Phương thức cập nhật trạng thái các nút dựa trên quyền
        private void UpdateButtonsByPermission()
        {
            if (_right == 0) // Lock Function
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnExport.Enabled = false;
            }
            else if (_right == 1) // View Only
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnExport.Enabled = true;
            }
            else // Full Function (2)
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnExport.Enabled = true;
            }
        }
    }
}
