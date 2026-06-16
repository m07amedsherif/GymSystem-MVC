using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositries.Classes
{
    public class MemberReopsitry : GenericRepositry<Member>, IMemberRepositry
    {
        private readonly GymDbContext dbContext;
        public MemberReopsitry(GymDbContext _dbContext)
            : base(_dbContext)
        {
            this.dbContext = _dbContext;
        }
    }
}
