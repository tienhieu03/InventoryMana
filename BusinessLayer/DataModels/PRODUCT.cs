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

        // Lấy sản phẩm theo ProductID
        public tb_Product getItem(int productID)
        {
            return db.tb_Product.FirstOrDefault(x => x.ProductID == productID);
        }

        // Lấy danh sách sản phẩm theo Category
        public List<tb_Product> getListByCategory(int categoryid)
        {
            return db.tb_Product.Where(x => x.CategoryID == categoryid).OrderBy(o => o.CreatedDate).ToList();
        }

        // Thêm sản phẩm mới
        public void add(tb_Product pd)
        {
            try
            {
                db.tb_Product.Add(pd);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception("An error occurred: " + ex.InnerException.Message);
                }
                else
                {
                    throw new Exception("An error occurred: " + ex.Message);
                }
            }

        }

        // Cập nhật sản phẩm dựa trên ProductID
        public void update(tb_Product pd)
        {
            tb_Product _pd = db.tb_Product.FirstOrDefault(x => x.ProductID == pd.ProductID);
            if (_pd != null)
            {
                _pd.BARCODE = pd.BARCODE;
                //_pd.QRCODE = pd.QRCODE;
                _pd.ProductName = pd.ProductName;
                _pd.ShortName = pd.ShortName;
                _pd.Unit = pd.Unit;
                _pd.Price = pd.Price;
                _pd.SupplierID = pd.SupplierID;
                _pd.OriginID = pd.OriginID;
                _pd.Description = pd.Description;
                _pd.IsDisabled = pd.IsDisabled;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
        }

        // Xóa sản phẩm bằng cách disable (không xóa vật lý)
        public void delete(int productID)
        {
            tb_Product pd = db.tb_Product.FirstOrDefault(x => x.ProductID == productID);
            if (pd != null)
            {
                pd.IsDisabled = true;
                pd.DeletedDate = DateTime.Now;

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
        }
    }
}
