using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Contextes
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<HomeTask> HomeTasks { get; set; } = null!;
        public DbSet<TaskAnswer> TaskAnswers { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;

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


            modelBuilder.Entity<HomeTask>()
                .HasOne(o => o.Topic)
                .WithOne(o => o.HomeTask)
                .HasForeignKey<Topic>(fk => fk.HomeTaskId);
        }
        //TODO: CRUD for Topic, CRUD for HomeTask, CRUD for TaskAnswer
    }
}
