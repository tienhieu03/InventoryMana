using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class ORIGIN
    {
        Entities db;
        public ORIGIN()
        {
            db = Entities.CreateEntities();
        }

        public tb_Origin getItem(int originid)
        {
            return db.tb_Origin.FirstOrDefault(x => x.OriginID == originid);
        }
        public List<tb_Origin> getAll()
        {
            return db.tb_Origin.ToList();
        }
        public void add(tb_Origin cp)
        {
            try
            {
                db.tb_Origin.Add(cp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Origin cp)
        {
            tb_Origin _cp = db.tb_Origin.FirstOrDefault(x => x.OriginID == cp.OriginID);
            _cp.OriginName = cp.OriginName;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(int originid)
        {
            tb_Origin cp = db.tb_Origin.FirstOrDefault(x => x.OriginID == originid);
            cp.IsDisabled = true;
            cp.DeletedDate = DateTime.Now;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }

    }
}
