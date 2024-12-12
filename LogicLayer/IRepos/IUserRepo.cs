using LogicLayer.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.IRepos
{
    public interface IUserRepo
    {
        void LinkUser(string LoggedInUserEmail, string TargetUserEmail);
        void ChangeUserRole(string email, int roleId);
        bool UserExists(int userId);
        void CreateUser(User user);
        List<User> GetAllEmployeesWithoutEmployer(string Input);
        List<User> GetAllUsersWithCorrespondingRoles(string Input);
    }
}
