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
using MATERIAL.MyFunctions;
using MaterialSkin.Controls;

namespace STOCK.PosControls
{
    public partial class frmDiscount : MaterialForm
    {
        public frmDiscount()
        {
            InitializeComponent();
        }
        string discount;
        public frmDiscount(DataGridView dgvRetail)
        {
            InitializeComponent();
            this._dgvDetail = dgvRetail;
        }
        List<obj_INVOICE_DETAIL> lstIV;
        DataGridView _dgvDetail;
        private void frmDiscount_Load(object sender, EventArgs e)
        {
            txtDiscount.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (myFunctions.sIsNumber(txtDiscount.Text))
            {
                discount = txtDiscount.Text;
                double discountPercent = double.Parse(discount);

                for (int i = 0; i < _dgvDetail.RowCount; i++)
                {
                    var price = Convert.ToDouble(_dgvDetail.Rows[i].Cells["Price"].Value);
                    var quantity = Convert.ToDouble(_dgvDetail.Rows[i].Cells["Quantity"].Value);

                    _dgvDetail.Rows[i].Cells["DiscountAmount"].Value = discount;
                    _dgvDetail.Rows[i].Cells["SubTotal"].Value = price * quantity * (1 - discountPercent / 100);
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Must be number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
