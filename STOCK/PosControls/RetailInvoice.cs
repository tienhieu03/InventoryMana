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
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using STOCK.DataSet;
using STOCK.StockHelpers;
using STOCK.PosControls;
using Microsoft.Reporting.WinForms;
using STOCK.Forms;

namespace STOCK.Controls
{
    public partial class RetailInvoice : UserControl
    {
        public RetailInvoice()
        {
            InitializeComponent();
        }
        public RetailInvoice(tb_SYS_USER user, int right) // Renamed parameters for clarity
        {
            InitializeComponent();
            this._user = user; // Assign the passed user object to the class field
            this._right = right; // Assign the passed right value to the class field
        }
        tb_SYS_USER _user;
        int _right;
        // List<tb_Invoice> _lstInvoice; // Replaced by _fullInvoiceListForPeriod
        List<tb_Invoice> _fullInvoiceListForPeriod; // Stores the full list for filtering
        // Dictionary<int, tb_Customer> _allCustomersDict; // Removed as CustomerID is not in tb_Invoice for filtering

        string err = "";
        string discount;
        COMPANY _company;
        DEPARTMENT _department;
        PRODUCT _product;
        INVOICE_DETAIL _invoiceDetail;
        INVOICE _invoice;

        BindingSource _bsInvoiceDT;
        BindingSource _bsInvoice;

        Guid _pinvoiceID;

        tb_SYS_SEQ _seq;
        SYS_SEQ _sequence;

        List<obj_INVOICE_DETAIL> lstInvoiceDetail;

        // Thêm biến điều khiển cho phép chuyển tab hay không
        bool _allowTabChange = false;
        private void RetailInvoice_Load(object sender, EventArgs e)
        {
            dgvRetail.AutoGenerateColumns = false;

            _company = new COMPANY();
            _department = new DEPARTMENT();
            _product = new PRODUCT();
            _invoice = new INVOICE();
            _invoiceDetail = new INVOICE_DETAIL();
            _sequence = new SYS_SEQ();
            lstInvoiceDetail = new List<obj_INVOICE_DETAIL>();

            // Initialize BindingSources
            _bsInvoice = new BindingSource();
            _bsInvoiceDT = new BindingSource();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvRetail.RowCount == 0)
            {
                MessageBox.Show("Chi tiết đơn hàng không được rỗng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Keep a reference to the details before clearing/saving might modify them implicitly
            List<obj_INVOICE_DETAIL> detailsToPrint = lstInvoiceDetail.ToList();

            saveData(); // This sets _pinvoiceID

            // Retrieve the saved invoice using the ID set in saveData
            tb_Invoice savedInvoice = _invoice.getItem(_pinvoiceID);
            if (savedInvoice == null)
            {
                MessageBox.Show("Failed to retrieve the saved invoice for printing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Clear the list even if printing fails, as saveData was called
                lstInvoiceDetail = new List<obj_INVOICE_DETAIL>();
                dgvRetail.DataSource = lstInvoiceDetail;
                UpdateTotals(); // Update totals after clearing
                return;
            }

            // Call the modified exportReport, passing the retrieved data
            exportReport("PHIEU_BANLE_VP", "Phiếu bán lẻ", savedInvoice, detailsToPrint);

            // Clear the list and grid *after* potentially successful printing
            lstInvoiceDetail = new List<obj_INVOICE_DETAIL>();
            dgvRetail.DataSource = lstInvoiceDetail;
            UpdateTotals(); // Update totals after clearing
        }

        // Modified exportReport to accept invoice and details directly
        private void exportReport(string _reportName, string _tieude, tb_Invoice currentInvoice, List<obj_INVOICE_DETAIL> details)
        {
            // Parameter validation
            if (currentInvoice == null)
            {
                MessageBox.Show("Invoice data is missing for the report.", "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (details == null || details.Count == 0)
            {
                 // Optionally try to reload details if needed as a fallback
                 details = _invoiceDetail.getListbyIDFull(currentInvoice.InvoiceID);
                 if (details == null || details.Count == 0)
                 {
                    MessageBox.Show("Invoice details are missing or empty for the report.", "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                 }
            }

            // Fetch related data using the passed currentInvoice
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
            dtReportData.Columns.Add("DepartmentPhone", typeof(string));
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
            report.ReportEmbeddedResource = "STOCK.RDLCReport.Retail.rdlc"; // Updated path

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dsRetailInvoice"; // This MUST match the DataSet Name in the RDLC file
            rds.Value = dtReportData;
            report.DataSources.Clear();
            report.DataSources.Add(rds);

            report.Refresh(); // Refresh the report definition and data

            // Create and show the report viewer form
            try
            {
                formReportViewer frmViewer = new formReportViewer(report);
                frmViewer.Text = string.IsNullOrEmpty(_tieude) ? "Retail Report" : _tieude; // Set form title
                frmViewer.ShowDialog(); // Show the form modally
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening report preview: {ex.Message}", "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvRetail.RowCount == 0)
            {
                MessageBox.Show("Chi tiết đơn hàng không được rỗng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            saveData();
            lstInvoiceDetail = new List<obj_INVOICE_DETAIL>();
            dgvRetail.DataSource = lstInvoiceDetail;
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(dgvRetail);
            frm.ShowDialog();
            discount = frm.txtDiscount.Text;
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            frmReturn frm = new frmReturn(lstInvoiceDetail, dgvRetail);
            frm.ShowDialog();
            UpdateTotals(); // Add this line to update totals after return
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            int index = 0;
            if(e.KeyCode == Keys.Enter)
            {
                if (!myFunctions.sIsNumber(txtBarcode.Text))
                {
                    MessageBox.Show("Mã hàng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                var hh = _product.getItemBarcode(txtBarcode.Text);
                if (hh == null)
                {
                    MessageBox.Show("Mã hàng không có trong danh mục.","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                obj_INVOICE_DETAIL invoiceDetail = new obj_INVOICE_DETAIL();
                obj_PRODUCT _pd = new obj_PRODUCT();

                _pd = _product.getItemFull(txtBarcode.Text);
                invoiceDetail.ProductID = _pd.ProductID;
                invoiceDetail.BARCODE = _pd.BARCODE;
                invoiceDetail.ProductName = _pd.ProductName;
                invoiceDetail.Unit = _pd.Unit;
                invoiceDetail.Quantity = 1;
                invoiceDetail.Price = _pd.Price;
                invoiceDetail.SubTotal = invoiceDetail.Quantity * invoiceDetail.Price;
                if(lstInvoiceDetail.Count > 0)
                {
                    var existingItem = lstInvoiceDetail.FirstOrDefault(x => x.BARCODE == txtBarcode.Text);
                    if (existingItem != null)
                    {
                        index = lstInvoiceDetail.IndexOf(existingItem);
                        lstInvoiceDetail[index].Quantity = existingItem.Quantity + 1;
                        lstInvoiceDetail[index].SubTotal = lstInvoiceDetail[index].Quantity * existingItem.Price;
                    }
                    else
                    {
                        lstInvoiceDetail.Add(invoiceDetail);
                    }
                }
                else
                    lstInvoiceDetail.Add(invoiceDetail);

                dgvRetail.DataSource = lstInvoiceDetail.ToList();
                UpdateTotals(); // Add this line to update totals
                txtBarcode.Clear();
            }
        }

        private void UpdateTotals()
        {
            double totalQuantity = 0;
            double totalPrice = 0;

            foreach (var item in lstInvoiceDetail)
            {
                // Use ?? 0 to handle potential null values
                totalQuantity += item.Quantity ?? 0;
                totalPrice += item.SubTotal ?? 0;
            }

            // Assuming txtTotalQuantity and txtTotalPrice are accessible TextBoxes in your Designer.cs
            // If not, you might need to declare them or ensure they are correctly named.
            // Check RetailInvoice.Designer.cs for the exact names if errors occur.
            txtTotalQuantity.Text = totalQuantity.ToString();
            txtTotalPrice.Text = totalPrice.ToString("N0"); // Format as number with thousand separators, no decimals
        }

        private void saveData()
        {
            tb_Invoice invoice  = new tb_Invoice();
            Invoice_Infor(invoice);
            var result = _invoice.add(invoice);
            _pinvoiceID = result.InvoiceID;
            _sequence.udpate(_seq);
            InvoiceDetail_Infor(result);
        }
        void InvoiceDetail_Infor(tb_Invoice invoice)
        {
            _invoiceDetail.deleteAll(invoice.InvoiceID); // Assuming deleteAll works correctly
            int stt = 1;
            foreach (var item in lstInvoiceDetail)
            {
                tb_InvoiceDetail _ct = new tb_InvoiceDetail();
                _ct.InvoiceDetail_ID = Guid.NewGuid(); // Simpler way to generate Guid
                _ct.InvoiceID = invoice.InvoiceID; // Use the correct InvoiceID from the saved invoice
                _ct.STT = stt++;
                _ct.Day = DateTime.Now; // Consider if this should be invoice.Day
                _ct.BARCODE = item.BARCODE;
                _ct.Quantity = item.Quantity ?? 0; // Handle potential null
                _ct.Price = item.Price ?? 0; // Handle potential null
                _ct.SubTotal = item.SubTotal ?? 0; // Add SubTotal field if it exists in tb_InvoiceDetail, otherwise calculate
                // _ct.DiscountAmount = item.DiscountAmount ?? 0; // Assuming obj_INVOICE_DETAIL has DiscountAmount, handle null
                // If tb_InvoiceDetail doesn't have SubTotal, calculate it:
                // _ct.SubTotal = (_ct.Quantity * _ct.Price) - (_ct.DiscountAmount ?? 0);
                _invoiceDetail.add(_ct);
            }
        }
        void Invoice_Infor(tb_Invoice invoice)
        {
            string departmentid = "";
            if (myFunctions._dpid == "~")
            {
                departmentid = "DW";
            }
            else
            {
                departmentid = myFunctions._dpid;
            }
            double _TONGCONG = 0;
            tb_Department dp = _department.getItem(departmentid);

            // Check if department exists before proceeding
            if (dp == null)
            {
                MessageBox.Show($"Không tìm thấy phòng ban với ID: {departmentid}. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception($"Department not found with ID: {departmentid}");
            }

            _seq = _sequence.getItem("BLE@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
            if (_seq == null)
            {
                _seq = new tb_SYS_SEQ();
                _seq.SEQNAME = "BLE@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol; // dp is guaranteed not null here
                _seq.SEQVALUE = 1;
                _sequence.add(_seq);
            }
            invoice.InvoiceID = Guid.NewGuid();
            invoice.Day = DateTime.Now;
            invoice.Invoice = _seq.SEQVALUE.Value.ToString("000000") + @"/" + DateTime.Today.Year.ToString().Substring(2, 2) + @"/BLE/" + dp.Symbol;
            invoice.CreatedBy = _user.UserID; // Ensure _user is assigned before calling saveData
            invoice.CreatedDate = DateTime.Now;
            invoice.InvoiceType = 4; // Consider making this dynamic or a constant
            if (!string.IsNullOrEmpty(discount)) // Check if discount string has value
            {
                // Safely parse the discount string
                if (double.TryParse(discount, out double discountAmount))
                {
                    invoice.DiscountAmount = (int)discountAmount; // Cast double to int?
                }
                else
                {
                    // Handle parsing error, maybe log it or default to 0
                    invoice.DiscountAmount = 0;
                    // Consider showing a message to the user about invalid discount format
                }
            }
            else
            {
                invoice.DiscountAmount = 0; // Default discount if not provided
            }
            
            invoice.CompanyID = myFunctions._compid; 
            invoice.DepartmentID = departmentid;
            invoice.ReceivingDepartmentID = "1";
            invoice.Status = 2; // Consider if this should be dynamic
            //invoice.GHICHU = txtGhiChu.Text; // Uncomment and use if a notes field exists

            // Calculate totals from lstInvoiceDetail instead of dgvRetail
            double totalQuantity = 0;
            double totalPrice = 0;
            foreach (var item in lstInvoiceDetail)
            {
                totalQuantity += item.Quantity ?? 0;
                totalPrice += item.SubTotal ?? 0;
            }
            invoice.Quantity = (int)totalQuantity; // Cast double to int
            invoice.TotalPrice = totalPrice;

            // The loop iterating dgvRetail is no longer needed for totals

            invoice.UpdatedBy = _user.UserID; // Ensure _user is assigned
            invoice.UpdatedDate = DateTime.Now;
        }
    }
}
