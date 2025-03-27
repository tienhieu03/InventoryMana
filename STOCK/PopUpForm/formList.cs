using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer.DataModels;
using MaterialSkin.Controls;
using BusinessLayer;
using DataLayer;
using BusinessLayer.Utils;


namespace STOCK.PopUpForm
{
    public partial class formList : MaterialForm
    {
        public formList(DataGridView gvDetail, string str, int rowIndex = -1)
        {
            InitializeComponent();
            this._str = str;
            this._gvDetail = gvDetail;
            this._rowIndex = rowIndex;
        }

        private string _str;
        private DataGridView _gvDetail;
        private int _rowIndex = -1; // Store the row index where "." was entered
        private INVOICE_DETAIL _invoice_Detail;
        private PRODUCT _product;

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Insert();
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formList_Load(object sender, EventArgs e)
        {
            _invoice_Detail = new INVOICE_DETAIL();
            _product = new PRODUCT();

            string barcode = _str.Trim();

            if (!string.IsNullOrEmpty(barcode) && barcode != ".")
            {
                var product = _product.getItemBarcode(barcode);
                if (product != null)
                {
                    gvListProduct.DataSource = new List<tb_Product> { product };
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm với BARCODE: " + barcode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gvListProduct.DataSource = null;
                }
            }
            else if (string.IsNullOrEmpty(barcode))
            {
                MessageBox.Show("Vui lòng nhập BARCODE hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (barcode == ".")
            {
                // Hiển thị tất cả sản phẩm để lựa chọn
                List<tb_Product> allProducts = _product.getList().Select(p => _product.getItemBarcode(p.BARCODE)).Where(p => p != null).ToList();
                gvListProduct.DataSource = allProducts;
            }

            // Debug information - log the available columns in the target grid
            Console.WriteLine("Columns in gvDetail:");
            foreach (DataGridViewColumn col in _gvDetail.Columns)
            {
                Console.WriteLine($"Column: {col.Name}, Visible: {col.Visible}, Index: {col.Index}");
            }
        }
        void Insert()
        {
            if (!gvListProduct.Columns.Contains("BARCODE"))
            {
                throw new Exception("Column 'BARCODE' does not exist in gvListProduct.");
            }

            // Get selected rows using standard DataGridView
            List<DataGridViewRow> _selectRow = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in gvListProduct.SelectedRows)
            {
                _selectRow.Add(row);
            }
            
            List<string> _selected = new List<string>();
            foreach (DataGridViewRow item in _selectRow)
            {
                _selected.Add(item.Cells["BARCODE"].Value.ToString());
            }
            
            List<errExport> _err = new List<errExport>();
            List<string> _valid = new List<string>();
            List<string> _exist = new List<string>();
            
            if (_gvDetail.RowCount > 1)
            {
                if (_gvDetail.Rows[_gvDetail.RowCount - 1].Cells["ProductName"].Value != null)
                {
                    for (int i = 0; i < _gvDetail.RowCount; i++)
                        _exist.Add(_gvDetail.Rows[i].Cells["BARCODE"].Value.ToString());
                }
                else
                {
                    for (int i = 0; i < _gvDetail.RowCount - 1; i++)
                        _exist.Add(_gvDetail.Rows[i].Cells["BARCODE"].Value.ToString());
                }
            }

            //kiểm tra trước khi import
            for (int i = 0; i < _selected.Count; i++)
            {
                tb_Product hh = _product.getItemBarcode(_selected[i]);
                if (_exist.Contains(_selected[i]) == true)
                {
                    errExport err = new errExport();
                    err._barcode = _selected[i];
                    err._quantity = 1;
                    err._errcode = "Mã đã tồn tại trên lưới dữ liệu";
                    _err.Add(err);
                    continue;
                }
                else
                {
                    _valid.Add(_selected[i]);
                    continue;
                }
            }

            //Import những mã hợp lệ
            foreach (string _item in _valid)
            {
                tb_Product hh = _product.getItemBarcode(_item);
                if (_gvDetail.RowCount > 1)
                {
                    int mautin = _gvDetail.RowCount;
                    int lastRowIndex = mautin - 1;
                    _gvDetail.ClearSelection();
                    _gvDetail.Rows[lastRowIndex].Selected = true;
                    
                    if (_gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value == null)
                    {
                        _gvDetail.Rows[lastRowIndex].Cells["STT"].Value = mautin;
                        _gvDetail.Rows[lastRowIndex].Cells["BARCODE"].Value = hh.BARCODE;
                        _gvDetail.Rows[lastRowIndex].Cells["Unit"].Value = hh.Unit;
                        _gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value = hh.ProductName;
                        _gvDetail.Rows[lastRowIndex].Cells["Quantity"].Value = 1;
                        _gvDetail.Rows[lastRowIndex].Cells["Price"].Value = hh.Price;
                        _gvDetail.Rows[lastRowIndex].Cells["TotalPrice"].Value = hh.Price;
                    }
                    else
                    {
                        int newRowIndex = _gvDetail.Rows.Add();
                        mautin++;
                        _gvDetail.ClearSelection();
                        _gvDetail.Rows[newRowIndex].Selected = true;
                        
                        _gvDetail.Rows[newRowIndex].Cells["STT"].Value = mautin;
                        _gvDetail.Rows[newRowIndex].Cells["BARCODE"].Value = hh.BARCODE;
                        _gvDetail.Rows[newRowIndex].Cells["Unit"].Value = hh.Unit;
                        _gvDetail.Rows[newRowIndex].Cells["ProductName"].Value = hh.ProductName;
                        _gvDetail.Rows[newRowIndex].Cells["Quantity"].Value = 1;
                        _gvDetail.Rows[newRowIndex].Cells["Price"].Value = hh.Price;
                        _gvDetail.Rows[newRowIndex].Cells["TotalPrice"].Value = hh.Price;
                    }
                }
                else
                {
                    if (_gvDetail.RowCount == 0)
                        _gvDetail.Rows.Add();

                    int mautin = _gvDetail.RowCount;
                    int lastRowIndex = mautin - 1;
                    _gvDetail.ClearSelection();
                    _gvDetail.Rows[lastRowIndex].Selected = true;
                    
                    if (_gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value == null)
                    {
                        _gvDetail.Rows[lastRowIndex].Cells["STT"].Value = mautin;
                        _gvDetail.Rows[lastRowIndex].Cells["BARCODE"].Value = hh.BARCODE;
                        _gvDetail.Rows[lastRowIndex].Cells["Unit"].Value = hh.Unit;
                        _gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value = hh.ProductName;
                        _gvDetail.Rows[lastRowIndex].Cells["Quantity"].Value = 1;
                        _gvDetail.Rows[lastRowIndex].Cells["Price"].Value = hh.Price;
                        _gvDetail.Rows[lastRowIndex].Cells["TotalPrice"].Value = hh.Price;
                    }
                    else
                    {
                        int newRowIndex = _gvDetail.Rows.Add();
                        mautin++;
                        _gvDetail.ClearSelection();
                        _gvDetail.Rows[newRowIndex].Selected = true;
                        
                        _gvDetail.Rows[newRowIndex].Cells["STT"].Value = mautin;
                        _gvDetail.Rows[newRowIndex].Cells["BARCODE"].Value = hh.BARCODE;
                        _gvDetail.Rows[newRowIndex].Cells["Unit"].Value = hh.Unit;
                        _gvDetail.Rows[newRowIndex].Cells["ProductName"].Value = hh.ProductName;
                        _gvDetail.Rows[newRowIndex].Cells["Quantity"].Value = 1;
                        _gvDetail.Rows[newRowIndex].Cells["Price"].Value = hh.Price;
                        _gvDetail.Rows[newRowIndex].Cells["TotalPrice"].Value = hh.Price;
                    }
                }
            }
            
            // Add and remove the last row to create a new blank row
            int lastIndex = _gvDetail.Rows.Add();
            _gvDetail.ClearSelection();
            _gvDetail.Rows[lastIndex].Selected = true;
            if (_gvDetail.SelectedRows.Count > 0)
                _gvDetail.Rows.Remove(_gvDetail.SelectedRows[0]);
            
            _gvDetail.Refresh();
            
            if (_err.Count > 0)
            {
                popErrImport _errPopup = new popErrImport(_err);
                _errPopup.ShowDialog();
                this.Close();
            }
            else
            {
                this.Close();
            }
        }
    }
}
