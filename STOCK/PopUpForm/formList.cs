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
                List<tb_Product> allProducts = _product.getList().Select(p => _product.getItemBarcode(p.BARCODE)).Where(p => p != null).ToList();
                gvList.DataSource = allProducts;
            }
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
                            // Tìm STT lớn nhất để sử dụng cho dòng mới
                            int maxSTT = 0;
                            foreach (var item in detailList)
                            {
                                if (item.STT.HasValue && item.STT.Value > maxSTT)
                                    maxSTT = item.STT.Value;
                            }
                            
                            // Tạo danh sách sản phẩm mới để thêm
                            List<obj_INVOICE_DETAIL> newDetails = new List<obj_INVOICE_DETAIL>();
                            
                            // Thêm các sản phẩm hợp lệ vào danh sách mới
                            foreach (string barcode in _valid)
                            {
                                tb_Product product = _product.getItemBarcode(barcode);
                                if (product != null)
                                {
                                    obj_INVOICE_DETAIL newDetail = new obj_INVOICE_DETAIL
                                    {
                                        STT = ++maxSTT,
                                        BARCODE = product.BARCODE,
                                        ProductName = product.ProductName,
                                        Unit = product.Unit,
                                        Quantity = 1,
                                        Price = product.Price,
                                        ProductID = product.ProductID
                                    };
                                    
                                    // Debug thông tin
                                    Console.WriteLine($"Added data-bound product: {product.BARCODE}, STT: {maxSTT}, Price: {product.Price}, SubTotal: {newDetail.SubTotal}");
                                    
                                    // Thêm vào danh sách mới
                                    newDetails.Add(newDetail);
                                }
                            }
                            
                            // Thêm ở đầu danh sách
                            newDetails.Reverse(); // Đảo ngược thứ tự để khi thêm vào đầu thì sản phẩm đầu tiên sẽ ở trên cùng
                            foreach (var newDetail in newDetails)
                            {
                                detailList.Insert(0, newDetail);
                            }
                            
                            // Cập nhật lại STT để đảm bảo thứ tự liên tục
                            for (int i = 0; i < detailList.Count; i++)
                            {
                                detailList[i].STT = i + 1;
                            }
                            
                            // Đảm bảo có một dòng trống ở cuối
                            bool hasEmptyLastRow = false;
                            if (detailList.Count > 0)
                            {
                                var lastItem = detailList[detailList.Count - 1];
                                hasEmptyLastRow = string.IsNullOrEmpty(lastItem.BARCODE);
                            }
                            
                            if (!hasEmptyLastRow)
                            {
                                obj_INVOICE_DETAIL emptyRow = new obj_INVOICE_DETAIL
                                {
                                    STT = detailList.Count + 1
                                };
                                detailList.Add(emptyRow);
                            }
                            
                            // Cập nhật BindingSource
                            bsSource.ResetBindings(false);
                        }
                        else if (bsSource.DataSource is DataTable dt)
                        {
                            // Xử lý khi nguồn dữ liệu là DataTable
                            List<DataRow> newRows = new List<DataRow>();
                            
                            // Tạo các dòng mới
                            foreach (string barcode in _valid)
                            {
                                tb_Product product = _product.getItemBarcode(barcode);
                                if (product != null)
                                {
                                    DataRow newRow = dt.NewRow();
                                    
                                    // Thiết lập các giá trị cho dòng mới
                                    newRow["BARCODE"] = product.BARCODE;
                                    newRow["ProductName"] = product.ProductName;
                                    newRow["Unit"] = product.Unit;
                                    newRow["QuantityDetail"] = 1;
                                    newRow["Price"] = product.Price;
                                    
                                    // Tính SubTotal = Price * QuantityDetail
                                    double price = Convert.ToDouble(product.Price);
                                    double quantity = 1; // Đã gán QuantityDetail là 1
                                    newRow["SubTotal"] = price * quantity;
                                    
                                    // Thêm thông tin ProductID
                                    if (dt.Columns.Contains("ProductID"))
                                        newRow["ProductID"] = product.ProductID;
                                    
                                    // Thêm vào danh sách dòng mới
                                    newRows.Add(newRow);
                                    
                                    // Debug thông tin
                                    Console.WriteLine($"Added DataTable product: {product.BARCODE}, Price: {price}, SubTotal: {price * quantity}");
                                }
                            }
                            
                            // Thêm các dòng mới vào đầu DataTable
                            newRows.Reverse();
                            foreach (DataRow newRow in newRows)
                            {
                                dt.Rows.InsertAt(newRow, 0);
                            }
                            
                            // Cập nhật lại STT
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["STT"] = i + 1;
                            }
                            
                            // Đảm bảo có một dòng trống ở cuối
                            bool hasEmptyLastRow = false;
                            if (dt.Rows.Count > 0)
                            {
                                var lastRow = dt.Rows[dt.Rows.Count - 1];
                                hasEmptyLastRow = string.IsNullOrEmpty(lastRow["BARCODE"]?.ToString());
                            }
                            
                            if (!hasEmptyLastRow)
                            {
                                DataRow emptyRow = dt.NewRow();
                                emptyRow["STT"] = dt.Rows.Count + 1;
                                dt.Rows.Add(emptyRow);
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
                    
                    // Lưu lại số lượng sản phẩm hợp lệ để debug
                    int validProductsCount = _valid.Count;
                    Console.WriteLine($"Valid products to add: {validProductsCount}");
                    
                    // Tạo danh sách các sản phẩm sẽ thêm
                    List<DataGridViewRow> rowsToAdd = new List<DataGridViewRow>();
                    
                    // Tạo các dòng mới
                    for (int i = 0; i < _valid.Count; i++)
                    {
                        string barcode = _valid[i];
                        tb_Product product = _product.getItemBarcode(barcode);
                        
                        // Tạo dòng mới cho DataGridView
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(_gvDetail);
                        
                        // Điền thông tin sản phẩm
                        newRow.Cells["BARCODE"].Value = product.BARCODE;
                        newRow.Cells["Unit"].Value = product.Unit;
                        newRow.Cells["ProductName"].Value = product.ProductName;
                        newRow.Cells["QuantityDetail"].Value = 1; // Đảm bảo QuantityDetail là 1
                        newRow.Cells["Price"].Value = product.Price;
                        
                        // Tính SubTotal = Price * QuantityDetail
                        double price = Convert.ToDouble(product.Price);
                        double quantity = 1; // Đã gán QuantityDetail là 1
                        newRow.Cells["SubTotal"].Value = price * quantity;
                        
                        // Thêm vào danh sách dòng mới
                        rowsToAdd.Add(newRow);
                        
                        // Debug thông tin
                        Console.WriteLine($"Created row for product {i+1}/{validProductsCount}: {product.BARCODE}, Price: {price}, SubTotal: {price * quantity}");
                    }
                    
                    // Thêm dòng vào đầu DataGridView
                    for (int i = rowsToAdd.Count - 1; i >= 0; i--)
                    {
                        _gvDetail.Rows.Insert(0, rowsToAdd[i]);
                        Console.WriteLine($"Added product {rowsToAdd.Count - i}/{validProductsCount} to row {rowsToAdd.Count - 1 - i}");
                    }
                    
                    // Cập nhật lại STT cho tất cả các dòng
                    for (int i = 0; i < _gvDetail.Rows.Count; i++)
                    {
                        if (_gvDetail.Rows[i].IsNewRow) continue;
                        _gvDetail.Rows[i].Cells["STT"].Value = i + 1;
                    }
                    
                    // Đảm bảo luôn có một hàng trống ở cuối để nhập sản phẩm mới
                    try
                    {
                        // Kiểm tra xem dòng cuối cùng có phải dòng trống không
                        bool hasEmptyLastRow = false;
                        int lastRowIndex = _gvDetail.Rows.Count - 1;
                        
                        if (lastRowIndex >= 0 && !_gvDetail.Rows[lastRowIndex].IsNewRow)
                        {
                            object barcodeValue = _gvDetail.Rows[lastRowIndex].Cells["BARCODE"].Value;
                            hasEmptyLastRow = barcodeValue == null || string.IsNullOrEmpty(barcodeValue.ToString());
                        }
                        
                        // Nếu không có dòng trống, thêm mới
                        if (!hasEmptyLastRow)
                        {
                            int emptyRowIndex = _gvDetail.Rows.Add();
                            _gvDetail.Rows[emptyRowIndex].Cells["STT"].Value = _gvDetail.Rows.Count;
                            Console.WriteLine($"Added empty row at index {emptyRowIndex}");
                        }
                        
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
