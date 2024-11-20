using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Entitys
{
    public class Employee
    {
        public int id { get; set; }
        public User user { get; set; }
        public Employer employer { get; set; }
    }
}
