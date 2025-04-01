using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class VIEW_USER_IN_GROUPS
    {
        Entities db;
        public VIEW_USER_IN_GROUPS()
        {
            db = Entities.CreateEntities();
        }
        public List<tb_SYS_USER> getGroupsByUser(string CompanyID, string DepartmentID, int UserID)
        {
            List<tb_SYS_USER> lstGroups = new List<tb_SYS_USER>();
            List<V_USER_IN_GROUP> lst = db.V_USER_IN_GROUP.Where(x => x.CompanyID == CompanyID && x.DepartmentID == DepartmentID && x.Member == UserID).ToList();
            tb_SYS_USER u;
            foreach (var item in lst)
            {
                u = new tb_SYS_USER();
                u = db.tb_SYS_USER.FirstOrDefault(x => x.UserID == item.Groups);
                lstGroups.Add(u);
            }
            return lstGroups;
        }
        public List<tb_SYS_USER> getGroupsByDonVi(string CompanyID, string DepartmentID)
        {
            return db.tb_SYS_USER.Where(x => x.CompanyID == CompanyID && x.DepartmentID == DepartmentID && x.IsGroup == true).ToList();
        }
        public bool checkGroupsByUser(int UserID, int idGroups)
        {
            var gr = db.tb_SYS_GROUP.FirstOrDefault(x => x.Member == UserID && x.Groups == idGroups);
            if (gr == null)
            {
                return false;
            }
            else
                return true;
        }
        public List<V_USER_IN_GROUP> getUserInGroup(string DepartmentID, string CompanyID, int idGroups)
        {
            return db.V_USER_IN_GROUP.Where(x => x.DepartmentID == DepartmentID && x.CompanyID == CompanyID && x.Groups == idGroups && x.IsGroup == false && x.IsDisable == false).ToList();
        }
    }
}
