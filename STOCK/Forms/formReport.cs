using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer.Utils;
using DataLayer;
using MaterialSkin.Controls;
using SharedControls;

namespace STOCK.Forms
{
    public partial class formReport : MaterialForm
    {
        public formReport()
        {
            InitializeComponent();
        }

        public formReport(tb_SYS_USER user)
        {
            InitializeComponent();
            _user = user;
        }

        tb_SYS_USER _user;
        SYS_USER _sysUser;
        SYS_REPORT _sysReport;
        SYS_RIGHT_REP _sysRightRep;
        Panel _panel;
        ucFrom _uFrom;
        ucCompany _uCompany;
        ucDepartment _uDepartment;

        private void formReport_Load(object sender, EventArgs e)
        {
            _sysReport = new SYS_REPORT();
            _sysUser = new SYS_USER();
            _sysRightRep = new SYS_RIGHT_REP();
            var right = _sysRightRep.getListByUser(_user.UserID);
            if (right.Count == 0)
            {
                MessageBox.Show("Không có quyền thao tác");
                this.Close();
            }
            else
                lstList.DataSource = _sysReport.getlistByRight(right);
            lstList.DisplayMember = "Description";
            lstList.ValueMember = "REP_CODE";
            lstList.SelectedIndexChanged += LstList_SelectedIndexChanged; ;
            loadUserControl();
        }

        private void LstList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadUserControl();
        }
        void loadUserControl()
        {
            int rep_code = 0;
            if (lstList.SelectedValue != null)
            {
                rep_code = int.Parse(lstList.SelectedValue.ToString());
            }
            else
            {
                rep_code = 0;
            }
            tb_SYS_REPORT rep = _sysReport.getItem(rep_code);
            if (_panel != null)
                _panel.Dispose();
            _panel = new Panel();
            _panel.Dock = DockStyle.Top;
            _panel.MinimumSize = new Size(_panel.Width, 500);
            List<Control> _ctrl = new List<Control>();
            if (rep != null)
            {
                if (rep.FromDate == true)
                {
                    _uFrom = new ucFrom();
                    _uFrom.Dock = DockStyle.Top;
                    _ctrl.Add(_uFrom);
                }
                if (rep.CompanyID == true)
                {
                    _uCompany = new ucCompany();
                    _uCompany.Dock = DockStyle.Top;
                    _ctrl.Add(_uCompany);
                }
                // Check if the current report is a department report
                // For department reports, don't show the department selection control
                bool isDepartmentReport = rep.Description != null && rep.Description.ToLower().Contains("department report");
                if (rep.DepartmentID == true && !isDepartmentReport)
                {
                    _uDepartment = new ucDepartment();
                    _uDepartment.Dock = DockStyle.Top;
                    _ctrl.Add(_uDepartment);
                }
                _ctrl.Reverse();
                _panel.Controls.AddRange(_ctrl.ToArray());
                this.splReport.Panel2.Controls.Add(_panel);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {

        }
    }
}
