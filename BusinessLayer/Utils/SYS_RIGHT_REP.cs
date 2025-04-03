using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.Utils
{
    public class SYS_RIGHT_REP
    {
        Entities db;
        public SYS_RIGHT_REP()
        {
            db = Entities.CreateEntities();
        }
        public List<tb_SYS_RIGHT_REP> getListByUser(int UserID)
        {
            SYS_GROUP sGroup = new SYS_GROUP();
            var group = sGroup.getGroupByMemBer(UserID);
            if (group == null)
            {
                return db.tb_SYS_RIGHT_REP.Where(x => x.UserID == UserID && x.UserRight == true).ToList();
            }
            else
            {
                List<tb_SYS_RIGHT_REP> lstByGroup = db.tb_SYS_RIGHT_REP.Where(x => x.UserID == group.Groups && x.UserRight == true).ToList();
                List<tb_SYS_RIGHT_REP> lstByUser = db.tb_SYS_RIGHT_REP.Where(x => x.UserID == UserID && x.UserRight == true).ToList();
                List<tb_SYS_RIGHT_REP> lstAll = lstByUser.Concat(lstByGroup).ToList();
                return lstAll;
            }
        }
        public void update(int UserID, int rep_code, bool right)
        {
            tb_SYS_RIGHT_REP sRight = db.tb_SYS_RIGHT_REP.FirstOrDefault(x => x.UserID == UserID && x.REP_CODE == rep_code);
            try
            {
                sRight.UserRight = right;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
