using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataModels
{
    public class _STATUS
    {
        public int _value { get; set; }
        public string _display { get; set; }

        public _STATUS(int _val, string _dis)
        {
            this._value = _val;
            this._display = _dis;
        }
        public static List<_STATUS> getList()
        {
            List<_STATUS> _lst = new List<_STATUS>();
            _STATUS[] collect = new _STATUS[2]
            {
                new _STATUS(1, "Not complete"),
                new _STATUS(2, "Confirmed"),
            };
            _lst.AddRange(collect);
            return _lst;
        }
    }
}
