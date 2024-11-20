using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;

namespace LogicLayer.IRepos
{
    public interface IAuthRepo
    {
        bool UserExists(string email);
        void AddUser(User user);
        User GetUserByEmail(string email);
    }
}
