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

namespace STOCK.Controls
{
    public partial class RetailInvoice : UserControl
    {
        public RetailInvoice()
        {
            InitializeComponent();
        }
        public RetailInvoice(tb_SYS_USER _user, int _right)
        {
            InitializeComponent();
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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

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
            string dpid = "";
            if (myFunctions._dpid == "~")
            {
                dpid = "DW01";
            }
            else
            {
                dpid = myFunctions._dpid;
            }
            double _TONGCONG = 0;
            tb_Department dp = _department.getItem(dpid);
            _seq = _sequence.getItem("BLE@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
            if (_seq == null)
            {
                _seq = new tb_SYS_SEQ();
                _seq.SEQNAME = "BLE@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol;
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
            invoice.CompanyID = myFunctions._compid; // Ensure myFunctions._compid is set
            invoice.DepartmentID = dpid;
            invoice.ReceivingDepartmentID = "1"; // Consider if this should be dynamic
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
