using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;
using BusinessLayer.DataModels;
using BusinessLayer.Utils;
using DataLayer;
using MATERIAL.MyFunctions;
using STOCK.Forms;
using STOCK.StockHelpers;
using STOCK.PopUpForm;
using Excel = Microsoft.Office.Interop.Excel;

namespace STOCK.Controls
{
    public partial class PurchaseInvoiceControl : UserControl
    {
        public PurchaseInvoiceControl()
        {
            InitializeComponent();
        }
        public PurchaseInvoiceControl(tb_SYS_USER user,int right)
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
        
      
        private void PurchaseInvoiceControl_Load(object sender, EventArgs e)
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
            cboCompany.SelectedIndexChanged += cboCompany_SelectedIndexChanged;

            _status = _STATUS.getList();
            cboStatus.DataSource = _status;
            cboStatus.DisplayMember = "_display";
            cboStatus.ValueMember = "_value";

            gvList.CellFormatting += gvList_CellFormatting;
            gvList.CellPainting += gvList_CellPainting;

            LoadWareHouse();
            LoadDepartment();
            LoadSupplier();
            _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboWarehouse.SelectedValue.ToString());
            _bsInvoice.DataSource = _lstInvoice;
            gvList.DataSource = _bsInvoice;

            exportInfor();
            cboPurchaseUnit.SelectedIndexChanged += cboPurchaseUnit_SelectedIndexChanged;
            cboWarehouse.SelectedIndexChanged += cboWarehouse_SelectedIndexChanged;
            gvDetail.CellMouseDown += gvDetail_CellMouseDown;
            gvDetail.CellValueChanged += gvDetail_CellValueChanged;
            gvDetail.CellEndEdit += gvDetail_CellEndEdit;  // Add this line
            cmdDeleteRow.Click += cmdDeleteRow_Click;
            cmdDeleteDetail.Click += cmdDeleteDetail_Click;

            // Set fixed row header width (optional)
            gvList.RowHeadersWidth = 25;
            gvDetail.RowHeadersWidth = 25;

            _enable(false);
            ShowHideControls(true);
            contextMenuDetail.Enabled = false;
            gvList.ClearSelection();
        }

        private void cboPurchaseUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void cboWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_gridData();
        }

        void load_gridData()
        {
            string madvi;
            if (cboWarehouse.SelectedValue != null)
            {
                madvi = cboWarehouse.SelectedValue.ToString();
                _enable(true);
            }
            else
            {
                madvi = "";
                _enable(false);
            }
            _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), madvi);
            _bsInvoice.DataSource = _lstInvoice;
            gvList.DataSource = _bsInvoice;
        }

        private void _bsInvoice_PositionChanged(object sender, EventArgs e)
        {
            if (!_add)
            {
                exportInfor();
            }
        }

        private void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWareHouse();
        }

        void LoadCompany()
        {
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
        }

        void LoadDepartment()
        {
            cboPurchaseUnit.DataSource = _department.getWarehoseByCp(cboCompany.SelectedValue.ToString());
            cboPurchaseUnit.DisplayMember = "DepartmentName";
            cboPurchaseUnit.ValueMember = "DepartmentID";
        }

        void LoadWareHouse()
        {
            var _lstStock = _department.getWarehoseByCp(cboCompany.SelectedValue.ToString());

            cboWarehouse.DataSource = _lstStock;
            cboWarehouse.DisplayMember = "DepartmentName";
            cboWarehouse.ValueMember = "DepartmentID";
            
            // Check if the list has any items before trying to set selected indices
            if(_lstStock != null && _lstStock.Count > 0)
            {
                LoadDepartment();
                cboWarehouse.SelectedIndex = 0;
                cboPurchaseUnit.SelectedIndex = 0;
            }
            else
            {
                // Handle the empty list case - perhaps clear other dependent controls
                LoadDepartment();
                // Don't try to set selected indices for empty lists
            }
        }

        void LoadSupplier()
        {
            cboSupplier.DataSource = _supplier.getList();
            cboSupplier.DisplayMember = "SupplierName";
            cboSupplier.ValueMember = "SupplierID";
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
            cboPurchaseUnit.Enabled = t;
            cboStatus.Enabled = t;
            cboSupplier.Enabled = t;
            dtDate.Enabled = t;
        }

        private void ResetFields()
        {
            txtInvoiceNo.Text = "";
            txtNote.Text = "";
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
            
            // Handle adding new row properly based on data source type
            if (_bsInvoiceDT.DataSource is List<obj_INVOICE_DETAIL> detailList)
            {
                // Create a new detail object and add it to the list
                obj_INVOICE_DETAIL newDetail = new obj_INVOICE_DETAIL();
                newDetail.STT = detailList.Count + 1;
                detailList.Add(newDetail);
                
                // Refresh the binding source to show the new row
                _bsInvoiceDT.ResetBindings(false);
            }
            else if (_bsInvoiceDT.DataSource is DataTable dt)
            {
                // Add a new row to the DataTable
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
            }
            else
            {
                // If no proper data source, create a new row directly in the grid
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
            contextMenuDetail.Enabled = true;

            _add = true;
            _edit = false;
            ShowHideControls(false);
            _enable(true);
            ResetFields();

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
                    contextMenuDetail.Enabled = true;
                    cboPurchaseUnit.Enabled = false;

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
                        _invoice.delete(invoiceID, _user.UserID); // Pass the user ID to the delete method
                        row.Cells["DeletedBy"].Value = _user.UserID; // Update the DeletedBy cell in the DataGridView
                        lblDelete.Visible = true;
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
            contextMenuDetail.Enabled = false;
            tabInvoice.TabPages[0].Enabled = true;
            ShowHideControls(true);
            _enable(false);
            tabInvoice.SelectedTab = pageList;
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
            contextMenuDetail.Enabled = false;
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

        void InvoiceDetail_Infor(tb_Invoice invoice)
        {
            // First delete all existing details for this invoice
            _invoiceDetail.deleteAllByInvoiceId(invoice.InvoiceID);

            List<tb_InvoiceDetail> detailsToAdd = new List<tb_InvoiceDetail>();

            // Find all column indices needed to safely access cells
            Dictionary<string, int> columnIndices = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            
            // List of columns we need to find (case-insensitive)
            string[] requiredColumns = new string[] { 
                "BARCODE", "ProductID", "ProductName", "Quantity", "Price", "TotalPrice", "Unit" 
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

                // Skip rows that are intentionally left blank (last row)
                if (i == gvDetail.Rows.Count - 1 && 
                    (columnIndices.TryGetValue("BARCODE", out int barcodeColIdx) && 
                     (row.Cells[barcodeColIdx].Value == null || 
                      string.IsNullOrWhiteSpace(row.Cells[barcodeColIdx].Value.ToString()))))
                    continue;

                // Skip rows that just have a "." in the BARCODE field
                if (columnIndices.TryGetValue("BARCODE", out int barcodeIdx) && 
                    row.Cells[barcodeIdx].Value?.ToString() == ".")
                    continue;
                    
                // Skip rows without product name or barcode
                if ((!columnIndices.ContainsKey("ProductName") || row.Cells[columnIndices["ProductName"]].Value == null) &&
                    (!columnIndices.ContainsKey("BARCODE") || 
                     row.Cells[columnIndices["BARCODE"]].Value == null || 
                     string.IsNullOrWhiteSpace(row.Cells[columnIndices["BARCODE"]].Value.ToString()) ||
                     row.Cells[columnIndices["BARCODE"]].Value.ToString() == "."))
                    continue;

                // Rest of the method remains the same...
                try
                {
                    tb_InvoiceDetail detail = new tb_InvoiceDetail
                    {
                        InvoiceDetail_ID = Guid.NewGuid(),
                        InvoiceID = invoice.InvoiceID,
                        STT = i + 1,
                        Day = dtDate.Value
                    };

                    // Get values using the column indices we found
                    if (columnIndices.TryGetValue("BARCODE", out int barIdx) && 
                        row.Cells[barIdx].Value != null &&
                        row.Cells[barIdx].Value.ToString() != ".")
                    {
                        detail.BARCODE = row.Cells[barIdx].Value.ToString();
                    }
                    else 
                    {
                        continue; // Skip rows without valid barcode
                    }

                    // Default quantity to 1 if not provided
                    if (columnIndices.TryGetValue("Quantity", out int qtyIdx) && row.Cells[qtyIdx].Value != null)
                    {
                        if (int.TryParse(row.Cells[qtyIdx].Value.ToString(), out int qty))
                            detail.Quantity = qty;
                        else
                            detail.Quantity = 1;
                    }
                    else
                        detail.Quantity = 1;

                    // Set price from grid or get from product if missing
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

                    // Calculate or get total price
                    if (columnIndices.TryGetValue("TotalPrice", out int totalPriceIdx) && row.Cells[totalPriceIdx].Value != null)
                    {
                        if (double.TryParse(row.Cells[totalPriceIdx].Value.ToString(), out double totalPrice))
                            detail.TotalPrice = totalPrice;
                        else
                            detail.TotalPrice = detail.Price * detail.Quantity; // Calculate if parsing fails
                    }
                    else
                        detail.TotalPrice = detail.Price * detail.Quantity; // Calculate if column doesn't exist

                    // Set ProductID
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
                                     $"Price={detail.Price}, TotalPrice={detail.TotalPrice}, ProductID={detail.ProductID}");

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
        }

        void Invoice_Infor(tb_Invoice invoice)
        {
            double _total = 0;
            tb_Department dp = _department.getItem(cboPurchaseUnit.SelectedValue.ToString());
            _seq = _sequence.getItem("NHM@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
            if (_seq == null)
            {
                _seq = new tb_SYS_SEQ();
                _seq.SEQNAME = "NHM@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol;
                _seq.SEQVALUE = 1;
                _sequence.add(_seq);
            }

            if (_add)
            {
                invoice.InvoiceID = Guid.NewGuid();
                invoice.Day = dtDate.Value;
                invoice.Invoice = _seq.SEQVALUE.Value.ToString("000000") + "/" + DateTime.Today.Year.ToString().Substring(2, 2) + "/NHM/" + dp.Symbol;
                invoice.CreatedBy = 1;
                invoice.CreatedDate = DateTime.Now;
            }

            invoice.InvoiceType = 1;
            invoice.CompanyID = cboCompany.SelectedValue.ToString();
            invoice.DepartmentID = cboPurchaseUnit.SelectedValue.ToString();
            invoice.ReceivingDepartmentID = cboSupplier.SelectedValue.ToString();
            invoice.Description = txtNote.Text;
            invoice.Status = Convert.ToInt32(cboStatus.SelectedValue);

            // Calculate total quantity - with robust column checking
            int totalQuantity = 0;
            
            // First, find the quantity column index to avoid using string indexer
            int quantityColIndex = -1;
            for (int i = 0; i < gvDetail.Columns.Count; i++)
            {
                if (gvDetail.Columns[i].Name.Equals("Quantity", StringComparison.OrdinalIgnoreCase))
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
                if (gvDetail.Columns[i].Name.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
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
            invoice.UpdatedBy = 1; //_user.UserID;
            invoice.UpdatedDate = DateTime.Now;
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
                cboPurchaseUnit.SelectedValue = current.DepartmentID;
                cboSupplier.SelectedValue = int.Parse(current.ReceivingDepartmentID);
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
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboWarehouse.SelectedValue.ToString());
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

        private void gvList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gvList.Columns[e.ColumnIndex].Name == "STATUS")
            {
                if (e.Value != null && e.Value.ToString() == "1")
                    e.Value = "Chưa hoàn tất";
                else
                    e.Value = "Đã hoàn tất";

                e.FormattingApplied = true; // Xác nhận đã xử lý format
            }
        }

        private void gvList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (gvList.Columns[e.ColumnIndex].Name == "DELETED_BY" && e.Value != null)
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

            //    Image img = Properties.Resources.del_Icon_x16; // Ảnh xóa
            //    int imgX = e.CellBounds.X + (e.CellBounds.Width - img.Width) / 2;
            //    int imgY = e.CellBounds.Y + (e.CellBounds.Height - img.Height) / 2;

            //    e.Graphics.DrawImage(img, new Point(imgX, imgY));
            //    e.Handled = true;
            //}
        }

        private void gvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
                    try { 
                        barcodeValue = gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value; 
                    } catch { return; }
                    
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
                                    if (gvDetail.Columns[i].Name.Equals("Quantity", StringComparison.OrdinalIgnoreCase))
                                        quantityColIndex = i;
                                    else if (gvDetail.Columns[i].Name.Equals("Price", StringComparison.OrdinalIgnoreCase))
                                        priceColIndex = i;
                                    else if (gvDetail.Columns[i].Name.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
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
                                Console.WriteLine($"Error updating TotalPrice after form close: {ex.Message}");
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
                                row.Cells["Quantity"].Value = 1;
                                row.Cells["Price"].Value = pd.Price;
                                row.Cells["TotalPrice"].Value = pd.Price;
                                
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
                if (columnName == "Quantity" || columnName == "Price")
                {
                    object barcodeValue = gvDetail.Rows[e.RowIndex].Cells["BARCODE"]?.Value;
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["Quantity"]?.Value;
                    object priceValue = gvDetail.Rows[e.RowIndex].Cells["Price"]?.Value;

                    if (barcodeValue != null && quantityValue != null && priceValue != null)
                    {
                        if (double.TryParse(quantityValue.ToString(), out double quantity) && 
                            double.TryParse(priceValue.ToString(), out double price))
                        {
                            if (quantity == 0 && columnName == "Quantity")
                            {
                                MessageBox.Show("Số lượng sản phẩm không thể bằng 0", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                gvDetail.Rows[e.RowIndex].Cells["Quantity"].Value = 1;
                                quantity = 1;
                            }
                            
                            // Calculate and update total price
                            double totalPrice = price * quantity;
                            gvDetail.Rows[e.RowIndex].Cells["TotalPrice"].Value = totalPrice;
                            
                            Console.WriteLine($"Updated TotalPrice for row {e.RowIndex}: {totalPrice} = {price} * {quantity}");
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

        // Add this new method to ensure there's always an empty row at the bottom
        private void EnsureEmptyRowAtBottom()
        {
            // Only do this in add or edit mode
            if (!_add && !_edit)
                return;
            
            // Use the utility method from DataGridViewHelper
            DataGridViewHelper.EnsureEmptyRowAtBottom(gvDetail);
        }

        private void cmdDeleteRow_Click(object sender, EventArgs e)
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


        private void cmdDeleteDetail_Click(object sender, EventArgs e)
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


        private void cmdImport_Click(object sender, EventArgs e)
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
                        row.Cells[gvDetail.Columns["Quantity"].Index].Value = _quantity;
                        row.Cells[gvDetail.Columns["Price"].Index].Value = _h.Price;
                        row.Cells[gvDetail.Columns["TotalPrice"].Index].Value = _h.Price * _quantity;

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

        private void tabInvoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_edit == false && tabInvoice.SelectedTab == pageDetail)
            {
                gvDetail.ReadOnly = true; // Nếu gvDetail là DataGridView
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
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboWarehouse.SelectedValue.ToString());
                _bsInvoice.DataSource = _lstInvoice;
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
                _lstInvoice = _invoice.getList(1, dtFrom.Value, dtTill.Value.AddDays(1), cboPurchaseUnit.SelectedValue.ToString());
                _bsInvoice.DataSource = _lstInvoice;
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateNotFuture(dtDate);
        }

        private void dtDate_Leave(object sender, EventArgs e)
        {
            FormValidationHelper.ValidateDateNotFuture(dtDate);
        }
        bool cal(int width, DataGridView dgv)
        {
            dgv.RowHeadersWidth = Math.Max(dgv.RowHeadersWidth, width);
            return true;
        }

        private void exportReport(string _reportName, string _tieude)
        {
            //if (_pinvoiceID != null)
            //{
            //    Form frm = new Form();
            //    CrystalReportViewer Crv = new CrystalReportViewer();
            //    Crv.ShowGroupTreeButton = false;
            //    Crv.ShowParameterPanelButton = false;
            //    Crv.ToolPanelView = ToolPanelViewType.None;
            //    TableLogOnInfo Thongtin;
            //    ReportDocument doc = new ReportDocument();
            //    doc.Load(Application.StartupPath + "\\Reports\\" + _reportName + @".rpt");
            //    Thongtin = doc.Database.Tables[0].LogOnInfo;
            //    Thongtin.ConnectionInfo.ServerName = myFunctions._srv;
            //    Thongtin.ConnectionInfo.DatabaseName = myFunctions._db;
            //    Thongtin.ConnectionInfo.UserID = myFunctions._us;
            //    Thongtin.ConnectionInfo.Password = myFunctions._pw;
            //    doc.Database.Tables[0].ApplyLogOnInfo(Thongtin);
            //    try
            //    {
            //        doc.SetParameterValue("khoa", "{" + _pinvoiceID.ToString() + "}");
            //        Crv.Dock = DockStyle.Fill;
            //        Crv.ReportSource = doc;
            //        frm.Controls.Add(Crv);
            //        Crv.Refresh();
            //        frm.Text = _tieude;
            //        frm.WindowState = FormWindowState.Maximized;
            //        frm.ShowDialog();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Lỗi: " + ex.ToString());
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Không có dữ liệu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void gvDetail_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Kiểm tra chuột phải & hàng hợp lệ
            {
                gvDetail.ClearSelection(); // Xóa chọn cũ
                gvDetail.Rows[e.RowIndex].Selected = true; // Chọn hàng hiện tại
                gvDetail.CurrentCell = gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex]; // Đặt ô hiện tại

                // Hiển thị context menu tại vị trí chuột
                contextMenuDetail.Show(Cursor.Position);
            }
        }

        private void gvList_DoubleClick(object sender, EventArgs e)
        {
            if (gvList.Rows.Count > 0)
            {
                tabInvoice.SelectedTab = pageDetail; // Chuyển tab
            }
        }

        // Add a method to handle data-bound grid value changes
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
                                item.TotalPrice = item.Quantity.Value * item.Price.Value;
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

        // Add this event handler to update TotalPrice when the user edits cells in the DataGridView
        private void gvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                string columnName = gvDetail.Columns[e.ColumnIndex].Name;
                
                // When Price or Quantity is edited, recalculate TotalPrice
                if (columnName.Equals("Quantity", StringComparison.OrdinalIgnoreCase) || 
                    columnName.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["Quantity"]?.Value;
                    object priceValue = gvDetail.Rows[e.RowIndex].Cells["Price"]?.Value;
                    
                    if (quantityValue != null && priceValue != null &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        double.TryParse(priceValue.ToString(), out double price))
                    {
                        double totalPrice = quantity * price;
                        gvDetail.Rows[e.RowIndex].Cells["TotalPrice"].Value = totalPrice;
                        Console.WriteLine($"Updated TotalPrice in cell edit: {totalPrice} = {quantity} * {price}");
                    }
                }
                // When TotalPrice is edited directly, update Price (assuming Quantity stays the same)
                else if (columnName.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
                {
                    object totalPriceValue = gvDetail.Rows[e.RowIndex].Cells["TotalPrice"]?.Value;
                    object quantityValue = gvDetail.Rows[e.RowIndex].Cells["Quantity"]?.Value;
                    
                    if (totalPriceValue != null && quantityValue != null &&
                        double.TryParse(totalPriceValue.ToString(), out double totalPrice) &&
                        double.TryParse(quantityValue.ToString(), out double quantity) &&
                        quantity > 0)
                    {
                        double price = totalPrice / quantity;
                        gvDetail.Rows[e.RowIndex].Cells["Price"].Value = price;
                        Console.WriteLine($"Updated Price based on TotalPrice: {price} = {totalPrice} / {quantity}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in gvDetail_CellEndEdit: {ex.Message}");
            }
        }

        // Add this new method to update all TotalPrice values
        private void UpdateAllTotalPrices()
        {
            DataGridViewHelper.UpdateAllTotalPrices(gvDetail);
        }
    }
}
