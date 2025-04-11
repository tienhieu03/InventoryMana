using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class INVENTORY
    {
        Entities db;
        public INVENTORY()
        {
            db = Entities.CreateEntities();
        }
        public bool CountStock(string dpid, DateTime day)
        {
            try
            {
                db.Inventory_Balance_by_Branch(day, dpid);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error in CountStock" + ex.Message);
            }
        }
    }
}