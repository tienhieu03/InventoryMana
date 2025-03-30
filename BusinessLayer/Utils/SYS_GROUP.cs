using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
namespace BusinessLayer.Utils
{
    public class SYS_GROUP
    {
        Entities db;
        public SYS_GROUP()
        {
            db = Entities.CreateEntities();
        }
        public tb_SYS_GROUP getGroupByMemBer(int idUser)
        {
            return db.tb_SYS_GROUP.FirstOrDefault(x => x.Member == idUser);
        }
        public void add(tb_SYS_GROUP gr)
        {
            try
            {
                db.tb_SYS_GROUP.Add(gr);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu" + ex.Message);
            }
        }

        public void delGroup(int idUser, int idGroup)
        {
            var gr = db.tb_SYS_GROUP.FirstOrDefault(x => x.Member == idUser && x.Groups == idGroup);
            if (gr != null)
            {
                try
                {
                    db.tb_SYS_GROUP.Remove(gr);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw new Exception("Có lỗi xảy ra trong quá trình xử lý dữ liệu" + ex.Message);

                }
            }
        }
    }
}
