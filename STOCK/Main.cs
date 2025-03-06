using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer;
using MaterialSkin.Controls;

namespace STOCK
{
    public partial class Main: MaterialForm
    {
        public Main()
        {
            InitializeComponent();
        }
        UNIT _unit;

        private void Main_Load(object sender, EventArgs e)
        {
            _unit = new UNIT();
            kryptonDataGridView1.DataSource = _unit.getList();
        }
    }
}
