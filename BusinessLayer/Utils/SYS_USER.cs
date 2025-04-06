using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.Utils
{
    public class SYS_USER
    {
        Entities db;
        public SYS_USER()
        {
            db = Entities.CreateEntities();
        }
        public tb_SYS_USER getItem(int userid)
        {
            return db.tb_SYS_USER.FirstOrDefault(x => x.UserID == userid);
        }
        public tb_SYS_USER getItem(string username, string macty, string madvi)
        {
            return db.tb_SYS_USER.FirstOrDefault(x => x.UserName == username && x.CompanyID == macty && x.DepartmentID == madvi);
        }
        public List<tb_SYS_USER>getAll()
        {
            return db.tb_SYS_USER.ToList();
        }
        public List<tb_SYS_USER>getUserByDp(string cpid, string dpid)
        {
            return db.tb_SYS_USER.Where(x=>x.CompanyID == cpid && x.DepartmentID == dpid).ToList();
        }
        public List<tb_SYS_USER> getUserByDpFunc(string macty, string madvi)
        {
            return db.tb_SYS_USER.Where(x => x.CompanyID == macty && x.DepartmentID == madvi && x.IsDisable == false).OrderByDescending(x => x.IsGroup).ToList();
        }
        public bool checkUserExist(string cpid, string dpid, string username)
        {
            var us = db.tb_SYS_USER.FirstOrDefault(x => x.CompanyID == cpid && x.DepartmentID == dpid && x.UserName == username);
            if (us != null)
            {
                return true;
            }
            else
                return false;
        }
        public tb_SYS_USER add(tb_SYS_USER us)
        {
            try
            {
                db.tb_SYS_USER.Add(us);
                db.SaveChanges();
                return us;
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi: " + ex.Message);
            }
        }
        public tb_SYS_USER update(tb_SYS_USER us)
        {
            var _us = db.tb_SYS_USER.FirstOrDefault(x => x.UserID == us.UserID);
            _us.UserName = us.UserName;
            _us.FullName = us.FullName;
            _us.IsGroup = us.IsGroup;
            _us.IsDisable = us.IsDisable;
            _us.CompanyID = us.CompanyID;
            _us.DepartmentID = us.DepartmentID;
            _us.Password = us.Password;
            _us.LastPasswordChange = DateTime.Now;
            try
            {
                db.SaveChanges();
                return us;
            }
            catch (Exception ex)
            {

                throw new Exception("Lỗi: " + ex.Message);
            }
        }
    }
}
