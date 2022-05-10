using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Contextes
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(60);

            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .HasOne(o => o.Group)
                .WithMany(m => m.Students)
                .HasForeignKey(f => f.GroupId);
        }
    }
}
