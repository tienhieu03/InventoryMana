using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.DataModels
{
    public class PRODUCT_CATEGORY
    {
        Entities db;
        public PRODUCT_CATEGORY()
        {
            db = Entities.CreateEntities();
        }

        public tb_ProductCategory getItem(int categoryid)
        {
            return db.tb_ProductCategory.FirstOrDefault(x => x.CategoryID == categoryid);
        }
        public List<tb_ProductCategory> getAll()
        {
            return db.tb_ProductCategory.ToList();
        }
        public void add(tb_ProductCategory cp)
        {
            try
            {
                db.tb_ProductCategory.Add(cp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_ProductCategory cp)
        {
            tb_ProductCategory _cp = db.tb_ProductCategory.FirstOrDefault(x => x.CategoryID == cp.CategoryID);
            _cp.Category = cp.Category;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(int categoryid)
        {
            tb_ProductCategory cp = db.tb_ProductCategory.FirstOrDefault(x => x.CategoryID == categoryid);
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
