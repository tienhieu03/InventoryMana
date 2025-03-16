using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace BusinessLayer.Utils
{
    public class SYS_SEQ
    {
        Entities db;
        public SYS_SEQ()
        {
            db = Entities.CreateEntities();
        }
        public tb_SYS_SEQ getItem(string seqName)
        {
            return db.tb_SYS_SEQ.FirstOrDefault(x => x.SEQNAME == seqName);
        }
        public void add(tb_SYS_SEQ seq)
        {
            try
            {
                db.tb_SYS_SEQ.Add(seq);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }
        public void udpate(tb_SYS_SEQ seq)
        {
            var sequence = db.tb_SYS_SEQ.FirstOrDefault(x => x.SEQNAME == seq.SEQNAME);
            sequence.SEQVALUE = sequence.SEQVALUE + 1;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during data processing: " + ex.Message);
            }
        }
    }
}
