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
using BusinessLayer.Utils;
using BusinessLayer;
using DataLayer;
using MATERIAL.MyFunctions;
using STOCK.StockHelpers;
using STOCK.PopUpForm;
using Excel = Microsoft.Office.Interop.Excel;

namespace STOCK.Controls
{
    public partial class InternalDeliveryControl : UserControl
    {
        public InternalDeliveryControl()
        {
            InitializeComponent();
        }

        public InternalDeliveryControl(tb_SYS_USER user, int right) 
        {
            InitializeComponent();
            this._user = user;
            this._right = right;
        }

        tb_SYS_USER _user;
        int _right; 
        bool _add;
        bool _edit = false;
        bool _import;
        bool _isImport;

        List<string> _lstBarcode;
        List<string> _lstQrcode;
        List<_STATUS> _status;
        List<tb_Invoice> _lstInvoice;

        string err = "";

        COMPANY _company;
        DEPARTMENT _department;
        PRODUCT _product;
        INVOICE_DETAIL _invoiceDetail;
        INVOICE _invoice;
        SUPPLIER _supplier;
        BindingSource _bsInvoiceDT;
        BindingSource _bsInvoice;

        Guid _id;
        Guid _pinvoiceID;

        tb_SYS_SEQ _seq;
        SYS_SEQ _sequence;

        private void InternalDeliveryControl_Load(object sender, EventArgs e)
        {
            gvList.AutoGenerateColumns = false;
            gvDetail.AutoGenerateColumns = false;

            _import = false;
            _lstBarcode = new List<string>();
            _lstQrcode = new List<string>();
            _supplier = new SUPPLIER();
            _company = new COMPANY();
            _department = new DEPARTMENT();
            _product = new PRODUCT();
            _invoice = new INVOICE();
            _invoiceDetail = new INVOICE_DETAIL();
            _sequence = new SYS_SEQ();
            _bsInvoice = new BindingSource();
            _bsInvoiceDT = new BindingSource();

            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTill.Value = DateTime.Now;

            _bsInvoice.PositionChanged += _bsInvoice_PositionChanged;
            LoadCompany();
            cboCompany.SelectedValue = "DEMO01";
            cboCompany.SelectedIndexChanged += CboCompany_SelectedIndexChanged;

            _status = _STATUS.getList();
            cboStatus.DataSource = _status;
            cboStatus.DisplayMember = "_display";
            cboStatus.ValueMember = "_value";

            gvList.CellFormatting += GvList_CellFormatting; ;
            gvList.CellPainting += GvList_CellPainting; ;

            LoadDepartment();
            LoadReceiveUnit();
            _lstInvoice = _invoice.getList(2, dtFrom.Value, dtTill.Value.AddDays(1), cboDepartment.SelectedValue.ToString());
            _bsInvoice.DataSource = _lstInvoice;
            gvList.DataSource = _bsInvoice;

            exportInfor();
            cboExportUnit.SelectedIndexChanged += CboExportUnit_SelectedIndexChanged;
            cboDepartment.SelectedIndexChanged += CboDepartment_SelectedIndexChanged;
            gvDetail.CellMouseDown += GvDetail_CellMouseDown;
            gvDetail.CellValueChanged += GvDetail_CellValueChanged;
            gvDetail.CellEndEdit += GvDetail_CellEndEdit;
            CmdDeleteRow.Click += CmdDeleteRow_Click;
            CmdDeleteDetail.Click += CmdDeleteDetail_Click;

            // Set fixed row header width (optional)
            gvList.RowHeadersWidth = 25;
            gvDetail.RowHeadersWidth = 25;

            _enable(false);
            ShowHideControls(true);
            contextMenuStrip.Enabled = false;
            gvList.ClearSelection();
        }

        private void _bsInvoice_PositionChanged(object sender, EventArgs e)
        {
            if (!_add)
            {
                exportInfor();
            }
        }

        void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }

        void LoadDepartment()
        {
            cboDepartment.DataSource = _department.getWarehoseByCp(cboCompany.SelectedValue.ToString());
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentID";
        }

        void LoadReceiveUnit()
        {
            cboReceiveUnit.DataSource = _department.getDepartmentByCp(cboCompany.SelectedValue.ToString(), false);
            cboReceiveUnit.DisplayMember = "DepartmentName";
            cboReceiveUnit.ValueMember = "DepartmentID";
        }

        void ShowHideControls(bool t)
        {
            btnAdd.Visible = t;
            btnEdit.Visible = t;
            btnDelete.Visible = t;
            btnSave.Visible = !t;
            btnCancel.Visible = !t;
        }

        private void _enable(bool t)
        {
            txtNote.Enabled = t;
            cboExportUnit.Enabled = t;
            cboStatus.Enabled = t;
            cboReceiveUnit.Enabled = t;
            dtDate.Enabled = t;
        }

        private void ResetFields()
        {
            txtInvoiceNo.Text = "";
            txtNote.Text = "";
        }
        private void GvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                string columnName = gvDetail.Columns[e.ColumnIndex].Name;

                // When Price or Quantity is edited, recalculate SubTotal
                if (columnName.Equals("QuantityDetail", StringComparison.OrdinalIgnoreCase) ||
                    columnName.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["QuantityDetail"]?.Value;
                    object priceValue = gvDetail.Rows[e.RowIndex].Cells["Price"]?.Value;

                    if (quantityValue != null && priceValue != null &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        double.TryParse(priceValue.ToString(), out double price))
                    {
                        double totalPrice = quantity * price;
                        gvDetail.Rows[e.RowIndex].Cells["SubTotal"].Value = totalPrice;
                        Console.WriteLine($"Updated SubTotal in cell edit: {totalPrice} = {quantity} * {price}");
                    }
                }
                // When SubTotal is edited directly, update Price (assuming Quantity stays the same)
                else if (columnName.Equals("SubTotal", StringComparison.OrdinalIgnoreCase))
                {
                    object totalPriceValue = gvDetail.Rows[e.RowIndex].Cells["SubTotal"]?.Value;
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["QuantityDetail"]?.Value;

                    if (totalPriceValue != null && quantityValue != null &&
                        double.TryParse(totalPriceValue.ToString(), out double totalPrice) &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        quantity > 0)
                    {
                        double price = totalPrice / quantity;
                        gvDetail.Rows[e.RowIndex].Cells["Price"].Value = price;
                        Console.WriteLine($"Updated Price based on SubTotal: {price} = {totalPrice} / {quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in gvDetail_CellEndEdit: {ex.Message}");
            }
        }

        private void GvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isImport) return; // Nếu đang nhập dữ liệu, bỏ qua
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Check valid cell

            try
            {
                // Lấy tên cột thay vì FieldName (DevExpress)
                string columnName = gvDetail.Columns[e.ColumnIndex].Name;

                // Kiểm tra nếu thay đổi cột "BARCODE"
                if (columnName == "BARCODE")
                {
                    if (e.RowIndex >= gvDetail.Rows.Count) return;

                    object barcodeValue = null;
                    try
                    {
                        barcodeValue = gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    }
                    catch { return; }

                    if (barcodeValue == null) return;

                    string barcode = barcodeValue.ToString();

                    if (barcode.StartsWith(".")) // Nếu barcode bắt đầu bằng "."
                    {
                        _isImport = true;
                        try
                        {
                            // Remember the original value
                            string originalValue = barcode;

                            // Pass the current row index to formList
                            formList _popList = new formList(gvDetail, barcode, e.RowIndex);
                            _popList.ShowDialog();

                            // Debug gridview để xem các sản phẩm đã được thêm vào
                            DebugDetailRows();

                            // Đảm bảo SubTotal được tính đúng sau khi thêm sản phẩm
                            AutoUpdateTotalPriceForBoundGrid();

                            // Clear the "." if it's still there after closing the form
                            if (e.RowIndex < gvDetail.Rows.Count && !gvDetail.Rows[e.RowIndex].IsNewRow)
                            {
                                if (gvDetail.Rows[e.RowIndex].Cells["BARCODE"].Value?.ToString() == originalValue)
                                {
                                    gvDetail.Rows[e.RowIndex].Cells["BARCODE"].Value = null;
                                }
                            }

                            // After the form closes, safely recalculate dependent values
                            try
                            {
                                // Check if columns exist first using column index method which is safer
                                int quantityColIndex = -1, priceColIndex = -1, totalPriceColIndex = -1;

                                for (int i = 0; i < gvDetail.Columns.Count; i++)
                                {
                                    if (gvDetail.Columns[i].Name.Equals("QuantityDetail", StringComparison.OrdinalIgnoreCase))
                                        quantityColIndex = i;
                                    else if (gvDetail.Columns[i].Name.Equals("Price", StringComparison.OrdinalIgnoreCase))
                                        priceColIndex = i;
                                    else if (gvDetail.Columns[i].Name.Equals("SubTotal", StringComparison.OrdinalIgnoreCase))
                                        totalPriceColIndex = i;
                                }

                                if (quantityColIndex >= 0 && priceColIndex >= 0 && totalPriceColIndex >= 0 &&
                                    e.RowIndex < gvDetail.Rows.Count) // Make sure row still exists
                                {
                                    var quantityValue = gvDetail.Rows[e.RowIndex].Cells[quantityColIndex].Value;
                                    var priceValue = gvDetail.Rows[e.RowIndex].Cells[priceColIndex].Value;

                                    if (quantityValue != null && priceValue != null &&
                                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                                        double.TryParse(priceValue.ToString(), out double price))
                                    {
                                        gvDetail.Rows[e.RowIndex].Cells[totalPriceColIndex].Value = quantity * price;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error updating SubTotal after form close: {ex.Message}");
                                // Don't show error to user, just log it
                            }
                        }
                        finally
                        {
                            _isImport = false;
                            gvDetail.Refresh();
                        }
                    }
                    else
                    {
                        tb_Product pd = _product.getItemBarcode(barcode);
                        if (pd != null)
                        {
                            // Check if this barcode already exists in a different row
                            bool barcodeExists = false;
                            for (int i = 0; i < gvDetail.Rows.Count; i++)
                            {
                                if (i == e.RowIndex || gvDetail.Rows[i].IsNewRow) continue; // Skip current row and new rows

                                if (gvDetail.Rows[i].Cells["BARCODE"].Value?.ToString() == barcode)
                                {
                                    barcodeExists = true;
                                    break;
                                }
                            }

                            if (barcodeExists)
                            {
                                MessageBox.Show("This product has already been added to the list", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                gvDetail.Rows[e.RowIndex].Cells["BARCODE"].Value = ""; // Clear the value
                            }
                            else
                            {
                                DataGridViewRow row = gvDetail.Rows[e.RowIndex];
                                row.Cells["ProductName"].Value = pd.ProductName;
                                row.Cells["Unit"].Value = pd.Unit;
                                row.Cells["QuantityDetail"].Value = 1;
                                row.Cells["Price"].Value = pd.Price;
                                row.Cells["SubTotal"].Value = pd.Price;

                                // If this is the last row, add a new row for convenience
                                if (e.RowIndex == gvDetail.Rows.Count - 1 && !gvDetail.Rows[e.RowIndex].IsNewRow)
                                {
                                    gvDetail.Rows.Add();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("This product ID does not exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            gvDetail.Rows[e.RowIndex].Cells["BARCODE"].Value = ""; // Clear the value
                            return;
                        }
                        gvDetail.Refresh();
                    }
                }

                // Kiểm tra nếu thay đổi cột "Quantity" hoặc "Price"
                if (columnName == "QuantityDetail" || columnName == "Price")
                {
                    object barcodeValue = gvDetail.Rows[e.RowIndex].Cells["BARCODE"]?.Value;
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["QuantityDetail"]?.Value;
                    object priceValue = gvDetail.Rows[e.RowIndex].Cells["Price"]?.Value;

                    if (barcodeValue != null && quantityValue != null && priceValue != null)
                    {
                        if (double.TryParse(quantityValue.ToString(), out double quantity) &&
                            double.TryParse(priceValue.ToString(), out double price))
                        {
                            if (quantity == 0 && columnName == "QuantityDetail")
                            {
                                MessageBox.Show("Số lượng sản phẩm không thể bằng 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                gvDetail.Rows[e.RowIndex].Cells["QuantityDetail"].Value = 1;
                                quantity = 1;
                            }

                            // Calculate and update total price
                            double totalPrice = price * quantity;
                            gvDetail.Rows[e.RowIndex].Cells["SubTotal"].Value = totalPrice;

                            Console.WriteLine($"Updated SubTotal for row {e.RowIndex}: {totalPrice} = {price} * {quantity}");
                        }
                        else
                        {
                            MessageBox.Show("Giá trị nhập vào không phải là số hợp lệ.", "Thông báo");
                        }
                    }
                    gvDetail.Refresh();
                }

                // After cell value changes, ensure there's an empty row at the bottom
                // Don't call this during import to avoid recursion
                if (!_isImport && (_add || _edit))
                {
                    EnsureEmptyRowAtBottom();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in gvDetail_CellValueChanged: {ex.Message}");
                // Don't show error to user, just log it
            }
        }

        private void DebugDetailRows()
        {
            try
            {
                // Đếm số hàng có dữ liệu thực sự
                int validRows = 0;
                List<string> productBarcodes = new List<string>();

                Console.WriteLine("\n==== DEBUG DETAIL ROWS ====");

                // Tìm các cột cần thiết
                int barcodeColIndex = -1, productNameColIndex = -1, quantityColIndex = -1;

                for (int i = 0; i < gvDetail.Columns.Count; i++)
                {
                    if (gvDetail.Columns[i].Name.Equals("BARCODE", StringComparison.OrdinalIgnoreCase))
                        barcodeColIndex = i;
                    else if (gvDetail.Columns[i].Name.Equals("ProductName", StringComparison.OrdinalIgnoreCase))
                        productNameColIndex = i;
                    else if (gvDetail.Columns[i].Name.Equals("QuantityDetail", StringComparison.OrdinalIgnoreCase))
                        quantityColIndex = i;
                }

                if (barcodeColIndex < 0 || productNameColIndex < 0)
                {
                    Console.WriteLine("Cannot find required columns for debugging.");
                    return;
                }

                // Kiểm tra từng dòng
                for (int i = 0; i < gvDetail.Rows.Count; i++)
                {
                    if (gvDetail.Rows[i].IsNewRow) continue;

                    string barcode = gvDetail.Rows[i].Cells[barcodeColIndex].Value?.ToString();
                    string productName = gvDetail.Rows[i].Cells[productNameColIndex].Value?.ToString();

                    if (!string.IsNullOrEmpty(barcode) && !string.IsNullOrEmpty(productName))
                    {
                        validRows++;
                        productBarcodes.Add(barcode);

                        // In thông tin chi tiết của dòng hợp lệ
                        string rowInfo = $"Row {i} - BARCODE: {barcode}, Product: {productName}";
                        if (quantityColIndex >= 0)
                        {
                            var qty = gvDetail.Rows[i].Cells[quantityColIndex].Value;
                            rowInfo += $", Quantity: {qty}";
                        }
                        Console.WriteLine(rowInfo);
                    }
                    else
                    {
                        Console.WriteLine($"Row {i} - INVALID/EMPTY: Barcode={barcode}, ProductName={productName}");
                    }
                }

                Console.WriteLine($"Total rows in grid: {gvDetail.Rows.Count}");
                Console.WriteLine($"Valid product rows: {validRows}");
                Console.WriteLine($"Products to be saved: {string.Join(", ", productBarcodes)}");
                Console.WriteLine("==== END DEBUG ====\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DebugDetailRows: {ex.Message}");
            }
        }
        private void AutoUpdateTotalPriceForBoundGrid()
        {
            try
            {
                if (gvDetail.DataSource is BindingSource bs)
                {
                    if (bs.DataSource is List<obj_INVOICE_DETAIL> detailList)
                    {
                        foreach (var item in detailList)
                        {
                            // Update total price if we have quantity and price
                            if (item.Quantity.HasValue && item.Price.HasValue)
                            {
                                item.SubTotal = item.Quantity.Value * item.Price.Value;
                            }
                        }
                        bs.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating TotalPrice in bound grid: {ex.Message}");
            }
        }

        private void GvDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Kiểm tra chuột phải & hàng hợp lệ
            {
                gvDetail.ClearSelection(); // Xóa chọn cũ
                gvDetail.Rows[e.RowIndex].Selected = true; // Chọn hàng hiện tại
                gvDetail.CurrentCell = gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex]; // Đặt ô hiện tại

                // Hiển thị context menu tại vị trí chuột
                contextMenuStrip.Show(Cursor.Position);
            }
        }

        private void CboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            _lstInvoice = _invoice.getList(2, dtFrom.Value, dtTill.Value.AddDays(1), cboDepartment.SelectedValue.ToString());
            _bsInvoice.DataSource = _lstInvoice;
            gvList.DataSource = _bsInvoice;
            exportInfor();
        }

        private void CboExportUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GvList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (gvList.Columns[e.ColumnIndex].Name == "DELETED_BY" && e.Value != null)
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

            //    Image img = Properties.Resources.trash.png; // Ảnh xóa
            //    int imgX = e.CellBounds.X + (e.CellBounds.Width - img.Width) / 2;
            //    int imgY = e.CellBounds.Y + (e.CellBounds.Height - img.Height) / 2;

            //    e.Graphics.DrawImage(img, new Point(imgX, imgY));
            //    e.Handled = true;
            //}
        }

        private void GvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gvList.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value != null)
                {
                    // Kiểm tra trên chuỗi hoặc trên số
                    if (e.Value.ToString() == "1")
                    {
                        e.Value = "Not Complete";
                    }
                    else if (e.Value.ToString() == "2")
                    {
                        e.Value = "Confirm";
                    }

                    // Đánh dấu rằng đã xử lý việc định dạng
                    e.FormattingApplied = true;

                    // Log để kiểm tra
                    Console.WriteLine($"Status value: {e.Value} (original: {gvList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value})");
                }
            }
        }

        private void CboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_right == 1)
            {
                MessageBox.Show("Không có quyền thao tác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _bsInvoiceDT.DataSource = _invoiceDetail.getListbyIDFull(_id);
            gvDetail.DataSource = _bsInvoiceDT;

            if (_bsInvoiceDT.DataSource is List<obj_INVOICE_DETAIL> detailList)
            {
                obj_INVOICE_DETAIL newDetail = new obj_INVOICE_DETAIL();
                newDetail.STT = detailList.Count + 1;
                detailList.Add(newDetail);

                _bsInvoiceDT.ResetBindings(false);
            }
            else if (_bsInvoiceDT.DataSource is DataTable dt)
            {
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            else
            {
                gvDetail.Rows.Add();
                if (gvDetail.Rows.Count > 0)
                {
                    int lastRowIndex = gvDetail.Rows.Count - 1;
                    if (!gvDetail.Rows[lastRowIndex].IsNewRow)
                    {
                        gvDetail.Rows[lastRowIndex].Cells["STT"].Value = lastRowIndex + 1;
                    }
                }
            }

            tabInvoice.SelectedTab = pageDetail;
            gvDetail.ReadOnly = false;
            contextMenuStrip.Enabled = true;

            _add = true;
            _edit = false;
            ShowHideControls(false);
            _enable(true);
            ResetFields();

            // Đặt trạng thái mặc định là "Not Complete" (giá trị 1)
            cboStatus.SelectedIndex = 0;

            // Always ensure there's at least one empty row at the bottom
            EnsureEmptyRowAtBottom();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_right == 1)
            {
                MessageBox.Show("You do not have permission to perform this operation", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                tb_Invoice current = (tb_Invoice)_bsInvoice.Current;
                if (current.Status == 1)
                {
                    _add = false;
                    _edit = true;
                    ShowHideControls(false);
                    _enable(true);
                    tabInvoice.SelectedTab = pageDetail;
                    tabInvoice.TabPages[0].Enabled = false; // Disable the first tab page
                    gvDetail.ReadOnly = false; // Enable editing in the DataGridView
                    contextMenuStrip.Enabled = true;
                    cboExportUnit.Enabled = false;

                    // Thêm một dòng trống mới để người dùng có thể thêm sản phẩm
                    if (gvDetail.DataSource is BindingSource bs)
                    {
                        // Nếu DataGridView được liên kết với BindingSource
                        if (bs.DataSource is List<obj_INVOICE_DETAIL> detailList)
                        {
                            // Tạo một đối tượng detail mới và thêm vào danh sách
                            obj_INVOICE_DETAIL newDetail = new obj_INVOICE_DETAIL();
                            newDetail.STT = detailList.Count + 1;
                            newDetail.InvoiceID = current.InvoiceID;
                            detailList.Add(newDetail);

                            // Cập nhật lại binding
                            bs.ResetBindings(false);
                        }
                    }
                    else
                    {
                        // Nếu DataGridView không liên kết với BindingSource, thêm một dòng trực tiếp
                        gvDetail.Rows.Add();
                    }

                    // Always ensure there's at least one empty row at the bottom
                    EnsureEmptyRowAtBottom();

                    if (gvDetail.Rows.Count == 0)
                    {
                        List<V_INVOICE_DETAIL> _lstDetail = new List<V_INVOICE_DETAIL>();
                        _bsInvoice.DataSource = _lstDetail;
                        gvDetail.DataSource = _bsInvoice;
                        gvDetail.Rows.Add(); // Add a new row to the DataGridView
                        gvDetail.Rows[gvDetail.Rows.Count - 1].Cells["STT"].Value = 1; // Set the value of the "STT" column in the newly added row
                    }
                }
                else
                {
                    MessageBox.Show("This invoice has been approved, you cannot edit it", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_right == 1)
            {
                MessageBox.Show("You do not have permission", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (gvList.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = gvList.SelectedRows[0];
                    Guid invoiceID = (Guid)row.Cells["InvoiceID"].Value;

                    if (MessageBox.Show("Do you want to delete this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        // Gọi phương thức delete trong INVOICE với UserID = 1
                        _invoice.delete(invoiceID, 1);

                        // Cập nhật UI để hiển thị trạng thái đã xóa
                        row.Cells["DeletedBy"].Value = 1;
                        lblDelete.Visible = true;
                        btnDelete.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a record to delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
            _add = false;
            _edit = false;
            gvDetail.ReadOnly = true;
            contextMenuStrip.Enabled = false;
            tabInvoice.TabPages[0].Enabled = true;
            ShowHideControls(true);
            _enable(false);
            tabInvoice.SelectedTab = pageList;
        }
        private void UpdateAllTotalPrices()
        {
            DataGridViewHelper.UpdateAllTotalPrices(gvDetail);
        }
        private void saveData()
        {
            err = "";

            tb_Invoice invoice;
            err += "Please enter the details of the invoice\r\n";
            if (!FormValidationHelper.ValidateGridHasData(gvDetail))
            {
                return;
            }

            // Calculate all total prices before saving to ensure they're correct
            DataGridViewHelper.UpdateAllTotalPrices(gvDetail);

            if (_add)
            {
                invoice = new tb_Invoice();
                Invoice_Infor(invoice);
                var resultInvoice = _invoice.add(invoice);
                _sequence.udpate(_seq);

                InvoiceDetail_Infor(resultInvoice);

                // Add to binding source and refresh the view
                if (_bsInvoice.DataSource is List<tb_Invoice> invoiceList)
                {
                    invoiceList.Add(resultInvoice);
                    _bsInvoice.ResetBindings(false);
                    _bsInvoice.Position = invoiceList.Count - 1; // Move to last item
                }
                else
                {
                    List<tb_Invoice> newList = new List<tb_Invoice> { resultInvoice };
                    _bsInvoice.DataSource = newList;
                }
            }
            else
            {
                invoice = (tb_Invoice)_bsInvoice.Current;
                invoice = _invoice.getItem(invoice.InvoiceID);
                Invoice_Infor(invoice);
                var resultInvoice = _invoice.update(invoice);
                InvoiceDetail_Infor(resultInvoice);

                // Refresh the data in the grid
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboDepartment.SelectedValue.ToString());
                _bsInvoice.DataSource = _lstInvoice;

                // Find and select the updated invoice
                for (int i = 0; i < _lstInvoice.Count; i++)
                {
                    if (_lstInvoice[i].Invoice == resultInvoice.Invoice)
                    {
                        _bsInvoice.Position = i;
                        break;
                    }
                }

                // Refresh the display
                gvList.Refresh();
            }

            exportInfor();
            _add = false;

            tabInvoice.SelectedTab = pageList;
        }

        void InvoiceDetail_Infor(tb_Invoice invoice)
        {
            // First delete all existing details for this invoice
            _invoiceDetail.deleteAllByInvoiceId(invoice.InvoiceID);

            List<tb_InvoiceDetail> detailsToAdd = new List<tb_InvoiceDetail>();

            // Find all column indices needed to safely access cells
            Dictionary<string, int> columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // List of columns we need to find (case-insensitive)
            string[] requiredColumns = new string[] {
                "BARCODE", "ProductID", "ProductName", "QuantityDetail", "Price", "SubTotal", "Unit"
            };

            // Find all column indices first (case-insensitive)
            for (int col = 0; col < gvDetail.Columns.Count; col++)
            {
                string colName = gvDetail.Columns[col].Name;

                foreach (string requiredCol in requiredColumns)
                {
                    if (colName.Equals(requiredCol, StringComparison.OrdinalIgnoreCase))
                    {
                        columnIndices[requiredCol] = col;
                        break;
                    }
                }
            }

            // Debug column indices
            Console.WriteLine("Column indices found:");
            foreach (var kvp in columnIndices)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            // Process each row
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                DataGridViewRow row = gvDetail.Rows[i];

                // Skip new/empty rows
                if (row.IsNewRow) continue;

                // Skip empty rows
                if (columnIndices.TryGetValue("BARCODE", out int barcodeColIdx) &&
                    (row.Cells[barcodeColIdx].Value == null ||
                     string.IsNullOrWhiteSpace(row.Cells[barcodeColIdx].Value.ToString())))
                {
                    Console.WriteLine($"Skipping row {i} - Empty BARCODE");
                    continue;
                }

                // Skip rows that just have a "." in the BARCODE field
                if (columnIndices.TryGetValue("BARCODE", out int barcodeIdx) &&
                    row.Cells[barcodeIdx].Value?.ToString() == ".")
                {
                    Console.WriteLine($"Skipping row {i} - BARCODE contains only '.'");
                    continue;
                }

                // Skip rows without product name
                if (columnIndices.TryGetValue("ProductName", out int productNameIdx) &&
                    (row.Cells[productNameIdx].Value == null ||
                     string.IsNullOrWhiteSpace(row.Cells[productNameIdx].Value.ToString())))
                {
                    Console.WriteLine($"Skipping row {i} - Empty ProductName");
                    continue;
                }

                try
                {
                    tb_InvoiceDetail detail = new tb_InvoiceDetail
                    {
                        InvoiceDetail_ID = Guid.NewGuid(),
                        InvoiceID = invoice.InvoiceID,
                        STT = i + 1,
                        Day = dtDate.Value
                    };

                    // Get BARCODE
                    if (columnIndices.TryGetValue("BARCODE", out int barIdx) &&
                        row.Cells[barIdx].Value != null)
                    {
                        detail.BARCODE = row.Cells[barIdx].Value.ToString();
                        Console.WriteLine($"Row {i} has BARCODE: {detail.BARCODE}");
                    }
                    else
                    {
                        Console.WriteLine($"Row {i} skipped - invalid BARCODE");
                        continue; // Skip rows without valid barcode
                    }

                    // Get Quantity
                    if (columnIndices.TryGetValue("QuantityDetail", out int qtyIdx) && row.Cells[qtyIdx].Value != null)
                    {
                        if (int.TryParse(row.Cells[qtyIdx].Value.ToString(), out int qty))
                            detail.Quantity = qty;
                        else
                            detail.Quantity = 1;
                    }
                    else
                        detail.Quantity = 1;

                    // Get Price
                    if (columnIndices.TryGetValue("Price", out int priceIdx) && row.Cells[priceIdx].Value != null)
                    {
                        if (double.TryParse(row.Cells[priceIdx].Value.ToString(), out double price))
                            detail.Price = price;
                        else
                        {
                            // Get price from product database if available
                            if (!string.IsNullOrEmpty(detail.BARCODE))
                            {
                                var product = _product.getItemBarcode(detail.BARCODE);
                                detail.Price = product?.Price ?? 0;
                            }
                            else
                                detail.Price = 0;
                        }
                    }
                    else
                    {
                        // Get price from product database if available
                        if (!string.IsNullOrEmpty(detail.BARCODE))
                        {
                            var product = _product.getItemBarcode(detail.BARCODE);
                            detail.Price = product?.Price ?? 0;
                        }
                        else
                            detail.Price = 0;
                    }

                    // Get SubTotal
                    if (columnIndices.TryGetValue("SubTotal", out int totalPriceIdx) && row.Cells[totalPriceIdx].Value != null)
                    {
                        if (double.TryParse(row.Cells[totalPriceIdx].Value.ToString(), out double totalPrice))
                            detail.SubTotal = totalPrice;
                        else
                            detail.SubTotal = detail.Price * detail.Quantity; // Calculate if parsing fails
                    }
                    else
                        detail.SubTotal = detail.Price * detail.Quantity; // Calculate if column doesn't exist

                    // Get ProductID
                    if (columnIndices.TryGetValue("ProductID", out int productIdIdx) && row.Cells[productIdIdx].Value != null)
                    {
                        if (int.TryParse(row.Cells[productIdIdx].Value.ToString(), out int productId))
                            detail.ProductID = productId;
                        else if (!string.IsNullOrEmpty(detail.BARCODE))
                        {
                            // Get product ID from barcode if possible
                            var product = _product.getItemBarcode(detail.BARCODE);
                            if (product != null)
                                detail.ProductID = product.ProductID;
                        }
                    }
                    else if (!string.IsNullOrEmpty(detail.BARCODE))
                    {
                        // Get product ID from barcode if column doesn't exist
                        var product = _product.getItemBarcode(detail.BARCODE);
                        if (product != null)
                            detail.ProductID = product.ProductID;
                    }

                    // Debug the values being saved
                    Console.WriteLine($"Row {i} detail: BARCODE={detail.BARCODE}, Qty={detail.Quantity}, " +
                                      $"Price={detail.Price}, TotalPrice={detail.SubTotal}, ProductID={detail.ProductID}");

                    detailsToAdd.Add(detail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing row {i}: {ex.Message}");
                    // Continue with the next row instead of failing the entire operation
                }
            }

            // Add all details at once for better performance
            if (detailsToAdd.Count > 0)
            {
                _invoiceDetail.addRange(detailsToAdd);
                Console.WriteLine($"Added {detailsToAdd.Count} invoice details to database");
            }
            else
            {
                Console.WriteLine("No invoice details to add to database");
            }

            // Hàm kiểm tra và debug số lượng sản phẩm trong gvDetail
            DebugDetailRows();
        }
        void Invoice_Infor(tb_Invoice invoice)
        {
            double _total = 0;
            tb_Department dp = _department.getItem(cboExportUnit.SelectedValue.ToString());
            _seq = _sequence.getItem("XNB@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
            if (_seq == null)
            {
                _seq = new tb_SYS_SEQ();
                _seq.SEQNAME = "XNB@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol;
                _seq.SEQVALUE = 1;
                _sequence.add(_seq);
            }

            if (_add)
            {
                invoice.InvoiceID = Guid.NewGuid();
                invoice.Day = dtDate.Value;
                invoice.Invoice = _seq.SEQVALUE.Value.ToString("000000") + "/" + DateTime.Today.Year.ToString().Substring(2, 2) + "/XNB/" + dp.Symbol;
                invoice.CreatedBy = 1;
                invoice.CreatedDate = DateTime.Now;
            }

            invoice.InvoiceType = 2;
            invoice.CompanyID = cboCompany.SelectedValue.ToString();
            invoice.DepartmentID = cboExportUnit.SelectedValue.ToString();
            invoice.ReceivingDepartmentID = cboReceiveUnit.SelectedValue.ToString();
            invoice.Description = txtNote.Text;
            invoice.Status = Convert.ToInt32(cboStatus.SelectedValue);

            // Calculate total quantity - with robust column checking
            int totalQuantity = 0;

            // First, find the quantity column index to avoid using string indexer
            int quantityColIndex = -1;
            for (int i = 0; i < gvDetail.Columns.Count; i++)
            {
                if (gvDetail.Columns[i].Name.Equals("QuantityDetail", StringComparison.OrdinalIgnoreCase))
                {
                    quantityColIndex = i;
                    break;
                }
            }

            // Only proceed if we found the column
            if (quantityColIndex >= 0)
            {
                foreach (DataGridViewRow row in gvDetail.Rows)
                {
                    if (row.IsNewRow) continue;

                    try
                    {
                        object quantityValue = row.Cells[quantityColIndex].Value;
                        if (quantityValue != null && int.TryParse(quantityValue.ToString(), out int qty))
                        {
                            totalQuantity += qty;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error calculating quantity: {ex.Message}");
                        // Continue processing other rows
                    }
                }
            }

            invoice.Quantity = totalQuantity;

            // Calculate total price from detail grid - also with robust column checking
            int totalPriceColIndex = -1;
            for (int i = 0; i < gvDetail.Columns.Count; i++)
            {
                if (gvDetail.Columns[i].Name.Equals("SubTotal", StringComparison.OrdinalIgnoreCase))
                {
                    totalPriceColIndex = i;
                    break;
                }
                else if (gvDetail.Columns[i].Name.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
                {
                    totalPriceColIndex = i;
                    break;
                }
            }

            if (totalPriceColIndex >= 0)
            {
                for (int i = 0; i < gvDetail.Rows.Count; i++)
                {
                    if (gvDetail.Rows[i].IsNewRow) continue;

                    try
                    {
                        object totalPriceValue = gvDetail.Rows[i].Cells[totalPriceColIndex].Value;
                        if (totalPriceValue == null)
                        {
                            gvDetail.Rows.RemoveAt(i);
                            i--; // Adjust index since we removed a row
                        }
                        else if (double.TryParse(totalPriceValue.ToString(), out double rowTotal))
                        {
                            _total += rowTotal;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error calculating total price: {ex.Message}");
                        // Continue with the next row
                    }
                }
            }

            invoice.TotalPrice = _total;
            invoice.UpdatedBy = 1;
            invoice.UpdatedDate = DateTime.Now;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _edit = false;
            ShowHideControls(true);
            _enable(false);

            ResetFields();
            exportInfor();

            tabInvoice.TabPages[0].Enabled = true;
            tabInvoice.SelectedTab = pageList;

            gvDetail.ReadOnly = true;
            contextMenuStrip.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        private void EnsureEmptyRowAtBottom()
        {
            // Only do this in add or edit mode
            if (!_add && !_edit)
                return;

            // Use the utility method from DataGridViewHelper
            DataGridViewHelper.EnsureEmptyRowAtBottom(gvDetail);
        }
        void exportInfor()
        {
            tb_Invoice current = (tb_Invoice)_bsInvoice.Current;
            if (current != null)
            {
                tb_Department dp = _department.getItem(current.DepartmentID);
                dtDate.Value = current.Day.Value;
                txtInvoiceNo.Text = current.Invoice;
                txtNote.Text = current.Description;
                cboExportUnit.SelectedValue = current.DepartmentID;
                cboReceiveUnit.SelectedValue = int.Parse(current.ReceivingDepartmentID);
                cboStatus.SelectedValue = current.Status;

                if (current.DeletedBy != null)
                {
                    lblDelete.Visible = true;
                    btnDelete.Enabled = false;
                }
                else
                {
                    lblDelete.Visible = false;
                    btnDelete.Enabled = true;
                }

                // Get the invoice details with product information included
                var detailsWithProductInfo = _invoiceDetail.getListbyIDFull(current.InvoiceID);
                _bsInvoiceDT.DataSource = detailsWithProductInfo;
                gvDetail.DataSource = _bsInvoiceDT;

                // Ensure all rows have complete data by looking up missing product information
                for (int i = 0; i < gvDetail.Rows.Count; i++)
                {
                    if (gvDetail.Rows[i].IsNewRow) continue;

                    try
                    {
                        string barcode = null;
                        int? productId = null;

                        // Try to get either the barcode or product ID
                        if (gvDetail.Columns.Contains("BARCODE") && gvDetail.Rows[i].Cells["BARCODE"].Value != null)
                        {
                            barcode = gvDetail.Rows[i].Cells["BARCODE"].Value.ToString();
                        }
                        else if (gvDetail.Columns.Contains("ProductID") && gvDetail.Rows[i].Cells["ProductID"].Value != null)
                        {
                            int.TryParse(gvDetail.Rows[i].Cells["ProductID"].Value.ToString(), out int pid);
                            productId = pid;
                        }

                        if (!string.IsNullOrEmpty(barcode) || productId.HasValue)
                        {
                            // Get full product details
                            tb_Product product = null;
                            if (!string.IsNullOrEmpty(barcode))
                            {
                                product = _product.getItemBarcode(barcode);
                            }
                            else if (productId.HasValue)
                            {
                                product = _product.getItem(productId.Value);
                            }

                            if (product != null)
                            {
                                // Fill in missing data
                                if (gvDetail.Columns.Contains("ProductName") &&
                                    (gvDetail.Rows[i].Cells["ProductName"].Value == null ||
                                     string.IsNullOrEmpty(gvDetail.Rows[i].Cells["ProductName"].Value.ToString())))
                                {
                                    gvDetail.Rows[i].Cells["ProductName"].Value = product.ProductName;
                                }

                                if (gvDetail.Columns.Contains("Unit") &&
                                    (gvDetail.Rows[i].Cells["Unit"].Value == null ||
                                     string.IsNullOrEmpty(gvDetail.Rows[i].Cells["Unit"].Value.ToString())))
                                {
                                    gvDetail.Rows[i].Cells["Unit"].Value = product.Unit;
                                }

                                if (gvDetail.Columns.Contains("Price") &&
                                    (gvDetail.Rows[i].Cells["Price"].Value == null))
                                {
                                    gvDetail.Rows[i].Cells["Price"].Value = product.Price;
                                }

                                // Set STT value
                                gvDetail.Rows[i].Cells["STT"].Value = i + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading row {i} info: {ex.Message}");
                    }
                }

                // Refresh display
                gvDetail.Refresh();
            }
        }

        private void gvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (gvDetail.ReadOnly) return; // Không làm gì nếu DataGridView không cho phép chỉnh sửa

            _isImport = false;

            // Nhấn phím ↓ (Down)
            if (e.KeyData == Keys.Down)
            {
                if (gvDetail.CurrentRow != null && gvDetail.CurrentRow.Index == gvDetail.Rows.Count - 1) // Hàng cuối cùng
                {
                    object productName = gvDetail.CurrentRow.Cells["ProductName"].Value;
                    if (productName != null && !string.IsNullOrEmpty(productName.ToString())) // Nếu ProductName có giá trị
                    {
                        gvDetail.Rows.Add(); // Thêm hàng mới
                    }
                }
            }

            // Nhấn phím ↑ (Up)
            if (e.KeyData == Keys.Up)
            {
                if (gvDetail.CurrentRow != null && gvDetail.CurrentRow.Index == gvDetail.Rows.Count - 1) // Hàng cuối cùng
                {
                    object cellValue = gvDetail.CurrentCell?.Value;
                    object productName = gvDetail.CurrentRow.Cells["ProductName"].Value;

                    if ((cellValue == null && gvDetail.Rows.Count > 1) || (productName == null && gvDetail.Rows.Count > 1))
                    {
                        gvDetail.Rows.RemoveAt(gvDetail.CurrentRow.Index); // Xóa hàng
                    }
                }
            }
        }

        private void CmdDeleteDetail_Click(object sender, EventArgs e)
        {
            _lstBarcode.Clear();

            // Duyệt từ dưới lên để xóa dữ liệu trong DataGridView
            for (int i = gvDetail.Rows.Count - 1; i >= 0; i--)
            {
                if (gvDetail.Rows[i].Cells["BARCODE"].Value != null)
                {
                    _lstBarcode.Add(gvDetail.Rows[i].Cells["BARCODE"].Value.ToString());
                }
                gvDetail.Rows.RemoveAt(i);
            }

            // Thêm dòng mới với STT = 1 nếu danh sách trống
            int newRowIndex = gvDetail.Rows.Add();
            gvDetail.Rows[newRowIndex].Cells["STT"].Value = 1;
        }

        private void CmdDeleteRow_Click(object sender, EventArgs e)
        {
            if (gvDetail.SelectedRows.Count > 0)
            {
                int index = gvDetail.SelectedRows[0].Index;
                DataGridViewRow selectedRow = gvDetail.Rows[index];

                if (selectedRow.Cells["BARCODE"].Value != null)
                {
                    if (_add)
                    {
                        gvDetail.Rows.RemoveAt(index);
                    }
                    else
                    {
                        _lstBarcode.Add(selectedRow.Cells["BARCODE"].Value.ToString());
                        gvDetail.Rows.RemoveAt(index);
                    }

                    // Update row numbers using our utility
                    DataGridViewHelper.UpdateRowNumbers(gvDetail);

                    // Ensure there's at least one row
                    if (gvDetail.Rows.Count == 0)
                    {
                        int newRowIndex = gvDetail.Rows.Add();
                        gvDetail.Rows[newRowIndex].Cells["STT"].Value = 1;
                    }

                    // Set focus to the appropriate row
                    if (index < gvDetail.Rows.Count)
                    {
                        gvDetail.CurrentCell = gvDetail.Rows[index].Cells[0];
                    }
                }
                else
                {
                    MessageBox.Show("No item selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No item selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmdImport_Click(object sender, EventArgs e)
        {
            importExcel();
        }
        private void importExcel()
        {
            string filename = "";
            List<BusinessLayer.Utils.errExport> err = new List<BusinessLayer.Utils.errExport>();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Excel 2000-2003 (.xls)|*.xls|Excel 2007 (.xlsx)|*.xlsx";

            if (op.ShowDialog() == DialogResult.OK)
            {
                _isImport = true;
                List<string> s = new List<string>();
                List<string> _exist = new List<string>();

                // Kiểm tra nếu DataGridView có dữ liệu trước đó
                if (gvDetail.Rows.Count > 1)
                {
                    foreach (DataGridViewRow row in gvDetail.Rows)
                    {
                        if (row.Cells["ProductName"].Value != null)
                        {
                            _exist.Add(row.Cells["BARCODE"].Value.ToString());
                        }
                    }
                }

                filename = op.FileName;

                // Mở file Excel
                Excel.Application app = new Excel.Application();
                Excel.Workbook wb = app.Workbooks.Open(filename);
                List<obj_INVOICE_DETAIL> lstCTCT = new List<obj_INVOICE_DETAIL>();

                try
                {
                    Excel._Worksheet sheet = wb.Sheets["Sheet1"];
                    Excel.Range range = sheet.UsedRange;

                    for (int i = 2; i <= range.Rows.Count; i++)
                    {
                        string barcode = range.Cells[i, 1].Value?.ToString();
                        if (string.IsNullOrEmpty(barcode)) continue;

                        tb_Product hh = _product.getItemBarcode(barcode);
                        if (hh == null)
                        {
                            err.Add(new BusinessLayer.Utils.errExport
                            {
                                _barcode = barcode,
                                _quantity = int.Parse(range.Cells[i, 2].Value.ToString()),
                                _errcode = "Barcode không tồn tại"
                            });
                            continue;
                        }

                        if (_exist.Contains(hh.BARCODE))
                        {
                            err.Add(new BusinessLayer.Utils.errExport
                            {
                                _barcode = barcode,
                                _quantity = int.Parse(range.Cells[i, 2].Value.ToString()),
                                _errcode = "Trùng Barcode"
                            });
                            continue;
                        }

                        s.Add(barcode + "," + range.Cells[i, 2].Value.ToString());
                        _exist.Add(barcode);
                    }

                    // Thêm dữ liệu vào DataGridView
                    foreach (string _validItem in s)
                    {
                        string[] item = _validItem.Split(',');
                        string _BARCODE = item[0];
                        double _quantity = double.Parse(item[1]);
                        obj_PRODUCT _h = _product.getItemFull(_BARCODE);

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(gvDetail);

                        row.Cells[gvDetail.Columns["STT"].Index].Value = gvDetail.Rows.Count + 1;
                        row.Cells[gvDetail.Columns["BARCODE"].Index].Value = _h.BARCODE;
                        row.Cells[gvDetail.Columns["Unit"].Index].Value = _h.Unit;
                        row.Cells[gvDetail.Columns["ProductName"].Index].Value = _h.ProductName;
                        row.Cells[gvDetail.Columns["QuantityDetail"].Index].Value = _quantity;
                        row.Cells[gvDetail.Columns["Price"].Index].Value = _h.Price;
                        row.Cells[gvDetail.Columns["SubTotal"].Index].Value = _h.Price * _quantity;

                        gvDetail.Rows.Add(row);
                    }

                    _isImport = false;
                }
                catch (Exception ex)
                {
                    app.Workbooks.Close();
                    MessageBox.Show("Import không thành công. Lỗi: " + ex.Message, "Thông báo");
                }
                finally
                {
                    wb.Close();
                    app.Quit();
                    ExcelHelper.ReleaseObject(wb);
                    ExcelHelper.ReleaseObject(app);
                }
            }

            // Xuất danh sách lỗi nếu có
            if (err.Count > 0)
            {
                // Convert BusinessLayer.Utils.errExport to STOCK.StockHelpers.errExport
                List<STOCK.StockHelpers.errExport> stockErrors = err.Select(e => new STOCK.StockHelpers.errExport
                {
                    _barcode = e._barcode,
                    _quantity = (int)e._quantity, // Convert double to int if needed
                    _errcode = e._errcode
                }).ToList();

                ExcelHelper.ExportErrorsToExcel(stockErrors, filename);

            }

            else
            {
                MessageBox.Show("Import dữ liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gvList_DoubleClick(object sender, EventArgs e)
        {
            if (gvList.Rows.Count > 0)
            {
                tabInvoice.SelectedTab = pageDetail;
            }
        }

        private void tabInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit == false && tabInvoice.SelectedTab == pageDetail)
            {
                gvDetail.ReadOnly = true; // Nếu gvDetail là DataGridView

                // Kiểm tra nếu hóa đơn hiện tại đã bị xóa thì hiển thị label Delete
                tb_Invoice current = (tb_Invoice)_bsInvoice.Current;
                if (current != null && current.DeletedBy != null)
                {
                    lblDelete.Visible = true;
                    btnDelete.Enabled = false;
                }
                else
                {
                    lblDelete.Visible = false;
                    btnDelete.Enabled = true;
                }
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateRange(dtFrom, dtTill);
        }

        private void dtTill_ValueChanged(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateRange(dtFrom, dtTill);
        }

        private void dtFrom_Leave(object sender, EventArgs e)
        {
            if (FormValidationHelper.ValidateDateRange(dtFrom, dtTill))
            {
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboDepartment.SelectedValue.ToString());
                _bsInvoice.DataSource = _lstInvoice;
            }
        }

        private void dtTill_Leave(object sender, EventArgs e)
        {
            if (FormValidationHelper.ValidateDateRange(dtFrom, dtTill))
            {
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboExportUnit.SelectedValue.ToString());
                _bsInvoice.DataSource = _lstInvoice;
            }
        }
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {

        }
        bool cal(int width, DataGridView dgv)
        {
            dgv.RowHeadersWidth = Math.Max(dgv.RowHeadersWidth, width);
            return true;
        }


    }
}
