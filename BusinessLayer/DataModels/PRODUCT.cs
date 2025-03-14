using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.DataModels
{
    public class PRODUCT
    {
        Entities db;
        public PRODUCT()
        {
            db = Entities.CreateEntities();
        }

        public tb_Product getItem(int productid)
        {
            return db.tb_Product.FirstOrDefault(x => x.ProductID == productid);
        }

        public List<tb_Product> getListByCategory(int categoryid)
        {
            return db.tb_Product.Where(x => x.CategoryID == categoryid).OrderBy(o => o.CreatedDate).ToList();
        }

        public void add(tb_Product pd)
        {
            try
            {
                db.tb_Product.Add(pd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Product pd)
        {
            tb_Product _pd = db.tb_Product.FirstOrDefault(x => x.ProductID == pd.ProductID);
            _pd.BARCODE = pd.BARCODE;
            _pd.QRCODE = pd.QRCODE;
            _pd.ProductName = pd.ProductName;
            _pd.ShortName = pd.ShortName;
            _pd.Unit = pd.Unit;
            _pd.Price = pd.Price;
            _pd.SupplierID = pd.SupplierID;
            _pd.Orgin = pd.Orgin;
            _pd.Description = pd.Description;
            _pd.IsDisabled = pd.IsDisabled;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(int productid)
        {
            tb_Product pd = db.tb_Product.FirstOrDefault(x => x.ProductID == productid);
            pd.IsDisabled = true;
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
