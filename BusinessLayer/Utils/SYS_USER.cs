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
            // Get all non-disabled users from the specified company and department
            var users = db.tb_SYS_USER.Where(x => x.CompanyID == macty && x.DepartmentID == madvi && x.IsDisable == false).ToList();
            
            // First get all the group accounts
            var groupAccounts = users.Where(u => u.IsGroup == true).ToList();
            
            // Create a dictionary to store users by their group
            var usersByGroup = new Dictionary<string, List<tb_SYS_USER>>();
            
            // Initialize SYS_GROUP to find group associations
            var sysGroup = new SYS_GROUP();
            
            // For each regular user, determine their group and add them to the appropriate list
            foreach (var user in users.Where(u => u.IsGroup != true))
            {
                // Get the group this user belongs to
                var group = sysGroup.getGroupByMemBer(user.UserID);
                
                if (group != null)
                {
                    // Get the group account
                    var groupAccount = users.FirstOrDefault(g => g.UserID == group.Groups);
                    
                    if (groupAccount != null)
                    {
                        // Use the group's name as key
                        string groupName = groupAccount.UserName;
                        
                        // Create entry in dictionary if it doesn't exist
                        if (!usersByGroup.ContainsKey(groupName))
                        {
                            usersByGroup[groupName] = new List<tb_SYS_USER>();
                        }
                        
                        // Add this user to their group's list
                        usersByGroup[groupName].Add(user);
                    }
                }
            }
            
            // Now create the ordered list starting with each group followed by its members
            var result = new List<tb_SYS_USER>();
            
            // Add groups and their members in order
            foreach (var groupAccount in groupAccounts)
            {
                // First add the group account
                result.Add(groupAccount);
                
                // Then add all users belonging to this group (if any)
                if (usersByGroup.ContainsKey(groupAccount.UserName))
                {
                    result.AddRange(usersByGroup[groupAccount.UserName]);
                }
            }
            
            // Finally add any users who aren't associated with any group
            foreach (var user in users.Where(u => u.IsGroup != true))
            {
                var group = sysGroup.getGroupByMemBer(user.UserID);
                if (group == null)
                {
                    result.Add(user);
                }
            }
            
            return result;
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
