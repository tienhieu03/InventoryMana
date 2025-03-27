using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.DataModels
{
    public class CUSTOMER
    {
        Entities db;
        public CUSTOMER()
        {
            db = new Entities();
        }
        public List<tb_Customer> getList()
        {
            return db.tb_Customer.ToList();
        }
    }
}
