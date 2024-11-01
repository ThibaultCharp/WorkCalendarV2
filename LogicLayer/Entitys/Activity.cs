using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Entitys
{
    public class Activity
    {
        public int id { get; set; }
        public string position { get; set; }
        public TimeOnly begintime { get; set; }
        public TimeOnly endtime { get; set; }
        public DateTime date { get; set; }
        public Employee employee { get; set; }
    }
}
