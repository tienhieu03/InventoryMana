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
        public tb_Unit getItem(int unitid)
        {
            return db.tb_Unit.FirstOrDefault(x => x.ID == unitid);
        }
        public List<tb_Unit> getAll()
        {
            return db.Set<tb_Unit>().ToList();
        }
        public void add(tb_Unit cp)
        {
            try
            {
                db.tb_Unit.Add(cp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Unit cp)
        {
            tb_Unit _cp = db.tb_Unit.FirstOrDefault(x => x.ID == cp.ID);
            _cp.Name = cp.Name;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(int unitid)
        {
            tb_Unit cp = db.tb_Unit.FirstOrDefault(x => x.ID == unitid);
            cp.IsDisabled = true;
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
