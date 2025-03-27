using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataModels
{
    public class obj_PRODUCT
    {
        public int ProductID { get; set; }
        public string BARCODE { get; set; }
        public string QRCODE { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public string Unit { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public string SupplierName { get; set; }
        public Nullable<int> OriginID { get; set; }
        public string OriginName { get; set; }
        public string Description { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Category { get; set; }
        public Nullable<bool> IsDisabled { get; set; }
    }
}
