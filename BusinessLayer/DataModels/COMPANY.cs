using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class COMPANY
    {
        Entities db;

        public COMPANY()
        {
            db = Entities.CreateEntities();
        }

        public tb_Company getItem(string companyid)
        {
            return db.tb_Company.FirstOrDefault(x => x.CompanyID == companyid);
        }

        public List<tb_Company> getAll()
        {
            return db.tb_Company.ToList();
        }

        public void add(tb_Company cp)
        {
            try
            {
                db.tb_Company.Add(cp);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Company cp)
        {
            tb_Company _cp = db.tb_Company.FirstOrDefault(x => x.CompanyID == cp.CompanyID);
            _cp.CompanyName = cp.CompanyName;
            _cp.CompanyPhone = cp.CompanyPhone;
            _cp.CompanyEmail = cp.CompanyEmail;
            _cp.CompanyFax = cp.CompanyFax;
            _cp.CompanyAddress = cp.CompanyAddress;
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
        public void delete(string companyid)
        {
            tb_Company cp = db.tb_Company.FirstOrDefault(x => x.CompanyID == companyid);
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
