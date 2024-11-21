using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;

namespace LogicLayer.IRepos
{
    public interface IEmployeeRepo
    {
        List<Employee> GetAllemployeesPerEmployer(string email);
    }
}
