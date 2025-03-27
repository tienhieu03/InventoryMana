using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.DataModels
{
    public class INVOICE
    {
        Entities db;
        public INVOICE()
        {
            db = Entities.CreateEntities();
        }

        public tb_Invoice getItem(Guid id)
        {
            return db.tb_Invoice.FirstOrDefault(x => x.InvoiceID == id);
        }

        public List<tb_Invoice> getList()
        {
            return db.tb_Invoice.ToList();
        }

        public List<tb_Invoice> getList(int lct, DateTime from, DateTime till, string dpid)
        {
            return db.tb_Invoice.Where(x => x.DepartmentID == dpid &&
                                           x.Day >= from &&
                                           x.Day < till &&
                                           x.InvoiceType == lct)
                              .OrderBy(x => x.Invoice)
                              .ToList();
        }

        public List<tb_Invoice> getReceiveInvoice(int lct, DateTime from, DateTime till, string dpid)
        {
            return db.tb_Invoice.Where(x => x.ReceivingDepartmentID == dpid &&
                                           x.Day >= from &&
                                           x.Day < till &&
                                           x.InvoiceType == lct &&
                                           x.Status == 2)
                              .OrderBy(x => x.Invoice)
                              .ToList();
        }

        public tb_Invoice add(tb_Invoice invoice)
        {
            try
            {
                db.tb_Invoice.Add(invoice);
                db.SaveChanges();
                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }

        public tb_Invoice update(tb_Invoice invoice)
        {
            var ct = db.tb_Invoice.FirstOrDefault(x => x.InvoiceID == invoice.InvoiceID);
            if (ct != null)
            {
                ct.Quantity = invoice.Quantity;
                ct.TotalPrice = invoice.TotalPrice;
                ct.Description = invoice.Description;
                ct.Status = invoice.Status;
                ct.UpdatedDate = invoice.UpdatedDate;
                ct.UpdatedBy = invoice.UpdatedBy;

                try
                {
                    db.SaveChanges();
                    return invoice;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return null;
        }

        public tb_Invoice delete(Guid id, int userid)
        {
            var ct = db.tb_Invoice.FirstOrDefault(x => x.InvoiceID == id);
            if (ct != null)
            {
                ct.DeletedDate = DateTime.Now;
                ct.DeletedBy = userid;

                try
                {
                    db.SaveChanges();
                    return ct;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return null;
        }

        public tb_Invoice restore(Guid id, int userid)
        {
            var ct = db.tb_Invoice.FirstOrDefault(x => x.InvoiceID == id);
            if (ct != null)
            {
                ct.DeletedDate = null;
                ct.DeletedBy = null;
                ct.RestoredDate = DateTime.Now;

                try
                {
                    db.SaveChanges();
                    return ct;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return null;
        }

        public tb_Invoice updateStatus(Guid id, int status, int userid)
        {
            var ct = db.tb_Invoice.FirstOrDefault(x => x.InvoiceID == id);
            if (ct != null)
            {
                ct.Status = status;
                ct.UpdatedDate = DateTime.Now;
                ct.UpdatedBy = userid;

                try
                {
                    db.SaveChanges();
                    return ct;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during data processing: " + ex.Message);
                }
            }
            return null;
        }

        public List<tb_Invoice> getBySupplier(int supplierID, DateTime from, DateTime till)
        {
            return db.tb_Invoice.Where(x => x.ReceivingDepartmentID == supplierID.ToString() &&
                                           x.Day >= from &&
                                           x.Day < till)
                              .OrderByDescending(x => x.Day)
                              .ToList();
        }
    }
}
