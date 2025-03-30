using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class VIEW_USER_NOTIN_GROUP
    {
        Entities db;

        public VIEW_USER_NOTIN_GROUP()
        {
            db = Entities.CreateEntities();
        }

        public List<V_USER_NOTIN_GROUP> getUserNotInGroup(string dpid, string cpid)
        {
            return db.V_USER_NOTIN_GROUP.Where(x => x.DepartmentID == dpid && x.CompanyID == cpid && x.IsGroup == false && x.IsDisable == false).ToList();
        }

    }
}
