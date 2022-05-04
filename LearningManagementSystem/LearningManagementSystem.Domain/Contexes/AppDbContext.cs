using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Contexes
{
    internal class AppDbContext: DbContext
    {

        public DbSet<User> Users { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

    }
}
