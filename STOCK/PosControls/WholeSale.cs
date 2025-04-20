﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System;
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
using Microsoft.Reporting.WinForms;
using STOCK.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using STOCK.PopUpForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace POS.PosControls
{
    public partial class WholeSale : UserControl
    {
        public WholeSale()
        {
            InitializeComponent();
        }
        public WholeSale(tb_SYS_USER user, int right)
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
        List<_STATUS> _status;
        // List<tb_Invoice> _lstInvoice; // Replaced by _fullInvoiceListForPeriod
        List<tb_Invoice> _fullInvoiceListForPeriod; // Stores the full list for filtering
        // Dictionary<int, tb_Customer> _allCustomersDict; // Removed as CustomerID is not in tb_Invoice for filtering

        string err = "";

        COMPANY _company;
        DEPARTMENT _department;
        PRODUCT _product;
        INVOICE_DETAIL _invoiceDetail;
        INVOICE _invoice;
        CUSTOMER _customer;
        BindingSource _bsInvoiceDT;
        BindingSource _bsInvoice;

        Guid _id;
        Guid _pinvoiceID;

        tb_SYS_SEQ _seq;
        SYS_SEQ _sequence;

        // Thêm biến điều khiển cho phép chuyển tab hay không
        bool _allowTabChange = false;


        private void WholeSale_Load(object sender, EventArgs e)
        {
            gvList.AutoGenerateColumns = false;
            gvDetail.AutoGenerateColumns = false;

            _import = false;
            _lstBarcode = new List<string>();
            _company = new COMPANY();
            _customer = new CUSTOMER();
            _department = new DEPARTMENT();
            _product = new PRODUCT();
            _invoice = new INVOICE();
            _invoiceDetail = new INVOICE_DETAIL();
            _sequence = new SYS_SEQ();
            _bsInvoice = new BindingSource();
            _bsInvoiceDT = new BindingSource();

            dtFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtTill.Value = DateTime.Now;

            _bsInvoice.PositionChanged += _bsInvoice_PositionChanged; ;
            LoadCompany();
            // Attach handler BEFORE setting value
            cboCompany.SelectedIndexChanged += CboCompany_SelectedIndexChanged;

            // Check if _compid is valid before trying to set the selection
            if (!string.IsNullOrWhiteSpace(myFunctions._compid))
            {
                int selectedIndex = -1;
                // Find the index of the item matching _compid in the DataSource
                if (cboCompany.DataSource is List<tb_Company> companyList)
                {
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        // Ensure case-insensitive comparison if CompanyID might have case variations
                        if (string.Equals(companyList[i].CompanyID, myFunctions._compid, StringComparison.OrdinalIgnoreCase))
                        {
                            selectedIndex = i;
                            break;
                        }
                    }
                }
                // Add checks for other potential DataSource types if necessary (e.g., DataTable)

                if (selectedIndex != -1)
                {
                    // Index found, safe to set SelectedIndex
                    cboCompany.SelectedIndex = selectedIndex;
                    Console.WriteLine($"Successfully set cboCompany.SelectedIndex to {selectedIndex} for CompanyID: {myFunctions._compid}");
                }
                else
                {
                    // Value does not exist in the DataSource
                    Console.WriteLine($"Warning: myFunctions._compid ('{myFunctions._compid}') not found in cboCompany DataSource. Cannot set selection by index.");
                    // Optionally select the first item as a fallback
                    if (cboCompany.Items.Count > 0 && cboCompany.SelectedIndex == -1)
                    {
                        // cboCompany.SelectedIndex = 0; // Uncomment to select the first item
                        Console.WriteLine("Selecting first available company as fallback (index 0).");
                    }
                }
            }
            else
            {
                 Console.WriteLine("Warning: myFunctions._compid is null or empty. Skipping cboCompany selection assignment.");
                 // Optionally select the first item if available and no default is set
                 if (cboCompany.Items.Count > 0 && cboCompany.SelectedIndex == -1)
                 {
                    // cboCompany.SelectedIndex = 0; // Uncomment to select the first item if none is pre-selected
                 }
            }

            _status = _STATUS.getList();
            cboStatus.DataSource = _status;
            cboStatus.DisplayMember = "_display";
            cboStatus.ValueMember = "_value";

            gvList.CellFormatting += GvList_CellFormatting; ;
            gvList.CellPainting += GvList_CellPainting; ;

            LoadDepartment();
            LoadExport();


            // Load all customers for filtering lookup - Removed as CustomerID is not in tb_Invoice for filtering
            // _allCustomersDict = _customer.getList().ToDictionary(c => c.CustomerID);

            // Load initial full invoice list - MOVED to be triggered by Department selection
            // LoadFullInvoiceList(); // New method to load/reload the full list

            cboCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;

            // Set the grid's data source
            gvList.DataSource = _bsInvoice;

            // Apply initial filter (which will be empty search text initially)
            // FilterInvoices(); // Renamed method call - Will be called by LoadFullInvoiceList

            // Subscribe to customer selection changes
            cboCustomer.SelectedIndexChanged += CboCustomer_SelectedIndexChanged; // Filter when selection changes
            // exportInfor(); // MOVED to FilterInvoices() - Load details for the initially selected invoice
            cboDepartment.SelectedIndexChanged += CboDepartment_SelectedIndexChanged;
            gvDetail.CellMouseDown += GvDetail_CellMouseDown;
            gvDetail.CellValueChanged += GvDetail_CellValueChanged; 
            gvDetail.CellEndEdit += GvDetail_CellEndEdit;
            CmdDeleteRow.Click += CmdDeleteRow_Click; ;
            CmdDeleteDetail.Click += CmdDeleteDetail_Click;
            gvDetail.CellFormatting += GvDetail_CellFormatting; // Add CellFormatting event handler

            // Subscribe to discount leave event
            // IMPORTANT: Ensure txtDiscount TextBox exists on your form!
            txtDiscount.Leave += txtDiscount_Leave;
            txtDiscount.KeyDown += txtDiscount_KeyDown; // Add KeyDown event handler

            // Set fixed row header width (optional)
            gvList.RowHeadersWidth = 25;
            gvDetail.RowHeadersWidth = 25;

            // Đăng ký sự kiện chuyển tab
            tabInvoice.Selecting += TabInvoice_Selecting;

            // Đăng ký sự kiện double click vào danh sách
            gvList.DoubleClick += GvList_DoubleClick; ; 

            _enable(false);
            ShowHideControls(true);
            contextMenuStrip.Enabled = false;
            gvList.ClearSelection();

            // Update buttons based on user permissions
            UpdateButtonsByPermission();
            LoadCustomer(); // Load customer data
            cboCustomer.Text = ""; // Ensure customer filter is clear initially

            // Initial load will be triggered by CboCompany_SelectedIndexChanged -> CboDepartment_SelectedIndexChanged
            // or explicitly if the company doesn't change but departments exist.
             if (cboDepartment.Items.Count > 0 && cboCompany.SelectedValue?.ToString() == myFunctions._compid)
             {
                 // If the initial company is the default one and departments loaded, trigger department change
                 cboDepartment.SelectedIndex = 0;
             }
             else if (cboDepartment.Items.Count == 0)
             {
                 // If no departments initially, clear the list
                 LoadFullInvoiceList();
             }
             // Otherwise, the CboCompany_SelectedIndexChanged will handle loading departments and triggering the next step.

        }

        void LoadFullInvoiceList()
        {
            if (cboDepartment.SelectedValue != null)
            {
                string departmentId = cboDepartment.SelectedValue.ToString();
                DateTime fromDate = dtFrom.Value;
                DateTime tillDate = dtTill.Value.AddDays(1);
                Console.WriteLine($"Loading invoices for Dept: {departmentId}, From: {fromDate:yyyy-MM-dd}, Till: {tillDate:yyyy-MM-dd}"); // Log parameters
                _fullInvoiceListForPeriod = _invoice.getList(3, fromDate, tillDate, departmentId);
                Console.WriteLine($"Fetched {_fullInvoiceListForPeriod?.Count ?? 0} invoices (InvoiceType=3) before filtering."); // Log count
            }
            else
            {
                _fullInvoiceListForPeriod = new List<tb_Invoice>(); // Handle case where department might not be selected yet
            }
            // Apply filter after loading new full list
            FilterInvoices(); // Renamed method call
        }

        // Renamed from FilterInvoicesByCustomer
        private void FilterInvoices()
        {
            if (_fullInvoiceListForPeriod == null) return; // Ensure the full list is loaded

            List<tb_Invoice> filteredList = _fullInvoiceListForPeriod; // Start with the full list for the period

            // Filter by selected customer ONLY if an item is actually selected
            if (cboCustomer.SelectedIndex != -1 && cboCustomer.SelectedValue != null)
            {
                if (int.TryParse(cboCustomer.SelectedValue.ToString(), out int selectedCustomerId))
                {
                    string customerIdString = selectedCustomerId.ToString();
                    // Filter based on ReceivingDepartmentID matching the selected CustomerID (stored as string)
                    filteredList = filteredList.Where(inv =>
                        inv.ReceivingDepartmentID == customerIdString
                    ).ToList();
                    Console.WriteLine($"Filtered by CustomerID: {selectedCustomerId}. Count: {filteredList.Count}");
                }
                else
                {
                     Console.WriteLine($"Warning: Could not parse CustomerID from SelectedValue '{cboCustomer.SelectedValue}'. Showing all.");
                     // Keep filteredList as _fullInvoiceListForPeriod if parsing fails
                }
            }
            else
            {
                 // If no customer is selected (index is -1), show all invoices for the period/department.
                 Console.WriteLine("No customer selected (Index -1). Showing all for period/department.");
            }


            gvList.DataSource = null; // Explicitly detach old source
            _bsInvoice.DataSource = filteredList;
            gvList.DataSource = _bsInvoice; // Re-attach the binding source
            // _bsInvoice.ResetBindings(false); // ResetBindings might be redundant now
            gvList.Refresh(); // Force UI refresh for the list grid
            Console.WriteLine($"Grid DataSource updated. Filtered list count: {filteredList?.Count ?? 0}"); // Log after binding

            // Only load details if there are items in the filtered list
            if (filteredList != null && filteredList.Count > 0)
            {
                exportInfor(); // Load details for the current selection
            }
            else
            {
                // Optionally clear detail view if list is empty
                 _bsInvoiceDT.DataSource = null;
                 gvDetail.Refresh();
                 ResetFields(); // Reset detail fields as well
            }
        }


        void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }

        void LoadCustomer()
        {
            cboCustomer.DataSource = _customer.getList();
            cboCustomer.DisplayMember = "FullName";
            cboCustomer.ValueMember = "CustomerID";
        }
        void LoadDepartment()
        {
            // Add null check before accessing SelectedValue
            if (cboCompany.SelectedValue == null)
            {
                Console.WriteLine("LoadDepartment: cboCompany.SelectedValue is null. Clearing cboDepartment.");
                cboDepartment.DataSource = null;
                // Optionally clear DisplayMember/ValueMember if needed
                // cboDepartment.DisplayMember = "";
                // cboDepartment.ValueMember = "";
                return;
            }
            cboDepartment.DataSource = _department.getAll(cboCompany.SelectedValue.ToString());
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentID";
            cboDepartment.SelectedIndex = -1;
        }

        void LoadExport()
        {
            // Add null check before accessing SelectedValue
            if (cboCompany.SelectedValue == null)
            {
                Console.WriteLine("LoadExport: cboCompany.SelectedValue is null. Clearing cboExportUnit.");
                cboExportUnit.DataSource = null;
                // Optionally clear DisplayMember/ValueMember if needed
                // cboExportUnit.DisplayMember = "";
                // cboExportUnit.ValueMember = "";
                return;
            }
            cboExportUnit.DataSource = _department.getAll(cboCompany.SelectedValue.ToString());
            cboExportUnit.DisplayMember = "DepartmentName";
            cboExportUnit.ValueMember = "DepartmentID";
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
            cboCustomer.Enabled = t;
            dtDate.Enabled = t;
            txtDiscount.Enabled = t; // Enable/disable discount field
        }

        private void ResetFields()
        {
            txtInvoiceNo.Text = "";
            txtNote.Text = "";
            txtDiscount.Text = "0"; // Reset discount field
        }

        private void GvList_DoubleClick(object sender, EventArgs e)
        {
            if (gvList.Rows.Count > 0)
            {
                _allowTabChange = true;
                tabInvoice.SelectedTab = pageDetail;
                _allowTabChange = false;
            }
        }

        private void TabInvoice_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == pageDetail && !_allowTabChange && !_add && !_edit)
            {
                // Hủy thao tác chuyển tab
                e.Cancel = true;
            }
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
                        int currentRowIndex = e.RowIndex; // Lưu lại vị trí hàng hiện tại

                        try
                        {
                            // Pass the current row index to formList
                            formList _popList = new formList(gvDetail, barcode, currentRowIndex);
                            _popList.ShowDialog();

                            // Sau khi đóng form, kiểm tra xem có dữ liệu nào được thêm vào không
                            bool dataAdded = false;

                            // Lấy số hàng trước khi kiểm tra để tránh lỗi khi xóa hàng
                            int totalRows = gvDetail.Rows.Count;

                            // Kiểm tra xem dòng hiện tại còn chứa dấu "." không
                            // Nếu còn tồn tại và chưa được cập nhật, xóa dấu "." đó
                            if (currentRowIndex < totalRows && !gvDetail.Rows[currentRowIndex].IsNewRow)
                            {
                                var currentBarcode = gvDetail.Rows[currentRowIndex].Cells["BARCODE"].Value?.ToString();
                                if (currentBarcode == barcode) // Vẫn còn giữ dấu "."
                                {
                                    // Kiểm tra xem có phải dòng cuối cùng không
                                    bool isLastDataRow = true;
                                    for (int i = currentRowIndex + 1; i < gvDetail.Rows.Count; i++)
                                    {
                                        if (!gvDetail.Rows[i].IsNewRow &&
                                            gvDetail.Rows[i].Cells["BARCODE"].Value != null &&
                                            !string.IsNullOrWhiteSpace(gvDetail.Rows[i].Cells["BARCODE"].Value.ToString()))
                                        {
                                            isLastDataRow = false;
                                            break;
                                        }
                                    }

                                    // Luôn giữ lại dòng cuối cùng và chỉ xóa dấu "."
                                    gvDetail.Rows[currentRowIndex].Cells["BARCODE"].Value = null;

                                    // Nếu không phải dòng cuối cùng có dữ liệu, xóa dòng
                                    if (!isLastDataRow)
                                    {
                                        gvDetail.Rows.RemoveAt(currentRowIndex);
                                    }
                                }
                                else
                                {
                                    dataAdded = true;
                                }
                            }

                            // Đảm bảo SubTotal được tính đúng sau khi thêm sản phẩm
                            AutoUpdateTotalPriceForBoundGrid();

                            // Đảm bảo có một dòng trống ở cuối
                            EnsureOneEmptyRowAtBottom();

                            // Debug gridview để xem các sản phẩm đã được thêm vào
                            DebugDetailRows();
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

                                // Đảm bảo có một dòng trống ở cuối
                                EnsureOneEmptyRowAtBottom();
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

                // Không gọi EnsureEmptyRowAtBottom ở đây để tránh tạo nhiều dòng trống
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in gvDetail_CellValueChanged: {ex.Message}");
                // Don't show error to user, just log it
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
            LoadFullInvoiceList();
        }

        // Event handler for Customer ComboBox selection change
        private void CboCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
             // Filter whenever the selected index changes (including clearing the selection)
             FilterInvoices();
        }

        // REMOVED TextChanged handler as it's replaced by SelectedIndexChanged
        // private void CboCustomer_TextChanged(object sender, EventArgs e)
        // {
        //     FilterInvoices(); // Call the filtering logic when text changes
        // }

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
            // Add null check here
            if (cboCompany.SelectedValue == null)
            {
                // Handle the case where no company is selected
                // Option 1: Clear dependent controls
                cboDepartment.DataSource = null;
                cboExportUnit.DataSource = null;
                _fullInvoiceListForPeriod = new List<tb_Invoice>();
                FilterInvoices(); // Clear the invoice list
                Console.WriteLine("Company selection cleared or invalid. Cleared dependent controls.");
                return; // Exit the handler
            }

            // Proceed only if SelectedValue is not null
            LoadDepartment();
            LoadExport();
            if (cboDepartment.Items.Count > 0)
            {
                 cboDepartment.SelectedIndex = 0;
            }
            else
            {
                 LoadFullInvoiceList();
            }
        }

        private void _bsInvoice_PositionChanged(object sender, EventArgs e)
        {
            if (!_add)
            {
                exportInfor();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            cboExportUnit.SelectedValue = cboDepartment.SelectedValue;
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

            // Cho phép chuyển tab tạm thời
            _allowTabChange = true;
            tabInvoice.SelectedTab = pageDetail;
            _allowTabChange = false;

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
            EnsureOneEmptyRowAtBottom();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tb_Invoice current = (tb_Invoice)_bsInvoice.Current;
            if (current.Status == 1)
            {
                _add = false;
                _edit = true;
                ShowHideControls(false);
                _enable(true);

                // Cho phép chuyển tab tạm thời
                _allowTabChange = true;
                tabInvoice.SelectedTab = pageDetail;
                _allowTabChange = false;

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
                EnsureOneEmptyRowAtBottom();

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
        private void EnsureOneEmptyRowAtBottom()
        {
            try
            {
                // Đếm số dòng trống ở cuối
                int emptyRows = 0;
                int lastRowWithData = -1;

                // Duyệt từ cuối lên để tìm dòng trống
                for (int i = gvDetail.Rows.Count - 1; i >= 0; i--)
                {
                    if (gvDetail.Rows[i].IsNewRow) continue;

                    var barcodeValue = gvDetail.Rows[i].Cells["BARCODE"].Value;
                    var productNameValue = gvDetail.Rows[i].Cells["ProductName"].Value;

                    if (barcodeValue == null || string.IsNullOrWhiteSpace(barcodeValue.ToString()) ||
                        productNameValue == null || string.IsNullOrWhiteSpace(productNameValue.ToString()))
                    {
                        emptyRows++;
                    }
                    else
                    {
                        lastRowWithData = i;
                        break;
                    }
                }

                // Nếu không có dòng trống nào, thêm một dòng trống mới
                if (emptyRows == 0 && lastRowWithData >= 0)
                {
                    int newRowIndex = gvDetail.Rows.Add();
                    gvDetail.Rows[newRowIndex].Cells["STT"].Value = lastRowWithData + 2; // STT = index của dòng dữ liệu cuối + 2
                }
                // Nếu có nhiều hơn 1 dòng trống, xóa bớt dòng trống
                else if (emptyRows > 1)
                {
                    for (int i = 0; i < emptyRows - 1; i++)
                    {
                        int indexToRemove = lastRowWithData + 1;
                        if (indexToRemove < gvDetail.Rows.Count && !gvDetail.Rows[indexToRemove].IsNewRow)
                        {
                            gvDetail.Rows.RemoveAt(indexToRemove);
                        }
                    }
                }

                // Cập nhật STT của dòng trống cuối cùng
                if (lastRowWithData >= 0 && lastRowWithData + 1 < gvDetail.Rows.Count)
                {
                    gvDetail.Rows[lastRowWithData + 1].Cells["STT"].Value = lastRowWithData + 2;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EnsureOneEmptyRowAtBottom: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];
                Guid invoiceID = (Guid)row.Cells["InvoiceID"].Value;

                if (MessageBox.Show("Do you want to delete this record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    // Gọi phương thức delete trong INVOICE với UserID = 1
                    _invoice.delete(invoiceID, _user.UserID);

                    // Cập nhật UI để hiển thị trạng thái đã xóa
                    row.Cells["DeletedBy"].Value = _user.UserID;
                    lblDelete.Visible = true;
                    btnDelete.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                cboStatus.SelectedValue = current.Status;
                // Load ReceivingDepartmentID - Attempt to select customer if ReceivingDepartmentID matches a CustomerID
                if (!string.IsNullOrEmpty(current.ReceivingDepartmentID) && int.TryParse(current.ReceivingDepartmentID, out int receivingCustId))
                {
                    // Check if this ID exists in the customer combobox
                    var customerItem = cboCustomer.Items.Cast<tb_Customer>().FirstOrDefault(c => c.CustomerID == receivingCustId);
                    if (customerItem != null)
                    {
                        cboCustomer.SelectedValue = receivingCustId;
                    }
                    else
                    {
                        // ID exists but doesn't match a customer, maybe it's a department? Clear selection.
                        cboCustomer.SelectedIndex = -1;
                        cboCustomer.Text = $"ID: {current.ReceivingDepartmentID}"; // Show the raw ID if no match
                    }
                }
                else
                {
                    // ReceivingDepartmentID is null, empty, or not an integer
                    cboCustomer.SelectedIndex = -1;
                    cboCustomer.Text = ""; // Clear text
                }
                // Load Discount from invoice header if applicable (assuming tb_Invoice has a Discount field)
                // txtDiscount.Text = (current.Discount ?? 0).ToString("N2"); // Example if Discount is stored in header


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

                                // REMOVED: Do not overwrite the loaded Price with the original product price.
                                // The loaded price should be the one saved (potentially discounted).
                                // if (gvDetail.Columns.Contains("Price") &&
                                //     (gvDetail.Rows[i].Cells["Price"].Value == null))
                                // {
                                //     gvDetail.Rows[i].Cells["Price"].Value = product.Price;
                                // }

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

                // Add to the underlying full list and refresh the view/filter
                if (_fullInvoiceListForPeriod != null)
                {
                    _fullInvoiceListForPeriod.Add(resultInvoice);
                    FilterInvoices(); // Renamed method call

                    // Try to select the newly added item in the filtered list
                    var currentFilteredList = _bsInvoice.DataSource as List<tb_Invoice>;
                    if (currentFilteredList != null)
                    {
                        int newIndex = currentFilteredList.FindIndex(inv => inv.InvoiceID == resultInvoice.InvoiceID);
                        if (newIndex >= 0)
                        {
                            _bsInvoice.Position = newIndex;
                        }
                    }
                }
                else // Should not happen if LoadFullInvoiceList was called, but handle defensively
                {
                    _fullInvoiceListForPeriod = new List<tb_Invoice> { resultInvoice };
                    FilterInvoices(); // Renamed method call
                }
            }
            else // Editing existing invoice
            {
                // Get the original invoice ID before potentially changing the current selection
                Guid currentInvoiceId = ((tb_Invoice)_bsInvoice.Current).InvoiceID;

                invoice = _invoice.getItem(currentInvoiceId); // Get the item directly by ID
                Invoice_Infor(invoice); // Populate changes
                var resultInvoice = _invoice.update(invoice); // Update in DB
                InvoiceDetail_Infor(resultInvoice); // Update details

                // Refresh the full list data from DB
                LoadFullInvoiceList(); // This reloads _fullInvoiceListForPeriod and applies filter via FilterInvoicesByCustomer

                // Try to find and re-select the updated invoice in the potentially filtered list
                var currentFilteredList = _bsInvoice.DataSource as List<tb_Invoice>;
                if (currentFilteredList != null)
                {
                    for (int i = 0; i < currentFilteredList.Count; i++)
                    {
                        if (currentFilteredList[i].InvoiceID == resultInvoice.InvoiceID)
                        {
                            _bsInvoice.Position = i;
                            break;
                        }
                    }
                }

                // Refresh the display explicitly if needed (ResetBindings usually handles this)
                // gvList.Refresh();
            }

            // exportInfor(); // This should be called automatically if _bsInvoice.Position changes, or called by FilterInvoicesByCustomer
            _add = false; // Reset add flag regardless

            tabInvoice.SelectedTab = pageList;
        }

        void InvoiceDetail_Infor(tb_Invoice invoice)
        {
            // First delete all existing details for this invoice
            _invoiceDetail.deleteAllByInvoiceId(invoice.InvoiceID);

            List<tb_InvoiceDetail> detailsToAdd = new List<tb_InvoiceDetail>();

            // Find all column indices needed to safely access cells
            Dictionary<string, int> columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            // List of columns we need to find (case-insensitive) - Added DiscountAmount
            string[] requiredColumns = new string[] {
                "BARCODE", "ProductID", "ProductName", "QuantityDetail", "Price", "SubTotal", "Unit", "DiscountAmount"
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
                            detail.Quantity = 1; // Default quantity if parsing fails

                        // === Saving Logic for Percentage Discount ===
                        double price = 0;
                        double subTotal = 0;
                        double discountPercent = 0; // Changed from int discountAmount to double discountPercent

                        // 1. Get Price (Original Price) from grid
                        if (columnIndices.TryGetValue("Price", out int priceIdx) && row.Cells[priceIdx].Value != null)
                        {
                            // Use InvariantCulture for parsing potentially formatted numbers
                            double.TryParse(row.Cells[priceIdx].Value.ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price);
                        }
                        else
                        {
                            // Fallback: Get original price from DB if grid price is missing (shouldn't happen ideally)
                            var product = _product.getItemBarcode(detail.BARCODE);
                            price = product?.Price ?? 0;
                            Console.WriteLine($"Warning: Row {i} Price missing from grid, fetched from DB: {price}");
                        }

                        // 2. Get Discount Percentage from grid
                        if (columnIndices.TryGetValue("DiscountAmount", out int discountAmtIdx) && row.Cells[discountAmtIdx].Value != null)
                        {
                            // Use InvariantCulture for parsing potentially formatted numbers (treat as percentage)
                            double.TryParse(row.Cells[discountAmtIdx].Value.ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out discountPercent);
                        }

                        // 3. Calculate SubTotal based on Price, Quantity, and Discount Percentage
                        // SubTotal = Price * Quantity * (1 - DiscountAmount / 100)
                        double quantity = detail.Quantity ?? 1; // Use quantity already parsed
                        subTotal = price * quantity * (1 - (discountPercent / 100.0));

                        // --- DEBUG ---
                        Console.WriteLine($"--- Row {i} Calculation ---");
                        Console.WriteLine($"  Price          : {price}");
                        Console.WriteLine($"  Quantity       : {quantity}");
                        Console.WriteLine($"  Discount %     : {discountPercent}");
                        Console.WriteLine($"  Calculated SubTotal: {subTotal} = {price} * {quantity} * (1 - ({discountPercent} / 100.0))");
                        // -------------

                        // 4. Assign values for saving
                        detail.Price = price;                   // Save the ORIGINAL price read from grid/DB
                        detail.SubTotal = subTotal;             // Save the calculated subtotal
                        detail.DiscountAmount = (int?)discountPercent; // Cast double to int? to save percentage discount

                        // =======================================================

                        // Get ProductID (using original logic, but after price/subtotal/discount are set from grid)
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

                        // Debug the values being saved (DiscountAmt is now DiscountPercent)
                        Console.WriteLine($"Row {i} SAVING detail: BARCODE={detail.BARCODE}, Qty={detail.Quantity}, " +
                                          $"Price={detail.Price}, SubTotal={detail.SubTotal}, DiscountPercent={(detail.DiscountAmount.HasValue ? detail.DiscountAmount.Value.ToString() : "NULL")}, ProductID={detail.ProductID}");
                        Console.WriteLine($"--- End Row {i} ---");
                        detailsToAdd.Add(detail);
                    }
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
            _seq = _sequence.getItem("XSI@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
            if (_seq == null)
            {
                _seq = new tb_SYS_SEQ();
                _seq.SEQNAME = "XSI@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol;
                _seq.SEQVALUE = 1;
                _sequence.add(_seq);
            }

            if (_add)
            {
                invoice.InvoiceID = Guid.NewGuid();
                invoice.Day = dtDate.Value;
                invoice.Invoice = _seq.SEQVALUE.Value.ToString("000000") + "/" + DateTime.Today.Year.ToString().Substring(2, 2) + "/XSI/" + dp.Symbol;
                invoice.CreatedBy = _user.UserID;
                invoice.CreatedDate = DateTime.Now;
            }

            invoice.InvoiceType = 3;
            invoice.CompanyID = cboCompany.SelectedValue.ToString();
            invoice.DepartmentID = cboExportUnit.SelectedValue.ToString();
            invoice.Description = txtNote.Text;
            invoice.Status = Convert.ToInt32(cboStatus.SelectedValue);
            // Save ReceivingDepartmentID from cboCustomer selection
            if (cboCustomer.SelectedValue != null && int.TryParse(cboCustomer.SelectedValue.ToString(), out int custId))
            {
                // Store the CustomerID (int) as a string in ReceivingDepartmentID
                invoice.ReceivingDepartmentID = custId.ToString();
            }
            else
            {
                // No customer selected, or invalid selection
                invoice.ReceivingDepartmentID = null; // Or handle as needed, maybe based on cboCustomer.Text?
            }
            // Save overall discount percentage if applicable (assuming tb_Invoice has a Discount field)
            // if (double.TryParse(txtDiscount.Text, out double overallDiscount))
            // {
            //     invoice.Discount = overallDiscount;
            // }
            // else
            // {
            //     invoice.Discount = null;
            // }


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
            invoice.UpdatedBy = _user.UserID;
            invoice.UpdatedDate = DateTime.Now;
        }

        private void exportReport(string _reportName, string _tieude)
        {
            // Ensure an invoice is selected
            if (_bsInvoice.Current == null)
            {
                MessageBox.Show("Please select an invoice to preview.", "No Invoice Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tb_Invoice currentInvoice = (tb_Invoice)_bsInvoice.Current;
            List<obj_INVOICE_DETAIL> details = _bsInvoiceDT.DataSource as List<obj_INVOICE_DETAIL>;

            // Attempt to reload details if empty or null
            if (details == null || details.Count == 0)
            {
                details = _invoiceDetail.getListbyIDFull(currentInvoice.InvoiceID);
                if (details == null || details.Count == 0)
                {
                    MessageBox.Show("No details found for the selected invoice.", "No Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Fetch related data
            tb_Company company = _company.getItem(currentInvoice.CompanyID);
            tb_Department exportDepartment = _department.getItem(currentInvoice.DepartmentID); // Exporting Dept
            tb_Department receiveDepartment = _department.getItem(currentInvoice.ReceivingDepartmentID); // Receiving Dept

            // Create DataTable matching the RDLC DataSet structure (dsInvoiceNB, DataTable1)
            DataTable dtReportData = new DataTable("DataTable1"); // Match table name in dsInvoiceNB.xsd

            // Add columns based on RDLC Fields used in OutInnerInvoice.rdlc (ensure names match exactly)
            // Referencing dsInvoiceNB.xsd and OutInnerInvoice.rdlc for required fields
            dtReportData.Columns.Add("Invoice", typeof(string));
            dtReportData.Columns.Add("Day", typeof(string)); // Format as string
            dtReportData.Columns.Add("Description", typeof(string));
            dtReportData.Columns.Add("TotalPrice", typeof(double)); // Detail TotalPrice (Sum in report footer)
            dtReportData.Columns.Add("CompanyName", typeof(string));
            dtReportData.Columns.Add("DepartmentName", typeof(string)); // Exporting Department Name
            dtReportData.Columns.Add("DepartmentAddress", typeof(string)); // Exporting Department Address
            dtReportData.Columns.Add("DepartmentPhone", typeof(string)); // Exporting Department Phone
            dtReportData.Columns.Add("DepartmentName1", typeof(string)); // Receiving Department Name
            dtReportData.Columns.Add("DepartmentAddress1", typeof(string)); // Receiving Department Address
            dtReportData.Columns.Add("DepartmentPhone1", typeof(string)); // Receiving Department Phone
            dtReportData.Columns.Add("BARCODE", typeof(string));
            dtReportData.Columns.Add("ProductName", typeof(string));
            dtReportData.Columns.Add("Unit", typeof(string));
            dtReportData.Columns.Add("Quantity", typeof(int)); // Detail Quantity (Sum in report footer)
            dtReportData.Columns.Add("Price", typeof(double)); // Detail Price
            dtReportData.Columns.Add("SubTotal", typeof(double)); // Detail SubTotal (Sum in report footer)
            dtReportData.Columns.Add("DiscountAmount", typeof(double)); // Detail Discount Amount (Sum in report footer)

            // Populate DataTable
            foreach (var detail in details)
            {
                DataRow dr = dtReportData.NewRow();
                dr["Invoice"] = currentInvoice.Invoice;
                dr["Day"] = (currentInvoice.Day ?? DateTime.Now).ToString("dd/MM/yyyy");
                dr["Description"] = currentInvoice.Description;
                dr["TotalPrice"] = detail.SubTotal ?? 0; // Use detail's SubTotal for row-level data
                dr["CompanyName"] = company?.CompanyName;
                dr["DepartmentName"] = exportDepartment?.DepartmentName;
                dr["DepartmentAddress"] = exportDepartment?.DepartmentAddress;
                dr["DepartmentPhone"] = exportDepartment?.DepartmentPhone;
                dr["DepartmentName1"] = receiveDepartment?.DepartmentName;
                dr["DepartmentAddress1"] = receiveDepartment?.DepartmentAddress;
                dr["DepartmentPhone1"] = receiveDepartment?.DepartmentPhone;
                dr["BARCODE"] = detail.BARCODE;
                dr["ProductName"] = detail.ProductName;
                dr["Unit"] = detail.Unit;
                dr["Quantity"] = detail.Quantity ?? 0;
                dr["Price"] = detail.Price ?? 0;
                dr["SubTotal"] = detail.SubTotal ?? 0; // Use detail's SubTotal for row-level data
                dr["DiscountAmount"] = detail.DiscountAmount ?? 0; // Use detail's DiscountAmount for row-level data
                // Note: SubTotal column exists in XSD but seems unused directly in RDLC row, TotalPrice is used instead.
                // If SubTotal *is* needed per row, add: dr["SubTotal"] = detail.SubTotal ?? 0;
                dtReportData.Rows.Add(dr);
            }

            // Configure ReportViewer
            LocalReport report = new LocalReport();
            // Ensure the path matches the embedded resource name.
            // Check project properties -> Build Action for the rdlc file should be "Embedded Resource"
            // The name is typically Namespace.FolderName.FileName.rdlc
            report.ReportEmbeddedResource = "STOCK.RDLCReport.WholeSale.rdlc"; // Updated path

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dsInvoiceWholeSale"; // This MUST match the DataSet Name in the RDLC file
            rds.Value = dtReportData;
            report.DataSources.Clear();
            report.DataSources.Add(rds);

            report.Refresh(); // Refresh the report definition and data

            // Create and show the report viewer form
            try
            {
                formReportViewer frmViewer = new formReportViewer(report);
                frmViewer.Text = string.IsNullOrEmpty(_tieude) ? "Whole Sale Report" : _tieude; // Set form title
                frmViewer.ShowDialog(); // Show the form modally
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening report preview: {ex.Message}", "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            exportReport("WHOLE_SALE_REPORT", "Whole Sale Report");
        }

        private void CmdDeleteDetail_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private void CmdImport_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            importExcel();
        }
        private void UpdateButtonsByPermission()
        {
            // Handle permissions based on _right value
            // 0 = Lock Function - All buttons disabled
            // 1 = View Only - Only view/export allowed
            // 2 = Full Function - All functions allowed

            if (_right == 0) // Lock Function
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnPrint.Enabled = false;
                CmdDeleteRow.Enabled = false;
                CmdDeleteDetail.Enabled = false;
                CmdImport.Enabled = false;
                contextMenuStrip.Enabled = false;
            }
            else if (_right == 1) // View Only
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnPrint.Enabled = true; // Allow printing/export
                CmdDeleteRow.Enabled = false;
                CmdDeleteDetail.Enabled = false;
                CmdImport.Enabled = false;
                contextMenuStrip.Enabled = false;
            }
            else // Full Function (2)
            {
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnPrint.Enabled = true;
                CmdDeleteRow.Enabled = true;
                CmdDeleteDetail.Enabled = true;
                CmdImport.Enabled = true;
                // Context menu will be enabled when in edit mode
            }
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

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateRange(dtFrom, dtTill);
        }

        private void dtFrom_Leave(object sender, EventArgs e)
        {
            if (FormValidationHelper.ValidateDateRange(dtFrom, dtTill))
            {
                // Reload the full list based on the new date range and apply filter
                LoadFullInvoiceList();
            }
        }

        private void dtTill_ValueChanged(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateRange(dtFrom, dtTill);
        }

        private void dtTill_Leave(object sender, EventArgs e)
        {
            if (FormValidationHelper.ValidateDateRange(dtFrom, dtTill))
            {
                // Reload the full list based on the new date range and apply filter
                LoadFullInvoiceList();
            }
        }

        // Event handler for when the user leaves the discount textbox


        // Helper method to apply discount to the grid - COMMENTED OUT as percentage discount is now applied per row via ApplyPercentageDiscountToSelectedRow
        /*
        private void ApplyDiscountToGrid(double discountPercent)
        {
            try
            {
                double discountFactor = 1 - (discountPercent / 100.0);

                for (int i = 0; i < gvDetail.Rows.Count; i++)
                {
                    if (gvDetail.Rows[i].IsNewRow) continue; // Skip the new row placeholder

                    string barcode = gvDetail.Rows[i].Cells["BARCODE"].Value?.ToString();
                    if (string.IsNullOrEmpty(barcode)) continue; // Skip rows without barcode

                    // Get original price from product database
                    tb_Product product = _product.getItemBarcode(barcode);
                    if (product == null || !product.Price.HasValue) continue; // Skip if product or price not found

                    double originalPrice = product.Price.Value;
                    double discountedPrice = originalPrice * discountFactor;
                    // double discountAmount = originalPrice - discountedPrice; // Removed monetary discount calculation

                    // Get quantity
                    object quantityValue = gvDetail.Rows[i].Cells["QuantityDetail"].Value;
                    if (quantityValue == null || !double.TryParse(quantityValue.ToString(), out double quantity))
                    {
                        quantity = 1; // Default to 1 if quantity is invalid
                    }

                    // Update both Price and SubTotal cells in the grid
                    gvDetail.Rows[i].Cells["Price"].Value = discountedPrice;
                    gvDetail.Rows[i].Cells["SubTotal"].Value = discountedPrice * quantity;


                    if (gvDetail.Columns.Contains("DiscountAmount"))
                    {

                        gvDetail.Rows[i].Cells["DiscountAmount"].Value = discountPercent.ToString("N3");
                    }
                }
                gvDetail.Refresh(); // Refresh grid to show updated values
                Console.WriteLine($"Applied {discountPercent}% discount to grid.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying discount: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error in ApplyDiscountToGrid: {ex.Message}");
            }
        }
        */

        private void CmdDeleteRow_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        // Applies percentage discount from txtDiscount to all selected rows in gvDetail
        private void ApplyPercentageDiscountToSelectedRow()
        {
            if (!(_add || _edit)) return; // Only apply when adding or editing
            if (gvDetail.SelectedRows.Count == 0)
            {
                // Optional: Show message if no rows are selected
                // MessageBox.Show("Please select one or more product rows to apply the discount.", "No Rows Selected", MessageBoxButtons.OK, Information);
                return;
            }

            // Parse discount percentage from the textbox
            if (double.TryParse(txtDiscount.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out double discountPercent))
            {
                // Validate discount percentage (e.g., 0 to 100)
                if (discountPercent < 0 || discountPercent > 100)
                {
                    MessageBox.Show("Discount percentage must be between 0 and 100.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscount.Focus();
                    txtDiscount.SelectAll();
                    return;
                }

                // Loop through all selected rows
                foreach (DataGridViewRow selectedRow in gvDetail.SelectedRows)
                {
                    if (selectedRow.IsNewRow) continue; // Skip the 'new row' placeholder if selected

                    // Get Price and Quantity for the current selected row
                    object priceValue = selectedRow.Cells["Price"].Value;
                    object quantityValue = selectedRow.Cells["QuantityDetail"].Value;

                    if (priceValue != null && quantityValue != null &&
                        double.TryParse(priceValue.ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double price) && // Use invariant culture for parsing price
                        double.TryParse(quantityValue.ToString(), out double quantity))
                    {
                        // Calculate new SubTotal using the percentage
                        double newSubTotal = price * quantity * (1 - (discountPercent / 100.0));

                        // Apply discount percentage and new subtotal to the row
                        selectedRow.Cells["DiscountAmount"].Value = discountPercent; // Store the percentage
                        selectedRow.Cells["SubTotal"].Value = newSubTotal;

                        Console.WriteLine($"Applied percentage discount: Row {selectedRow.Index}, Discount %: {discountPercent}, New SubTotal: {newSubTotal}");
                    }
                    else
                    {
                        // Log or inform about rows where price/quantity couldn't be read
                        Console.WriteLine($"Warning: Could not read Price or Quantity for selected row {selectedRow.Index}. Discount not applied.");
                        // Optionally show a single message after the loop if any rows failed
                    }
                }
                gvDetail.Refresh(); // Refresh the entire grid to show changes in all selected rows
            }
            else
            {
                // Allow clearing the discount by entering 0 or empty string for all selected rows
                if (string.IsNullOrWhiteSpace(txtDiscount.Text) || txtDiscount.Text == "0") // Check against txtDiscount.Text directly
                {
                    foreach (DataGridViewRow selectedRow in gvDetail.SelectedRows)
                    {
                         if (selectedRow.IsNewRow) continue;

                         object priceValue = selectedRow.Cells["Price"].Value;
                         object quantityValue = selectedRow.Cells["QuantityDetail"].Value;

                         if (priceValue != null && quantityValue != null &&
                             double.TryParse(priceValue.ToString().Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double price) &&
                             double.TryParse(quantityValue.ToString(), out double quantity))
                         {
                             selectedRow.Cells["DiscountAmount"].Value = 0.0; // Clear percentage
                             selectedRow.Cells["SubTotal"].Value = price * quantity; // Reset subtotal
                             Console.WriteLine($"Cleared percentage discount: Row {selectedRow.Index}, New SubTotal: {selectedRow.Cells["SubTotal"].Value}");
                         }
                         else
                         {
                              Console.WriteLine($"Warning: Could not read Price or Quantity for selected row {selectedRow.Index} while clearing discount.");
                         }
                    }
                    gvDetail.Refresh(); // Refresh grid after clearing
                }
                else // Parsing failed for non-zero/non-empty input
                {
                    MessageBox.Show("Invalid discount percentage. Please enter a number between 0 and 100.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiscount.Focus();
                    txtDiscount.SelectAll();
                }
            }
        }


        private void txtDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ApplyPercentageDiscountToSelectedRow(); // Call renamed method
                e.SuppressKeyPress = true; // Prevent the 'ding' sound on Enter
                // Optionally move focus to the next control or back to the grid
                // gvDetail.Focus();
            }
        }


        private void txtDiscount_Leave(object sender, EventArgs e)
        {
             ApplyPercentageDiscountToSelectedRow(); // Call renamed method
             // Note: The ApplyDiscountToGrid(discountPercent) logic is commented out.
        }

        // Event handler for formatting cells in gvDetail
        private void GvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the column is SubTotal and the value is a number
            if (gvDetail.Columns[e.ColumnIndex].Name == "SubTotal" && e.Value != null)
            {
                if (double.TryParse(e.Value.ToString(), out double value))
                {
                    // Format as number with thousands separators and no decimal places (VND style)
                    e.Value = value.ToString("N0");
                    e.FormattingApplied = true; // Indicate that formatting is handled
                }
            }
            // Format Price column with 2 decimal places
            else if (gvDetail.Columns[e.ColumnIndex].Name == "Price" && e.Value != null)
            {
                 if (double.TryParse(e.Value.ToString(), out double value))
                {
                    // Format as number with thousands separators and 2 decimal places
                    e.Value = value.ToString("N0"); // Changed format to N2
                    e.FormattingApplied = true; // Indicate that formatting is handled
                 }
            }
             // Format DiscountAmount column (percentage) - Display as plain number
            else if (gvDetail.Columns[e.ColumnIndex].Name == "DiscountAmount" && e.Value != null)
            {
                 if (double.TryParse(e.Value.ToString(), out double value)) // Use double for TryParse flexibility
                 {
                    // Display as plain number (e.g., 10 for 10%)
                    e.Value = value.ToString(); // Display the number directly
                    e.FormattingApplied = true;
                 }
            }
        }
    }
}
