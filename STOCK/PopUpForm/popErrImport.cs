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
using MaterialSkin.Controls;

namespace STOCK.PopUpForm
{
    public partial class popErrImport : MaterialForm
    {
        private List<errExport> _err;
        public popErrImport(List<errExport> errors)
        {
            InitializeComponent();
            _err = errors;
        }

        private void popErrImport_Load(object sender, EventArgs e)
        {
            dgvErrors.DataSource = _err;
            dgvErrors.ReadOnly = true;
        }
    }
}
