using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer
{
    public class INVENTORY
    {
        Entities db;
        public INVENTORY()
        {
            db = Entities.CreateEntities();
        }
        public bool CountStock(string dpid, DateTime day)
        {
            try
            {
                db.Inventory_Balance_by_Branch(day, dpid);
                // Tạo các ObjectParameter tương ứng
                var datecParam = new System.Data.Entity.Core.Objects.ObjectParameter("DATEC", typeof(DateTime));
                var datedParam = new System.Data.Entity.Core.Objects.ObjectParameter("DATED", day);
                var yearParam = new System.Data.Entity.Core.Objects.ObjectParameter("Year", typeof(int));
                var periodParam = new System.Data.Entity.Core.Objects.ObjectParameter("Period", typeof(int));

                // Gọi procedure
                db.Stock_Evaluation_Date(datecParam, datedParam, yearParam, periodParam);

                // (Optional) Trích xuất kết quả
                DateTime? datec = datecParam.Value as DateTime?;
                int? year = yearParam.Value as int?;
                int? period = periodParam.Value as int?;

                // Có thể log hoặc xử lý tiếp giá trị lấy được
                Console.WriteLine($"Stock Count for {dpid}: DATEC = {datec}, Year = {year}, Period = {period}");

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi, không nên throw sau return
                Console.WriteLine("Error in CountStock: " + ex.Message);
                return false;
                throw new Exception("Error in CountStock" + ex.Message);
            }
        }

    }
}