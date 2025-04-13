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
// Add necessary using statements for reporting
using Microsoft.Reporting.WinForms;
using System.Data;
using STOCK.Forms; // For formReportViewer

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
            // Call the exportReport method
            exportReport("INTERNAL_RECEIVE_INVOICE", "Internal Receive Invoice"); // Pass report name and title
        }

        private void exportReport(string _reportName, string _tieude)
        {
            // Ensure an invoice is selected
            if (_bsInvoice.Current == null)
            {
                MessageBox.Show("Please select an invoice to export.", "No Invoice Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            tb_Invoice currentInvoice = (tb_Invoice)_bsInvoice.Current;
            List<obj_INVOICE_DETAIL> details = _bsInvoiceDT.DataSource as List<obj_INVOICE_DETAIL>;

            if (details == null || details.Count == 0)
            {
                // Attempt to reload details if empty
                details = _invoiceDetail.getListbyIDFull(currentInvoice.InvoiceID);
                if (details == null || details.Count == 0)
                {
                    MessageBox.Show("No details found for the selected invoice.", "No Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Fetch related data
            tb_Company company = _company.getItem(currentInvoice.CompanyID);
            // DepartmentID is the Exporting Department in this context
            tb_Department exportingDepartment = _department.getItem(currentInvoice.DepartmentID);
            // ReceivingDepartmentID is the Receiving Department in this context
            tb_Department receivingDepartment = _department.getItem(currentInvoice.ReceivingDepartmentID);


            // Create DataTable matching the RDLC DataSet structure (dsInvoiceNB.xsd -> DataTable1)
            DataTable dtReportData = new DataTable("DataTable1"); // Match table name in dsInvoiceNB.xsd

            // Add columns based on dsInvoiceNB.xsd DataTable1 (ensure names match EXACTLY)
            // Invoice Info
            dtReportData.Columns.Add("InvoiceID", typeof(Guid));
            dtReportData.Columns.Add("Invoice", typeof(string)); // Export Invoice No
            dtReportData.Columns.Add("Invoice2", typeof(string)); // Receive Invoice No
            dtReportData.Columns.Add("Day", typeof(string)); // Export Date (Formatted)
            dtReportData.Columns.Add("Day2", typeof(string)); // Receive Date (Formatted)
            dtReportData.Columns.Add("Description", typeof(string));
            dtReportData.Columns.Add("TotalPrice", typeof(double)); // Invoice Total
            dtReportData.Columns.Add("Quantity", typeof(int)); // Invoice Total Quantity

            // Company Info
            dtReportData.Columns.Add("CompanyName", typeof(string));
            dtReportData.Columns.Add("CompanyAddress", typeof(string));
            dtReportData.Columns.Add("CompanyPhone", typeof(string));
            dtReportData.Columns.Add("CompanyFax", typeof(string)); // Added based on XSD
            dtReportData.Columns.Add("CompanyEmail", typeof(string)); // Added based on XSD

            // Exporting Department Info (Maps to tb_Department in XSD)
            dtReportData.Columns.Add("DepartmentID", typeof(string)); // Exporting Dept ID
            dtReportData.Columns.Add("DepartmentName", typeof(string)); // Exporting Dept Name
            dtReportData.Columns.Add("DepartmentAddress", typeof(string)); // Exporting Dept Address
            dtReportData.Columns.Add("DepartmentPhone", typeof(string)); // Exporting Dept Phone
            dtReportData.Columns.Add("DepartmentFax", typeof(string)); // Exporting Dept Fax
            dtReportData.Columns.Add("DepartmentEmail", typeof(string)); // Exporting Dept Email
            dtReportData.Columns.Add("Symbol", typeof(string)); // Exporting Dept Symbol

            // Receiving Department Info (Maps to tb_Department_1 in XSD)
            dtReportData.Columns.Add("DepartmentID1", typeof(string)); // Receiving Dept ID
            dtReportData.Columns.Add("DepartmentName1", typeof(string)); // Receiving Dept Name
            dtReportData.Columns.Add("DepartmentAddress1", typeof(string)); // Receiving Dept Address
            dtReportData.Columns.Add("DepartmentPhone1", typeof(string)); // Receiving Dept Phone
            dtReportData.Columns.Add("DepartmentFax1", typeof(string)); // Receiving Dept Fax
            dtReportData.Columns.Add("DepartmentEmail1", typeof(string)); // Receiving Dept Email
            dtReportData.Columns.Add("Symbol1", typeof(string)); // Receiving Dept Symbol

            // Detail Info
            dtReportData.Columns.Add("STT", typeof(int));
            dtReportData.Columns.Add("BARCODE", typeof(string));
            dtReportData.Columns.Add("ProductName", typeof(string));
            dtReportData.Columns.Add("Unit", typeof(string));
            dtReportData.Columns.Add("Quantity1", typeof(int)); // Detail Quantity (Mapped to Quantity1 in XSD) - Renaming for clarity
            dtReportData.Columns.Add("Price", typeof(double)); // Detail Price
            dtReportData.Columns.Add("SubTotal", typeof(double)); // Detail SubTotal
            dtReportData.Columns.Add("ProductID", typeof(int)); // Added based on XSD

            // Populate DataTable
            foreach (var detail in details)
            {
                DataRow dr = dtReportData.NewRow();
                // Invoice Info
                dr["InvoiceID"] = currentInvoice.InvoiceID;
                dr["Invoice"] = currentInvoice.Invoice; // Export Invoice No
                dr["Invoice2"] = currentInvoice.Invoice2; // Receive Invoice No
                dr["Day"] = (currentInvoice.Day ?? DateTime.Now).ToString("dd/MM/yyyy"); // Export Date
                dr["Day2"] = (currentInvoice.Day2 ?? DateTime.MinValue) == DateTime.MinValue ? "" : (currentInvoice.Day2.Value).ToString("dd/MM/yyyy"); // Receive Date (handle null)
                dr["Description"] = currentInvoice.Description;
                dr["TotalPrice"] = currentInvoice.TotalPrice ?? 0;
                dr["Quantity"] = currentInvoice.Quantity ?? 0; // Invoice Total Quantity

                // Company Info
                dr["CompanyName"] = company?.CompanyName;
                dr["CompanyAddress"] = company?.CompanyAddress;
                dr["CompanyPhone"] = company?.CompanyPhone;
                dr["CompanyFax"] = company?.CompanyFax;
                dr["CompanyEmail"] = company?.CompanyEmail;

                // Exporting Department Info
                dr["DepartmentID"] = exportingDepartment?.DepartmentID;
                dr["DepartmentName"] = exportingDepartment?.DepartmentName;
                dr["DepartmentAddress"] = exportingDepartment?.DepartmentAddress;
                dr["DepartmentPhone"] = exportingDepartment?.DepartmentPhone;
                dr["DepartmentFax"] = exportingDepartment?.DepartmentFax;
                dr["DepartmentEmail"] = exportingDepartment?.DepartmentEmail;
                dr["Symbol"] = exportingDepartment?.Symbol;

                // Receiving Department Info
                dr["DepartmentID1"] = receivingDepartment?.DepartmentID; // Map to DepartmentID1
                dr["DepartmentName1"] = receivingDepartment?.DepartmentName; // Map to DepartmentName1
                dr["DepartmentAddress1"] = receivingDepartment?.DepartmentAddress; // Map to DepartmentAddress1
                dr["DepartmentPhone1"] = receivingDepartment?.DepartmentPhone; // Map to DepartmentPhone1
                dr["DepartmentFax1"] = receivingDepartment?.DepartmentFax; // Map to DepartmentFax1
                dr["DepartmentEmail1"] = receivingDepartment?.DepartmentEmail; // Map to DepartmentEmail1
                dr["Symbol1"] = receivingDepartment?.Symbol; // Map to Symbol1

                // Detail Info
                dr["STT"] = detail.STT ?? 0;
                dr["BARCODE"] = detail.BARCODE;
                dr["ProductName"] = detail.ProductName;
                dr["Unit"] = detail.Unit;
                dr["Quantity1"] = detail.Quantity ?? 0; // Map to Quantity1
                dr["Price"] = detail.Price ?? 0;
                dr["SubTotal"] = detail.SubTotal ?? 0;
                // Directly assign the non-nullable int
                dr["ProductID"] = detail.ProductID; 

                dtReportData.Rows.Add(dr);
            }

            // Configure ReportViewer
            LocalReport report = new LocalReport();
            // Make sure the path matches the embedded resource name.
            // Check project properties -> Build Action for the rdlc file should be "Embedded Resource"
            // The name is typically Namespace.FolderName.FileName.rdlc
            report.ReportEmbeddedResource = "STOCK.RDLCReport.RecInnerInvoice.rdlc"; // Use the correct report file

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "dsInvoiceNB"; // This MUST match the DataSet Name in the RDLC file
            rds.Value = dtReportData;
            report.DataSources.Clear();
            report.DataSources.Add(rds);

            report.Refresh(); // Refresh the report definition and data

            // Create and show the report viewer form
            try
            {
                formReportViewer frmViewer = new formReportViewer(report);
                frmViewer.Text = string.IsNullOrEmpty(_tieude) ? "Internal Receive Invoice Report" : _tieude; // Set form title
                frmViewer.ShowDialog(); // Show the form modally
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening report preview: {ex.Message}\n{ex.StackTrace}", "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
