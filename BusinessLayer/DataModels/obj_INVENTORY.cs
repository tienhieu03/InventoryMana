using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataModels
{
    public class obj_INVENTORY
    {
        public System.Guid InventoryID { get; set; }
        public Nullable<int> PeriodYear { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Period { get; set; }
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string BARCODE { get; set; }
        public string QRCODE { get; set; }
        public Nullable<int> BeginningQuantity { get; set; }
        public Nullable<int> ReceivedQuantity { get; set; }
        public Nullable<int> RecInnerQuantity { get; set; }
        public Nullable<int> OutInnerQuantity { get; set; }
        public Nullable<int> BulkQuantity { get; set; }
        public Nullable<int> IssuedQuantity { get; set; }
        public Nullable<int> FinalQuantity { get; set; }
        public Nullable<double> Value { get; set; }
        public Nullable<double> TotalValue { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }

        public string ProductName { get; set; }
        public string Unit { get; set; }
    }
}
