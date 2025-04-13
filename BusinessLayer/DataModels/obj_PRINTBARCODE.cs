using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataModels
{
    public class obj_PRINTBARCODE
    {
        public string BARCODE { get; set; }
        public string ProductName { get; set; }
        public string ShortName { get; set; }
        public Double? Price { get; set; }
        public int? StampNumber { get; set; }
    }
}
