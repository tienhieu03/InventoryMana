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

namespace STOCK.PosControls
{
    public partial class frmReturn : MaterialForm
    {
        public frmReturn()
        {
            InitializeComponent();
        }
        public frmReturn(List<obj_INVOICE_DETAIL> lstInvoice, DataGridView dgvRetail)
        {
            InitializeComponent();
            this._lstInvoiceDetail = lstInvoice;
            this._dgvRetail = dgvRetail;
        }
        List<obj_INVOICE_DETAIL> _lstInvoiceDetail;
        DataGridView _dgvRetail;

        private void frmReturn_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            obj_INVOICE_DETAIL obj;
            var item = _lstInvoiceDetail.FirstOrDefault(x => x.BARCODE == txtBarcode.Text);
            if (item != null)
            {
                obj = new obj_INVOICE_DETAIL();
                obj.BARCODE = item.BARCODE;
                obj.ProductName = item.ProductName;
                obj.Unit = item.Unit;
                obj.Quantity = int.Parse("-" + txtQuantity.Text);
                obj.Price = item.Price;
                obj.SubTotal = obj.Quantity * obj.Price;
                if (item.Quantity < int.Parse(txtQuantity.Text))
                {
                    MessageBox.Show("Số lượng trả không được lớn hơn số lượng mua.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; // Exit the method if quantity is invalid
                }
                _lstInvoiceDetail.Add(obj);
                _dgvRetail.DataSource = _lstInvoiceDetail.ToList();
            }
            else
            {
                MessageBox.Show("Mã hàng không có trong đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
