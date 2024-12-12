using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entities;
using LogicLayer.Entitys;

namespace LogicLayer.IRepos
{
    public interface IPositionRepo
    {
        List<Position> GetAllPositions();
        List<Role> GetAllRoles();
    }
}
