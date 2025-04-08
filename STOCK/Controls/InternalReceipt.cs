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
using STOCK.PopUpForm;

namespace STOCK.Controls
{
    public partial class InternalReceipt : UserControl
    {
        public InternalReceipt(tb_SYS_USER user, int right)
        {
            InitializeComponent();
            this._user = user;
            this._right = right;
        }

        tb_SYS_USER _user;
        int _right;
        bool _add = false;

        List<_STATUS> _status;
        List<tb_Invoice> _lstInvoice;

        COMPANY _company;
        DEPARTMENT _department;
        PRODUCT _product;
        INVOICE_DETAIL _invoiceDetail;
        INVOICE _invoice;
        BindingSource _bsInvoiceDT;
        BindingSource _bsInvoice;

        Guid _id;
        Guid _pinvoiceID;

        tb_SYS_SEQ _seq;
        SYS_SEQ _sequence;

        // Thêm biến điều khiển cho phép chuyển tab hay không
        bool _allowTabChange = false;

        private void InternalReceipt_Load(object sender, EventArgs e)
        {
            gvList.AutoGenerateColumns = false;
            gvDetail.AutoGenerateColumns = false;
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
            cboCompany.SelectedValue = myFunctions._compid;
            cboCompany.SelectedIndexChanged += CboCompany_SelectedIndexChanged;

            _status = _STATUS.getList();
            cboStatus.DataSource = _status;
            cboStatus.DisplayMember = "_display";
            cboStatus.ValueMember = "_value";

            gvList.CellFormatting += GvList_CellFormatting; ;
            gvList.CellPainting += GvList_CellPainting; ;

            // Thiết lập giá trị cho các cột trong DataGridView
            SetupDataGridViewColumns();

            LoadDepartment();
            LoadExport();
            LoadReceiveUnit();
            
            // Thiết lập giá trị cho cboExportUnit dựa trên myFunctions._dpid
            if (myFunctions._dpid == "~")
            {
                cboExportUnit.SelectedValue = "DW2";
                cboExportUnit.Enabled = false;
            }
            else
            {
                cboExportUnit.SelectedValue = myFunctions._dpid;
                cboExportUnit.Enabled = false;
            }
            
            // Thiết lập giá trị cho cboDepartment dựa trên myFunctions._dpid
            if (myFunctions._dpid != "~")
            {
                cboDepartment.SelectedValue = myFunctions._dpid;
            }
            
            // Đăng ký sự kiện SelectedIndexChanged sau khi đã thiết lập giá trị
            cboDepartment.SelectedIndexChanged += CboDepartment_SelectedIndexChanged;
            
            // Load dữ liệu invoice dựa trên department đã chọn
            LoadInvoiceData();

            // Set fixed row header width (optional)
            gvList.RowHeadersWidth = 25;
            gvDetail.RowHeadersWidth = 25;

            // Đăng ký sự kiện chuyển tab
            tabInvoice.Selecting += TabInvoice_Selecting; ;

            // Đăng ký sự kiện double click vào danh sách
            gvList.DoubleClick += GvList_DoubleClick; ;

            _enable(false);
            gvList.ClearSelection();

            // Update buttons based on user permissions
            UpdateButtonsByPermission();
            btnCreateCode.Enabled = false;
        }

        private void GvList_DoubleClick(object sender, EventArgs e)
        {
            if (gvList.Rows.Count > 0)
            {
                // Cho phép chuyển tab tạm thởi
                _allowTabChange = true;
                tabInvoice.SelectedTab = pageDetail;
                _allowTabChange = false;
            }
        }

        private void TabInvoice_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // Nếu đang cố gắng chuyển đến pageDetail nhưng không được phép
            if (e.TabPage == pageDetail && !_allowTabChange)
            {
                // Hủy thao tác chuyển tab
                e.Cancel = true;
            }
        }

        private void UpdateButtonsByPermission()
        {
            // Handle permissions based on _right value
            // 0 = Lock Function - All buttons disabled
            // 1 = View Only - Only view/export allowed
            // 2 = Full Function - All functions allowed

            if (_right == 0) // Lock Function
            {
                btnPrint.Enabled = false;
                btnCreateCode.Enabled = false;
            }
            else if (_right == 1) // View Only
            {
                btnCreateCode.Enabled = false;
                btnPrint.Enabled = true; // Allow printing/export
            }
            else // Full Function (2)
            {
                btnCreateCode.Enabled = true;
                btnPrint.Enabled = true;
            }
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

        private void CboDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadInvoiceData();
        }

        private void CboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment();     // Cập nhật cboDepartment
            LoadExport();         // Cập nhật cboExportUnit 
            LoadReceiveUnit();    // Cập nhật cboReceiveUnit

            // Nếu có tồn tại dữ liệu trong Department, mặc định chọn giá trị đầu tiên
            if (cboDepartment.Items.Count > 0)
            {
                cboDepartment.SelectedIndex = 0;
            }
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
            cboDepartment.DataSource = _department.getAll(cboCompany.SelectedValue.ToString());
            cboDepartment.DisplayMember = "DepartmentName";
            cboDepartment.ValueMember = "DepartmentID";
        }

        void LoadExport()
        {
            cboExportUnit.DataSource = _department.getAll(cboCompany.SelectedValue.ToString());
            cboExportUnit.DisplayMember = "DepartmentName";
            cboExportUnit.ValueMember = "DepartmentID";
        }

        void LoadReceiveUnit()
        {
            cboReceiveUnit.DataSource = _department.getDepartmentByCp(cboCompany.SelectedValue.ToString(), false);
            cboReceiveUnit.DisplayMember = "DepartmentName";
            cboReceiveUnit.ValueMember = "DepartmentID";
        }
        private void _enable(bool t)
        {
            txtNote.Enabled = t;
            cboExportUnit.Enabled = t;
            cboStatus.Enabled = t;
            cboReceiveUnit.Enabled = t;
            dtDate.Enabled = t;
            txtInvoiceNo.Enabled = t;
            dtDate.Enabled = t;
            dtReceiveDate.Enabled = t;
            txtReceiveInvoiceNo.Enabled = t;
        }
        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            if (_right != 2)
            {
                MessageBox.Show("You don't have permission to perform this operation", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                tb_Invoice invoice;
                string dpid = "";
                if (myFunctions._dpid == "~")
                {
                    dpid = "DW2";
                }
                else
                {
                    dpid = cboExportUnit.SelectedValue.ToString();
                }
                tb_Department dp = _department.getItem(dpid);

                _seq = _sequence.getItem("NNB@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol);
                if (_seq == null)
                {
                    _seq = new tb_SYS_SEQ();
                    _seq.SEQNAME = "NNB@" + DateTime.Today.Year.ToString() + "@" + dp.Symbol;
                    _seq.SEQVALUE = 1;
                    _sequence.add(_seq);
                }
                invoice = (tb_Invoice)_bsInvoice.Current;
                invoice = _invoice.getItem(invoice.InvoiceID);
                invoice.Invoice2 = _seq.SEQVALUE.Value.ToString("000000") + "/" + DateTime.Today.Year.ToString().Substring(2, 2) + "/NNB/" + dp.Symbol;
                invoice.Day2 = DateTime.Now;
                var resultInvoice = _invoice.update(invoice);
                _sequence.udpate(_seq);

                // Refresh the data in the grid
                _lstInvoice = _invoice.getReceiveInvoice(2, dtFrom.Value, dtTill.Value.AddDays(1), cboDepartment.SelectedValue.ToString());
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
                exportInfor();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }
        void exportInfor()
        {
            tb_Invoice current = (tb_Invoice)_bsInvoice.Current;
            if (current != null)
            {
                tb_Department dp = _department.getItem(current.DepartmentID);
                dtDate.Value = current.Day.Value;
                txtInvoiceNo.Text = current.Invoice2;
                txtNote.Text = current.Description;
                cboExportUnit.SelectedValue = current.DepartmentID;
                cboReceiveUnit.SelectedValue = current.ReceivingDepartmentID;
                if (current.Day2 != null)
                {
                    dtReceiveDate.Value = current.Day2.Value;
                }
                txtReceiveInvoiceNo.Text = current.Invoice2;
                cboStatus.SelectedValue = current.Status;

                if (current.Invoice2 != null)
                {
                    btnCreateCode.Enabled = false;
                }
                else
                {
                    btnCreateCode.Enabled = true;
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

        private void gvList_DoubleClick_1(object sender, EventArgs e)
        {
            tabInvoice.SelectedTab = pageDetail;
        }

        // Phương thức mới để load dữ liệu invoice
        private void LoadInvoiceData()
        {
            if (cboDepartment.SelectedValue != null)
            {
                string departmentId = cboDepartment.SelectedValue.ToString();
                _lstInvoice = _invoice.getReceiveInvoice(2, dtFrom.Value, dtTill.Value.AddDays(1), departmentId);
                _bsInvoice.DataSource = _lstInvoice;
                gvList.DataSource = _bsInvoice;
                exportInfor();
            }
        }

        // Phương thức mới để thiết lập giá trị cho các cột trong DataGridView
        private void SetupDataGridViewColumns()
        {
            // Thiết lập giá trị cho các cột trong gvList
            if (gvList.Columns.Contains("DeletedBy"))
                gvList.Columns["DeletedBy"].DataPropertyName = "DeletedBy";
                
            if (gvList.Columns.Contains("InvoiceID"))
                gvList.Columns["InvoiceID"].DataPropertyName = "InvoiceID";
                
            // Sử dụng Invoice2 thay vì Invoice cho cột InvoiceNo
            if (gvList.Columns.Contains("InvoiceNo2"))
                gvList.Columns["InvoiceNo2"].DataPropertyName = "Invoice2";
                
            // Sử dụng Day2 thay vì Day cho cột Date
            if (gvList.Columns.Contains("Date"))
                gvList.Columns["Date"].DataPropertyName = "Day2";
                
            if (gvList.Columns.Contains("InvoiceNo"))
                gvList.Columns["InvoiceNo"].DataPropertyName = "Invoice";
                
            if (gvList.Columns.Contains("Day2"))
                gvList.Columns["Day2"].DataPropertyName = "Day";
                
            if (gvList.Columns.Contains("Quantity"))
                gvList.Columns["Quantity"].DataPropertyName = "Quantity";
                
            if (gvList.Columns.Contains("TotalPrice"))
                gvList.Columns["TotalPrice"].DataPropertyName = "TotalPrice";
                
            if (gvList.Columns.Contains("Description"))
                gvList.Columns["Description"].DataPropertyName = "Description";
                
            if (gvList.Columns.Contains("Status"))
                gvList.Columns["Status"].DataPropertyName = "Status";
        }
    }
}
