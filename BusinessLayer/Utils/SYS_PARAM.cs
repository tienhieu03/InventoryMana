using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    [Serializable]
    public class SYS_PARAM
    {
        string _companyId;
        public string companyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }
        string _departmentId;
        public string departmentId
        {
            get { return _departmentId; }
            set { _departmentId = value; }
        }
        public SYS_PARAM(string companyId, string departmentId)
        {
            this._companyId = companyId;
            this._departmentId = departmentId;
        }
        public void SaveFile()
        {
            if (File.Exists("sysparam.ini"))
                File.Delete("sysparam.ini");
            FileStream fs = File.Open("sysparam.ini", FileMode.OpenOrCreate, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            fs.Close();
        }
    }
}
