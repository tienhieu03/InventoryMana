using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.Utils
{
    public class VIEW_FUNC_SYS_RIGHT
    {
        Entities db;
        public VIEW_FUNC_SYS_RIGHT()
        {
            db = Entities.CreateEntities();
        }
        public List<V_FUNC_SYS_RIGHT> getFuncByUser(int idUser)
        {
            return db.V_FUNC_SYS_RIGHT.Where(x => x.UserID == idUser).OrderBy(x => x.Sort).ToList();
        }
    }
}
