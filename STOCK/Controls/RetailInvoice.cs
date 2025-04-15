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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {

        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!myFunctions.sIsNumber(txtBarcode.Text))
                {

                }
            }
        }
    }
}
