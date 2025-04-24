using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS;
using BusinessLayer;// Add this import for the correct namespace

namespace SharedControls
{
    public partial class ucCompany : UserControl
    {
        public ucCompany()
        {
            InitializeComponent();
        }

        private void ucCompany_Load(object sender, EventArgs e)
        {
            COMPANY _company = new COMPANY();
            cboCompany.DataSource = _company.getAll();
            cboCompany.DisplayMember = "CompanyName";
            cboCompany.ValueMember = "CompanyID";
            
            // Check for null before assigning to prevent ArgumentNullException
            if (myFunctions._compid != null)
            {
                cboCompany.SelectedValue = myFunctions._compid;
            }
        }
    }
}
