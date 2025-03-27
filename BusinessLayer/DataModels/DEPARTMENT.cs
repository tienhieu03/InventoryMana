using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class DEPARTMENT
    {
        Entities db;
        public DEPARTMENT()
        {
            db = Entities.CreateEntities();
        }
        public tb_Department getItem(string departmentid)
        {
            return db.tb_Department.FirstOrDefault(x => x.DepartmentID == departmentid);
        }
        public List<tb_Department> getAll()
        {
            return db.tb_Department.ToList();
        }
        public List<tb_Department> getAll(string companyid)
        {
            return db.tb_Department.Where(x => x.CompanyID == companyid).ToList();
        }
        public List<tb_Department> getWarehoseByCp(string companyid)
        {
            return db.tb_Department.Where(x => x.CompanyID == companyid && x.Warehouse == true).ToList();
        }
        public void add(tb_Department department)
        {
            try
            {
                db.tb_Department.Add(department);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void update(tb_Department department)
        {
            tb_Department _department = db.tb_Department.FirstOrDefault(x => x.DepartmentID == department.DepartmentID);
            _department.CompanyID = department.CompanyID;
            _department.DepartmentName = department.DepartmentName;
            _department.DepartmentPhone = department.DepartmentPhone;
            _department.DepartmentFax = department.DepartmentFax;
            _department.DepartmentEmail = department.DepartmentEmail;
            _department.DepartmentAddress = department.DepartmentAddress;
            _department.IsDisabled = department.IsDisabled;
            _department.Warehouse = department.Warehouse;
            _department.Symbol = department.Symbol;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing" + ex.Message);
            }
        }
        public void delete(string departmentid)
        {
            tb_Department _department = db.tb_Department.FirstOrDefault(x => x.DepartmentID == departmentid);
            _department.IsDisabled = true;
            _department.DeletedDate = DateTime.Now;
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
