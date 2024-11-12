﻿using LogicLayer.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.IRepos
{
    public interface IUserRepo
    {
        bool UserExists(string userId);
        void CreateUser(User user);
        List<User> GetAllUsers();
    }
}
