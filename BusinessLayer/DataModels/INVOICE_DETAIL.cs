using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.DataModels
{
    public class INVOICE_DETAIL
    {
        Entities db;
        public INVOICE_DETAIL()
        {
            db = Entities.CreateEntities();
        }

        public tb_InvoiceDetail getItem(Guid invoiceid)
        {
            return db.tb_InvoiceDetail.FirstOrDefault(x => x.InvoiceID == invoiceid);
        }

        public tb_InvoiceDetail getItemById(Guid detailId)
        {
            return db.tb_InvoiceDetail.FirstOrDefault(x => x.InvoiceDetail_ID == detailId);
        }

        public List<tb_InvoiceDetail> getList(Guid id)
        {
            return db.tb_InvoiceDetail.Where(x => x.InvoiceID == id).ToList();
        }

        public List<tb_InvoiceDetail> getListByDetailId(Guid detailId)
        {
            return db.tb_InvoiceDetail.Where(x => x.InvoiceDetail_ID == detailId).ToList();
        }

        public List<obj_INVOICE_DETAIL> getListbyIDFull(Guid id)
        {
            var lst = db.tb_InvoiceDetail.Where(x => x.InvoiceID == id).ToList();
            List<obj_INVOICE_DETAIL> lstInvoice = new List<obj_INVOICE_DETAIL>();
            obj_INVOICE_DETAIL obj;
            foreach (var item in lst)
            {
                obj = new obj_INVOICE_DETAIL();
                obj.InvoiceDetail_ID = item.InvoiceDetail_ID;
                obj.InvoiceID = item.InvoiceID;
                obj.BARCODE = item.BARCODE;
                var h = db.tb_Product.FirstOrDefault(x => x.BARCODE == item.BARCODE);
                if (h != null)
                {
                    obj.ProductName = h.ProductName;
                    obj.Unit = h.Unit;
                }
                obj.QRCODE = item.QRCODE;
                obj.Quantity = item.Quantity;
                obj.Price = item.Price;
                obj.SubTotal = item.SubTotal;
                obj.Day = item.Day;
                obj.STT = item.STT;
                lstInvoice.Add(obj);
            }
            return lstInvoice;
        }

        public double getTotalPrice(Guid invoiceId)
        {
            var details = db.tb_InvoiceDetail.Where(x => x.InvoiceID == invoiceId);
            if (details != null && details.Any())
            {
                return details.Sum(x => x.SubTotal ?? 0);
            }
            return 0;
        }

        public int getTotalQuantity(Guid invoiceId)
        {
            var details = db.tb_InvoiceDetail.Where(x => x.InvoiceID == invoiceId);
            if (details != null && details.Any())
            {
                return details.Sum(x => x.Quantity ?? 0);
            }
            return 0;
        }

        public tb_InvoiceDetail add(tb_InvoiceDetail invoicedetail)
        {
            try
            {
                db.tb_InvoiceDetail.Add(invoicedetail);
                db.SaveChanges();
                return invoicedetail;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }

        public List<tb_InvoiceDetail> addRange(List<tb_InvoiceDetail> details)
        {
            try
            {
                db.tb_InvoiceDetail.AddRange(details);
                db.SaveChanges();
                return details;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }

        // Cập nhật chi tiết hóa đơn
        public tb_InvoiceDetail update(tb_InvoiceDetail invoicedetail)
        {
            tb_InvoiceDetail _detail = db.tb_InvoiceDetail.FirstOrDefault(x => x.InvoiceDetail_ID == invoicedetail.InvoiceDetail_ID);
            if (_detail != null)
            {
                _detail.BARCODE = invoicedetail.BARCODE;
                _detail.QRCODE = invoicedetail.QRCODE;
                _detail.Quantity = invoicedetail.Quantity;
                _detail.Price = invoicedetail.Price;
                _detail.SubTotal = invoicedetail.SubTotal;
                _detail.Day = invoicedetail.Day;
                _detail.STT = invoicedetail.STT;
                _detail.ProductID = invoicedetail.ProductID;
                _detail.InvoiceQuantity = invoicedetail.InvoiceQuantity;
                _detail.DiscountAmount = invoicedetail.DiscountAmount;

                try
                {
                    db.SaveChanges();
                    return _detail;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return null;
        }

        // Xóa chi tiết hóa đơn
        public bool delete(Guid invoiceDetailID)
        {
            tb_InvoiceDetail detail = db.tb_InvoiceDetail.FirstOrDefault(x => x.InvoiceDetail_ID == invoiceDetailID);
            if (detail != null)
            {
                db.tb_InvoiceDetail.Remove(detail);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return false;
        }

        // Xóa tất cả chi tiết của một hóa đơn
        public bool deleteAllByInvoiceId(Guid invoiceId)
        {
            var details = db.tb_InvoiceDetail.Where(x => x.InvoiceID == invoiceId).ToList();
            if (details != null && details.Count > 0)
            {
                db.tb_InvoiceDetail.RemoveRange(details);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return false;
        }

        // Cập nhật số thứ tự cho tất cả chi tiết hóa đơn
        public void updateSTT(Guid invoiceId)
        {
            var details = db.tb_InvoiceDetail.Where(x => x.InvoiceID == invoiceId).OrderBy(x => x.STT).ToList();
            for (int i = 0; i < details.Count; i++)
            {
                details[i].STT = i + 1;
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }

        // Kiểm tra sản phẩm đã tồn tại trong hóa đơn chưa
        public bool isProductExist(Guid invoiceId, string barcode)
        {
            return db.tb_InvoiceDetail.Any(x => x.InvoiceID == invoiceId && x.BARCODE == barcode);
        }

        // Lấy danh sách chi tiết hóa đơn dưới dạng view
        public List<V_INVOICE_DETAIL> getInvoiceDetailView(Guid invoiceId)
        {
            return db.V_INVOICE_DETAIL.Where(x => x.InvoiceID == invoiceId).ToList();
        }
    }
}
