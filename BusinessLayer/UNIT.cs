using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class UNIT
    {
        Entities db;
        public UNIT()
        {
            db = Entities.CreateEntities();
        }
        public List<tb_Unit> getList()
        {
            return db.Set<tb_Unit>().ToList();
        }
    }
}
