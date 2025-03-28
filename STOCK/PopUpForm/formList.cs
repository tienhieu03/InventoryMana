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
                    gvList.DataSource = new List<tb_Product> { product };
                    // Tự động chọn sản phẩm duy nhất
                    if (gvList.Rows.Count > 0)
                        gvList.Rows[0].Selected = true;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm với BARCODE: " + barcode, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gvList.DataSource = null;
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
                gvList.DataSource = allProducts;
                
                // Thêm thông báo hướng dẫn người dùng
                MessageBox.Show("Chọn các sản phẩm bằng cách giữ Ctrl và click vào từng dòng, hoặc giữ Shift để chọn một dải sản phẩm.", 
                                "Hướng dẫn", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!gvList.Columns.Contains("BARCODE"))
            {
                throw new Exception("Column 'BARCODE' does not exist in gvListProduct.");
            }

            // Kiểm tra có hàng được chọn không
            if (gvList.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm để import.", "Thông báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Get selected rows using standard DataGridView
            List<DataGridViewRow> _selectRow = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in gvList.SelectedRows)
            {
                _selectRow.Add(row);
            }
            
            // Log số lượng hàng đã chọn để debug
            Console.WriteLine($"Selected rows count: {_selectRow.Count}");
            
            List<string> _selected = new List<string>();
            foreach (DataGridViewRow item in _selectRow)
            {
                string barcode = item.Cells["BARCODE"].Value.ToString();
                _selected.Add(barcode);
                Console.WriteLine($"Selected BARCODE: {barcode}");
            }
            
            List<errExport> _err = new List<errExport>();
            List<string> _valid = new List<string>();
            List<string> _exist = new List<string>();
            
            // Debug thông tin các cột trong gvDetail để kiểm tra tên cột
            Console.WriteLine("Columns in gvDetail:");
            foreach (DataGridViewColumn col in _gvDetail.Columns)
            {
                Console.WriteLine($"Column: {col.Name}, Visible: {col.Visible}, Index: {col.Index}");
            }
            
            // Kiểm tra các mã đã tồn tại
            if (_gvDetail.RowCount > 0)
            {
                for (int i = 0; i < _gvDetail.RowCount; i++)
                {
                    var barcodeCell = _gvDetail.Rows[i].Cells["BARCODE"].Value;
                    if (barcodeCell != null && !string.IsNullOrEmpty(barcodeCell.ToString()))
                        _exist.Add(barcodeCell.ToString());
                }
            }

            // Kiểm tra trước khi import
            for (int i = 0; i < _selected.Count; i++)
            {
                tb_Product hh = _product.getItemBarcode(_selected[i]);
                if (_exist.Contains(_selected[i]))
                {
                    errExport err = new errExport();
                    err._barcode = _selected[i];
                    err._quantity = 1;
                    err._errcode = "Mã đã tồn tại trên lưới dữ liệu";
                    _err.Add(err);
                }
                else
                {
                    _valid.Add(_selected[i]);
                }
            }

            // Debug số lượng mã hợp lệ
            Console.WriteLine($"Valid barcodes count: {_valid.Count}");
            
            // Kiểm tra xem DataGridView có được liên kết với dữ liệu không
            bool isDataBound = _gvDetail.DataSource != null;
            Console.WriteLine($"Is gvDetail data-bound: {isDataBound}");
            
            // Xử lý khác nhau dựa trên việc DataGridView có được liên kết với dữ liệu hay không
            if (isDataBound)
            {
                try
                {
                    // Lấy BindingSource nếu có
                    BindingSource bsSource = _gvDetail.DataSource as BindingSource;
                    
                    if (bsSource != null)
                    {
                        Console.WriteLine($"DataSource type: {bsSource.DataSource?.GetType().Name}");
                        
                        // Kiểm tra loại nguồn dữ liệu
                        if (bsSource.DataSource is List<obj_INVOICE_DETAIL> detailList)
                        {
                            // Tìm STT lớn nhất
                            int maxSTT = 0;
                            foreach (var item in detailList)
                            {
                                if (item.STT.HasValue && item.STT.Value > maxSTT)
                                    maxSTT = item.STT.Value;
                            }
                            
                            // Thêm các sản phẩm hợp lệ vào danh sách
                            foreach (string barcode in _valid)
                            {
                                tb_Product product = _product.getItemBarcode(barcode);
                                if (product != null)
                                {
                                    obj_INVOICE_DETAIL newDetail = new obj_INVOICE_DETAIL
                                    {
                                        STT = maxSTT + 1,
                                        BARCODE = product.BARCODE,
                                        ProductName = product.ProductName,
                                        Unit = product.Unit,
                                        Quantity = 1,
                                        Price = product.Price,
                                        SubTotal = product.Price,
                                        ProductID = product.ProductID
                                    };
                                    
                                    // Tăng maxSTT cho item tiếp theo
                                    maxSTT++;
                                    
                                    // Thêm vào danh sách
                                    detailList.Add(newDetail);
                                    
                                    // Debug thông tin
                                    Console.WriteLine($"Added data-bound product: {product.BARCODE}, STT: {maxSTT}");
                                }
                            }
                            
                            // Cập nhật BindingSource
                            bsSource.ResetBindings(false);
                        }
                        else if (bsSource.DataSource is DataTable dt)
                        {
                            // Xử lý khi nguồn dữ liệu là DataTable
                            foreach (string barcode in _valid)
                            {
                                tb_Product product = _product.getItemBarcode(barcode);
                                if (product != null)
                                {
                                    DataRow newRow = dt.NewRow();
                                    
                                    // Thiết lập các giá trị cho dòng mới
                                    newRow["STT"] = dt.Rows.Count + 1;
                                    newRow["BARCODE"] = product.BARCODE;
                                    newRow["ProductName"] = product.ProductName;
                                    newRow["Unit"] = product.Unit;
                                    newRow["QuantityDetail"] = 1;
                                    newRow["Price"] = product.Price;
                                    newRow["SubTotal"] = product.Price;
                                    
                                    // Thêm thông tin ProductID
                                    if (dt.Columns.Contains("ProductID"))
                                        newRow["ProductID"] = product.ProductID;
                                    
                                    // Thêm dòng vào DataTable
                                    dt.Rows.Add(newRow);
                                    
                                    // Debug thông tin
                                    Console.WriteLine($"Added DataTable product: {product.BARCODE}");
                                }
                            }
                            
                            // Cập nhật BindingSource
                            bsSource.ResetBindings(false);
                        }
                        else
                        {
                            Console.WriteLine("Cannot determine how to add items to this data source type");
                            MessageBox.Show("Không thể xác định cách thêm dữ liệu vào nguồn này. Vui lòng thử thêm thủ công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        Console.WriteLine("DataSource is not a BindingSource");
                        MessageBox.Show("Nguồn dữ liệu không phải là BindingSource. Vui lòng thử thêm thủ công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling data-bound grid: {ex.Message}");
                    MessageBox.Show($"Lỗi khi thêm dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Thêm dữ liệu trực tiếp vào DataGridView khi không có nguồn dữ liệu
                if (_valid.Count > 0)
                {
                    int startingRowCount = _gvDetail.RowCount;
                    Console.WriteLine($"Starting row count: {startingRowCount}");
                    
                    // Tìm dòng cuối để bắt đầu thêm dữ liệu
                    int lastRowIndex = _gvDetail.RowCount - 1;
                    bool useExistingLastRow = lastRowIndex >= 0 && 
                                              (_gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value == null || 
                                               string.IsNullOrEmpty(_gvDetail.Rows[lastRowIndex].Cells["ProductName"].Value?.ToString()));
                    
                    // Thêm tất cả các sản phẩm hợp lệ vào grid
                    int currentSTT = 1;
                    
                    // Tính STT hiện tại
                    if (_gvDetail.RowCount > 0)
                    {
                        for (int i = 0; i < _gvDetail.RowCount; i++)
                        {
                            if (_gvDetail.Rows[i].Cells["STT"].Value != null &&
                                int.TryParse(_gvDetail.Rows[i].Cells["STT"].Value.ToString(), out int stt))
                            {
                                if (stt > currentSTT)
                                    currentSTT = stt;
                            }
                        }
                        
                        // Nếu dòng cuối chưa có dữ liệu, dùng STT hiện tại
                        // Nếu không, tăng STT lên 1 cho dòng mới
                        if (!useExistingLastRow)
                            currentSTT++;
                    }
                    
                    // Lưu lại số lượng sản phẩm hợp lệ để debug
                    int validProductsCount = _valid.Count;
                    Console.WriteLine($"Valid products to add: {validProductsCount}");
                    
                    for (int i = 0; i < _valid.Count; i++)
                    {
                        string barcode = _valid[i];
                        tb_Product product = _product.getItemBarcode(barcode);
                        
                        // Xác định dòng để thêm dữ liệu
                        int rowIndex;
                        if (i == 0 && useExistingLastRow)
                        {
                            // Sử dụng dòng cuối cùng nếu nó trống và đây là sản phẩm đầu tiên
                            rowIndex = lastRowIndex;
                        }
                        else
                        {
                            // Thêm dòng mới
                            rowIndex = _gvDetail.Rows.Add();
                        }
                        
                        // Điền thông tin sản phẩm
                        _gvDetail.Rows[rowIndex].Cells["STT"].Value = currentSTT++;
                        _gvDetail.Rows[rowIndex].Cells["BARCODE"].Value = product.BARCODE;
                        _gvDetail.Rows[rowIndex].Cells["Unit"].Value = product.Unit;
                        _gvDetail.Rows[rowIndex].Cells["ProductName"].Value = product.ProductName;
                        _gvDetail.Rows[rowIndex].Cells["QuantityDetail"].Value = 1;
                        _gvDetail.Rows[rowIndex].Cells["Price"].Value = product.Price;
                        _gvDetail.Rows[rowIndex].Cells["SubTotal"].Value = product.Price;
                        
                        // Debug thông tin chi tiết của dòng đã thêm
                        Console.WriteLine($"Added product {i+1}/{validProductsCount}: {product.BARCODE} at row {rowIndex}");
                        string rowDetail = "Row " + rowIndex + " detail: ";
                        foreach (DataGridViewCell cell in _gvDetail.Rows[rowIndex].Cells)
                        {
                            if (cell.Value != null)
                                rowDetail += $"{_gvDetail.Columns[cell.ColumnIndex].Name}={cell.Value}, ";
                        }
                        Console.WriteLine(rowDetail);
                    }
                    
                    // Đảm bảo luôn có một hàng trống ở cuối để nhập sản phẩm mới
                    try
                    {
                        int emptyRowIndex = _gvDetail.Rows.Add();
                        _gvDetail.Rows[emptyRowIndex].Cells["STT"].Value = currentSTT;
                        Console.WriteLine($"Final row count after adding: {_gvDetail.RowCount}");
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Xử lý trường hợp không thể thêm dòng vào DataGridView
                        Console.WriteLine($"Cannot add empty row: {ex.Message}");
                    }
                }
            }
            
            // Buộc refresh lưới để hiển thị dữ liệu
            _gvDetail.Refresh();
            
            // Hiển thị thông báo lỗi nếu có
            if (_err.Count > 0)
            {
                popErrImport _errPopup = new popErrImport(_err);
                _errPopup.ShowDialog();
            }
            
            // Thông báo số lượng sản phẩm đã import thành công
            if (_valid.Count > 0)
            {
                MessageBox.Show($"Đã import thành công {_valid.Count} sản phẩm.", "Thông báo", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            this.Close();
        }
    }
}
