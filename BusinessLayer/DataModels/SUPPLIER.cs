using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class SUPPLIER
    {
        Entities db;

        public SUPPLIER()
        {
            db = Entities.CreateEntities();
        }

        public tb_Supplier getItem(int supplierid)
        {
            return db.tb_Supplier.FirstOrDefault(x => x.SupplierID == supplierid);
        }

        public List<tb_Supplier> getList()
        {
            return db.tb_Supplier.ToList();
        }
        public void add(tb_Supplier cp)
        {
            try
            {
                db.tb_Supplier.Add(cp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Supplier cp)
        {
            tb_Supplier _cp = db.tb_Supplier.FirstOrDefault(x => x.SupplierID == cp.SupplierID);
            _cp.SupplierName = cp.SupplierName;
            _cp.SupplierPhone = cp.SupplierPhone;
            _cp.SupplierEmail = cp.SupplierEmail;
            _cp.SupplierFax = cp.SupplierFax;
            _cp.SupplierAddress = cp.SupplierAddress;
            _cp.IsDisabled = cp.IsDisabled;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(int supplierid)
        {
            tb_Supplier cp = db.tb_Supplier.FirstOrDefault(x => x.SupplierID == supplierid);
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
