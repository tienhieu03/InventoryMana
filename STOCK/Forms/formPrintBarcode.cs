using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using BusinessLayer;
using BusinessLayer.DataModels;

namespace STOCK.Forms
{
    public partial class formPrintBarcode : MaterialForm
    {
        public formPrintBarcode()
        {
            InitializeComponent();
        }

        PRODUCT_CATEGORY _category;
        PRODUCT _product;

        private void formPrintBarcode_Load(object sender, EventArgs e)
        {
            _category = new PRODUCT_CATEGORY();
            _product = new PRODUCT();
            LoadCate();
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            LoadList();
        }

        void LoadList()
        {
            dgvList.DataSource = _product.getListByCategory(int.Parse(cboCategory.SelectedValue.ToString()));
        }

        private void CboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }

        void LoadCate()
        {
            cboCategory.DataSource = _category.getAll();
            cboCategory.DisplayMember = "Category";
            cboCategory.ValueMember = "CategoryID";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
