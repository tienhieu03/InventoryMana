using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataLayer;
using BusinessLayer;
using BusinessLayer.DataModels;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using BusinessLayer.Utils;

namespace STOCK.Controls
{
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
        }

        tb_SYS_USER _user;
        int _right;
        bool _add;
        int _productID;
        SUPPLIER _supplier;
        UNIT _unit;
        PRODUCT_CATEGORY _category;
        ORIGIN _origin;
        SYS_SEQ _sysSeq;
        tb_SYS_SEQ _seq;
        PRODUCT _product;

        void loadCategory()
        {
            cboCategory.ComboBox.DataSource = _category.getAll();
            cboCategory.ComboBox.DisplayMember = "Category";
            cboCategory.ComboBox.ValueMember = "CategoryID";
        }

        void loadSupplier()
        {
            cboSupplier.DataSource = _supplier.getList();
            cboSupplier.DisplayMember = "SupplierName";
            cboSupplier.ValueMember = "SupplierID";
        }

        void loadUnit()
        {
            cboUnit.DataSource = _unit.getList();
            cboUnit.DisplayMember = "UnitName";
            cboUnit.ValueMember = "UnitID";
        }

        void loadOrigin()
        {
            cboOrigin.DataSource = _origin.getAll();
            cboOrigin.DisplayMember = "OriginName";
            cboOrigin.ValueMember = "OriginID";
        }


        void ShowHideControls(bool t)
        {
            btnAdd.Visible = t;
            btnEdit.Visible = t;
            btnDelete.Visible = t;
            btnSave.Visible = !t;
            btnCancel.Visible = !t;
        }

        void _enable(bool t)
        {
            txtName.Enabled = t;
            txtShortName.Enabled = t;
            rtxtDetail.Enabled = t;
            nudPrice.Enabled = t;
            cboSupplier.Enabled = t;
            cboUnit.Enabled = t;
            cboOrigin.Enabled = t;
            chkDisable.Enabled = t;
        }

        void ResetFields()
        {
            txtBarcode.Text = "";
            txtQrCode.Text = "";
            txtName.Text = "";
            txtShortName.Text = "";
            rtxtDetail.Text = "";
            nudPrice.Value = 0;
            chkDisable.Checked = false;
        }

        private void loadData()
        {
            gvList.AutoGenerateColumns = false;
            if (cboCategory.ComboBox.SelectedValue != null)
            {
                int categoryId;
                if (int.TryParse(cboCategory.ComboBox.SelectedValue.ToString(), out categoryId))
                {
                    gvList.DataSource = _product.getListByCategory(categoryId);
                }
            }
            gvList.ReadOnly = true;
            gvList.ClearSelection();
        }


        private void ProductControl_Load(object sender, EventArgs e)
        {
            _supplier = new SUPPLIER();
            _unit = new UNIT();
            _category = new PRODUCT_CATEGORY();
            _origin = new ORIGIN();
            _product = new PRODUCT();
            _sysSeq = new SYS_SEQ();
            _enable(false);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ShowHideControls(true);
            loadCategory();
            loadData();

            loadSupplier();
            loadUnit();
            loadOrigin();

            cboCategory.SelectedIndexChanged += cboCategory_SelectedIndexChanged;
            gvList.ClearSelection();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            _add = true;
            _enable(true);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ResetFields();
            ShowHideControls(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                int productID = Convert.ToInt32(gvList.SelectedRows[0].Cells["ProductID"].Value);

                if (MessageBox.Show("Do you want to delete this record?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _product.delete(productID);
                    loadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(true);
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
            ShowHideControls(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_add)
            {
                tb_Product pd = new tb_Product();
                _seq = _sysSeq.getItem("PD@" + DateTime.Now.Year.ToString() + "@" + cboCategory.ComboBox.SelectedValue.ToString());
                if (_seq == null)
                {
                    _seq = new tb_SYS_SEQ();
                    _seq.SEQNAME = "PD@" + DateTime.Now.Year.ToString() + "@" + cboCategory.ComboBox.SelectedValue.ToString();
                    _seq.SEQVALUE = 1;
                    _sysSeq.add(_seq);
                }
                pd.BARCODE = BarcodeEAN13.BuildEan13(DateTime.Now.Year.ToString() + cboCategory.ComboBox.SelectedValue.ToString() + _seq.SEQVALUE.Value.ToString("0000000"));
                pd.ProductName = txtName.Text;
                pd.ShortName = txtShortName.Text;
                pd.CategoryID = int.Parse(cboCategory.ComboBox.SelectedValue.ToString());
                pd.Description = rtxtDetail.Text;
                pd.SupplierID = int.Parse(cboSupplier.SelectedValue.ToString());
                pd.OriginID = int.Parse(cboOrigin.SelectedValue.ToString());
                pd.Unit = cboUnit.Text;
                pd.IsDisabled = chkDisable.Checked;
                pd.CreatedDate = DateTime.Now;
                pd.Price = float.Parse(nudPrice.Value.ToString());
                _product.add(pd);
                txtBarcode.Text = pd.BARCODE;
                _sysSeq.udpate(_seq);
                MessageBox.Show(pd.BARCODE);

            }
            else
            {
                tb_Product product = _product.getItem(_productID);
            }
            _add = false;
            _enable(false);
            loadData();
            ResetFields();
            ShowHideControls(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _add = false;
            _enable(false);
            ShowHideControls(true);
            ResetFields();
            txtBarcode.Enabled = false;
            txtQrCode.Enabled = false;
        }

        private void gvList_Click(object sender, EventArgs e)
        {
            if (gvList.SelectedRows.Count > 0)
            {
                DataGridViewRow row = gvList.SelectedRows[0];

                _productID = (int)row.Cells["ProductID"].Value;
                cboCategory.ComboBox.SelectedValue = row.Cells["CategoryID"].Value;
                txtBarcode.Text = row.Cells["BARCODE"].Value.ToString();
                txtQrCode.Text = row.Cells["QRCODE"].Value.ToString();
                txtName.Text = row.Cells["ProductName"].Value.ToString();
                txtShortName.Text = row.Cells["ShortName"].Value.ToString();
                rtxtDetail.Text = row.Cells["Description"].Value.ToString();
                cboSupplier.SelectedValue = row.Cells["SupplierID"].Value;
                cboUnit.SelectedValue = row.Cells["Unit"].Value;
                cboOrigin.SelectedValue = row.Cells["OriginID"].Value;
                nudPrice.Value = Convert.ToDecimal(row.Cells["Price"].Value);

                object isDisabledValue = row.Cells["IsDisabled"].Value;
                chkDisable.Checked = (isDisabledValue != null) && Convert.ToBoolean(isDisabledValue);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
