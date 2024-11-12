﻿using LogicLayer.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.Entitys;

namespace LogicLayer.Services
{
    public class PositionService
    {
        private readonly IPositionRepo repository;

        public PositionService(IPositionRepo positionRepo)
        {
            repository = positionRepo;
        }

        public List<Position> GetAllActivitiesPerEmployee()
        {
            return repository.GetAllPositions();
        }
    }
}
