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
        public tb_Product getItemBarcode(string barcode)
        {
            return db.tb_Product.FirstOrDefault(x => x.BARCODE == barcode);
        }

        public bool checkExist(string barcode)
        {
            var h = db.tb_Product.FirstOrDefault(x => x.BARCODE == barcode);
            if (h == null)
                return true;
            else
                return false;
        }

        public List<tb_Product> getListByKeyword(string keyword)
        {
            return db.tb_Product.Where(ts => ts.ProductName.Contains(keyword)).ToList();
        }

        // Lấy danh sách sản phẩm theo Category
        public List<tb_Product> getListByCategory(int categoryid)
        {
            return db.tb_Product.Where(x => x.CategoryID == categoryid).OrderBy(o => o.CreatedDate).ToList();
        }
        public List<obj_PRODUCT> getListByCategoryFull(int categoryid)
        {
            var lst = db.tb_Product.Where(x => x.CategoryID == categoryid).OrderBy(o => o.CreatedDate).ToList();
            List<obj_PRODUCT> lstObj = new List<obj_PRODUCT>();
            obj_PRODUCT pd;
            foreach (var item in lst)
            {
                pd = new obj_PRODUCT();
                pd.BARCODE = item.BARCODE;
                pd.QRCODE = item.QRCODE;
                pd.ProductID = item.ProductID;
                pd.ProductName = item.ProductName;
                pd.ShortName = item.ShortName;
                pd.CategoryID = item.CategoryID;
                var c = db.tb_ProductCategory.FirstOrDefault(x=>x.CategoryID==item.CategoryID);
                pd.Category = c.Category;
                pd.SupplierID = item.SupplierID;
                var s = db.tb_Supplier.FirstOrDefault(x => x.SupplierID == item.SupplierID);
                pd.SupplierName = s.SupplierName;
                pd.OriginID = item.OriginID;
                var o = db.tb_Origin.FirstOrDefault(x => x.OriginID == item.OriginID);
                pd.OriginName = o.OriginName;
                pd.Unit = item.Unit;
                pd.Price = item.Price;
                pd.Description = item.Description;
                lstObj.Add(pd);
            }
            return lstObj;
        }

        public obj_PRODUCT getItemFull(string barcode)
        {
            var _h = db.tb_Product.FirstOrDefault(x => x.BARCODE == barcode);
            obj_PRODUCT hh = new obj_PRODUCT();
            hh.BARCODE = _h.BARCODE;
            hh.QRCODE = _h.QRCODE;
            hh.ProductID = _h.ProductID;
            hh.ProductName = _h.ProductName;
            hh.ShortName = _h.ShortName;
            hh.CategoryID = _h.CategoryID;
            hh.OriginID = _h.OriginID;
            hh.Price = _h.Price;
            hh.Description = _h.Description;
            hh.Unit = _h.Unit;
            hh.IsDisabled = _h.IsDisabled;
            hh.SupplierID = _h.SupplierID;
            var cc = db.tb_Supplier.FirstOrDefault(x => x.SupplierID == _h.SupplierID);
            hh.SupplierName = cc.SupplierName;

            return hh;
        }

        public List<obj_PRODUCT> getList()
        {
            List<obj_PRODUCT> _lstHH = new List<obj_PRODUCT>();
            var lstHH = db.tb_Product.ToList();
            obj_PRODUCT hh;
            foreach (var item in lstHH)
            {
                hh = new obj_PRODUCT();
                hh.ProductID = item.ProductID;
                hh.QRCODE = item.QRCODE;
                hh.BARCODE = item.BARCODE;
                hh.ProductName = item.ProductName;
                hh.ShortName = item.ShortName;
                hh.CategoryID = item.CategoryID;
                hh.Price = item.Price;
                var n = db.tb_ProductCategory.FirstOrDefault(x => x.CategoryID == item.CategoryID);
                hh.Category = n.Category;
                hh.OriginID = item.OriginID;
                var xx = db.tb_Origin.FirstOrDefault(x => x.OriginID == item.OriginID);
                hh.OriginName = xx.OriginName;
                hh.Description = item.Description;
                hh.Unit = item.Unit;
                hh.IsDisabled = item.IsDisabled;
                hh.SupplierID = item.SupplierID;
                var cc = db.tb_Supplier.FirstOrDefault(x => x.SupplierID == item.SupplierID);
                hh.SupplierName = cc.SupplierName;
                _lstHH.Add(hh);
            }
            return _lstHH;
        }

        // Thêm sản phẩm mới
        public void add(tb_Product pd)
        {
            try
            {
                db.tb_Product.Add(pd);
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                throw new Exception("Entity validation error occurred. Check console output for details.");
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
