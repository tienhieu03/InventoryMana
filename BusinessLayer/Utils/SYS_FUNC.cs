using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class SYS_FUNC
    {
        Entities db;
        public SYS_FUNC()
        {
            db = Entities.CreateEntities();
        }
        public List<tb_SYS_FUNC> getParent()
        {
            return db.tb_SYS_FUNC.Where(x => x.IsGroup == true && x.Menu == true).OrderBy(s => s.Sort).ToList();
        }
        public List<tb_SYS_FUNC> getChild(string parent)
        {
            return db.tb_SYS_FUNC.Where(x => x.IsGroup == false && x.Menu == true && x.Parent == parent).OrderBy(s => s.Sort).ToList();
        }
        public List<V_FUNC_SYS_RIGHT> getFuncByUser(int idUser)
        {
            try
            {
                // Đảm bảo đồng bộ dữ liệu trong bảng SYS_RIGHT
                SyncFunctionRights(idUser);
                
                // Sau đó lấy dữ liệu đã được đồng bộ
                return db.V_FUNC_SYS_RIGHT.Where(x => x.UserID == idUser).OrderBy(x => x.Sort).ToList();
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu cần
                Console.WriteLine("Error in getFuncByUser: " + ex.Message);
                return new List<V_FUNC_SYS_RIGHT>();
            }
        }
        // Phương thức mới để đồng bộ dữ liệu
        private void SyncFunctionRights(int userId)
        {
            // Lấy tất cả các chức năng từ bảng tb_SYS_FUNC
            var allFunctions = db.tb_SYS_FUNC.ToList();
            
            // Lấy tất cả quyền hiện có của người dùng
            var existingRights = db.tb_SYS_RIGHT.Where(x => x.UserID == userId).ToList();
            
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var func in allFunctions)
                    {
                        // Tìm quyền tương ứng của người dùng
                        var existingRight = existingRights.FirstOrDefault(r => r.FUNC_CODE == func.FUNC_CODE);
                        
                        if (existingRight == null)
                        {
                            // Nếu chưa có quyền, tạo mới với quyền mặc định là "View Only" (1)
                            var newRight = new tb_SYS_RIGHT
                            {
                                FUNC_CODE = func.FUNC_CODE,
                                UserID = userId,
                                UserRight = 1 // View Only
                            };
                            db.tb_SYS_RIGHT.Add(newRight);
                        }
                    }
                    
                    // Lưu thay đổi
                    db.SaveChanges();
                    
                    // Cập nhật view thông qua stored procedure
                    db.Database.ExecuteSqlCommand("EXEC sp_UpdateFuncRightView @UserID", 
                        new System.Data.SqlClient.SqlParameter("@UserID", userId));
                        
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void updateFunction(tb_SYS_FUNC func)
        {
            try
            {
                // Tìm chức năng cần cập nhật
                var existingFunc = db.tb_SYS_FUNC.FirstOrDefault(x => x.FUNC_CODE == func.FUNC_CODE);
                
                if (existingFunc != null)
                {
                    // Cập nhật thông tin
                    existingFunc.Description = func.Description;
                    existingFunc.IsGroup = func.IsGroup;
                    existingFunc.Parent = func.Parent;
                    existingFunc.Sort = func.Sort;
                    existingFunc.Menu = func.Menu;
                    
                    db.SaveChanges();
                    
                    // Cập nhật view cho tất cả người dùng có quyền với chức năng này
                    db.Database.ExecuteSqlCommand("UPDATE V_FUNC_SYS_RIGHT SET Description = @Description, IsGroup = @IsGroup, Parent = @Parent, Sort = @Sort WHERE FUNC_CODE = @FuncCode",
                        new System.Data.SqlClient.SqlParameter("@Description", func.Description),
                        new System.Data.SqlClient.SqlParameter("@IsGroup", func.IsGroup),
                        new System.Data.SqlClient.SqlParameter("@Parent", func.Parent),
                        new System.Data.SqlClient.SqlParameter("@Sort", func.Sort),
                        new System.Data.SqlClient.SqlParameter("@FuncCode", func.FUNC_CODE));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating function: " + ex.Message);
            }
        }
    }
}
