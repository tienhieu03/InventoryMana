using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataModels;
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
        public List<obj_INVENTORY> getCompanyStock(string cpid, int? year, int? period)
        {
            var st = db.tb_Inventory.Where(x => x.CompanyID == cpid && x.Year == year && x.Period == period).ToList();
            List<obj_INVENTORY> lstST = new List<obj_INVENTORY>();
            obj_INVENTORY jst;
            foreach (var item in st)
            {
                jst = new obj_INVENTORY();
                jst.BARCODE = item.BARCODE;
                var hh = db.tb_Product.FirstOrDefault(h => h.BARCODE == item.BARCODE);
                // Add null check for hh
                if (hh != null)
                {
                    jst.ProductName = hh.ProductName;
                    jst.Unit = hh.Unit;
                }
                else
                {
                    jst.ProductName = "N/A"; // Or string.Empty or "Unknown Product"
                    jst.Unit = "N/A";        // Or string.Empty
                }
                jst.BeginningQuantity = item.BeginningQuantity;
                jst.ReceivedQuantity = item.ReceivedQuantity;
                jst.RecInnerQuantity = item.RecInnerQuantity;
                jst.OutInnerQuantity = item.OutInnerQuantity;
                jst.BulkQuantity = item.BulkQuantity;
                jst.IssuedQuantity = item.IssuedQuantity;
                jst.FinalQuantity = item.FinalQuantity;
                jst.Value = item.Value;
                jst.TotalValue = item.TotalValue;
                jst.CompanyID = item.CompanyID;
                jst.DepartmentID = item.DepartmentID;
                jst.PeriodYear = item.PeriodYear;
                jst.Year = item.Year;
                jst.Period = item.Period;
                lstST.Add(jst);
            }
            return lstST;
        }
        public List<obj_INVENTORY> getDepartmentStock(string dpid, int? year, int? period)
        {
            var tk = db.tb_Inventory.Where(x => x.DepartmentID == dpid && x.Year == year && x.Period == period).ToList();
            List<obj_INVENTORY> lstTK = new List<obj_INVENTORY>();
            obj_INVENTORY jtk;
            foreach (var item in tk)
            {
                jtk = new obj_INVENTORY();
                jtk.BARCODE = item.BARCODE;
                var hh = db.tb_Product.FirstOrDefault(h => h.BARCODE == item.BARCODE);
                // Add null check for hh
                if (hh != null)
                {
                    jtk.ProductName = hh.ProductName;
                    jtk.Unit = hh.Unit;
                }
                else
                {
                    jtk.ProductName = "N/A"; // Or string.Empty or "Unknown Product"
                    jtk.Unit = "N/A";        // Or string.Empty
                }
                jtk.BeginningQuantity = item.BeginningQuantity;
                jtk.ReceivedQuantity = item.ReceivedQuantity;
                jtk.RecInnerQuantity = item.RecInnerQuantity;
                jtk.OutInnerQuantity = item.OutInnerQuantity;
                jtk.BulkQuantity = item.BulkQuantity;
                jtk.IssuedQuantity = item.IssuedQuantity;
                jtk.FinalQuantity = item.FinalQuantity;
                jtk.Value = item.Value;
                jtk.TotalValue = item.TotalValue;
                jtk.CompanyID = item.CompanyID;
                jtk.DepartmentID = item.DepartmentID;
                jtk.PeriodYear = item.PeriodYear;
                jtk.Year = item.Year;
                jtk.Period = item.Period;
                lstTK.Add(jtk);
            }
            return lstTK;
        }
        public tb_Inventory getStockNumber(string barcode, string dpid, int? year, int? period)
        {
            return db.tb_Inventory.FirstOrDefault(x=> x.BARCODE == barcode && x.DepartmentID ==dpid && x.Year == year && x.Period == period);
        }
    }
}
